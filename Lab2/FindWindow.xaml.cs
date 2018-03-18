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
    /// Логика взаимодействия для FindWindow.xaml
    /// </summary>
    public partial class FindWindow : Window
    {
        BindingElement<string> elem;
        public FindWindow(BindingElement<string> _elem)
        {
            InitializeComponent();
            elem = _elem;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            elem.Item = findTextBox.Text;
            DialogResult = true;
        }
    }
}
