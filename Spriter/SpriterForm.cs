using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;



namespace Spriter
{

    public partial class SpriterForm : Form
    {
        private ArrayList sprites;
        private int key;
        private PaletteFile pal;


        public SpriterForm()
        {

            InitializeComponent();
            this.openFileDialog1.Filter =
            "Images (*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|" +
            "Old pascal sprite files from mode256/mode64k (*.HMG;*.IMG;*.PAL)|*.hmg;*.HMG;*.img;*.IMG;*.PAL;*.pal|" +
            "All files (*.*)|*.*";
            this.openFileDialog1.Multiselect = true;
            spriteView.MultiSelect = true;
            sprites = new ArrayList();
            key = 0;
        }



        private void plusButton_Click(object sender, EventArgs e)
        {
            DialogResult r = openFileDialog1.ShowDialog();
            if (r != DialogResult.OK)
            {
                return;
            }
            foreach (String fn in openFileDialog1.FileNames)
            {
                // MessageBox.Show(openFileDialog1.FileName);
                LoadImage(fn);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            DialogResult r = saveFileDialog1.ShowDialog();
            if (r != DialogResult.OK)
            {
                return;
            }
            int width = 0, height = 0, offsetX = 0;
            foreach (Image img in sprites)
            {
                width += img.Width;
                if (img.Height > height)
                {
                    height = img.Height;
                }
            }
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            // g.SmoothingMode = SmoothingMode.None;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            foreach (Image img in sprites)
            {
                // g.DrawImageUnscaled(img, new Point(offsetX, 0));
                g.DrawImage(img, new Rectangle(offsetX, 0, img.Width, img.Height));
                offsetX += img.Width;
            }
            try
            {
                bmp.Save(saveFileDialog1.FileName, ImageFormat.Png);
            }
            catch (ExternalException ex)
            {
                MessageBox.Show("Error saving the bitmap : " + ex.ToString());
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            // String msg = "";
            int deleted = 0;
            foreach (int i in spriteView.SelectedIndices)
            {
                spriteView.Items.RemoveAt(i - deleted);

                imageList1.Images.RemoveAt(i - deleted);
                sprites.RemoveAt(i - deleted);

                deleted++;
            }
            // MessageBox.Show(msg);
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            sprites.Clear();
            spriteView.Items.Clear();
            imageList1.Images.Clear();
            key = 0;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (pal != null)
            {
                for (int y = 0; y < 16; y++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        SolidBrush sb = new SolidBrush(pal[x * 16 + y]);
                        Rectangle r = new Rectangle((int)(1 + x * 10), (int)(1 + y * 10), 9, 9);
                        g.FillRectangle(sb, r);
                    }
                }
            }

        }



        private bool LoadImage(String FileName)
        {
            String FileExt = FileName.Substring(FileName.IndexOf(".")).ToLower();
            String imgKey;

            if (FileExt == ".pal")
            {
                pal = new PaletteFile(FileName);
                panel1.Width = 180;
                panel1.Update();
                return true;
            }

            if (FileExt == ".img")
            {
                ImgFile imgf = new ImgFile();
                String palFN = FileName.Substring(0, FileName.IndexOf(".")) + ".pal";
                if (System.IO.File.Exists(palFN))
                {
                    pal = new PaletteFile(palFN);
                    panel1.Width = 180;
                    panel1.Update();
                }


                imgf.PaletteFile = pal;
                imgf.Load(FileName);
                spriteView.BeginUpdate();
                for (int idx = 0; idx < imgf.Count; idx++)
                {
                    Bitmap bmp = imgf[idx];
                    imgKey = String.Format("Image{0}", key);
                    key++;
                    imageList1.Images.Add(imgKey, bmp);
                    sprites.Add(bmp);

                    spriteView.Items.Add(String.Format("{0}:{1}", FileName, idx), imgKey);

                }
                spriteView.EndUpdate();
                imgf = null;
                return true;
            }

            if (FileExt == ".hmg")
            {
                ImgFile imgf = new ImgFile();

                imgf.Load(FileName, PixelFormat.Format16bppRgb565);
                spriteView.BeginUpdate();
                for (int idx = 0; idx < imgf.Count; idx++)
                {
                    Bitmap bmp = imgf[idx];
                    imgKey = String.Format("Image{0}", key);
                    key++;
                    imageList1.Images.Add(imgKey, bmp);
                    sprites.Add(bmp);

                    spriteView.Items.Add(String.Format("{0}:{1}", FileName, idx), imgKey);

                }
                spriteView.EndUpdate();
                imgf = null;
                return true;

            }


            Image sprite = null;
            try
            {
                sprite = Image.FromFile(FileName);
            }
            catch (Exception)
            {
                return false;
            }
            imgKey = String.Format("Image{0}", key);
            key++;
            imageList1.Images.Add(imgKey, sprite);

            sprites.Add(sprite);
            spriteView.BeginUpdate();
            spriteView.Items.Add(FileName, imgKey);
            spriteView.EndUpdate();
            return true;
        }

        private void explodeImage(int idx, int w, int h)
        {
            Bitmap bmp1 = (Bitmap)(sprites[idx]);
            String title = spriteView.Items[idx].Text;
            int number = 0;
            spriteView.BeginUpdate();
            for (int y = 0; y < bmp1.Height; y += h)
            {
                for (int x = 0; x < bmp1.Width; x += w)
                {

                    Rectangle rect = new Rectangle(x, y, w, h);
                    Bitmap bmp2 = bmp1.Clone(rect, bmp1.PixelFormat);

                    String imgKey = String.Format("Image{0}", key);
                    key++;

                    imageList1.Images.Add(imgKey, bmp2);
                    sprites.Add(bmp2);
                    spriteView.Items.Add(String.Format("{0}:{1}", title, number), imgKey);

                    number++;
                }
            }
            spriteView.EndUpdate();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            foreach (int idx in spriteView.SelectedIndices)
            {
                Bitmap bmp = (Bitmap)(sprites[idx]);

                ExplodeForm explodeForm = new ExplodeForm();
                explodeForm.ShowDialog();
                if (explodeForm.Okay)
                {
                    int w = explodeForm.InputWidth;
                    int h = explodeForm.InputHeight;
                    if (w < -1 || h < -1 || w > bmp.Width || h > bmp.Height)
                    {
                        MessageBox.Show("Invalid Argument.");
                        return;
                    }
                    explodeImage(idx, w, h);
                }
            }
        }

    }
}
