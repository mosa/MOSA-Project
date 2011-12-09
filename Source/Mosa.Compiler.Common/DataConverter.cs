//
// Authors:
//   Miguel de Icaza (miguel@novell.com)
// MOSA Contributers:
//   Bruce Markham (illuminus) <illuminus86@gmail.com>
//
//
// See the following url for documentation:
//     http://www.mono-project.com/Mono_DataConvert
//
// NOTE: This file has been extensively modified from the Mono original.
//
// MONO TODO:
//   Support for "DoubleWordsAreSwapped" for ARM devices
// MOSA TODO:
//   Find a better home for this class
//
// Copyright (C) 2006 Novell, Inc (http://www.novell.com)
//  (Some changes may be copyrighted by MOSA contributors or affiliates,
//   but distributed under the following license regardless.)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;

#pragma warning disable 3021

// (NOTE: In the original Mono file, this is actually in the Mono namespace. )
namespace Mosa.Compiler.Common
{
	/// <summary>
	/// Provides platform-aware binary conversion using either a specified or a native
	/// Endianness
	/// </summary>
	unsafe public abstract class DataConverter
		: IEquatable<DataConverter>
	{
		static DataConverter SwapConv = new SwapConverter();
		static DataConverter CopyConv = new CopyConverter();

		/// <summary>
		/// Indicates whether or not the current platform is Little Endian
		/// </summary>
		// NOTE: Renamed from the Mono implementation to not interfere with instance properties
		public static readonly bool NativeIsLittleEndian = BitConverter.IsLittleEndian;

		/// <summary>
		/// Decodes a System.Double from the given bytes, using this instance's Endianness
		/// </summary>
		/// <param name="data">The array of bytes containing the source data</param>
		/// <param name="index">The index within the array of bytes that the source data originates at</param>
		/// <returns>A System.Double decoded from the given bytes</returns>
		/// 
		public abstract double GetDouble(byte[] data, int index);

		/// <summary>
		/// Decodes a System.Float from the given bytes, using this instance's Endianness
		/// </summary>
		/// <param name="data">The array of bytes containing the source data</param>
		/// <param name="index">The index within the array of bytes that the source data originates at</param>
		/// <returns>A System.Float decoded from the given bytes</returns>
		public abstract float GetFloat(byte[] data, int index);

		/// <summary>
		/// Decodes a System.Int64 from the given bytes, using this instance's Endianness
		/// </summary>
		/// <param name="data">The array of bytes containing the source data</param>
		/// <param name="index">The index within the array of bytes that the source data originates at</param>
		/// <returns>A System.Int64 decoded from the given bytes</returns>
		public abstract long GetInt64(byte[] data, int index);

		/// <summary>
		/// Decodes a System.Int32 from the given bytes, using this instance's Endianness
		/// </summary>
		/// <param name="data">The array of bytes containing the source data</param>
		/// <param name="index">The index within the array of bytes that the source data originates at</param>
		/// <returns>A System.Int32 decoded from the given bytes</returns>
		public abstract int GetInt32(byte[] data, int index);

		/// <summary>
		/// Decodes a System.Int16 from the given bytes, using this instance's Endianness
		/// </summary>
		/// <param name="data">The array of bytes containing the source data</param>
		/// <param name="index">The index within the array of bytes that the source data originates at</param>
		/// <returns>A System.Int16 decoded from the given bytes</returns>
		public abstract short GetInt16(byte[] data, int index);

		/// <summary>
		/// Decodes a System.UInt32 from the given bytes, using this instance's Endianness
		/// </summary>
		/// <param name="data">The array of bytes containing the source data</param>
		/// <param name="index">The index within the array of bytes that the source data originates at</param>
		/// <returns>A System.UInt32 decoded from the given bytes</returns>
		[CLSCompliant(false)]
		public abstract uint GetUInt32(byte[] data, int index);

		/// <summary>
		/// Decodes a System.UInt16 from the given bytes, using this instance's Endianness
		/// </summary>
		/// <param name="data">The array of bytes containing the source data</param>
		/// <param name="index">The index within the array of bytes that the source data originates at</param>
		/// <returns>A System.UInt16 decoded from the given bytes</returns>
		[CLSCompliant(false)]
		public abstract ushort GetUInt16(byte[] data, int index);

		/// <summary>
		/// Decodes a System.UInt64 from the given bytes, using this instance's Endianness
		/// </summary>
		/// <param name="data">The array of bytes containing the source data</param>
		/// <param name="index">The index within the array of bytes that the source data originates at</param>
		/// <returns>A System.UInt64 decoded from the given bytes</returns>
		[CLSCompliant(false)]
		public abstract ulong GetUInt64(byte[] data, int index);

		/// <summary>
		/// Puts the binary representation of the given System.Double into the given
		/// destination beginning at the given index
		/// </summary>
		/// <param name="dest">The byte array to write to</param>
		/// <param name="destIdx">The index within the byte array to begin writing at</param>
		/// <param name="value">The value to enccode and write into the destination byte array</param>
		public abstract void PutBytes(byte[] dest, int destIdx, double value);

		/// <summary>
		/// Puts the binary representation of the given System.Float into the given
		/// destination beginning at the given index
		/// </summary>
		/// <param name="dest">The byte array to write to</param>
		/// <param name="destIdx">The index within the byte array to begin writing at</param>
		/// <param name="value">The value to enccode and write into the destination byte array</param>
		public abstract void PutBytes(byte[] dest, int destIdx, float value);

		/// <summary>
		/// Puts the binary representation of the given System.Int32 into the given
		/// destination beginning at the given index
		/// </summary>
		/// <param name="dest">The byte array to write to</param>
		/// <param name="destIdx">The index within the byte array to begin writing at</param>
		/// <param name="value">The value to enccode and write into the destination byte array</param>
		public abstract void PutBytes(byte[] dest, int destIdx, int value);

		/// <summary>
		/// Puts the binary representation of the given System.Int64 into the given
		/// destination beginning at the given index
		/// </summary>
		/// <param name="dest">The byte array to write to</param>
		/// <param name="destIdx">The index within the byte array to begin writing at</param>
		/// <param name="value">The value to enccode and write into the destination byte array</param>
		public abstract void PutBytes(byte[] dest, int destIdx, long value);

		/// <summary>
		/// Puts the binary representation of the given System.Int16 into the given
		/// destination beginning at the given index
		/// </summary>
		/// <param name="dest">The byte array to write to</param>
		/// <param name="destIdx">The index within the byte array to begin writing at</param>
		/// <param name="value">The value to enccode and write into the destination byte array</param>
		public abstract void PutBytes(byte[] dest, int destIdx, short value);

		/// <summary>
		/// Puts the binary representation of the given System.UInt16 into the given
		/// destination beginning at the given index
		/// </summary>
		/// <param name="dest">The byte array to write to</param>
		/// <param name="destIdx">The index within the byte array to begin writing at</param>
		/// <param name="value">The value to enccode and write into the destination byte array</param>
		[CLSCompliant(false)]
		public abstract void PutBytes(byte[] dest, int destIdx, ushort value);

		/// <summary>
		/// Puts the binary representation of the given System.UInt32 into the given
		/// destination beginning at the given index
		/// </summary>
		/// <param name="dest">The byte array to write to</param>
		/// <param name="destIdx">The index within the byte array to begin writing at</param>
		/// <param name="value">The value to enccode and write into the destination byte array</param>
		[CLSCompliant(false)]
		public abstract void PutBytes(byte[] dest, int destIdx, uint value);

		/// <summary>
		/// Puts the binary representation of the given System.UInt64 into the given
		/// destination beginning at the given index
		/// </summary>
		/// <param name="dest">The byte array to write to</param>
		/// <param name="destIdx">The index within the byte array to begin writing at</param>
		/// <param name="value">The value to encode and write into the destination byte array</param>
		[CLSCompliant(false)]
		public abstract void PutBytes(byte[] dest, int destIdx, ulong value);

		/// <summary>
		/// Gets a byte array representing the given System.Double value
		/// </summary>
		/// <param name="value">The value to convert into a byte array</param>
		/// <returns>A byte array representing the provided value</returns>
		public byte[] GetBytes(double value)
		{
			byte[] ret = new byte[8];
			PutBytes(ret, 0, value);
			return ret;
		}

		/// <summary>
		/// Gets a byte array representing the given System.Float value
		/// </summary>
		/// <param name="value">The value to convert into a byte array</param>
		/// <returns>A byte array representing the provided value</returns>
		public byte[] GetBytes(float value)
		{
			byte[] ret = new byte[4];
			PutBytes(ret, 0, value);
			return ret;
		}

		/// <summary>
		/// Gets a byte array representing the given System.Int32 value
		/// </summary>
		/// <param name="value">The value to convert into a byte array</param>
		/// <returns>A byte array representing the provided value</returns>
		public byte[] GetBytes(int value)
		{
			byte[] ret = new byte[4];
			PutBytes(ret, 0, value);
			return ret;
		}

		/// <summary>
		/// Gets a byte array representing the given System.Int64 value
		/// </summary>
		/// <param name="value">The value to convert into a byte array</param>
		/// <returns>A byte array representing the provided value</returns>
		public byte[] GetBytes(long value)
		{
			byte[] ret = new byte[8];
			PutBytes(ret, 0, value);
			return ret;
		}

		/// <summary>
		/// Gets a byte array representing the given System.Int16 value
		/// </summary>
		/// <param name="value">The value to convert into a byte array</param>
		/// <returns>A byte array representing the provided value</returns>
		public byte[] GetBytes(short value)
		{
			byte[] ret = new byte[2];
			PutBytes(ret, 0, value);
			return ret;
		}

		/// <summary>
		/// Gets a byte array representing the given System.UInt16 value
		/// </summary>
		/// <param name="value">The value to convert into a byte array</param>
		/// <returns>A byte array representing the provided value</returns>
		[CLSCompliant(false)]
		public byte[] GetBytes(ushort value)
		{
			byte[] ret = new byte[2];
			PutBytes(ret, 0, value);
			return ret;
		}

		/// <summary>
		/// Gets a byte array representing the given System.UInt32 value
		/// </summary>
		/// <param name="value">The value to convert into a byte array</param>
		/// <returns>A byte array representing the provided value</returns>
		[CLSCompliant(false)]
		public byte[] GetBytes(uint value)
		{
			byte[] ret = new byte[4];
			PutBytes(ret, 0, value);
			return ret;
		}

		/// <summary>
		/// Gets a byte array representing the given System.UInt64 value
		/// </summary>
		/// <param name="value">The value to convert into a byte array</param>
		/// <returns>A byte array representing the provided value</returns>
		[CLSCompliant(false)]
		public byte[] GetBytes(ulong value)
		{
			byte[] ret = new byte[8];
			PutBytes(ret, 0, value);
			return ret;
		}

		/// <summary>
		/// Gets an instance of DataConverter that always outputs in Little Endian
		/// </summary>
		static public DataConverter LittleEndian
		{
			get
			{
				return BitConverter.IsLittleEndian ? CopyConv : SwapConv;
			}
		}

		/// <summary>
		/// Gets an instance of DataConverter that always outputs in Big Endian
		/// </summary>
		static public DataConverter BigEndian
		{
			get
			{
				return BitConverter.IsLittleEndian ? SwapConv : CopyConv;
			}
		}

		/// <summary>
		/// Gets an instance of DataConverter that always outputs in native Endianness
		/// </summary>
		static public DataConverter Native
		{
			get
			{
				return CopyConv;
			}
		}

		/// <summary>
		/// Aligns the given integer to a given alignment
		/// </summary>
		/// <param name="current">The integer to align</param>
		/// <param name="align">The alignment to use</param>
		/// <returns>An integer representing the original value aligned to the given alignment</returns>
		// NOTE: Better documentation needed. I'm not sure what this is. (BMarkham 1/13/09)
		static int Align(int current, int align)
		{
			return ((current + align - 1) / align) * align;
		}

		/// <summary>
		/// Gets whether or not the current instance is encoding in Little Endian format
		/// </summary>
		public bool IsLittleEndian
		{
			get
			{
				return (this.IsNativeEndian && DataConverter.NativeIsLittleEndian)
					|| (!this.IsNativeEndian && !DataConverter.NativeIsLittleEndian);
			}
		}

		/// <summary>
		/// Gets whether or not the current instance is encoding in Big Endian format
		/// </summary>
		public bool IsBigEndian
		{
			get
			{
				return !this.IsLittleEndian;
			}
		}

		/// <summary>
		/// Gets whether or not the current instance is encoding in the platform's native Endian format
		/// </summary>
		public abstract bool IsNativeEndian
		{ get; }

		internal void Check(byte[] dest, int destIdx, int size)
		{
			if (dest == null)
				throw new ArgumentNullException("dest");
			if (destIdx < 0 || destIdx > dest.Length - size)
				throw new ArgumentException("destIdx");
		}

		class CopyConverter : DataConverter
		{
			public override double GetDouble(byte[] data, int index)
			{
				if (data == null)
					throw new ArgumentNullException("data");
				if (data.Length - index < 8)
					throw new ArgumentException("index");
				if (index < 0)
					throw new ArgumentException("index");
				double ret;
				byte* b = (byte*)&ret;

				for (int i = 0; i < 8; i++)
					b[i] = data[index + i];

				return ret;
			}

			public override ulong GetUInt64(byte[] data, int index)
			{
				if (data == null)
					throw new ArgumentNullException("data");
				if (data.Length - index < 8)
					throw new ArgumentException("index");
				if (index < 0)
					throw new ArgumentException("index");

				ulong ret;
				byte* b = (byte*)&ret;

				for (int i = 0; i < 8; i++)
					b[i] = data[index + i];

				return ret;
			}

			public override long GetInt64(byte[] data, int index)
			{
				if (data == null)
					throw new ArgumentNullException("data");
				if (data.Length - index < 8)
					throw new ArgumentException("index");
				if (index < 0)
					throw new ArgumentException("index");

				long ret;
				byte* b = (byte*)&ret;

				for (int i = 0; i < 8; i++)
					b[i] = data[index + i];

				return ret;
			}

			public override float GetFloat(byte[] data, int index)
			{
				if (data == null)
					throw new ArgumentNullException("data");
				if (data.Length - index < 4)
					throw new ArgumentException("index");
				if (index < 0)
					throw new ArgumentException("index");

				float ret;
				byte* b = (byte*)&ret;

				for (int i = 0; i < 4; i++)
					b[i] = data[index + i];

				return ret;
			}

			public override int GetInt32(byte[] data, int index)
			{
				if (data == null)
					throw new ArgumentNullException("data");
				if (data.Length - index < 4)
					throw new ArgumentException("index");
				if (index < 0)
					throw new ArgumentException("index");

				int ret;
				byte* b = (byte*)&ret;

				for (int i = 0; i < 4; i++)
					b[i] = data[index + i];

				return ret;
			}

			public override uint GetUInt32(byte[] data, int index)
			{
				if (data == null)
					throw new ArgumentNullException("data");
				if (data.Length - index < 4)
					throw new ArgumentException("index");
				if (index < 0)
					throw new ArgumentException("index");

				uint ret;
				byte* b = (byte*)&ret;

				for (int i = 0; i < 4; i++)
					b[i] = data[index + i];

				return ret;
			}

			public override short GetInt16(byte[] data, int index)
			{
				if (data == null)
					throw new ArgumentNullException("data");
				if (data.Length - index < 2)
					throw new ArgumentException("index");
				if (index < 0)
					throw new ArgumentException("index");

				short ret;
				byte* b = (byte*)&ret;

				for (int i = 0; i < 2; i++)
					b[i] = data[index + i];

				return ret;
			}

			public override ushort GetUInt16(byte[] data, int index)
			{
				if (data == null)
					throw new ArgumentNullException("data");
				if (data.Length - index < 2)
					throw new ArgumentException("index");
				if (index < 0)
					throw new ArgumentException("index");

				ushort ret;
				byte* b = (byte*)&ret;

				for (int i = 0; i < 2; i++)
					b[i] = data[index + i];

				return ret;
			}

			public override void PutBytes(byte[] dest, int destIdx, double value)
			{
				Check(dest, destIdx, 8);
				fixed (byte* target = &dest[destIdx])
				{
					long* source = (long*)&value;

					*((long*)target) = *source;
				}
			}

			public override void PutBytes(byte[] dest, int destIdx, float value)
			{
				Check(dest, destIdx, 4);
				fixed (byte* target = &dest[destIdx])
				{
					uint* source = (uint*)&value;

					*((uint*)target) = *source;
				}
			}

			public override void PutBytes(byte[] dest, int destIdx, int value)
			{
				Check(dest, destIdx, 4);
				fixed (byte* target = &dest[destIdx])
				{
					uint* source = (uint*)&value;

					*((uint*)target) = *source;
				}
			}

			public override void PutBytes(byte[] dest, int destIdx, uint value)
			{
				Check(dest, destIdx, 4);
				fixed (byte* target = &dest[destIdx])
				{
					uint* source = (uint*)&value;

					*((uint*)target) = *source;
				}
			}

			public override void PutBytes(byte[] dest, int destIdx, long value)
			{
				Check(dest, destIdx, 8);
				fixed (byte* target = &dest[destIdx])
				{
					long* source = (long*)&value;

					*((long*)target) = *source;
				}
			}

			public override void PutBytes(byte[] dest, int destIdx, ulong value)
			{
				Check(dest, destIdx, 8);
				fixed (byte* target = &dest[destIdx])
				{
					ulong* source = (ulong*)&value;

					*((ulong*)target) = *source;
				}
			}

			public override void PutBytes(byte[] dest, int destIdx, short value)
			{
				Check(dest, destIdx, 2);
				fixed (byte* target = &dest[destIdx])
				{
					ushort* source = (ushort*)&value;

					*((ushort*)target) = *source;
				}
			}

			public override void PutBytes(byte[] dest, int destIdx, ushort value)
			{
				Check(dest, destIdx, 2);
				fixed (byte* target = &dest[destIdx])
				{
					ushort* source = (ushort*)&value;

					*((ushort*)target) = *source;
				}
			}

			public override bool IsNativeEndian
			{
				get { return true; }
			}
		}

		class SwapConverter : DataConverter
		{
			public override double GetDouble(byte[] data, int index)
			{
				if (data == null)
					throw new ArgumentNullException("data");
				if (data.Length - index < 8)
					throw new ArgumentException("index");
				if (index < 0)
					throw new ArgumentException("index");

				double ret;
				byte* b = (byte*)&ret;

				for (int i = 0; i < 8; i++)
					b[7 - i] = data[index + i];

				return ret;
			}

			public override ulong GetUInt64(byte[] data, int index)
			{
				if (data == null)
					throw new ArgumentNullException("data");
				if (data.Length - index < 8)
					throw new ArgumentException("index");
				if (index < 0)
					throw new ArgumentException("index");

				ulong ret;
				byte* b = (byte*)&ret;

				for (int i = 0; i < 8; i++)
					b[7 - i] = data[index + i];

				return ret;
			}

			public override long GetInt64(byte[] data, int index)
			{
				if (data == null)
					throw new ArgumentNullException("data");
				if (data.Length - index < 8)
					throw new ArgumentException("index");
				if (index < 0)
					throw new ArgumentException("index");

				long ret;
				byte* b = (byte*)&ret;

				for (int i = 0; i < 8; i++)
					b[7 - i] = data[index + i];

				return ret;
			}

			public override float GetFloat(byte[] data, int index)
			{
				if (data == null)
					throw new ArgumentNullException("data");
				if (data.Length - index < 4)
					throw new ArgumentException("index");
				if (index < 0)
					throw new ArgumentException("index");

				float ret;
				byte* b = (byte*)&ret;

				for (int i = 0; i < 4; i++)
					b[3 - i] = data[index + i];

				return ret;
			}

			public override int GetInt32(byte[] data, int index)
			{
				if (data == null)
					throw new ArgumentNullException("data");
				if (data.Length - index < 4)
					throw new ArgumentException("index");
				if (index < 0)
					throw new ArgumentException("index");

				int ret;
				byte* b = (byte*)&ret;

				for (int i = 0; i < 4; i++)
					b[3 - i] = data[index + i];

				return ret;
			}

			public override uint GetUInt32(byte[] data, int index)
			{
				if (data == null)
					throw new ArgumentNullException("data");
				if (data.Length - index < 4)
					throw new ArgumentException("index");
				if (index < 0)
					throw new ArgumentException("index");

				uint ret;
				byte* b = (byte*)&ret;

				for (int i = 0; i < 4; i++)
					b[3 - i] = data[index + i];

				return ret;
			}

			public override short GetInt16(byte[] data, int index)
			{
				if (data == null)
					throw new ArgumentNullException("data");
				if (data.Length - index < 2)
					throw new ArgumentException("index");
				if (index < 0)
					throw new ArgumentException("index");

				short ret;
				byte* b = (byte*)&ret;

				for (int i = 0; i < 2; i++)
					b[1 - i] = data[index + i];

				return ret;
			}

			public override ushort GetUInt16(byte[] data, int index)
			{
				if (data == null)
					throw new ArgumentNullException("data");
				if (data.Length - index < 2)
					throw new ArgumentException("index");
				if (index < 0)
					throw new ArgumentException("index");

				ushort ret;
				byte* b = (byte*)&ret;

				for (int i = 0; i < 2; i++)
					b[1 - i] = data[index + i];

				return ret;
			}

			public override void PutBytes(byte[] dest, int destIdx, double value)
			{
				Check(dest, destIdx, 8);

				fixed (byte* target = &dest[destIdx])
				{
					byte* source = (byte*)&value;

					for (int i = 0; i < 8; i++)
						target[i] = source[7 - i];
				}
			}

			public override void PutBytes(byte[] dest, int destIdx, float value)
			{
				Check(dest, destIdx, 4);

				fixed (byte* target = &dest[destIdx])
				{
					byte* source = (byte*)&value;

					for (int i = 0; i < 4; i++)
						target[i] = source[3 - i];
				}
			}

			public override void PutBytes(byte[] dest, int destIdx, int value)
			{
				Check(dest, destIdx, 4);

				fixed (byte* target = &dest[destIdx])
				{
					byte* source = (byte*)&value;

					for (int i = 0; i < 4; i++)
						target[i] = source[3 - i];
				}
			}

			public override void PutBytes(byte[] dest, int destIdx, uint value)
			{
				Check(dest, destIdx, 4);

				fixed (byte* target = &dest[destIdx])
				{
					byte* source = (byte*)&value;

					for (int i = 0; i < 4; i++)
						target[i] = source[3 - i];
				}
			}

			public override void PutBytes(byte[] dest, int destIdx, long value)
			{
				Check(dest, destIdx, 8);

				fixed (byte* target = &dest[destIdx])
				{
					byte* source = (byte*)&value;

					for (int i = 0; i < 8; i++)
						target[i] = source[7 - i];
				}
			}

			public override void PutBytes(byte[] dest, int destIdx, ulong value)
			{
				Check(dest, destIdx, 8);

				fixed (byte* target = &dest[destIdx])
				{
					byte* source = (byte*)&value;

					for (int i = 0; i < 4; i++)
						target[i] = source[7 - i];
				}
			}

			public override void PutBytes(byte[] dest, int destIdx, short value)
			{
				Check(dest, destIdx, 2);

				fixed (byte* target = &dest[destIdx])
				{
					byte* source = (byte*)&value;

					for (int i = 0; i < 2; i++)
						target[i] = source[1 - i];
				}
			}

			public override void PutBytes(byte[] dest, int destIdx, ushort value)
			{
				Check(dest, destIdx, 2);

				fixed (byte* target = &dest[destIdx])
				{
					byte* source = (byte*)&value;

					for (int i = 0; i < 2; i++)
						target[i] = source[1 - i];
				}
			}

			public override bool IsNativeEndian
			{
				get { return false; }
			}
		}

		#region IEquatable<DataConverter> Members

		/// <summary>
		/// Determines whether or not the current DataConverter uses the same
		/// Endian-ness of the given DataConverter instance
		/// </summary>
		/// <param name="other">The DataConverter to check for equality against</param>
		/// <returns>True if Endian-ness is equal, otherwise false</returns>
		/// <remarks>Makes comparison by comparing the 'IsNativeEndian' property on both instances</remarks>
		public bool Equals(DataConverter other)
		{
			return this.IsNativeEndian == other.IsNativeEndian;
		}

		#endregion

		/// <summary>
		/// Determines if the current object is equal to the given one
		/// </summary>
		/// <param name="obj">The object that equality is being compared against</param>
		/// <returns>True if the objects are the same, otherwise false</returns>
		public override bool Equals(object obj)
		{
			if (obj == null || !typeof(DataConverter).IsAssignableFrom(obj.GetType()))
				return base.Equals(obj);
			else
				return this.Equals(obj as DataConverter);
		}

		/// <summary>
		/// Gets the runtime hash code for the object
		/// </summary>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
