using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lab2
{
    public class Commands
    {
        static Commands()
        {
            Save = new RoutedCommand("Save", typeof(MainWindow));
            Open = new RoutedCommand("Open", typeof(MainWindow));
            Find = new RoutedCommand("Find", typeof(MainWindow));
            Close = new RoutedCommand("Close", typeof(MainWindow));
        }

        public static RoutedCommand Save { get; set; }
        public static RoutedCommand Open { get; set; }
        public static RoutedCommand Find { get; set; }
        public static RoutedCommand Close { get; set; }
    }
}
