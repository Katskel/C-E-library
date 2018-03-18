using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lab2
{
    public class BindingElement<T> : INotifyPropertyChanged
    {
        private T item;
        public T Item
        {
            get
            {
                return item;
            }
            set
            {
                item = value;
                NotifyPropertyChanged("Item");
            }
        }

        public BindingElement(T item)
        {
            this.item = item;
        }

        public override string ToString()
        {
            return item.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
            
    }
}
