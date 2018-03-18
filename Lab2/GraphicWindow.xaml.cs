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
    /// Логика взаимодействия для GraphicWindow.xaml
    /// </summary>
    public partial class GraphicWindow : Window
    {
        public GraphicWindow(ProjectBase myBase)
        {
            InitializeComponent();
            ItemCollection<Author> authors = myBase.authors;
            if (authors.Count == 0)
                return;
            int max = authors.Max(x => x.Books.Count);
            for (int i = 1; i <= authors.Count; i++)
            {
                Line ln = new Line();
                ln.X1 = 50 + 1.0 * 530 * i / authors.Count;
                ln.X2 = 50 + 1.0 * 530 * i / authors.Count;
                ln.Y1 = 425;
                ln.Y2 = 435;
                ln.Stroke = Brushes.Black;
                field.Children.Add(ln);
                RotateTransform r = new RotateTransform() { Angle = 25 };
                TextBlock t = new TextBlock() { RenderTransform = r };
                t.Foreground = Brushes.BlueViolet;
                t.Text = authors[i - 1].Name;
                t.Margin = new Thickness(30 + 1.0 * 530 * i / authors.Count, 445, 0, 0);
                field.Children.Add(t);
            }

            for (int i = 1; i <= max; i++)
            {
                Line ln = new Line();
                ln.Y1 = 430 - 1.0 * 350 * i / authors.Count;
                ln.Y2 = 430 - 1.0 * 350 * i / authors.Count;
                ln.X1 = 45;
                ln.X2 = 55;
                ln.Stroke = Brushes.Black;
                field.Children.Add(ln);
                TextBlock t = new TextBlock();
                t.Text = i.ToString();
                t.Margin = new Thickness(35, 420 - 1.0 * 350 * i / authors.Count, 0, 0);
                field.Children.Add(t);
            }

            Polyline pl = new Polyline()
            {
                Stroke = Brushes.Red,
                StrokeThickness = 3,
            };
            Panel.SetZIndex(pl, 1);
            for (int i = 1; i <= authors.Count; i++)
            {
                pl.Points.Add(new Point(50 + 1.0 * 530 * i / authors.Count, 430 - 350.0 * authors[i - 1].Books.Count / authors.Count));
            }
            for (int i = 1; i <= authors.Count; i++)
            {
                Ellipse el = new Ellipse();
                el.Fill = Brushes.Blue;
                el.Width = 10;
                el.Height = 10;
                Panel.SetZIndex(el, 2);
                el.Margin = new Thickness(45 + 1.0 * 530 * i / authors.Count, 425 - 350.0 * authors[i - 1].Books.Count / authors.Count, 0, 0);
                field.Children.Add(el);
            }
            field.Children.Add(pl);
        }
    }
}
