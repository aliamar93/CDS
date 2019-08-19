using DAL.DBEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class AllPermissionResponseModel : BaseRespone
    {
        public tblRole tblRole { get; set; }
        public List<tblPermission> tblPermission { get; set; }
        public List<tblModule> tblModule { get; set; }
        public List<tblPage> tblPage { get; set; }
        public List<tblRolePermissionJunc> tblRolePermissionJunc { get; set; }

    }

    public class updatePermissionRequestModel : BaseRequest
    {
        [Required(ErrorMessage = "PermissionList is required")]
        public string permissionlist { get; set; }
        [Required(ErrorMessage = "RoleID is required")]
        public int roleID { get; set; }
    }



    //SysPermissions

    public class sysPermissionReponseModel : BaseRespone
    {
        public SysPermissionViewModel sysPermissions { get; set; }
    }


    public class SysPermissionViewModel
    {
        public List<tblActionEndPoint> tblActionEndPoints;
        public List<tblModule> tblModules { get; set; }
        public List<tblPage> tblPages { get; set; }
        public List<tblPermission> tblPermissions { get; set; }
        public List<tblPermissionActionJunc> tblPermissionActionsJunc { get; set; }
        public List<tblRolePermissionJunc> tblRolePermissionsJunc { get; set; }
    }



}