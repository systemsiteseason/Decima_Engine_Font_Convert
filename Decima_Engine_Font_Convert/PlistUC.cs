using Decima_Engine_Font_Convert.FontInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Decima_Engine_Font_Convert
{
    public class PlistUC
    {
        public void MetaInfo(string dir)
        {
            XmlTextWriter writer = new XmlTextWriter(dir + "\\metainfo.plist", System.Text.Encoding.UTF8);
            writer.WriteStartDocument();
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            writer.WriteDocType("plist", "-//Apple//DTD PLIST 1.0//EN", "http://www.apple.com/DTDs/PropertyList-1.0.dtd", null);
            writer.WriteStartElement("plist");
            writer.WriteAttributeString("version", "1.0");
            writer.WriteStartElement("dict");
            writer.WriteElementString("key", "creator");
            writer.WriteElementString("string", "com.fontlab.ufoLib");
            writer.WriteElementString("key", "formatVersion");
            writer.WriteElementString("integer", "3");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        public void LayerContents(string dir)
        {
            XmlTextWriter writer = new XmlTextWriter(dir + "\\layercontents.plist", System.Text.Encoding.UTF8);
            writer.WriteStartDocument();
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            writer.WriteDocType("plist", "-//Apple//DTD PLIST 1.0//EN", "http://www.apple.com/DTDs/PropertyList-1.0.dtd", null);
            writer.WriteStartElement("plist");
            writer.WriteAttributeString("version", "1.0");
            writer.WriteStartElement("array");
            writer.WriteStartElement("array");
            writer.WriteElementString("string", "public.default");
            writer.WriteElementString("string", "glyphs");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        public void LayerInfo(string dir)
        {
            XmlTextWriter writer = new XmlTextWriter(dir + "\\layerinfo.plist", System.Text.Encoding.UTF8);
            writer.WriteStartDocument();
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            writer.WriteDocType("plist", "-//Apple//DTD PLIST 1.0//EN", "http://www.apple.com/DTDs/PropertyList-1.0.dtd", null);
            writer.WriteStartElement("plist");
            writer.WriteAttributeString("version", "1.0");
            writer.WriteStartElement("dict");
            writer.WriteElementString("key", "lib");
            writer.WriteStartElement("dict");
            writer.WriteElementString("key", "com.fontlab.layer.name");
            writer.WriteElementString("string", "Regular");
            writer.WriteElementString("key", "com.fontlab.layer.opacity");
            writer.WriteElementString("integer", "1");
            writer.WriteElementString("key", "com.fontlab.layer.visible");
            writer.WriteStartElement("false");
            writer.WriteEndElement();
            writer.WriteElementString("key", "com.fontlab.layer.locked");
            writer.WriteStartElement("false");
            writer.WriteEndElement();
            writer.WriteElementString("key", "com.fontlab.layer.service");
            writer.WriteStartElement("false");
            writer.WriteEndElement();
            writer.WriteElementString("key", "com.fontlab.layer.wireframe");
            writer.WriteStartElement("false");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        public void Contents(List<Glyph> glyphs, string dir)
        {
            XmlTextWriter writer = new XmlTextWriter(dir + "\\contents.plist", System.Text.Encoding.UTF8);
            writer.WriteStartDocument();
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            writer.WriteDocType("plist", "-//Apple//DTD PLIST 1.0//EN", "http://www.apple.com/DTDs/PropertyList-1.0.dtd", null);
            writer.WriteStartElement("plist");
            writer.WriteAttributeString("version", "1.0");
            writer.WriteStartElement("dict");
            foreach (Glyph glyph in glyphs)
            {
                byte[] charnamebytes = Encoding.Unicode.GetBytes(glyph.Character);
                Array.Reverse(charnamebytes, 0, 2);
                string charname = "uni" + BitConverter.ToString(charnamebytes).Replace("-", "");

                if (glyph.Character == " ")
                {
                    writer.WriteElementString("key", ".notdef");
                    writer.WriteElementString("string", "_notdef.glif");
                }
                else
                {
                    writer.WriteElementString("key", $"{charname}");
                    writer.WriteElementString("string", $"{charname}.glif");
                }
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        public void Lib(List<Glyph> glyphs, string dir)
        {
            XmlTextWriter writer = new XmlTextWriter(dir + "\\lib.plist", System.Text.Encoding.UTF8);
            writer.WriteStartDocument();
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            writer.WriteDocType("plist", "-//Apple//DTD PLIST 1.0//EN", "http://www.apple.com/DTDs/PropertyList-1.0.dtd", null);
            writer.WriteStartElement("plist");
            writer.WriteAttributeString("version", "1.0");
            writer.WriteStartElement("dict");

            writer.WriteElementString("key", "public.glyphOrder");
            writer.WriteStartElement("array");
            foreach (Glyph glyph in glyphs)
            {
                byte[] charnamebytes = Encoding.Unicode.GetBytes(glyph.Character);
                Array.Reverse(charnamebytes, 0, 2);
                string charname = "uni" + BitConverter.ToString(charnamebytes).Replace("-", "");

                if (glyph.Character == " ")
                    writer.WriteElementString("string", ".notdef");
                else
                    writer.WriteElementString("string", $"{charname}");
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        public void FontInfo(FontGlyphs font, string dir)
        {
            XmlTextWriter writer = new XmlTextWriter(dir + "\\fontinfo.plist", System.Text.Encoding.UTF8);
            writer.WriteStartDocument();
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            writer.WriteDocType("plist", "-//Apple//DTD PLIST 1.0//EN", "http://www.apple.com/DTDs/PropertyList-1.0.dtd", null);
            writer.WriteStartElement("plist");
            writer.WriteAttributeString("version", "1.0");
            writer.WriteStartElement("dict");
            {
                writer.WriteElementString("key", "ascender");
                writer.WriteElementString("integer", $"{font.ascender}");
                writer.WriteElementString("key", "capHeight");
                writer.WriteElementString("integer", $"{font.ascender}");
                writer.WriteElementString("key", "descender");
                writer.WriteElementString("integer", $"{font.descender * -1}");
                writer.WriteElementString("key", "familyName");
                writer.WriteElementString("string", $"{font.familyName}");
                writer.WriteElementString("key", "guidelines");
                writer.WriteStartElement("array");
                writer.WriteStartElement("dict");
                {
                    writer.WriteElementString("key", "angle");
                    writer.WriteElementString("integer", $"0");
                    writer.WriteElementString("key", "name");
                    writer.WriteElementString("string", $"m");
                    writer.WriteElementString("key", "x");
                    writer.WriteElementString("integer", $"0");
                    writer.WriteElementString("key", "y");
                    writer.WriteElementString("integer", $"{font.descender}");
                }
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteElementString("key", "italicAngle");
                writer.WriteElementString("integer", $"0");
                writer.WriteElementString("key", "openTypeGaspRangeRecords");

                writer.WriteStartElement("array");
                writer.WriteStartElement("dict");
                {
                    writer.WriteElementString("key", "rangeGaspBehavior");
                    writer.WriteStartElement("array");
                    {
                        writer.WriteElementString("integer", $"0");
                        writer.WriteElementString("integer", $"1");
                        writer.WriteElementString("integer", $"2");
                        writer.WriteElementString("integer", $"3");
                    }
                    writer.WriteEndElement();
                    writer.WriteElementString("key", "rangeMaxPPEM");
                    writer.WriteElementString("integer", $"65535");
                }
                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.WriteElementString("key", "openTypeHeadCreated");
                writer.WriteElementString("string", $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") }");
                writer.WriteElementString("key", "openTypeHeadFlags");

                writer.WriteElementString("array", "\n\t");

                writer.WriteElementString("key", "openTypeHheaCaretOffset");
                writer.WriteElementString("integer", $"0");
                writer.WriteElementString("key", "openTypeNamePreferredFamilyName");
                writer.WriteElementString("string", $"{font.familyName}");
                writer.WriteElementString("key", "openTypeNamePreferredSubfamilyName");
                writer.WriteElementString("string", $"Regular");
                writer.WriteElementString("key", "openTypeNameVersion");
                writer.WriteElementString("string", $"Version 1.000");
                writer.WriteElementString("key", "openTypeOS2Selection");

                writer.WriteStartElement("array");
                writer.WriteElementString("integer", "7");
                writer.WriteEndElement();

                writer.WriteElementString("key", "openTypeOS2Type");

                writer.WriteStartElement("array");
                writer.WriteElementString("integer", "2");
                writer.WriteEndElement();

                writer.WriteElementString("key", "openTypeOS2TypoAscender");
                writer.WriteElementString("integer", $"{font.openTypeOS2TypoAscender}");
                writer.WriteElementString("key", "openTypeOS2TypoDescender");
                writer.WriteElementString("integer", $"{font.descender * -1}");
                writer.WriteElementString("key", "openTypeOS2TypoLineGap");
                writer.WriteElementString("integer", $"100");
                writer.WriteElementString("key", "openTypeOS2WeightClass");
                writer.WriteElementString("integer", $"400");
                writer.WriteElementString("key", "openTypeOS2WidthClass");
                writer.WriteElementString("integer", $"5");
                writer.WriteElementString("key", "openTypeOS2WinAscent");
                writer.WriteElementString("integer", $"{font.ascender}");
                writer.WriteElementString("key", "openTypeOS2WinDescent");
                writer.WriteElementString("integer", $"{font.descender}");
                writer.WriteElementString("key", "postscriptBlueFuzz");
                writer.WriteElementString("integer", $"1");
                writer.WriteElementString("key", "postscriptBlueScale");
                writer.WriteElementString("real", $"0.039625");
                writer.WriteElementString("key", "postscriptBlueShift");
                writer.WriteElementString("integer", $"7");

                writer.WriteElementString("key", "postscriptBlueValues");

                writer.WriteStartElement("array");
                writer.WriteElementString("integer", "-20");
                writer.WriteElementString("integer", "0");
                writer.WriteEndElement();

                writer.WriteElementString("key", "postscriptFontName");
                writer.WriteElementString("string", $"{font.familyName}-Regular");
                writer.WriteElementString("key", "postscriptForceBold");
                writer.WriteStartElement("false");
                writer.WriteEndElement();
                writer.WriteElementString("key", "postscriptFullName");
                writer.WriteElementString("string", $" {font.familyName} Regular");
                writer.WriteElementString("key", "postscriptIsFixedPitch");
                writer.WriteStartElement("false");
                writer.WriteEndElement();
                writer.WriteElementString("key", "postscriptUnderlinePosition");
                writer.WriteElementString("integer", $"-100");
                writer.WriteElementString("key", "postscriptUnderlineThickness");
                writer.WriteElementString("integer", $"50");
                writer.WriteElementString("key", "postscriptWeightName");
                writer.WriteElementString("string", "Regular");
                writer.WriteElementString("key", "styleMapFamilyName");
                writer.WriteElementString("string", $"{font.familyName}");
                writer.WriteElementString("key", "styleMapStyleName");
                writer.WriteElementString("string", "regular");
                writer.WriteElementString("key", "styleName");
                writer.WriteElementString("string", "Regular");
                writer.WriteElementString("key", "unitsPerEm");
                writer.WriteElementString("integer", $"{font.unitsPerEm}");
                writer.WriteElementString("key", "versionMajor");
                writer.WriteElementString("integer", "1");
                writer.WriteElementString("key", "versionMinor");
                writer.WriteElementString("integer", "0");
                writer.WriteElementString("key", "xHeight");
                writer.WriteElementString("integer", $"{font.ascender - font.descender}");
            }
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }
    }
}
