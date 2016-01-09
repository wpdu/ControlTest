using Microsoft.Xaml.Interactivity;
using SB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ControlTest.Behavior
{
    public class FlyBehavior : DependencyObject, IBehavior
    {
        public string ElementName { get; set; }
        
        private DependencyObject _associatedObj;
        public DependencyObject AssociatedObject
        {
            get
            {
                return _associatedObj;
            }
        }

        public void Attach(DependencyObject associatedObject)
        {
            _associatedObj = associatedObject;

            if(_associatedObj is FrameworkElement)
            {
                (_associatedObj as FrameworkElement).Tapped += FlyButtonBehavior_Tapped;
            }
        }

        private void FlyButtonBehavior_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement)
            {
                FrameworkElement element = sender as FrameworkElement;
                FrameworkElement flyImg = element.FindName(ElementName) as FrameworkElement;

                Point pTouchInElement = e.GetPosition(element);
                Point pTouchInFrame = e.GetPosition(Window.Current.Content as Frame);

                Point pElementCenter = new Point();
                pElementCenter.X = pTouchInFrame.X - pTouchInElement.X + element.ActualWidth / 2 - flyImg.Height / 2;
                pElementCenter.Y = pTouchInFrame.Y - pTouchInElement.Y + element.ActualHeight / 2 - flyImg.Width/2;

                Point pElementTo = new Point(Window.Current.Bounds.Width, (Window.Current.Bounds.Height - flyImg.ActualHeight) / 2);

                flyImg.Visibility = Visibility.Visible;

                MoveAnimation.MoveFromTo(flyImg, pElementCenter.X, pElementCenter.Y, pElementTo.X, pElementTo.Y, TimeSpan.FromSeconds(0.8), (ele) => { flyImg.Visibility = Visibility.Collapsed; });
            }
        }

        public void Detach()
        {
            (_associatedObj as Button).Tapped -= FlyButtonBehavior_Tapped;
        }
    }
}
