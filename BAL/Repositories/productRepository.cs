using DAL.DBEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace BAL.Repositories
{
    public class productRepository : BaseRepository
    {
        public productRepository()
            : base()
        { }

        public productRepository(DBEntities contextDB)
            : base(contextDB)
        {
            DBContext = contextDB;
        }
        private int? _totalProductCount { get; set; }
        public int TotalProductCount
        {
            get
            {
                if (_totalProductCount == null)
                {

                    _totalProductCount = DBContext.tblProducts.Where(x => x.Deleted == null).Count();
                }
                return (int)_totalProductCount;
            }
        }


        public tblProduct getProductById(long ProductID, string[] includeList)
        {
            var query = getProductByIdQuery(ProductID, includeList);
            return query.FirstOrDefault();
        }

        public async Task<tblProduct> getProductByIdAsync(long ProductID, string[] includeList)
        {
            var query = getProductByIdQuery(ProductID, includeList);
            return await query.FirstOrDefaultAsync();
        }



        public List<tblProduct> getProductList(string[] includeList, long? productid, int? skip, int? take, string searchTerm, string orderBy)
        {

            var query = getProductListQuery(includeList, productid, skip, take, searchTerm, orderBy);
            return query.ToList();
        }

        public async Task<List<tblProduct>> getProductListAsync(string[] includeList, int? productid, int? skip, int? take, string searchTerm, string orderBy)
        {
            var query = getProductListQuery(includeList, productid, skip, take, searchTerm, orderBy);
            return await query.ToListAsync();
        }



        public tblProduct createProduct(tblProduct product, int createdBy)
        {
            product.CreatedBy = createdBy;
            product.Created = DateTimeUTC.Now;
            DBContext.tblProducts.Add(product);
            return product;
        }

        public tblProduct updateProduct(tblProduct product, int updatedBy)
        {
            product.UpdatedBy = updatedBy;
            product.Updated = DateTimeUTC.Now;

            DBContext.tblProducts.Attach(product);
            DBContext.UpdateExcept<tblProduct>(product, x => x.Created, x => x.CreatedBy);
            return product;
        }

        public tblProduct deleteProduct(tblProduct product, int deletedBy)
        {
            product.DeletedBy = deletedBy;
            product.Deleted = DateTimeUTC.Now;

            DBContext.tblProducts.Attach(product);
            DBContext.UpdateOnly<tblProduct>(product, x => x.Deleted, x => x.DeletedBy);
            return product;
        }



        //Other Entities
        public async Task<List<tblProductType>> getProductTypeList()
        {
            return await DBContext.ExclueAll().tblProductTypes.AsNoTracking().ToListAsync();
        }
        #region privateMethods

        private IQueryable<tblProduct> getProductByIdQuery(long ProductID, string[] includeList)
        {
            var query = DBContext.ExclueAll().tblProducts.AsQueryable();

            if (includeList != null && includeList.Count() > 0)
            {
                foreach (string tbl in includeList)
                {
                    query = query.Include(tbl);
                }
            }
            return query.Where(x => x.ID == ProductID && x.Deleted == null);
        }

        private IQueryable<tblProduct> getProductListQuery(string[] includeList, long? productid, int? skip, int? take, string searchTerm, string orderBy)
        {

            var query = DBContext.ExclueAll().tblProducts.AsNoTracking().AsQueryable();
            if (includeList != null && includeList.Count() > 0)
            {
                foreach (string tbl in includeList)
                {
                    query = query.Include(tbl);
                }
            }
            if (productid != null)
            {
                query = query.Where(x => x.ID == (int)productid && x.Deleted == null);
            }
            else
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    var searchList = searchTerm.Split(',').Select(x => x.ToLower()).ToList();
                    foreach (string term in searchList)
                    {
                        query = query.Where(x => x.Deleted == null && (x.ID.ToString().Contains(term) || x.Name.ToLower().Contains(term) || x.ProductCode.ToLower().Contains(term)
                                || x.SKU.ToLower().Contains(term) || x.tblProductType.TypeName.ToLower().Contains(term)));
                    }
                }
                else
                {
                    query = query.Where(x => x.Deleted == null);
                }

                if (!string.IsNullOrEmpty(orderBy))
                {
                    query = query.OrderBy(orderBy);
                }
                if (skip != null)
                {
                    query = query.Skip((int)skip);
                }
                if (take != null)
                {
                    query = query.Take((int)take);
                }

            }

            return query;
        }
        #endregion
    }
}
