using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using TextToExcel.Command;
using TextToExcel.Commons.Filter;
using TextToExcel.Commons.Utils;

namespace TextToExcel.ViewModel
{
    class MainViewModel
    {
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
                using (FileStream fs = File.OpenRead(@"F:\VerifyResult_2016-06-23.txt"))
                {
                    if (fs.CanRead)
                    {
                        int i = 0;
                        int buffer = 32;

                        byte[] bytes = new byte[fs.Length];
                        byte[] tmpBytes = new byte[buffer];

                        while ((i = fs.Read(tmpBytes, 0, buffer)) > 0)
                        {
                            long aviLength = fs.Length - fs.Position;
                            for (long j = 0; j < buffer; j++)
                            {
                                bytes[fs.Position - buffer + j] = tmpBytes[j];
                            }
                            buffer = buffer < aviLength ? buffer : (int)aviLength;
                        }

                        MemoryStream bytesMs = new MemoryStream(bytes);
                        StreamReader bytesReader = new StreamReader(bytesMs, Encoding.Default);

                        string readLine;
                        string outReadLine;
                        List<string[]> data = new List<string[]>();
                        while (null != (readLine = bytesReader.ReadLine()))
                        {
                            FilterChain chain = new FilterChain()
                                                                .AddFilter(new NameAndIdCardFilter())
                                                                .AddFilter(new KeywordFilter());
                            if (chain.DoFilter(readLine, out outReadLine))
                            {
                                string[] strArr = Regex.Split(outReadLine, " ");
                                string[] tempStrArr = new string[strArr.Length - 1];
                                for (int ix = 0; ix < tempStrArr.Length; ix++)
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
                        }

                        MemoryStream templateMs = new MemoryStream(TextToExcel.Properties.Resources.Template, false);
                        ExcelExportUtil.Export(fbd.SelectedPath, "333.xls", data, templateMs);
                    }
                }
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

            return true;
        }
    }
}
