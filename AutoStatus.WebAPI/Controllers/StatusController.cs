using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoStatus.WebAPI.Interfaces;
using AutoStatus.WebAPI.Models;

namespace AutoStatus.WebAPI.Controllers
{
    // [Route("api/[controller]")]
    public class StatusController : ApiController
    {
        private readonly IStatusSender statusSender;
        public StatusController()
        {

        }
        public StatusController(IStatusSender _statusSender)
        {
            statusSender = _statusSender;
        }

        [HttpGet]
        [Route("api/status/get")]
        public APIResponse Get()
        {
            return statusSender.GetStatus().Result;
        }

        [HttpPost]
        [Route("api/status/sendMail")]
        public bool SendMail([FromBody] string statusHtml)
        {
            return statusSender.SendMail(statusHtml);
        }

        [HttpPost]
        [Route("api/status/notifyUser")]
        public bool NotifyUser([FromBody] string userMailAddress)
        {
            return statusSender.NotifyUser(userMailAddress);
        }


    }
}
