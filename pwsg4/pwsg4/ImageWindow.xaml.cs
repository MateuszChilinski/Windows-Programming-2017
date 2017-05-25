using Microsoft.Win32;
using PluginContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
namespace pwsg4
{
    /// <summary>
    /// Interaction logic for ImageWindow.xaml
    /// </summary>
    public partial class ImageWindow : Window
    {
        BitmapImage original;
        BitmapImage current;
        List<IPlugin> intf = new List<IPlugin>();
        public ImageWindow(It im)
        {
            Uri n = new Uri(im.Src);
            BitmapImage bi = new BitmapImage(n);
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
            t1.Text = im.Name;
            t2.Text = current.Width.ToString() + " px";
            t3.Text = current.Height.ToString() + " px";
            t4.Text = File.GetCreationTime(im.Src).ToString();
            cb.ItemsSource = intf;
            cb.DisplayMemberPath = "Name";
            cb.SelectedValuePath = "Name";
            loadPlugins();
            cb.SelectedItem = intf[0];
            cb.Items.Refresh();
        }

        private void loadPlugins()
        {
            intf.Clear();
            String path = System.AppDomain.CurrentDomain.BaseDirectory;
            string[] pluginFiles = Directory.GetFiles(path, "*.dll");
            //loop through the found dlls and load them 

            var forceLoad = typeof(IPlugin);
            foreach (string dll in pluginFiles)
            {
                System.Reflection.Assembly plugin = System.Reflection.Assembly.LoadFile(dll);

                //now find the classes that implement the interface IMyPluginInterface and get an object of that type 

                foreach (var z in plugin.DefinedTypes)
                {
                    foreach (var i in z.ImplementedInterfaces)
                    {
                        if (i.Name == "IPlugin")
                        {
                            IPlugin myPlugin = (IPlugin)Activator.CreateInstance(z);
                            intf.Add(myPlugin);
                        }
                    }
                }
            }

        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        void SaveUsingEncoder(string fileName, BitmapEncoder encoder)
        {
            BitmapFrame frame = BitmapFrame.Create(current);
            encoder.Frames.Add(frame);
            
            using (var stream = File.Create(fileName))
            {
                encoder.Save(stream);
            }
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Bitmap Image (.bmp)|*.bmp|JPEG Image (.jpeg)|*.jpeg|JPEG Image (.jpg)|*.jpg|Png Image (.png)|*.png";

            if (saveFileDialog.ShowDialog() == true)
            {
                var extension = Path.GetExtension(saveFileDialog.FileName);

                switch (extension.ToLower())
                {
                    case ".jpg":
                    case ".jpeg":
                        var encoder1 = new JpegBitmapEncoder();
                        SaveUsingEncoder(saveFileDialog.FileName, encoder1);
                        break;
                    case ".png":
                        var encoder2 = new PngBitmapEncoder();
                        SaveUsingEncoder(saveFileDialog.FileName, encoder2);
                        break;
                    case ".bmp":
                        var encoder3 = new BmpBitmapEncoder();
                        SaveUsingEncoder(saveFileDialog.FileName, encoder3);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(extension);
                }
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            current = original.Clone();
            mainImage.Source = current;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var z = (IPlugin)cb.SelectedItem;
            current = z.Do(original);
            mainImage.Source = current;
        }
    }
}
