using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COD.Models
{
    //User
    public class UserGridViewModal
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }

    //Product
    public class ProductGridViewModal
    {
        public long ProductID { get; set; }
        public string Name { get; set; }
        public string ProductCode { get; set; }
        public string ProductType { get; set; }
        public string SKU { get; set; }
    }

}