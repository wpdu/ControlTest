using ControlTest.Sensor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ControlTest.BaseControl
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FlipViewTest : Page
    {
        ObservableCollection<ImageModel> Images { get; set; }
        List<ImageModel> ImgSrc;

        public FlipViewTest()
        {
            this.InitializeComponent();
            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            ImgSrc = e.Parameter as List<ImageModel>;
            Images = new ObservableCollection<ImageModel>();
            //For Test
            if (ImgSrc == null)
            {
                ImgSrc = new List<ImageModel>();
                for (int i = 1; i <= 5; i++)
                {
                    BitmapImage image = new BitmapImage();
                    StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///Image/000" + i + ".jpg"));
                    image.SetSource(await file.OpenAsync(FileAccessMode.Read));
                    ImageModel im = new ImageModel() { Image = image };
                    Images.Add(im);
                    ImgSrc.Add(im);
                }
            }
            else
            {
                foreach (var item in ImgSrc)
                {
                    BitmapImage image = new BitmapImage();
                    image.SetSource(await item.ImgFile.OpenAsync(FileAccessMode.Read));
                    item.Image = image;
                    Images.Add(item);
                }
            }
            this.DataContext = Images;
            
        }

        private void img_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Image img = sender as Image;
            CompositeTransform imageTransform = img.FindName("imageTransform") as CompositeTransform;
            // Scale the photo
            imageTransform.ScaleX *= e.Delta.Scale;
            imageTransform.ScaleY *= e.Delta.Scale;
            // Constrain scale factor
            imageTransform.ScaleX = Math.Min(imageTransform.ScaleX, 5.0);
            imageTransform.ScaleY = Math.Min(imageTransform.ScaleY, 5.0);
            imageTransform.ScaleX = Math.Max(imageTransform.ScaleX, 0.5);
            imageTransform.ScaleY = Math.Max(imageTransform.ScaleY, 0.5);

            double X = 0;
            double Y = 0;
            if (img.ActualWidth * imageTransform.ScaleX > fv.ActualWidth)
            {
                X = imageTransform.TranslateX + e.Delta.Translation.X;
                double absX = Math.Abs(X);
                double maxX = (img.ActualWidth * imageTransform.ScaleX - fv.ActualWidth) / 2;
                X = absX > maxX ? maxX * (X/absX) : X;
            }
            if (img.ActualHeight * imageTransform.ScaleY > fv.ActualHeight)
            {
                Y = imageTransform.TranslateY + e.Delta.Translation.Y;
                double absY = Math.Abs(Y);
                double maxY = (img.ActualHeight * imageTransform.ScaleY - fv.ActualHeight) / 2;
                Y = absY > maxY ? maxY * (Y/absY) : Y;
            }

            imageTransform.TranslateX = X;
            imageTransform.TranslateY = Y;

            if (imageTransform.ScaleX == 1)
            {
                //base.OnManipulationDelta(e);
                //if (e.Delta.Translation.X > 50)
                //    fv.SelectedIndex -= 1;
                //else if (e.Delta.Translation.Y < -50)
                //    fv.SelectedIndex += 1;
            }
            e.Handled = false;

        }

        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FlipView flipView = sender as FlipView;
            if (e.AddedItems != null && e.AddedItems.Count == 1)
            {
                // flipView.Items.IndexOf(e.AddedItems[0]);
                FlipViewItem item = flipView.ContainerFromItem(e.AddedItems[0]) as FlipViewItem;
                if (item != null)
                {
                    ScrollViewer sv = FindVisualChildByName<ScrollViewer>(item, "sv");
                    Image img = FindVisualChildByName<Image>(item, "img");
                    UpdateImageScale(sv, img);
                }
            }
        }

        public static T FindVisualChildByName<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                string controlName = child.GetValue(Control.NameProperty) as string;
                if (controlName == name)
                {
                    return child as T;
                }
                else
                {
                    T result = FindVisualChildByName<T>(child, name);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        private void btn_Delete(object sender, RoutedEventArgs e)
        {
            if (Images != null)
            {
                ImageModel image = Images[fv.SelectedIndex];

                Images.Remove(image);
                ImgSrc.Remove(image);
                if (ImgSrc.Count == 0)
                {
                    if (Frame.CanGoBack)
                    {
                        Frame.GoBack();
                    }
                }

            }
        }

        private void sv_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer sv = sender as ScrollViewer;
            Image img = sv.Content as Image;
            UpdateImageScale(sv, img);
        }

        private static void UpdateImageScale(ScrollViewer sv, Image img)
        {
            double radio = sv.ActualWidth / sv.ActualHeight;
            double radioImg = img.ActualWidth / img.ActualHeight;
            if (radio > radioImg)
            {
                sv.ChangeView(0, 0, (float)(sv.ActualHeight / img.ActualHeight));
            }
            else
            {
                sv.ChangeView(0, 0, (float)(sv.ActualWidth / img.ActualWidth));
            }
        }
    }

}
