using DAL.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ProductTypeListResponseVM : BaseRespone
    {
        public List<tblProductType> productTypeList { get; set; }
    }

    public class ProductListResponseVM : BaseRespone
    {
        public int TotalProductsCount { get; set; }
        public List<tblProduct> products { get; set; }
    }

    public class CreateProductRequest : BaseRequest
    {
        public tblProduct product { get; set; }
    }

    public class DeleteProductRequest : BaseRequest
    {
        public long productid { get; set; }
    }

    public class CheckProductNameExistResponse : BaseRespone
    {
        public bool IsProductNameAvailable { get; set; }
    }

}
