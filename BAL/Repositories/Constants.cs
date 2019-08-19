using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Repositories
{
    public static class Constants
    {
        public static bool enableEventLog
        {
            get
            {
                string enable = "0";
                if (ConfigurationManager.AppSettings["EnableEventLog"] != null)
                {
                    enable = ConfigurationManager.AppSettings["EnableEventLog"].ToString();
                }
                return enable == "1" ? true : false;
            }
        }

        public static string ConfigFileBasePath
        {
            get
            {
                return "~/ConfigFiles/";
            }
        }

        public static string NotificationsFileBasePath
        {
            get { return "~/ConfigFiles/Notifications/"; }
        }
    }
}
