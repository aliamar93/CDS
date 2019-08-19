using DAL.DBEntities;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Repositories
{
    public class permissionRepository : BaseRepository
    {
        public permissionRepository()
            : base()
        { }

        public permissionRepository(DBEntities contextDB)
            : base(contextDB)
        {
            DBContext = contextDB;
        }

        public AllPermissionResponseModel getAllPermissionList(int roleId)
        {
            AllPermissionResponseModel lst = new AllPermissionResponseModel();
            //lst.tblModule = DBContext.tblModules.Where(x => x.IsActive == true).ToList();
            //lst.tblPage = DBContext.tblPages.Where(x => x.IsActive == true).ToList();
            //lst.tblPermission = DBContext.tblPermissions.Where(x => x.IsActive == true).ToList();
            //lst.tblRolePermissionJunc = DBContext.tblRolePermissionJuncs.Where(x => x.RoleID == roleId).ToList();            
       

            List<int> dummyList = new List<int>();
            dummyList.Add(1);

            var res = (from n in dummyList
                       select new
                       {
                           num = n,
                           modules = DBContext.ExclueAll().tblModules.Where(x => x.IsActive == true).ToList(),
                           pages = DBContext.ExclueAll().tblPages.Where(x => x.IsActive == true).ToList(),
                           permisssions = DBContext.ExclueAll().tblPermissions.Where(x => x.IsActive == true).ToList(),
                           rolePermissions = DBContext.ExclueAll().tblRolePermissionJuncs.Where(x => x.RoleID == roleId).ToList(),
                           role = DBContext.ExclueAll().tblRoles.Where(r => r.RoleID == roleId).FirstOrDefault()
                       }).FirstOrDefault();

            lst.tblModule = res.modules.ToList();
            lst.tblPage = res.pages.ToList();
            lst.tblPermission = res.permisssions.ToList();
            lst.tblRolePermissionJunc = res.rolePermissions.ToList();
            lst.tblRole = res.role;    
                               
            return lst;
        }




        public tblRolePermissionJunc createPermission(tblRolePermissionJunc permission)
        {
            DBContext.tblRolePermissionJuncs.Add(permission);
            return permission;
        }

        public bool removeAllPermissionByRoleId(int roleId)
        {
            bool isDeleted = false;
            try
            {
                DBContext.tblRolePermissionJuncs.RemoveRange(DBContext.tblRolePermissionJuncs.Where(x => x.RoleID == roleId).ToList());
                DBContext.SaveChanges();
                isDeleted = true;
            }
            catch (Exception ex)
            {
                isDeleted = false;
            }

            return isDeleted;
        }
    }
}
