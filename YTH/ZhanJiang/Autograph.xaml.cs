using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YTH.ZhanJiang
{
    /// <summary>
    /// 签名留底
    /// </summary>
    public partial class Autograph : UserControl
    {
        int index = 0;
        string path = "";
        public delegate void NEXT(string style);
        NEXT nextStep;
        public Autograph()
        {
            InitializeComponent();
            Business.ok_write = ok;
            Business.re_write = retWrite;
        }

        static Autograph self = null;
        public static Autograph GetObject()
        {
            if (self == null)
                self = new Autograph();
            return self;
        }

        public void Goin(NEXT nextStep)
        {
            this.nextStep = nextStep;
            Functions.CD.business1.setBoxStatus(true);
            Functions.CD.business1.setBusinessValue(this);
            image.Visibility = Visibility.Visible;
        }

        //保存
        public void saveToBitmap(string path)
        {
            var rtb = new RenderTargetBitmap((int)n.ActualWidth, (int)n.ActualHeight, 96, 96, PixelFormats.Default);
            rtb.Render(n);
            PngBitmapEncoder encode = new PngBitmapEncoder();
            encode.Frames.Add(BitmapFrame.Create(rtb));
            MemoryStream ms = new MemoryStream();
            encode.Save(ms);
            FileStream fs = File.Create(path);
            ms.WriteTo(fs);
            fs.Flush();
            fs.Close();
            n.Strokes.Clear();
        }
        //重写
        public void retWrite()
        {
            n.Strokes.Clear();
            image.Visibility = Visibility.Visible;
        }

        public void ok()
        {
            path = Functions.CD.getBasePath() + (index++) + ".png";
            saveToBitmap(path);
            Functions.CD.business1.setBoxStatus(false);
            this.nextStep(@"data:image/png;base64," + tools.CommonFunction.FileToBase64(path));
        }

        private void n_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            image.Visibility = Visibility.Hidden;
        }
    }
}
