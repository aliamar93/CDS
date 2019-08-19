using DAL.DBEntities;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Repositories
{
    public class AppSettingViewModel
    {
        public string FCMKey { get; set; }
        public string KeySec { get; set; }
        public bool EnableErrorLog { get; set; }
        public bool EnableEventLog { get; set; }
        public bool EnableAuthActivityLog { get; set; }
        public bool EnableUnAuthActivityLog { get; set; }
        public bool UseCookies { get; set; }
        public string CODServiceAppID { get; set; }
        public string CODServiceAppSecret { get; set; }
        public DateTime objectInitiated { get; set; }
        public int TokenValidHours { get; set; }
    }

    public static class CacheModel
    {
        private static AppSettingViewModel _appSetting;
        //private static List<tblModule> _moduleList;
        private static SysPermissionViewModel _sysPermissionsViewModel;
        private static string _codServiceAccessToken;

        public static AppSettingViewModel AppSettings
        {
            get
            {

                if (_appSetting == null || DateTimeUTC.Now.Subtract(_appSetting.objectInitiated).TotalMinutes >= 10)
                {
                    try
                    {
                        _appSetting = Newtonsoft.Json.JsonConvert.DeserializeObject<AppSettingViewModel>(utilityRepository.ReadFile(System.Web.Hosting.HostingEnvironment.MapPath(Constants.ConfigFileBasePath + "COD_AppSettings.txt")));
                        _appSetting.objectInitiated = DateTimeUTC.Now;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return _appSetting;
            }
            set
            {
                _appSetting = value;
            }
        }

        //public static List<tblModule> moduleList
        //{
        //    get
        //    {

        //        if (_moduleList == null)
        //        {
        //            accessControlRepository accessRepo = new accessControlRepository();
        //            _moduleList = accessRepo.getActiveModuleList();
        //        }
        //        return _moduleList;
        //    }
        //    set
        //    {
        //        _moduleList = value;
        //    }
        //}

        public static SysPermissionViewModel SysPermissionsViewModelByDB
        {
            get
            {

                if (_sysPermissionsViewModel == null)
                {
                    accessControlRepository accessRepo = new accessControlRepository();
                    _sysPermissionsViewModel = accessRepo.getSysPermissionsAndModule();
                }
                return _sysPermissionsViewModel;
            }
            set
            {
                _sysPermissionsViewModel = value;
            }
        }

        public static SysPermissionViewModel SysPermissionsViewModelByAPI
        {
            get
            {

                if (_sysPermissionsViewModel == null)
                {
                    string url = utilityRepository.getBaseUrl() + "permission/getsyspermissions";
                    var model = utilityRepository.getResponse<sysPermissionReponseModel>(url, string.Empty, false);
                    if (model.Code == (int)ResponseCode.Success)
                    {
                        _sysPermissionsViewModel = model.sysPermissions;
                    }
                }
                return _sysPermissionsViewModel;
            }
            set
            {
                _sysPermissionsViewModel = value;
            }
        }

        public static string codServiceAccessToken
        {
            get
            {
                if (string.IsNullOrEmpty(_codServiceAccessToken))
                {
                    TokenRequestVM vm = new TokenRequestVM();
                    vm.AppID = AppSettings.CODServiceAppID;
                    vm.AppSecret = AppSettings.CODServiceAppSecret;
                    string url = utilityRepository.getBaseUrl() + "token/getToken";
                    var response = utilityRepository.getResponse<TokenResponseVM>(url, Newtonsoft.Json.JsonConvert.SerializeObject(vm), true, null);
                    if(response.Code == (int)ResponseCode.Success)
                    {
                        _codServiceAccessToken = response.BearerToken;
                    }
                    else
                    {
                        _codServiceAccessToken = string.Empty;
                    } 
                }
                return _codServiceAccessToken;
            }
            set
            {
                _codServiceAccessToken = value;
            }
        }
    }

  
}
