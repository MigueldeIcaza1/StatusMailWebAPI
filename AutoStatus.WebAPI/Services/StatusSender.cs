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

        public async Task<APIResponse> GetStatus(string statusType = null)
        {
            if (string.IsNullOrEmpty(statusType))
            {
                statusType = StatusType.Daily.ToString();
            }
            if (statusType.ToLower() == StatusType.Daily.ToString().ToLower())
            {
                return await GetStatus();
            }
            else if (statusType.ToLower() == StatusType.Monthly.ToString().ToLower())
            {
                return await GetMonthlyStatus();
            }
            return null;
        }
        public async Task<APIResponse> GetStatus()
        {
            Uri collectionUri = new Uri(ConfigurationManager.AppSettings.Get("collectionUri"));
            string projectName = ConfigurationManager.AppSettings.Get("projectName");
            string folderHierarchy = ConfigurationManager.AppSettings.Get("queryFolderHierarchy");
            var folders = ExtractFolderNames(folderHierarchy, ',');
            var statusHtml = string.Empty;
            var result = new APIResponse();
            try
            {
                var statusList = await tmService.GetData(collectionUri, projectName, folders);
                var membersList = tmService.GetTeamMembers();
                foreach (var i in statusList)
                {
                  foreach(var j in membersList)
                    {
                        if(j.DisplayName == i.AssignedTo)
                        {
                            j.IsStatusFilled = true;
                        }
                    }
                }
                statusHtml = emailSender.GetEmailBody(statusList);
                result.MembersList = membersList;
                result.StatusHtml = statusHtml;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception :" + ex.Message);
            }

            return result;
        }

        public async Task<APIResponse> GetMonthlyStatus()
        {
            Uri collectionUri = new Uri(ConfigurationManager.AppSettings.Get("collectionUri"));
            string projectName = ConfigurationManager.AppSettings.Get("projectName");
            string folderHierarchy = ConfigurationManager.AppSettings.Get("MonthlyQueryFolderHierarchy");
            var folders = ExtractFolderNames(folderHierarchy, ',');
            var statusHtml = string.Empty;
            var result = new APIResponse();
            try
            {
                var statusList = await tmService.GetData(collectionUri, projectName, folders);
                var membersList = tmService.GetTeamMembers();
                //statusHtml = emailSender.GetEmailBody(statusList);
                statusHtml = MonthlyEmailSender.GetEmailBody(statusList);
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
    }
}
