using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Library;

namespace Lab2
{
    /// <summary>
    /// Логика взаимодействия для LINQWindow.xaml
    /// </summary>
    public partial class LINQWindow : Window
    {
        IEnumerable<Book> books;
        IEnumerable<Author> authors;
        IEnumerable<PublishmentHouse> houses;
        public LINQWindow(object value, Type type)
        { 
            InitializeComponent();
            if (type == typeof(Book))
            {
                books = (IEnumerable<Book>)value;
                linqListBox.ItemsSource = books;
            }
            else if (type == typeof(Author))
            {
                authors = (IEnumerable<Author>)value;
                linqListBox.ItemsSource = authors;
            }
            else
            {
                houses = (IEnumerable<PublishmentHouse>)value;
                linqListBox.ItemsSource = houses;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
