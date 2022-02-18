// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics.CodeAnalysis;

namespace System
{
	internal class Base64Decoder
	{
		public const char PaddingChar = '=';

		public readonly byte[] Map;
		public readonly char[] CharacterSet;

		public Base64Decoder(char[] characterSet)
		{
			CharacterSet = characterSet;
			Map = Create(characterSet);
		}

		private byte[] Create(char[] characterSet)
		{
			byte[] x = new byte[123];

			for (byte i = 0; i < characterSet.Length; i++)
				x[(byte)characterSet[i]] = i;

			return x;
		}

		public byte[] FromBase(string data)
		{
			int length = data == null ? 0 : data.Length;

			if (length == 0)
				return new byte[0];

			unsafe
			{
				fixed (char* _p = data.ToCharArray())
				{
					char* p2 = _p;

					int blocks = (length - 1) / 4 + 1;
					int bytes = blocks * 3;

					int padding = blocks * 4 - length;

					if (length > 2 && p2[length - 2] == PaddingChar)
						padding = 2;
					else if (length > 1 && p2[length - 1] == PaddingChar)
						padding = 1;

					byte[] _data = new byte[bytes - padding];

					byte temp1, temp2;
					byte* dp;

					fixed (byte* _d = _data)
					{
						dp = _d;

						for (int i = 1; i < blocks; i++)
						{
							temp1 = Map[*p2++];
							temp2 = Map[*p2++];

							*dp++ = (byte)((temp1 << 2) | ((temp2 & 0x30) >> 4));
							temp1 = Map[*p2++];
							*dp++ = (byte)(((temp1 & 0x3C) >> 2) | ((temp2 & 0x0F) << 4));
							temp2 = Map[*p2++];
							*dp++ = (byte)(((temp1 & 0x03) << 6) | temp2);
						}

						temp1 = Map[*p2++];
						temp2 = Map[*p2++];

						*dp++ = (byte)((temp1 << 2) | ((temp2 & 0x30) >> 4));

						temp1 = Map[*p2++];

						if (padding != 2)
							*dp++ = (byte)(((temp1 & 0x3C) >> 2) | ((temp2 & 0x0F) << 4));


						temp2 = Map[*p2++];
						if (padding == 0)
							*dp++ = (byte)(((temp1 & 0x03) << 6) | temp2);


					}
					return _data;
				}
			}
		}
	}

	/// <summary>
	/// Implementation of the "Convert" class.
	/// </summary>
	public static class Convert
	{
		private static Base64Decoder Decoder;

		public static byte[] FromBase64String(string s)
		{
			if (Decoder == null)
				Decoder = new Base64Decoder("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".ToCharArray());

			return Decoder.FromBase(s);
		}

		public static bool ToBoolean(bool value)
		{
			return value;
		}

		public static bool ToBoolean(byte value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(double value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(float value)
		{
			return (value != 0f);
		}

		public static bool ToBoolean(int value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(long value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(sbyte value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(short value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(uint value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(ulong value)
		{
			return (value != 0);
		}

		public static bool ToBoolean(ushort value)
		{
			return (value != 0);
		}

		public static int ToInt32(bool value)
		{
			return value ? 1 : 0;
		}

		public static int ToInt32(byte value)
		{
			return value;
		}

		public static int ToInt32(char value)
		{
			return value;
		}

		// Conversions to Byte

		public static byte ToByte(object value)
		{
			return value == null ? (byte)0 : ((IConvertible)value).ToByte(null);
		}

		public static byte ToByte(object value, IFormatProvider provider)
		{
			return value == null ? (byte)0 : ((IConvertible)value).ToByte(provider);
		}

		public static byte ToByte(bool value)
		{
			return value ? (byte)bool.True : (byte)bool.False;
		}

		public static byte ToByte(byte value)
		{
			return value;
		}

		public static byte ToByte(char value)
		{
			if (value > byte.MaxValue) ThrowByteOverflowException();
			return (byte)value;
		}

		[CLSCompliant(false)]
		public static byte ToByte(sbyte value)
		{
			if (value < 0) ThrowByteOverflowException();
			return (byte)value;
		}

		public static byte ToByte(short value)
		{
			if ((uint)value > byte.MaxValue) ThrowByteOverflowException();
			return (byte)value;
		}

		[CLSCompliant(false)]
		public static byte ToByte(ushort value)
		{
			if (value > byte.MaxValue) ThrowByteOverflowException();
			return (byte)value;
		}

		public static byte ToByte(int value) => ToByte((uint)value);

		[CLSCompliant(false)]
		public static byte ToByte(uint value)
		{
			if (value > byte.MaxValue) ThrowByteOverflowException();
			return (byte)value;
		}

		public static byte ToByte(long value) => ToByte((ulong)value);

		[CLSCompliant(false)]
		public static byte ToByte(ulong value)
		{
			if (value > byte.MaxValue) ThrowByteOverflowException();
			return (byte)value;
		}

		[DoesNotReturn]
		private static void ThrowByteOverflowException() { throw new OverflowException("Byte overflow"); }
	}
}
