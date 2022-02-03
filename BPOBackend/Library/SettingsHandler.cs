using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPOBackend
{
    public class SettingsHandler
    {
        private static SettingsHandler instance = null;
        public static SettingsHandler Instance
        {
            get
            {
                return instance ?? new SettingsHandler();
            }
        }
        public String GetFromAppSetting(string value)
        {
            IConfiguration configuration = SetConfig();
            return configuration[value];
        }
        private IConfiguration SetConfig()
        {
            return new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", false, true)
                    .Build();
        }
    }
}
