using System;
using System.Drawing;

namespace Spriter
{
    class PixelFormatConverter
    {
        public static Color R5G6B5(UInt16 w)
        {
            return Color.FromArgb((int)(w >> 8 & 0xf8), (int)(w >> 3 & 0xfc), (int)(w << 3 & 0xf8));
        }


        public static Color RGB5A1(UInt16 w)
        {
            return Color.FromArgb(
                            (int)(w & 1) * 255,
                            (int)(w >> 8 & 0xf8),
                            (int)(w >> 3 & 0xf8),
                            (int)(w << 2 & 0xf8));
        }

        public static Color B5G6R5(UInt16 w)
        {
            return Color.FromArgb((int)(w << 3 & 0xf8), (int)(w >> 3 & 0xfc), (int)(w >> 8 & 0xf8));
        }


        public static Color BGR5A1(UInt16 w)
        {
            return Color.FromArgb(
                            (int)(w & 1) * 255,
                            (int)(w << 2 & 0xf8),
                            (int)(w >> 3 & 0xf8),
                            (int)(w >> 8 & 0xf8));
        }


        public static UInt16 ToR5G6B5(Color c)
        {
            UInt16 w = (UInt16)(((UInt16)(c.B) & 0xf8) >> 3);
            w |= (UInt16)(((UInt16)(c.G) & 0xfc) << 3);
            w |= (UInt16)(((UInt16)(c.R) & 0xf8) << 8);
            return w;
        }


        public static UInt16 ToRGB5A1(Color c)
        {
            UInt16 w = (UInt16)(((UInt16)(c.B) & 0xf8) >> 2);
            w |= (UInt16)(((UInt16)(c.G) & 0xf8) << 3);
            w |= (UInt16)(((UInt16)(c.R) & 0xf8) << 8);
            return w;
        }


        // 0xf8 = 11111000
        // 0xfc = 11111100
        // 0xfc << 2 = 0000001111110000


        public static UInt16 ToB5G6R5(Color c)
        {
            UInt16 w = (UInt16)(((UInt16)(c.R) & 0xf8) >> 3);
            w |= (UInt16)(((UInt16)(c.G) & 0xfc) << 3);
            w |= (UInt16)(((UInt16)(c.B) & 0xf8) << 8);
            return w;
        }


        public static UInt16 ToBGR5A1(Color c)
        {
            UInt16 w = (UInt16)(((UInt16)(c.R) & 0xf8) >> 2);
            w |= (UInt16)(((UInt16)(c.G) & 0xf8) << 3);
            w |= (UInt16)(((UInt16)(c.B) & 0xf8) << 8);
            return w;
        }


    }
}
