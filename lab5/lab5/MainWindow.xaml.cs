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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Item> items = new ObservableCollection<Item>();
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            dg.ItemsSource = items;
            listBox.ItemsSource = new ListCollectionView(items);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
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
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
           MessageBox.Show("Made by Mateusz Chiliński", "Confirmation", MessageBoxButton.OK);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Added to cart!", "Confirmation", MessageBoxButton.OK);
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
        private void Search(object sender, RoutedEventArgs e)
        {
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
}
