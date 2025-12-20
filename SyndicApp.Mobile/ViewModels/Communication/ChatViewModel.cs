using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
using SyndicApp.Mobile.Api;
using SyndicApp.Mobile.Api.Communication;
using SyndicApp.Mobile.Models;
using SyndicApp.Mobile.Services.Communication;
using SyndicApp.Mobile.Views.AppelVocal;
using System.Collections.ObjectModel;

namespace SyndicApp.Mobile.ViewModels.Communication;

[QueryProperty(nameof(ConversationIdString), "conversationId")]
[QueryProperty(nameof(NomDestinataire), "name")]
[QueryProperty(nameof(OtherUserIdString), "otherUserId")]
public partial class ChatViewModel : ObservableObject
{
    private readonly IMessagesApi _api;
    private readonly ChatHubService _hub;
    private readonly AudioRecorderService _recorder;
    private readonly AudioPlayerService _player;
    private readonly ICallApi _callsApi;
    private MessageDto? _currentAudioMessage;

    [ObservableProperty]
    private bool isRecording;

    [ObservableProperty]
    private ObservableCollection<MessageDto> messages = new();

    [ObservableProperty]
    private string otherUserIdString = string.Empty;


    [ObservableProperty]
    private string newMessage = string.Empty;

    [ObservableProperty]
    private string nomDestinataire = string.Empty;

    [ObservableProperty]
    private string conversationIdString = string.Empty;

    [ObservableProperty]
    private bool isUserTyping;

    [ObservableProperty]
    private string typingText = string.Empty;

    [ObservableProperty]
    private Guid otherUserId;

    private CancellationTokenSource? _typingCts;

    private DateTime _lastTypingSent = DateTime.MinValue;


    public Guid ConversationId => Guid.Parse(ConversationIdString);

    public ChatViewModel(
        IMessagesApi api,
        ChatHubService hub,
        AudioRecorderService recorder,
        AudioPlayerService player,
        ICallApi callsApi)
    {
        _api = api;
        _hub = hub;
        _recorder = recorder;
        _player = player;

        // 🎧 AUDIO PLAYER — INCHANGÉ
        _player.OnProgress += (current, duration) =>
        {
            if (_currentAudioMessage == null || duration <= 0)
                return;

            _currentAudioMessage.AudioProgress = (double)current / duration;
            _currentAudioMessage.AudioTime =
                $"{TimeSpan.FromMilliseconds(current):mm\\:ss} / " +
                $"{TimeSpan.FromMilliseconds(duration):mm\\:ss}";
        };

        _player.OnCompleted += () =>
        {
            if (_currentAudioMessage == null) return;

            _currentAudioMessage.IsPlaying = false;
            _currentAudioMessage.AudioProgress = 0;
            _currentAudioMessage.AudioTime = "00:00";
        };
        _callsApi = callsApi;
    }


    partial void OnOtherUserIdStringChanged(string value)
    {
        if (Guid.TryParse(value, out var id))
            OtherUserId = id;
    }

    // =====================================================
    // 🔔 SIGNALR — ABONNEMENT APRES RECEPTION DU CONVERSATION ID
    // =====================================================
    partial void OnConversationIdStringChanged(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return;

        _hub.OnMessageReceived -= OnMessageReceived;
        _hub.OnMessageReceived += OnMessageReceived;

        _hub.OnMessageReacted -= OnMessageReacted;
        _hub.OnMessageReacted += OnMessageReacted;

        _hub.OnUserTyping -= OnUserTyping;
        _hub.OnUserTyping += OnUserTyping;
    }

