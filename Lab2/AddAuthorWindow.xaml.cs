using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.Win32;
using Library;

namespace Lab2
{
    /// <summary>
    /// Логика взаимодействия для AddAuthorWindow.xaml
    /// </summary>
    public partial class AddAuthorWindow : Window
    {
        Author temp, _author;
        bool hasAuthor;
        ProjectBase _base;
        public AddAuthorWindow(Author author, ProjectBase myBase)
        {
            InitializeComponent();
            _base = myBase;
            _author = author;
            if (author == null)
            {
                temp = new Author(null, default(DateTime), null);
                temp.Books = new ItemCollection<Book>();
                hasAuthor = false;
            }
            else
            {
                temp = new Author(author.Name, author.Date, author.Photo);
                temp.Books = new ItemCollection<Book>();
                foreach (Book book in author.Books)
                    temp.Books.Add(book);
                hasAuthor = true;
            }
            this.Resources.Add("Author", temp);

            SetResourceReference(DataContextProperty, "Author");
            booksComboBox.ItemsSource = _base.books;
            booksPanel.ItemsSource = temp.Books;

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = textName.Text;
            DateTime date = (DateTime)datePicker.SelectedDate;
            string photo = temp.Photo;
            if(name == "")
            {
                MessageBox.Show("Введите имя автора");
                return;
            }
            if (hasAuthor)
            {
                for (int i = 0; i < _base.authors.Count; i++)
                {
                    if (_base.authors[i].Name == name && _base.authors[i].Name != _author.Name)
                    {
                        MessageBox.Show("Автор с данным именем уже есть в коллекции");
                        return;
                    }
                }
                _author.Name = name;
                _author.Date = date;
                _author.Photo = photo;
                _author.Books = temp.Books;

                DialogResult = true;
            }
            else
            {
                if (_base.FindAuthor(textName.Text) != -1)
                {
                    MessageBox.Show("Автор с данным именем уже есть в коллекции");
                    return;
                }
                temp.Name = name;
                temp.Date = date;
                temp.Photo = photo;
                _base.AddAuthor(temp);

                DialogResult = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string name = (string)((Label)sender).Content;
            for (int i = 0; i < temp.Books.Count; i++)
            {
                if(temp.Books[i].Name == name)
                {
                    temp.Books.Remove(temp.Books[i]);
                    return;
                }
            }
        }

        private void booksComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string name = ((ComboBox)sender).SelectedItem?.ToString();
            for (int i = 0; i < temp.Books.Count; i++)
            {
                if (temp.Books[i].Name == name)
                    return;
            }
            for (int i = 0; i < _base.books.Count; i++)
                if( _base.books[i].Name == name )
                {
                    temp.Books.Add(_base.books[i]);
                    return;
                }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog(); 
            open.Filter = "Image files (*.BMP, *.JPG, *.PNG)|*.bmp;*.jpg;*.png";
            if (open.ShowDialog() == true)
                temp.Photo = open.FileName;
        }

        
    }
}
