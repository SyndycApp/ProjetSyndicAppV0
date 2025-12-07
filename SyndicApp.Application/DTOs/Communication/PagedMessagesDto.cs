using System;
using System.Collections.Generic;

namespace SyndicApp.Application.DTOs.Communication
{
    public class PagedMessagesDto
    {
        public int Total { get; set; }         
        public int Page { get; set; }          
        public int PageSize { get; set; }       

        public List<MessageDto> Messages { get; set; } = new();
    }
}
