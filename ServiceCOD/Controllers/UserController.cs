using BAL.Repositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ServiceCOD.Controllers
{
    public class UserController : BaseApiController
    {
        userRepository userRepo;
        public UserController()
        {
            userRepo = new userRepository(new DAL.DBEntities.DBEntities());
        }

        //[HttpGet]
        //public IHttpActionResult getUsers(string accesstoken, int? userid)
        //{
        //    var token = accessControlRepository.validatePortalAccessToken(accesstoken);
        //    if (token.isValid)
        //    {
        //        UserListResponseVM vm = new UserListResponseVM();
        //        vm.Users = userRepo.getUserList(new string[] { "tblUserRoles.tblRole" }, userid);
        //        vm.Code = (int)ResponseCode.Success;
        //        vm.Message = ResponseCode.Success.ToString();
        //        vm.Detail = vm.Users.Count.ToString() + " record found.";
        //        return Ok(vm);
        //    }
        //    else
        //    {
        //        BaseRespone vm = new BaseRespone();
        //        vm.Code = (int)ResponseCode.AccessDenied;
        //        vm.Message = ResponseCode.AccessDenied.ToString();
        //        vm.Detail = "Invalid Access Token";
        //        return Content(HttpStatusCode.Forbidden, vm);
        //    }
        //}

        [HttpGet]
        public IHttpActionResult getUsers(string accesstoken, int? userid, int? startfrom, int? pagesize, string search, string orderby)
        {
                UserListResponseVM vm = new UserListResponseVM();
            try
            {
                vm.Users = userRepo.getUserList(new string[] { "tblUserRoles.tblRole" }, userid, startfrom, pagesize, search, orderby);
                if (userid == null || userid == 0)
                {
                    vm.TotalUsersCount = userRepo.TotalUserCount;
                }
                else
                {
                    if (vm.Users != null && vm.Users.Count > 0)
                    {
                        vm.Users.FirstOrDefault().RolesName = string.Join(",", vm.Users.FirstOrDefault().tblUserRoles.Select(x => x.RoleID.ToString()).ToList());
                        vm.Users.FirstOrDefault().Password = string.Empty;
                    }
                }
                string msgsuccess = vm.Users.Count.ToString() + " record found.";
                vm.Response(ResponseCode.Success, msgsuccess);
            }
            catch(Exception ex)
            {
                vm.Response(ResponseCode.Error, ex.Message);
            }
                return Ok(vm);
            
        }


        [HttpPost]
        public IHttpActionResult createUser(CreateUserRequest model)
        {
            BaseRespone response = new BaseRespone();
            ModelState.Remove("UserID");
            var err = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => x).ToList();
            ResponseCode code = ResponseCode.Failed;
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    model.user.Password = utilityRepository.ComputeHash(model.user.Password);
                    model.user.ConfirmPassword = model.user.Password;

                    using (var dbtran = userRepo.DBContext.Database.BeginTransaction())
                    {
                        try
                        {
                            model.user = userRepo.createUser(model.user, currentToken.userId);
                            userRepo.SaveChanges();

                            userRepo.AssignRolesAndPagesToUser(model.user, model.user.RoleIdList, model.user.LandingPageId);
                            userRepo.SaveChanges();
                            dbtran.Commit();
                            code = ResponseCode.Success;
                             message = "User '" + model.user.UserID.ToString() + "' created successfully"; ;
                        }
                        catch (Exception ex)
                        {
                            dbtran.Rollback();
                            code = ResponseCode.Error;
                            message = ex.Message;
                        }
                    }
                    response.Response(code, message);
                }
                catch (Exception ex)
                {                    
                    response.Response(ResponseCode.Error, ex.Message);
                }

            }
            else
            {
                string errormodel= string.Empty;
                ModelState.Values.Where(x => x.Errors.Count > 0).ToList().ForEach(x => { errormodel += string.Join(",", x.Errors.Select(e => e.ErrorMessage).ToList()); });
                response.Response(ResponseCode.InvalidModel, errormodel);
            }

            return Ok(response);
        }


        [HttpGet]
        public IHttpActionResult isUsernameAailable(string accesstoken, string UserName)
        {
            CheckUsernameExistResponse response = new CheckUsernameExistResponse();
            
            if (!string.IsNullOrEmpty(UserName))
            {
                var user = userRepo.getUserByUserId(UserName);
                if (user == null)
                {
                    response.IsUsernameAvailable = true;
                    response.Response(ResponseCode.Success, "Username is available");
                }
                else
                {
                    response.Response(ResponseCode.Failed, "Username is already taken");
                }
            }
            return Ok(response);
        }
    }
}
