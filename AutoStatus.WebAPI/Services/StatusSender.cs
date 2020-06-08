﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoStatus.WebAPI.Interfaces;
using AutoStatus.WebAPI.Models;
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

        public void SendMail(List<StatusRecord> statusList)
        {
           // emailSender.SendEmail(statusList);
        }

        public bool SendMail(string statusHtml)
        {
           return emailSender.SendStatusEmail(statusHtml);
        }

        public bool NotifyUser(string toEmail)
        {
            string subject = ConfigurationManager.AppSettings.Get("NotifyUserSubject"); 
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;
            var notifyUserHTMLPath = rootPath + "/Assets/NotifyUser.html";
            var htmlString = File.ReadAllText(notifyUserHTMLPath);

            return emailSender.SendUserNotificationEmail(htmlString, toEmail, subject);
        }

        private List<string> ExtractFolderNames(string folderHierarchy, char seperator)
        {
            return folderHierarchy.Split(seperator).ToList();
        }
    }
}
