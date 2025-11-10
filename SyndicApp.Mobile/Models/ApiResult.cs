// SyndicApp.Mobile/Models/ApiResult.cs
namespace SyndicApp.Mobile.Models
{
    public sealed class ApiResult<T>
    {
        public bool Success { get; set; }
        public List<string>? Errors { get; set; }
        public T? Data { get; set; }
    }
}
