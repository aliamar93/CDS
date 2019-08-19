using BAL.Repositories;
using DAL.DBEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COD.Models
{
    //public class AppSettingViewModel
    //{
    //    public string FCMKey { get; set; }
    //    public string KeySec { get; set; }
    //    public bool EnableErrorLog { get; set; }
    //    public bool EnableEventLog { get; set; }
    //    public bool EnableAuthActivityLog { get; set; }
    //    public bool EnableUnAuthActivityLog { get; set; }
    //    public bool UseCookies { get; set; }
    //    public DateTime objectInitiated { get; set; }
    //}

    //public static class CacheModel
    //{
    //    private static AppSettingViewModel _appSetting;
    //    private static List<tblModule> _moduleList;
    //    public static AppSettingViewModel AppSettings
    //    {
    //        get
    //        {

    //            if (_appSetting == null || DateTime.Now.Subtract(_appSetting.objectInitiated).TotalMinutes >= 10)
    //            {
    //                try
    //                {
    //                    _appSetting = Newtonsoft.Json.JsonConvert.DeserializeObject<AppSettingViewModel>(utilityRepository.ReadFile(System.Web.Hosting.HostingEnvironment.MapPath(Constants.ConfigFileBasePath + "COD_AppSettings.txt")));
    //                    _appSetting.objectInitiated = DateTime.Now;
    //                }
    //                catch (Exception ex)
    //                {

    //                }
    //            }
    //            return _appSetting;
    //        }
    //        set
    //        {
    //            _appSetting = value;
    //        }
    //    }

    //    public static List<tblModule> moduleList
    //    {
    //        get
    //        {

    //            if (_moduleList == null)
    //            {
    //                accessControlRepository accessRepo = new accessControlRepository();
    //                _moduleList = accessRepo.getActiveModuleList();
    //            }
    //            return _moduleList;
    //        }
    //        set
    //        {
    //            _moduleList = value;
    //        }
    //    }
        
    //}
}