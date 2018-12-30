using System;

namespace HBCrestronLibrary.Accessories.Lightbulb
{
    static class RGBTempObject
    {
        public static ushort Hue = 0;
        public static ushort Saturation = 0;
        public static ushort Brightness = 100;
    }

    class RGB
    {
        public RGB(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }

        public bool Equals(RGB rgb)
        {
            return (R == rgb.R) && (G == rgb.G) && (B == rgb.B);
        }
    }
    class HSV
    {
        public HSV(double h, double s, double v)
        {
            H = h;
            S = s;
            V = v;
        }

        public double H { get; set; }

        public double S { get; set; }

        public double V { get; set; }

        public bool Equals(HSV hsv)
        {
            return (H == hsv.H) && (S == hsv.S) && (V == hsv.V);
        }
    }

    static class HBLightbulbConversion
    {
        public static RGB HSVtoRGB(HSV hsv)
        {
            double r, g, b;
            hsv.S = hsv.S/100;
            hsv.V = hsv.V/100;
            if (hsv.S == 0)
            {
                r = hsv.V;
                g = hsv.V;
                b = hsv.V;
            }
            else
            {
                int i;
                double f, p, q, t;

                if (hsv.H == 360)
                    hsv.H = 0;
                else
                    hsv.H = hsv.H / 60;

                
                i = (int)Math.Floor(hsv.H);
                f = hsv.H - i;

                p = hsv.V * (1.0 - hsv.S);
                q = hsv.V * (1.0 - (hsv.S * f));
                t = hsv.V * (1.0 - (hsv.S * (1.0 - f)));

                switch (i)
                {
                    case 0:
                        r = hsv.V;
                        g = t;
                        b = p;
                        break;

                    case 1:
                        r = q;
                        g = hsv.V;
                        b = p;
                        break;

                    case 2:
                        r = p;
                        g = hsv.V;
                        b = t;
                        break;

                    case 3:
                        r = p;
                        g = q;
                        b = hsv.V;
                        break;

                    case 4:
                        r = t;
                        g = p;
                        b = hsv.V;
                        break;

                    default:
                        r = hsv.V;
                        g = p;
                        b = q;
                        break;
                }

            }

            return new RGB((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }
        public static HSV RGBtoHSV(RGB rgb)
        {
            double delta, min;
            double h = 0, s, v;

            min = Math.Min(Math.Min(rgb.R, rgb.G), rgb.B);
            v = Math.Max(Math.Max(rgb.R, rgb.G), rgb.B);
            delta = v - min;

            // ReSharper disable once CompareOfFloatsByEqualityOperator This could probable be done better, but works for now.
            if (v == 0.0)
                s = 0;
            else
                s = delta / v;

            // ReSharper disable once CompareOfFloatsByEqualityOperator This could probable be done better, but works for now.
            if (s == 0)
                h = 0.0;

            else
            {
                if (rgb.R == v)
                    h = (rgb.G - rgb.B) / delta;
                else if (rgb.G == v)
                    h = 2 + (rgb.B - rgb.R) / delta;
                else if (rgb.B == v)
                    h = 4 + (rgb.R - rgb.G) / delta;

                h *= 60;

                if (h < 0.0)
                    h = h + 360;
            }

            return new HSV(h, s * 100, (v / 255)*100);
        }
    }
}