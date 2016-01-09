using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace WinSFA.Common.Animations
{
    public class RotateAnimation : AnimationBase
    {
        #region Constructor

        public RotateAnimation()
        {
            Init();
        }

        public static RotateAnimation PickupAnimationNonPooling()
        {
            return new RotateAnimation();
        }

        #endregion Constructor

        #region Properties

        private DoubleAnimationUsingKeyFrames _Animation = null;

        private EasingDoubleKeyFrame _KeyFrame_from = null;
        private EasingDoubleKeyFrame _KeyFrame_to = null;

        private double TargetRotation = 0;

        private static Stack<RotateAnimation> AnimationPool = new Stack<RotateAnimation>();

        #endregion Properties

        #region Animation

        private void Init()
        {
            _Storyboard = new Storyboard();
            _Storyboard.Completed += _Storyboard_Completed;

            /***animation ***/
            _Animation = new DoubleAnimationUsingKeyFrames();

            /*key frame from*/
            _KeyFrame_from = new EasingDoubleKeyFrame();
            _KeyFrame_from.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
            _KeyFrame_from.Value = 0;
            _Animation.KeyFrames.Add(_KeyFrame_from);

            /*key frame to*/
            _KeyFrame_to = new EasingDoubleKeyFrame();
            _KeyFrame_to.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(1));
            _KeyFrame_to.Value = 1;
            _Animation.KeyFrames.Add(_KeyFrame_to);

            Storyboard.SetTargetProperty(_Animation, "(UIElement.RenderTransform).(CompositeTransform.Rotation)");
            _Storyboard.Children.Add(_Animation);
        }

        public static RotateAnimation RotateTo(FrameworkElement cell, double to, TimeSpan duration, Action<FrameworkElement> completed)
        {
            RotateAnimation animation = null;
            if (AnimationPool.Count == 0)
            {
                animation = new RotateAnimation();
            }
            else
            {
                animation = AnimationPool.Pop();
            }
            animation.InstanceRotateTo(cell, to, duration, completed);
            return animation;
        }

        public static RotateAnimation RotateBy(FrameworkElement cell, double by, TimeSpan duration, Action<FrameworkElement> completed)
        {
            RotateAnimation animation = null;
            if (AnimationPool.Count == 0)
            {
                animation = new RotateAnimation();
            }
            else
            {
                animation = AnimationPool.Pop();
            }

            animation.InstanceRotateBy(cell, by, duration, completed);
            return animation;
        }

        public static RotateAnimation RotateFromTo(FrameworkElement cell,
            double from,
            double to,
            TimeSpan duration, Action<FrameworkElement> completed)
        {
            RotateAnimation animation = null;
            if (AnimationPool.Count == 0)
            {
                animation = new RotateAnimation();
            }
            else
            {
                animation = AnimationPool.Pop();
            }

            animation.InstanceRotateFromTo(cell, from, to, duration, completed);
            return animation;
        }

        public void InstanceRotateBy(FrameworkElement cell, double by, TimeSpan duration, Action<FrameworkElement> completed)
        {
            /*value*/
            CompositeTransform transform = cell.RenderTransform as CompositeTransform;
            if (transform == null)
            {
                cell.RenderTransform = transform = new CompositeTransform();
                cell.RenderTransformOrigin = new Point(0.5d, 0.5d);
            }
            var from = transform.Rotation;
            var to = from + by;

            this.InstanceRotateTo(cell, to, duration, completed);
        }

        public void InstanceRotateFromTo(FrameworkElement cell,
            double from,
            double to,
            TimeSpan duration, Action<FrameworkElement> completed)
        {
            cell.RenderTransform.SetValue(CompositeTransform.RotationProperty, from);
            this.InstanceRotateTo(cell, to, duration, completed);
        }

        public void InstanceRotateTo(FrameworkElement cell, double to, TimeSpan duration, Action<FrameworkElement> completed)
        {
            this.Animate(cell, to, duration, completed);
        }

        private void Animate(FrameworkElement cell, double to, TimeSpan duration, Action<FrameworkElement> completed)
        {
            AnimationTarget = cell;
            TargetRotation = to;
            AnimationCompleted = completed;

            if (_Storyboard == null)
            {
                Init();
            }
            else
            {
                _Storyboard.Stop();
            }

            /*time*/
            _KeyFrame_to.KeyTime = KeyTime.FromTimeSpan(duration);

            /*value*/
            CompositeTransform transform = cell.RenderTransform as CompositeTransform;
            _KeyFrame_from.Value = transform.Rotation;
            _KeyFrame_to.Value = to;

            Storyboard.SetTarget(_Animation, AnimationTarget);

            _Storyboard.Begin();
        }

        private void _Storyboard_Completed(object sender, object e)
        {
            AnimationTarget.RenderTransform.SetValue(CompositeTransform.RotationProperty, TargetRotation);

            if (AnimationCompleted != null)
            {
                AnimationCompleted(AnimationTarget);
            }

            AnimationPool.Push(this);
        }

        #endregion Animation
    }
}