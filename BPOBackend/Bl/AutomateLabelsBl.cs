using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPOBackend
{
    public class AutomateLabelsBl
    {
        private static AutomateLabelsBl instance = null;
        public static AutomateLabelsBl Instance
        {
            get
            {
                return instance ?? new AutomateLabelsBl();
            }
        }
        public List<AutomateLabel> GetIdsByUserIdAndDate(String userId, DateTime startDate, DateTime endDate)
        {
            return AutomateLabelsDao.Instance.GetIdsByUserIdAndDate(userId, startDate, endDate);
        }
        public List<AutomateLabel> GetAutomateLabel(String userId, DateTime startDate, DateTime endDate)
        {
            try
            {
                string webResult = WebRequestHandler.ApiGet(SettingsHandler.Instance.GetFromAppSetting("MainDomain") + "getAutomateLabel?" +
                    string.Format("userId={0}&startDate={1}&endDate={2}",userId,startDate.ToString("yyyy-MM-dd"),endDate.ToString("yyyy-MM-dd")));
                return JsonConvert.DeserializeObject<List<AutomateLabel>>(webResult);
            }
            catch (Exception ex)
            {
                return new List<AutomateLabel>();
            }
        }
    }
}
