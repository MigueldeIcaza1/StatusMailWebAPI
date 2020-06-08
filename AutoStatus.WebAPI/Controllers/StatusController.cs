using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoStatus.WebAPI.Interfaces;
using AutoStatus.WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
            // return "sample data";
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
            //return false;
            return statusSender.NotifyUser(userMailAddress);
        }


    }
}
