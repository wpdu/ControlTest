using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Global
{
    /// <summary>
    /// For View Mapping
    /// Eg:TB_V10 <-->MsgPage
    /// </summary>
    public class View
    {
        public const string ViewProject = "WinSFA";
        public const string ViewNamespace = "WinSFA.View";

        // Standard
        public const string MainPage = "MainPage";
        public const string LoginPage = "LoginPage";
        public const string HomePage = "HomePage";
        public const string MsgContentPage = "MsgContentPage";
        public const string MsgReplyPage = "MsgReplyPage";
        public const string SettingsPage = "SettingsPage";

        // Individual
        public const string UnileverPaneZone = "Unilever.UnileverPaneZone";
        public const string RichMediaHomePage = "Unilever.RichMediaHomePage";
        public const string AboutUsPage = "Unilever.AboutUsPage";
        public const string ChannelProjectPage = "Unilever.ChannelProjectPage";
        public const string CookMenuPage = "Unilever.CookMenuPage";
        public const string ProductShowPage = "Unilever.ProductShowPage";
        public const string ProServicePage = "Unilever.ProServicePage";
        public const string SalesPromotionPage = "Unilever.SalesPromotionPage";

        public const string RichMediaPage = "Unilever.RichMediaPage";

        static ResourceLoader ViewMapping { get; set; }
        static View()
        {
            ViewMapping = ResourceLoader.GetForCurrentView("View");
        }

        /// <summary>
        /// Find page by mapping key
        /// </summary>
        /// <param name="mappingId"> mapping key , eg:TB_V10 </param>
        /// <returns> pageName,eg:MsgPage </returns>
        public static string FindPage(string mappingId)
        {
            var pageName = ViewMapping.GetString(mappingId.Trim());
            if (!string.IsNullOrEmpty(pageName))
            {
                return pageName;
            }

            return null;
        }
    }
}
