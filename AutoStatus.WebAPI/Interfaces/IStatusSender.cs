using AutoStatus.WebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoStatus.WebAPI.Interfaces
{
    public interface IStatusSender
    {
        // Task<APIResponse> GetStatus();
        Task<APIResponse> GetStatus(string statusType = null);
        bool SendMail(string statusHtml);
        void SendMail(List<StatusRecord> statusList);
        bool Notify(List<MembersInfo> members);
    }
}
