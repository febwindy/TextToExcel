using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TextToExcel.Command;
using TextToExcel.ViewModel;

namespace TextToExcel
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel mainViewModel {get; set;}

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
            // 初始化command
            mainViewModel = new MainViewModel(this);
            this.DataContext = mainViewModel;
        }
    }
}
