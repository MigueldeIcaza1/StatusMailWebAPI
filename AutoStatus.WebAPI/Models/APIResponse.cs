using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoStatus.WebAPI.Models
{
    public class APIResponse
    {
        public string StatusHtml { get; set; }
        public List<MembersInfo> MembersList { get; set; }

    }
}