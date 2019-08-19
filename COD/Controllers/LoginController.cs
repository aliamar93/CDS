using BAL.Repositories;
using COD.Models;
using DAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//To Generate Permission Class
using System.CodeDom;
using System.CodeDom.Compiler;


namespace COD.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
          //  string test = CacheModel.codServiceAccessToken;
            UserSession sess = new UserSession();
            var userSess = Session["UserSession"];
            var cookie = Request.Cookies["acc_token"];
            string accessToken = string.Empty;
            string landingController = string.Empty;
            string landingtAction = string.Empty;
            if (userSess != null)
            {
                sess = (UserSession)userSess;
                Session["UserSession"] = sess;

                //Redirect User to landing page
                if (!string.IsNullOrEmpty(sess.User.LandingPage))
                {
                    landingController = sess.User.LandingPage.Split('/')[0];
                    landingtAction = sess.User.LandingPage.Split('/')[1];
                    return Redirect(sess.User.LandingPage);
                }
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }
                Session.Clear();
                Session.Abandon();
            }
            else
            {
                if(CacheModel.AppSettings.UseCookies && cookie != null && cookie["keep_live"].ToString() == "1")
                {
                    var userAccToken = cookie["acc_token"];
                    if(userAccToken != null && !string.IsNullOrEmpty(userAccToken.ToString()))
                    {
                        accessToken = userAccToken.ToString();
                        var token = accessControlRepository.validateUserAccessToken(accessToken);
                        if(token.isValid)
                        {
                            if (token.isExpired)
                            {
                                string url = utilityRepository.getBaseUrl() + "token/refreshaccesstoken";
                                string urlParameter = @"accesstoken=" + HttpUtility.UrlEncode(userAccToken.ToString());
                                var model = utilityRepository.getResponse<RefreshTokenResponseModel>(url, urlParameter, false);
                                //Update new Token
                                if (model.Code == (int)ResponseCode.Success)
                                {
                                    accessToken = model.accessToken;
                                    cookie["acc_Token"] = accessToken;
                                    Response.Cookies.Set(cookie);
                                }
                            }

                            var userModel = commonServices.getUserById(token.userId, accessToken); //new userRepository().getUserById(token.userId, new string[] { "tblUserRoles.tblRole" });
                            if (userModel != null && userModel.Users.Count > 0)
                            {
                                //New Session -- Keep User Logged In
                                sess.User = userModel.Users.FirstOrDefault();
                                System.Web.HttpContext.Current.Session["UserSession"] = sess;
                                
                                //Redirect User to landing page
                                if (!string.IsNullOrEmpty(sess.User.LandingPage))
                                {
                                    landingController = sess.User.LandingPage.Split('/')[0];
                                    landingtAction = sess.User.LandingPage.Split('/')[1];
                                }
                                else
                                {
                                    landingController = "Dashboard";
                                    landingtAction = "Index";
                                }
                                return RedirectToAction(landingtAction, landingController);

                            }
                            else
                            {
                                Response.Cookies.Clear();
                            }
                        }
                    }
                }
            }
              //  GenerateClass();
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(LoginViewModel vm)
        {
            //AuthenticateUser Api Call
            string url = utilityRepository.getBaseUrl() + "login/AuthenticateUser";
            var model = utilityRepository.getResponse<AuthenticationResponseModel>(url, JsonConvert.SerializeObject(vm), true);

            //Checking Response
            if (model != null && model.Code == (int)ResponseCode.Success)
            {
                //Create New Session
                UserSession sess = new UserSession();
                sess.User = model.user;
                Session.Add("UserSession", sess);

                double days = vm.remember && CacheModel.AppSettings.UseCookies ? 100d : 1d;

                //Write access token in cookie
                HttpCookie cookie = Request.Cookies["acc_token"];
                if (cookie == null)
                {
                    cookie = new HttpCookie("acc_token");
                }

                cookie["acc_token"] = model.accessToken;
                cookie["keep_live"] = vm.remember && CacheModel.AppSettings.UseCookies ? "1" : "0";
                cookie.Expires = DateTime.Now.AddDays(days);
                Response.Cookies.Add(cookie);

                string landingController = string.Empty;
                string landingtAction = string.Empty;
                //Redirect User to landing page
                if (!string.IsNullOrEmpty(sess.User.LandingPage))
                {
                    return Redirect(sess.User.LandingPage);
                }
                return RedirectToAction("Logout");



            }
            else
            {
                ViewBag.lblMsg = model.Detail;
                return View("Index", vm);
            }

            //return RedirectToAction("Index");
        }


        public ActionResult Logout()
        {
            HttpCookie cookie = Request.Cookies["acc_token"];
            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);

            Session.Clear();
            Session.Abandon();

            return RedirectToAction("Index");
        }

        #region GeneratePermissionClass
        private void GenerateClass()
        {
            CodeCompileUnit targetUnit = new CodeCompileUnit();
            CodeNamespace globalNamespace = new CodeNamespace();
            CodeTypeDeclaration targetClass = new CodeTypeDeclaration("PermissionEnum");
            targetClass.Attributes = MemberAttributes.Public;
            targetClass.IsClass = true;

            globalNamespace.Imports.Add(new CodeNamespaceImport("System"));
            targetUnit.Namespaces.Add(globalNamespace);

            CodeNamespace samples = new CodeNamespace();
            var dblst = new accessControlRepository().getPermissionName().Select(x => x.Permission).Distinct().ToList();
            foreach (var item in dblst)
            {

                CodeMemberField cField = new CodeMemberField();
                cField.Attributes = MemberAttributes.Public | MemberAttributes.Const;
                cField.Type = new CodeTypeReference(typeof(string));
                cField.Name = item.Replace(" ", "");
                cField.InitExpression = new CodePrimitiveExpression(item);
                targetClass.Members.Add(cField);


            }
            samples.Types.Add(targetClass);
            targetUnit.Namespaces.Add(samples);

            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            CodeGeneratorOptions options = new CodeGeneratorOptions();
            options.BracingStyle = "C";
            string path = System.Web.HttpContext.Current.Server.MapPath("\\Models\\PermissionEnum.cs");
            using (System.IO.StreamWriter sourceWriter = new System.IO.StreamWriter(path))
            {
                provider.GenerateCodeFromCompileUnit(targetUnit, sourceWriter, options);
            }
        }
        #endregion
    }
}