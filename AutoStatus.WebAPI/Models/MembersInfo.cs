using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoStatus.WebAPI.Models
{
    public class MembersInfo
    {
        public string DisplayName { get; set; }
        public string MailAddress { get; set; }
        public string Domain { get; set; }
        public string PrincipalName { get; set; }
        public string Origin { get; set; }
        public bool IsStatusFilled { get; set; }
    }
}