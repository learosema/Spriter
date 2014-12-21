using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace Spriter
{
    class PaletteFile
    {

        public PaletteFile()
        {
            palette = new Color[256];
            DefaultPalette();
        }

        public PaletteFile(String strFile)
        {
            Load(strFile);
        }

        public void Load(String strFile)
        {
            palette = new Color[256];
            FileStream fs = new FileStream(strFile, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            palette = new Color[256];
            Byte[] reds = new Byte[256];
            Byte[] greens = new Byte[256];
            Byte[] blues = new Byte[256];
            reds = br.ReadBytes(256);
            greens = br.ReadBytes(256);
            blues = br.ReadBytes(256);
            for (UInt16 i = 0; i < 256; i++)
            {
                palette[i] = Color.FromArgb((Byte)reds[i] << 2, (Byte)greens[i] << 2, blues[i] << 2);
            }
            br.Close();
            fs.Close();
        }

        public void Save(String strFile)
        {
            FileStream fs = new FileStream(strFile, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            for (UInt16 i = 0; i < 256; i++)
            {
                Byte b = (Byte)(palette[i].R >> 2);
                bw.Write(b);
            }
            for (UInt16 i = 0; i < 256; i++)
            {
                Byte b = (Byte)(palette[i].G >> 2);
                bw.Write(b);
            }
            for (UInt16 i = 0; i < 256; i++)
            {
                Byte b = (Byte)(palette[i].B >> 2);
                bw.Write(b);
            }
            bw.Close();
            fs.Close();
        }

        public void DefaultPalette()
        {
            palette[0] = Color.Black;
            palette[1] = Color.Blue;
            palette[2] = Color.Green;
            palette[3] = Color.Cyan;
            palette[4] = Color.Red;
            palette[5] = Color.Purple;
            palette[6] = Color.Orange;
            palette[7] = Color.Gray;
            palette[8] = Color.DarkGray;
            palette[9] = Color.LightBlue;
            palette[10] = Color.LightGreen;
            palette[11] = Color.LightCyan;
            palette[12] = Color.LightSalmon;
            palette[13] = Color.LightPink;
            palette[14] = Color.LightYellow;
            palette[15] = Color.White;
            for (int i = 0; i < 16; i++)
            {
                palette[16 + i] = Color.FromArgb(15 + 16 * i, 15 + 16 * i, 15 + 16 * i);
            }
            for (int i = 32; i < 256; i++)
            {
                palette[i] = Color.Black;
            }

        }

        public Color this[int index] {
            get { return palette[index]; }
            set { palette[index] = value;  }  
        }
        
        

        private Color[] palette = new Color[256];

    }
}