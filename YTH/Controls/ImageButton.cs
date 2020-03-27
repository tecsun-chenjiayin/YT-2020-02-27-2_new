using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ImageMagick;

namespace YTH.Controls
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:PersonalFunctions.ButtonTest"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:PersonalFunctions.ButtonTest;assembly=PersonalFunctions.ButtonTest"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:ImageButton/>
    ///
    /// </summary>
    public class ImageButton : Image
    {
        static ImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageButton), new FrameworkPropertyMetadata(typeof(ImageButton)));
        }

        private ImageSource up = null;
        private ImageSource down = null;
        System.Drawing.Bitmap bitmap = null;
        public ImageButton()
        {
            MouseDown += MouseButtonDown;
            MouseUp += MouseButtonUp;
            MouseLeave += MouseButtonLeave;
            //up = Source;

            //System.Drawing.Bitmap bitmap = ImageSourceToBitmap(this.Source); // img是前台Image控件
            //originalMagickImage = new ImageMagick.MagickImage(bitmap);

            Loaded += loaded;
        }

        public void loaded(object sender, EventArgs args)
        {
            if(originalMagickImage == null)
            {
                up = Source;
                if (down == null)
                {
                    bitmap = ImageSourceToBitmap(this.Source); // img是前台Image控件
                    new Task(new Action(() =>
                    {
                        originalMagickImage = new ImageMagick.MagickImage(bitmap);
                        ImageMagick.Percentage brightness = new ImageMagick.Percentage(100); // 100%表示不改变该属性
                        ImageMagick.Percentage saturation = new ImageMagick.Percentage(50);
                        ImageMagick.Percentage hue = new ImageMagick.Percentage(100); // 滑动条范围值0%~200%
                        ImageMagick.MagickImage newImage = new ImageMagick.MagickImage(originalMagickImage); // 相当于深复制
                        newImage.Modulate(brightness, saturation, hue);

                        // 重新给Image控件赋值新图像
                        down = newImage.ToBitmapSource();
                    })).Start();
                }
            }
        }

        private ImageMagick.MagickImage originalMagickImage = null; // 图层图像修改前的状态
        //点击状态下饱和度调节
        private void MouseButtonDown(object sender, MouseButtonEventArgs args)
        {
            if (down == null) return;
            originalMagickImage = new ImageMagick.MagickImage(bitmap);
            ImageMagick.Percentage brightness = new ImageMagick.Percentage(100); // 100%表示不改变该属性
            ImageMagick.Percentage saturation = new ImageMagick.Percentage(50);
            ImageMagick.Percentage hue = new ImageMagick.Percentage(100); // 滑动条范围值0%~200%
            ImageMagick.MagickImage newImage = new ImageMagick.MagickImage(originalMagickImage); // 相当于深复制
            newImage.Modulate(brightness, saturation, hue);

            // 重新给Image控件赋值新图像
            down = newImage.ToBitmapSource();
            Source = down;
            originalMagickImage.Dispose();
        }
        private void MouseButtonUp(object sender, MouseButtonEventArgs args)
        {
            if (Source != up)
                Source = up;
        }
        private void MouseButtonLeave(object sender, MouseEventArgs args)
        {
            if (Source != up)
                Source = up;
        }


        // 工具方法：ImageSource --> Bitmap
        public System.Drawing.Bitmap ImageSourceToBitmap(ImageSource imageSource)
        {
            BitmapSource m = (BitmapSource)imageSource;

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(m.PixelWidth, m.PixelHeight, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            System.Drawing.Imaging.BitmapData data = bmp.LockBits(
            new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            m.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride); bmp.UnlockBits(data);

            return bmp;
        }
    }
}
