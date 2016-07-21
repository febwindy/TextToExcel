using System;
using System.Windows;
using TextToExcel.Command;
using TextToExcel.Utils;

namespace TextToExcel.ViewModel
{
    class MainViewModel
    {
        //
        // 摘要:
        //     定义导出功能Command对象
        public DelegateCommand ExportCommand { get; set; }

        //
        // 摘要:
        //     定义预览功能Command对象
        public DelegateCommand PreviewCommand { get; set; }

        //
        // 摘要:
        //     构建对象并初始化ICommand对象
        public MainViewModel()
        {
            ExportCommand = new DelegateCommand(ExecuteExport, CanExecuteExport);
            PreviewCommand = new DelegateCommand(ExecutePreview, CanExecutePreview);
        }

        //
        // 摘要:
        //     导出功能
        //
        // 参数:
        //   parameter:
        //     该参数为MainWindow对象，目的是为了获取窗体中控件的值
        public void ExecuteExport(object parameter)
        {
            // 将参数对象转换为MainWindow对象
            MainWindow mainWindow = ((MainWindow)parameter);

            // 定义DateTime对象用于接收开始日期及结束日期
            DateTime startDateTime = DateTimeUtil.ConvertDateTime(mainWindow.StartDatePicker.Text, DateTimeUtil.DATE_FORMAT);
            DateTime endDateTime = DateTimeUtil.ConvertDateTime(mainWindow.EndDatePicker.Text, DateTimeUtil.DATE_FORMAT);

            // 判断开始日期是否正确
            if (startDateTime == DateTime.MinValue)
            {
                MessageBox.Show(mainWindow, TextToExcel.Properties.Resources.StartDateTimeTip,
                                            TextToExcel.Properties.Resources.ExportMessageBoxTitle,
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Warning);
                return;
            }

            // 判断结束日期是否正确
            if (endDateTime == DateTime.MinValue)
            {
                MessageBox.Show(mainWindow, TextToExcel.Properties.Resources.EndDateTimeTip,
                                            TextToExcel.Properties.Resources.ExportMessageBoxTitle,
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Warning);
                return;
            }

            // 判断开始日期是否大于结束日期
            if (startDateTime >= endDateTime)
            {
                MessageBox.Show(mainWindow, TextToExcel.Properties.Resources.StartGEEndDateTime,
                                            TextToExcel.Properties.Resources.ExportMessageBoxTitle,
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Warning);
                return;
            }

            // 开始日期与结束日期是否在一个月内
            if (startDateTime.Year == endDateTime.Year && (endDateTime.Month - startDateTime.Month) != 0) {
                MessageBox.Show(mainWindow, TextToExcel.Properties.Resources.StartAndEndDateTimeDiffTip,
                                            TextToExcel.Properties.Resources.ExportMessageBoxTitle,
                                            MessageBoxButton.OK,
                                            MessageBoxImage.Warning);
                return;
            }

            Console.WriteLine("right");
        }

        //
        // 摘要:
        //     判断导出功能是否可用
        //
        // 参数:
        //   parameter:
        //     没有参数值，目的是满足于DelegateCommand中CanExecute方法
        //
        // 返回结果:
        //     默认为true
        public bool CanExecuteExport(object parameter)
        {
            return true;
        }

        //
        // 摘要:
        //     预览功能
        //
        // 参数:
        //   parameter:
        //     该参数为MainWindow对象，目的是为了获取窗体中控件的值
        public void ExecutePreview(object parameter)
        {
            ((MainWindow)parameter).Title = "报表预览";
        }

        //
        // 摘要:
        //     判断预览功能是否可用
        //
        // 参数:
        //   parameter:
        //     没有参数值，目的是满足于DelegateCommand中CanExecute方法
        //
        // 返回结果:
        //     默认为true
        public bool CanExecutePreview(object parameter)
        {
            return true;
        }
    }
}
