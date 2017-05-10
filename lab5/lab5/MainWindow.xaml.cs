using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace lab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Item> items = new ObservableCollection<Item>();
        ObservableCollection<CartItem> cartItems = new ObservableCollection<CartItem>();
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            dg.ItemsSource = items;
            listBox.ItemsSource = new ListCollectionView(items);
            listBoxCart.ItemsSource = new ListCollectionView(cartItems);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            items.Add(new Item() { Title = "Computer", Description = "The program '[6132] lab5.vshost.exe: Program Trace' has exited with code 0 (0x0).", Category = FieldTypes.Electronics, Price = 2499.0 });
            items.Add(new Item() { Title = "Apple", Description = "The thread 0x1bcc has exited with code 0 (0x0).", Category = FieldTypes.Food, Price = 1.60 });
            items.Add(new Item() { Title = "Computer", Description = "The program '[6132] lab5.vshost.exe: Program Trace' has exited with code 0 (0x0).", Category = FieldTypes.Electronics, Price = 2499.0 });
            items.Add(new Item() { Title = "Apple", Description = "The thread 0x1bcc has exited with code 0 (0x0).", Category = FieldTypes.Food, Price = 1.60 });
            items.Add(new Item() { Title = "Computer", Description = "The program '[6132] lab5.vshost.exe: Program Trace' has exited with code 0 (0x0).", Category = FieldTypes.Electronics, Price = 2499.0 });
            items.Add(new Item() { Title = "Apple", Description = "The thread 0x1bcc has exited with code 0 (0x0).", Category = FieldTypes.Food, Price = 1.60 });
            dg.Items.Refresh();
            listBox.Items.Refresh();
            
        }
        private void MenuItem_Clear(object sender, RoutedEventArgs e)
        {
            items.Clear();
            dg.Items.Refresh();
            listBox.Items.Refresh();
            ReallyVerifyCart();
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }
        private void ReallyVerifyCart()
        {
            List<CartItem> toRemove = new List<CartItem>();
            totalprice.Text = "0";
            foreach (var i in cartItems)
            {
                if (!items.Contains(i.item))
                {
                    toRemove.Add(i);
                }
                else
                    totalprice.Text = Math.Round((double.Parse(totalprice.Text) + i.count * i.item.Price),2).ToString();
            }
            foreach (var i in toRemove)
            {
                cartItems.Remove(i);
            }
            listBoxCart.Items.Refresh();
        }
        private void VerifyCart(object sender, RoutedEventArgs e)
        {
            ReallyVerifyCart();
        }
        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
           MessageBox.Show("Made by Mateusz Chiliński", "Confirmation", MessageBoxButton.OK);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Item currItem = (Item)(((Button)sender).DataContext);
            AddToCartWindow atc = new AddToCartWindow(currItem.Title);
            atc.ShowDialog();
            if (atc.DialogResult.HasValue && !atc.DialogResult.Value)
                return;
            if (!int.TryParse(atc.count, out int n))
            {
                MessageBox.Show("Invaild format of count!", "Error", MessageBoxButton.OK);
                return;
            }
            int count = int.Parse(atc.count);
            var query = from citem in cartItems
                        where citem.item == currItem
                        select citem;
            if(query.Count() >= 1)
            {
                ((CartItem)query.First()).count += count;
                listBoxCart.Items.Refresh();
            }
            else
            {
                var cIt = new CartItem();
                cIt.item = currItem;
                cIt.count = count;
                cartItems.Add(cIt);
            }
            ReallyVerifyCart();
        }
        private void RemoveFromCart(object sender, RoutedEventArgs e)
        {
            CartItem currItem = (CartItem)(((Button)sender).DataContext);
            cartItems.Remove(currItem);
            ReallyVerifyCart();
        }
        private void Plus(object sender, RoutedEventArgs e)
        {
            CartItem currItem = (CartItem)(((Button)sender).DataContext);
            currItem.count++;
            ReallyVerifyCart();
        }
        private void Minus(object sender, RoutedEventArgs e)
        {
            CartItem currItem = (CartItem)(((Button)sender).DataContext);
            if (currItem.count > 0)
            {
                currItem.count--;
                ReallyVerifyCart();
            }
        }
        private void Name_Checked(object sender, RoutedEventArgs e)
        {
            name.IsEnabled = true;
        }
        private void Name_UnChecked(object sender, RoutedEventArgs e)
        {
            name.IsEnabled = false;
        }
        private void Price_Checked(object sender, RoutedEventArgs e)
        {
            to.IsEnabled = true;
            fromT.IsEnabled = true;
        }
        private void Price_UnChecked(object sender, RoutedEventArgs e)
        {
            to.IsEnabled = false;
            fromT.IsEnabled = false;
        }
        private void Category_Checked(object sender, RoutedEventArgs e)
        {
            category.IsEnabled = true;

        }
        private void Category_UnChecked(object sender, RoutedEventArgs e)
        {
            category.IsEnabled = false;
        }
        private void Save(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML file (*.xml)|*.xml";
            if (saveFileDialog.ShowDialog() == true)
            {
                var fs = new FileStream(saveFileDialog.FileName, FileMode.Create);
                XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Item>));
                xs.Serialize(fs, items);
                fs.Close();
            }
        }
        private void Load(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML file (*.xml)|*.xml";
            if (openFileDialog.ShowDialog() == true)
            {
                var fs = new FileStream(openFileDialog.FileName, FileMode.Open);
                XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Item>));
                items = (ObservableCollection<Item>) xs.Deserialize(fs);
                fs.Close();
                dg.ItemsSource = items;
                listBox.ItemsSource = new ListCollectionView(items);
            }
        }
        private void Checkout(object sender, RoutedEventArgs e)
        {
            double total = (from citem in cartItems
                        select citem.item.Price*citem.count).Sum();
            MessageBox.Show("Checkout completed. Total price: " + total + "zł.", "Checkout", MessageBoxButton.OK);
        }
        private void Search(object sender, RoutedEventArgs e)
        {
            if(!to.IsEnabled && !name.IsEnabled && !category.IsEnabled)
            {
                MessageBox.Show("You have not selected anything to search for, fool!", "Error", MessageBoxButton.OK);
                return;
            }
            if(to.IsEnabled == true)
            {
                if(!int.TryParse(to.Text, out int n) || !int.TryParse(fromT.Text, out int n2))
                {
                    MessageBox.Show("Invaild format of price!", "Error", MessageBoxButton.OK);
                    return;
                }
            }
            var query = from item in items
                        where (item.Title.ToLower().Contains(name.Text.ToLower()) || name.Text == "" || name.IsEnabled == false) &&
                        ((item.Price <= int.Parse(to.Text) && item.Price >= int.Parse(fromT.Text)) || to.IsEnabled == false) &&
                        ((category.IsEnabled == false) || category.Text == item.Category.ToString())
                        select item;
            listBox.ItemsSource = query;
        }
        private void ShowAll(object sender, RoutedEventArgs e)
        {
            listBox.ItemsSource = new ListCollectionView(items);
        }
    }
    public enum FieldTypes
    {
        Electronics,
        Food,
        Clothes
    }
    public class Item
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public FieldTypes Category { get; set; }
        public double Price { get; set; }
    }
    public class CartItem
    {
        public Item item { get; set; }
        public int count { get; set; }
    }
}
