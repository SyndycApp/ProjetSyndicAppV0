using System;
using System.Collections.Generic;

namespace SyndicApp.Application.DTOs.Auth
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;

        public string? Token { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
