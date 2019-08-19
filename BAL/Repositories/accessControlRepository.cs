using DAL.DBEntities;
using DAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Repositories
{
    public class accessControlRepository : BaseRepository
    {
        public accessControlRepository()
            : base()
        { }

        public accessControlRepository(DBEntities contextDB)
            : base(contextDB)
        {
            DBContext = contextDB;
        }

        public List<tblModule> getActiveModuleList()
        {
            return DBContext.tblModules.Include("tblPages.tblPermissions.tblPermissionActionJuncs").Include("tblPages.tblPermissions.tblRolePermissionJuncs")
                   .Where(x => x.IsActive == true && x.tblPages.Where(p => p.IsActive == true).Count() > 0).ToList();
        }


        public SysPermissionViewModel getSysPermissionsAndModule()
        {
            List<int> numList = new List<int>();
            numList.Add(1);

            var vm = numList.Select(x => new SysPermissionViewModel
            {
                tblModules = DBContext.ExclueAll().tblModules.AsNoTracking().Where(m => m.IsActive == true).ToList(),
                tblPages = DBContext.ExclueAll().tblPages.AsNoTracking().Where(p => p.IsActive == true && p.tblModule.IsActive == true).ToList(),
                tblPermissions = DBContext.ExclueAll().tblPermissions.AsNoTracking().Where(p => p.IsActive == true && p.tblPage.IsActive == true && p.tblPage.tblModule.IsActive == true).ToList(),
                tblPermissionActionsJunc = DBContext.ExclueAll().tblPermissionActionJuncs.AsNoTracking().Where(a => a.tblPermission.IsActive == true && a.tblPermission.tblPage.IsActive == true && a.tblPermission.tblPage.tblModule.IsActive == true).ToList(),
                tblRolePermissionsJunc = DBContext.ExclueAll().tblRolePermissionJuncs.AsNoTracking().Where(rp => rp.tblPermission.IsActive == true && rp.tblPermission.tblPage.IsActive == true && rp.tblPermission.tblPage.tblModule.IsActive == true).ToList(),
                tblActionEndPoints = DBContext.ExclueAll().tblActionEndPoints.AsNoTracking().Where(e => e.tblPermissionActionJunc.tblPermission.IsActive == true && e.tblPermissionActionJunc.tblPermission.tblPage.IsActive == true && e.tblPermissionActionJunc.tblPermission.tblPage.tblModule.IsActive == true).ToList()
            }).FirstOrDefault();

            return vm;
        }


        public tblUser authinticateUser(string username, string pass)
        {
            var user = DBContext.tblUsers.Include("tblUserRoles.tblRole").Where(x => x.UserLoginID.ToLower() == username.ToLower() && x.Password == pass && x.IsActive == true && x.Deleted == null).FirstOrDefault();
            if (user != null && user.Password != pass)
            {
                user = null;
            }
            return user;
        }


        public List<tblActionEndPoint> getAuthorizeEndPointList(int[] roleIdList, bool fromDB)
        {

            //var cacheSysEntities = CacheModel.SysPermissionsViewModel;
            var cacheSysEntities = fromDB ? CacheModel.SysPermissionsViewModelByDB : CacheModel.SysPermissionsViewModelByAPI;

            var rolePermissionJunc = cacheSysEntities.tblRolePermissionsJunc.Where(x => roleIdList.ToList().Contains(x.RoleID)).ToList();
            var permActionJunc = cacheSysEntities.tblPermissionActionsJunc.Where(a => rolePermissionJunc.Select(s => s.PermissionID).Distinct().ToList().Contains(a.PermissionID)).ToList();
            var apiEndPoints = cacheSysEntities.tblActionEndPoints.Where(e => permActionJunc.Select(a => a.PermissionActionID).Distinct().ToList().Contains(e.PermissionActionID)).ToList();

            return apiEndPoints;
        }

        public List<tblModule> getAuthorizePageList(List<int> roleIdList, int userId, bool fromDB)
        {

            var cacheSysEntities = fromDB ? CacheModel.SysPermissionsViewModelByDB : CacheModel.SysPermissionsViewModelByAPI;
            
            if(cacheSysEntities == null)
            {
                return new List<tblModule>();
            }

            var rolePermissionJunc = cacheSysEntities.tblRolePermissionsJunc.Where(x => roleIdList.Contains(x.RoleID)).ToList();
            var permissions = cacheSysEntities.tblPermissions.Where(x => rolePermissionJunc.Select(s => s.PermissionID).Distinct().ToList().Contains(x.PermissionID)).ToList();
            var permActionJunc = cacheSysEntities.tblPermissionActionsJunc.Where(a => permissions.Select(s => s.PermissionID).Distinct().ToList().Contains(a.PermissionID)).ToList();
            var apiEndPoints = cacheSysEntities.tblActionEndPoints.Where(e => permActionJunc.Select(a => a.PermissionActionID).Distinct().ToList().Contains(e.PermissionActionID)).ToList();
            var pageList = cacheSysEntities.tblPages.Where(p => permissions.Select(s => s.PageID).ToList().Contains(p.PageID)).ToList();
            var modules = cacheSysEntities.tblModules.Where(m => pageList.Select(s => s.ModuleID).ToList().Contains(m.ModuleID)).ToList();


            var result = (from module in modules
                      select new tblModule()
                      {
                          ModuleID = module.ModuleID,
                          ModuleName = module.ModuleName,
                          IsActive = module.IsActive,
                          ModuleOrder = module.ModuleOrder,
                          IsChargeable = module.IsChargeable,
                          ModuleIcon = module.ModuleIcon,
                          ParentID = module.ParentID,
                          Charges = module.Charges,
                          tblPages = pageList.Where(pg => pg.ModuleID == module.ModuleID).Select(x => new tblPage
                          {
                              PageID = x.PageID,
                              Controller = x.Controller,
                              ModuleID = x.ModuleID,
                              PageName = x.PageName,
                              IsActive = x.IsActive,
                              PageUrl = x.PageUrl,
                              PageIcon = x.PageIcon,
                              PageOrder = x.PageOrder,
                              ShowOnMenu = x.ShowOnMenu,
                              tblPermissions = permissions.Where(perm => perm.PageID == x.PageID).Select(pr => new tblPermission
                              {
                                  PermissionID = pr.PermissionID,
                                  PageID = pr.PageID,
                                  IsActive = pr.IsActive,
                                  Permission = pr.Permission,
                                  tblPermissionActionJuncs = permActionJunc.Where(acj => acj != null && acj.PermissionID == pr.PermissionID).Select(ac => new tblPermissionActionJunc
                                  {
                                      PermissionActionID = ac.PermissionActionID,
                                      Action = ac.Action,
                                      PermissionID = ac.PermissionID,
                                      tblActionEndPoints = apiEndPoints.Where(api => api != null &&  api.PermissionActionID == ac.PermissionActionID).Select(ep => new tblActionEndPoint
                                      {
                                          EndPointID = ep.EndPointID,
                                          APIController = ep.APIController,
                                          APIName = ep.APIName,
                                          PermissionActionID = ep.PermissionActionID
                                      }).ToList()
                                  }).ToList(),
                                  tblRolePermissionJuncs = rolePermissionJunc.Where(rpr => rpr != null && rpr.PermissionID == pr.PermissionID).Select(rp => new tblRolePermissionJunc
                                  {
                                      RolePermissionJuncID = rp.RolePermissionJuncID,
                                      PermissionID = rp.PermissionID,
                                      RoleID = rp.RoleID
                                  }).ToList()
                              }).ToList()
                          }).ToList(),
                      }).ToList();

            return result;
        }

        public static TokenDetailUserVM validateUserAccessToken(string token)
        {
            TokenDetailUserVM tok = new TokenDetailUserVM();
            tok.isValid = false;
            tok.isExpired = true;
            try
            {
                if (!string.IsNullOrEmpty(token))
                {
                    //  tok = validateApiAccessToken(token);
                    // tok.roleIds = tok.Data.Split(',').Select(x => Convert.ToInt32(x)).ToArray();

                    string plainToken = utilityRepository.Decrypt(token);
                    plainToken = plainToken.Replace("@", "/").Replace("+", " ").Replace("#",":");

                    string[] strArr = plainToken.Split('-');

                    tok.userId = Convert.ToInt32(strArr[0]);
                    tok.Data = strArr[1];
                    tok.roleIds = strArr[1].Split(',').Select(x => Convert.ToInt32(x)).ToArray();

                    DateTime createdTime = Convert.ToDateTime(strArr[2]);
                    int validHours = CacheModel.AppSettings.TokenValidHours;

                    if ((createdTime.AddHours(validHours) - DateTimeUTC.Now).TotalHours <= validHours)
                    {
                        tok.Expire = (createdTime.AddHours(validHours) - DateTimeUTC.Now).Minutes.ToString("0.00") + " Mins";
                        tok.isExpired = false;
                    }
                    tok.isValid = true;
                }
            }
            catch (Exception ex)
            {
                tok.isValid = false;
            }

            return tok;
        }

        public static TokenDetailAPIVM validateBearerToken(string token)
        {
            TokenDetailAPIVM vm = new TokenDetailAPIVM();
            vm.isValid = false;
            vm.isExpired = true;
            try
            {
                if (!string.IsNullOrEmpty(token) && token.ToLower().Contains("bearer "))
                {
                    string excludeBearer = token.Replace("Bearer", "").TrimStart();
                    string plainToken = utilityRepository.Decrypt(excludeBearer);

                    string[] strArr = plainToken.Split('-');
                    vm.AppID = strArr[0];
                    vm.Data = strArr[1];

                    DateTime createdTime = Convert.ToDateTime(strArr[2]);
                    int validHours = CacheModel.AppSettings.TokenValidHours;
                    if ((createdTime.AddHours(validHours) - DateTimeUTC.Now).TotalHours <= validHours)
                    {
                        vm.Expire = (createdTime.AddHours(validHours) - DateTimeUTC.Now).Minutes.ToString("0.00") + " Mins";
                        vm.isExpired = false;
                    }
                    vm.isValid = true;
                }
            }
            catch (Exception ex)
            {
                vm.isValid = false;
            }

            return vm;
        }

        public List<tblPermission> getPermissionName()
        {
            var permission = DBContext.tblPermissions.Where(x => x.IsActive == true).ToList();
            if (permission == null)
            {
                permission = null;
            }
            return permission;
        }


    }
}
