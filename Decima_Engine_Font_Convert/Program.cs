using Decima_Engine_Font_Convert.FontInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

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
                    if(type == 0xE0E68D6C7B7B08F3) //DS
                    {
                        int len = rd.ReadInt32();
                        byte[] hash = rd.ReadBytes(16);
                        int size = rd.ReadInt32();
                        byte[] hashName = rd.ReadBytes(4);
                        string fontName = Encoding.ASCII.GetString(rd.ReadBytes(size));
                        float UMP = rd.ReadSingle();
                        float openTypeOS2TypoAscender = rd.ReadSingle();
                        float Descender = rd.ReadSingle();
                        float Ascender = rd.ReadSingle();
                        int numGlyph = rd.ReadInt32();
                        FontGlyphs font = new FontGlyphs();
                        List<Glyph> glyphs = new List<Glyph>();

                        for(int i = 0; i < numGlyph; i++)
                        {
                            float advwidth = rd.ReadSingle();
                            string character = Encoding.Unicode.GetString(rd.ReadBytes(4)).Replace("\0", "");
                            float minx = rd.ReadSingle();
                            float miny = rd.ReadSingle();
                            float maxx = rd.ReadSingle();
                            float maxy = rd.ReadSingle();
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

                            glyphs.Add(new Glyph { AdvWidth = advwidth, Character = character, minX = minx, minY = miny, maxX = maxx, maxY = maxy, Pivots = pivots });
                        }

                        font = new FontGlyphs { familyName = fontName, unitsPerEm = UMP, openTypeOS2TypoAscender = openTypeOS2TypoAscender, descender = Descender, ascender = Ascender, Glyphs = glyphs};


                        // Convert to UFO

                        Creater_UFO_Folder(font, dirpath);
                    }

                    if(type == 0x2B2A73E2413BA60E) // HZD
                    {
                        int len = rd.ReadInt32();
                        byte[] hash = rd.ReadBytes(16);
                        int size = rd.ReadInt32();
                        byte[] hashName = rd.ReadBytes(4);
                        string fontName = Encoding.ASCII.GetString(rd.ReadBytes(size));
                        size = rd.ReadInt32();
                        hashName = rd.ReadBytes(4);
                        string fontNameFull = Encoding.ASCII.GetString(rd.ReadBytes(size));
                        float UMP = rd.ReadSingle();
                        float openTypeOS2TypoAscender = rd.ReadSingle();
                        float Descender = rd.ReadSingle();
                        float Ascender = rd.ReadSingle();
                        int numGlyph = rd.ReadInt32();
                        FontGlyphs font = new FontGlyphs();
                        List<Glyph> glyphs = new List<Glyph>();

                        for (int i = 0; i < numGlyph; i++)
                        {
                            float sideRight = rd.ReadSingle();
                            float height = rd.ReadSingle();
                            float advwidth = rd.ReadSingle();
                            string character = Encoding.Unicode.GetString(rd.ReadBytes(2)).Replace("\0", "");
                            float minx = rd.ReadSingle();
                            float miny = rd.ReadSingle();
                            float maxx = rd.ReadSingle();
                            float maxy = rd.ReadSingle();
                            int numPivot = rd.ReadInt32();
                            List<Pivot> pivots = new List<Pivot>();
                            for (int j = 0; j < numPivot; j++)
                            {
                                List<byte> signal = new List<byte>();
                                List<Point> points = new List<Point>();
                                int padding = rd.ReadInt32();
                                signal.AddRange(rd.ReadBytes(padding).ToArray());
                                int numPoint = rd.ReadInt32();
                                for (int k = 0; k < numPoint; k++)
                                {
                                    float x = rd.ReadSingle();
                                    float y = rd.ReadSingle();
                                    points.Add(new Point { X = x, Y = y });
                                }
                                pivots.Add(new Pivot { signal = signal, Points = points });
                            }

                            glyphs.Add(new Glyph {AdvWidth = advwidth, Character = character, minX = minx, minY = miny, maxX = maxx, maxY = maxy, Pivots = pivots });
                        }

                        font = new FontGlyphs { familyName = fontName, unitsPerEm = UMP, openTypeOS2TypoAscender = openTypeOS2TypoAscender, descender = Descender, ascender = Ascender, Glyphs = glyphs };


                        // Convert to UFO

                        Creater_UFO_Folder(font, dirpath);
                    }
                }
                else if(ext == ".ufo" && args.Length == 2)
                {
                    var fontinfo = path + "\\fontinfo.plist";
                    var contents = path + "\\glyphs\\contents.plist";
                    var glyphs = path + "\\glyphs";

                    XmlDocument doc = new XmlDocument();
                    doc.Load(fontinfo);
                    FontGlyphs fontGlyphs = new FontGlyphs();
                    List<string> charName = new List<string>();
                    foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                    {
                        foreach(XmlNode x in node.ChildNodes)
                        {
                            try
                            {
                                string valueNode = x.FirstChild.Value;
                                switch (valueNode)
                                {
                                    case "familyName":
                                        string name = x.NextSibling.FirstChild.Value;
                                        fontGlyphs.familyName = name;
                                        break;
                                    case "openTypeOS2TypoAscender":
                                        float des = float.Parse(x.NextSibling.FirstChild.Value);
                                        fontGlyphs.openTypeOS2TypoAscender = des;
                                        break;
                                    case "unitsPerEm":
                                        float upm = float.Parse(x.NextSibling.FirstChild.Value);
                                        fontGlyphs.unitsPerEm = upm;
                                        break;
                                    case "ascender":
                                        float ascender = float.Parse(x.NextSibling.FirstChild.Value);
                                        fontGlyphs.ascender = ascender;
                                        break;
                                    case "descender":
                                        float descender = float.Parse(x.NextSibling.FirstChild.Value) * -1;
                                        fontGlyphs.descender = descender;
                                        break;
                                }
                            }
                            catch { }
                        }
                    }

                    doc.Load(contents);
                    foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                    {
                        foreach (XmlNode x in node.ChildNodes)
                        {
                            string attr = x.Name;
                            if (attr == "string")
                                charName.Add(glyphs + "\\" + x.FirstChild.Value);
                        }
                    }


                    //Create binary Font

                    Build_Font_Binary(fontGlyphs, charName, args[1]);
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

        static void Build_Font_Binary(FontGlyphs fontGlyphs, List<string> charName, string filebinary)
        {
            string binName = Path.GetFileName(filebinary);
            string dir = Path.GetDirectoryName(filebinary);

            MemoryStream ms = new MemoryStream(File.ReadAllBytes(filebinary));
            bool isDs = true;
            MemoryStream newFont = new MemoryStream();

            UInt64 type = ms.ReadUInt64();
            if (type == 0x2B2A73E2413BA60E)
                isDs = false;
            newFont.Write(BitConverter.GetBytes(type));
            newFont.Write(ms.ReadBytes(4));
            newFont.Write(ms.ReadBytes(16));

            int sizeName = ms.ReadInt32();
            newFont.Write(sizeName);
            newFont.Write(ms.ReadBytes(4));
            newFont.Write(ms.ReadBytes(sizeName));
            if(isDs == false)
            {
                sizeName = ms.ReadInt32();
                newFont.Write(sizeName);
                newFont.Write(ms.ReadBytes(4));
                newFont.Write(ms.ReadBytes(sizeName));
            }

            newFont.Write(fontGlyphs.unitsPerEm);
            newFont.Write(fontGlyphs.openTypeOS2TypoAscender);
            newFont.Write(fontGlyphs.descender);
            newFont.Write(fontGlyphs.ascender);
            newFont.Write(charName.Count);
            ms.Skip(16);
            sizeName = ms.ReadInt32();
            for(int i = 0; i < sizeName; i++)
            {
                if (isDs == false)
                    ms.Skip(30);
                else
                    ms.Skip(24);
                int numPivot = ms.ReadInt32();
                for (int j = 0; j < numPivot; j++)
                {
                    int padding = ms.ReadInt32();
                    ms.Skip(padding);
                    int numPoint = ms.ReadInt32();
                    ms.Skip(numPoint * 8);
                }
            }

            List<Kernel> kernels = new List<Kernel>();
            sizeName = ms.ReadInt32();
            for(int i = 0; i < sizeName; i++)
            {
                string charA = ms.ReadString(2, Encoding.Unicode);
                string charB = ms.ReadString(2, Encoding.Unicode);
                float sz = ms.ReadSingle();
                kernels.Add(new Kernel { charA = charA, charB = charB, size = sz });
            }

            for (int i = 0; i < charName.Count; i++)
            {
                var doc = XDocument.Load(charName[i]);

                var query = doc.Elements("glyph")
                .Select(department => new Glyph
                {
                    AdvWidth = float.Parse(department.Element("advance").Attribute("width").Value),
                    Character = department.Element("unicode") != null ? Encoding.Unicode.GetString(BitConverter.GetBytes(Int16.Parse(department.Element("unicode").Attribute("hex").Value, System.Globalization.NumberStyles.HexNumber))) : " ",
                    Pivots = department.Element("outline") == null ? new List<Pivot>() : department.Element("outline").Elements("contour")
                    .Select(x => new Pivot
                    {
                        Points = x.Elements("point").Select(c => new Point
                        {
                            X = float.Parse(c.Attribute("x").Value),
                            Y = float.Parse(c.Attribute("y").Value),
                        }).ToList(),
                        signal = Convert.makeSignal(x.Elements("point").Select(c => c.Attribute("type") != null ? (byte)0x01 : (byte)0x00).ToList())
                    }).ToList(),
                    minX = department.Element("outline") == null ? 3.40282e+38f : department.Element("outline").Elements("contour").Elements("point").Select(x => float.Parse(x.Attribute("x").Value)).Min(),
                    minY = department.Element("outline") == null ? 3.40282e+38f : department.Element("outline").Elements("contour").Elements("point").Select(x => float.Parse(x.Attribute("y").Value)).Min(),
                    maxX = department.Element("outline") == null ? -3.40282e+38f : department.Element("outline").Elements("contour").Elements("point").Select(x => float.Parse(x.Attribute("x").Value)).Max(),
                    maxY = department.Element("outline") == null ? -3.40282e+38f : department.Element("outline").Elements("contour").Elements("point").Select(x => float.Parse(x.Attribute("y").Value)).Max()
                }).ToList();
                newFont.Write(Convert.ParseGlyph(query[0], isDs));
            }

            newFont.Write(kernels.Count);
            foreach(var kernel in kernels)
            {
                newFont.Write(Encoding.Unicode.GetBytes(kernel.charA));
                newFont.Write(Encoding.Unicode.GetBytes(kernel.charB));
                newFont.Write(kernel.size);
            }
            newFont.Seek(8);
            newFont.Write((int)newFont.Length - 12);
            var file = File.Create(dir + $"\\new_{binName}");
            file.Write(newFont.ToArray(), 0, (int)newFont.Length);
            file.Close();
        }
    }
}
