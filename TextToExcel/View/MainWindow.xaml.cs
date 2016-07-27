using EnterpriseDT.Net.Ftp;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using TextToExcel.Commons.Utils;
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
        private IniConfUtil _IniConfUtil { get; set; }

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

            // 加载配置
            _IniConfUtil = IniConfUtil.getInstance();
        }

        private void FTPConfClick(object sender, RoutedEventArgs e)
        {
            ConfWindow confWindow = new ConfWindow();
            confWindow.ShowDialog();
        }

        private void ExportClick(object sender, RoutedEventArgs e)
        {
            // 定义DateTime对象用于接收开始日期及结束日期
            DateTime startDateTime = DateTimeUtil.ConvertDateTime(this.StartDatePicker.Text, DateTimeUtil.DATE_FORMAT);
            DateTime endDateTime = DateTimeUtil.ConvertDateTime(this.EndDatePicker.Text, DateTimeUtil.DATE_FORMAT);

            // 验证条件是否满足
            if (!CheckWidget(startDateTime, endDateTime) || !IsExistForConf())
            {
                return;
            }

            // 初始化ftp服务器参数
            FTPConnection connection = new FTPConnection();
            connection.UserName = _IniConfUtil.GetPrivateProfileString("Username");
            connection.Password = _IniConfUtil.GetPrivateProfileString("Password");
            connection.ServerAddress = _IniConfUtil.GetPrivateProfileString("Address");
            connection.ServerPort = Convert.ToInt32(_IniConfUtil.GetPrivateProfileString("Port"));
            connection.ServerDirectory = _IniConfUtil.GetPrivateProfileString("Path");
            connection.CloseStreamsAfterTransfer = false;

            try
            {
                // 连接ftp服务器获取文件列表,获取完后,关闭连接
                connection.Connect();
                string[] files = connection.GetFiles();
                connection.Close();

                // 根据条件筛选文件列表
                List<string> filenames = new List<string>();
                foreach (string file in files)
                {
                    int firstIndex = file.IndexOf("_") + 1;
                    int lastIndex = file.IndexOf(".txt");
                    string substr = file.Substring(firstIndex, lastIndex - firstIndex);

                    DateTime dt = DateTimeUtil.ConvertDateTime(substr, DateTimeUtil.DATE_FORMAT);
                    if (dt >= startDateTime && dt <= endDateTime)
                    {
                        filenames.Add(file);
                    }
                }

                // 判断FTP中是否存在文件
                if (filenames.Count == 0)
                {
                    System.Windows.MessageBox.Show(this, TextToExcel.Properties.Resources.ExportNoFilesInFTP,
                                            TextToExcel.Properties.Resources.ExportMessageBoxTitle,
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Warning);
                    return;
                }

                // 打开摭罩
                this._loadText.Text = "导出中";
                this._loadCtl.Visibility = Visibility.Visible;
                this._loadGrid.Visibility = Visibility.Visible;

                // 文件夹选择
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "选择文件夹";
                fbd.RootFolder = System.Environment.SpecialFolder.MyComputer;
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // 导出
                    _MainViewModel.ExecuteExport(startDateTime, endDateTime, fbd.SelectedPath, filenames, this);
                }
                else
                {
                    // 关闭摭罩
                    this._loadGrid.Visibility = Visibility.Collapsed;
                    this._loadCtl.Visibility = Visibility.Collapsed;
                    return;
                }
            }
            catch (Exception e1)
            {
                System.Windows.MessageBox.Show("连接失败,请检查您输入的信息正确及FTP服务器是否启动!",
                    "错误",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// 检查窗体中的控件是否满足条件
        /// </summary>
        /// <returns>条件不满足,返回false,否则,返回true</returns>
        private bool CheckWidget(DateTime startDateTime, DateTime endDateTime)
        {
            // 判断开始日期是否正确
            if (startDateTime == DateTime.MinValue)
            {
                System.Windows.MessageBox.Show(this, TextToExcel.Properties.Resources.StartDateTimeTip,
                                            TextToExcel.Properties.Resources.ExportMessageBoxTitle,
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Warning);
                return false;
            }

            // 判断结束日期是否正确
            if (endDateTime == DateTime.MinValue)
            {
                System.Windows.MessageBox.Show(this, TextToExcel.Properties.Resources.EndDateTimeTip,
                                            TextToExcel.Properties.Resources.ExportMessageBoxTitle,
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Warning);
                return false;
            }

            // 判断开始日期是否大于结束日期
            if (startDateTime > endDateTime)
            {
                System.Windows.MessageBox.Show(this, TextToExcel.Properties.Resources.StartGEEndDateTime,
                                            TextToExcel.Properties.Resources.ExportMessageBoxTitle,
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Warning);
                return false;
            }

            // 开始日期与结束日期是否在30天内
            if ((endDateTime - startDateTime).Days > 30)
            {
                System.Windows.MessageBox.Show(this, TextToExcel.Properties.Resources.StartAndEndDateTimeDiffTip,
                                            TextToExcel.Properties.Resources.ExportMessageBoxTitle,
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查配置文件是否存在
        /// </summary>
        /// <returns>条件不满足,返回false,否则,返回true</returns>
        private bool IsExistForConf()
        {
            if (!_IniConfUtil.IsExistForConfFile())
            {
                System.Windows.MessageBox.Show(this, TextToExcel.Properties.Resources.ConfFileIsNotExist,
                            TextToExcel.Properties.Resources.ExportMessageBoxTitle,
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
    }
}
