using Refit;
using SyndicApp.Mobile.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.Api
{
    public interface IUsersApi
    {
        [Get("/api/Users")]
        Task<List<UserDto>> GetAllAsync();

        [Get("/api/Users/search")]
        Task<List<UserDto>> SearchAsync([Query] string name);
    }
}
