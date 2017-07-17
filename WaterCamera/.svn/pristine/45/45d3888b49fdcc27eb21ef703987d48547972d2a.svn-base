using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RekTec.Mobile.Client.IOS
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        protected virtual ViewModelBase SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            field = value;
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            return this;
        }

        protected virtual ViewModelBase Raise(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            return this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static ViewModelBase CurrentViewModel { get; private set; }
    }
}
