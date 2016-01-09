using System;
using System.Collections.Generic;
using Tool;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Common.Control
{
    public sealed partial class MyBgImage : UserControl
    {
        public MyBgImage()
        {
            this.InitializeComponent();
        }


        public delegate void ImageChangeDoneDel(object sender, bool isEnable);
        public event ImageChangeDoneDel ImgChangeDone;

        public BitmapSource NewImage
        {
            get { return (BitmapSource)GetValue(NewImageProperty); }
            set { SetValue(NewImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NewImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NewImageProperty = DependencyProperty.Register("NewImage", typeof(BitmapSource), typeof(MyBgImage), new PropertyMetadata(new BitmapImage(new Uri("/Image/bg_main.png", UriKind.RelativeOrAbsolute)), NewImageChandedCallback));


        private static void NewImageChandedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MyBgImage mImg = (MyBgImage)d;
            mImg.ChangeBGImage((BitmapSource)e.NewValue);
        }

        public void ChangeBGImage(BitmapSource newImg)
        {
            ImageBrush i1 = this.BgImage1;  // 1 在底层 （换成新图片后） 透明度逐渐增加， 2在顶层颜色逐渐变淡， 结束后 2图片改为新图，然后 显示2，隐藏1
            ImageBrush i2 = this.BgImage2;
            if (this.milliSeconds == 0)
            {
                i2.ImageSource = (BitmapSource)(newImg);
                if (this.ImgChangeDone != null)
                {
                    this.ImgChangeDone(this, true);
                }
                return;
            }

            i1.ImageSource = (BitmapSource)(newImg);

            Grid g1 = this.G1;
            Grid g2 = this.G2;
            Storyboard sb1 = AnimationFactory.CreateDoubleSB(g1, "Opacity", this.milliSeconds, 1);
            Storyboard sb2 = AnimationFactory.CreateDoubleSB(g2, "Opacity", this.milliSeconds + 500, 0);

            sb2.Completed += (s, ee) =>
            {
                i2.Stretch = Stretch.UniformToFill;

                i2.ImageSource = i1.ImageSource;
                g2.Opacity = 1;
                g1.Opacity = 0;
                if (this.ImgChangeDone != null)
                {
                    this.ImgChangeDone(this, true);
                }
            };
            sb1.Begin();
            sb2.Begin();
        }

        private double milliSeconds;
        public double MilliSeconds
        {
            get { return milliSeconds; }
            set { milliSeconds = value; }
        }

        public void ClearDependencyValue()
        {
            ClearValue(NewImageProperty);
        }
    }
}
