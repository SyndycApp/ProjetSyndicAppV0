using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;
using SyndicApp.Mobile.Api.Communication;
using SyndicApp.Mobile.Models;
using SyndicApp.Mobile.Services.Communication;
using System.Collections.ObjectModel;

namespace SyndicApp.Mobile.ViewModels.Communication;

[QueryProperty(nameof(ConversationIdString), "conversationId")]
[QueryProperty(nameof(NomDestinataire), "name")]
public partial class ChatViewModel : ObservableObject
{
    private readonly IMessagesApi _api;
    private readonly ChatHubService _hub;
    private readonly AudioRecorderService _recorder;
    private readonly AudioPlayerService _player;

    private MessageDto? _currentAudioMessage;

    [ObservableProperty]
    private bool isRecording;

    [ObservableProperty]
    private ObservableCollection<MessageDto> messages = new();

    [ObservableProperty]
    private string newMessage = string.Empty;

    [ObservableProperty]
    private string nomDestinataire = string.Empty;

    [ObservableProperty]
    private string conversationIdString = string.Empty;


    public Guid ConversationId => Guid.Parse(ConversationIdString);

    public ChatViewModel(
        IMessagesApi api,
        ChatHubService hub,
        AudioRecorderService recorder,
        AudioPlayerService player)
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
