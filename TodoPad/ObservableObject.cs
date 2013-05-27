using System.ComponentModel;

namespace TodoPad
{
    public class ObservableObject : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void OnPropertyChanging(string propertyName)
        {
            PropertyChangingEventHandler handler = PropertyChanging;
            if (handler != null) handler(this, new PropertyChangingEventArgs(propertyName));
        }
    }
}
