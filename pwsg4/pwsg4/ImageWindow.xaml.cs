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

namespace pwsg4
{
    /// <summary>
    /// Interaction logic for ImageWindow.xaml
    /// </summary>
    public partial class ImageWindow : Window
    {
        BitmapImage original;
        BitmapImage current;
        public ImageWindow(It im)
        {
            BitmapImage bi = new BitmapImage(new Uri(im.Src));
            InitializeComponent();
            this.Title = im.Name;
            original = bi;
            current = original.Clone();
            this.mainImage.Source = current;
            this.Width = bi.Width+200;
            this.Height = bi.Height;
            if (Width+100 > SystemParameters.PrimaryScreenWidth)
                Width = SystemParameters.PrimaryScreenWidth-100;
            if (Height+100 > SystemParameters.PrimaryScreenHeight)
                Height = SystemParameters.PrimaryScreenHeight-100;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
