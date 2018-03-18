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
    /// Логика взаимодействия для AddTagWindow.xaml
    /// </summary>
    public partial class AddTagWindow : Window
    {
        ItemCollection<TagClass> temp;
        ProjectBase _base;
        public AddTagWindow(ProjectBase myBase)
        {
            InitializeComponent();
            _base = myBase;
            temp = new ItemCollection<TagClass>();
            foreach (TagClass tag in myBase.tags)
                temp.Add(tag);
            tagsPanel.ItemsSource = temp;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string tag = tagTextBox.Text;
            if (tag == "")
            {
                MessageBox.Show("Введите тег");
                return;
            }
            temp.Add(new TagClass(tag));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            foreach (TagClass tag in temp)
                _base.tags.Add(tag);
            DialogResult = true;
        }
    }
}