    private void OnUserTyping(Guid userId)
    {
        // Ignore soi-même
        if (userId == Guid.Parse(App.UserId!))
            return;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            TypingText = $"✍️ {NomDestinataire} est en train d’écrire…";
            IsUserTyping = true;

            // Reset timer
            _typingCts?.Cancel();
            _typingCts = new CancellationTokenSource();

            _ = HideTypingAfterDelayAsync(_typingCts.Token);
        });
    }

    private async Task HideTypingAfterDelayAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(2500, token); // 2.5s = UX parfait
            IsUserTyping = false;
            TypingText = string.Empty;
        }
        catch (TaskCanceledException)
        {
            // normal
        }
    }


    private void OnMessageReacted(Guid messageId, string emoji, Guid userId)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            var msg = Messages.FirstOrDefault(m => m.Id == messageId);
            if (msg == null)
                return;

            var existing = msg.Reactions
                .FirstOrDefault(r => r.UserId == userId);

            if (existing != null)
            {
                if (existing.Emoji == emoji)
                    return;

                msg.Reactions.Remove(existing);
            }

            msg.Reactions.Add(new MessageReactionDto
            {
                UserId = userId,
                Emoji = emoji
            });
        });
    }

    private void OnMessageReceived(MessageDto msg)
    {
        if (msg.ConversationId != ConversationId)
            return;

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Messages.Add(msg);
        });
    }

    // ========================React msg=============================
    [RelayCommand]
    private async Task ReactAsync(ReactionCommandParam param)
    {
        if (param?.Message == null || string.IsNullOrWhiteSpace(param.Emoji))
            return;

        var currentUserId = Guid.Parse(App.UserId!);

        // 🔁 SUPPRIMER ANCIENNE REACTION DU USER
        var existing = param.Message.Reactions
            .FirstOrDefault(r => r.UserId == currentUserId);

        if (existing != null)
        {
            // même emoji → rien à faire
            if (existing.Emoji == param.Emoji)
                return;

            param.Message.Reactions.Remove(existing);
        }

        // ⚡ AJOUT NOUVELLE REACTION
        param.Message.Reactions.Add(new MessageReactionDto
        {
            UserId = currentUserId,
            Emoji = param.Emoji
        });

        // 📡 SignalR (safe)
        await _hub.SendReaction(
            ConversationId,
            param.Message.Id,
            param.Emoji
        );
    }


    public async Task OnTypingAsync()
    {
        if (DateTime.UtcNow - _lastTypingSent < TimeSpan.FromSeconds(1))
            return;

        _lastTypingSent = DateTime.UtcNow;
        await _hub.SendTyping(ConversationId);
    }



    // =====================================================
    // ▶️ PLAY AUDIO (INCHANGÉ)
    // =====================================================
    [RelayCommand]
    private async Task PlayAudioAsync(MessageDto message)
    {
        if (message.AudioUrl == null)
            return;

        var url = message.AbsoluteAudioUrl;

        if (_currentAudioMessage != null && _currentAudioMessage != message)
        {
            _currentAudioMessage.IsPlaying = false;
            _currentAudioMessage.AudioProgress = 0;
            _currentAudioMessage.AudioTime = "00:00";
        }

        if (!message.IsPlaying)
        {
            message.IsPlaying = true;
            _currentAudioMessage = message;
            await _player.PlayAsync(url);
        }
        else
        {
            message.IsPlaying = false;
            _player.Pause();
        }
    }

    // =====================================================
    // 🎤 RECORD AUDIO (INCHANGÉ)
    // =====================================================
    [RelayCommand]
    private async Task ToggleRecordingAsync()
    {
        var status = await Permissions.RequestAsync<MicrophonePermission>();
        if (status != PermissionStatus.Granted)
            return;

        if (!IsRecording)
        {
            IsRecording = true;
            await _recorder.StartAsync();
        }
        else
        {
            IsRecording = false;
            await StopAndSendAudioAsync();
        }
    }

    [RelayCommand]
    private async Task StopAndSendAudioAsync()
    {
        var path = await _recorder.StopAsync();
        if (!File.Exists(path)) return;

        using var stream = File.OpenRead(path);
        var part = new StreamPart(stream, Path.GetFileName(path), "application/octet-stream");

        var message = await _api.SendAudioMessageAsync(ConversationId, part);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Messages.Add(message);
        });
    }

    // =====================================================
    // 📩 LOAD MESSAGES
    // =====================================================
    [RelayCommand]
    public async Task LoadMessagesAsync()
    {
        var list = await _api.GetMessagesAsync(ConversationId);
        await _api.MarkConversationAsReadAsync(ConversationId);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Messages.Clear();
            foreach (var msg in list.OrderBy(m => m.CreatedAt))
                Messages.Add(msg);
        });
    }

    // =====================================================
    // 📄 DOCUMENT
    // =====================================================
    [RelayCommand]
    private async Task SendDocumentAsync()
    {
        var file = await FilePicker.PickAsync();
        if (file == null) return;

        await using var stream = await file.OpenReadAsync();

        var part = new StreamPart(
            stream,
            file.FileName,
            file.ContentType ?? "application/octet-stream"
        );

        var message = await _api.SendDocumentAsync(ConversationId, part);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Messages.Add(message);
        });
    }

    // =====================================================
    // 📍 LOCATION
    // =====================================================
    [RelayCommand]
    private async Task SendLocationAsync()
    {
        var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
            return;

        var location = await Geolocation.GetLastKnownLocationAsync()
                       ?? await Geolocation.GetLocationAsync(
                           new GeolocationRequest(GeolocationAccuracy.Medium));

        if (location == null) return;

        var message = await _api.SendLocationAsync(new SendLocationDto
        {
            ConversationId = ConversationId,
            Latitude = location.Latitude,
            Longitude = location.Longitude
        });

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Messages.Add(message);
        });
    }

    // =====================================================
    // 📸 IMAGE
    // =====================================================
    [RelayCommand]
    private async Task SendImageAsync()
    {
        var photo = await MediaPicker.PickPhotoAsync();
        if (photo == null) return;

        await using var stream = await photo.OpenReadAsync();

        var part = new StreamPart(stream, photo.FileName, "image/jpeg");

        var message = await _api.SendImageAsync(ConversationId, part);

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Messages.Add(message);
        });
    }

    [RelayCommand]
    private void SeekAudio((MessageDto message, double progress) param)
    {
        var (message, progress) = param;

        if (_currentAudioMessage != message)
            return;

        progress = Math.Clamp(progress, 0, 1);

        _player.SeekTo(progress);

        message.AudioProgress = progress;
    }


    [RelayCommand]
    private async Task OpenMapAsync(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return;

        await Launcher.OpenAsync(url);
    }


    [RelayCommand]
    private async Task StartCall()
    {
        Console.WriteLine("📞 StartCallCommand EXECUTED");

        var call = await _callsApi.StartCallAsync(new StartCallRequest
        {
            ReceiverId = OtherUserId
        });

        await Shell.Current.GoToAsync("active-call", new Dictionary<string, object>
        {
            ["CallId"] = call.Id,
            ["OtherUserName"] = NomDestinataire
        });
    }



    [RelayCommand]
    private async Task OpenFileAsync(MessageDto message)
    {
        if (message == null || string.IsNullOrWhiteSpace(message.AbsoluteFileUrl))
            return;

        try
        {
            var fileName = string.IsNullOrWhiteSpace(message.FileName)
                ? Path.GetFileName(message.AbsoluteFileUrl)
                : message.FileName;

            var localPath = Path.Combine(FileSystem.CacheDirectory, fileName);

            // ⬇️ Télécharger le fichier
            using var http = new HttpClient();
            var bytes = await http.GetByteArrayAsync(message.AbsoluteFileUrl);
            await File.WriteAllBytesAsync(localPath, bytes);

            // 📂 Ouvrir le fichier avec l’app système
            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(localPath)
            });
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert(
                "Erreur",
                "Impossible d’ouvrir le fichier.",
                "OK");
        }
    }

    // =====================================================
    // 📝 TEXTE
    // =====================================================
    [RelayCommand]
    public async Task SendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(NewMessage)) return;

        var sent = await _api.SendMessageAsync(new SendMessageRequest
        {
            ConversationId = ConversationId,
            Contenu = NewMessage
        });

        MainThread.BeginInvokeOnMainThread(() =>
        {
            Messages.Add(sent);
            NewMessage = string.Empty;
        });
    }
}
