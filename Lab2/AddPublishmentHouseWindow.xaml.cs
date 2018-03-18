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
    /// Логика взаимодействия для AddPublishmentHouse.xaml
    /// </summary>
    public partial class AddPublishmentHouseWindow : Window
    {
        PublishmentHouse temp, _house;
        bool hasHouse;
        ProjectBase _base;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = textName.Text;
            string town = townName.Text;
            if (name == "")
            {
                MessageBox.Show("Введите название издательства");
                return;
            }
            if (town == "")
            {
                MessageBox.Show("Введите город издательства");
                return;
            }

            if (hasHouse)
            {
                for (int i = 0; i < _base.houses.Count; i++)
                {
                    if (_base.houses[i].Name == name && _base.houses[i].Name != _house.Name)
                    {
                        MessageBox.Show("Издательство с данным именем уже есть в коллекции");
                        return;
                    }
                }
                _house.Name = name;
                _house.Town = town;
                DialogResult = true;
            }
            else
            {
                if (_base.FindAuthor(textName.Text) != -1)
                {
                    MessageBox.Show("Издательство с данным именем уже есть в коллекции");
                    return;
                }
                temp.Name = name;
                temp.Town = town;
                _base.houses.Add(temp);
                DialogResult = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public AddPublishmentHouseWindow(PublishmentHouse house, ProjectBase myBase)
        {
            InitializeComponent();
            _base = myBase;
            _house = house;
            if (house == null)
            {
                temp = new PublishmentHouse(null, null);
                temp.Books = new ItemCollection<Book>();
                hasHouse = false;
            }
            else
            {
                temp = new PublishmentHouse(house.Name, house.Town);
                temp.Books = house.Books;
                hasHouse = true;
            }

            this.Resources.Add("House", temp);
            SetResourceReference(DataContextProperty, "House");
            booksPanel.ItemsSource = temp.Books;
        }
    }
}
