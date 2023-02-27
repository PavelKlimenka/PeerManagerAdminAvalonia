
using System.ComponentModel;

namespace Client.Utilities
{
    public class Observable<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private T? _value;
        public T? Value 
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                PropertyChanged?.Invoke(this, new(nameof(T)));
            }
        }

        public static Observable<T?> operator <<(Observable<T?> lhs, T? rhs)
        {
            lhs.Value = rhs;
            return lhs;
        }
    }
}
