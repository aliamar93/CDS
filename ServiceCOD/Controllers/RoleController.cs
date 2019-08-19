using BAL.Repositories;
using DAL.Models;
using DAL.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ServiceCOD.Controllers
{
    public class RoleController : BaseApiController
    {
        roleRepository roleRepo;
        permissionRepository permissionRepo;
        public RoleController()
        {
            roleRepo = new roleRepository(new DAL.DBEntities.DBEntities());
            permissionRepo = new permissionRepository(new DAL.DBEntities.DBEntities());
        }

        public IHttpActionResult getRoles(string accesstoken, int? roleid)
        {
            RoleListResponseVM vm = new RoleListResponseVM();
            try
            {
                var notifications = utilityRepository.getNotificationJSON<RoleNotification>(NotificationConstants.RoleNotificationFile);
                vm.Roles = roleRepo.getRoleList(null, roleid);

                vm.Response(ResponseCode.Success, notifications.found.ReplaceNoticationToken(vm.Roles.Count.ToString()));
            }
            catch (Exception ex)
            {
                vm.Response(ResponseCode.Error, ex.Message);
            }
            return Ok(vm);

        }

        public async Task<IHttpActionResult> getAsyncRoles(string accesstoken, int? roleid)
        {

            RoleListResponseVM vm = new RoleListResponseVM();
            try
            {
                var notifications = utilityRepository.getNotificationJSON<RoleNotification>(NotificationConstants.RoleNotificationFile);
                vm.Roles = await roleRepo.getAsyncRoleList(null, roleid);

                vm.Response(ResponseCode.Success, notifications.found.ReplaceNoticationToken(vm.Roles.Count.ToString()));
            }
            catch (Exception ex)
            {
                vm.Response(ResponseCode.Error, ex.Message);
            }
            return Ok(vm);

        }

        public async Task<IHttpActionResult> createRoleAsync(CreateRoleRequest model)
        {
            BaseRespone response = new BaseRespone();
            if (ModelState.IsValid)
            {
                var notifications = utilityRepository.getNotificationJSON<RoleNotification>(NotificationConstants.RoleNotificationFile);
                roleRepo.createRole(model.Role, currentToken.userId);
                await roleRepo.SaveChangesAsync();
                response.Response(ResponseCode.Success, notifications.onsuccess.ReplaceNoticationToken(model.Role.RoleName));
            }
            else
            {
                string errormodel = string.Empty;
                ModelState.Values.Where(x => x.Errors.Count > 0).ToList().ForEach(x => { errormodel += string.Join(",", x.Errors.Select(e => e.ErrorMessage).ToList()); });
                response.Response(ResponseCode.InvalidModel, errormodel);
            }
            return Ok(response);
        }



        //Role Permission APIs


        public IHttpActionResult getAllPermissions(string accesstoken, int roleid)
        {
            var token = accessControlRepository.validateUserAccessToken(accesstoken);
            if (token.isValid)
            {
                var vm = permissionRepo.getAllPermissionList(roleid);
                string Detail = vm.tblPermission.Count.ToString() + " record found.";
                vm.Response(ResponseCode.Success, Detail);
                return Ok(vm);
            }
            else
            {
                BaseRespone vm = new BaseRespone();
                vm.Response(ResponseCode.AccessDenied, "Invalid Access Token");
                return Content(HttpStatusCode.Forbidden, vm);
            }
        }

        [HttpPost]
        public IHttpActionResult updateRolePermission(updatePermissionRequestModel vm)
        {
            BaseRespone resp = new BaseRespone();
            var code = HttpStatusCode.InternalServerError;
            if (ModelState.IsValid)
            {
                using (var dbTrans = permissionRepo.DBContext.Database.BeginTransaction())
                {
                    try
                    {
                        DataTable lstPermission = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(vm.permissionlist);

                        if (permissionRepo.removeAllPermissionByRoleId(vm.roleID))
                        {
                            foreach (DataRow Permission in lstPermission.Rows)
                            {
                                if (Convert.ToBoolean(Permission["Allow"]))
                                {
                                    DAL.DBEntities.tblRolePermissionJunc RP = new DAL.DBEntities.tblRolePermissionJunc();
                                    RP.PermissionID = Convert.ToInt32(Permission["PermissionID"]);
                                    RP.RoleID = vm.roleID;
                                    permissionRepo.createPermission(RP);
                                }
                            }

                            permissionRepo.SaveChanges();
                        }
                        dbTrans.Commit();
                        string msg = "Permissions updated successfully.";
                        resp.Response(ResponseCode.Success, msg);
                        code = HttpStatusCode.OK;
                        CacheModel.SysPermissionsViewModelByDB = null;
                    }
                    catch (Exception ex)
                    {
                        dbTrans.Rollback();
                        resp.Response(ResponseCode.Error, ex.Message);
                        code = HttpStatusCode.InternalServerError;
                    }
                }
            }
            else
            {
                string errormodel = string.Empty;
                ModelState.Values.Where(x => x.Errors.Count > 0).ToList().ForEach(x => { errormodel += string.Join(",", x.Errors.Select(e => e.ErrorMessage).ToList()); });
                resp.Response(ResponseCode.Failed, errormodel);
                code = HttpStatusCode.OK;
            }
            return Content(code, resp);
        }

    }
}
