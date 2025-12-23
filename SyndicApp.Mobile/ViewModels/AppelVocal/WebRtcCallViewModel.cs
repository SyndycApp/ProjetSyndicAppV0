namespace SyndicApp.Mobile.ViewModels.AppelVocal;

public class WebRtcCallViewModel
{
    public Guid CallId { get; set; }
    public Guid OtherUserId { get; set; }
    public string Token { get; set; } = "";
    public string BaseUrl { get; set; } = "";
    public bool IsCaller { get; set; }

    public string GetStartScript()
    {
        return $@"
            start(
              '{CallId}',
              '{OtherUserId}',
              '{Token}',
              '{BaseUrl}',
              {(IsCaller ? "true" : "false")}
            );
        ";
    }
}
