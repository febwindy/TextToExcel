using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TextToExcel.Command
{
    /**
     * 委托command 
     **/
    class DelegateCommand : ICommand
    {
        public delegate void ICommandOnExecute(object parameter);

        public delegate bool ICommandOnCanExecute(object parameter);

        private ICommandOnExecute _execute;

        private ICommandOnCanExecute _canExecute;

        public DelegateCommand(ICommandOnExecute onExecuteMethod, ICommandOnCanExecute onCanExecuteMethod)
        {
            this._execute = onExecuteMethod;
            this._canExecute = onCanExecuteMethod;
        }

        #region ICommand Members
        bool ICommand.CanExecute(object parameter)
        {
            return _canExecute.Invoke(parameter);
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        void ICommand.Execute(object parameter)
        {
            _execute.Invoke(parameter);
        }
        #endregion
    }
}
