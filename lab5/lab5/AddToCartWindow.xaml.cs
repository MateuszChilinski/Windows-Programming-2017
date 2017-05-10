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

namespace lab5
{
    /// <summary>
    /// Interaction logic for AddToCartWindow.xaml
    /// </summary>
    public partial class AddToCartWindow : Window
    {
        public string count;
        public AddToCartWindow(String name)
        {
            InitializeComponent();
            nop.Text = name;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            count = c.Text;
            this.Close();
        }
    }
}
