using Decima_Engine_Font_Convert.FontInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decima_Engine_Font_Convert
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 0)
            {
                var path = args[0];
                var dirpath = Path.GetDirectoryName(args[0]);
                var filename = Path.GetFileName(args[0]);
                var nonext = Path.GetFileNameWithoutExtension(args[0]);
                var ext = Path.GetExtension(args[0]);

                if(ext == ".core")
                {
                    // Parse data
                    BinaryReader rd = new BinaryReader(File.OpenRead(path));
                    UInt64 type = rd.ReadUInt64();
                    if(type == 0xE0E68D6C7B7B08F3)
                    {
                        int len = rd.ReadInt32();
                        byte[] hash = rd.ReadBytes(16);
                        int size = rd.ReadInt32();
                        byte[] hashName = rd.ReadBytes(4);
                        string fontName = Encoding.ASCII.GetString(rd.ReadBytes(size));
                        float UMP = rd.ReadSingle();
                        float openTypeHheaAscender = rd.ReadSingle();
                        float Descender = rd.ReadSingle();
                        float Ascender = rd.ReadSingle();
                        int numGlyph = rd.ReadInt32();
                        FontGlyphs font = new FontGlyphs();
                        List<Glyph> glyphs = new List<Glyph>();

                        for(int i = 0; i < numGlyph; i++)
                        {
                            float advwidth = rd.ReadSingle();
                            string character = Encoding.Unicode.GetString(rd.ReadBytes(4)).Replace("\0", "");
                            float l = rd.ReadSingle();
                            float b = rd.ReadSingle();
                            float t = rd.ReadSingle();
                            float r = rd.ReadSingle();
                            int numPivot = rd.ReadInt32();
                            List<Pivot> pivots = new List<Pivot>();
                            for(int j = 0; j < numPivot; j++)
                            {
                                List<byte> signal = new List<byte>();
                                List<Point> points = new List<Point>();
                                int padding = rd.ReadInt32();
                                signal.AddRange(rd.ReadBytes(padding).ToArray());
                                int numPoint = rd.ReadInt32();
                                for(int k = 0; k < numPoint; k++)
                                {
                                    float x = rd.ReadSingle();
                                    float y = rd.ReadSingle();
                                    points.Add(new Point { X = x, Y = y });
                                }
                                pivots.Add(new Pivot { signal = signal, Points = points });
                            }

                            glyphs.Add(new Glyph { AdvWidth = advwidth, Character = character, Left = l, Bottom = b, Top = t, Right = r, Pivots = pivots });
                        }

                        font = new FontGlyphs { familyName = fontName, unitsPerEm = UMP, openTypeHheaAscender = openTypeHheaAscender, descender = Descender, ascender = Ascender, Glyphs = glyphs};


                        // Convert to UFO

                        Creater_UFO_Folder(font, dirpath);
                    }
                }
                else
                {

                }
            }
        }

        static void Creater_UFO_Folder(FontGlyphs font, string dir)
        {
            if (!Directory.Exists(dir + "\\" + font.familyName + ".ufo"))
                Directory.CreateDirectory(dir + "\\" + font.familyName + ".ufo");

            if (!Directory.Exists(dir + "\\" + font.familyName + ".ufo\\glyphs"))
                Directory.CreateDirectory(dir + "\\" + font.familyName + ".ufo\\glyphs");

            string pufo = dir + "\\" + font.familyName + ".ufo\\";
            string pglyphs = dir + "\\" + font.familyName + ".ufo\\glyphs\\";

            PlistUC wpuc = new PlistUC();
            Glif wpglif = new Glif();

            wpuc.FontInfo(font, pufo);
            wpuc.MetaInfo(pufo);
            wpuc.LayerContents(pufo);
            wpuc.LayerInfo(pglyphs);
            wpuc.Contents(font.Glyphs, pglyphs);
            wpuc.Lib(font.Glyphs, pufo);

            wpglif.writeGlyphs(font.Glyphs, pglyphs);

        }
    }
}
