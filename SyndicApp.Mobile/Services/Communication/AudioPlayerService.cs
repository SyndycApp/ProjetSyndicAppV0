using Android.Media;
using Microsoft.Maui.ApplicationModel;

namespace SyndicApp.Mobile.Services.Communication;

public class AudioPlayerService
{
    private MediaPlayer? _player;
    private CancellationTokenSource? _cts;

    public bool IsPlaying => _player?.IsPlaying == true;
    public long Duration => _player?.Duration ?? 0;

    public event Action<long, long>? OnProgress;
    public event Action? OnCompleted;



    public void SeekTo(double progress)
    {
        if (_player == null || !_player.IsPlaying || Duration <= 0)
            return;

        var position = (int)(Duration * progress);
        _player.SeekTo(position);
    }

    // =====================
    // ▶️ PLAY
    // =====================
    public async Task PlayAsync(string url)
    {
        StopInternal(); 

        _player = new MediaPlayer();
        _player.SetAudioAttributes(
            new AudioAttributes.Builder()
                .SetUsage(AudioUsageKind.Media)
                .SetContentType(AudioContentType.Music)
                .Build());

        _player.SetDataSource(url);

        _player.Prepared += (_, _) =>
        {
            _player.Start();
            StartProgressLoop();
        };

        _player.Completion += (_, _) =>
        {
            StopInternal();
            OnCompleted?.Invoke();
        };

        _player.PrepareAsync();
        await Task.CompletedTask;
    }

    // =====================
    // ⏸ PAUSE
    // =====================
    public void Pause()
    {
        if (_player?.IsPlaying == true)
        {
            _player.Pause();
        }
    }

    // =====================
    // ⏹ STOP + CLEAN
    // =====================
    private void StopInternal()
    {
        try
        {
            _cts?.Cancel();
            _cts = null;

            if (_player != null)
            {
                if (_player.IsPlaying)
                    _player.Stop();

                _player.Reset();
                _player.Release();
                _player.Dispose();
                _player = null;
            }
        }
        catch
        {
            // sécurité Android
        }
    }

    // =====================
    // ⏱ PROGRESS LOOP
    // =====================
    private void StartProgressLoop()
    {
        _cts = new CancellationTokenSource();
        var token = _cts.Token;

        Task.Run(async () =>
        {
            while (!token.IsCancellationRequested && _player != null)
            {
                try
                {
                    var current = _player.CurrentPosition;
                    var duration = _player.Duration;

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        OnProgress?.Invoke(current, duration);
                    });

                    await Task.Delay(300, token);
                }
                catch
                {
                    break;
                }
            }
        }, token);
    }
}
