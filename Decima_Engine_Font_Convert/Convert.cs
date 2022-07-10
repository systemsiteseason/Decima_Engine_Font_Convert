using Decima_Engine_Font_Convert.FontInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decima_Engine_Font_Convert
{
    public class Convert
    {
        public static List<byte> makeSignal(List<byte> input)
        {
            List<byte> output = new List<byte>();
            byte j = 0;
            for(int i = 0; i < input.Count - 1; i++)
            {
                j++;
                if (input[i] == 1 && input[i + 1] == 0)
                {
                    j += 64;
                    continue;
                }
                try
                {
                    if (input[i] == 1 && input[i + 1] == 1 && input[i + 2] == 0)
                    {
                        output.Add(j);
                        j = 0;
                        continue;
                    }
                }
                catch { }

                if (input[i] == 0 && input[i + 1] == 1 )
                {
                    output.Add(j);
                    j = 0;
                    continue;
                }

                if(input[i] == 1 && input[i + 1] == 1 && i == input.Count - 2)
                {
                    if(j > 64)
                    {
                        output.Add((byte)(j - (j - 64)));
                        output.Add((byte)(j - 64));
                        j = 0;
                    }
                    else
                    {
                        output.Add(j);
                        j = 0;
                    }
                    continue;
                }

                if(i == input.Count - 2)
                {
                    output.Add(j);
                    j = 0;
                    continue;
                }
            }
            return output;
        }

        public static byte[] ParseGlyph(Glyph glyph, bool ds = true)
        {
            using(var ms = new MemoryStream())
            {
                if (ds == false)
                {
                    ms.Write(glyph.Character == " " ? 0 : glyph.minX);
                    ms.Write(glyph.Character == " " ? 0 : glyph.maxY);
                }
                ms.Write(glyph.AdvWidth);
                ms.Write(Encoding.Unicode.GetBytes(ds == false ? glyph.Character : glyph.Character + "\0"));
                ms.Write(glyph.minX);
                ms.Write(glyph.minY);
                ms.Write(glyph.maxX);
                ms.Write(glyph.maxY);
                ms.Write(glyph.Pivots.Count);
                foreach(var pivot in glyph.Pivots)
                {
                    ms.Write(pivot.signal.Count);
                    ms.Write(pivot.signal.ToArray());
                    ms.Write(pivot.Points.Count);
                    foreach(var point in pivot.Points)
                    {
                        ms.Write(point.X);
                        ms.Write(point.Y);
                    }
                }
                return ms.ToArray();
            }
        }
    }
}
