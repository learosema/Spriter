using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using System.Collections;
using System.IO;

namespace Spriter
{
    class ImgFile : IEnumerator
    {
        private ArrayList sprites;
        private PaletteFile pal;
        private String palFN;

        private int enumCursor = -1;

        public IEnumerator GetEnumerator() {
            return (IEnumerator)this;
        }

        public bool MoveNext()
        {
            enumCursor++;
            return (enumCursor < sprites.Count);
        }

        public void Reset()
        {
            enumCursor = -1;
        }

        public object Current
        {
            get
            {
                try
                {
                    return (Bitmap)sprites[enumCursor];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public int Count {
            get { return sprites.Count; }
        }
        

        public ImgFile() {
            sprites = new ArrayList();
            pal = null;
            palFN = "";
        }

        public void Load(String fileName, PixelFormat format = PixelFormat.Format8bppIndexed) 
        {
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            UInt16 numSprites = br.ReadUInt16();
            numSprites++;
            UInt16[] width = new UInt16[numSprites];
            UInt16[] height = new UInt16[numSprites];
            if (sprites == null) {
                sprites = new ArrayList();
            }
            for (UInt16 i = 0; i < numSprites; i++)
            {
                width[i] = br.ReadUInt16();
            }
            for (UInt16 i = 0; i < numSprites; i++)
            {
                height[i] = br.ReadUInt16();
            }
            switch (format) { 
                case PixelFormat.Format8bppIndexed:
                case PixelFormat.Indexed:
                    if (pal == null) {
                        // load a default palette 
                        pal = new PaletteFile();
                    }
                    for (UInt16 i = 0; i < numSprites; i++)
                    {
                        
                        Bitmap bmp = new Bitmap((int)(width[i]), (int)(height[i]));
                        for (UInt16 y = 0; y < height[i]; y++)
                        {
                            for (UInt16 x = 0; x < width[i]; x++)
                            {
                                Byte col = br.ReadByte();
                                bmp.SetPixel(x, y, pal[col]);
                            }
                        }
                        sprites.Add(bmp);
                    }
                    break;
                case PixelFormat.Format16bppRgb565:
                    for (UInt16 i = 0; i < numSprites; i++)
                    {
                        Bitmap bmp = new Bitmap((width[i] / 2), height[i]);
                        for (UInt16 y = 0; y < height[i]; y++)
                        {
                            for (UInt16 x = 0; x < (width[i] / 2); x++)
                            {
                                UInt16 w = br.ReadUInt16();
                                bmp.SetPixel(x, y, PixelFormatConverter.R5G6B5(w));   
                            }
                        }
                        sprites.Add(bmp);
                    }
                    break;
                default:
                    throw new ArgumentException("PixelFormat not supported.");            
            }
            br.Close();
            fs.Close();    
        }



        public Bitmap this[int index] { 
            get { return (Bitmap) sprites[index]; }
        }

        public String Palette {
            get { return palFN; }
            set { palFN = value; pal = new PaletteFile(palFN); }
        }

        public PaletteFile PaletteFile {
            get { return pal; }
            set { pal = value; }
        }

    }
}
