using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace AutoStatus.WebAPI.Helpers
{
    public static class Helper
    {
        public static string GetSubject()
        {
            var subject = ConfigurationManager.AppSettings.Get("subject");
            var todayDate = DateTime.Now.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("es-ES"));
            if (subject.Contains("#todayDate#"))
            {
                subject = subject.Replace("#todayDate#", todayDate);
            }
            return subject;
        }
    }
}