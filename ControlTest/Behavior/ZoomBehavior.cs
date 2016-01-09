using Microsoft.Xaml.Interactivity;
using SB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ControlTest.Behavior
{
    public class ZoomBehavior : DependencyObject, IBehavior
    {


        private DependencyObject _associatedObject;
        public DependencyObject AssociatedObject
        {
            get
            {
                return _associatedObject;
            }
        }

        public void Attach(DependencyObject associatedObject)
        {
            _associatedObject = associatedObject;
            if (_associatedObject is FrameworkElement)
            {
                (_associatedObject as Image).ManipulationDelta += ZoomBehavior_ManipulationDelta;
                (_associatedObject as Image).DoubleTapped += ZoomBehavior_DoubleTapped;
            }
        }

        private void ZoomBehavior_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            Image img = sender as Image;
            //ScaleTransform scaleTF = img.RenderTransform as ScaleTransform;
            //if (scaleTF == null)
            //{
            //    scaleTF = new ScaleTransform();
            //}
            //if (scaleTF.ScaleX != 1)
            //{
            //    scaleTF.ScaleY = scaleTF.ScaleX = 1;
            //}
            //else
            //{
            //    scaleTF.ScaleY = scaleTF.ScaleX = 2;
            //}

            CompositeTransform ct = img.RenderTransform as CompositeTransform;
            if (ct.SkewX != 0 || ct.SkewX != 1)
            {
                ScaleAnimation.ScaleTo(img, 1, 1, TimeSpan.FromSeconds(0.3), null);
            }
            else
            {
                ScaleAnimation.ScaleTo(img, 2, 2, TimeSpan.FromSeconds(0.3), null);
            }
        }

        private void ZoomBehavior_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            Image img = sender as Image;
            ScaleTransform scaleTF = img.RenderTransform as ScaleTransform;
            if (scaleTF == null)
            {
                scaleTF = new ScaleTransform();
            }
            float value = e.Delta.Scale / 2;
            scaleTF.ScaleY = scaleTF.ScaleX = scaleTF.ScaleX + value;
        }

        public void Detach()
        {
            (_associatedObject as FrameworkElement).ManipulationDelta -= ZoomBehavior_ManipulationDelta;
        }
    }
}
