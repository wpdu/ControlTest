using System;
using Windows.ApplicationModel.Resources;

namespace Global
{
    public static class Config
    {
        //Const String
        public const string AppName = "WinSFA";

        //Local Storage File
        public const string DbName = "MSDNFinder.db";

        //Local Storage Folder
        public const string DownloadFolder = "download";

        //Installation Folder
        public const string ImgFolder = "Resource/image/";

        static Config()
        {
        }
        
        private static string[] _DocExtension = { ".pdf", ".doc", ".xls", ".ppt" };
        public static string[] DocExtension
        {
            get { return _DocExtension; }
        }

        public static ResourceLoader _currentResourceLoader;

        public static ResourceLoader CurrentResourceLoader
        {
            get
            {
                if (_currentResourceLoader == null)
                {
                    _currentResourceLoader = ResourceLoader.GetForCurrentView("Config");
                }
                return _currentResourceLoader;
            }
        }

        public static string GetString(string resourceKey, System.Globalization.CultureInfo resourceCulture = null)
        {
            return CurrentResourceLoader.GetString(resourceKey);
        }

        //Config Properties From Resource/ConfigMapping.resw

        /// <summary>
        ///   Looks up a localized string similar to  0.
        /// </summary>
        public static string ABOUT_RIGHT_BUTTOB_GONE
        {
            get
            {
                return GetString("ABOUT_RIGHT_BUTTOB_GONE");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  .
        /// </summary>
        public static string AKU
        {
            get
            {
                return GetString("AKU");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  0.
        /// </summary>
        public static string BOTTOM_MENU
        {
            get
            {
                return GetString("BOTTOM_MENU");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  http://inside.winchannel.net:8820/.
        /// </summary>
        public static string CONFIG_PARAM_URL
        {
            get
            {
                return GetString("CONFIG_PARAM_URL");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  0.
        /// </summary>
        public static string CONTACTS_CLCT
        {
            get
            {
                return GetString("CONTACTS_CLCT");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  1.
        /// </summary>
        public static string ENABLE_PLATFORM_RPT
        {
            get
            {
                return GetString("ENABLE_PLATFORM_RPT");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  0.
        /// </summary>
        public static string ENABLE_PRE_UPGRADE
        {
            get
            {
                return GetString("ENABLE_PRE_UPGRADE");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  0.
        /// </summary>
        public static string ENABLE_WAKELOCK
        {
            get
            {
                return GetString("ENABLE_WAKELOCK");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  .
        /// </summary>
        public static string ENABLE_WHITE_LIST
        {
            get
            {
                return GetString("ENABLE_WHITE_LIST");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  0;.
        /// </summary>
        public static string GET_PARAM_IN_LOGIN_DATA
        {
            get
            {
                return GetString("GET_PARAM_IN_LOGIN_DATA");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  0.
        /// </summary>
        public static string IMAGE_SCALE
        {
            get
            {
                return GetString("IMAGE_SCALE");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  1.
        /// </summary>
        public static string IS_LOGIN_PASSWORD
        {
            get
            {
                return GetString("IS_LOGIN_PASSWORD");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  .
        /// </summary>
        public static string IS_LOGIN_WELCOME
        {
            get
            {
                return GetString("IS_LOGIN_WELCOME");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  1.
        /// </summary>
        public static string ISINFLATE
        {
            get
            {
                return GetString("ISINFLATE");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  .
        /// </summary>
        public static string LOGIN_CHECK_GPS
        {
            get
            {
                return GetString("LOGIN_CHECK_GPS");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  http://go.winmdm.com/2013/sfaa_winchannel.jpg.
        /// </summary>
        public static string NAVI_ADDRESS
        {
            get
            {
                return GetString("NAVI_ADDRESS");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  0.
        /// </summary>
        public static string OPEN_TC_EVERY_TIME
        {
            get
            {
                return GetString("OPEN_TC_EVERY_TIME");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  0.
        /// </summary>
        public static string PASSWORD_ENCRYPT
        {
            get
            {
                return GetString("PASSWORD_ENCRYPT");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  .
        /// </summary>
        public static string PRODUCT
        {
            get
            {
                return GetString("PRODUCT");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  1.
        /// </summary>
        public static string REMENBER_PASSWORD
        {
            get
            {
                return GetString("REMENBER_PASSWORD");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  0.
        /// </summary>
        public static string STEP_PARSE
        {
            get
            {
                return GetString("STEP_PARSE");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        public static string SVN
        {
            get
            {
                return GetString("SVN");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  30.
        /// </summary>
        public static string TIME_OUT
        {
            get
            {
                return GetString("TIME_OUT");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  4.20.
        /// </summary>
        public static string VERSION
        {
            get
            {
                return GetString("VERSION");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  /mobile/post.do?methodonChangePass.
        /// </summary>
        public static string WEB_ADDR_CHANGPASS
        {
            get
            {
                return GetString("WEB_ADDR_CHANGPASS");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  /mobile/get.do?methodgetMobileRootConfig.
        /// </summary>
        public static string WEB_ADDR_CONFIG_PARAM
        {
            get
            {
                return GetString("WEB_ADDR_CONFIG_PARAM");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  /mobile/get.do?methoddataInfo.
        /// </summary>
        public static string WEB_ADDR_DATAINFO
        {
            get
            {
                return GetString("WEB_ADDR_DATAINFO");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  /mobile/get.do?methodlogin.
        /// </summary>
        public static string WEB_ADDR_LOGIN
        {
            get
            {
                return GetString("WEB_ADDR_LOGIN");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  /mobile/post.do?methodsave.
        /// </summary>
        public static string WEB_ADDR_UPLOAD_COMMON_DATA
        {
            get
            {
                return GetString("WEB_ADDR_UPLOAD_COMMON_DATA");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  /mobile/post.do?methodsaveImg.
        /// </summary>
        public static string WEB_ADDR_UPLOAD_IMG
        {
            get
            {
                return GetString("WEB_ADDR_UPLOAD_IMG");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  /mobile/post.do?methodsaveVideo.
        /// </summary>
        public static string WEB_ADDR_UPLOAD_VIDEO
        {
            get
            {
                return GetString("WEB_ADDR_UPLOAD_VIDEO");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  .
        /// </summary>
        public static string WECHAT_SHARE_ID
        {
            get
            {
                return GetString("WECHAT_SHARE_ID");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  400-650-8622.
        /// </summary>
        public static string WINCHANNEL_HOTLINE
        {
            get
            {
                return GetString("WINCHANNEL_HOTLINE");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to  1.
        /// </summary>
        public static string WINCHANNEL_PHONE_ISCALL
        {
            get
            {
                return GetString("WINCHANNEL_PHONE_ISCALL");
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        public static string WORK_TIME
        {
            get
            {
                return GetString("WORK_TIME");
            }
        }
    }

    public class Product
    {
        public const string Standard = "Standard";
        public const string Unilever = "Unilever";

        private static readonly Product _current = new Product();

        public static Product Current
        {
            get
            {
                return _current;
            }
        }

        public string ProductName

        {
            get
            {
                return Config.PRODUCT?.Trim() ?? "";
            }
        }

        public bool IsProduct(string productName)
        {
            return ProductName.Equals(productName, StringComparison.CurrentCultureIgnoreCase);
        }
    }

    public static class ServerSettings
    {
        public const string ID = "id";
        public const string PID = "pid";
        public const string PK = "pk";
        public const string FK = "fk";
        public const string P = "p";
        public const string S = "s";
        public const string PRODS = "prods";
        /**
         * 所有门店，取消计划内外节点.
         */
        public const string STORES = "stores";
        public const string OUT_EMP_PLAN = "outempplan";
        public const string IN_EMP_PLAN = "inempplan1";
        /**
         * 门店属性中isPlaned值，开发环境-箭牌，根据城市搜索门店中有计划内门店.
         */
        public const string ISPLAN = "isPlaned";
        public const string HOS = "hos";
        public const string ICE_BOX_CHK = "iceboxChk";
        public const string FUNCS2 = "funcs2";
        public const string MSGS = "msgs";
        public const string MSG = "msg";
        public const string STORE_INFO = "storeInfo";
        public const string STORE_INFO_KEY = "storeInfoKey";
        public const string STORE_INFO_VALUE = "storeInfoValue";
        public const string IN_STORE_PROD = "instoreprod";
        public const string DICTS = "dicts";
        public const string TIMEUPDATE = "timeUpdate";
        public const string NO_REPLY = "noreply";
        public const string SHIP_TO = "shipTo";
        public const string ACVT = "acvt";
        public const string ACVT_SHOW = "acvtshow";
        public const string ACVT_QST = "qst";
        public const string ACVT_QST_OPT = "opt";
        public const string WHITE_LIST = "whiteList";
        public const string ACVT_PROD_RELATION = "acvtprodrelation";
        public const string KPI = "kpi";
        public const string BASE_EMPLOYEE = "baseemployee";

        public const string SPE_COFCOMCM_ = "e3sbwohs";
        public const string SUBEMP_OUTSTORE = "subempoutstore";
        public const string SUBEMP_INSTORE = "subempinstore";
        public const string ALL_PLANSTORE_ONTIME = "allplanstoreontime";
        public const string ALL_PLAN_STORE_OTHER_INFO_ONTIME = "allplanstoreotherinfoontime";
        public const string SPESTOREINFO = "spestoreinfo";
        public const string UPDATE_STORE_INFO = "updateStoreInfo";
        public const string EMP_INFO = "empInfo";
        public const string STORE_DICT_DIS = "storedictdis";
        public const string STORE_PROD_DIS = "storeproddis";
        public const string STORE_ACVT_DIS = "storeacvtdis";
        public const string ACVTDIS = "acvtdis";
        public const string ACVTDIS_BYADD = "acvtdisByAdd";
        /**
         * 辉瑞用来控制门店下指定菜单是否必须拍照.
         */
        public const string STORE_IS_CYCLE_CAMERA = "isCycleCamera";
        public const string PIM_MEETING_ACVTS = "PIMMeeting";
        public const string MENU_ACVT_LIST_FLAG = "menuAcvtListFlag";
        public const string STOREINFO_UPDATE_TIME = "updateTime";
        public const string othersAttendance = "othersAttendance";
        public const string BUSIACVT = "busiAcvt";
        public const string SPBAINFO = "spbaInfo";
        public const string BASESCHEDULE_BRAND = "scheduleBrand";
        public const string BASESCHEDULE_STORE = "scheduleStore";
        public const string PAYDISPLAY = "payDisplay";
        public const string PAYDISPLAY_INCLUDE_IN = "payDisplay_in";
        public const string BRAND_TREE = "brandTree";
        public const string STORE_ASSET = "storeAsset";
        public const string STORE_ASSET_ACVT = "storeAssetAcvt";
        public const string SUGEMPLIST = "sugemplist";
        public const string DIHON_STORE_POA = "dihonStorePoa";
        public const string DIHON_POA = "dihonPoa";
        public const string DIHON_POA_STORE = "dihonPoaStore";
        public const string DIHON_POA_TASK_STORE = "dihonPoaTaskStore";
        public const string DIHON_POA_TASK_RES = "dihonPoaTaskRes";
        public const string VISITED_MENU = "visitedMenu";
        public const string CITY_NAMES = "citynames";
        public const string MOBILE_PICTURE = "mobilePicture";
        public const string MSGS_QUERY = "msgsQuery";
        public const string CACHE_DATA_VERSION = "cacheDataVersion";
        public const string IN_PLAN_STORE_PARENT = "inplanstoreparent";
        public const string HOME_PLAN = "homeplan";
        public const string HOME_PLAN_ITEM = "homeplanitem";
        public const string CACHE_GLOBAL_DATA_VERSION = "cacheGlobalDataVersion";
        /**
         * .
         */
        public const string APP_URL = "appUrl";
        /**
         * WRIGLEY-33 箭牌表格间填报校验.
         */
        public const string PRODUCT_VALIDATE = "productValidate";
        /**
         * 拜访计划.
         */
        public const string VISIT_PLAN = "storeacvtdis:visitplan";
        /**
         * 调查问卷门店.
         */
        public const string STORE_ACVT = "store_acvt";

        /**
         * 后台刷新缓存时间.
         */
        public const string SERVER_REFRESH_CACHE_TIME = "timems";

        public const string SHARE_KEY_EMP_ID = "empid";
        public const string SHARE_KEY_EMP_NAME = "empName";
        public const string SHARE_KEY_BIZ_DATE = "bizDate";
        public const string SHARE_KEY_PRODSPEC = "prodspec";
        public const string SHARE_KEY_PRODSPECDIS = "prodspecdis";
        public const string SHARE_KEY_SERVER_URL = "serverURL";
        public const string SHARE_KEY_GPS_ACCURACY = "GPSAccuracy";
        public const string SHARE_KEY_SERVERREQUIRE = "serverRequire";
        public const string SHARE_KEY_MOBILECHECKTIME = "mobileCheckTime";
        public const string SHARE_KEY_FORCE_EXIT_TIME = "forceExitTime";
        public const string SHARE_NO_REPLY = "noreply";
        public const string SHARE_KEY_DAYS = "days";
        public const string SHARE_KEY_MOBILEHOMEPAGE = "mobileHomePage";
        /**
         * SHARE_KEY_HOMEPAGE_READING_TIME.
         */
        public const string SHARE_KEY_HOMEPAGE_READING_TIME = "readingTime";
        public const string SHARE_KEY_STORE_PRODDISTYPE = "storeProdDistType";
        public const string SHARE_KEY_LOGIN_FILE_PATH = "sharekeyloginfilepath";
        public const string SHARE_KEY_PSW_VALIDDAY_MSG = "pswValidDayMsg";
        public const string SHARE_KEY_PROJECT_ID = "projectId";
        public const string SHARE_KEY_HOME_PLAN_ZIP = "homeplanzip";
        public const string SHARE_KEY_DOWNLOAD_ZIP = "downloadZip";
        /**
         * .
         */
        public const string SHARE_KEY_FORCE_EXIT_TIME_MINUTE = "forceExitTimeMinute";
    }
}