using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yoga.ImageControl
{
    public partial class PictureUnit : UserControl
    {
        Bitmap curBitmap;
        public PictureUnit()
        {
            InitializeComponent();
        }
        public void ShowImage(Image image)
        {
            pictureBox1.Image = image;
        }
        public void ShowScreenshots(string savePath)
        {
            if (curBitmap != null)
            {
                curBitmap.Dispose();
            }
            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;
            curBitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(curBitmap))
            {
                g.CopyFromScreen(0, 0, 0, 0, new Size(width, height));
            }
            pictureBox1.Image = curBitmap;
            if (savePath!=string.Empty)
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(savePath);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                curBitmap.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
        }
        public void ShowScreenshots()
        {
            ShowScreenshots(string.Empty);
        }
    }
}
