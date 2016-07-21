using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextToExcel.Model
{
    class VerifyResult : INotifyPropertyChanged
    {
        private long   Id { get; set; }

        private string Date { get; set; }

        private string Time { get; set; }

        private string Name { get; set; }

        private string IdCard { get; set; }

        private string Result { get; set; }

        private float  Score { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
