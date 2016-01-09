using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ControlTest.SB
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DoubleAnimationTest : Page
    {
        public DoubleAnimationTest()
        {
            this.InitializeComponent();
        }

        private void border_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //Storyboard1.Begin();
            //CreateDoubleSB(sender as DependencyObject, "FrameworkElement.Height", 1, 400, 0, EasingMode.EaseInOut).Begin();
            CreateDoubleSB(sender as DependencyObject, "FrameworkElement.Height", 1, 400, 0, EasingMode.EaseInOut).Begin();
        }

        public static Storyboard CreateDoubleSB(DependencyObject dpnObj, string property, double secondTime, Double to, EasingMode em)
        {
            DoubleAnimation ca = new DoubleAnimation();
            ca.EnableDependentAnimation = true;
            ca.Duration = TimeSpan.FromSeconds(secondTime);
            ca.To = to;

            CircleEase ce = new CircleEase();
            ce.EasingMode = em;
            ca.EasingFunction = ce;

            Storyboard.SetTarget(ca, dpnObj);
            Storyboard.SetTargetProperty(ca, property);
            Storyboard sb = new Storyboard();

            sb.Children.Add(ca);
            return sb;
        }

        public static Storyboard CreateDoubleSB(DependencyObject dpnObj, string property, double secondTime, Double from, Double to, EasingMode em)
        {
        //<Storyboard x:Name="Storyboard1">
        //    <DoubleAnimationUsingKeyFrames EnableDependentAnimation="True" Storyboard.TargetProperty="(FrameworkElement.Height)" Storyboard.TargetName="border">
        //        <EasingDoubleKeyFrame KeyTime="0:0:1" Value="0"/>
        //    </DoubleAnimationUsingKeyFrames>
        //</Storyboard>
            DoubleAnimationUsingKeyFrames daKeyFrame = new DoubleAnimationUsingKeyFrames();
            EasingDoubleKeyFrame edKeyFrame = new EasingDoubleKeyFrame();
            edKeyFrame.KeyTime = TimeSpan.FromSeconds(secondTime);
            edKeyFrame.Value = to;
            CircleEase ce = new CircleEase();
            ce.EasingMode = em;
            edKeyFrame.EasingFunction = ce;

            daKeyFrame.KeyFrames.Add(edKeyFrame);
            daKeyFrame.EnableDependentAnimation = true;

            Storyboard.SetTarget(daKeyFrame, dpnObj);
            Storyboard.SetTargetProperty(daKeyFrame, property);
            Storyboard sb = new Storyboard();

            sb.Children.Add(daKeyFrame);
            return sb;
        }


    }
}
