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
        public ImageWindow(It im)
        {
            BitmapImage bi = new BitmapImage(new Uri(im.Src));
            InitializeComponent();
            this.Title = im.Name;
            mainImage.Source = bi;
            this.Width = bi.Width;
            this.Height = bi.Height;
        }
    }
}
