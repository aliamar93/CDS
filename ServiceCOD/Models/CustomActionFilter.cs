using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using BAL.Repositories;
using DAL.Models;
using System.Net;
using System.Net.Http;

namespace ServiceCOD.Controllers //Models
{
    public class CustomActionFilter : ActionFilterAttribute, IActionFilter //ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var baseController = (BaseApiController)actionContext.ControllerContext.Controller;

            string currentController = actionContext.ControllerContext.ControllerDescriptor.ControllerName;
            string currentAction = actionContext.ActionDescriptor.ActionName;
            string verb = Convert.ToString(actionContext.Request.Method);

            //Check Api Authentication
            var headerAuthorization = actionContext.Request.Headers.Where(h => h.Key.ToLower() == "authentication").FirstOrDefault().Value;
            if (headerAuthorization != null)
            {
                string headerToken = headerAuthorization.FirstOrDefault();
                var bearerToken = accessControlRepository.validateBearerToken(headerToken);

                if (bearerToken == null || !bearerToken.isValid || bearerToken.isExpired)
                {
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, ResponseCode.TokenExpired.ToString().FromCamelCase());
                }
                else
                {
                    if (actionContext.ActionDescriptor.GetCustomAttributes<IgnorePermission>(true).Count == 0)
                    {
                        //Check User Authorization
                        string accessToken = string.Empty;
                        var qryStringCollection = actionContext.Request.GetQueryNameValuePairs();

                        if (qryStringCollection != null && qryStringCollection.Count() > 0)
                        {
                            accessToken = qryStringCollection.Where(x => x.Key.ToLower() == "accesstoken").FirstOrDefault().Value;
                        }

                        if (string.IsNullOrEmpty(accessToken))
                        {
                            if (actionContext.ActionArguments != null && actionContext.ActionArguments.Count > 0)
                            {
                                var actionModel = actionContext.ActionArguments.FirstOrDefault().Value;
                                var modelCast = (BaseRequest)actionModel;
                                accessToken = modelCast.accesstoken;
                            }
                        }

                        //Validate User Access Token
                        var userTokenDetail = accessControlRepository.validateUserAccessToken(accessToken);
                        if (!userTokenDetail.isValid || userTokenDetail.isExpired)
                        {
                            BaseRespone bResponse = new BaseRespone();
                            bResponse.Code = (int)ResponseCode.TokenExpired;
                            bResponse.Message = ResponseCode.TokenExpired.ToString().FromCamelCase();
                            bResponse.Detail = "Invalid access token";

                            var respMsg = actionContext.Request.CreateResponse(HttpStatusCode.NonAuthoritativeInformation, ResponseCode.TokenExpired.ToString().FromCamelCase());
                            respMsg.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(bResponse));
                            actionContext.Response = respMsg;
                        }
                        else
                        {
                            //Validate API Access
                            var endPointList = new accessControlRepository().getAuthorizeEndPointList(userTokenDetail.roleIds, true);
                            var requestedEndPoint = endPointList.Where(e => e.APIController.ToLower() == currentController.ToLower() && e.APIName.ToLower() == currentAction.ToLower()).FirstOrDefault();
                            baseController.currentToken = userTokenDetail;
                            if (requestedEndPoint == null)
                            {
                                BaseRespone bResponse = new BaseRespone();
                                bResponse.Code = (int)ResponseCode.AccessDenied;
                                bResponse.Message = ResponseCode.AccessDenied.ToString().FromCamelCase();
                                bResponse.Detail = "Access control list violation occured.";

                                var respMsg = actionContext.Request.CreateResponse(HttpStatusCode.NonAuthoritativeInformation, ResponseCode.AccessDenied.ToString().FromCamelCase());
                                respMsg.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(bResponse));
                                actionContext.Response = respMsg;
                            }
                        }
                    }
                }
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, ResponseCode.UnAuthorized.ToString().FromCamelCase());
            }

            base.OnActionExecuting(actionContext);
        }

    }
}