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
        public MailInfo MailInfo { get; set; }

    }

    public class MailInfo
    {
        public string Subject { get; set; }
        public string ToMailAdress { get; set; }
        public string CcMailAdress { get; set; }
    }
}