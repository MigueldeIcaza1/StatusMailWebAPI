using AutoStatus.WebAPI.Models;
using System.Collections.Generic;

namespace AutoStatus.WebAPI.Interfaces
{

    public interface IEmailSender
    {
        string GetEmailBody(List<StatusRecord> workItems);
        // void SendEmail(List<StatusRecord> statusList);
        bool SendStatusEmail(string statusList);
        bool SendUserNotificationEmail(string htmlString, List<string> toEmail, string subject, string ccMailAddress = null);
    }
}
