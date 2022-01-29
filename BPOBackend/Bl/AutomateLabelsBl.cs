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
    }
}
