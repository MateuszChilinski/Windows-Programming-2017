using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
namespace pwsg4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RadialGradientBrush wyellow = new RadialGradientBrush();
        RadialGradientBrush normal = new RadialGradientBrush();
        List<It> s = new List<It>();
        public MainWindow()
        {
            InitializeComponent();
            
            GradientStop frist = new GradientStop();
            frist.Color = Colors.White;
            frist.Offset = 0.5;
            wyellow.GradientStops.Add(frist);
            GradientStop second = new GradientStop();
            second.Color = Colors.Yellow;
            second.Offset = 0.2;
            wyellow.GradientStops.Add(second);
            GradientStop third = new GradientStop();
            Color color = (Color)ColorConverter.ConvertFromString("#FF8b9a59");
            third.Color = color;
            third.Offset = 1;
            wyellow.GradientStops.Add(third);


            GradientStop frist2 = new GradientStop();
            frist2.Color = Colors.White;
            frist2.Offset = 0.5;
            normal.GradientStops.Add(frist2);
            GradientStop third2 = new GradientStop();
            Color color2 = (Color)ColorConverter.ConvertFromString("#FF8b9a59");
            third2.Color = color;
            third2.Offset = 1;
            normal.GradientStops.Add(third2);

            e1.Fill = normal;
            e2.Fill = normal;
            e3.Fill = normal;
        }

        private void e3_MouseEnter(object sender, MouseEventArgs e)
        {
            Ellipse el = (Ellipse)sender;
            el.Fill = wyellow;
        }

        private void e3_MouseLeave(object sender, MouseEventArgs e)
        {
            Ellipse el = (Ellipse)sender;
            el.Fill = normal;
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                ImageWindow n = new ImageWindow(new BitmapImage(new Uri(op.FileName)));
                n.Show();
            }
        }
        private void OpenFile2(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                It xd = (It)(((Image)sender).DataContext);
                ImageWindow n = new ImageWindow(new BitmapImage(new Uri(xd.Src)));
                n.Show();
            }
        }
        private void OpenFolder(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string foldername = dialog.SelectedPath;
                this.listBox.ItemsSource = s;
                foreach (string f in Directory.GetFiles(foldername))
                {
                    if (f.EndsWith(".jpg") || f.EndsWith(".png"))
                    {
                        var n = new BitmapImage(new Uri(f));
                        It z = new It();
                        z.Src = f;
                        int idx = f.LastIndexOf('\\');
                        z.Name = f.Substring(idx + 1);
                        z.X = sli.Value*5;
                        z.Y = sli.Value*5;
                        s.Add(z);
                    }
                }
                listBox.Items.Refresh();
            }
        }

        private void Slider_DragCompleted(object sender, RoutedEventArgs e)
        {
            foreach(var z in s)
            {
                z.X = sli.Value*5;
                z.Y = sli.Value*5;
            }
            listBox.Items.Refresh();
        }
    }
    public class It
    {
        public double X { get; set; }
        public double Y { get; set; }
        public string Name { get; set; }
        public string Src { get; set; }
    }
}
