using AutoStatus.WebAPI.Interfaces;
using AutoStatus.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;


namespace EmailSender
{
    public class MonthlyEmailSender 
    {

        public static string GetEmailBody(List<StatusRecord> workItems)
        {
            //var rootPath = Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()));
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;
            var monthlyStatusHTMLPath = rootPath + "/Assets/MonthlyStatus.html";
            var monthlyStatusHtml = File.ReadAllText(monthlyStatusHTMLPath);

            var statusRowHtmlPath = rootPath + "/Assets/MonthlyStatusRow.html";
            var statusRowHtml = File.ReadAllText(statusRowHtmlPath);
            var allStatusRowsHtml = AppenedStatusRows(statusRowHtml, workItems);

            monthlyStatusHtml = monthlyStatusHtml.Replace("#StatusRows#", allStatusRowsHtml);
            return monthlyStatusHtml;
        }

        private static string AppenedStatusRows(string rowHTML, List<StatusRecord> workItems)
        {
            var statusRowsHTML = string.Empty;
            var groupedWorkItems = workItems.GroupBy(t => t.ParentTitle).ToList();

            foreach (var groupItem in groupedWorkItems)
            {
                var count = 0;
                foreach (var workItem in groupItem)
                {
                    count++;
                    string rowHtmlWithReplacedWorkItem;
                    if (count == 1)
                    {
                        var workItemHtml = $"<td rowspan = \"{groupItem.Count()}\"> {groupItem.Key} </td>";
                        rowHtmlWithReplacedWorkItem = rowHTML.Replace("#GroupedWorkItemHTML#", workItemHtml);
                    } 
                    else
                    {
                        rowHtmlWithReplacedWorkItem = rowHTML.Replace("#GroupedWorkItemHTML#", string.Empty);
                    }

                    statusRowsHTML = statusRowsHTML + ReplaceWithWorkItem(rowHtmlWithReplacedWorkItem, workItem);
                }
            }

            return statusRowsHTML;
        }

        // need to do dynamically
        private static string ReplaceWithWorkItem(string rowHtml, StatusRecord workItem)
        {
            rowHtml = rowHtml.Replace("#Title#", workItem.TaskTitle);
            rowHtml = rowHtml.Replace("#ID#", workItem.TaskIdWithLink.Id.ToString());
            rowHtml = rowHtml.Replace("#IDLink#", workItem.TaskIdWithLink.Link);
            rowHtml = rowHtml.Replace("#AssignedTo#", workItem.AssignedTo);
            rowHtml = rowHtml.Replace("#Status#", workItem.TaskStatus.ToString());
            rowHtml = rowHtml.Replace("#CompletedWork#", workItem.CompletedWork.ToString());
            return rowHtml;
        }


    }
}
