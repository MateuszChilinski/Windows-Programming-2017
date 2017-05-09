using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace pwsg_Lab3
{
    public class PictureBoxExtra : PictureBox
    {
        private string fp;
        public string filePath
        {
            get { return fp; }
            set { fp = value; }
        }
    }
    public class LibraryManager
    {
        private string path;
        HashSet<KeyValuePair<string, Bitmap>> myImages = new HashSet<KeyValuePair<string, Bitmap>>();
        FlowLayoutPanel flowLayoutPanel;
        XmlDocument myFile;
        public LibraryManager(string p)
        {
            path = p;
            
            if(!File.Exists(path))
            {
                new XDocument(new XElement("data")).Save(path);
            }
            myFile = new XmlDocument();
            myFile.Load(path);
            XmlNodeList query = myFile.SelectNodes("data/Image/@path");
            foreach(XmlNode img in query)
            {
                myImages.Add(new KeyValuePair<string, Bitmap>(img.Value, new Bitmap(img.Value)));
            }
        }
        public void displayImage(KeyValuePair<string, Bitmap> kvp)
        {
            PictureBoxExtra tmp = new PictureBoxExtra();
            tmp.BackgroundImage = global::pwsg_Lab3.Properties.Resources.NoImage;
            tmp.Location = new System.Drawing.Point(3, 3);
            tmp.Name = "pictureBox11";
            tmp.Size = new System.Drawing.Size(150, 150);
            tmp.Image = kvp.Value;
            tmp.filePath = kvp.Key;
            tmp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            tmp.MouseClick += Tmp_MouseClick;
            flowLayoutPanel.Controls.Add(tmp);
        }

        private void Tmp_MouseClick(object sender, MouseEventArgs e)
        {
            PictureBoxExtra curr = sender as PictureBoxExtra;
            
            System.Console.Out.WriteLine(curr.filePath);
        }

        public void addNewImage(Bitmap map, string fileName)
        {
            XmlNodeList query = myFile.SelectNodes("data/Image/@path");
            foreach (XmlNode img in query)
            {
                if(img.Value == fileName)
                {
                    return;
                }
            }
            KeyValuePair<string, Bitmap> tmp = new KeyValuePair<string, Bitmap>(fileName, map);
            myImages.Add(tmp);
            displayImage(tmp);
        }

        public void initializePictures(FlowLayoutPanel flp)
        {
            flowLayoutPanel = flp;
            foreach(var img in myImages)
            {
                displayImage(img);
            }
        }
        
    }
}
