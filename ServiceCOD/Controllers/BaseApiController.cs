using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace ServiceCOD.Controllers
{
    [CustomActionFilter] //Models.
    public class BaseApiController : ApiController
    {

        public DAL.Models.TokenDetailUserVM currentToken { get; set; }
        public BaseApiController()
        {

        }
    }
}
