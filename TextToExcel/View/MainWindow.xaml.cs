using System;
using System.Windows;
using System.Windows.Media.Imaging;
using TextToExcel.Test;
using TextToExcel.View;
using TextToExcel.ViewModel;

namespace TextToExcel
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// 作者:李文禾
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _MainViewModel {get; set;}

        public MainWindow()
        {
            // 初始化窗体
            InitializeComponent();

            // 对窗体进行参数设置
            this.Icon = new BitmapImage(new Uri(TextToExcel.Properties.Resources.MainWindowIcon));
            this.Title = TextToExcel.Properties.Resources.MainWindowTitle;
            this.Width = Double.Parse(TextToExcel.Properties.Resources.MainWindowWidth);
            this.Height = Double.Parse(TextToExcel.Properties.Resources.MainWindowHeight);
            this.ResizeMode = ResizeMode.CanMinimize;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            // 初始化MainViewModel并绑定至DataContext
            _MainViewModel = new MainViewModel();
            this.DataContext = _MainViewModel;
        }

        private void FTPConfClick(object sender, RoutedEventArgs e)
        {
            ConfWindow confWindow = new ConfWindow();
            confWindow.ShowDialog();
        }
    }
}
