using BAL.Repositories;
using DAL.DBEntities;
using DAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ServiceCOD.Controllers
{
    public class LoginController : BaseApiController
    {

        accessControlRepository accessControlRepo;

        public LoginController()
        {
            accessControlRepo = new accessControlRepository(new DBEntities());
        }

        public string getTest()
        {

            return "testing";
        }

        [IgnorePermission]
        [HttpPost]
        public AuthenticationResponseModel AuthenticateUser(LoginViewModel vm)
        {
            AuthenticationResponseModel sess = new AuthenticationResponseModel();
            if (ModelState.IsValid)
            {
                string encpass = utilityRepository.ComputeHash(vm.password);
                tblUser user = accessControlRepo.authinticateUser(vm.username, encpass);

                if (user != null)
                {
                    if (user.tblUserRoles.Where(x => x.tblRole.IsActive == true).Count() > 0)
                    {

                        sess.user = user;
                        var pplst = accessControlRepo.getAuthorizePageList(user.tblUserRoles.Where(x => x.tblRole.IsActive == true).Select(x => x.RoleID).ToList(), user.UserID, true);

                        if (pplst.Count() > 0)
                        {
                            sess.tblModule = pplst;
                            string plainToken = user.UserID.ToString() + "-" + String.Join(",", user.tblUserRoles.Select(x => x.RoleID.ToString()).ToList()).TrimEnd(',') + "-" + DateTimeUTC.Now.ToString();
                            plainToken = plainToken.Replace("/", "@").Replace(" ", "+").Replace(":", "#");

                            sess.accessToken = utilityRepository.Encrypt(plainToken);
                            sess.Code = (int)ResponseCode.Success;
                            sess.Message = ResponseCode.Success.ToString();
                            sess.Detail = "User Logged in Successfully.";
                        }
                        else
                        {
                            sess.Code = (int)ResponseCode.AccessDenied;
                            sess.Message = ResponseCode.AccessDenied.ToString().FromCamelCase();
                            sess.Detail = "No permission found. Please assign some permission to user role.";
                        }
                    }
                    else
                    {
                        sess.Code = (int)ResponseCode.AccessDenied;
                        sess.Message = ResponseCode.AccessDenied.ToString().FromCamelCase();
                        sess.Detail = "Inactive Account. User role is inactive!";
                    }

                }
                else
                {
                    sess.Code = (int)ResponseCode.UnAuthorized;
                    sess.Message = ResponseCode.UnAuthorized.ToString();
                    sess.Detail = "Invalid Username / Password";
                }
            }
            else
            {
                sess.Code = (int)ResponseCode.InvalidModel;
                sess.Message = ResponseCode.InvalidModel.ToString().FromCamelCase();
                
                foreach(var err in ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => x).ToList())
                {
                    err.Value.Errors.ToList().ForEach(x => { sess.Detail += x.ErrorMessage + " "; });                    
                }
                sess.Detail = sess.Detail.TrimEnd(' ');
            }
            return sess;

        }

    }
}