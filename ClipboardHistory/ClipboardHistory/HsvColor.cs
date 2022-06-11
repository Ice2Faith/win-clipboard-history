using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ClipboardHistory
{
    /**
     * 色相（H,hue）：在0~360°的标准色轮上，色相是按位置度量的。在通常的使用中，色相是由颜色名称标识的，比如红、绿或橙色。黑色和白色无色相。
    饱和度（S,saturation）：表示色彩的纯度，为0时为灰色。白、黑和其他灰色色彩都没有饱和度的。在最大饱和度时，每一色相具有最纯的色光。取值范围0～100%。
    亮度（B,brightness或V,value）：是色彩的明亮度。为0时即为黑色。最大亮度是色彩最鲜明的状态。取值范围0～100%。
     */
    public class HslColor
    {
        public double h;
        public double s;
        public double l;
        public int a;
        public HslColor()
        {

        }
        public HslColor(double h, double s, double l, int a)
        {
            this.h = stdHsl(h);
            this.s = stdHsl(s);
            this.l = stdHsl(l);
            this.a = a;
        }

        public static double stdHsl(double val)
        {
            if (val < 0)
            {
                val = 0;
            }
            if (val > 1)
            {
                val = 1;
            }
            return val;
        }

        public static double stdRange(double val, double min, double max)
        {
            if (val > max)
            {
                val = max;
            }
            if (val < min)
            {
                val = min;
            }
            return val;
        }

        public static int stdRange(int val, int min, int max)
        {
            if (val > max)
            {
                val = max;
            }
            if (val < min)
            {
                val = min;
            }
            return val;
        }

        public static HslColor rgbToHsv(Color rgb)
        {
            double H = 0, L = 0, S = 0;
            double R, G, B, Max, Min, del_R, del_G, del_B, del_Max;
            R = rgb.R / 255.0;       //Where RGB values = 0 ÷ 255
            G = rgb.G / 255.0;
            B = rgb.B / 255.0;

            Min = Math.Min(R, Math.Min(G, B));    //Min. value of RGB
            Max = Math.Max(R, Math.Max(G, B));    //Max. value of RGB
            del_Max = Max - Min;        //Delta RGB value

            L = (Max + Min) / 2.0;

            if (del_Max == 0)           //This is a gray, no chroma...
            {
                //H = 2.0/3.0;          //Windows下S值为0时，H值始终为160（2/3*240）
                H = 0;                  //HSL results = 0 ÷ 1
                S = 0;
            }
            else                        //Chromatic data...
            {
                if (L < 0.5) S = del_Max / (Max + Min);
                else S = del_Max / (2 - Max - Min);

                del_R = (((Max - R) / 6.0) + (del_Max / 2.0)) / del_Max;
                del_G = (((Max - G) / 6.0) + (del_Max / 2.0)) / del_Max;
                del_B = (((Max - B) / 6.0) + (del_Max / 2.0)) / del_Max;

                if (R == Max) H = del_B - del_G;
                else if (G == Max) H = (1.0 / 3.0) + del_R - del_B;
                else if (B == Max) H = (2.0 / 3.0) + del_G - del_R;

                if (H < 0) H += 1;
                if (H > 1) H -= 1;
            }
            return new HslColor(H, S, L, rgb.A);
        }

        public static Color hsvToRgb(HslColor hsv)
        {
            double H = hsv.h, S = hsv.s, L = hsv.l;
            double R, G, B;
            double var_1, var_2;
            if (S == 0)                       //HSL values = 0 ÷ 1
            {
                R = L * 255.0;                   //RGB results = 0 ÷ 255
                G = L * 255.0;
                B = L * 255.0;
            }
            else
            {
                if (L < 0.5) var_2 = L * (1 + S);
                else var_2 = (L + S) - (S * L);

                var_1 = 2.0 * L - var_2;

                R = 255.0 * Hue2RGB(var_1, var_2, H + (1.0 / 3.0));
                G = 255.0 * Hue2RGB(var_1, var_2, H);
                B = 255.0 * Hue2RGB(var_1, var_2, H - (1.0 / 3.0));
            }
            R = stdRange(R, 0, 255);
            G = stdRange(G, 0, 255);
            R = stdRange(R, 0, 255);
            return Color.FromArgb(hsv.a, (int)R, (int)G, (int)B);
        }

        public static double Hue2RGB(double v1, double v2, double vH)
        {
            if (vH < 0) vH += 1;
            if (vH > 1) vH -= 1;
            if (6.0 * vH < 1) return v1 + (v2 - v1) * 6.0 * vH;
            if (2.0 * vH < 1) return v2;
            if (3.0 * vH < 2) return v1 + (v2 - v1) * ((2.0 / 3.0) - vH) * 6.0;
            return (v1);
        }
        
    }
}
