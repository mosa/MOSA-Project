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

		public static byte SetBits(this byte self, byte index, byte count, byte value)
		{
			byte mask = (byte)(0xFF >> (8 - count));
			byte bits = (byte)((value & mask) << index);
			return (byte)((self & ~(mask << index)) | bits);
		}

		#endregion Byte

		#region uint

		public static bool IsMaskSet(this uint self, uint mask)
		{
			return (self & mask) == mask;
		}

		public static bool IsBitSet(this uint self, byte bit)
		{
			return (self & (0x1 << bit)) == (0x1 << bit);
		}

		public static uint SetMask(this uint self, byte mask)
		{
			return self | mask;
		}

		public static uint SetBit(this uint self, byte bit)
		{
			return self | (0x1U << bit);
		}

		public static uint ClearMask(this uint self, uint mask)
		{
			return self & ~mask;
		}

		public static uint ClearBit(this uint self, byte bit)
		{
			return self & ~(0x1U << bit);
		}

		public static uint SetMask(this uint self, uint mask, bool state)
		{
			if (state)
				return self | mask;
			else
				return self & ~mask;
		}

		public static uint SetBit(this uint self, byte bit, bool state)
		{
			if (state)
				return self | (0x1U << bit);
			else
				return self & ~(0x1U << bit);
		}

		public static uint CircularLeftShift(this uint a, byte n)
		{
			return a << n | a >> (8 - n);
		}

		public static uint CircularRightShift(this uint a, byte n)
		{
			return a >> n | a << (8 - n);
		}

		public static uint GetBits(this uint self, byte index, byte count)
		{
			return (self >> index) << (8 - count);
		}

		public static uint GetBits(this uint self, byte index, byte count, byte sourceIndex)
		{
			return ((self >> index) << (8 - count)) << sourceIndex;
		}

		public static uint SetBits(this uint self, byte index, byte count, uint value)
		{
			uint mask = 0xFFFFFFFFU >> (8 - count);
			uint bits = (value & mask) << index;
			return (self & ~(mask << index)) | bits;
		}

		public static uint SetBits(this uint self, byte index, byte count, uint value, byte sourceIndex)
		{
			uint mask = 0xFFFFFFFFU >> (8 - count);
			uint bits = ((value >> sourceIndex) & mask) << index;
			return (self & ~(mask << index)) | bits;
		}

		#endregion uint

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

	/// <summary>
	/// Represents the HEX value of a bit position
	/// </summary>
	public static class BitMask
	{
		public const uint Bit0 = 0x1;
		public const uint Bit1 = 0x2;
		public const uint Bit2 = 0x4;
		public const uint Bit3 = 0x8;
		public const uint Bit4 = 0x10;
		public const uint Bit5 = 0x20;
		public const uint Bit6 = 0x40;
		public const uint Bit7 = 0x80;
		public const uint Bit8 = 0x100;
		public const uint Bit9 = 0x200;
		public const uint Bit10 = 0x400;
		public const uint Bit11 = 0x800;
		public const uint Bit12 = 0x1000;
		public const uint Bit13 = 0x2000;
		public const uint Bit14 = 0x4000;
		public const uint Bit15 = 0x8000;
		public const uint Bit16 = 0x10000;
		public const uint Bit17 = 0x20000;
		public const uint Bit18 = 0x40000;
		public const uint Bit19 = 0x80000;
		public const uint Bit20 = 0x100000;
		public const uint Bit21 = 0x200000;
		public const uint Bit22 = 0x400000;
		public const uint Bit23 = 0x800000;
		public const uint Bit24 = 0x1000000;
		public const uint Bit25 = 0x2000000;
		public const uint Bit26 = 0x4000000;
		public const uint Bit27 = 0x8000000;
		public const uint Bit28 = 0x10000000;
		public const uint Bit29 = 0x20000000;
		public const uint Bit30 = 0x40000000;
		public const uint Bit31 = 0x80000000;
	}
}