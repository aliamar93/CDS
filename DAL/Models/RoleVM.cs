using DAL.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class RoleListResponseVM : BaseRespone
    {
        public List<tblRole> Roles { get; set; }
    }

    public class CreateRoleRequest : BaseRequest
    {
        public tblRole Role { get; set; }
    }
}
