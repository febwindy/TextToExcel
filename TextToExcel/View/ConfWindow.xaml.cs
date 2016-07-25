using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TextToExcel.ViewModel;

namespace TextToExcel.View
{
    /// <summary>
    /// ConfWindow.xaml 的交互逻辑
    /// 作者:李文禾
    /// </summary>
    public partial class ConfWindow : Window
    {
        private ConfViewModel _ConfViewModel { get; set; }

        public ConfWindow()
        {
            InitializeComponent();

            // 对窗体进行参数设置
            this.Icon = new BitmapImage(new Uri(TextToExcel.Properties.Resources.MainWindowIcon));
            this.ResizeMode = ResizeMode.CanMinimize;
            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            // 初始化ConfViewModel并绑定DataContext
            _ConfViewModel = new ConfViewModel();
            this.DataContext = _ConfViewModel;
        }

        /// <summary>
        /// 连接事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ConnectConfFTPWindowClick(object sender, RoutedEventArgs e)
        {
            // 获取输入框中的值
            string addr = this.Address.Text;
            string username = this.Username.Text;
            string password = this.Password.Password;
            string path = this.Path.Text;
            string port = this.Port.Text;

            // 判断checkbox是否选中,根据是否选中调用不同方法
            bool result;
            if (this.Anonymous.IsChecked == true)
            {
                result = _ConfViewModel.connect(addr, port, path);
            }
            else
            {
                result = _ConfViewModel.connect(addr, username, password, path, port);
            }

            // 判断连接是否成功,根据返回结果给出相应操作
            if (false == result)
            {
                MessageBox.Show("连接失败,请确保您输入的信息正确!", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("连接成功!", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseConfFTPWindowClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// checkbox事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnonymousConfFTPWindowClick(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(CheckBox))
            {
                // 如果选中,用户名与密码不能输入,反之,可以输入
                if (((CheckBox)sender).IsChecked == true)
                {
                    this.Username.IsEnabled = false;
                    this.Password.IsEnabled = false;
                }
                else
                {
                    this.Username.IsEnabled = true;
                    this.Password.IsEnabled = true;
                }
            }
        }
    }
}
