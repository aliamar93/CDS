using BAL.Repositories;
using DAL.DBEntities;
using DAL.Models;
using DAL.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ServiceCOD.Controllers
{
    public class ProductController : BaseApiController
    {

        productRepository prodRepo;

        public ProductController()
        {
            prodRepo = new productRepository(new DBEntities());
        }


        public async Task<IHttpActionResult> getProductTypeList(string accesstoken)
        {
          ProductTypeListResponseVM  vm = new ProductTypeListResponseVM();
            try
            {
                var notifications = utilityRepository.getNotificationJSON<ProductNotification>(NotificationConstants.ProductNotificationFile);
                vm.productTypeList = await prodRepo.getProductTypeList();

                vm.Response(ResponseCode.Success, notifications.found.ReplaceNoticationToken(vm.productTypeList.Count.ToString()));
            }
            catch (Exception ex)
            {
                vm.Response(ResponseCode.Error, ex.Message);
            }
            return Ok(vm);

        }

        [HttpGet]
        public async Task<IHttpActionResult> getProducts(string accesstoken, int? productid, int? startfrom, int? pagesize, string search, string orderby)
        {
            ProductListResponseVM vm = new ProductListResponseVM();
            try
            {
                var notifications = utilityRepository.getNotificationJSON<ProductNotification>(NotificationConstants.ProductNotificationFile);
                vm.products = await prodRepo.getProductListAsync(new string[] { "tblProductType" }, productid, startfrom, pagesize, search, orderby);
                if (productid == null || productid == 0)
                {
                    vm.TotalProductsCount = prodRepo.TotalProductCount;
                }
                vm.Response(ResponseCode.Success, notifications.found.ReplaceNoticationToken(vm.products.Count.ToString()));
            }
            catch (Exception ex)
            {
                vm.Response(ResponseCode.Error, ex.Message);
            }
            return Ok(vm);
        }
        [HttpPost]
        public async Task<IHttpActionResult> createProduct(CreateProductRequest model)
        {
            BaseRespone response = new BaseRespone();
            if (ModelState.IsValid)
            {
                var notifications = utilityRepository.getNotificationJSON<ProductNotification>(NotificationConstants.ProductNotificationFile);
                var productTypeList = await prodRepo.getProductTypeList();
                var productType = productTypeList.Where(x => x.ID == model.product.ProductTypeID).FirstOrDefault();

                if (productType != null)
                {
                    model.product.ProductCode = productType.Initials + productType.ID.ToString().PadLeft(4,'0');
                    prodRepo.createProduct(model.product, currentToken.userId);
                    await prodRepo.SaveChangesAsync();
                    response.Response(ResponseCode.Success, notifications.onsuccess.ReplaceNoticationToken(model.product.Name));
                }
                else
                {
                    response.Response(ResponseCode.Failed, notifications.onfailed);
                }
            }
            else
            {
                string errormodel = string.Empty;
                ModelState.Values.Where(x => x.Errors.Count > 0).ToList().ForEach(x => { errormodel += string.Join(",", x.Errors.Select(e => e.ErrorMessage).ToList()); });
                response.Response(ResponseCode.InvalidModel, errormodel);
            }
            return Ok(response);
        }
        [HttpPost]
        public async Task<IHttpActionResult> updateProduct(CreateProductRequest model)
        {
            BaseRespone response = new BaseRespone();
            if (ModelState.IsValid)
            {
                var notifications = utilityRepository.getNotificationJSON<ProductNotification>(NotificationConstants.ProductNotificationFile);
                var productTypeList = await prodRepo.getProductTypeList();
                var productType = productTypeList.Where(x => x.ID == model.product.ProductTypeID).FirstOrDefault();

                if (productType != null)
                {
                    model.product.ProductCode = productType.Initials + productType.ID.ToString().PadLeft(4, '0');
                    prodRepo.updateProduct(model.product, currentToken.userId);
                    await prodRepo.SaveChangesAsync();
                    response.Response(ResponseCode.Success, notifications.onupdate.ReplaceNoticationToken(model.product.Name));
                }
                else
                {
                    response.Response(ResponseCode.Failed, notifications.onfailed);
                }
            }
            else
            {
                string errormodel = string.Empty;
                ModelState.Values.Where(x => x.Errors.Count > 0).ToList().ForEach(x => { errormodel += string.Join(",", x.Errors.Select(e => e.ErrorMessage).ToList()); });
                response.Response(ResponseCode.InvalidModel, errormodel);
            }
            return Ok(response);
        }
        [HttpPost]
        public async Task<IHttpActionResult> deleteProduct(DeleteProductRequest model)
        {
            BaseRespone response = new BaseRespone();
            if (ModelState.IsValid)
            {
                var notifications = utilityRepository.getNotificationJSON<ProductNotification>(NotificationConstants.ProductNotificationFile);
                var product = await prodRepo.getProductByIdAsync(model.productid, null);
                if (product != null)
                {                   
                    prodRepo.deleteProduct(product, currentToken.userId);
                    await prodRepo.SaveChangesAsync();
                    response.Response(ResponseCode.Success, notifications.ondeleted.ReplaceNoticationToken(product.Name));
                }
                else
                {
                    response.Response(ResponseCode.Failed, notifications.onfailed);
                }
            }
            else
            {
                string errormodel = string.Empty;
                ModelState.Values.Where(x => x.Errors.Count > 0).ToList().ForEach(x => { errormodel += string.Join(",", x.Errors.Select(e => e.ErrorMessage).ToList()); });
                response.Response(ResponseCode.InvalidModel, errormodel);
            }
            return Ok(response);
        }
    }
}
