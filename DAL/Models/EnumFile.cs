using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public enum ResponseCode
    {
        Success = 200,
        BadRequest = 400,
        UnAuthorized = 401,
        NotFound = 404,
        Failed = 405,
        Error = 500,
        AccessDenied = 501,
        InvalidModel = 505,
        TokenExpired = 510
    }

}
