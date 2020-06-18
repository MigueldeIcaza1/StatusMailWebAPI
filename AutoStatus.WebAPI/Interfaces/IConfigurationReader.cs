using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoStatus.WebAPI.Interfaces
{
    public interface IConfigurationReader
    {
        Dictionary<string, string> GetConfigurations();
    }
}
