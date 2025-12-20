using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyndicApp.Mobile.Services.AppelVocal;

public partial class ActiveCallViewModel : ObservableObject, IQueryAttributable
{
    private readonly CallHubService _callHub;
    private Timer? _timer;
    private DateTime _start;

    [ObservableProperty] string callDuration = "00:00";
    [ObservableProperty] string initials = "";
    [ObservableProperty] string otherUserName = "";

    public Guid CallId { get; set; }

    public ActiveCallViewModel(CallHubService callHub)
    {
        _callHub = callHub;

        _callHub.CallAccepted += id =>
        {
            if (id == CallId)
                StartTimer();
        };

        _callHub.CallEnded += id =>
        {
            if (id == CallId)
                Shell.Current.GoToAsync("..");
        };
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        CallId = (Guid)query["CallId"];
        OtherUserName = query["OtherUserName"]?.ToString() ?? "";

        Initials = string.Join("", OtherUserName.Split(' ')
            .Where(x => x.Length > 0)
            .Select(x => x[0]));
    }

    private void StartTimer()
    {
        _start = DateTime.UtcNow;
        _timer = new Timer(_ =>
        {
            CallDuration = (DateTime.UtcNow - _start)
                .ToString(@"mm\:ss");
        }, null, 0, 1000);
    }

    [RelayCommand]
    private async Task EndCall()
    {
        await _callHub.EndCall(CallId);
        _timer?.Dispose();
        await Shell.Current.GoToAsync("..");
    }
}
