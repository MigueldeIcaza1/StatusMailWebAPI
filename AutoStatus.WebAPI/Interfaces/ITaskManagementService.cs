using AutoStatus.WebAPI.Models;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoStatus.WebAPI.Interfaces
{ 
    public interface ITaskManagementService
    {
        Task<List<StatusRecord>> GetData(Uri collectionUri, string projectName, List<string> queryFolderHierarchy);
        List<MembersInfo> GetTeamMembers();
        Task<List<QueryHierarchyItem>> GetAllQueries(Uri collectionUri, string projectName);
    }
}
