using AutoStatus.WebAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AutoStatus.WebAPI.Services
{
    public class ConfigurationReader: IConfigurationReader
    {
        public Dictionary<string, string> GetConfigurations()
        {
            var dictionary = new Dictionary<string, string>();

            foreach (string key in ConfigurationManager.AppSettings)
            {
                dictionary.Add(key, ConfigurationManager.AppSettings[key]);
            }
            return dictionary;
        }
    }
}