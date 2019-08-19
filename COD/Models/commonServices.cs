using BAL.Repositories;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COD.Models
{
    public class commonServices
    {
        public static UserListResponseVM getUserById(int UserID, string AccessToken)
        {
            string url = utilityRepository.getBaseUrl() + "user/getusers";
            string urlParameter = @"accesstoken=" + HttpUtility.UrlEncode(AccessToken) + "&&userid=" + UserID.ToString();
            urlParameter += "&&startfrom=&&pagesize=&&search=&&orderby=";
            return utilityRepository.getResponse<UserListResponseVM>(url, urlParameter, false);
        }

        public static ProductListResponseVM getProductById(int ProductID, string AccessToken)
        {
            string url = utilityRepository.getBaseUrl() + "product/getproducts";
            string urlParameter = @"accesstoken=" + HttpUtility.UrlEncode(AccessToken) + "&&productid=" + ProductID.ToString();
            urlParameter += "&&startfrom=&&pagesize=&&search=&&orderby=";
            return utilityRepository.getResponse<ProductListResponseVM>(url, urlParameter, false);
        }
    }
}