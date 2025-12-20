using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using SyndicApp.Mobile.Api;

public partial class ActiveCallViewModel : ObservableObject, IQueryAttributable
{
    private HubConnection? _connection;
    private Timer? _timer;
    private DateTime _start;
    private readonly ICallApi _callApi;

    [ObservableProperty] string callDuration = "00:00";
    [ObservableProperty] string initials = "";
    [ObservableProperty] string otherUserName = "";

    public Guid CallId { get; set; }

    public ActiveCallViewModel(ICallApi callApi)
    {
        _callApi = callApi;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        CallId = (Guid)query["CallId"];
        OtherUserName = query["OtherUserName"]?.ToString() ?? "";

        Initials = OtherUserName.Length > 0
            ? string.Join("", OtherUserName.Split(' ').Select(x => x[0]))
            : "?";

        StartTimer();
    }

    private void StartTimer()
    {
        _start = DateTime.UtcNow;
        _timer = new Timer(_ =>
        {
            var elapsed = DateTime.UtcNow - _start;
            CallDuration = elapsed.ToString(@"mm\:ss");
        }, null, 0, 1000);
    }

    [RelayCommand]
    private async Task EndCall()
    {
        try
        {
            // ✅ TOUJOURS appeler le backend
            await _callApi.EndCallAsync(CallId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur EndCall API : {ex}");
        }
        finally
        {
            _timer?.Dispose();
            await Shell.Current.GoToAsync("..");
        }
    }
}
