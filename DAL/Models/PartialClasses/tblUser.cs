using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DAL.DBEntities
{
    [MetadataType(typeof(tblUser.tblUserMetaData))]
    public partial class tblUser
    {
        public sealed class tblUserMetaData
        {
            [Required(ErrorMessage = "Name is Required.")]
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Username is Required.")]
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            [RegularExpression(@"[(\S)+]{4,}$", ErrorMessage = "Username must have minimum 4 characters without space")]
            [Remote("isUsernameAailable", "User", ErrorMessage = "This {0} is already in use.")]
            public string UserLoginID { get; set; }

            [Required(ErrorMessage = "Password is Required.")]
            //  [RegularExpression(@"(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])([a-zA-Z0-9]{5,})$", ErrorMessage = "Minimum 5 characters one digit and one lower alpha and one upper alpha are required")]
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            public string Password { get; set; }

            [Required(ErrorMessage = "Password is Required.")]

            [DisplayFormat(ConvertEmptyStringToNull = false)]
            [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Password mismatch")]
            //[System.ComponentModel.DataAnnotations.Schema.NotMapped]

            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "phone is required.")]
          //  [RegularExpression(@"^[+. ]?([0-9]{11,11})$", ErrorMessage = "Invalid phone number")]
           // [Remote("isPhoneNoAvailable", "User", AdditionalFields = "UserID", ErrorMessage = "This {0} is already in use.")]
            public string Phone { get; set; }
           // [Required(ErrorMessage = "email is required.")]
            [EmailAddress(ErrorMessage = "Please type valid email address.")]
         //   [Remote("isEmailAvailable", "User", AdditionalFields = "UserID", ErrorMessage = "This {0} is already in use.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Landing Page is Required.")]
            public string LandingPageId { get; set; }

            [Required(ErrorMessage = "Role is Required.")]
            public string[] RoleIdList { get; set; }
        }
        public string[] RoleIdList { get; set; }
        public string ConfirmPassword { get; set; }
        public string RolesName { get; set; }
        public int LandingPageId { get; set; }
    }
}
