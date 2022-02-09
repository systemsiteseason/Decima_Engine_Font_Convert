using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decima_Engine_Font_Convert.FontInfo
{
    public class Glyph
    {
        public float AdvWidth { get; set; }
        public string Character { get; set; }
        public float Left { get; set; }
        public float Bottom { get; set; }
        public float Top { get; set; }
        public float Right { get; set; }
        public List<Pivot> Pivots { get; set; }
    }

    public class Pivot
    {
        public List<byte> signal { get; set; }
        public List<Point> Points { get; set; }
    }

    public class Point
    {
        public float X { get; set; }
        public float Y { get; set; }
    }
}
