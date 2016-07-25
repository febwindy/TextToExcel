using System.ComponentModel;

namespace TextToExcel.Model
{
    /// <summary>
    /// 属性变动时事件处理
    /// 作者:李文禾
    /// </summary>
    abstract class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void onPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
