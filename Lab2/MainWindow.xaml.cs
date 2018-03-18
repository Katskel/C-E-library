using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using Library;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Configuration;
using System.Threading;
using System.Resources;
using System.Windows.Media.Animation;

namespace Lab2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public ProjectBase myBase;
        public Book SourceBook;
        public PublishmentHouse SourceHouse;
        string curPath;
        List<PluginInterface.IPlugin<int>> IntPlugins = new List<PluginInterface.IPlugin<int>>();
        List<PluginInterface.IPlugin<Book>> BookPlugins = new List<PluginInterface.IPlugin<Book>>();
        List<PluginInterface.IPlugin<Author>> AuthorPlugins = new List<PluginInterface.IPlugin<Author>>();
        List<PluginInterface.IPlugin<PublishmentHouse>> HousePlugins = new List<PluginInterface.IPlugin<PublishmentHouse>>();
        ItemCollection<Book> bookNextPageStorage = new ItemCollection<Book>();
        ItemCollection<Book> bookPrevPageStorage = new ItemCollection<Book>();
        ItemCollection<Book> bookFirstPageStorage = new ItemCollection<Book>();
        ItemCollection<Book> bookLastPageStorage = new ItemCollection<Book>();
        ItemCollection<Book> bookCurrentPageStorage = new ItemCollection<Book>();
        ItemCollection<Author> authorNextPageStorage = new ItemCollection<Author>();
        ItemCollection<Author> authorPrevPageStorage = new ItemCollection<Author>();
        ItemCollection<Author> authorFirstPageStorage = new ItemCollection<Author>();
        ItemCollection<Author> authorLastPageStorage = new ItemCollection<Author>();
        ItemCollection<Author> authorCurrentPageStorage = new ItemCollection<Author>();
        ItemCollection<PublishmentHouse> houseNextPageStorage = new ItemCollection<PublishmentHouse>();
        ItemCollection<PublishmentHouse> housePrevPageStorage = new ItemCollection<PublishmentHouse>();
        ItemCollection<PublishmentHouse> houseLastPageStorage = new ItemCollection<PublishmentHouse>();
        ItemCollection<PublishmentHouse> houseFirstPageStorage = new ItemCollection<PublishmentHouse>();
        ItemCollection<PublishmentHouse> houseCurrentPageStorage = new ItemCollection<PublishmentHouse>();
        ItemCollection<Book> curBooksStorage = new ItemCollection<Book>();
        ItemCollection<Author> curAuthorsStorage = new ItemCollection<Author>();
        ItemCollection<PublishmentHouse> curHousesStorage = new ItemCollection<PublishmentHouse>();
        BindingElement<int> currentBookPage = new BindingElement<int>(1);
        ItemCollection<Author> authorPageStorage = new ItemCollection<Author>();
        BindingElement<int> currentAuthorPage = new BindingElement<int>(1);
        ItemCollection<PublishmentHouse> housePageStorage = new ItemCollection<PublishmentHouse>();
        BindingElement<int> currentHousePage = new BindingElement<int>(1);
        int itemsPerPage = 2;

        public MainWindow()
        {
            InitializeComponent();
            myBase = new ProjectBase();
            curPath = null;
            CommandBinding cm = new CommandBinding();
            SetCommands();
            LoadPluginsAsync();
            GetResources();
            SourceBook = new Book(null, 0, 0, 0);
            SourceHouse = new PublishmentHouse(null, null);
            this.Resources.Add("SourceBook", SourceBook);
            this.Resources.Add("SourceHouse", SourceHouse);
            this.Resources.Add("SourceBookPage", currentBookPage);
            this.Resources.Add("SourceHousePage", currentHousePage);
            this.Resources.Add("SourceAuthorPage", currentAuthorPage);
            housesGrid.SetResourceReference(DataContextProperty, "SourceHouse");
            bookGrid.SetResourceReference(DataContextProperty, "SourceBook");
            bookPageGrid.SetResourceReference(DataContextProperty, "SourceBookPage");
            housePageGrid.SetResourceReference(DataContextProperty, "SourceHousePage");
            authorPageGrid.SetResourceReference(DataContextProperty, "SourceAuthorPage");
            booksCollection.ItemsSource = myBase.books;
            housesCollection.ItemsSource = myBase.houses;
            authorsCollection.ItemsSource = myBase.authors;
        }

        
        
        private async void LoadPluginsAsync()
        {
            string PluginsDirectory = ConfigurationManager.AppSettings["PluginsDirectory"];
            
            IEnumerable<string> assemblies;
            try
            {
                assemblies = Directory.EnumerateFiles(System.IO.Path.Combine(Environment.CurrentDirectory, PluginsDirectory), "*.dll");
            }
            catch
            {
                return;
            }
            List<Task> tasks = new List<Task>();
            Parallel.ForEach(assemblies, new Action<string>(path => tasks.Add(LoadItem(path))));
            await Task.WhenAll(tasks);
            
            if (IntPlugins.Count() + BookPlugins.Count() + AuthorPlugins.Count() + HousePlugins.Count() == 0)
                return;
            MenuItem plugins = new MenuItem();
            plugins.Header = "Плагины";
            plugins.Height = 25;
            foreach (PluginInterface.IPlugin<Book> item in BookPlugins)
            {
                MenuItem plug = new MenuItem();
                plug.Header = item.Name;
                plug.Height = 40;
                plug.Click += (object sender, RoutedEventArgs e) => TransformBooks(item.Method(myBase.books));
                plugins.Items.Add(plug);
            }
            foreach (PluginInterface.IPlugin<Author> item in AuthorPlugins)
            {
                MenuItem plug = new MenuItem();
                plug.Header = item.Name;
                plug.Height = 40;
                plug.Click += (object sender, RoutedEventArgs e) => TransformAuthors(item.Method(myBase.authors));
                plugins.Items.Add(plug);
            }
            foreach (PluginInterface.IPlugin<PublishmentHouse> item in HousePlugins)
            {
                MenuItem plug = new MenuItem();
                plug.Header = item.Name;
                plug.Height = 40;
                plug.Click += (object sender, RoutedEventArgs e) => TransformHouses(item.Method(myBase.houses));
                plugins.Items.Add(plug);
            }
            foreach (PluginInterface.IPlugin<int> item in IntPlugins)
            {
                MenuItem plug = new MenuItem();
                plug.Header = item.Name;
                plug.Height = 40;
                plug.Click += (object sender, RoutedEventArgs e) => item.Method(default(IEnumerable<int>));
                plugins.Items.Add(plug);
            }
            MainMenu.Items.Add(plugins);
        }

        private Task LoadItem(string path)
        {
            return Task.Run(() =>
            {
                Thread.Sleep(5000);
                var assembly = Assembly.LoadFile(path);
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    var inters = type.GetInterfaces();
                    object instance = type.Assembly.CreateInstance(type.FullName);
                    if (inters.Contains(typeof(PluginInterface.IPlugin<int>)))
                    {
                        PluginInterface.IPlugin<int> inter = (PluginInterface.IPlugin<int>)instance;
                        IntPlugins.Add(inter);
                    }
                    if (inters.Contains(typeof(PluginInterface.IPlugin<Book>)))
                    {
                        PluginInterface.IPlugin<Book> inter = (PluginInterface.IPlugin<Book>)instance;
                        BookPlugins.Add(inter);
                    }
                    if (inters.Contains(typeof(PluginInterface.IPlugin<Author>)))
                    {
                        PluginInterface.IPlugin<Author> inter = (PluginInterface.IPlugin<Author>)instance;
                        AuthorPlugins.Add(inter);
                    }
                    if (inters.Contains(typeof(PluginInterface.IPlugin<PublishmentHouse>)))
                    {
                        PluginInterface.IPlugin<PublishmentHouse> inter = (PluginInterface.IPlugin<PublishmentHouse>)instance;
                        HousePlugins.Add(inter);
                    }
                }
            }); 
        }

        private async Task<ItemCollection<T>> SetPage<T>(ItemCollection<T> source, int pageNumber, int itemsPerPage) where T: IEquatable<T>
        {
            return await Task.Run(() =>
            {
                if (source.Count() == 0)
                    return new ItemCollection<T>();
            int prevItemsCount = (pageNumber - 1) * itemsPerPage;
            if (itemsPerPage * pageNumber > source.Count())
                return Enumerable.Range(prevItemsCount + 1, source.Count() - prevItemsCount).AsParallel().AsOrdered().Select(i => source[i - 1]).ToList().ToItemCollection();

            return Enumerable.Range(prevItemsCount + 1, itemsPerPage).AsParallel().AsOrdered().Select(i => source[i - 1]).ToList().ToItemCollection();
            });
        }
        

        /*private void FindPlugins()
        {
            var collection = Directory.EnumerateFiles(System.IO.Path.Combine(Environment.CurrentDirectory, "Plugins"), "*.dll");
            
            foreach(string path in collection)
            {
                var assembly = Assembly.LoadFile(path);
                var types = assembly.GetTypes();
                foreach(var type in types)  
                {
                    var inters = type.GetInterfaces();
                    object instance = type.Assembly.CreateInstance(type.FullName);
                    if(inters.Contains(typeof(PluginInterface.IPlugin<int>)))
                    {
                        PluginInterface.IPlugin<int> inter = (PluginInterface.IPlugin<int>)instance;
                        MenuItem newplugin = new MenuItem();
                        newplugin.Header = inter.Name;
                        newplugin.Click += (object sender, RoutedEventArgs e) => inter.Method(default(IEnumerable<int>));
                        pluginsMenuItem.Items.Add(newplugin);
                    }
                    if(inters.Contains(typeof(PluginInterface.IPlugin<Book>)))
                    {
                        PluginInterface.IPlugin<Book> inter = (PluginInterface.IPlugin<Book>)instance;
                        MenuItem newplugin = new MenuItem();
                        newplugin.Header = inter.Name;
                        newplugin.Click += (object sender, RoutedEventArgs e) => TransformBooks(inter.Method(myBase.books));
                        pluginsMenuItem.Items.Add(newplugin);
                    }
                    if(inters.Contains(typeof(PluginInterface.IPlugin<Author>)))
                    {
                        PluginInterface.IPlugin<Author> inter = (PluginInterface.IPlugin<Author>)instance;
                        MenuItem newPlugin = new MenuItem();
                        newPlugin.Header = inter.Name;
                        newPlugin.Click += (object sender, RoutedEventArgs e) => TransformAuthors(inter.Method(myBase.authors));
                        pluginsMenuItem.Items.Add(newPlugin);
                    }
                    if (inters.Contains(typeof(PluginInterface.IPlugin<PublishmentHouse>)))
                    {
                        PluginInterface.IPlugin<PublishmentHouse> inter = (PluginInterface.IPlugin<PublishmentHouse>)instance;
                        MenuItem newPlugin = new MenuItem();
                        newPlugin.Header = inter.Name;
                        newPlugin.Click += (object sender, RoutedEventArgs e) => TransformHouses(inter.Method(myBase.houses));
                        pluginsMenuItem.Items.Add(newPlugin);
                    }
                }
            }
        }*/

        private void TransformBooks(IEnumerable<Book> books)
        {
            LINQWindow window = new LINQWindow(books, typeof(Book));
            window.ShowDialog();
        }

        private void TransformAuthors(IEnumerable<Author> authors)
        {
            LINQWindow window = new LINQWindow(authors, typeof(Author));
            window.ShowDialog();
        }

        private void TransformHouses(IEnumerable<PublishmentHouse> houses)
        {
            LINQWindow window = new LINQWindow(houses, typeof(PublishmentHouse));
            window.ShowDialog();
        }

        private void booksCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox list = (ListBox)sender;
            if (list.SelectedItem == null)
                return;
            foreach(Book book in myBase.books)
            {
                if( book.Name == list.SelectedItem.ToString() )
                {
                    SourceBook.Name = book.Name;
                    SourceBook.Year = book.Year;
                    SourceBook.PageNumber = book.PageNumber;
                    SourceBook.House = book.House;
                    SourceBook.Authors = book.Authors;
                    SourceBook.Tags = book.Tags;
                    break;
                }
            } 
        }
        

        private void housesCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox list = (ListBox)sender;
            if (list.SelectedItem == null)
                return;
            foreach(PublishmentHouse house in myBase.houses)
            {
                if( house.Name == list.SelectedItem.ToString() )
                {
                    SourceHouse.Name = house.Name;
                    SourceHouse.Town = house.Town;
                    SourceHouse.Books = house.Books;
                    break;
                }
            }
        }


        private void authorsCollection_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Author item = ((ListBox)sender).SelectedItem as Author;
            if (item == null)
                return;
            AddAuthorWindow window = new AddAuthorWindow(item, myBase);
            window.ShowDialog();
        }

        private void housesCollection_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PublishmentHouse item = ((ListBox)sender).SelectedItem as PublishmentHouse;
            if (item == null)
                return;
            AddPublishmentHouseWindow window = new AddPublishmentHouseWindow(item, myBase);
            window.ShowDialog();
            PublishmentHouse temp = (sender as ListBox).SelectedItem as PublishmentHouse;
            if (temp == null)
                return;
            SourceHouse.Name = temp.Name;
            SourceHouse.Town = temp.Town;
            SourceHouse.Books = temp.Books;
        }

        private void booksCollection_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Book item = ((ListBox)sender).SelectedItem as Book;
            if (item == null)
                return;
            AddBookWindow window = new AddBookWindow(item, myBase);
            window.ShowDialog();
            Book temp = (sender as ListBox).SelectedItem as Book;
            if (temp == null)
                return;
            SourceBook.Name = temp.Name;
            SourceBook.Year = temp.Year;
            SourceBook.PageNumber = temp.PageNumber;
            SourceBook.House = temp.House;
            SourceBook.Authors = temp.Authors;
            SourceBook.Tags = temp.Tags;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow window = new AddBookWindow(null, myBase);
            window.ShowDialog();
            UpdateData();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            AddAuthorWindow window = new AddAuthorWindow(null, myBase);
            window.ShowDialog();
            UpdateData();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            AddPublishmentHouseWindow window = new AddPublishmentHouseWindow(null, myBase);
            window.ShowDialog();
            UpdateData();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            AddTagWindow window = new AddTagWindow(myBase);
            window.ShowDialog();
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Bin files(*.bin)|*.bin";
            save.InitialDirectory = Directory.GetCurrentDirectory() + "\\Libraries";
            if (save.ShowDialog() == false)
                return;
            SaveToFile(save.FileName);
            curPath = save.FileName;
        }



        public void SaveToFile(string FilePath)
        {
            using (Stream s = File.Create(FilePath))
            {
                using (var ds = new DeflateStream(s, CompressionMode.Compress))
                {
                    using (TextWriter w = new StreamWriter(ds))
                    {
                        w.WriteLine("<houses>");
                        foreach (PublishmentHouse house in myBase.houses)
                        {
                            w.WriteLine(house.Name);
                            w.WriteLine(house.Town);
                        }
                        w.WriteLine("</houses>");
                        w.WriteLine("<authors>");
                        foreach (Author author in myBase.authors)
                        {
                            w.WriteLine(author.Name);
                            w.WriteLine(author.Date.Day + "." + author.Date.Month + "." + author.Date.Year);
                            if (author.Photo == "" || author.Photo == null)
                                w.WriteLine("null");
                            else
                                w.WriteLine(author.Photo);
                        }
                        w.WriteLine("</authors>");
                        w.WriteLine("<books>");
                        foreach (Book book in myBase.books)
                        {
                            w.WriteLine("<book>");
                            w.WriteLine(book.ISBN);
                            w.WriteLine(book.Name);
                            w.WriteLine(book.PageNumber);
                            w.WriteLine(book.Year);
                            if (book.House == null)
                                w.WriteLine("null");
                            else
                                w.WriteLine(book.House);
                            foreach (TagClass tag in book.Tags)
                                w.WriteLine(tag.ToString());
                            w.WriteLine("</book>");
                        }
                        w.WriteLine("</books>");
                        w.WriteLine("<pairs>");
                        foreach (Book book in myBase.books)
                        {
                            foreach (Author author in book.Authors)
                            {
                                w.WriteLine(book.ISBN);
                                w.WriteLine(author.Name);
                            }
                        }
                        w.WriteLine("</pairs>");
                    }
                }
            }
        }

        public void LoadFromFile(string FilePath)
        {
            myBase = new ProjectBase();
            using (Stream s = File.OpenRead(FilePath))
            {
                using (var ds = new DeflateStream(s, CompressionMode.Decompress))
                {
                    using (TextReader t = new StreamReader(ds))
                    {
                        string name, town, str, photo;
                        DateTime date;
                        int isbn, page, year;
                        while ((str = t.ReadLine()) != null)
                        {
                            if (str == "<houses>")
                            {
                                str = t.ReadLine();
                                while (str != "</houses>")
                                {
                                    name = str;
                                    town = t.ReadLine();
                                    myBase.AddPublishmentHouse(new PublishmentHouse(name, town));
                                    str = t.ReadLine();
                                }
                            }
                            str = t.ReadLine();
                            if (str == "<authors>")
                            {
                                str = t.ReadLine();
                                while (str != "</authors>")
                                {
                                    name = str;
                                    date = DateTime.Parse(t.ReadLine());
                                    photo = t.ReadLine();
                                    if (photo == "null")
                                        photo = null;
                                    myBase.AddAuthor(new Author(name, date, photo));
                                    str = t.ReadLine();
                                }
                            }
                            str = t.ReadLine();

                            if(str == "<books>")
                            {
                                while (str != "</books>")
                                {
                                    str = t.ReadLine();
                                    if (str == "<book>")
                                    {
                                        isbn = int.Parse(t.ReadLine());
                                        name = t.ReadLine();
                                        page = int.Parse(t.ReadLine());
                                        year = int.Parse(t.ReadLine());
                                        Book book = new Book(name, isbn, page, year);
                                        myBase.AddBook(book);
                                        if ((str = t.ReadLine()) == "null")
                                            book.House = null;
                                        else
                                            myBase.ChangePublishmentHouse(isbn, str);
                                        str = t.ReadLine();
                                        while (str != "</book>")
                                        {
                                            myBase.tags.Add(new TagClass(str));
                                            myBase.AddTag(isbn, str);
                                            str = t.ReadLine();
                                        }
                                    }
                                }
                            }

                            str = t.ReadLine();
                            if (str == "<pairs>")
                            {
                                str = t.ReadLine();
                                while (str != "</pairs>")
                                {
                                    isbn = int.Parse(str);
                                    name = t.ReadLine();
                                    myBase.AddPair(isbn, name);
                                    str = t.ReadLine();
                                }
                            }
                            str = t.ReadLine();
                        }
                    }
                }
            }
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            int result = CheckForSave();
            if (result == 1)
            {
                if (curPath == null)
                    MenuItem_Click_5(sender, e);
                else
                    SaveToFile(curPath);
            }
            else if (result == 0)
                return;
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Bin files(*.bin)|*.bin";
            open.InitialDirectory = Directory.GetCurrentDirectory() + "\\Libraries";
            if (open.ShowDialog() == false)
                return;
            LoadFromFile(open.FileName);
            currentBookPage.Item = 1;
            currentAuthorPage.Item = 1;
            currentHousePage.Item = 1;
            curPath = open.FileName;
            MenuItem_Click_21(sender, e);
            UpdateData();   
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            int result = CheckForSave();
            if (result == 1)
            {
                if (curPath == null)
                    MenuItem_Click_5(sender, e);
                else
                    SaveToFile(curPath);
            }
            else if (result == 0)
                return;
            myBase = new ProjectBase();
            currentBookPage.Item = 1;
            currentAuthorPage.Item = 1;
            currentHousePage.Item = 1;
            curPath = null;
            UpdateData();
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            if (curPath == null)
                MenuItem_Click_5(sender, e);
            else
                SaveToFile(curPath);
        }

        private int CheckForSave()
        {
            string message = "Вы хотите сохранить изменения в текущем файле?";
            var result = MessageBox.Show(message, "Библиотека", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                return 1;
            if (result == MessageBoxResult.No)
                return -1;
            return 0;
        }

        private int CheckForDelete()
        {
            string message = "Вы действительно хотите удалить элемент?";
            var result = MessageBox.Show(message, "Библиотека", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
                return 1;
            if (result == MessageBoxResult.No)
                return -1;
            return 0;
        }


        private void MenuItem_Click_10(object sender, RoutedEventArgs e)
        {
            myBase.books = curBooksStorage.AsParallel().OrderBy(book => book.ISBN).ToList().ToItemCollection() ;
            UpdateData();
        }


        private async void UpdateData()
        {
            bookCurrentPageStorage = await SetPage(curBooksStorage, currentBookPage.Item, itemsPerPage);
            booksCollection.ItemsSource = bookCurrentPageStorage;
            authorCurrentPageStorage = await SetPage(curAuthorsStorage, currentAuthorPage.Item, itemsPerPage);
            authorsCollection.ItemsSource = authorCurrentPageStorage;
            houseCurrentPageStorage = await SetPage(curHousesStorage, currentHousePage.Item, itemsPerPage);
            housesCollection.ItemsSource = houseCurrentPageStorage;
            LoadBookPagesAsync();
            LoadAuthorPagesAsync();
            LoadHousePagesAsync();
            SourceBook.Name = null;
            SourceBook.ISBN = 0;
            SourceBook.Year = 0;
            SourceBook.PageNumber = 0;
            SourceBook.House = null;
            SourceBook.Authors = new ItemCollection<Author>();
            SourceBook.Tags = new ItemCollection<TagClass>();
            SourceHouse.Name = null;
            SourceHouse.Town = null;
            SourceHouse.Books = new ItemCollection<Book>();
        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            myBase.authors = curAuthorsStorage.AsParallel().OrderBy(author => author.Name).ToList().ToItemCollection();
            UpdateData();
        }

        private void MenuItem_Click_11(object sender, RoutedEventArgs e)
        {
            myBase.houses = curHousesStorage.AsParallel().OrderBy(house => house.Name).ToList().ToItemCollection();
            UpdateData();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            int result = CheckForSave();
            if (result == 1)
            {
                if (curPath == null)
                    MenuItem_Click_5(sender, new RoutedEventArgs());
                else
                    SaveToFile(curPath);
            }
            else if (result == 0)
            {
                e.Cancel = true;
                return;
            }
                
        }

        private void MenuItem_Click_12(object sender, RoutedEventArgs e)
        {
            booksCollection_MouseDoubleClick(booksCollection, null);
        }

        private void MenuItem_Click_13(object sender, RoutedEventArgs e)
        {
            Book item = booksCollection.SelectedItem as Book;
            if (item == null)
                return;
            if (CheckForDelete() != 1)
                return;
            myBase.DeleteBook(item);
            int maxPages = curBooksStorage.Count() / itemsPerPage + ((curBooksStorage.Count() % itemsPerPage == 0) ? 0 : 1);
            if (currentBookPage.Item == maxPages + 1 && maxPages != 0)
                currentBookPage.Item--;
            UpdateData();
        }

        private void MenuItem_Click_14(object sender, RoutedEventArgs e)
        {
            authorsCollection_MouseDoubleClick(authorsCollection, null);
        }

        private void MenuItem_Click_15(object sender, RoutedEventArgs e)
        {
            Author item = authorsCollection.SelectedItem as Author;
            if (item == null)
                return;
            if (CheckForDelete() != 1)
                return;
            myBase.DeleteAuthor(item.Name);
            int maxPages = curAuthorsStorage.Count() / itemsPerPage + ((curAuthorsStorage.Count() % itemsPerPage == 0) ? 0 : 1);
            if (currentAuthorPage.Item == maxPages + 1 && maxPages != 0)
                currentAuthorPage.Item--;
            UpdateData();
        }

        private void MenuItem_Click_16(object sender, RoutedEventArgs e)
        {
            housesCollection_MouseDoubleClick(housesCollection, null);
        }

        private void MenuItem_Click_17(object sender, RoutedEventArgs e)
        {
            PublishmentHouse item = housesCollection.SelectedItem as PublishmentHouse;
            if (item == null)
                return;
            if (CheckForDelete() != 1)
                return;
            myBase.DeletePublishmentHouse(item.Name);
            int maxPages = curHousesStorage.Count() / itemsPerPage + ((curHousesStorage.Count() % itemsPerPage == 0) ? 0 : 1);
            if (currentHousePage.Item == maxPages + 1 && maxPages != 0)
                currentHousePage.Item--;
            UpdateData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (myBase.books.Count == 0)
                return;
            int maxPages = curBooksStorage.Count() / itemsPerPage + ((curBooksStorage.Count() % itemsPerPage == 0) ? 0 : 1);
            if (currentBookPage.Item == maxPages)
                return;
            booksCollection.ItemsSource = bookNextPageStorage;
            currentBookPage.Item++;
            LoadBookPagesAsync();
        }

        private async void LoadBookPagesAsync()
        {
            int maxPages = curBooksStorage.Count() / itemsPerPage + ((curBooksStorage.Count() % itemsPerPage == 0) ? 0 : 1);
            if (currentBookPage.Item != maxPages)
                bookNextPageStorage = await SetPage(curBooksStorage, currentBookPage.Item + 1, itemsPerPage);
            if (currentBookPage.Item != 1)
                bookPrevPageStorage = await SetPage(curBooksStorage, currentBookPage.Item - 1, itemsPerPage);
            bookFirstPageStorage = await SetPage(curBooksStorage, 1, itemsPerPage);
            bookLastPageStorage = await SetPage(curBooksStorage, maxPages, itemsPerPage);
        }

        private async void LoadAuthorPagesAsync()
        {
            int maxPages = curAuthorsStorage.Count() / itemsPerPage + ((curAuthorsStorage.Count() % itemsPerPage == 0) ? 0 : 1);
            if (currentAuthorPage.Item != maxPages)
                authorNextPageStorage = await SetPage(curAuthorsStorage, currentAuthorPage.Item + 1, itemsPerPage);
            if (currentAuthorPage.Item != 1)
                authorPrevPageStorage = await SetPage(curAuthorsStorage, currentAuthorPage.Item - 1, itemsPerPage);
            authorFirstPageStorage = await SetPage(curAuthorsStorage, 1, itemsPerPage);
            authorLastPageStorage = await SetPage(curAuthorsStorage, maxPages, itemsPerPage);
        }

        private async void LoadHousePagesAsync()
        {
            int maxPages = curHousesStorage.Count() / itemsPerPage + ((curHousesStorage.Count() % itemsPerPage == 0) ? 0 : 1);
            if (currentHousePage.Item != maxPages)
                houseNextPageStorage = await SetPage(curHousesStorage, currentHousePage.Item + 1, itemsPerPage);
            if (currentHousePage.Item != 1)
                housePrevPageStorage = await SetPage(curHousesStorage, currentHousePage.Item - 1, itemsPerPage);
            houseFirstPageStorage = await SetPage(curHousesStorage, 1, itemsPerPage);
            houseLastPageStorage = await SetPage(curHousesStorage, maxPages, itemsPerPage);
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (myBase.books.Count == 0)
                return;
            int maxPages = curBooksStorage.Count() / itemsPerPage + ((curBooksStorage.Count() % itemsPerPage == 0) ? 0 : 1);
            if (currentBookPage.Item == maxPages)
                return;
            booksCollection.ItemsSource = bookLastPageStorage;
            currentBookPage.Item = maxPages;
            LoadBookPagesAsync();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (myBase.books.Count == 0)
                return;
            if (currentBookPage.Item == 1)
                return;
            booksCollection.ItemsSource = bookPrevPageStorage;
            currentBookPage.Item--;
            LoadBookPagesAsync();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (myBase.books.Count == 0)
                return;
            if (currentBookPage.Item == 1)
                return;
            booksCollection.ItemsSource = bookFirstPageStorage;
            currentBookPage.Item = 1;
            LoadBookPagesAsync();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            if (myBase.houses.Count == 0)
                return;
            if (currentHousePage.Item == 1)
                return;
            housesCollection.ItemsSource = houseFirstPageStorage;
            currentHousePage.Item = 1;
            LoadHousePagesAsync();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            if (myBase.houses.Count == 0)
                return;
            if (currentHousePage.Item == 1)
                return;
            housesCollection.ItemsSource = housePrevPageStorage;
            currentHousePage.Item--;
            LoadHousePagesAsync();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            if (myBase.houses.Count == 0)
                return;
            int maxPages = curHousesStorage.Count() / itemsPerPage + ((curHousesStorage.Count() % itemsPerPage == 0) ? 0 : 1);
            if (currentHousePage.Item == maxPages)
                return;
            housesCollection.ItemsSource = houseNextPageStorage;
            currentHousePage.Item++;
            LoadHousePagesAsync();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            if (myBase.houses.Count == 0)
                return;
            int maxPages = curHousesStorage.Count() / itemsPerPage + ((curHousesStorage.Count() % itemsPerPage == 0) ? 0 : 1);
            if (currentHousePage.Item == maxPages)
                return;
            housesCollection.ItemsSource = houseLastPageStorage;
            currentHousePage.Item = maxPages;
            LoadHousePagesAsync();
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            if (myBase.authors.Count == 0)
                return;
            if (currentAuthorPage.Item == 1)
                return;
            authorsCollection.ItemsSource = authorFirstPageStorage;
            currentAuthorPage.Item = 1;
            LoadAuthorPagesAsync();
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            if (myBase.authors.Count == 0)
                return;
            if (currentAuthorPage.Item == 1)
                return;
            authorsCollection.ItemsSource = authorPrevPageStorage;
            currentAuthorPage.Item--;
            LoadAuthorPagesAsync();
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            if (myBase.authors.Count == 0)
                return;
            int maxPages = curAuthorsStorage.Count() / itemsPerPage + ((curAuthorsStorage.Count() % itemsPerPage == 0) ? 0 : 1);
            if (currentAuthorPage.Item == maxPages)
                return;
            authorsCollection.ItemsSource = authorNextPageStorage;
            currentAuthorPage.Item++;
            LoadAuthorPagesAsync();
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            if (myBase.authors.Count == 0)
                return;
            int maxPages = curAuthorsStorage.Count() / itemsPerPage + ((curAuthorsStorage.Count() % itemsPerPage == 0) ? 0 : 1);
            if (currentAuthorPage.Item == maxPages)
                return;
            authorsCollection.ItemsSource = authorLastPageStorage;
            currentAuthorPage.Item = maxPages;
            LoadAuthorPagesAsync();
        }
        

        private void SetCommandBindings()
        {
            CommandBinding saveBinding = new CommandBinding();
            CommandBinding openBinding = new CommandBinding();
            CommandBinding findBinding = new CommandBinding();
            CommandBinding closeBinding = new CommandBinding();
            saveBinding.Command = Commands.Save;
            saveBinding.Executed += Save_Executed;
            openBinding.Command = Commands.Open;
            openBinding.Executed += Open_Executed;
            findBinding.Command = Commands.Find;
            findBinding.Executed += Find_Executed;
            closeBinding.Command = Commands.Close;
            closeBinding.Executed += Close_Executed;
            this.CommandBindings.Add(saveBinding);
            this.CommandBindings.Add(openBinding);
            this.CommandBindings.Add(findBinding);
            this.CommandBindings.Add(closeBinding);
        }

        private void SetCommands()
        {
            SetCommandBindings();
            HotKeySection customSection;
            try
            {
                customSection = (HotKeySection)ConfigurationManager.GetSection("HotKeySection");
                foreach (HotKeyElement element in customSection.HotKeys)
                {
                    try
                    {
                        RoutedCommand elementCmd = GetCommand(element.Command);
                        KeyGesture elementGesture = GetKeyGesture(element.Gesture);
                        this.InputBindings.Add(new KeyBinding(elementCmd, elementGesture));
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch
            {
                return;
            }
            
        }

        private RoutedCommand GetCommand(string command)
        {
            switch(command)
            {
                case "Save": return Commands.Save;
                case "Open": return Commands.Open;
                case "Find": return Commands.Find;
                case "Close": return Commands.Close;
            }
            throw new Exception();
        }

        private KeyGesture GetKeyGesture(string gesture)
        {
            switch(gesture)
            {
                case "CTRL+S": return new KeyGesture(Key.S, ModifierKeys.Control);
                case "CTRL+O": return new KeyGesture(Key.O, ModifierKeys.Control);
                case "CTRL+F": return new KeyGesture(Key.F, ModifierKeys.Control);
                case "CTRL+Q": return new KeyGesture(Key.Q, ModifierKeys.Control);
            }
            throw new Exception();
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MenuItem_Click_8(sender, e);
        }
        private void Open_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MenuItem_Click_6(sender, e);
        }
        private void Find_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MenuItem_Click_22(sender, e);
        }
        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MenuItem_Click_4(sender, e);
        }

        private void GetResources()
        {
            var fileMenuItems = (MainMenu.Items[0] as MenuItem).Items;
            (fileMenuItems[0] as MenuItem).Icon = GetIcon("NewLibraryIcon");
            (fileMenuItems[1] as MenuItem).Icon = GetIcon("OpenIcon");
            (fileMenuItems[2] as MenuItem).Icon = GetIcon("SaveIcon");
            (fileMenuItems[5] as MenuItem).Icon = GetIcon("ExitIcon");
            var editMenuItems = (MainMenu.Items[1] as MenuItem).Items;
            (editMenuItems[0] as MenuItem).Icon = GetIcon("AddIcon");
            (editMenuItems[1] as MenuItem).Icon = GetIcon("SortIcon");
            this.Icon = GetIcon("WindowIcon").Source;
            Application.Current.Resources.Add("WindowIcon", GetIcon("WindowIcon").Source);
        }

        private void SaveResources()
        {
            string path = @"d:\Work\ISP\3sem\pict\";
            using (ResXResourceWriter resx = new ResXResourceWriter("icons.resx"))
            {
                resx.AddResource("NewLibraryIcon", GetImageBuffer(path + "new.png"));
                resx.AddResource("OpenIcon", GetImageBuffer(path + "open.jpg"));
                resx.AddResource("SaveIcon", GetImageBuffer(path + "save.jpg"));
                resx.AddResource("ExitIcon", GetImageBuffer(path + "exit.png"));
                resx.AddResource("AddIcon", GetImageBuffer(path + "add.png"));
                resx.AddResource("SortIcon", GetImageBuffer(path + "sort.jpg"));
                resx.AddResource("WindowIcon", GetImageBuffer(path + "library.png"));
            } 
        }


        private byte[] GetImageBuffer(string path)
        {
            var source = new BitmapImage();
            source.BeginInit();
            source.StreamSource = new MemoryStream(File.ReadAllBytes(path));
            source.EndInit();

            byte[] image_buffer = null;
            using (var stream = new MemoryStream())
            {
                var encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(source));
                encoder.Save(stream);
                image_buffer = stream.ToArray();

            }
            return image_buffer;
        }

        private System.Windows.Controls.Image GetIcon(string resource_name)
        {
            byte[] image_buff = null;

            using (ResXResourceSet resxSet = new ResXResourceSet(@".\icons.resx"))
            {
                image_buff = (byte[])resxSet.GetObject(resource_name);
            }


            var icon = new System.Windows.Controls.Image();
            using (var stream = new MemoryStream(image_buff))
            {
                var decoder = BitmapDecoder.Create(stream,
                        BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                var image_source = decoder.Frames[0];
                icon.Source = image_source;
            }

            return icon;
        }


        private void MenuItem_Click_18(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml") });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml") });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Indigo.xaml") });
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.Lime.xaml") });
        }

        private void MenuItem_Click_19(object sender, RoutedEventArgs e)
        {
            Application.Current.Resources.MergedDictionaries.Clear();
        }

        private void tabItemClick(object sender, RoutedEventArgs e)
        {
            tabItem1.BeginAnimation(WidthProperty, GetAnim(tabItem1));
            tabItem2.BeginAnimation(WidthProperty, GetAnim(tabItem2));
            tabItem3.BeginAnimation(WidthProperty, GetAnim(tabItem3));
        }

        private DoubleAnimationUsingKeyFrames GetAnim (TabItem item)
        {
            double width = item.ActualWidth;
            DoubleAnimationUsingKeyFrames anim = new DoubleAnimationUsingKeyFrames()
            {
                Duration = new TimeSpan(0, 0, 3),
                AutoReverse = true,
                FillBehavior = FillBehavior.Stop

            };
            KeyTime a = KeyTime.Uniform;
            LinearDoubleKeyFrame fr1 = new LinearDoubleKeyFrame(width, a);
            LinearDoubleKeyFrame fr2 = new LinearDoubleKeyFrame(1.1 * width, a);
            LinearDoubleKeyFrame fr3 = new LinearDoubleKeyFrame(1.3 * width, a);
            LinearDoubleKeyFrame fr4 = new LinearDoubleKeyFrame(1.6 * width, a);
            LinearDoubleKeyFrame fr5 = new LinearDoubleKeyFrame(2 * width, a);
            anim.KeyFrames.Add(fr1);
            anim.KeyFrames.Add(fr2);
            anim.KeyFrames.Add(fr3);
            anim.KeyFrames.Add(fr4);
            anim.KeyFrames.Add(fr5);
            return anim;
        }

        private void MenuItem_Click_20(object sender, RoutedEventArgs e)
        {
            GraphicWindow graphic = new GraphicWindow(myBase);
            graphic.ShowDialog();
        }

        private void MenuItem_Click_21(object sender, RoutedEventArgs e)
        {
            curAuthorsStorage = myBase.authors;
            curBooksStorage = myBase.books;
            curHousesStorage = myBase.houses;
            UpdateData();
        }

        private void MenuItem_Click_22(object sender, RoutedEventArgs e)
        {
            BindingElement<string> item = new BindingElement<string>("");
            FindWindow find = new FindWindow(item);
            if (find.ShowDialog() == false)
                return;
            switch(mainTab.SelectedIndex)
            {
                case 0:
                    curBooksStorage = myBase.books.Where(x => x.Name.ToLower().Contains(item.Item.ToLower())).ToItemCollection();
                    currentBookPage.Item = 1;
                    break;
                case 1: 
                    curAuthorsStorage = myBase.authors.Where(x => x.Name.ToLower().Contains(item.Item.ToLower())).ToItemCollection();
                    currentAuthorPage.Item = 1;
                    break;
                case 2:
                    curHousesStorage = myBase.houses.Where(x => x.Name.ToLower().Contains(item.Item.ToLower())).ToItemCollection();
                    currentHousePage.Item = 1;
                    break;
            }
            UpdateData();
        }
    }
}
