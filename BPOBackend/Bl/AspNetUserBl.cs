using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPOBackend
{
    public class AspNetUserBl
    {
        private static AspNetUserBl instance = null;
        public static AspNetUserBl Instance
        {
            get
            {
                return instance ?? new AspNetUserBl();
            }
        }
        public List<AspNetUser> GetAspNetUser()
        {
            return AspNetUserDao.Instance.GetAspNetUser();
        }
    }
}
