namespace SyndicApp.Mobile.Models
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new();
        public T? Data { get; set; }
    }
}
