using Microsoft.Extensions.Configuration;
using System;

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
        public string GetFromAppSetting(string value)
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
