using System;
using TextToExcel.Command;

namespace TextToExcel.ViewModel
{
    class MainViewModel
    {
        public DelegateCommand exportCommand { get; set; }

        public DelegateCommand detailCommand { get; set; }

        public MainViewModel(MainWindow parentWindow)
        {
            exportCommand = new DelegateCommand(executeExport, canExecuteExport);
            detailCommand = new DelegateCommand(executeDetail, canExecuteDetail);
        }

        /**
         * 导出功能
         **/
        public void executeExport(object parameter)
        {
             Console.WriteLine(parameter);
        }

        public bool canExecuteExport(object parameter)
        {
            return true;
        }

        /**
         * 查看功能 
         **/
        public void executeDetail(object parameter)
        {
            ((MainWindow)parameter).Title = "报表查看";
        }

        public bool canExecuteDetail(object parameter)
        {
            return true;
        }

    }
}
