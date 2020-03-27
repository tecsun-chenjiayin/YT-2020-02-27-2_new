using Microsoft.Ink;
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
using System.Windows.Threading;

namespace WpfControlLibrary
{
    /// <summary>
    /// HandWrite.xaml 的交互逻辑
    /// </summary>
    public partial class HandWrite : UserControl
    {
        public HandWrite()
        {
            InitializeComponent();
        }

        private RecognizerContext rct;

        private void inkInput_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            if (inkInput.Strokes.Count == 0)
                return;
            MemoryStream ms = new MemoryStream();
            inkInput.Strokes.Save(ms);
            InkCollector myInkCollector = new InkCollector();
            Ink ink = new Ink();
            ink.Load(ms.ToArray());


            rct.StopBackgroundRecognition();

            rct.Strokes = ink.Strokes;
            // rct.CharacterAutoCompletion = RecognizerCharacterAutoCompletionMode.Full;

            rct.BackgroundRecognizeWithAlternates(0);


            //timer.Start();
        }

        private void ink_Here()
        {
            Recognizers recos = new Recognizers();
            Recognizer chineseReco = recos.GetDefaultRecognizer();

            rct = chineseReco.CreateRecognizerContext();
            rct.RecognitionFlags = Microsoft.Ink.RecognitionModes.WordMode;

            this.rct.RecognitionWithAlternates += new RecognizerContextRecognitionWithAlternatesEventHandler(rct_RecognitionWithAlternates);
        }
        private void rct_RecognitionWithAlternates(object sender, RecognizerContextRecognitionWithAlternatesEventArgs e)
        {


            if (!Dispatcher.CheckAccess())
            {

                Dispatcher.Invoke(DispatcherPriority.Send, new RecognizerContextRecognitionWithAlternatesEventHandler(rct_RecognitionWithAlternates), sender, e);

                return;
            }


            if (RecognitionStatus.NoError == e.RecognitionStatus)
            {
                this.panelChoose.Children.Clear();
                RecognitionAlternates allAlternates = e.Result.GetAlternatesFromSelection();
                // show each of the other alternates
                int i = 1;
                double width = 56;
                double height = 56;
                bool isFirst = true;
                foreach (RecognitionAlternate oneAlternate in allAlternates)
                {
                    Label lbl = new Label();
                    lbl.Name = "lbl" + i;
                    lbl.Tag = oneAlternate.ToString();
                    lbl.Content = lbl.Tag.ToString();
                    lbl.Width = width;
                    lbl.Height = height;
                    lbl.FontSize = 24F;
                    if (isFirst)
                    {
                        lbl.Width = 172;
                        lbl.Height = 172;
                        lbl.FontSize = 35F;
                        isFirst = false;
                    }
                    lbl.FontFamily = new System.Windows.Media.FontFamily("微软雅黑");
                    lbl.BorderThickness = new Thickness(1);
                    lbl.Margin = new Thickness(1, 1, 1, 1);
                    lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
                    lbl.VerticalContentAlignment = VerticalAlignment.Center;
                    lbl.BorderBrush = border.BorderBrush;// System.Windows.Media.Brushes.Gray;
                    lbl.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(lbl_MouseLeftButtonDown);
                    this.panelChoose.Children.Add(lbl);
                    ++i;
                }
            }
        }

        private void lbl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Label label = e.Source as Label;
            Send(label.Content.ToString());
            clear_PreviewMouseDown(null, null);
        }

        private void Send(string txt)
        {
            try
            {
                if (txt[0] == '{' && txt[txt.Length - 1] == '}')
                    System.Windows.Forms.SendKeys.SendWait(txt);
                else
                {
                    System.Windows.Clipboard.SetText(txt);
                    System.Windows.Forms.SendKeys.SendWait("^v");
                }
            }
            catch { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            inkInput.Strokes.Clear();
        }

        private void clear_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            inkInput.Strokes.Clear();
            panelChoose.Children.Clear();
        }

        private void cancel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (inkInput.Strokes.Count == 0)
                return;
            inkInput.Strokes.RemoveAt(inkInput.Strokes.Count - 1);
            inkInput_StrokeCollected(null, null);
            if (inkInput.Strokes.Count == 0)
                panelChoose.Children.Clear();
        }

        private void delete_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            inkInput.Strokes.Clear();
            panelChoose.Children.Clear();
            Send("{BACKSPACE}");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ink_Here();
        }
    }
}
