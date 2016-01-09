using MSDNFinder.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MSDNFinder.Common.Selectors
{
    public class ListTemplateSelector : DataTemplateSelector
    {
        public DataTemplate NormalDataTemplate { get; set; }

        public DataTemplate ListDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            ClientModel cm = item as ClientModel;
            if (cm != null && cm.HaveChild && cm.SubExpanding)
            {
                return ListDataTemplate;
            }
            return NormalDataTemplate;
        }
        


    }
}
