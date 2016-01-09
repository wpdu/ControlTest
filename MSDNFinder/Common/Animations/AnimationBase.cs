using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace WinSFA.Common.Animations
{
    public abstract class AnimationBase
    {
        #region Constructor

        public AnimationBase()
        {
        }

        #endregion Constructor

        #region Properties

        protected Storyboard _Storyboard = null;
        protected FrameworkElement AnimationTarget = null;

        #endregion Properties

        #region Animation

        public virtual void Stop()
        {
            this._Storyboard.Stop();
        }

        #endregion Animation

        #region Events

        protected Action<FrameworkElement> AnimationCompleted = null;

        #endregion Events
    }
}