using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class FCMNotificationViewModel
    {
        public string to { get; set; }
        public FcmMessage data { get; set; }
        public Notification notification { get; set; }
    }


    public class FcmMessage
    {
        public string ShortDesc { get; set; }
        public string IncidentNo { get; set; }
        public string Description { get; set; }
    }

    public class Notification
    {
        public string title { get; set; }
        public string text { get; set; }
        public string sound { get; set; }
    }


    //FCM Notification Response

    public class FcmResponseResult
    {
        public string message_id { get; set; }
    }

    public class FcmResponseViewModel
    {
        public long multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public int canonical_ids { get; set; }
        public List<FcmResponseResult> results { get; set; }
        public string code { get; set; }
        public string error { get; set; }
    }
}
