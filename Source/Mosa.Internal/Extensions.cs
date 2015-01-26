using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Kernel.Helpers
{
	public static class NumberExtensions
	{
		public static int ToInt(this bool self)
		{
			return self ? 1 : 0;
		}

		public static byte ToByte(this bool self)
		{
			return self ? (byte)1 : (byte)0;
		}

		public static char ToChar(this bool self)
		{
			return self ? '1' : '0';
		}

		public static int ToInt(this Enum self)
		{
			return (int)(object)self;
		}

		public static string ToStringNumber(this Enum self)
		{
			return ((int)(object)self).ToString();
		}

		public static string ToHex(this uint self)
		{
			return self.ToString("X");
		}

		public static string ToHex(this byte self)
		{
			return self.ToString("X");
		}
	}

	public static class BitHelper
	{
		#region Byte

		public static bool IsMaskSet(this byte self, byte mask)
		{
			return (self & mask) == mask;
		}

		public static bool IsBitSet(this byte self, byte bit)
		{
			return (self & (0x1 << bit)) == (0x1 << bit);
		}

		public static byte SetMask(this byte self, byte mask)
		{
			return (byte)(self | mask);
		}

		public static byte SetBit(this byte self, byte bit)
		{
			return (byte)(self | (0x1 << bit));
		}

		public static byte ClearMask(this byte self, byte mask)
		{
			return (byte)(self & ~mask);
		}

		public static byte ClearBit(this byte self, byte bit)
		{
			return (byte)(self & ~(0x1 << bit));
		}

		public static byte SetMask(this byte self, byte mask, bool state)
		{
			if (state)
				return (byte)(self | mask);
			else
				return (byte)(self & ~mask);
		}

		public static byte SetBit(this byte self, byte bit, bool state)
		{
			if (state)
				return (byte)(self | (0x1 << bit));
			else
				return (byte)(self & ~(0x1 << bit));
		}

		public static byte CircularLeftShift(this byte a, byte n)
		{
			return (byte)(a << n | a >> (8 - n));
		}

		public static byte CircularRightShift(this byte a, byte n)
		{
			return (byte)(a >> n | a << (8 - n));
		}

		public static byte GetBits(this byte self, byte index, byte count)
		{
			return (byte)((self >> index) << (8 - count));
		}

		public static byte SetBits(this byte self, byte source, byte index, byte count)
		{
			byte mask = (byte)(0xFF >> (8 - count));
			byte bits = (byte)((source & mask) << index);
			return (byte)((self & ~(mask << index)) | bits);
		}

		#endregion Byte

		#region *Byte

		unsafe public static bool IsMaskSet(byte* self, byte mask)
		{
			return (*self & mask) == mask;
		}

		unsafe public static bool IsBitSet(byte* self, byte bit)
		{
			return (*self & (0x1 << bit)) == (0x1 << bit);
		}

		unsafe public static void SetMask(byte* self, byte mask)
		{
			*self = (byte)(*self | mask);
		}

		unsafe public static void SetBit(byte* self, byte bit)
		{
			*self = (byte)(*self | (0x1 << bit));
		}

		unsafe public static void ClearMask(byte* self, byte mask)
		{
			*self = (byte)(*self & ~mask);
		}

		unsafe public static void ClearBit(byte* self, byte bit)
		{
			*self = (byte)(*self & ~(0x1 << bit));
		}

		unsafe public static void SetMask(byte* self, byte mask, bool state)
		{
			if (state)
				*self = (byte)(*self | mask);
			else
				*self = (byte)(*self & ~mask);
		}

		unsafe public static void SetBit(byte* self, byte bit, bool state)
		{
			if (state)
				*self = (byte)(*self | (0x1 << bit));
			else
				*self = (byte)(*self & ~(0x1 << bit));
		}

		unsafe public static void CircularLeftShift(byte* a, byte n)
		{
			*a = (byte)(*a << n | *a >> (8 - n));
		}

		unsafe public static void CircularRightShift(byte* a, byte n)
		{
			*a = (byte)(*a >> n | *a << (8 - n));
		}

		unsafe public static void GetBits(byte* self, byte index, byte count)
		{
			*self = (byte)((*self >> index) << (8 - count));
		}

		unsafe public static void SetBits(byte* self, byte source, byte index, byte count)
		{
			byte mask = (byte)(0xFF >> (8 - count));
			byte bits = (byte)((source & mask) << index);
			*self = (byte)((*self & ~(mask << index)) | bits);
		}

		#endregion *Byte
	}
}