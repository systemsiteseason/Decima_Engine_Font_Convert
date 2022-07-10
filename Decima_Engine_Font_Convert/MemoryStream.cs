using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Decima_Engine_Font_Convert
{
    class MemoryStream : System.IO.MemoryStream
    {
        public MemoryStream(byte[] stream) : base(stream) { }

        public MemoryStream() : base() { }

        #region Edit Method
        public void Insert(int index, byte[] des)
        {
            List<byte> output = new List<byte>();
            output.AddRange(base.ToArray());
            if (output.Count < index)
            {
                output.AddRange(new byte[index - output.Count]);
            }
            output.InsertRange(index, des);
            byte[] buffer = base.GetBuffer();
            Array.Clear(buffer, 0, buffer.Length);
            base.SetLength(0);
            base.Write(output.ToArray(), 0, output.ToArray().Length);
        }

        public MemoryStream Insert(byte[] des)
        {

            List<byte> output = new List<byte>();
            output.AddRange(base.ToArray());
            if (output.Count < base.Position)
            {
                output.AddRange(new byte[base.Position - output.Count]);
            }
            output.InsertRange((int)base.Position, des);
            var editStream = new MemoryStream(output.ToArray());
            editStream.Seek(base.Position + des.Length);
            return editStream;
        }

        public MemoryStream Remove(int index, int size)
        {
            List<byte> output = new List<byte>();
            output.AddRange(base.ToArray());
            output.RemoveRange(index, size);
            var editStream = new MemoryStream(output.ToArray());
            editStream.Seek(index);
            return editStream;
        }
        #endregion

        #region Write Method
        //Write Method
        public void Write(byte[] input)
        {
            base.Write(input, 0, input.Length);
        }

        public void Write(string input, Encoding encoding)
        {
            base.Write(encoding.GetBytes(input), 0, encoding.GetBytes(input).Length);
        }

        public void Write(double input)
        {
            base.Write(BitConverter.GetBytes(input), 0, 8);
        }

        public void Write(float input)
        {
            base.Write(BitConverter.GetBytes(input), 0, 4);
        }

        public void Write(Int64 input)
        {
            base.Write(BitConverter.GetBytes(input), 0, 8);
        }

        public void Write(int input)
        {
            base.Write(BitConverter.GetBytes(input), 0, 4);
        }

        public void Write(short input)
        {
            base.Write(BitConverter.GetBytes(input), 0, 2);
        }

        public void Write(byte input)
        {
            base.WriteByte(input);
        }

        #endregion

        #region Read Method
        //Read Method
        public byte[] ReadBytes(int size)
        {
            var data = new byte[size];
            base.Read(data, 0, size);
            return data;
        }

        public byte[] ReadToEnd()
        {
            var data = new byte[base.Length - base.Position];
            base.Read(data, 0, (int)(base.Length - base.Position));
            return data;
        }

        public byte[] ReadBytes(long size)
        {
            var data = new byte[size];
            base.Read(data, 0, (int)size);
            return data;
        }

        public Int16 ReadInt16()
        {
            var data = new byte[2];
            base.Read(data, 0, 2);
            return BitConverter.ToInt16(data, 0);
        }

        public UInt16 ReadUInt16()
        {
            var data = new byte[2];
            base.Read(data, 0, 2);
            return BitConverter.ToUInt16(data, 0);
        }

        public Int32 ReadInt32()
        {
            var data = new byte[4];
            base.Read(data, 0, 4);
            return BitConverter.ToInt32(data, 0);
        }

        public UInt32 ReadUInt32()
        {
            var data = new byte[4];
            base.Read(data, 0, 4);
            return BitConverter.ToUInt32(data, 0);
        }

        public Int64 ReadInt64()
        {
            var data = new byte[8];
            base.Read(data, 0, 8);
            return BitConverter.ToInt64(data, 0);
        }

        public UInt64 ReadUInt64()
        {
            var data = new byte[8];
            base.Read(data, 0, 8);
            return BitConverter.ToUInt64(data, 0);
        }

        public float ReadSingle()
        {
            var data = new byte[4];
            base.Read(data, 0, 4);
            return BitConverter.ToSingle(data, 0);
        }

        public string ReadString(int size, Encoding encoding)
        {
            var data = new byte[size];
            base.Read(data, 0, size);
            return encoding.GetString(data);
        }

        public string ReadString()
        {
            List<byte> output = new List<byte>();
            byte reader = (byte)base.ReadByte();
            do
            {
                output.Add(reader);
                reader = (byte)base.ReadByte();
            } while (reader != 0);
            base.Seek(-1, SeekOrigin.Current);
            return Encoding.ASCII.GetString(output.ToArray());
        }
        #endregion

        #region Location Method
        //Location Method
        public void Skip(int to)
        {
            base.Seek(to, SeekOrigin.Current);
        }

        public void Skip(long to)
        {
            base.Seek(to, SeekOrigin.Current);
        }


        public long Tell()
        {
            return base.Position;
        }

        public void Seek(long to)
        {
            base.Seek(to, SeekOrigin.Begin);
        }

        public void INDIE()
        {

        }
        #endregion
    }
}
