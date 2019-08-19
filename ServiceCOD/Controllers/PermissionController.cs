using BAL.Repositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ServiceCOD.Controllers
{
    public class PermissionController : BaseApiController
    {
        accessControlRepository accessControlRepo;
        public PermissionController()
        {
            accessControlRepo = new accessControlRepository(new DAL.DBEntities.DBEntities());
        }

        [IgnorePermission]
        public IHttpActionResult getSysPermissions()
        {

            sysPermissionReponseModel vm = new sysPermissionReponseModel();
            try
            {
                vm.sysPermissions = accessControlRepo.getSysPermissionsAndModule();
                vm.Code = (int)ResponseCode.Success;
                vm.Message = ResponseCode.Success.ToString();
                vm.Detail = "Sys Permissions";
            }
            catch (Exception ex)
            {
                vm.Response(ResponseCode.Error, ex.Message);
            }
            return Ok(vm);

        }


    }
}
