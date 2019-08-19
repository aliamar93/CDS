using BAL.Repositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using DAL.DBEntities;
using Newtonsoft.Json;
using DAL.Models.Notifications;

namespace COD.Controllers
{
    public class RoleController : BaseController
    {
        // GET: Role
        public async Task<ActionResult> Index()
        {
            string url = utilityRepository.getBaseUrl() + "role/getAsyncRoles";
            string urlParameter = @"accesstoken=" + HttpUtility.UrlEncode(currentUser.userAccessToken) + "&&roleid";
            var model = await utilityRepository.getAsyncResponse<RoleListResponseVM>(url, urlParameter, false);

            return View(model.Roles);
        }


        public async Task<ActionResult> Detail(int id)
        {
            var model = await getRoleById(id);
            if (model.Code == (int)ResponseCode.Success)
            {
                var role = model.Roles.FirstOrDefault();
                return View("Save", role);
            }

            return Json(new { success = false, msg = model.Message }, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> Edit(int id)
        {

            var model = await getRoleById(id);
            if (model.Code == (int)ResponseCode.Success)
            {
                var role = model.Roles.FirstOrDefault();
                return View("Save", role);
            }

            return Json(new { success = false, msg = model.Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save()
        {
            return View(new tblRole());
        }

        [HttpPost]
        public async Task<ActionResult> Save(tblRole role)
        {
            bool isSuccess = false;
            string MessageKey = TempKey.FailedMsg.ToString();
            ModelState.Remove("RoleID");
            var err = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => x).ToList();

            if (ModelState.IsValid)
            {
                CreateRoleRequest req = new CreateRoleRequest();
                req.accesstoken = currentUser.userAccessToken;
                req.Role = role;
                string url = utilityRepository.getBaseUrl() + "user/createroleasync";
                var model = await utilityRepository.getAsyncResponse<BaseRespone>(url, JsonConvert.SerializeObject(req), false);
                isSuccess = model.Code == (int)ResponseCode.Success ? true : false;
                MessageKey = isSuccess ? TempKey.SuccessMsg.ToString() : MessageKey;
                TempData[MessageKey] = model.Detail;
                return Json(new { success = isSuccess }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = isSuccess }, JsonRequestBehavior.AllowGet);
        }

        //Permission
        #region Permission
        // GET: Permission

        public ActionResult RolePermission(int? id)
        {
            //getAllPermissions Api Call
            string url = utilityRepository.getBaseUrl() + "role/getAllPermissions";
            string urlParameter = @"accesstoken=" + HttpUtility.UrlEncode(currentUser.userAccessToken) + "&&roleid=" + id.ToString();
            var model = utilityRepository.getResponse<AllPermissionResponseModel>(url, urlParameter, false);
            ViewBag.roleid = id;
            return View(model);
        }

        [HttpPost]
        public JsonResult SavePermission(updatePermissionRequestModel vm)
        {
            bool isSave = false;
            string message = string.Empty;

            vm.accesstoken = currentUser.userAccessToken;
            ModelState.Clear();
            TryValidateModel(vm);
            try
            {
                if (ModelState.IsValid)
                {
                    string url = utilityRepository.getBaseUrl() + "role/updateRolePermission";
                    var model = utilityRepository.getResponse<AllPermissionResponseModel>(url, Newtonsoft.Json.JsonConvert.SerializeObject(vm), true);
                    if (model.Code == (int)ResponseCode.Success)
                    {
                        isSave = true;
                        CacheModel.SysPermissionsViewModelByAPI = null;
                        message = "Permissions updated successfully.";
                        TempData[TempKey.SuccessMsg.ToString()] = message;
                    }
                    else
                    {
                        isSave = false;
                        message = model.Detail;
                        TempData[TempKey.FailedMsg.ToString()] = message;
                    }
                }
            }
            catch (Exception ex)
            {
                isSave = false;
                message = ex.Message;
                TempData[TempKey.FailedMsg.ToString()] = message;
            }

            return Json(new { IsSave = isSave, Message = message }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region privateMethod
        private async Task<RoleListResponseVM> getRoleById(int id)
        {
            string url = utilityRepository.getBaseUrl() + "role/getAsyncRoles";
            string urlParameter = @"accesstoken=" + HttpUtility.UrlEncode(currentUser.userAccessToken) + "&&roleid=" + id.ToString();
            return await utilityRepository.getAsyncResponse<RoleListResponseVM>(url, urlParameter, false);

        }
        #endregion

    }
}