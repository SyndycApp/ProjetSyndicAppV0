using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyndicApp.Mobile.Models
{
    public class AuthListResponse
    {
        public bool Success { get; set; }
        public List<string>? Errors { get; set; }
        public List<AuthUserDto>? Data { get; set; }
    }
}
