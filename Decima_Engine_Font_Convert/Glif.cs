using Decima_Engine_Font_Convert.FontInfo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Decima_Engine_Font_Convert
{
    public class Glif
    {
        public void writeGlyphs(List<Glyph> glyphs, string dir)
        {
            foreach(Glyph glyph in glyphs)
            {
                byte[] charnamebytes = Encoding.Unicode.GetBytes(glyph.Character);
                Array.Reverse(charnamebytes, 0, 2);
                string charnamestringbytes = BitConverter.ToString(charnamebytes).Replace("-", "");
                string charname = "uni" + BitConverter.ToString(charnamebytes).Replace("-", "");

                if(glyph.Character == " ")
                {
                    XmlTextWriter writer = new XmlTextWriter(dir + "\\_notdef.glif", System.Text.Encoding.UTF8);
                    writer.WriteStartDocument();
                    writer.Formatting = Formatting.Indented;
                    writer.Indentation = 2;

                    writer.WriteStartElement("glyph");
                    writer.WriteAttributeString("name", ".notdef");
                    writer.WriteAttributeString("format", "2");

                        writer.WriteStartElement("advance");
                        writer.WriteAttributeString("width", "250");
                        writer.WriteEndElement();

                    writer.WriteEndElement();

                    writer.WriteEndDocument();
                    writer.Close();
                }
                else
                {
                    XmlTextWriter writer = new XmlTextWriter(dir + $"\\{charname}.glif", System.Text.Encoding.UTF8);
                    writer.WriteStartDocument();
                    writer.Formatting = Formatting.Indented;
                    writer.Indentation = 2;

                    writer.WriteStartElement("glyph");
                    writer.WriteAttributeString("name", $"{charname}");
                    writer.WriteAttributeString("format", "2");
                    {
                        writer.WriteStartElement("advance");
                        writer.WriteAttributeString("width", $"{glyph.AdvWidth}");
                        writer.WriteEndElement();

                        writer.WriteStartElement("unicode");
                        writer.WriteAttributeString("hex", $"{charnamestringbytes}");
                        writer.WriteEndElement();

                        writer.WriteStartElement("outline");

                        foreach(var pivot in glyph.Pivots)
                        {
                            writer.WriteStartElement("contour");
                            int i = 0;
                            bool isctpoint = false;
                            int j = 0;
                            //Debug.WriteLine(BitConverter.ToString(pivot.signal.ToArray()));
                            foreach (var point in pivot.Points)
                            {
                                writer.WriteStartElement("point");
                                writer.WriteAttributeString("x", $"{point.X}");
                                writer.WriteAttributeString("y", $"{point.Y}");
                                //if (pivot.signal[i] >= 0x32)
                                {

                                }
                                /*else if(pivot.signal[i] == 0x43)
                                {

                                }
                                else if (pivot.signal[i] == 0x44)
                                {

                                }
                                else if (pivot.signal[i] == 0x45)
                                {

                                }*/
                               // else
                                {
                                  /*  if (j == 0)
                                        j = pivot.signal[i];*/
                                    writer.WriteAttributeString("type", "line");
                                    /*j--;
                                    if (j == 0 && pivot.signal.Count > 1)
                                        i++;*/
                                }
                                writer.WriteEndElement();
                            }

                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Close();
                }

            }
        }
    }
}
