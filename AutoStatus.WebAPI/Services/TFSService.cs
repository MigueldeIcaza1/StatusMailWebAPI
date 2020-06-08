using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoStatus.WebAPI.Interfaces;
using AutoStatus.WebAPI.Models;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.WebApi;

namespace AutoStatus.WebAPI.Services
{
    public class TFSService : ITaskManagementService
    {
        private WorkItemTrackingHttpClient witClient;

        private TswaClientHyperlinkService hyperLinkService;

        public async Task<List<StatusRecord>> GetData(Uri collectionUri, string projectName, List<string> queryFolderHierarchy)
        {

            QueryHierarchyItem query = await GetQueryFromTFS(collectionUri, projectName, queryFolderHierarchy);
            Console.WriteLine("Retrieved the Query");

            WorkItemQueryResult queryResult = this.witClient.QueryByIdAsync(query.Id).Result;
            Console.WriteLine("Executed the Query");

            List<StatusRecord> statusRecords = await MapQueryResultToStatusRecords(queryResult);

            PrintStatusRecords(statusRecords);

            return statusRecords;
        }
       
        private async Task<QueryHierarchyItem> GetQueryFromTFS(Uri collectionUri, string projectName, List<string> queryFolderHierarchy)
         {
            witClient = await ConnectToTFS(collectionUri);

            Console.WriteLine("Connected to TFS");
            
            List<QueryHierarchyItem> queryHierarchyItems = this.witClient.GetQueriesAsync(projectName, depth: 2).Result;

            // QueryHierarchyItem queryHierarchyItem = queryHierarchyItems.FirstOrDefault(qhi => qhi.Name.Equals("Shared Queries"));

            QueryHierarchyItem queryHierarchyItem = queryHierarchyItems.FirstOrDefault();

            for (int index = 0; index < queryFolderHierarchy.Count; index++)
            {
                if (index == 0)
                {
                    queryHierarchyItem = queryHierarchyItems.FirstOrDefault(qhi => qhi.Name.Equals(queryFolderHierarchy[index]));
                }
                else
                {
                    queryHierarchyItem = queryHierarchyItem.Children.FirstOrDefault(qhi => qhi.Name.Equals(queryFolderHierarchy[index]));
                }
            }
            return queryHierarchyItem;
        }

        private async Task<WorkItemTrackingHttpClient> ConnectToTFS(Uri collectionUri) // Supressed the no await warning
        {
            VssConnection connection = new VssConnection(collectionUri, new VssClientCredentials());

            using (TfsTeamProjectCollection collection = new TfsTeamProjectCollection(collectionUri, new VssClientCredentials()))
            {
                collection.EnsureAuthenticated();
                this.hyperLinkService =
                   collection.GetService<TswaClientHyperlinkService>();
            }

            return connection.GetClient<WorkItemTrackingHttpClient>();
        }

        // TODO: Make this query configurable
        private async Task<List<StatusRecord>> MapQueryResultToStatusRecords(WorkItemQueryResult queryResult) 
        {
            var StatusRecordlist = new List<StatusRecord>();

            var count = 1;
            foreach (WorkItemLink item in queryResult.WorkItemRelations)
            {
                if (item.Source != null)
                {
                    var workItemInstance = this.witClient.GetWorkItemAsync(item.Target.Id).Result;
                    var parentItemInstance = this.witClient.GetWorkItemAsync(item.Source.Id).Result;

                    Enum.TryParse(workItemInstance.Fields["System.State"].ToString().Replace(" ", string.Empty), out CurrentStatus workItemStatus);

                    var record = new StatusRecord()
                    {
                        SerialNumber = count++,
                        ParentIdWithLink = new IdWithLink()
                        {
                            Id = item.Source.Id,
                            Link = this.hyperLinkService.GetWorkItemEditorUrl(item.Source.Id).ToString()
                        },
                        TaskIdWithLink = new IdWithLink()
                        {
                            Id = item.Target.Id,
                            Link = this.hyperLinkService.GetWorkItemEditorUrl(item.Target.Id).ToString()
                        },
                        TaskTitle = workItemInstance.Fields["System.Title"].ToString(),
                        TaskStatus = workItemStatus,
                        AssignedTo = ((IdentityRef)workItemInstance.Fields["System.AssignedTo"]).DisplayName,
                        ParentTitle = parentItemInstance?.Fields["System.Title"]?.ToString(),
                        CompletedWork = workItemInstance.Fields.Keys.Contains("Microsoft.VSTS.Scheduling.CompletedWork") ? workItemInstance?.Fields["Microsoft.VSTS.Scheduling.CompletedWork"]?.ToString() : string.Empty
                    };

                    StatusRecordlist.Add(record);
                }
            }
            return StatusRecordlist;

        }

