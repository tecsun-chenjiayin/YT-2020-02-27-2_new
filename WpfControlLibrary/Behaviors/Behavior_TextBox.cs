using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace WpfControlLibrary
{
    /*
     使用：
     1、添加引用：System.Windows.Interactivity.dll
     2、xmlns:i="clr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity"
     3、 
        <TextBox FontSize="20" BorderThickness="3" BorderBrush="Black" Margin="60,282,428,91" >
            <i:Interaction.Behaviors>
                <local:Behavior_TextBox Text="测试66666" FontSize="15" Foreground="Blue" Opacity="0.8"/>
            </i:Interaction.Behaviors>
        </TextBox>
         */

    class Behavior_TextBox : Behavior<TextBox>
    {
        public static DependencyProperty TextProperty = 
            DependencyProperty.Register("Text", typeof(string), typeof(Behavior_TextBox), new PropertyMetadata("提示文字"), new ValidateValueCallback(TextCallBack));
        public string Text
        {
            set { SetValue(TextProperty, value);}
            get { return (string)GetValue(TextProperty); }
        }
        static bool TextCallBack(object val)
        {
            return true;
        }

        public static DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(Behavior_TextBox), new PropertyMetadata(20.0), new ValidateValueCallback(FontSizeCallBack));
        public double FontSize
        {
            set { SetValue(TextProperty, value); }
            get { return (double)GetValue(TextProperty); }
        }
        static bool FontSizeCallBack(object val)
        {
            return true;
        }

        public static DependencyProperty ForegroundProperty =
    DependencyProperty.Register("Foreground", typeof(Brush), typeof(Behavior_TextBox), new PropertyMetadata(Brushes.Black), new ValidateValueCallback(ForegroundCallBack));
        public Brush Foreground
        {
            set { SetValue(TextProperty, value); }
            get { return (Brush)GetValue(TextProperty); }
        }
        static bool ForegroundCallBack(object val)
        {
            return true;
        }

        public static DependencyProperty OpacityProperty =
            DependencyProperty.Register("Opacity", typeof(double), typeof(Behavior_TextBox), new PropertyMetadata(0.45), new ValidateValueCallback(OpacityCallBack));
        public double Opacity
        {
            set { SetValue(TextProperty, value); }
            get { return (double)GetValue(TextProperty); }
        }
        static bool OpacityCallBack(object val)
        {
            return true;
        }

        TextBlock tip = new TextBlock();
        VisualBrush vb = new VisualBrush();
        public Behavior_TextBox()
        {
            tip.Text = "测试测试";
            tip.FontSize = 20;
            tip.Foreground = Brushes.Gray;

            Binding bindingText = new Binding("Text");
            bindingText.Source = this;
            tip.SetBinding(TextBlock.TextProperty, bindingText);

            Binding bindingFontSize = new Binding("FontSize");
            bindingFontSize.Source = this;
            tip.SetBinding(TextBlock.FontSizeProperty, bindingFontSize);

            Binding bindingForeground = new Binding("Foreground");
            bindingForeground.Source = this;
            tip.SetBinding(TextBlock.ForegroundProperty, bindingForeground);

            Binding bindingOpacity = new Binding("Opacity");
            bindingOpacity.Source = this;
            tip.SetBinding(TextBlock.OpacityProperty, bindingOpacity);


            vb.TileMode = TileMode.None;
            vb.Stretch = Stretch.None;
            vb.AlignmentX = AlignmentX.Left;
            vb.Visual = tip;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.GotFocus += AssociatedObject_GotFocus;
            AssociatedObject.LostFocus += AssociatedObject_LostFocus;
            AssociatedObject.Loaded += AssociatedObject_OnLoaded;
            AssociatedObject.TextChanged += AssociatedObject_TextChange;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.GotFocus -= AssociatedObject_GotFocus;
            AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
            AssociatedObject.TextChanged -= AssociatedObject_TextChange;
        }

        private void AssociatedObject_OnLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject_LostFocus(AssociatedObject, e);
        }

        Brush old = null;
        private void AssociatedObject_GotFocus(object sender, EventArgs e)
        {
            string text = (string)AssociatedObject.GetValue(TextBox.TextProperty);
            if (text == null && text != "")
                return;
            AssociatedObject.SetValue(TextBox.BackgroundProperty, old);
        }
        private void AssociatedObject_TextChange(object sender, EventArgs e)
        {
            string text = (string)AssociatedObject.GetValue(TextBox.TextProperty);
            if (text == null && text != "")
                return;

            if(text != null && text != "")
                AssociatedObject.SetValue(TextBox.BackgroundProperty, old);
            else
            {
                if (old == null)
                    old = (Brush)AssociatedObject.GetValue(TextBox.BackgroundProperty);
                tip.Background = old;
                AssociatedObject.SetValue(TextBox.BackgroundProperty, vb);
            }
        }
        private void AssociatedObject_LostFocus(object sender, EventArgs e)
        {
            string text = (string)AssociatedObject.GetValue(TextBox.TextProperty);
            if (text != null && text != "")
                return;
            if (old == null)
                old = (Brush)AssociatedObject.GetValue(TextBox.BackgroundProperty);
            tip.Background = old;
            AssociatedObject.SetValue(TextBox.BackgroundProperty, vb);
        }
    }
}
