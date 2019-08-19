using BAL.Repositories;
using COD.Models;
using DAL.DBEntities;
using DAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COD.Controllers
{
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index(string id)
        {
            //Decode URL Parameter
            int? userid = string.IsNullOrEmpty(id) ? (int?)null : Convert.ToInt32(id);
            TempData["userid"] = userid;
            return View(new List<DAL.DBEntities.tblUser>());
        }

        public ActionResult Detail(int id)
        {
            initializeDropdownsData();
            var model = commonServices.getUserById(id, currentUser.userAccessToken);
            if (model.Code == (int)ResponseCode.Success && model.Users.Count > 0)
            {
                return View("_Save", modelBinder(model.Users.FirstOrDefault()));
            }
            return Json(new { success = false, msg = model.Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            initializeDropdownsData();
            var model = commonServices.getUserById(id, currentUser.userAccessToken);
            if (model.Code == (int)ResponseCode.Success && model.Users.Count > 0)
            {
                return View("_Save", modelBinder(model.Users.FirstOrDefault()));
            }

            return Json(new { success = false, msg = model.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index()
        {
            string userid = TempData["userid"] == null ? string.Empty : TempData["userid"].ToString();
            string search = Request.Form.GetValues("search[value]")[0];
            string draw = Request.Form.GetValues("draw")[0];
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
            int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
            string dir = orderDir == "asc" ? "ascending" : "descending";
            string sortColumn = getSortedColumnName(order) + " " + dir;

            string url = utilityRepository.getBaseUrl() + "user/getusers";
            string urlParameter = @"accesstoken=" + HttpUtility.UrlEncode(currentUser.userAccessToken) + "&&userid=" + userid;
            urlParameter += "&&startfrom=" + startRec.ToString() + "&&pagesize=" + pageSize.ToString() + "&&search=" + search + "&&orderby=" + sortColumn;
            var model = utilityRepository.getResponse<UserListResponseVM>(url, urlParameter, false);
            if (model.Code == (int)ResponseCode.Success)
            {
                var grdViewList = model.Users.Select(x => new UserGridViewModal
                {
                    UserID = x.UserID,
                    FullName = x.FirstName + " " + x.LastName,
                    Role = string.Join(",", x.tblUserRoles.Select(r => r.tblRole.RoleName).ToList()),
                    UserName = x.UserLoginID,
                    Email = x.Email

                }).ToList();

                return Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = grdViewList.Count,
                    recordsFiltered = model.TotalUsersCount,
                    data = grdViewList
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { draw = 1, recordsTotal = "", recordsFiltered = "", error =  new { code = -32601, message = model.Detail } }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Save()
        {
            initializeDropdownsData();
            return View("_Save", new DAL.DBEntities.tblUser());
        }

        [HttpPost]
        public ActionResult Save(DAL.DBEntities.tblUser user)
        {
            bool isSuccess = false;
            string MessageKey = TempKey.FailedMsg.ToString();
            ModelState.Remove("UserID");
            var err = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => x).ToList();

            if (ModelState.IsValid)
            {
                CreateUserRequest req = new CreateUserRequest();
                req.accesstoken = currentUser.userAccessToken;
                req.user = user;
                string url = utilityRepository.getBaseUrl() + "user/createuser";
                var model = utilityRepository.getResponse<BaseRespone>(url, JsonConvert.SerializeObject(req), true);
                isSuccess = model.Code == (int)ResponseCode.Success ? true : false;
                MessageKey = isSuccess ? TempKey.SuccessMsg.ToString() : MessageKey;
                TempData[MessageKey] = model.Detail;
                return Json(new { success = isSuccess }, JsonRequestBehavior.AllowGet);
            }
            initializeDropdownsData();
            return View("_Save", modelBinder(user));
        }


        public JsonResult isUsernameAailable(string UserLoginID)
        {
            bool isAvailable = false;
            if(!string.IsNullOrEmpty(UserLoginID))
            {
                string url = utilityRepository.getBaseUrl() + "user/isUsernameAailable";
                string urlParameter = @"accesstoken=" + HttpUtility.UrlEncode(currentUser.userAccessToken) + "&&UserName=" + UserLoginID; ;
                var model = utilityRepository.getResponse<CheckUsernameExistResponse>(url, urlParameter, false);
                isAvailable = model.IsUsernameAvailable;
            }
            return Json(isAvailable, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getLandingPageList(int[] roleids)
        {
            if (roleids != null && roleids.Count() > 0)
            {
                //roleids.Select(x => Convert.ToInt32(x)).ToArray())
                var pages = new userRepository().getPageList(roleids).Select(x => new { x.PageID, x.PageName }).ToList();
                return Json(pages, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = false }, JsonRequestBehavior.AllowGet);
        }
        #region privateMethods
        private RoleListResponseVM getRoleList()
        {
            //getUsers Api Call
            string url = utilityRepository.getBaseUrl() + "role/getAsyncRoles";
            string urlParameter = @"accesstoken=" + HttpUtility.UrlEncode(currentUser.userAccessToken) + "&&roleid";
            var model = utilityRepository.getResponse<RoleListResponseVM>(url, urlParameter, false);
            return model;
        }

        private string getSortedColumnName(string index)
        {
            int ind = Convert.ToInt32(index);
            string fieldName = string.Empty;
            switch (ind)
            {
                case 0:
                    fieldName = "UserID";
                    break;
                case 1:
                    fieldName = "FirstName";
                    break;
                case 2:
                    fieldName = "UserID";
                    break;
                case 3:
                    fieldName = "UserLoginID";
                    break;
                case 4:
                    fieldName = "Email";
                    break;
                default:
                    fieldName = "UserID";
                    break;
            }

            return fieldName;
        }
        #endregion


        #region temporarycalldb
        //Temporary using DAL.DBEntities
        private List<tblPage> bindLandingPages(int[] roleids)
        {
            var pages = new userRepository().getPageList(roleids);
            ViewBag.LandingPageList = pages.Select(x => new { x.PageID, x.PageName }).ToList(); ;
            return pages;
        }

        private void initializeDropdownsData()
        {
            ViewBag.RoleList = getRoleList().Roles.Select(x => new { x.RoleID, x.RoleName }).ToList();
            if (ViewBag.LandingPageList == null)
                ViewBag.LandingPageList = new List<object>();
        }
        private tblUser modelBinder(tblUser model)
        {
            var pages = bindLandingPages(model.tblUserRoles.Select(x => x.RoleID).ToArray());
            model.LandingPageId = pages.Where(x => x.PageUrl == model.LandingPage).ToList().Select(x => int.Parse(x.PageID.ToString())).FirstOrDefault();            
            model.ConfirmPassword = model.Password;
            return model;
        }
        #endregion

    }
}