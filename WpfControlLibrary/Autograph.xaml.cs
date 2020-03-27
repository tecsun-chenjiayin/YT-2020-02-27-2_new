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

namespace WpfControlLibrary
{
    /// <summary>
    /// 签名留底
    /// </summary>
    public partial class Autograph : UserControl
    {
        Action nextStep = null;

        public Autograph()
        {
            InitializeComponent();
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
        }
    }
}
