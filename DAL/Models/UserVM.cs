using DAL.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class UserListResponseVM : BaseRespone
    {
        public int TotalUsersCount { get; set; }
        public List<tblUser> Users {get;set;}
    }
    
    public class CreateUserRequest : BaseRequest
    {
        public tblUser user { get; set; }
    }

    public class CheckUsernameExistResponse : BaseRespone
    {
        public bool IsUsernameAvailable { get; set; }
    }
}
