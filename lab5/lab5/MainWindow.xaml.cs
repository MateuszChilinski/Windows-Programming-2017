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
    }
    public enum FieldTypes
    {
        Electronics,
        Food,
        SomeRandomShit
    }
    public class Item
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public FieldTypes Category { get; set; }
        public double Price { get; set; }
    }
}