        public List<MembersInfo> GetTeamMembers()
        {
            var list = new List<MembersInfo>();
            var item1 = new MembersInfo() { DisplayName = "ravi.kannegundla", MailAddress = "ravi.kannegundla@ggktech.com", Origin = "aad" };
            var item2 = new MembersInfo() { DisplayName = "Chaitanya.Satkuri", MailAddress = "Chaitanya.Satkuri@ggktech.com", Origin = "aad" };
            var item3 = new MembersInfo() { DisplayName = "Sravya.Konapalli", MailAddress = "Sravya.Konapalli@ggktech.com", Origin = "aad" };
            var item4 = new MembersInfo() { DisplayName = "Junez Riyaz Shaik", MailAddress = "riyaz.shaik@ggktech.com", Origin = "aad" };
            var item5 = new MembersInfo() { DisplayName = "rajesh.ganapuram", MailAddress = "rajesh.ganapuram@ggktech.com", Origin = "aad" };
            var item6 = new MembersInfo() { DisplayName = "srikanth.rokkam", MailAddress = "srikanth.rokkam@ggktech.com", Origin = "aad" };
            var item7 = new MembersInfo() { DisplayName = "mahalakshmi.gunti", MailAddress = "mahalakshmi.gunti@ggktech.com", Origin = "aad" };
            var item8 = new MembersInfo() { DisplayName = "madhuri.nandamuri", MailAddress = "madhuri.nandamuri@ggktech.com", Origin = "aad" };
            var item9 = new MembersInfo() { DisplayName = "geetha.burugupalli", MailAddress = "geetha.burugupalli@ggktech.com", Origin = "aad" };
            var item10 = new MembersInfo() { DisplayName = "siddarth.kalidindi", MailAddress = "siddarth.kalidindi@ggktech.com", Origin = "aad" };
            var item11 = new MembersInfo() { DisplayName = "brahmateja.pulipati", MailAddress = "brahmateja.pulipati@ggktech.com", Origin = "aad" };
            var item12 = new MembersInfo() { DisplayName = "Sudhir Bandi", MailAddress = "Sudhir.Bandi@ggktech.com", Origin = "aad" };

            var item13 = new MembersInfo() { DisplayName = "ankit.singh", MailAddress = "ankit.singh@ggktech.com", Origin = "aad" };
            var item14 = new MembersInfo() { DisplayName = "Ross Walsmith", MailAddress = "rwalsmith@kantola.com", Origin = "aad" };
            var item15 = new MembersInfo() { DisplayName = "ross walsmith.net", MailAddress = "ross@walsmith.net", Origin = "aad" };

            list.Add(item1);
            list.Add(item2);
            list.Add(item3);
            list.Add(item4);
            list.Add(item5);
            list.Add(item6);
            list.Add(item7);
            list.Add(item8);
            list.Add(item9);
            list.Add(item10);
            list.Add(item11);
            list.Add(item12);
            return list.Where(t => t.MailAddress.Contains("@ggktech")).ToList();
        }

        //public async void GetMembers()
        //{
        //    var url = "https://vssps.dev.azure.com/KantolaTraining/_apis/graph/users?api-version=5.1-preview.1";
        //    try
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            //The HttpResponseMessage which contains status code, and data from response.
        //            using (HttpResponseMessage res = await client.GetAsync(url))
        //            {
        //                using (HttpContent content = res.Content)
        //                {
        //                    var data = await content.ReadAsStringAsync();
        //                    //If the data isn't null return log convert the data using newtonsoft JObject Parse class method on the data.
        //                    if (data != null)
        //                    {
        //                        Console.WriteLine("data------------{0}", data);
        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine("NO Data----------");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine("Exception :: "+ exception);
        //    }
        //}

        private void PrintStatusRecords(List<StatusRecord> statusListToPrint)
        {
            foreach (StatusRecord item in statusListToPrint)
            {
                Console.WriteLine($"{item.SerialNumber}  | " +
                    $"{item.ParentTitle}  | " +
                    $"{item.TaskIdWithLink.Id} | " +
                    $"{item.TaskTitle} | " +
                    $"{item.TaskStatus} | " +
                    $"{item.AssignedTo}");
            }
        }
    }
}
