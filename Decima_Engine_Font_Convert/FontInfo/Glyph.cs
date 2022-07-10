using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Decima_Engine_Font_Convert.FontInfo
{
    public class Glyph
    {
        public float AdvWidth { get; set; }
        public string Character { get; set; }
        public float minX { get; set; }
        public float minY { get; set; }
        public float maxX { get; set; }
        public float maxY { get; set; }
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
