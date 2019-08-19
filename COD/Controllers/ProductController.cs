using BAL.Repositories;
using COD.Models;
using DAL.DBEntities;
using DAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace COD.Controllers
{
    public class ProductController : BaseController
    {

        // GET: Product
        public ActionResult Index(string id)
        {
            long? productid = string.IsNullOrEmpty(id) ? (int?)null : Convert.ToInt32(id);
            TempData["productid"] = productid;
            return View();
        }

        [HttpPost]
        public ActionResult Index()
        {
            string productid = TempData["productid"] == null ? string.Empty : TempData["productid"].ToString();
            string search = Request.Form.GetValues("search[value]")[0];
            string draw = Request.Form.GetValues("draw")[0];
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
            int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
            string dir = orderDir == "asc" ? "ascending" : "descending";
            string sortColumn = getSortedColumnName(order) + " " + dir;

            string url = utilityRepository.getBaseUrl() + "product/getproducts";
            string urlParameter = @"accesstoken=" + HttpUtility.UrlEncode(currentUser.userAccessToken) + "&&productid=" + productid;
            urlParameter += "&&startfrom=" + startRec.ToString() + "&&pagesize=" + pageSize.ToString() + "&&search=" + search + "&&orderby=" + sortColumn;
            var model = utilityRepository.getResponse<ProductListResponseVM>(url, urlParameter, false);
            if (model.Code == (int)ResponseCode.Success)
            {
                var grdViewList = model.products.Select(x => new ProductGridViewModal
                {
                     ProductID = x.ID,
                     Name = x.Name,
                    ProductCode = x.ProductCode,
                    ProductType = x.tblProductType != null ? x.tblProductType.TypeName : string.Empty,
                    SKU = x.SKU

                }).ToList();

                return Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = grdViewList.Count,
                    recordsFiltered = model.TotalProductsCount,
                    data = grdViewList
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { draw = 1, recordsTotal = "", recordsFiltered = "", error = new { code = -32601, message = model.Detail } }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(int id)
        {
            var model = commonServices.getProductById(id, currentUser.userAccessToken);
            if (model.Code == (int)ResponseCode.Success && model.products.Count > 0)
            {
                ViewBag.ProductTypeList = getProductTypeList().productTypeList.Select(x => new { x.ID, x.TypeName }).ToList();               
                return View("_Save", model.products.FirstOrDefault());
            }

            return Json(new { success = false, msg = model.Message }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Save()
        {
            ViewBag.ProductTypeList = getProductTypeList().productTypeList.Select(x => new { x.ID, x.TypeName }).ToList();
            return View("_Save", new tblProduct());
        }

        [HttpPost]
        public async Task<ActionResult> Save(tblProduct product)
        {
            bool isSuccess = false;
            string MessageKey = TempKey.FailedMsg.ToString();
            ModelState.Remove("ID");
            //var err = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => x).ToList();

            if (ModelState.IsValid)
            {
                CreateProductRequest req = new CreateProductRequest();
                req.accesstoken = currentUser.userAccessToken;
                req.product = product;
                string url = utilityRepository.getBaseUrl() + "product/createproduct";
                var model = await utilityRepository.getAsyncResponse<BaseRespone>(url, JsonConvert.SerializeObject(req), true);
                isSuccess = model.Code == (int)ResponseCode.Success ? true : false;
                MessageKey = isSuccess ? TempKey.SuccessMsg.ToString() : MessageKey;
                TempData[MessageKey] = model.Detail;
                return Json(new { success = isSuccess }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.ProductTypeList = getProductTypeList().productTypeList.Select(x => new { x.ID, x.TypeName}).ToList();
            return View("_Save", product);
        }


        public ActionResult Update(int id)
        {
            var model = commonServices.getProductById(id, currentUser.userAccessToken);
            if (model.Code == (int)ResponseCode.Success && model.products.Count > 0)
            {
                ViewBag.ProductTypeList = getProductTypeList().productTypeList.Select(x => new { x.ID, x.TypeName }).ToList();
                ViewBag.ActionName = "Update";
                return View("_Save", model.products.FirstOrDefault());
            }

            return Json(new { success = false, msg = model.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Update(tblProduct product)
        {
            bool isSuccess = false;
            string MessageKey = TempKey.FailedMsg.ToString();
            ModelState.Remove("ID");
            //var err = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => x).ToList();

            if (ModelState.IsValid)
            {
                CreateProductRequest req = new CreateProductRequest();
                req.accesstoken = currentUser.userAccessToken;
                req.product = product;
                string url = utilityRepository.getBaseUrl() + "product/updateproduct";
                var model = await utilityRepository.getAsyncResponse<BaseRespone>(url, JsonConvert.SerializeObject(req), true);
                isSuccess = model.Code == (int)ResponseCode.Success ? true : false;
                MessageKey = isSuccess ? TempKey.SuccessMsg.ToString() : MessageKey;
                TempData[MessageKey] = model.Detail;
                return Json(new { success = isSuccess }, JsonRequestBehavior.AllowGet);
            }
            ViewBag.ProductTypeList = getProductTypeList().productTypeList.Select(x => new { x.ID, x.TypeName }).ToList();
            return View("_Save", product);
        }

        [HttpGet]
        public async Task<JsonResult> Delete(long id)
        {
            bool isSuccess = false;
            string MessageKey = TempKey.FailedMsg.ToString();

            if (id > 0)
            {
                DeleteProductRequest req = new DeleteProductRequest();
                req.accesstoken = currentUser.userAccessToken;
                req.productid = id;
                string url = utilityRepository.getBaseUrl() + "product/deleteproduct";
                var model = await utilityRepository.getAsyncResponse<BaseRespone>(url, JsonConvert.SerializeObject(req), true);
                isSuccess = model.Code == (int)ResponseCode.Success ? true : false;
                MessageKey = isSuccess ? TempKey.SuccessMsg.ToString() : MessageKey;
                TempData[MessageKey] = model.Detail;
            }
            return Json(new { success = isSuccess }, JsonRequestBehavior.AllowGet);
        }

        #region privateMethods

        private ProductTypeListResponseVM getProductTypeList()
        {
            //getUsers Api Call
            string url = utilityRepository.getBaseUrl() + "product/getProductTypeList";
            string urlParameter = @"accesstoken=" + HttpUtility.UrlEncode(currentUser.userAccessToken);
            var model = utilityRepository.getResponse<ProductTypeListResponseVM>(url, urlParameter, false);
            return model;
        }

        private string getSortedColumnName(string index)
        {
            int ind = Convert.ToInt32(index);
            string fieldName = string.Empty;
            switch (ind)
            {
                case 0:
                    fieldName = "ID";
                    break;
                case 1:
                    fieldName = "Name";
                    break;
                case 2:
                    fieldName = "ProductCode";
                    break;
                case 3:
                    fieldName = "ProductType";
                    break;
                case 4:
                    fieldName = "SKU";
                    break;
                default:
                    fieldName = "ID";
                    break;
            }

            return fieldName;
        }
        #endregion
    }
}