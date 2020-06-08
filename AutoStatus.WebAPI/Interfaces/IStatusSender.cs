using AutoStatus.WebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoStatus.WebAPI.Interfaces
{
    public interface IStatusSender
    {
        Task<APIResponse> GetStatus();
        bool SendMail(string statusHtml);
        void SendMail(List<StatusRecord> statusList);
        bool NotifyUser(string htmlString);
    }
}
