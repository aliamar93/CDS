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
    public class userRepository : BaseRepository
    {
        public userRepository()
            : base()
        { }

        public userRepository(DBEntities contextDB)
            : base(contextDB)
        {
            DBContext = contextDB;
        }
        
        public tblUser getUserById(int UserID, string[] includeList)
        {
            var query = DBContext.ExclueAll().tblUsers.AsQueryable();
            
            if(includeList != null && includeList.Count() > 0)
            {
                foreach(string tbl in includeList)
                {
                    query = query.Include(tbl);
                }
            }
            return query.Where(x => x.UserID == UserID && x.Deleted == null).FirstOrDefault();
        }

        public tblUser getUserByUserId(string UserName)
        {
            return DBContext.ExclueAll().tblUsers.AsNoTracking().Where(x => x.UserLoginID.ToLower() == UserName.ToLower() && x.Deleted == null).FirstOrDefault();
        }
        public int TotalUserCount { get {return DBContext.tblUsers.Where(x => x.Deleted == null).Count(); } }

        public List<tblUser> getUserList(string[] includeList, int? userID, int? skip, int? take, string searchTerm, string orderBy)
        {

            var query = DBContext.ExclueAll().tblUsers.AsNoTracking().AsQueryable();
            if (includeList != null && includeList.Count() > 0)
            {
                foreach (string tbl in includeList)
                {
                    query = query.Include(tbl);
                }
            }
            if (userID != null)
            {
                query = query.Where(x => x.UserID == (int)userID && x.Deleted == null);
            }
            else
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    var searchList = searchTerm.Split(',').Select(x => x.ToLower()).ToList();
                    foreach (string term in searchList)
                    {
                        query = query.Where(x => x.Deleted == null && (x.UserID.ToString().Contains(term) || x.FirstName.ToLower().Contains(term) || x.LastName.ToLower().Contains(term)
                                || x.UserLoginID.ToLower().Contains(term) || x.Email.ToLower().Contains(term)));
                    }
                }

                if (!string.IsNullOrEmpty(orderBy))
                {
                 query =   query.OrderBy(orderBy);
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

            return query.ToList();
        }


        public tblUser createUser(tblUser user, int createdBy)
        {
            user.CreateBy = createdBy;
            user.CreateDate = DateTimeUTC.Now;
            DBContext.tblUsers.Add(user);

            return user;
        }

        
        public tblUser updateUser(tblUser user, int updatedBy)
        {
            user.UpdateBy = updatedBy;
            user.UpdateDate = DateTimeUTC.Now;
            DBContext.tblUsers.Attach(user);
            DBContext.UpdateExcept<tblUser>(user, x => x.CreateBy, x => x.CreateDate, x => x.UserLoginID, x => x.UserType);
            return user;
        }

        public List<tblPage> getPageList(int[] roleids)
        {
            var roleWisePermissions = DBContext.tblRolePermissionJuncs
                                    .Where(x => roleids.Contains(x.RoleID))
                                    .Select(x => x.tblPermission.PermissionID).ToList();

            return DBContext.tblPermissionActionJuncs.Where(x => roleWisePermissions.Contains(x.PermissionID) && x.IsLandingAction == 1)
                .Select(x => x.tblPermission.tblPage).ToList();
        }

        public bool AssignRolesAndPagesToUser(tblUser user, string[] roleIds, int LandingPageId)
        {
            try
            {

                DeleteUserRoles(user.UserID);

                for (int i = 0; i < roleIds.Length; i++)
                {
                    DBContext.tblUserRoles.Add(new tblUserRole
                    {
                        RoleID = Convert.ToInt32(roleIds[i]),
                        UserID = user.UserID
                    });
                }
                InsertLandingPage(user.UserID, LandingPageId);

                return true;

            }
            catch (Exception ex)
            {
                //logRepository.ErrorLog(ex,"User","Assign User Roles")
            }
            return false;
        }

        private bool InsertLandingPage(decimal userId, int pageId)
        {
            var user = DBContext.tblUsers.Where(x => x.UserID == userId).FirstOrDefault();
            user.LandingPage = DBContext.tblPages.Where(x => x.PageID == pageId).Select(x => x.PageUrl).FirstOrDefault();
            return true;
        }
        private bool DeleteUserRoles(decimal userId)
        {
            var Existingroles = DBContext.tblUserRoles.Where(x => x.UserID == userId).ToList();
            DBContext.tblUserRoles.RemoveRange(Existingroles);
            //DBContext.SaveChanges();
            return true;
        }

    }


}
