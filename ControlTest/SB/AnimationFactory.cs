using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace SB
{
    class AnimationFactory
    {
        public static Storyboard CreateDoubleSB(DependencyObject dpnObj, string property, double secondTime, Double to)
        {
            DoubleAnimation ca = new DoubleAnimation();
            ca.Duration = TimeSpan.FromSeconds(secondTime);
            ca.To = to;

            Storyboard.SetTarget(ca, dpnObj);
            Storyboard.SetTargetProperty(ca, property);
            Storyboard sb = new Storyboard();

            sb.Children.Add(ca);
            return sb;
        }

        public static Storyboard CreateDoubleSB(DependencyObject dpnObj, string property, double secondTime, Double to, EasingMode em, EventHandler<object> complete)
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
            if (complete != null)
                sb.Completed += complete;
            sb.Children.Add(ca);
            return sb;
        }
        
        public static Storyboard CreateDoubleSB(DependencyObject dpnObj, string property, double secondTime, Double from, Double to, EasingMode em, EventHandler<object> complete)
        {
            DoubleAnimation ca = new DoubleAnimation();
            ca.EnableDependentAnimation = true;
            ca.Duration = TimeSpan.FromSeconds(secondTime);
            ca.From = from;
            ca.To = to;

            CircleEase ce = new CircleEase();
            ce.EasingMode = em;
            ca.EasingFunction = ce;

            Storyboard.SetTarget(ca, dpnObj);
            Storyboard.SetTargetProperty(ca, property);
            Storyboard sb = new Storyboard();
            sb.Completed += complete;
            sb.Children.Add(ca);
            return sb;


            //DoubleAnimationUsingKeyFrames daKeyFrame = new DoubleAnimationUsingKeyFrames();
            //EasingDoubleKeyFrame edKeyFrame = new EasingDoubleKeyFrame();
            //edKeyFrame.KeyTime = TimeSpan.FromSeconds(secondTime);
            //edKeyFrame.Value = to;
            //CircleEase ce = new CircleEase();
            //ce.EasingMode = em;
            //edKeyFrame.EasingFunction = ce;

            //daKeyFrame.KeyFrames.Add(edKeyFrame);
            //daKeyFrame.EnableDependentAnimation = true;

            //Storyboard.SetTarget(daKeyFrame, dpnObj);
            //Storyboard.SetTargetProperty(daKeyFrame, property);
            //Storyboard sb = new Storyboard();
            //sb.Completed += complete;

            //sb.Children.Add(daKeyFrame);
            //return sb;
        }

        public static Storyboard CreateColorSB(DependencyObject dpnObj, string property, double secondTime, Color to, EasingMode em)
        {
            ColorAnimation ca = new ColorAnimation();
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
    }
}
