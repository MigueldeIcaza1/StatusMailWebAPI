using AutoStatus.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoStatus.WebAPI.Interfaces
{ 
    public interface ITaskManagementService
    {
        Task<List<StatusRecord>> GetData(Uri collectionUri, string projectName, List<string> queryFolderHierarchy);
        List<MembersInfo> GetTeamMembers();
    }
}
