/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Sebastian Loncar (Arakis) <sebastian.loncar@gmail.com>
 */

using System.Runtime.InteropServices;

namespace Mosa.Platform.Internal.x86
{
	/// <summary>
	/// Represents a string as struct, so it can used before memory and runtime initialization.
	/// Use only where needed. Do not incease the struct size more as needed. A good limit would be the maximum horizontal text resolution.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = 132 * 2 + 4)]
	public struct StringBuffer
	{
		[FieldOffset(0)]
		private int length;

		//[FieldOffset(4)]
		//private byte isSet; //Compiler crash!

		public const int MaxLength = 132;
		public const int EntrySize = 132 * 2 + 4;

		//[FieldOffset(4)]
		//unsafe private char* chars;

		unsafe public static StringBuffer CreateFromNullTerminatedString(uint start)
		{
			return CreateFromNullTerminatedString((byte*)start);
		}

		unsafe public static StringBuffer CreateFromNullTerminatedString(byte* start)
		{
			var buf = new StringBuffer();
			while (*start != 0)
			{
				buf.Append((char)*start++);
			}
			return buf;
		}

		private unsafe char* firstChar()
		{
			//Does not work!
			//return (char*)((uint)(Mosa.Internal.Intrinsic.GetValueTypeAddress(this)) + 4);

			//Compiler crash
			//fixed (void* ptr = &this)
			//return (char*)(((uint)ptr) + 4);

			//Workarround
			uint ui;
			fixed (void* ptr = &this)
				ui = (uint)ptr;
			return (char*)(ui + 4);
		}

		/// <summary>
		/// Acces a char at a specific index
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		unsafe public char this[int index]
		{
			get
			{
				if (index >= Length) //TODO: Error
					return '\x0';
				return firstChar()[index];
			}
			set
			{
				if (index >= Length) //TODO: Error
					return;
				firstChar()[index] = value;
			}
		}

		public void Clear()
		{
			length = 0;
		}

		/// <summary>
		/// Overwrite the current value with a new one
		/// </summary>
		/// <param name="value"></param>
		public void Set(string value)
		{
			Clear();
			//if (value == null)
			//	isSet = 0;
			//else
			Append(value);
		}

		//public bool IsNull
		//{
		//	get { return isSet == 0; }
		//}

		#region Constructor

		public StringBuffer(string value)
			: this()
		{
			Append(value);
		}

		public StringBuffer(byte value)
			: this()
		{
			Append(value);
		}

		public StringBuffer(int value)
			: this()
		{
			Append(value);
		}

		public StringBuffer(int value, string format)
			: this()
		{
			Append(value, format);
		}

		public StringBuffer(uint value)
			: this()
		{
			Append(value);
		}

		public StringBuffer(uint value, string format)
			: this()
		{
			Append(value, format);
		}

		#endregion Constructor

		#region Append

		/// <summary>
		/// Appends a string
		/// </summary>
		/// <param name="value"></param>
		public void Append(string value)
		{
			if (value == null)
				return;

			for (var i = 0; i < value.Length; i++)
				Append(value[i]);
		}

		public void Append(string value, int start)
		{
			if (value == null) return;
			Append(value, start, value.Length - start);
		}

		public void Append(string value, int start, int length)
		{
			if (value == null) return;
			for (var i = 0; i < length; i++)
				Append(value[i + start]);
		}

		public void Append(StringBuffer value)
		{
			if (value.length == 0)
				return;

			for (var i = 0; i < value.Length; i++)
				Append(value[i]);
		}

		public void Append(StringBuffer value, int start)
		{
			if (value.length == 0) return;
			Append(value, start, value.Length - start);
		}

		public void Append(StringBuffer value, int start, int length)
		{
			if (value.length == 0) return;
			for (var i = 0; i < length; i++)
				Append(value[i + start]);
		}

		unsafe public void Append(char value)
		{
			if (length + 1 >= MaxLength)
			{
				//TODO: Error
				return;
			}
			//isSet = 1;
			length++;
			this[length - 1] = value;
		}

		public void Append(uint value)
		{
			Append(value, false, false);
		}

		public void Append(int value)
		{
			Append((uint)value, true, false);
		}

		/// <summary>
		/// Appends a number to the string. Use format to output as Hex.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="format"></param>
		public void Append(uint value, string format)
		{
			Append(value, false, true);
		}

		/// <summary>
		/// Appends a number to the string. Use format to output as Hex.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="format"></param>
		public void Append(int value, string format)
		{
			var u = (uint)value;
			Append(u, true, true);
		}

		unsafe internal void Append(uint value, bool signed, bool hex)
		{
			int offset = 0;

			uint uvalue = (uint)value;
			ushort divisor = hex ? (ushort)16 : (ushort)10;
			int len = 0;
			int count = 0;
			uint temp;
			bool negative = false;

			if (value < 0 && !hex && signed)
			{
				count++;
				uvalue = (uint)-value;
				negative = true;
			}

			temp = uvalue;

			do
			{
				temp /= divisor;
				count++;
			}
			while (temp != 0);

			var first = (firstChar() + this.length);

			len = count;
			Length += len;

			if (negative)
			{
				*(first + offset) = '-';
				offset++;
				count--;
			}

			for (int i = 0; i < count; i++)
			{
				uint remainder = uvalue % divisor;

				if (remainder < 10)
					*(first + offset + count - 1 - i) = (char)('0' + remainder);
				else
					*(first + offset + count - 1 - i) = (char)('A' + remainder - 10);

				uvalue /= divisor;
			}
		}

		#endregion Append

		/// <summary>
		/// The length of the string
		/// </summary>
		/// <value>
		/// The length.
		/// </value>
		public int Length
		{
			get { return length; }
			set
			{
				if (value > MaxLength)
				{
					//TODO: Error
					value = MaxLength;
				}
				length = value;
				//isSet = 1;
			}
		}

		/// <summary>
		/// Gets the index of a specific value
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int IndexOf(string value)
		{
			if (this.length == 0)
				return -1;

			return IndexOfImpl(value, 0, this.length);
		}

		private int IndexOfImpl(string value, int startIndex, int count)
		{
			for (int i = startIndex; i < count; i++)
			{
				bool found = true;
				for (int n = 0; n < value.Length; n++)
				{
					if (this[i + n] != value[n])
					{
						found = false;
						break;
					}
				}
				if (found)
					return i;
			}

			return -1;
		}
	}
}