using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class TokenRequestVM
    {
        public string AppID { get; set; }
        public string AppSecret { get; set; }
        public string CallBackUrl { get; set; }
    }
 
    public class TokenResponseVM : BaseRespone
    {
        public string BearerToken { get; set; }
        public string ValidTill { get; set; }
    }

    public class TokenDetailBaseVM
    {
        public string Data { get; set; }
        public string Expire { get; set; }
        public bool isExpired { get; set; }
        public bool isValid { get; set; }
    }

    public class TokenDetailUserVM : TokenDetailBaseVM
    {
        public int userId { get; set; }
        public int[] roleIds { get; set; }
    }

    public class TokenDetailAPIVM : TokenDetailBaseVM
    {
        public string AppID { get; set; }
    }

    public class RefreshTokenResponseModel : BaseRespone
    {
        public string accessToken { get; set; }
    }
}
