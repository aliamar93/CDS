using DAL.DBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class BaseRespone
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }

        public virtual void Response(ResponseCode code)
        {
            this.Code = (int)code;
            this.Message = code.ToString().FromCamelCase();
        }


        public virtual void Response(ResponseCode code, string detailMessage)
        {
            this.Code = (int)code;
            this.Message = code.ToString().FromCamelCase();
            this.Detail = detailMessage;
        }
    }

    public class BaseRequest
    {
        [Required( ErrorMessage = "Access Token is required" )]
        public string accesstoken { get; set; }
    }

    public class AuthenticationResponseModel : BaseRespone
    {
        public tblUser user { get; set; }
        public List<tblModule> tblModule { get; set; }
        public string accessToken { get; set; }
    }


    public class UserSession
    {

        public tblUser User { get; set; }
        public tblPage thisPage { get; set; }
        public List<tblModule> moduleList { get; set; }
        public string userAccessToken { get; set; }
    }

    //public class PermissionsViewModel
    //{
    //    public tblRolePermissionJunc tblRolePermissionJunc { get; set; }
    //    public tblPermission tblPermission { get; set; }
    //    public List<tblPermissionActionJunc> tblPermissionActionJunc { get; set; }
    //    public tblPage tblPages { get; set; }
    //    public tblModule tblModules { get; set; }

    //    public List<tblModule> moduleList { get; set; }
    //}
  

    public class ExtraPermissions
    {
        public int PermissionGroupID { get; set; }
        public int UserExtraPermissionjuncID { get; set; }
        public string Name { get; set; }
        public int UserID { get; set; }
        public bool Allow { get; set; }
    }
}
