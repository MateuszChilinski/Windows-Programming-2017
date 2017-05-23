using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        List<It> s = new List<It>();
        bool initialised = false;
        public MainWindow()
        {
            InitializeComponent();
            this.LoadDirectories();
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
                It it = new It();
                it.Src = op.FileName;
                int idx = op.FileName.LastIndexOf('\\');
                it.Name = op.FileName.Substring(idx + 1);
                ImageWindow n = new ImageWindow(it);
                n.Show();
            }
        }
        private void OpenFile2(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                It xd = (It)(((Border)sender).DataContext);
                ImageWindow n = new ImageWindow(xd);
                n.Show();
            }
        }
        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        void item_Expanded(object sender, RoutedEventArgs e)
        {
            var item = (TreeViewItem)sender;
            if (this.HasDummy(item))
            {
                this.Cursor = Cursors.Wait;
                this.RemoveDummy(item);
                this.ExploreDirectories(item);
                this.ExploreFiles(item);
                this.Cursor = Cursors.Arrow;
            }
        }
        private void ParseFolder(string foldername)
        {
            s.Clear();
            this.listBox.ItemsSource = s;
            It.X = sli.Value + 20;
            It.Y = sli.Value + 20;
            foreach (string f in Directory.GetFiles(foldername))
            {
                if (f.EndsWith(".jpg") || f.EndsWith(".png") || f.EndsWith(".jpeg"))
                {
                    var n = new BitmapImage(new Uri(f));
                    It z = new It();
                    z.Src = f;
                    int idx = f.LastIndexOf('\\');
                    z.Name = f.Substring(idx + 1);
                    s.Add(z);
                }
            }
            listBox.Items.Refresh();
        }
        private void OpenFolder(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string foldername = dialog.SelectedPath;
                ParseFolder(foldername);
            }
        }

        public void LoadDirectories()
        {
            var drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                this.treeView.Items.Add(this.GetItem(drive));
            }
        }
        private void ExploreDirectories(TreeViewItem item)
        {
            var directoryInfo = (DirectoryInfo)null;
            if (item.Tag is DriveInfo)
            {
                directoryInfo = ((DriveInfo)item.Tag).RootDirectory;
            }
            else if (item.Tag is DirectoryInfo)
            {
                directoryInfo = (DirectoryInfo)item.Tag;
            }
            else if (item.Tag is FileInfo)
            {
                directoryInfo = ((FileInfo)item.Tag).Directory;
            }
            if (object.ReferenceEquals(directoryInfo, null)) return;
            foreach (var directory in directoryInfo.GetDirectories())
            {
                var isHidden = (directory.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                var isSystem = (directory.Attributes & FileAttributes.System) == FileAttributes.System;
                if (!isHidden && !isSystem)
                {
                    item.Items.Add(this.GetItem(directory));
                }
            }
        }
        private void ParseFolderEvent(object sender, RoutedEventArgs e)
        {
            TreeViewItem s = sender as TreeViewItem;
            if (!s.IsSelected) return;
            if ((s.DataContext).GetType() == typeof(DirectoryInfo))
                ParseFolder(((DirectoryInfo)s.DataContext).FullName);
            else
                ParseFolder(((DriveInfo)s.DataContext).Name);
            

        }
        private void ExploreFiles(TreeViewItem item)
        {
            var directoryInfo = (DirectoryInfo)null;
            if (item.Tag is DriveInfo)
            {
                directoryInfo = ((DriveInfo)item.Tag).RootDirectory;
            }
            else if (item.Tag is DirectoryInfo)
            {
                directoryInfo = (DirectoryInfo)item.Tag;
            }
            else if (item.Tag is FileInfo)
            {
                directoryInfo = ((FileInfo)item.Tag).Directory;
            }
            if (object.ReferenceEquals(directoryInfo, null)) return;
        }
        private TreeViewItem GetItem(DriveInfo drive)
        {
            var item = new TreeViewFolder
            {
                Header = drive.Name,
                DataContext = drive,
                Tag = drive,
            };
            this.AddDummy(item);
            item.Expanded += new RoutedEventHandler(item_Expanded);
            item.Selected += new RoutedEventHandler(ParseFolderEvent);
            return item;
        }

        private void Item_Unselected(object sender, RoutedEventArgs e)
        {
            s.Clear();
        }

        private TreeViewItem GetItem(DirectoryInfo directory)
        {
            var item = new TreeViewFolder
            {
                Header = directory.Name,
                DataContext = directory,
                Tag = directory
            };
            this.AddDummy(item);
            item.Selected += new RoutedEventHandler(ParseFolderEvent);
            item.Expanded += new RoutedEventHandler(item_Expanded);
            return item;
        }

        private void AddDummy(TreeViewItem item)
        {
            item.Items.Add(new DummyTreeViewItem());
        }

        private bool HasDummy(TreeViewItem item)
        {
            return item.HasItems && (item.Items.OfType<TreeViewItem>().ToList().FindAll(tvi => tvi is DummyTreeViewItem).Count > 0);
        }

        private void RemoveDummy(TreeViewItem item)
        {
            var dummies = item.Items.OfType<TreeViewItem>().ToList().FindAll(tvi => tvi is DummyTreeViewItem);
            foreach (var dummy in dummies)
            {
                item.Items.Remove(dummy);
            }
        }
        private void Slider_DragCompleted(object sender, DragDeltaEventArgs e)
        {
            It.X = sli.Value+20;
            It.Y = sli.Value+20;
            listBox.Items.Refresh();
        }

        private void About(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Made by Mateusz Chilinski", "About", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (initialised)
            {
                treeView.IsEnabled = true;
                ((Storyboard)FindResource("in")).Begin(tree);
            }
            initialised = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            treeView.IsEnabled = false;
            ((Storyboard)FindResource("out")).Begin(tree);
        }
    }
    public class It
    {
        public static double X { get; set; }
        public static double Y { get; set; }
        public string Name { get; set; }
        public string Src { get; set; }
    }
    public class TreeViewFolder : TreeViewItem
    {
        public string Path { get; set; }
        public TreeViewFolder() : base()
        {

        }
    }
    public class DummyTreeViewItem : TreeViewItem
    {
        public DummyTreeViewItem()
            : base()
        {
            base.Header = "Dummy";
            base.Tag = "Dummy";
        }
    }
}
