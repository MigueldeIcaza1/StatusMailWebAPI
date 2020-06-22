using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoStatus.WebAPI.Enums;
using AutoStatus.WebAPI.Interfaces;
using AutoStatus.WebAPI.Models;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace AutoStatus.WebAPI.Controllers
{
    // [Route("api/[controller]")]
    public class StatusController : ApiController
    {
        private readonly IStatusSender statusSender;
        private readonly IConfigurationReader configurationReader;
        public StatusController()
        {

        }
        public StatusController(IStatusSender _statusSender, IConfigurationReader _configurationReader)
        {
            statusSender = _statusSender;
            configurationReader = _configurationReader;
        }

        [HttpGet]
        [Route("api/status/get")]
        public APIResponse Get(string statusType, string folderHierarchy = null)
        {
            return statusSender.GetStatus(statusType, folderHierarchy).Result;
        }

        [HttpPost]
        [Route("api/status/sendMail")]
        public bool SendMail([FromBody] string statusHtml)
        {
            return statusSender.SendMail(statusHtml);
        }

        [HttpPost]
        [Route("api/status/notifyUser")]
        public bool Notify([FromBody] List<MembersInfo> members)
        {
            return statusSender.Notify(members);
        }

        [HttpGet]
        [Route("api/status/getallqueries")]
        public List<QueryHierarchyItem> GetAllQueries()
        {
            return statusSender.GetAllQueries();
        }

        [HttpGet]
        [Route("api/status/getconfigurations")]
        public Dictionary<string,string> GetConfigurations()
        {
            return configurationReader.GetConfigurations();
        }

        [HttpPost]
        [Route("api/status/saveConfigutaions")]
        public bool SaveConfigurations(List<ConfigurationMap> configList)
        {
            return configurationReader.SaveConfigurations(configList);
        }

    }
}
