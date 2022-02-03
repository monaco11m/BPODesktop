using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BPOBackend
{
    public class AppUserAutoBatchingBl
    {
        private static AppUserAutoBatchingBl instance = null;
        public static AppUserAutoBatchingBl Instance
        {
            get
            {
                return instance ?? new AppUserAutoBatchingBl();
            }
        }
        public List<AppUserAutoBatching> GetAutoBatchingUsers()
        {
            try
            {
                string webResult = WebRequestHandler.ApiGet(SettingsHandler.Instance.GetFromAppSetting("MainDomain") + "getAutoBatchingUsers");
                return JsonConvert.DeserializeObject<List<AppUserAutoBatching>>(webResult);
            }
            catch(Exception ex)
            {
                return new List<AppUserAutoBatching>();
            }
        }
    }
}
