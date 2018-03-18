using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using Library;

namespace Lab2
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((DateTime)value).ToString("dd.MM.yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date;
            bool isOk = DateTime.TryParse((string)value, out date);
            return isOk ? date : default(DateTime);
        }
    }
    public class HouseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((PublishmentHouse)value)?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }


    public class BookCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ItemCollection<Book> coll = (ItemCollection<Book>)value;
            if (coll.Count == 0)
                return null;
            string ans = "";
            foreach (Book item in coll)
                ans += item.ToString() + ", ";
            ans = ans.Remove(ans.Count() - 2);
            return ans;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class AuthorCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ItemCollection<Author> coll = (ItemCollection<Author>)value;
            if (coll.Count == 0)
                return null;
            string ans = "";
            foreach (Author item in coll)
                ans += item.ToString() + ", ";
            ans = ans.Remove(ans.Count() - 2);
            return ans;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class StringCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ItemCollection<TagClass> coll = (ItemCollection<TagClass>)value;
            if (coll.Count == 0)
                return null;
            string ans = "";
            foreach (TagClass item in coll)
                ans += item.ToString() + ", ";
            ans = ans.Remove(ans.Count() - 2);
            return ans;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
