using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Notifications
{
    public abstract class BaseNotification
    {
        public string onsuccess { get; set; }
        public string onupdate { get; set; }
        public string ondeleted { get; set; }
        public string ondenied { get; set; }
        public string onfailed { get; set; }
        public string found { get; set; }
        public string othermsg { get; set; }
    }
}
