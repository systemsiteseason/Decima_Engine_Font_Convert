using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decima_Engine_Font_Convert.FontInfo
{
    public class FontGlyphs
    {
        public string familyName { get; set; }
        public float unitsPerEm { get; set; }
        public float openTypeOS2TypoAscender { get; set; }
        public float descender { get; set; }
        public float ascender { get; set; }
        public List<Glyph> Glyphs { get; set; }
    }

    public class Kernel
    {
        public string charA { get; set; }
        public string charB { get; set; }
        public float size { get; set; }
    }
}
