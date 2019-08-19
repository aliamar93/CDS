using BAL.Repositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COD.Controllers
{
    public class PermissionController : BaseController
    {
        public PermissionController()
        {

        }
        // GET: Permission

        public ActionResult Index(int? id)
        {
            //getAllPermissions Api Call
            string url = utilityRepository.getBaseUrl() + "permission/getAllPermissions";
            string urlParameter = @"accesstoken=" + HttpUtility.UrlEncode(currentUser.userAccessToken) + "&&roleid=" + id.ToString();
            var model = utilityRepository.getResponse<AllPermissionResponseModel>(url, urlParameter, false);
            ViewBag.roleid = id;
            return View(model);
        }


        [HttpPost]
        public JsonResult Save(updatePermissionRequestModel vm)
        {
            bool isSave = false;
            vm.accesstoken = currentUser.userAccessToken;
            try
            {
                if (ModelState.IsValid)
                {
                    string url = utilityRepository.getBaseUrl() + "permission/updateRolePermission";
                    var model = utilityRepository.getResponse<AllPermissionResponseModel>(url, Newtonsoft.Json.JsonConvert.SerializeObject(vm), true);
                    if (model.Code == (int)ResponseCode.Success)
                    {
                        isSave = true;
                        TempData[TempKey.SuccessMsg.ToString()] = "Permissions updated successfully.";
                    }
                    else
                    {
                        isSave = false;
                        TempData[TempKey.FailedMsg.ToString()] = model.Detail;
                    }
                }
            }
            catch (Exception ex)
            {
                isSave = false;
                TempData[TempKey.FailedMsg.ToString()] = ex.Message;
            }

            return Json(new { IsSave = isSave }, JsonRequestBehavior.AllowGet);
        }
    }
}