using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DBEntities
{
    [MetadataType(typeof(tblProduct.tblProductMetaData))]
    public partial class tblProduct
    {
        public sealed class tblProductMetaData
        {
            [Required(ErrorMessage = "Product Name is Required.")]
            [MaxLength(50, ErrorMessage = "Maximum 50 characters are allowed.")]
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            public string Name { get; set; }

            [Required(ErrorMessage = "Product Type is Required.")]
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            public int ProductTypeID { get; set; }

            [MaxLength(50, ErrorMessage = "Maximum 50 characters are allowed.")]
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            public string SKU { get; set; }

            [MaxLength(200, ErrorMessage = "Maximum 200 characters are allowed.")]
            [DisplayFormat(ConvertEmptyStringToNull = false)]
            public string Description { get; set; }
        }
    }
}
