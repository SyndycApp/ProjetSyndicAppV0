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
    private const string BaseApiUrl = "http://192.168.1.200:5041";

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
    private string newMessage;

    [ObservableProperty]
    private string nomDestinataire;

    [ObservableProperty]
    private string conversationIdString;

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

        _hub.OnMessageReceived += msg =>
        {
            if (msg.ConversationId == ConversationId)
                Messages.Add(msg);
        };

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

    // ▶️ PLAY AUDIO (PAR MESSAGE)
    [RelayCommand]
    private async Task PlayAudioAsync(MessageDto message)
    {
        if (message.AudioUrl == null)
            return;

        var url = message.AbsoluteAudioUrl;

        // Stop ancien audio
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

    // 🎤 RECORD
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
        Messages.Add(message);
    }

    [RelayCommand]
    public async Task LoadMessagesAsync()
    {
        var list = await _api.GetMessagesAsync(ConversationId);
        await _api.MarkConversationAsReadAsync(ConversationId);

        Messages.Clear();
        foreach (var msg in list.OrderBy(m => m.CreatedAt))
            Messages.Add(msg);
    }

    [RelayCommand]
    public async Task SendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(NewMessage)) return;

        var sent = await _api.SendMessageAsync(new SendMessageRequest
        {
            ConversationId = ConversationId,
            Contenu = NewMessage
        });

        Messages.Add(sent);
        NewMessage = string.Empty;
    }
}
