
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

using Microsoft.Practices.ServiceLocation;
using MSDNFinder.Manager;
using MSDNFinder.Model.DataJson;
using MSDNFinder.ViewModel;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace MSDNFinder.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : BasePage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void AutoSuggestBox_TextChanged(Windows.UI.Xaml.Controls.AutoSuggestBox sender, Windows.UI.Xaml.Controls.AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var matchingItems = DataManager.GetData(sender.Text);
                if (matchingItems.Count > 0)
                {
                    (sender as AutoSuggestBox).ItemsSource = matchingItems.OrderByDescending(i => i.Title.StartsWith(sender.Text,
                        System.StringComparison.CurrentCultureIgnoreCase)).ThenBy(i => i.Title);
                }
                else
                {
                    (sender as AutoSuggestBox).ItemsSource = new string[] { "No results found" };
                }
            }
        }

        private void AutoSuggestBox_QuerySubmitted(Windows.UI.Xaml.Controls.AutoSuggestBox sender, Windows.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null && args.ChosenSuggestion is ServerModel)
            {
                ServiceLocator.Current.GetInstance<MainPageViewModel>().GoSugges(args.ChosenSuggestion as ServerModel);
            }
            else if (!string.IsNullOrEmpty(args.QueryText))
            {
                ServiceLocator.Current.GetInstance<MainPageViewModel>().ResetLayout();
            }

        }

        private void wb_LoadCompleted(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            ServiceLocator.Current.GetInstance<MainPageViewModel>().IsProgressActive = false;
        }
    }
}
