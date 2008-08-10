/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.ClassLib
{
    public struct BinaryFormat
    {
        private byte[] data;

        public BinaryFormat(byte[] data) { this.data = data; }

        public BinaryFormat(uint length) { this.data = new byte[length]; }

        public uint Length
        {
            get { return (uint)data.Length; }
        }

        public byte[] Data
        {
            get { return data; }
        }

        public byte this[int index]
        {
            get { return data[index]; }
            set { data[index] = value; }
        }

        public char GetChar(uint offset) { return (char)(data[offset]); }

        public void SetChar(uint offset, char value) { data[offset] = (byte)value; }

        public char[] GetChars(uint offset, uint length)
        {
            char[] value = new char[length];

            for (uint index = 0; index < length; index++) { value[index] = GetChar(offset + index); }

            return value;
        }

        public byte[] GetBytes(uint offset, uint length)
        {
            byte[] value = new byte[length];

            for (uint index = 0; index < length; index++) { value[index] = data[offset + index]; }

            return value;
        }

        public void SetChars(uint offset, char[] value)
        {
            for (uint index = 0; index < value.Length; index++) { data[offset + index] = (byte)value[index]; }
        }

        public void SetString(uint offset, string value)
        {
            for (int index = 0; index < value.Length; index++) { data[offset + index] = (byte)value[index]; }
        }

        public void SetString(uint offset, string value, uint length)
        {
            for (int index = 0; index < length; index++) { data[offset + index] = (byte)value[index]; }
        }

        public uint GetUInt(uint offset)
        {
            uint value = data[offset++];
            value += (uint)(data[offset++] << 8);
            value += (uint)(data[offset++] << 16);
            value += (uint)(data[offset++] << 24);

            return value;
        }

        public void SetUInt(uint offset, uint value)
        {
            data[offset++] = (byte)(value & 0xFF);
            data[offset++] = (byte)((value >> 8) & 0xFF);
            data[offset++] = (byte)((value >> 16) & 0xFF);
            data[offset++] = (byte)((value >> 24) & 0xFF);
        }

        public ushort GetUShort(uint offset)
        {
            ushort value = data[offset++];
            value += (ushort)(data[offset++] << 8);

            return value;
        }

        public void SetUShort(uint offset, ushort value)
        {
            data[offset++] = (byte)(value & 0xFF);
            data[offset++] = (byte)((value >> 8) & 0xFF);
        }

        public int GetInt(uint offset) { return (int)GetUInt(offset); }

        public void SetInt(uint offset, int value) { SetUInt(offset, (uint)value); }

        public byte GetByte(uint offset) { return data[offset]; }

        public void SetByte(uint offset, byte value) { data[offset] = value; }

        public void SetBytes(uint offset, byte[] value)
        {
            for (uint index = 0; index < value.Length; index++) { data[offset] = value[index]; }
        }

        public void SetBytes(uint offset, byte[] value, uint start, uint length)
        {
            for (uint index = 0; index < length; index++) { data[offset] = value[start + index]; }
        }

        public void Fill(uint offset, byte value, uint length)
        {
            for (uint index = 0; index < length; index++) { data[offset] = value; }
        }
    }
}
