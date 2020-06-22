using AutoStatus.WebAPI.Interfaces;
using AutoStatus.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace AutoStatus.WebAPI.Services
{
    public class ConfigurationReader : IConfigurationReader
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

        public bool SaveConfigurations(List<ConfigurationMap> list)
        {
            try
            {
                var config = WebConfigurationManager.OpenWebConfiguration("~");
                ConfigurationManager.RefreshSection("appSettings");

                foreach (var item in list)
                {
                    if (config.AppSettings.Settings.AllKeys.Any(t => t == item.Key))
                    {
                        config.AppSettings.Settings[item.Key].Value = item.Value;
                    }
                }

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }
    }
}