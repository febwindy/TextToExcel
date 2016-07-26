using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TextToExcel.View
{
    /// <summary>
    /// LoadWindow.xaml 的交互逻辑
    /// 作者:李文禾
    /// </summary>
    public partial class LoadWindow : Window
    {
        public Label LabelTip;

        public LoadWindow()
        {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri(TextToExcel.Properties.Resources.MainWindowIcon));
            Init("加载中", 10, 10, Color.FromRgb(51,51,255), 6);
        }

        public LoadWindow(string tip, int elliWidth, int elliHeight, Color elliColor, int elliSize)
        {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri(TextToExcel.Properties.Resources.MainWindowIcon));
            Init(tip, elliWidth, elliHeight, elliColor, elliSize);
        }

        public void Init(string tip, int elliWidth, int elliHeight, Color elliColor, int elliSize)
        {
            Canvas canvas = new Canvas();
            canvas.RenderTransformOrigin = new Point(0.5, 0.5);
            canvas.HorizontalAlignment = HorizontalAlignment.Center;
            canvas.VerticalAlignment = VerticalAlignment.Center;
            canvas.Width = this.Width;
            canvas.Height = this.Height;
            canvas.Name = "canvans";

            double windowRadius = (this.Width - elliWidth) / 2;
            double angle = 180 / elliSize;

            for (int i = 0; i <= elliSize; i++)
            {
                Ellipse elli = new Ellipse();
                elli.Width = elliWidth;
                elli.Height = elliHeight;

                elli.Fill = new SolidColorBrush(elliColor);
                elli.Opacity = 1 - i * 0.15;

                elli.SetValue(Canvas.LeftProperty, windowRadius + Math.Cos(Math.PI * (90 + i * angle) / 180) * windowRadius);
                elli.SetValue(Canvas.TopProperty, windowRadius - Math.Sin(Math.PI * (90 + i * angle) / 180) * windowRadius);
                canvas.Children.Add(elli);
            }

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 0;
            animation.To = 360;
            animation.Duration = new Duration(TimeSpan.FromSeconds(1.2d));
            animation.RepeatBehavior = RepeatBehavior.Forever;

            RotateTransform rotateTransform = new RotateTransform();
            canvas.RenderTransform = rotateTransform;
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, animation);

            Grid grid = new Grid();
            grid.Children.Add(canvas);

            LabelTip = new Label();
            LabelTip.Content = tip;
            LabelTip.Name = "LableTip";
            LabelTip.FontWeight = FontWeights.Bold;
            LabelTip.HorizontalAlignment = HorizontalAlignment.Center;
            LabelTip.VerticalAlignment = VerticalAlignment.Center;
            grid.Children.Add(LabelTip);

            this.Content = grid;
        }
    }
}
