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
using Microsoft.Win32;

namespace Lab2
{
    /// <summary>
    /// Логика взаимодействия для AddBookWindow.xaml
    /// </summary>
    public partial class AddBookWindow : Window
    {
        ProjectBase _base;
        bool hasBook;
        Book temp, _book;   
        public AddBookWindow(Book book, ProjectBase myBase)
        {
            InitializeComponent();
            _base = myBase;
            _book = book;
            if (book == null)
            {
                temp = new Book(null, 0, 0, 0);
                temp.Authors = new ItemCollection<Author>();
                hasBook = false;
            }
            else
            {
                temp = new Book(book.Name, book.ISBN, book.PageNumber, book.Year);
                temp.House = book.House;
                temp.Authors = new ItemCollection<Author>();
                foreach (Author author in book.Authors)
                    temp.Authors.Add(author);
                temp.Tags = new ItemCollection<TagClass>();
                foreach (TagClass tag in book.Tags)
                    temp.Tags.Add(tag);
                hasBook = true;
            }
            this.Resources.Add("Book", temp);

            SetResourceReference(DataContextProperty, "Book");

            housesComboBox.ItemsSource = _base.houses;
            authorsComboBox.ItemsSource = _base.authors;
            authorsPanel.ItemsSource = temp.Authors;
            tagsComboBox.ItemsSource = _base.tags;
            tagsPanel.ItemsSource = temp.Tags;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void housesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            temp.House = (PublishmentHouse)((ComboBox)sender).SelectedItem;
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string name = (string)((Label)sender).Content;
            for (int i = 0; i < temp.Authors.Count; i++)
            {
                if (temp.Authors[i].Name == name)
                {
                    temp.Authors.Remove(temp.Authors[i]);
                    return;
                }
            }
        }

        private void authorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string name = ((ComboBox)sender).SelectedItem?.ToString();
            for (int i = 0; i < temp.Authors.Count; i++)
            {
                if (temp.Authors[i].Name == name)
                    return;
            }
            for (int i = 0; i < _base.authors.Count; i++)
                if (_base.authors[i].Name == name)
                {
                    temp.Authors.Add(_base.authors[i]);
                    return;
                }
        }

        private void tagsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string tag = ((ComboBox)sender).SelectedItem?.ToString();
            for (int i = 0; i < temp.Tags.Count; i++)
            {
                if (temp.Tags[i].Tag == tag)
                    return;
            }
            for (int i = 0; i < _base.tags.Count; i++)
                if (_base.tags[i].Tag == tag)
                {
                    temp.Tags.Add(_base.tags[i]);
                    return;
                }
        }

        private void Label_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            string tag = (string)((Label)sender).Content;
            for (int i = 0; i < temp.Tags.Count; i++)
            {
                if (temp.Tags[i].Tag == tag)
                {
                    temp.Tags.Remove(temp.Tags[i]);
                    return;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int isbn, year, page;
            bool isCorrect;
            string name;
            isCorrect = int.TryParse(bookISBN.Text, out isbn);
            if (!isCorrect)
            {
                MessageBox.Show("Введите номер книги корректно");
                return;
            }
            name = bookName.Text;
            if(name == "")
            {
                MessageBox.Show("Название книги не может быть пустым");
                return;
            }
            isCorrect = int.TryParse(bookYear.Text, out year);
            if (!isCorrect)
            {
                MessageBox.Show("Введите год издания книги корректно");
                return;
            }
            isCorrect = int.TryParse(bookPage.Text, out page);
            if (!isCorrect)
            {
                MessageBox.Show("Введите кол-во страниц книги корректно");
                return;
            }

            if (hasBook)
            {
                for (int i = 0; i < _base.books.Count; i++)
                {
                    if (_base.books[i].ISBN == isbn && _base.books[i].ISBN != _book.ISBN)
                    {
                        MessageBox.Show("Книга с данным номером уже есть в коллекции");
                        return;
                    }
                }
                _book.Name = name;
                _book.ISBN = isbn;
                _book.PageNumber = page;
                _book.Year = year;
                _book.Authors = temp.Authors;
                _book.Tags = temp.Tags;
                _base.ChangePublishmentHouse(_book.ISBN, temp.House?.ToString());
                DialogResult = true;
            }
            else
            {
                if (_base.FindBook(isbn) != -1)
                {
                    MessageBox.Show("Книга с таким номером уже есть в коллекции");
                    return;
                }
                temp.Name = name;
                temp.ISBN = isbn;
                temp.PageNumber = page;
                temp.Year = year;
                _base.AddBook(temp);
                _base.ChangePublishmentHouse(temp.ISBN, temp.House?.ToString());
                DialogResult = true;
            }
        }
    }
}
