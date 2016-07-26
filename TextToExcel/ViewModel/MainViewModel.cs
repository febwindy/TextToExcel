using EnterpriseDT.Net.Ftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using TextToExcel.Command;
using TextToExcel.Commons.Filter;
using TextToExcel.Commons.Utils;
using TextToExcel.View;

namespace TextToExcel.ViewModel
{
    /// <summary>
    /// MainViewModel
    /// 作者:李文禾
    /// </summary>
    class MainViewModel
    {
        private IniConfUtil _IniConfUtil { get; set; }

        /// <summary>
        /// 定义导出功能Command对象
        /// </summary>
        public DelegateCommand ExportCommand { get; set; }

        /// <summary>
        /// 定义预览功能Command对象
        /// </summary>
        public DelegateCommand PreviewCommand { get; set; }

        /// <summary>
        /// 构建对象并初始化ICommand对象
        /// </summary>
        public MainViewModel()
        {
            _IniConfUtil = IniConfUtil.getInstance();
            ExportCommand = new DelegateCommand(ExecuteExport, CanExecuteExport);
            PreviewCommand = new DelegateCommand(ExecutePreview, CanExecutePreview);
        }

        /// <summary>
        /// 导出功能
        /// </summary>
        /// <param name="parameter">该参数为MainWindow对象，目的是为了获取窗体中控件的值</param>
        public void ExecuteExport(object parameter)
        {
            // 将参数对象转换为MainWindow对象
            MainWindow mainWindow = ((MainWindow)parameter);

            // 定义DateTime对象用于接收开始日期及结束日期
            DateTime startDateTime = DateTimeUtil.ConvertDateTime(mainWindow.StartDatePicker.Text, DateTimeUtil.DATE_FORMAT);
            DateTime endDateTime = DateTimeUtil.ConvertDateTime(mainWindow.EndDatePicker.Text, DateTimeUtil.DATE_FORMAT);

            // 检查控件
            if (!CheckWidget(mainWindow))
            {
                return;
            }

            // 文件夹选择
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "选择文件夹";
            fbd.RootFolder = System.Environment.SpecialFolder.MyComputer;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                // 初始化ftp服务器参数
                FTPConnection connection = new FTPConnection();
                connection.UserName = _IniConfUtil.GetPrivateProfileString("Username");
                connection.Password = _IniConfUtil.GetPrivateProfileString("Password");
                connection.ServerAddress = _IniConfUtil.GetPrivateProfileString("Address");
                connection.ServerPort = Convert.ToInt32(_IniConfUtil.GetPrivateProfileString("Port"));
                connection.ServerDirectory = _IniConfUtil.GetPrivateProfileString("Path");
                connection.CloseStreamsAfterTransfer = false;

                // 连接ftp服务器
                connection.Connect();
                string[] files = connection.GetFiles();

                // 读取内容
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

                LoadWindow _LoadWindow = new LoadWindow("导出中", 10, 10, Color.FromRgb(51, 51, 255), 6);

                // 异步处理导出任务
                _LoadWindow.Dispatcher.BeginInvoke(new Action(() =>
                {
                    // 异步处理内容
                    List<Task> taskList = new List<Task>();
                    foreach (string file in filenames)
                    {
                        taskList.Add(new Task(
                             delegate(object filename)
                             {
                                 // 初始化ftp服务器参数
                                 FTPConnection _connection = new FTPConnection();
                                 _connection.UserName = _IniConfUtil.GetPrivateProfileString("Username");
                                 _connection.Password = _IniConfUtil.GetPrivateProfileString("Password");
                                 _connection.ServerAddress = _IniConfUtil.GetPrivateProfileString("Address");
                                 _connection.ServerPort = Convert.ToInt32(_IniConfUtil.GetPrivateProfileString("Port"));
                                 _connection.ServerDirectory = _IniConfUtil.GetPrivateProfileString("Path");
                                 _connection.CloseStreamsAfterTransfer = false;

                                 // 连接ftp服务器
                                 _connection.Connect();

                                 // 下载数据流
                                 MemoryStream fileMs = new MemoryStream();
                                 _connection.DownloadStream(fileMs, filename.ToString());
                                 fileMs.Seek(0, SeekOrigin.Begin);

                                 // 读取流中的内容
                                 StreamReader reader = new StreamReader(fileMs, Encoding.UTF8);
                                 string str = reader.ReadLine();
                                 List<string[]> data = new List<string[]>();
                                 while (null != str)
                                 {
                                     FilterChain chain = new FilterChain().AddFilter(new NameAndIdCardFilter())
                                                                          .AddFilter(new KeywordFilter());
                                     string outStr;
                                     if (chain.DoFilter(str, out outStr))
                                     {
                                         string[] strArr = Regex.Split(outStr, " ");
                                         string[] tempStrArr = new string[strArr.Length - 1];
                                         for (Int32 ix = 0; ix < tempStrArr.Length; ix++)
                                         {
                                             if (ix == 0)
                                             {
                                                 tempStrArr[ix] = strArr[0] + " " + strArr[1];
                                             }
                                             else
                                             {
                                                 tempStrArr[ix] = strArr[ix + 1];
                                             }
                                         }
                                         data.Add(tempStrArr);
                                     }
                                     str = reader.ReadLine();
                                 }

                                 MemoryStream templateMs = new MemoryStream(TextToExcel.Properties.Resources.Template_xlsx, false);
                                 ExcelExportUtil.Export(fbd.SelectedPath, filename.ToString().Replace(".txt", ".xlsx"), data, templateMs);

                                 reader.Close();
                                 fileMs.Close();
                                 _connection.Close();

                             }, file));
                    }

                    // 开始任务
                    foreach (Task t in taskList)
                    {
                        t.Start();
                    }

                    // 判断任务是否处理完成
                    bool flag = true;
                    while (flag)
                    {
                        int i = 0;
                        for (; i < taskList.Count; i++)
                        {
                            if (!taskList[i].IsCompleted)
                            {
                                break;
                            }
                        }
                        if (i == taskList.Count)
                        {
                            break;
                        }
                    }
                    _LoadWindow.LabelTip.Content = "导出成功";
                    _LoadWindow.Close();
                }));

                // 异步调用加载动画
                _LoadWindow.ShowDialog();

                connection.Close();
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 判断导出功能是否可用
        /// </summary>
        /// <param name="parameter">没有参数值，目的是满足于DelegateCommand中CanExecute方法</param>
        /// <returns>默认为true</returns>
        public bool CanExecuteExport(object parameter)
        {
            return true;
        }

        /// <summary>
        /// 预览功能
        /// </summary>
        /// <param name="parameter">该参数为MainWindow对象，目的是为了获取窗体中控件的值</param>
        public void ExecutePreview(object parameter)
        {
            ((MainWindow)parameter).Title = "报表预览";
        }

        /// <summary>
        /// 判断预览功能是否可用
        /// </summary>
        /// <param name="parameter">没有参数值，目的是满足于DelegateCommand中CanExecute方法</param>
        /// <returns>默认为true</returns>
        public bool CanExecutePreview(object parameter)
        {
            return true;
        }

        /// <summary>
        /// 检查窗体中的控件是否满足条件
        /// </summary>
        /// <param name="mainWindow">要检查的窗体</param>
        /// <returns>条件不满足,返回false,否则,返回true</returns>
        private bool CheckWidget(MainWindow mainWindow)
        {
            // 定义DateTime对象用于接收开始日期及结束日期
            DateTime startDateTime = DateTimeUtil.ConvertDateTime(mainWindow.StartDatePicker.Text, DateTimeUtil.DATE_FORMAT);
            DateTime endDateTime = DateTimeUtil.ConvertDateTime(mainWindow.EndDatePicker.Text, DateTimeUtil.DATE_FORMAT);

            // 判断开始日期是否正确
            if (startDateTime == DateTime.MinValue)
            {
                System.Windows.MessageBox.Show(mainWindow, TextToExcel.Properties.Resources.StartDateTimeTip,
                                            TextToExcel.Properties.Resources.ExportMessageBoxTitle,
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Warning);
                return false;
            }

            // 判断结束日期是否正确
            if (endDateTime == DateTime.MinValue)
            {
                System.Windows.MessageBox.Show(mainWindow, TextToExcel.Properties.Resources.EndDateTimeTip,
                                            TextToExcel.Properties.Resources.ExportMessageBoxTitle,
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Warning);
                return false;
            }

            // 判断开始日期是否大于结束日期
            if (startDateTime > endDateTime)
            {
                System.Windows.MessageBox.Show(mainWindow, TextToExcel.Properties.Resources.StartGEEndDateTime,
                                            TextToExcel.Properties.Resources.ExportMessageBoxTitle,
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Warning);
                return false;
            }

            // 开始日期与结束日期是否在30天内
            if ((endDateTime - startDateTime).Days > 30)
            {
                System.Windows.MessageBox.Show(mainWindow, TextToExcel.Properties.Resources.StartAndEndDateTimeDiffTip,
                                            TextToExcel.Properties.Resources.ExportMessageBoxTitle,
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Warning);
                return false;
            }

            if (!_IniConfUtil.IsExistForConfFile())
            {
                System.Windows.MessageBox.Show(mainWindow, TextToExcel.Properties.Resources.ConfFileIsNotExist,
                            TextToExcel.Properties.Resources.ExportMessageBoxTitle,
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                return false;
            }

            return true;
        }
    }
}
