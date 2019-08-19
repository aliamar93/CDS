using DAL.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BAL.Repositories
{
   public class roleRepository : BaseRepository
    {
        public roleRepository()
            : base()
        { }

        public roleRepository(DBEntities contextDB)
            : base(contextDB)
        {
            DBContext = contextDB;
        }

        public List<tblRole> getRoleList(string[] includeList, int? roleid)
        {
            //return DBContext.ExclueAll().tblRoles.Where(x => x.Deleted == null).ToList();
            return getQueryRoleList(includeList, roleid).ToList();
        }

        public async Task<List<tblRole>> getAsyncRoleList(string[] includeList, int? roleid)
        {
            return await getQueryRoleList(includeList, roleid).ToListAsync();
        }

        public tblRole createRole(tblRole role, int createdBy)
        {
            role.Created = DateTimeUTC.Now;
            role.CreatedBy = createdBy;
            DBContext.tblRoles.Add(role);
            return role;
        }

        private IQueryable<tblRole> getQueryRoleList(string[] includeList, int? roleid)
        {
            var qry = DBContext.ExclueAll().tblRoles.AsNoTracking().AsQueryable();
            if (includeList != null && includeList.Where(x => !string.IsNullOrEmpty(x)).Count() > 0)
            {
                foreach (string tbl in includeList)
                {
                    qry = qry.Include(tbl);
                }
            }
            if (roleid != null && roleid > 0)
            {
                qry = qry.Where(x => x.RoleID == roleid);
            }
            return qry.Where(x => x.Deleted == null);
        }
    }
}
