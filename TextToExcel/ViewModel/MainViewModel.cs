using EnterpriseDT.Net.Ftp;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using TextToExcel.Commons.Filter;
using TextToExcel.Commons.Utils;

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
        /// 构建对象并初始化ICommand对象
        /// </summary>
        public MainViewModel()
        {
            _IniConfUtil = IniConfUtil.getInstance();
        }

        /// <summary>
        /// 导出功能
        /// </summary>
        /// <param name="parameter">该参数为MainWindow对象，目的是为了获取窗体中控件的值</param>
        public void ExecuteExport(DateTime startDateTime, DateTime endDateTime, string savePath, List<string> filenames, MainWindow mainWindow)
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

                            // 下载数据流并关闭连接
                            MemoryStream fileMs = new MemoryStream();
                            _connection.DownloadStream(fileMs, filename.ToString());
                            _connection.Close();
                            fileMs.Seek(0, SeekOrigin.Begin);

                            // 读取流中的内容
                            StreamReader reader = new StreamReader(fileMs, Encoding.UTF8);

                            // 读取模板文件
                            MemoryStream templateMs = new MemoryStream(TextToExcel.Properties.Resources.Template_xlsx, false);
                            IWorkbook workbook = new XSSFWorkbook(templateMs);
                            ISheet sheet = workbook.GetSheetAt(0);
                            IRow row = sheet.GetRow(0);
                            Int32 numbersOfCol = row.Cells.Count;

                            // 解析数据
                            string str = reader.ReadLine();
                            for (Int32 i = 0; null != str; str = reader.ReadLine())
                            {
                                FilterChain chain = new FilterChain().AddFilter(new NameAndIdCardFilter())
                                                                    .AddFilter(new KeywordFilter());

                                string outStr;
                                if (chain.DoFilter(str, out outStr))
                                {
                                    string[] strArr = Regex.Split(outStr, " ");
                                    sheet.CreateRow(i + 1).CreateCell(0).SetCellValue(i + 1);
                                    for (Int32 j = 0; j < strArr.Length - 1; j++)
                                    {
                                        if ((j + 1) == numbersOfCol)
                                        {
                                            break;
                                        }
                                        if (j == 0)
                                        {
                                            sheet.GetRow(i + 1).CreateCell(j + 1).SetCellValue(strArr[0] + " " + strArr[1]);
                                        }
                                        else
                                        {
                                            sheet.GetRow(i + 1).CreateCell(j + 1).SetCellValue(strArr[j + 1]);
                                        }
                                    }
                                    i++;
                                }
                            }

                            // 导出成xlsx文件
                            ExcelExportUtil.Export(savePath, filename.ToString().Replace(".txt", ".xlsx"), workbook);

                            templateMs.Close();
                            reader.Close();
                            fileMs.Close();
                        }, file));
            }

            // 开始任务
            foreach (Task t in taskList)
            {
                t.Start();
            }

            new Task((() =>
            {
                    // 判断任务是否处理完成
                    while (true)
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
                            mainWindow.Dispatcher.Invoke(new Action(() =>
                            {
                                mainWindow._loadGrid.Visibility = Visibility.Collapsed;
                                mainWindow._loadCtl.Visibility = Visibility.Collapsed;
                            }));
                            break;
                        }
                    }
            })).Start();
        }
    }
}
