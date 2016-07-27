using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TextToExcel.View.Control
{
    /// <summary>
    /// LoadControl.xaml 的交互逻辑
    /// </summary>
    public partial class LoadControl : UserControl
    {
        private readonly Storyboard sb = new Storyboard();

        public LoadControl()
        {
            InitializeComponent();
            Init(10, 10, Color.FromRgb(51, 51, 255), 6);
        }

        public void Init(int elliWidth, int elliHeight, Color elliColor, int elliSize)
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
            //rotateTransform.BeginAnimation(RotateTransform.AngleProperty, animation);

            sb.Children.Add(animation);
            Storyboard.SetTarget(animation, canvas);
            Storyboard.SetTargetProperty(animation, new PropertyPath("RenderTransform.Angle"));

            Grid grid = new Grid();
            grid.Children.Add(canvas);

            this.Content = grid;
        }

        private void HandleVisibleChanged(object sender,
                            DependencyPropertyChangedEventArgs e)
        {
            bool isVisible = (bool)e.NewValue;
            if (isVisible)
                sb.Begin();
            else
                sb.Stop();
        }  
    }
}
