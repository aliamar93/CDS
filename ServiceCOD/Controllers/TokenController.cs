using BAL.Repositories;
using DAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ServiceCOD.Controllers
{
    public class TokenController : ApiController
    {
        appClientRepository appClientRepo;

        public TokenController()
        {
            appClientRepo = new appClientRepository(new DAL.DBEntities.DBEntities());
        }

        [HttpPost]
        public IHttpActionResult getToken(TokenRequestVM vm)
        {
            if (vm != null)
            {
                var app = appClientRepo.authenticateApp(vm.AppID, vm.AppSecret);
                if (app != null)
                {

                    var response = new TokenResponseVM() { };
                    string plainToken = vm.AppID + "-" + Guid.NewGuid().ToString().Replace("-", "") + "-" + DateTimeUTC.Now.ToString();
                    response.BearerToken = utilityRepository.Encrypt(plainToken);
                    response.ValidTill = DateTimeUTC.Now.AddHours(CacheModel.AppSettings.TokenValidHours).ToString();                                        
                    response.Response(ResponseCode.Success, "Authenticated Successfully.");
                    return Ok(response);
                }
                else
                {
                    var response = new BaseRespone();
                    response.Response(ResponseCode.UnAuthorized, "Invalid App ID or Secret.");
                    return Ok(response);
                }
            }
            return BadRequest();                      
        }

        public IHttpActionResult getBearerTokenDetails(string token)
        {
            var tok = accessControlRepository.validateBearerToken(token);
            if(tok != null && tok.isValid && !tok.isExpired)
            {
                return Ok(tok);
            }
            else
            {
                var response = new BaseRespone();
                response.Response(ResponseCode.NotFound, "Invalid Token");
                return Ok(response);
            }
        }

        public IHttpActionResult getAccessTokenDetails(string token)
        {
            var tok = accessControlRepository.validateUserAccessToken(token);
            if (tok != null && tok.isValid && !tok.isExpired)
            {
                return Ok(tok);
            }
            else
            {
                var response = new BaseRespone();
                response.Response(ResponseCode.NotFound, "Invalid Token");
                return Ok(response);
            }
        }

        [IgnorePermission]
        public IHttpActionResult refreshAccesstoken(string accesstoken)
        {
            RefreshTokenResponseModel model = new RefreshTokenResponseModel();
            string msg = string.Empty;

            if (!string.IsNullOrEmpty(accesstoken))
            {
                var vm = accessControlRepository.validateUserAccessToken(accesstoken);
                if (vm.isValid)
                {
                    if (vm.isExpired)
                    {
                        string plainToken = vm.userId.ToString() + "-" + String.Join(",", vm.roleIds.ToList()).TrimEnd(',') + "-" + DateTimeUTC.Now.ToString();
                        plainToken = plainToken.Replace("/", "@").Replace(" ", "+");
                        model.accessToken = utilityRepository.Encrypt(plainToken);
                        msg = "Success";
                    }
                    else
                    {
                        model.accessToken = accesstoken;
                        msg = "No need to refresh token until its expired.";

                    }
                    model.Response(ResponseCode.Success, msg);
                }
                else
                {
                    model.Response(ResponseCode.NotFound, "Invalid Token");
                }
            }
            else
            {
                model.Response(ResponseCode.InvalidModel, ResponseCode.InvalidModel.ToString().FromCamelCase());
            }

            return Ok(model);
        }

    }
}
