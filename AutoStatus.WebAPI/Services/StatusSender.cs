using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoStatus.WebAPI.Enums;
using AutoStatus.WebAPI.Interfaces;
using AutoStatus.WebAPI.Models;
using EmailSender;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Newtonsoft.Json;

namespace AutoStatus
{
    public class StatusSender : IStatusSender
    {
        private readonly ITaskManagementService tmService;
        private readonly IEmailSender emailSender;

        public StatusSender(ITaskManagementService _tmService, IEmailSender _emailSender)
        {
            tmService = _tmService;
            emailSender = _emailSender;
        }

        public async Task<APIResponse> GetStatus(string statusType = null, string folderHierarchy = null)
        {
            if (string.IsNullOrEmpty(statusType))
            {
                statusType = StatusType.Daily.ToString();
            }

            Uri collectionUri = new Uri(ConfigurationManager.AppSettings.Get("collectionUri"));
            string projectName = ConfigurationManager.AppSettings.Get("projectName");

            if (statusType.ToLower() == StatusType.Daily.ToString().ToLower())
            {
                folderHierarchy = ConfigurationManager.AppSettings.Get("DailyStatusQueryFolderHierarchy");
            }
            else if (statusType.ToLower() == StatusType.Monthly.ToString().ToLower())
            {
                folderHierarchy = ConfigurationManager.AppSettings.Get("MonthlyStatusQueryFolderHierarchy");
            }

            var folders = ExtractFolderNames(folderHierarchy, '/');
            var statusHtml = string.Empty;
            var result = new APIResponse();
            try
            {
                var statusList = await tmService.GetData(collectionUri, projectName, folders);
                var membersList = tmService.GetTeamMembers();

                if (statusType.ToLower() == StatusType.Daily.ToString().ToLower())
                {
                    foreach (var statusItem in statusList)
                    {
                        foreach (var member in membersList)
                        {
                            if (member.DisplayName == statusItem.AssignedTo)
                            {
                                member.IsStatusFilled = true;
                            }
                        }
                    }
                }

                if (statusType.ToLower() == StatusType.Monthly.ToString().ToLower())
                {
                    statusHtml = MonthlyEmailSender.GetEmailBody(statusList);
                }
                else
                {
                    statusHtml = emailSender.GetEmailBody(statusList);
                }

                var subject = ConfigurationManager.AppSettings.Get("subject");
                var toEmail = ConfigurationManager.AppSettings.Get("toMail");
                var ccMail = ConfigurationManager.AppSettings.Get("ccMail");

                var mailInfo = new MailInfo();

                mailInfo.Subject = subject;
                mailInfo.ToMailAdress = toEmail;
                mailInfo.CcMailAdress = ccMail;

                result.MailInfo = mailInfo;
                result.MembersList = membersList;
                result.StatusHtml = statusHtml;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception :" + ex.Message);
            }

            return result;
        }

        public void SendMail(List<StatusRecord> statusList)
        {
            // emailSender.SendEmail(statusList);
        }

        public bool SendMail(string statusHtml)
        {
            return emailSender.SendStatusEmail(statusHtml);
        }

        private List<string> ExtractFolderNames(string folderHierarchy, char seperator)
        {
            return folderHierarchy.Split(seperator).ToList();
        }

        public bool Notify(List<MembersInfo> members)
        {
            string subject = ConfigurationManager.AppSettings.Get("NotifyUserSubject");
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;
            var notifyUserHTMLPath = rootPath + "/Assets/NotifyUser.html";
            var htmlString = File.ReadAllText(notifyUserHTMLPath);
            List<string> toEmails = new List<string>();
            foreach (var member in members)
            {
                if (!member.IsStatusFilled)
                {
                    toEmails.Add(member.MailAddress);
                }
            }
            return emailSender.SendUserNotificationEmail(htmlString, toEmails, subject);
        }

        public List<QueryHierarchyItem> GetAllQueries()
        {
            var collectionUri = new Uri(ConfigurationManager.AppSettings.Get("collectionUri"));
            string projectName = ConfigurationManager.AppSettings.Get("projectName");
            List<QueryHierarchyItem> queriesList = new List<QueryHierarchyItem>();
            try
            {
                queriesList = tmService.GetAllQueries(collectionUri, projectName).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception :" + ex.Message);
            }
            return queriesList;
        }
    }
}
