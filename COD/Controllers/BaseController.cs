using BAL.Repositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace COD.Controllers
{
    public class BaseController : Controller
    {
        accessControlRepository accessControlRepo;
        public BaseController()
        {
            accessControlRepo = new accessControlRepository();
        }

        UserSession _user;

        public UserSession currentUser
        {
            get
            {
                if (_user == null && System.Web.HttpContext.Current.Session["UserSession"] != null)
                {
                    _user = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];
                    _user.moduleList = accessControlRepo.getAuthorizePageList(_user.User.tblUserRoles.Select(x => x.RoleID).ToList(), _user.User.UserID, false);
                }
                return _user;
            }
            set
            {
                _user = value;
            }
        }



        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var urlHelper = new UrlHelper(filterContext.RequestContext);

            string redirectController = string.Empty;
            string redirectAction = string.Empty;
            string ErrorMessage = string.Empty;
            int StatusCode = 0;
            bool authorizedRequest = true;

            //Current Request
            HttpRequest req = System.Web.HttpContext.Current.Request;

            //Read Access Token
            bool isValidToken = false;
            bool rememberMe = false;
            string accToken = string.Empty;
            TokenDetailUserVM token = null;
            AppSettingViewModel appSetting = CacheModel.AppSettings;

            //Read Token From Cookies
            HttpCookie cookie = req.Cookies["acc_token"];
            if (cookie != null)
            {
                accToken = cookie["acc_token"].ToString();
                rememberMe = cookie["keep_live"].ToString() == "1" ? true : false;
                token = accessControlRepository.validateUserAccessToken(accToken);
                isValidToken = token != null ? token.isValid : false;
            }

            //Re Initialize Session if expired and app settings usecookies is true
            if (currentUser == null && appSetting.UseCookies && rememberMe && isValidToken)
            {
                //Refresh Token if expired
                if(token.isExpired)
                {
                    var refreshToken = refreshAccessToken(accToken);
                    if(refreshToken != null && refreshToken.Code == (int)ResponseCode.Success)
                    {
                        accToken = refreshToken.accessToken;
                        token = accessControlRepository.validateUserAccessToken(accToken);
                        cookie["acc_token"] = accToken;
                        Response.Cookies.Set(cookie);
                    }
                }

                var model =  Models.commonServices.getUserById(token.userId, accToken);
                if (model != null && model.Users != null && model.Users.Count > 0)
                {
                    UserSession sess = new UserSession();
                    sess.User = model.Users.FirstOrDefault();
                    System.Web.HttpContext.Current.Session.Add("UserSession", sess);
                }
            }


            if (currentUser != null && isValidToken && !token.isExpired)
            {

                currentUser.userAccessToken = accToken;
                //Find Requested Controller and Action Name
                var rd = req.RequestContext.RouteData;
                string currentController = rd.GetRequiredString("controller");
                string currentAction = filterContext.ActionDescriptor.ActionName;

                //Build Attributes List Applied On Requested Action
                var attrFilter = filterContext.ActionDescriptor.GetFilterAttributes(true);
                List<string> attrList = new List<string>();
                if (attrFilter != null && attrFilter.Count() > 0)
                {
                    attrFilter.ToList().ForEach(x =>
                    {
                        var attr = x.TypeId as Type;
                        attrList.Add(attr.Name);
                    });
                }

                currentUser.thisPage = currentUser.moduleList.Where(s => s.tblPages.Where(x => x.Controller.ToLower() == currentController.ToLower()).Count() > 0).Select(s => s.tblPages.Where(p => p.Controller.ToLower() == currentController.ToLower()).FirstOrDefault()).FirstOrDefault();
                var rights = currentUser.thisPage != null ? currentUser.thisPage.tblPermissions.Where(pe => pe.tblPermissionActionJuncs.Where(a => a.Action.ToLower() == currentAction.ToLower()).Count() > 0).FirstOrDefault() : null;

                //currentUser.thisPage = currentUser.moduleList.Select(s => s.tblPages.Where(x => x.Controller.ToLower() == currentController.ToLower()).FirstOrDefault()).FirstOrDefault();
                //var rights = currentUser.thisPage.tblPermissions.Where(pe => pe.tblPermissionActionJuncs.Where(a => a.Action.ToLower() == currentAction.ToLower()).Count() > 0).FirstOrDefault();


                if (rights == null)
                {
                    authorizedRequest = false;
                    redirectController = "Error";
                    redirectAction = "Permission";
                    ErrorMessage = "UnAuthorized";
                    StatusCode = (int)ResponseCode.AccessDenied;
                }
            }
            else
            {
                authorizedRequest = false;
                redirectController = "Login";
                redirectAction = "Index";
                ErrorMessage = "SessionExpired";
                StatusCode = (int)ResponseCode.UnAuthorized;
            }


            if (!authorizedRequest)
            {
                if (!filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new RedirectResult("~/" + redirectController + "/" + redirectAction + "");
                }
                else
                {
                    filterContext.HttpContext.Response.StatusCode = StatusCode;
                    filterContext.Result = new JsonResult
                    {
                        Data = new
                        {
                            Error = ErrorMessage,
                            LogOnUrl = urlHelper.Action(redirectAction, redirectController)
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
            }

            base.OnActionExecuting(filterContext);
        }


        private RefreshTokenResponseModel refreshAccessToken(string oldToken)
        {
            string url = utilityRepository.getBaseUrl() + "token/refreshaccesstoken";
            string urlParameter = @"accesstoken=" + HttpUtility.UrlEncode(oldToken);
            return utilityRepository.getResponse<RefreshTokenResponseModel>(url, urlParameter, false);
        }

     
    }
}