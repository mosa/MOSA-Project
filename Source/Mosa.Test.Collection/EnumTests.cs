/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Andrey Turkin <andrey.turkin@gmail.com>
 */
 
using System;

namespace Mosa.Test.Collection
{
	enum EnumU1 : byte {
		Positive49 = 49,
		Positive50,
		Positive51
	}

	public static class TestEnumU1Class {
		public static bool PositiveConversion() {
			EnumU1 e = EnumU1.Positive50;
			return 50 == (byte)e;
		}
		public static bool PositivePlusOne_1() {
			EnumU1 e = EnumU1.Positive50;
			return 51 == (byte)(e + 1);
		}
		public static bool PositivePlusOne_2() {
			EnumU1 e = EnumU1.Positive50;
			return EnumU1.Positive51 == e + 1;
		}
		public static bool PositiveMinusOne_1() {
			EnumU1 e = EnumU1.Positive50;
			return 49 == (byte)(e - 1);
		}
		public static bool PositiveMinusOne_2() {
			EnumU1 e = EnumU1.Positive50;
			return EnumU1.Positive49 == e - 1;
		}
		public static bool PositiveShl() {
			EnumU1 e = EnumU1.Positive50;
			return 100 == (byte)e << 1;
		}
		public static bool PositiveShr() {
			EnumU1 e = EnumU1.Positive50;
			return 25 == (byte)e >> 1;
		}
		public static bool PositiveMul2() {
			EnumU1 e = EnumU1.Positive50;
			return 100 == (byte)e * 2;
		}
		public static bool PositiveDiv2() {
			EnumU1 e = EnumU1.Positive50;
			return 25 == (byte)e / 2;
		}
		public static bool PositiveRem2() {
			EnumU1 e = EnumU1.Positive50;
			return 0 == (byte)e % 2;
		}
		public static bool PositiveAssignPlusOne() {
			EnumU1 e = EnumU1.Positive50;
			e += 1;
			return 51 == (byte)e;
		}
		public static bool PositiveAssignMinusOne() {
			EnumU1 e = EnumU1.Positive50;
			e -= 1;
			return 49 == (byte)e;
		}
		public static bool PositivePreincrement() {
			EnumU1 e = EnumU1.Positive50;
			bool retval = 51 == (byte)(++e);
			return retval && 51 == (byte)e;
		}
		public static bool PositivePredecrement() {
			EnumU1 e = EnumU1.Positive50;
			bool retval = 49 == (byte)(--e);
			return retval && 49 == (byte)e;
		}
		public static bool PositivePostincrement() {
			EnumU1 e = EnumU1.Positive50;
			bool retval = 50 == (byte)(e++);
			return retval && 51 == (byte)e;
		}
		public static bool PositivePostdecrement() {
			EnumU1 e = EnumU1.Positive50;
			bool retval = 50 == (byte)(e--);
			return retval && 49 == (byte)e;
		}
		public static bool PositiveAnd() {
			EnumU1 e = EnumU1.Positive50;
			return 50 == ((byte)e & 0xFF);
		}
		public static bool PositiveOr() {
			EnumU1 e = EnumU1.Positive50;
			return 51 == ((byte)e | 1);
		}
		public static bool PositiveXOr() {
			EnumU1 e = EnumU1.Positive50;
			return 51 == ((byte)e ^ 1);
		}
		private static bool _PositiveEqual(EnumU1 e, byte v) {
			return (byte)e == v;
		}
		public static bool PositiveEqual1() {
			return (50 == 50) == _PositiveEqual(EnumU1.Positive50, 50);
		}
		public static bool PositiveEqual2() {
			return (50 == 51) == _PositiveEqual(EnumU1.Positive50, 51);
		}
		public static bool PositiveEqual3() {
			return (50 == 49) == _PositiveEqual(EnumU1.Positive50, 49);
		}

		private static bool _PositiveNotEqual(EnumU1 e, byte v) {
			return (byte)e != v;
		}
		public static bool PositiveNotEqual1() {
			return (50 != 50) == _PositiveNotEqual(EnumU1.Positive50, 50);
		}
		public static bool PositiveNotEqual2() {
			return (50 != 51) == _PositiveNotEqual(EnumU1.Positive50, 51);
		}
		public static bool PositiveNotEqual3() {
			return (50 != 49) == _PositiveNotEqual(EnumU1.Positive50, 49);
		}

		private static bool _PositiveGreaterThan(EnumU1 e, byte v) {
			return (byte)e > v;
		}
		public static bool PositiveGreaterThan1() {
			return (50 > 50) == _PositiveGreaterThan(EnumU1.Positive50, 50);
		}
		public static bool PositiveGreaterThan2() {
			return (50 > 51) == _PositiveGreaterThan(EnumU1.Positive50, 51);
		}
		public static bool PositiveGreaterThan3() {
			return (50 > 49) == _PositiveGreaterThan(EnumU1.Positive50, 49);
		}

		private static bool _PositiveLessThan(EnumU1 e, byte v) {
			return (byte)e < v;
		}
		public static bool PositiveLessThan1() {
			return (50 < 50) == _PositiveLessThan(EnumU1.Positive50, 50);
		}
		public static bool PositiveLessThan2() {
			return (50 < 51) == _PositiveLessThan(EnumU1.Positive50, 51);
		}
		public static bool PositiveLessThan3() {
			return (50 < 49) == _PositiveLessThan(EnumU1.Positive50, 49);
		}

		private static bool _PositiveGreaterThanOrEqual(EnumU1 e, byte v) {
			return (byte)e >= v;
		}
		public static bool PositiveGreaterThanOrEqual1() {
			return (50 >= 50) == _PositiveGreaterThanOrEqual(EnumU1.Positive50, 50);
		}
		public static bool PositiveGreaterThanOrEqual2() {
			return (50 >= 51) == _PositiveGreaterThanOrEqual(EnumU1.Positive50, 51);
		}
		public static bool PositiveGreaterThanOrEqual3() {
			return (50 >= 49) == _PositiveGreaterThanOrEqual(EnumU1.Positive50, 49);
		}

		private static bool _PositiveLessThanOrEqual(EnumU1 e, byte v) {
			return (byte)e <= v;
		}
		public static bool PositiveLessThanOrEqual1() {
			return (50 <= 50) == _PositiveLessThanOrEqual(EnumU1.Positive50, 50);
		}
		public static bool PositiveLessThanOrEqual2() {
			return (50 <= 51) == _PositiveLessThanOrEqual(EnumU1.Positive50, 51);
		}
		public static bool PositiveLessThanOrEqual3() {
			return (50 <= 49) == _PositiveLessThanOrEqual(EnumU1.Positive50, 49);
		}

	}

	enum EnumU2 : ushort {
		Positive49 = 49,
		Positive50,
		Positive51
	}

	public static class TestEnumU2Class {
		public static bool PositiveConversion() {
			EnumU2 e = EnumU2.Positive50;
			return 50 == (ushort)e;
		}
		public static bool PositivePlusOne_1() {
			EnumU2 e = EnumU2.Positive50;
			return 51 == (ushort)(e + 1);
		}
		public static bool PositivePlusOne_2() {
			EnumU2 e = EnumU2.Positive50;
			return EnumU2.Positive51 == e + 1;
		}
		public static bool PositiveMinusOne_1() {
			EnumU2 e = EnumU2.Positive50;
			return 49 == (ushort)(e - 1);
		}
		public static bool PositiveMinusOne_2() {
			EnumU2 e = EnumU2.Positive50;
			return EnumU2.Positive49 == e - 1;
		}
		public static bool PositiveShl() {
			EnumU2 e = EnumU2.Positive50;
			return 100 == (ushort)e << 1;
		}
		public static bool PositiveShr() {
			EnumU2 e = EnumU2.Positive50;
			return 25 == (ushort)e >> 1;
		}
		public static bool PositiveMul2() {
			EnumU2 e = EnumU2.Positive50;
			return 100 == (ushort)e * 2;
		}
		public static bool PositiveDiv2() {
			EnumU2 e = EnumU2.Positive50;
			return 25 == (ushort)e / 2;
		}
		public static bool PositiveRem2() {
			EnumU2 e = EnumU2.Positive50;
			return 0 == (ushort)e % 2;
		}
		public static bool PositiveAssignPlusOne() {
			EnumU2 e = EnumU2.Positive50;
			e += 1;
			return 51 == (ushort)e;
		}
		public static bool PositiveAssignMinusOne() {
			EnumU2 e = EnumU2.Positive50;
			e -= 1;
			return 49 == (ushort)e;
		}
		public static bool PositivePreincrement() {
			EnumU2 e = EnumU2.Positive50;
			bool retval = 51 == (ushort)(++e);
			return retval && 51 == (ushort)e;
		}
		public static bool PositivePredecrement() {
			EnumU2 e = EnumU2.Positive50;
			bool retval = 49 == (ushort)(--e);
			return retval && 49 == (ushort)e;
		}
		public static bool PositivePostincrement() {
			EnumU2 e = EnumU2.Positive50;
			bool retval = 50 == (ushort)(e++);
			return retval && 51 == (ushort)e;
		}
		public static bool PositivePostdecrement() {
			EnumU2 e = EnumU2.Positive50;
			bool retval = 50 == (ushort)(e--);
			return retval && 49 == (ushort)e;
		}
		public static bool PositiveAnd() {
			EnumU2 e = EnumU2.Positive50;
			return 50 == ((ushort)e & 0xFF);
		}
		public static bool PositiveOr() {
			EnumU2 e = EnumU2.Positive50;
			return 51 == ((ushort)e | 1);
		}
		public static bool PositiveXOr() {
			EnumU2 e = EnumU2.Positive50;
			return 51 == ((ushort)e ^ 1);
		}
		private static bool _PositiveEqual(EnumU2 e, ushort v) {
			return (ushort)e == v;
		}
		public static bool PositiveEqual1() {
			return (50 == 50) == _PositiveEqual(EnumU2.Positive50, 50);
		}
		public static bool PositiveEqual2() {
			return (50 == 51) == _PositiveEqual(EnumU2.Positive50, 51);
		}
		public static bool PositiveEqual3() {
			return (50 == 49) == _PositiveEqual(EnumU2.Positive50, 49);
		}

		private static bool _PositiveNotEqual(EnumU2 e, ushort v) {
			return (ushort)e != v;
		}
		public static bool PositiveNotEqual1() {
			return (50 != 50) == _PositiveNotEqual(EnumU2.Positive50, 50);
		}
		public static bool PositiveNotEqual2() {
			return (50 != 51) == _PositiveNotEqual(EnumU2.Positive50, 51);
		}
		public static bool PositiveNotEqual3() {
			return (50 != 49) == _PositiveNotEqual(EnumU2.Positive50, 49);
		}

		private static bool _PositiveGreaterThan(EnumU2 e, ushort v) {
			return (ushort)e > v;
		}
		public static bool PositiveGreaterThan1() {
			return (50 > 50) == _PositiveGreaterThan(EnumU2.Positive50, 50);
		}
		public static bool PositiveGreaterThan2() {
			return (50 > 51) == _PositiveGreaterThan(EnumU2.Positive50, 51);
		}
		public static bool PositiveGreaterThan3() {
			return (50 > 49) == _PositiveGreaterThan(EnumU2.Positive50, 49);
		}

		private static bool _PositiveLessThan(EnumU2 e, ushort v) {
			return (ushort)e < v;
		}
		public static bool PositiveLessThan1() {
			return (50 < 50) == _PositiveLessThan(EnumU2.Positive50, 50);
		}
		public static bool PositiveLessThan2() {
			return (50 < 51) == _PositiveLessThan(EnumU2.Positive50, 51);
		}
		public static bool PositiveLessThan3() {
			return (50 < 49) == _PositiveLessThan(EnumU2.Positive50, 49);
		}

		private static bool _PositiveGreaterThanOrEqual(EnumU2 e, ushort v) {
			return (ushort)e >= v;
		}
		public static bool PositiveGreaterThanOrEqual1() {
			return (50 >= 50) == _PositiveGreaterThanOrEqual(EnumU2.Positive50, 50);
		}
		public static bool PositiveGreaterThanOrEqual2() {
			return (50 >= 51) == _PositiveGreaterThanOrEqual(EnumU2.Positive50, 51);
		}
		public static bool PositiveGreaterThanOrEqual3() {
			return (50 >= 49) == _PositiveGreaterThanOrEqual(EnumU2.Positive50, 49);
		}

		private static bool _PositiveLessThanOrEqual(EnumU2 e, ushort v) {
			return (ushort)e <= v;
		}
		public static bool PositiveLessThanOrEqual1() {
			return (50 <= 50) == _PositiveLessThanOrEqual(EnumU2.Positive50, 50);
		}
		public static bool PositiveLessThanOrEqual2() {
			return (50 <= 51) == _PositiveLessThanOrEqual(EnumU2.Positive50, 51);
		}
		public static bool PositiveLessThanOrEqual3() {
			return (50 <= 49) == _PositiveLessThanOrEqual(EnumU2.Positive50, 49);
		}

	}

	enum EnumU4 : uint {
		Positive49 = 49,
		Positive50,
		Positive51
	}

	public static class TestEnumU4Class {
		public static bool PositiveConversion() {
			EnumU4 e = EnumU4.Positive50;
			return 50 == (uint)e;
		}
		public static bool PositivePlusOne_1() {
			EnumU4 e = EnumU4.Positive50;
			return 51 == (uint)(e + 1);
		}
		public static bool PositivePlusOne_2() {
			EnumU4 e = EnumU4.Positive50;
			return EnumU4.Positive51 == e + 1;
		}
		public static bool PositiveMinusOne_1() {
			EnumU4 e = EnumU4.Positive50;
			return 49 == (uint)(e - 1);
		}
		public static bool PositiveMinusOne_2() {
			EnumU4 e = EnumU4.Positive50;
			return EnumU4.Positive49 == e - 1;
		}
		public static bool PositiveShl() {
			EnumU4 e = EnumU4.Positive50;
			return 100 == (uint)e << 1;
		}
		public static bool PositiveShr() {
			EnumU4 e = EnumU4.Positive50;
			return 25 == (uint)e >> 1;
		}
		public static bool PositiveMul2() {
			EnumU4 e = EnumU4.Positive50;
			return 100 == (uint)e * 2;
		}
		public static bool PositiveDiv2() {
			EnumU4 e = EnumU4.Positive50;
			return 25 == (uint)e / 2;
		}
		public static bool PositiveRem2() {
			EnumU4 e = EnumU4.Positive50;
			return 0 == (uint)e % 2;
		}
		public static bool PositiveAssignPlusOne() {
			EnumU4 e = EnumU4.Positive50;
			e += 1;
			return 51 == (uint)e;
		}
		public static bool PositiveAssignMinusOne() {
			EnumU4 e = EnumU4.Positive50;
			e -= 1;
			return 49 == (uint)e;
		}
		public static bool PositivePreincrement() {
			EnumU4 e = EnumU4.Positive50;
			bool retval = 51 == (uint)(++e);
			return retval && 51 == (uint)e;
		}
		public static bool PositivePredecrement() {
			EnumU4 e = EnumU4.Positive50;
			bool retval = 49 == (uint)(--e);
			return retval && 49 == (uint)e;
		}
		public static bool PositivePostincrement() {
			EnumU4 e = EnumU4.Positive50;
			bool retval = 50 == (uint)(e++);
			return retval && 51 == (uint)e;
		}
		public static bool PositivePostdecrement() {
			EnumU4 e = EnumU4.Positive50;
			bool retval = 50 == (uint)(e--);
			return retval && 49 == (uint)e;
		}
		public static bool PositiveAnd() {
			EnumU4 e = EnumU4.Positive50;
			return 50 == ((uint)e & 0xFF);
		}
		public static bool PositiveOr() {
			EnumU4 e = EnumU4.Positive50;
			return 51 == ((uint)e | 1);
		}
		public static bool PositiveXOr() {
			EnumU4 e = EnumU4.Positive50;
			return 51 == ((uint)e ^ 1);
		}
		private static bool _PositiveEqual(EnumU4 e, uint v) {
			return (uint)e == v;
		}
		public static bool PositiveEqual1() {
			return (50 == 50) == _PositiveEqual(EnumU4.Positive50, 50);
		}
		public static bool PositiveEqual2() {
			return (50 == 51) == _PositiveEqual(EnumU4.Positive50, 51);
		}
		public static bool PositiveEqual3() {
			return (50 == 49) == _PositiveEqual(EnumU4.Positive50, 49);
		}

		private static bool _PositiveNotEqual(EnumU4 e, uint v) {
			return (uint)e != v;
		}
		public static bool PositiveNotEqual1() {
			return (50 != 50) == _PositiveNotEqual(EnumU4.Positive50, 50);
		}
		public static bool PositiveNotEqual2() {
			return (50 != 51) == _PositiveNotEqual(EnumU4.Positive50, 51);
		}
		public static bool PositiveNotEqual3() {
			return (50 != 49) == _PositiveNotEqual(EnumU4.Positive50, 49);
		}

		private static bool _PositiveGreaterThan(EnumU4 e, uint v) {
			return (uint)e > v;
		}
		public static bool PositiveGreaterThan1() {
			return (50 > 50) == _PositiveGreaterThan(EnumU4.Positive50, 50);
		}
		public static bool PositiveGreaterThan2() {
			return (50 > 51) == _PositiveGreaterThan(EnumU4.Positive50, 51);
		}
		public static bool PositiveGreaterThan3() {
			return (50 > 49) == _PositiveGreaterThan(EnumU4.Positive50, 49);
		}

		private static bool _PositiveLessThan(EnumU4 e, uint v) {
			return (uint)e < v;
		}
		public static bool PositiveLessThan1() {
			return (50 < 50) == _PositiveLessThan(EnumU4.Positive50, 50);
		}
		public static bool PositiveLessThan2() {
			return (50 < 51) == _PositiveLessThan(EnumU4.Positive50, 51);
		}
		public static bool PositiveLessThan3() {
			return (50 < 49) == _PositiveLessThan(EnumU4.Positive50, 49);
		}

		private static bool _PositiveGreaterThanOrEqual(EnumU4 e, uint v) {
			return (uint)e >= v;
		}
		public static bool PositiveGreaterThanOrEqual1() {
			return (50 >= 50) == _PositiveGreaterThanOrEqual(EnumU4.Positive50, 50);
		}
		public static bool PositiveGreaterThanOrEqual2() {
			return (50 >= 51) == _PositiveGreaterThanOrEqual(EnumU4.Positive50, 51);
		}
		public static bool PositiveGreaterThanOrEqual3() {
			return (50 >= 49) == _PositiveGreaterThanOrEqual(EnumU4.Positive50, 49);
		}

		private static bool _PositiveLessThanOrEqual(EnumU4 e, uint v) {
			return (uint)e <= v;
		}
		public static bool PositiveLessThanOrEqual1() {
			return (50 <= 50) == _PositiveLessThanOrEqual(EnumU4.Positive50, 50);
		}
		public static bool PositiveLessThanOrEqual2() {
			return (50 <= 51) == _PositiveLessThanOrEqual(EnumU4.Positive50, 51);
		}
		public static bool PositiveLessThanOrEqual3() {
			return (50 <= 49) == _PositiveLessThanOrEqual(EnumU4.Positive50, 49);
		}

	}

	enum EnumU8 : ulong {
		Positive49 = 49,
		Positive50,
		Positive51
	}

	public static class TestEnumU8Class {
		public static bool PositiveConversion() {
			EnumU8 e = EnumU8.Positive50;
			return 50 == (ulong)e;
		}
		public static bool PositivePlusOne_1() {
			EnumU8 e = EnumU8.Positive50;
			return 51 == (ulong)(e + 1);
		}
		public static bool PositivePlusOne_2() {
			EnumU8 e = EnumU8.Positive50;
			return EnumU8.Positive51 == e + 1;
		}
		public static bool PositiveMinusOne_1() {
			EnumU8 e = EnumU8.Positive50;
			return 49 == (ulong)(e - 1);
		}
		public static bool PositiveMinusOne_2() {
			EnumU8 e = EnumU8.Positive50;
			return EnumU8.Positive49 == e - 1;
		}
		public static bool PositiveShl() {
			EnumU8 e = EnumU8.Positive50;
			return 100 == (ulong)e << 1;
		}
		public static bool PositiveShr() {
			EnumU8 e = EnumU8.Positive50;
			return 25 == (ulong)e >> 1;
		}
		public static bool PositiveMul2() {
			EnumU8 e = EnumU8.Positive50;
			return 100 == (ulong)e * 2;
		}
		public static bool PositiveDiv2() {
			EnumU8 e = EnumU8.Positive50;
			return 25 == (ulong)e / 2;
		}
		public static bool PositiveRem2() {
			EnumU8 e = EnumU8.Positive50;
			return 0 == (ulong)e % 2;
		}
		public static bool PositiveAssignPlusOne() {
			EnumU8 e = EnumU8.Positive50;
			e += 1;
			return 51 == (ulong)e;
		}
		public static bool PositiveAssignMinusOne() {
			EnumU8 e = EnumU8.Positive50;
			e -= 1;
			return 49 == (ulong)e;
		}
		public static bool PositivePreincrement() {
			EnumU8 e = EnumU8.Positive50;
			bool retval = 51 == (ulong)(++e);
			return retval && 51 == (ulong)e;
		}
		public static bool PositivePredecrement() {
			EnumU8 e = EnumU8.Positive50;
			bool retval = 49 == (ulong)(--e);
			return retval && 49 == (ulong)e;
		}
		public static bool PositivePostincrement() {
			EnumU8 e = EnumU8.Positive50;
			bool retval = 50 == (ulong)(e++);
			return retval && 51 == (ulong)e;
		}
		public static bool PositivePostdecrement() {
			EnumU8 e = EnumU8.Positive50;
			bool retval = 50 == (ulong)(e--);
			return retval && 49 == (ulong)e;
		}
		public static bool PositiveAnd() {
			EnumU8 e = EnumU8.Positive50;
			return 50 == ((ulong)e & 0xFF);
		}
		public static bool PositiveOr() {
			EnumU8 e = EnumU8.Positive50;
			return 51 == ((ulong)e | 1);
		}
		public static bool PositiveXOr() {
			EnumU8 e = EnumU8.Positive50;
			return 51 == ((ulong)e ^ 1);
		}
		private static bool _PositiveEqual(EnumU8 e, ulong v) {
			return (ulong)e == v;
		}
		public static bool PositiveEqual1() {
			return (50 == 50) == _PositiveEqual(EnumU8.Positive50, 50);
		}
		public static bool PositiveEqual2() {
			return (50 == 51) == _PositiveEqual(EnumU8.Positive50, 51);
		}
		public static bool PositiveEqual3() {
			return (50 == 49) == _PositiveEqual(EnumU8.Positive50, 49);
		}

		private static bool _PositiveNotEqual(EnumU8 e, ulong v) {
			return (ulong)e != v;
		}
		public static bool PositiveNotEqual1() {
			return (50 != 50) == _PositiveNotEqual(EnumU8.Positive50, 50);
		}
		public static bool PositiveNotEqual2() {
			return (50 != 51) == _PositiveNotEqual(EnumU8.Positive50, 51);
		}
		public static bool PositiveNotEqual3() {
			return (50 != 49) == _PositiveNotEqual(EnumU8.Positive50, 49);
		}

		private static bool _PositiveGreaterThan(EnumU8 e, ulong v) {
			return (ulong)e > v;
		}
		public static bool PositiveGreaterThan1() {
			return (50 > 50) == _PositiveGreaterThan(EnumU8.Positive50, 50);
		}
		public static bool PositiveGreaterThan2() {
			return (50 > 51) == _PositiveGreaterThan(EnumU8.Positive50, 51);
		}
		public static bool PositiveGreaterThan3() {
			return (50 > 49) == _PositiveGreaterThan(EnumU8.Positive50, 49);
		}

		private static bool _PositiveLessThan(EnumU8 e, ulong v) {
			return (ulong)e < v;
		}
		public static bool PositiveLessThan1() {
			return (50 < 50) == _PositiveLessThan(EnumU8.Positive50, 50);
		}
		public static bool PositiveLessThan2() {
			return (50 < 51) == _PositiveLessThan(EnumU8.Positive50, 51);
		}
		public static bool PositiveLessThan3() {
			return (50 < 49) == _PositiveLessThan(EnumU8.Positive50, 49);
		}

		private static bool _PositiveGreaterThanOrEqual(EnumU8 e, ulong v) {
			return (ulong)e >= v;
		}
		public static bool PositiveGreaterThanOrEqual1() {
			return (50 >= 50) == _PositiveGreaterThanOrEqual(EnumU8.Positive50, 50);
		}
		public static bool PositiveGreaterThanOrEqual2() {
			return (50 >= 51) == _PositiveGreaterThanOrEqual(EnumU8.Positive50, 51);
		}
		public static bool PositiveGreaterThanOrEqual3() {
			return (50 >= 49) == _PositiveGreaterThanOrEqual(EnumU8.Positive50, 49);
		}

		private static bool _PositiveLessThanOrEqual(EnumU8 e, ulong v) {
			return (ulong)e <= v;
		}
		public static bool PositiveLessThanOrEqual1() {
			return (50 <= 50) == _PositiveLessThanOrEqual(EnumU8.Positive50, 50);
		}
		public static bool PositiveLessThanOrEqual2() {
			return (50 <= 51) == _PositiveLessThanOrEqual(EnumU8.Positive50, 51);
		}
		public static bool PositiveLessThanOrEqual3() {
			return (50 <= 49) == _PositiveLessThanOrEqual(EnumU8.Positive50, 49);
		}

	}

	enum EnumI1 : sbyte {
		Negative51 = -51,
		Negative50,
		Negative49,
		Positive49 = 49,
		Positive50,
		Positive51
	}

	public static class TestEnumI1Class {
		public static bool PositiveConversion() {
			EnumI1 e = EnumI1.Positive50;
			return 50 == (sbyte)e;
		}
		public static bool PositivePlusOne_1() {
			EnumI1 e = EnumI1.Positive50;
			return 51 == (sbyte)(e + 1);
		}
		public static bool PositivePlusOne_2() {
			EnumI1 e = EnumI1.Positive50;
			return EnumI1.Positive51 == e + 1;
		}
		public static bool PositiveMinusOne_1() {
			EnumI1 e = EnumI1.Positive50;
			return 49 == (sbyte)(e - 1);
		}
		public static bool PositiveMinusOne_2() {
			EnumI1 e = EnumI1.Positive50;
			return EnumI1.Positive49 == e - 1;
		}
		public static bool PositiveShl() {
			EnumI1 e = EnumI1.Positive50;
			return 100 == (sbyte)e << 1;
		}
		public static bool PositiveShr() {
			EnumI1 e = EnumI1.Positive50;
			return 25 == (sbyte)e >> 1;
		}
		public static bool PositiveMul2() {
			EnumI1 e = EnumI1.Positive50;
			return 100 == (sbyte)e * 2;
		}
		public static bool PositiveDiv2() {
			EnumI1 e = EnumI1.Positive50;
			return 25 == (sbyte)e / 2;
		}
		public static bool PositiveRem2() {
			EnumI1 e = EnumI1.Positive50;
			return 0 == (sbyte)e % 2;
		}
		public static bool PositiveAssignPlusOne() {
			EnumI1 e = EnumI1.Positive50;
			e += 1;
			return 51 == (sbyte)e;
		}
		public static bool PositiveAssignMinusOne() {
			EnumI1 e = EnumI1.Positive50;
			e -= 1;
			return 49 == (sbyte)e;
		}
		public static bool PositivePreincrement() {
			EnumI1 e = EnumI1.Positive50;
			bool retval = 51 == (sbyte)(++e);
			return retval && 51 == (sbyte)e;
		}
		public static bool PositivePredecrement() {
			EnumI1 e = EnumI1.Positive50;
			bool retval = 49 == (sbyte)(--e);
			return retval && 49 == (sbyte)e;
		}
		public static bool PositivePostincrement() {
			EnumI1 e = EnumI1.Positive50;
			bool retval = 50 == (sbyte)(e++);
			return retval && 51 == (sbyte)e;
		}
		public static bool PositivePostdecrement() {
			EnumI1 e = EnumI1.Positive50;
			bool retval = 50 == (sbyte)(e--);
			return retval && 49 == (sbyte)e;
		}
		public static bool PositiveAnd() {
			EnumI1 e = EnumI1.Positive50;
			return 50 == ((sbyte)e & 0xFF);
		}
		public static bool PositiveOr() {
			EnumI1 e = EnumI1.Positive50;
			return 51 == ((sbyte)e | 1);
		}
		public static bool PositiveXOr() {
			EnumI1 e = EnumI1.Positive50;
			return 51 == ((sbyte)e ^ 1);
		}
		private static bool _PositiveEqual(EnumI1 e, sbyte v) {
			return (sbyte)e == v;
		}
		public static bool PositiveEqual1() {
			return (50 == 50) == _PositiveEqual(EnumI1.Positive50, 50);
		}
		public static bool PositiveEqual2() {
			return (50 == 51) == _PositiveEqual(EnumI1.Positive50, 51);
		}
		public static bool PositiveEqual3() {
			return (50 == 49) == _PositiveEqual(EnumI1.Positive50, 49);
		}

		private static bool _PositiveNotEqual(EnumI1 e, sbyte v) {
			return (sbyte)e != v;
		}
		public static bool PositiveNotEqual1() {
			return (50 != 50) == _PositiveNotEqual(EnumI1.Positive50, 50);
		}
		public static bool PositiveNotEqual2() {
			return (50 != 51) == _PositiveNotEqual(EnumI1.Positive50, 51);
		}
		public static bool PositiveNotEqual3() {
			return (50 != 49) == _PositiveNotEqual(EnumI1.Positive50, 49);
		}

		private static bool _PositiveGreaterThan(EnumI1 e, sbyte v) {
			return (sbyte)e > v;
		}
		public static bool PositiveGreaterThan1() {
			return (50 > 50) == _PositiveGreaterThan(EnumI1.Positive50, 50);
		}
		public static bool PositiveGreaterThan2() {
			return (50 > 51) == _PositiveGreaterThan(EnumI1.Positive50, 51);
		}
		public static bool PositiveGreaterThan3() {
			return (50 > 49) == _PositiveGreaterThan(EnumI1.Positive50, 49);
		}

		private static bool _PositiveLessThan(EnumI1 e, sbyte v) {
			return (sbyte)e < v;
		}
		public static bool PositiveLessThan1() {
			return (50 < 50) == _PositiveLessThan(EnumI1.Positive50, 50);
		}
		public static bool PositiveLessThan2() {
			return (50 < 51) == _PositiveLessThan(EnumI1.Positive50, 51);
		}
		public static bool PositiveLessThan3() {
			return (50 < 49) == _PositiveLessThan(EnumI1.Positive50, 49);
		}

		private static bool _PositiveGreaterThanOrEqual(EnumI1 e, sbyte v) {
			return (sbyte)e >= v;
		}
		public static bool PositiveGreaterThanOrEqual1() {
			return (50 >= 50) == _PositiveGreaterThanOrEqual(EnumI1.Positive50, 50);
		}
		public static bool PositiveGreaterThanOrEqual2() {
			return (50 >= 51) == _PositiveGreaterThanOrEqual(EnumI1.Positive50, 51);
		}
		public static bool PositiveGreaterThanOrEqual3() {
			return (50 >= 49) == _PositiveGreaterThanOrEqual(EnumI1.Positive50, 49);
		}

		private static bool _PositiveLessThanOrEqual(EnumI1 e, sbyte v) {
			return (sbyte)e <= v;
		}
		public static bool PositiveLessThanOrEqual1() {
			return (50 <= 50) == _PositiveLessThanOrEqual(EnumI1.Positive50, 50);
		}
		public static bool PositiveLessThanOrEqual2() {
			return (50 <= 51) == _PositiveLessThanOrEqual(EnumI1.Positive50, 51);
		}
		public static bool PositiveLessThanOrEqual3() {
			return (50 <= 49) == _PositiveLessThanOrEqual(EnumI1.Positive50, 49);
		}

		public static bool NegativeConversion() {
			EnumI1 e = EnumI1.Negative50;
			return -50 == (sbyte)e;
		}
		public static bool NegativePlusOne_1() {
			EnumI1 e = EnumI1.Negative50;
			return -49 == (sbyte)(e + 1);
		}
		public static bool NegativePlusOne_2() {
			EnumI1 e = EnumI1.Negative50;
			return EnumI1.Negative49 == e + 1;
		}
		public static bool NegativeMinusOne_1() {
			EnumI1 e = EnumI1.Negative50;
			return -51 == (sbyte)(e - 1);
		}
		public static bool NegativeMinusOne_2() {
			EnumI1 e = EnumI1.Negative50;
			return EnumI1.Negative51 == e - 1;
		}
		public static bool NegativeShl() {
			EnumI1 e = EnumI1.Negative50;
			return -100 == (sbyte)e << 1;
		}
		public static bool NegativeShr() {
			EnumI1 e = EnumI1.Negative50;
			return -25 == (sbyte)e >> 1;
		}
		public static bool NegativeMul2() {
			EnumI1 e = EnumI1.Negative50;
			return -100 == (sbyte)e * 2;
		}
		public static bool NegativeDiv2() {
			EnumI1 e = EnumI1.Negative50;
			return -25 == (sbyte)e / 2;
		}
		public static bool NegativeRem2() {
			EnumI1 e = EnumI1.Negative50;
			return 0 == (sbyte)e % 2;
		}
		public static bool NegativeAssignPlusOne() {
			EnumI1 e = EnumI1.Negative50;
			e += 1;
			return -49 == (sbyte)e;
		}
		public static bool NegativeAssignMinusOne() {
			EnumI1 e = EnumI1.Negative50;
			e -= 1;
			return -51 == (sbyte)e;
		}
		public static bool NegativePreincrement() {
			EnumI1 e = EnumI1.Negative50;
			bool retval = -49 == (sbyte)(++e);
			return retval && -49 == (sbyte)e;
		}
		public static bool NegativePredecrement() {
			EnumI1 e = EnumI1.Negative50;
			bool retval = -51 == (sbyte)(--e);
			return retval && -51 == (sbyte)e;
		}
		public static bool NegativePostincrement() {
			EnumI1 e = EnumI1.Negative50;
			bool retval = -50 == (sbyte)(e++);
			return retval && -49 == (sbyte)e;
		}
		public static bool NegativePostdecrement() {
			EnumI1 e = EnumI1.Negative50;
			bool retval = -50 == (sbyte)(e--);
			return retval && -51 == (sbyte)e;
		}
		public static bool NegativeAnd() {
			EnumI1 e = EnumI1.Negative50;
			return 206 == ((sbyte)e & 0xFF);
		}
		public static bool NegativeOr() {
			EnumI1 e = EnumI1.Negative50;
			return -49 == ((sbyte)e | 1);
		}
		public static bool NegativeXOr() {
			EnumI1 e = EnumI1.Negative50;
			return -49 == ((sbyte)e ^ 1);
		}
		private static bool _NegativeEqual(EnumI1 e, sbyte v) {
			return (sbyte)e == v;
		}
		public static bool NegativeEqual1() {
			return (-50 == -50) == _NegativeEqual(EnumI1.Negative50, -50);
		}
		public static bool NegativeEqual2() {
			return (-50 == -49) == _NegativeEqual(EnumI1.Negative50, -49);
		}
		public static bool NegativeEqual3() {
			return (-50 == -51) == _NegativeEqual(EnumI1.Negative50, -51);
		}

		private static bool _NegativeNotEqual(EnumI1 e, sbyte v) {
			return (sbyte)e != v;
		}
		public static bool NegativeNotEqual1() {
			return (-50 != -50) == _NegativeNotEqual(EnumI1.Negative50, -50);
		}
		public static bool NegativeNotEqual2() {
			return (-50 != -49) == _NegativeNotEqual(EnumI1.Negative50, -49);
		}
		public static bool NegativeNotEqual3() {
			return (-50 != -51) == _NegativeNotEqual(EnumI1.Negative50, -51);
		}

		private static bool _NegativeGreaterThan(EnumI1 e, sbyte v) {
			return (sbyte)e > v;
		}
		public static bool NegativeGreaterThan1() {
			return (-50 > -50) == _NegativeGreaterThan(EnumI1.Negative50, -50);
		}
		public static bool NegativeGreaterThan2() {
			return (-50 > -49) == _NegativeGreaterThan(EnumI1.Negative50, -49);
		}
		public static bool NegativeGreaterThan3() {
			return (-50 > -51) == _NegativeGreaterThan(EnumI1.Negative50, -51);
		}

		private static bool _NegativeLessThan(EnumI1 e, sbyte v) {
			return (sbyte)e < v;
		}
		public static bool NegativeLessThan1() {
			return (-50 < -50) == _NegativeLessThan(EnumI1.Negative50, -50);
		}
		public static bool NegativeLessThan2() {
			return (-50 < -49) == _NegativeLessThan(EnumI1.Negative50, -49);
		}
		public static bool NegativeLessThan3() {
			return (-50 < -51) == _NegativeLessThan(EnumI1.Negative50, -51);
		}

		private static bool _NegativeGreaterThanOrEqual(EnumI1 e, sbyte v) {
			return (sbyte)e >= v;
		}
		public static bool NegativeGreaterThanOrEqual1() {
			return (-50 >= -50) == _NegativeGreaterThanOrEqual(EnumI1.Negative50, -50);
		}
		public static bool NegativeGreaterThanOrEqual2() {
			return (-50 >= -49) == _NegativeGreaterThanOrEqual(EnumI1.Negative50, -49);
		}
		public static bool NegativeGreaterThanOrEqual3() {
			return (-50 >= -51) == _NegativeGreaterThanOrEqual(EnumI1.Negative50, -51);
		}

		private static bool _NegativeLessThanOrEqual(EnumI1 e, sbyte v) {
			return (sbyte)e <= v;
		}
		public static bool NegativeLessThanOrEqual1() {
			return (-50 <= -50) == _NegativeLessThanOrEqual(EnumI1.Negative50, -50);
		}
		public static bool NegativeLessThanOrEqual2() {
			return (-50 <= -49) == _NegativeLessThanOrEqual(EnumI1.Negative50, -49);
		}
		public static bool NegativeLessThanOrEqual3() {
			return (-50 <= -51) == _NegativeLessThanOrEqual(EnumI1.Negative50, -51);
		}

	}

	enum EnumI2 : short {
		Negative51 = -51,
		Negative50,
		Negative49,
		Positive49 = 49,
		Positive50,
		Positive51
	}

	public static class TestEnumI2Class {
		public static bool PositiveConversion() {
			EnumI2 e = EnumI2.Positive50;
			return 50 == (short)e;
		}
		public static bool PositivePlusOne_1() {
			EnumI2 e = EnumI2.Positive50;
			return 51 == (short)(e + 1);
		}
		public static bool PositivePlusOne_2() {
			EnumI2 e = EnumI2.Positive50;
			return EnumI2.Positive51 == e + 1;
		}
		public static bool PositiveMinusOne_1() {
			EnumI2 e = EnumI2.Positive50;
			return 49 == (short)(e - 1);
		}
		public static bool PositiveMinusOne_2() {
			EnumI2 e = EnumI2.Positive50;
			return EnumI2.Positive49 == e - 1;
		}
		public static bool PositiveShl() {
			EnumI2 e = EnumI2.Positive50;
			return 100 == (short)e << 1;
		}
		public static bool PositiveShr() {
			EnumI2 e = EnumI2.Positive50;
			return 25 == (short)e >> 1;
		}
		public static bool PositiveMul2() {
			EnumI2 e = EnumI2.Positive50;
			return 100 == (short)e * 2;
		}
		public static bool PositiveDiv2() {
			EnumI2 e = EnumI2.Positive50;
			return 25 == (short)e / 2;
		}
		public static bool PositiveRem2() {
			EnumI2 e = EnumI2.Positive50;
			return 0 == (short)e % 2;
		}
		public static bool PositiveAssignPlusOne() {
			EnumI2 e = EnumI2.Positive50;
			e += 1;
			return 51 == (short)e;
		}
		public static bool PositiveAssignMinusOne() {
			EnumI2 e = EnumI2.Positive50;
			e -= 1;
			return 49 == (short)e;
		}
		public static bool PositivePreincrement() {
			EnumI2 e = EnumI2.Positive50;
			bool retval = 51 == (short)(++e);
			return retval && 51 == (short)e;
		}
		public static bool PositivePredecrement() {
			EnumI2 e = EnumI2.Positive50;
			bool retval = 49 == (short)(--e);
			return retval && 49 == (short)e;
		}
		public static bool PositivePostincrement() {
			EnumI2 e = EnumI2.Positive50;
			bool retval = 50 == (short)(e++);
			return retval && 51 == (short)e;
		}
		public static bool PositivePostdecrement() {
			EnumI2 e = EnumI2.Positive50;
			bool retval = 50 == (short)(e--);
			return retval && 49 == (short)e;
		}
		public static bool PositiveAnd() {
			EnumI2 e = EnumI2.Positive50;
			return 50 == ((short)e & 0xFF);
		}
		public static bool PositiveOr() {
			EnumI2 e = EnumI2.Positive50;
			return 51 == ((short)e | 1);
		}
		public static bool PositiveXOr() {
			EnumI2 e = EnumI2.Positive50;
			return 51 == ((short)e ^ 1);
		}
		private static bool _PositiveEqual(EnumI2 e, short v) {
			return (short)e == v;
		}
		public static bool PositiveEqual1() {
			return (50 == 50) == _PositiveEqual(EnumI2.Positive50, 50);
		}
		public static bool PositiveEqual2() {
			return (50 == 51) == _PositiveEqual(EnumI2.Positive50, 51);
		}
		public static bool PositiveEqual3() {
			return (50 == 49) == _PositiveEqual(EnumI2.Positive50, 49);
		}

		private static bool _PositiveNotEqual(EnumI2 e, short v) {
			return (short)e != v;
		}
		public static bool PositiveNotEqual1() {
			return (50 != 50) == _PositiveNotEqual(EnumI2.Positive50, 50);
		}
		public static bool PositiveNotEqual2() {
			return (50 != 51) == _PositiveNotEqual(EnumI2.Positive50, 51);
		}
		public static bool PositiveNotEqual3() {
			return (50 != 49) == _PositiveNotEqual(EnumI2.Positive50, 49);
		}

		private static bool _PositiveGreaterThan(EnumI2 e, short v) {
			return (short)e > v;
		}
		public static bool PositiveGreaterThan1() {
			return (50 > 50) == _PositiveGreaterThan(EnumI2.Positive50, 50);
		}
		public static bool PositiveGreaterThan2() {
			return (50 > 51) == _PositiveGreaterThan(EnumI2.Positive50, 51);
		}
		public static bool PositiveGreaterThan3() {
			return (50 > 49) == _PositiveGreaterThan(EnumI2.Positive50, 49);
		}

		private static bool _PositiveLessThan(EnumI2 e, short v) {
			return (short)e < v;
		}
		public static bool PositiveLessThan1() {
			return (50 < 50) == _PositiveLessThan(EnumI2.Positive50, 50);
		}
		public static bool PositiveLessThan2() {
			return (50 < 51) == _PositiveLessThan(EnumI2.Positive50, 51);
		}
		public static bool PositiveLessThan3() {
			return (50 < 49) == _PositiveLessThan(EnumI2.Positive50, 49);
		}

		private static bool _PositiveGreaterThanOrEqual(EnumI2 e, short v) {
			return (short)e >= v;
		}
		public static bool PositiveGreaterThanOrEqual1() {
			return (50 >= 50) == _PositiveGreaterThanOrEqual(EnumI2.Positive50, 50);
		}
		public static bool PositiveGreaterThanOrEqual2() {
			return (50 >= 51) == _PositiveGreaterThanOrEqual(EnumI2.Positive50, 51);
		}
		public static bool PositiveGreaterThanOrEqual3() {
			return (50 >= 49) == _PositiveGreaterThanOrEqual(EnumI2.Positive50, 49);
		}

		private static bool _PositiveLessThanOrEqual(EnumI2 e, short v) {
			return (short)e <= v;
		}
		public static bool PositiveLessThanOrEqual1() {
			return (50 <= 50) == _PositiveLessThanOrEqual(EnumI2.Positive50, 50);
		}
		public static bool PositiveLessThanOrEqual2() {
			return (50 <= 51) == _PositiveLessThanOrEqual(EnumI2.Positive50, 51);
		}
		public static bool PositiveLessThanOrEqual3() {
			return (50 <= 49) == _PositiveLessThanOrEqual(EnumI2.Positive50, 49);
		}

		public static bool NegativeConversion() {
			EnumI2 e = EnumI2.Negative50;
			return -50 == (short)e;
		}
		public static bool NegativePlusOne_1() {
			EnumI2 e = EnumI2.Negative50;
			return -49 == (short)(e + 1);
		}
		public static bool NegativePlusOne_2() {
			EnumI2 e = EnumI2.Negative50;
			return EnumI2.Negative49 == e + 1;
		}
		public static bool NegativeMinusOne_1() {
			EnumI2 e = EnumI2.Negative50;
			return -51 == (short)(e - 1);
		}
		public static bool NegativeMinusOne_2() {
			EnumI2 e = EnumI2.Negative50;
			return EnumI2.Negative51 == e - 1;
		}
		public static bool NegativeShl() {
			EnumI2 e = EnumI2.Negative50;
			return -100 == (short)e << 1;
		}
		public static bool NegativeShr() {
			EnumI2 e = EnumI2.Negative50;
			return -25 == (short)e >> 1;
		}
		public static bool NegativeMul2() {
			EnumI2 e = EnumI2.Negative50;
			return -100 == (short)e * 2;
		}
		public static bool NegativeDiv2() {
			EnumI2 e = EnumI2.Negative50;
			return -25 == (short)e / 2;
		}
		public static bool NegativeRem2() {
			EnumI2 e = EnumI2.Negative50;
			return 0 == (short)e % 2;
		}
		public static bool NegativeAssignPlusOne() {
			EnumI2 e = EnumI2.Negative50;
			e += 1;
			return -49 == (short)e;
		}
		public static bool NegativeAssignMinusOne() {
			EnumI2 e = EnumI2.Negative50;
			e -= 1;
			return -51 == (short)e;
		}
		public static bool NegativePreincrement() {
			EnumI2 e = EnumI2.Negative50;
			bool retval = -49 == (short)(++e);
			return retval && -49 == (short)e;
		}
		public static bool NegativePredecrement() {
			EnumI2 e = EnumI2.Negative50;
			bool retval = -51 == (short)(--e);
			return retval && -51 == (short)e;
		}
		public static bool NegativePostincrement() {
			EnumI2 e = EnumI2.Negative50;
			bool retval = -50 == (short)(e++);
			return retval && -49 == (short)e;
		}
		public static bool NegativePostdecrement() {
			EnumI2 e = EnumI2.Negative50;
			bool retval = -50 == (short)(e--);
			return retval && -51 == (short)e;
		}
		public static bool NegativeAnd() {
			EnumI2 e = EnumI2.Negative50;
			return 206 == ((short)e & 0xFF);
		}
		public static bool NegativeOr() {
			EnumI2 e = EnumI2.Negative50;
			return -49 == ((short)e | 1);
		}
		public static bool NegativeXOr() {
			EnumI2 e = EnumI2.Negative50;
			return -49 == ((short)e ^ 1);
		}
		private static bool _NegativeEqual(EnumI2 e, short v) {
			return (short)e == v;
		}
		public static bool NegativeEqual1() {
			return (-50 == -50) == _NegativeEqual(EnumI2.Negative50, -50);
		}
		public static bool NegativeEqual2() {
			return (-50 == -49) == _NegativeEqual(EnumI2.Negative50, -49);
		}
		public static bool NegativeEqual3() {
			return (-50 == -51) == _NegativeEqual(EnumI2.Negative50, -51);
		}

		private static bool _NegativeNotEqual(EnumI2 e, short v) {
			return (short)e != v;
		}
		public static bool NegativeNotEqual1() {
			return (-50 != -50) == _NegativeNotEqual(EnumI2.Negative50, -50);
		}
		public static bool NegativeNotEqual2() {
			return (-50 != -49) == _NegativeNotEqual(EnumI2.Negative50, -49);
		}
		public static bool NegativeNotEqual3() {
			return (-50 != -51) == _NegativeNotEqual(EnumI2.Negative50, -51);
		}

		private static bool _NegativeGreaterThan(EnumI2 e, short v) {
			return (short)e > v;
		}
		public static bool NegativeGreaterThan1() {
			return (-50 > -50) == _NegativeGreaterThan(EnumI2.Negative50, -50);
		}
		public static bool NegativeGreaterThan2() {
			return (-50 > -49) == _NegativeGreaterThan(EnumI2.Negative50, -49);
		}
		public static bool NegativeGreaterThan3() {
			return (-50 > -51) == _NegativeGreaterThan(EnumI2.Negative50, -51);
		}

		private static bool _NegativeLessThan(EnumI2 e, short v) {
			return (short)e < v;
		}
		public static bool NegativeLessThan1() {
			return (-50 < -50) == _NegativeLessThan(EnumI2.Negative50, -50);
		}
		public static bool NegativeLessThan2() {
			return (-50 < -49) == _NegativeLessThan(EnumI2.Negative50, -49);
		}
		public static bool NegativeLessThan3() {
			return (-50 < -51) == _NegativeLessThan(EnumI2.Negative50, -51);
		}

		private static bool _NegativeGreaterThanOrEqual(EnumI2 e, short v) {
			return (short)e >= v;
		}
		public static bool NegativeGreaterThanOrEqual1() {
			return (-50 >= -50) == _NegativeGreaterThanOrEqual(EnumI2.Negative50, -50);
		}
		public static bool NegativeGreaterThanOrEqual2() {
			return (-50 >= -49) == _NegativeGreaterThanOrEqual(EnumI2.Negative50, -49);
		}
		public static bool NegativeGreaterThanOrEqual3() {
			return (-50 >= -51) == _NegativeGreaterThanOrEqual(EnumI2.Negative50, -51);
		}

		private static bool _NegativeLessThanOrEqual(EnumI2 e, short v) {
			return (short)e <= v;
		}
		public static bool NegativeLessThanOrEqual1() {
			return (-50 <= -50) == _NegativeLessThanOrEqual(EnumI2.Negative50, -50);
		}
		public static bool NegativeLessThanOrEqual2() {
			return (-50 <= -49) == _NegativeLessThanOrEqual(EnumI2.Negative50, -49);
		}
		public static bool NegativeLessThanOrEqual3() {
			return (-50 <= -51) == _NegativeLessThanOrEqual(EnumI2.Negative50, -51);
		}

	}

	enum EnumI4 : int {
		Negative51 = -51,
		Negative50,
		Negative49,
		Positive49 = 49,
		Positive50,
		Positive51
	}

	public static class TestEnumI4Class {
		public static bool PositiveConversion() {
			EnumI4 e = EnumI4.Positive50;
			return 50 == (int)e;
		}
		public static bool PositivePlusOne_1() {
			EnumI4 e = EnumI4.Positive50;
			return 51 == (int)(e + 1);
		}
		public static bool PositivePlusOne_2() {
			EnumI4 e = EnumI4.Positive50;
			return EnumI4.Positive51 == e + 1;
		}
		public static bool PositiveMinusOne_1() {
			EnumI4 e = EnumI4.Positive50;
			return 49 == (int)(e - 1);
		}
		public static bool PositiveMinusOne_2() {
			EnumI4 e = EnumI4.Positive50;
			return EnumI4.Positive49 == e - 1;
		}
		public static bool PositiveShl() {
			EnumI4 e = EnumI4.Positive50;
			return 100 == (int)e << 1;
		}
		public static bool PositiveShr() {
			EnumI4 e = EnumI4.Positive50;
			return 25 == (int)e >> 1;
		}
		public static bool PositiveMul2() {
			EnumI4 e = EnumI4.Positive50;
			return 100 == (int)e * 2;
		}
		public static bool PositiveDiv2() {
			EnumI4 e = EnumI4.Positive50;
			return 25 == (int)e / 2;
		}
		public static bool PositiveRem2() {
			EnumI4 e = EnumI4.Positive50;
			return 0 == (int)e % 2;
		}
		public static bool PositiveAssignPlusOne() {
			EnumI4 e = EnumI4.Positive50;
			e += 1;
			return 51 == (int)e;
		}
		public static bool PositiveAssignMinusOne() {
			EnumI4 e = EnumI4.Positive50;
			e -= 1;
			return 49 == (int)e;
		}
		public static bool PositivePreincrement() {
			EnumI4 e = EnumI4.Positive50;
			bool retval = 51 == (int)(++e);
			return retval && 51 == (int)e;
		}
		public static bool PositivePredecrement() {
			EnumI4 e = EnumI4.Positive50;
			bool retval = 49 == (int)(--e);
			return retval && 49 == (int)e;
		}
		public static bool PositivePostincrement() {
			EnumI4 e = EnumI4.Positive50;
			bool retval = 50 == (int)(e++);
			return retval && 51 == (int)e;
		}
		public static bool PositivePostdecrement() {
			EnumI4 e = EnumI4.Positive50;
			bool retval = 50 == (int)(e--);
			return retval && 49 == (int)e;
		}
		public static bool PositiveAnd() {
			EnumI4 e = EnumI4.Positive50;
			return 50 == ((int)e & 0xFF);
		}
		public static bool PositiveOr() {
			EnumI4 e = EnumI4.Positive50;
			return 51 == ((int)e | 1);
		}
		public static bool PositiveXOr() {
			EnumI4 e = EnumI4.Positive50;
			return 51 == ((int)e ^ 1);
		}
		private static bool _PositiveEqual(EnumI4 e, int v) {
			return (int)e == v;
		}
		public static bool PositiveEqual1() {
			return (50 == 50) == _PositiveEqual(EnumI4.Positive50, 50);
		}
		public static bool PositiveEqual2() {
			return (50 == 51) == _PositiveEqual(EnumI4.Positive50, 51);
		}
		public static bool PositiveEqual3() {
			return (50 == 49) == _PositiveEqual(EnumI4.Positive50, 49);
		}

		private static bool _PositiveNotEqual(EnumI4 e, int v) {
			return (int)e != v;
		}
		public static bool PositiveNotEqual1() {
			return (50 != 50) == _PositiveNotEqual(EnumI4.Positive50, 50);
		}
		public static bool PositiveNotEqual2() {
			return (50 != 51) == _PositiveNotEqual(EnumI4.Positive50, 51);
		}
		public static bool PositiveNotEqual3() {
			return (50 != 49) == _PositiveNotEqual(EnumI4.Positive50, 49);
		}

		private static bool _PositiveGreaterThan(EnumI4 e, int v) {
			return (int)e > v;
		}
		public static bool PositiveGreaterThan1() {
			return (50 > 50) == _PositiveGreaterThan(EnumI4.Positive50, 50);
		}
		public static bool PositiveGreaterThan2() {
			return (50 > 51) == _PositiveGreaterThan(EnumI4.Positive50, 51);
		}
		public static bool PositiveGreaterThan3() {
			return (50 > 49) == _PositiveGreaterThan(EnumI4.Positive50, 49);
		}

		private static bool _PositiveLessThan(EnumI4 e, int v) {
			return (int)e < v;
		}
		public static bool PositiveLessThan1() {
			return (50 < 50) == _PositiveLessThan(EnumI4.Positive50, 50);
		}
		public static bool PositiveLessThan2() {
			return (50 < 51) == _PositiveLessThan(EnumI4.Positive50, 51);
		}
		public static bool PositiveLessThan3() {
			return (50 < 49) == _PositiveLessThan(EnumI4.Positive50, 49);
		}

		private static bool _PositiveGreaterThanOrEqual(EnumI4 e, int v) {
			return (int)e >= v;
		}
		public static bool PositiveGreaterThanOrEqual1() {
			return (50 >= 50) == _PositiveGreaterThanOrEqual(EnumI4.Positive50, 50);
		}
		public static bool PositiveGreaterThanOrEqual2() {
			return (50 >= 51) == _PositiveGreaterThanOrEqual(EnumI4.Positive50, 51);
		}
		public static bool PositiveGreaterThanOrEqual3() {
			return (50 >= 49) == _PositiveGreaterThanOrEqual(EnumI4.Positive50, 49);
		}

		private static bool _PositiveLessThanOrEqual(EnumI4 e, int v) {
			return (int)e <= v;
		}
		public static bool PositiveLessThanOrEqual1() {
			return (50 <= 50) == _PositiveLessThanOrEqual(EnumI4.Positive50, 50);
		}
		public static bool PositiveLessThanOrEqual2() {
			return (50 <= 51) == _PositiveLessThanOrEqual(EnumI4.Positive50, 51);
		}
		public static bool PositiveLessThanOrEqual3() {
			return (50 <= 49) == _PositiveLessThanOrEqual(EnumI4.Positive50, 49);
		}

		public static bool NegativeConversion() {
			EnumI4 e = EnumI4.Negative50;
			return -50 == (int)e;
		}
		public static bool NegativePlusOne_1() {
			EnumI4 e = EnumI4.Negative50;
			return -49 == (int)(e + 1);
		}
		public static bool NegativePlusOne_2() {
			EnumI4 e = EnumI4.Negative50;
			return EnumI4.Negative49 == e + 1;
		}
		public static bool NegativeMinusOne_1() {
			EnumI4 e = EnumI4.Negative50;
			return -51 == (int)(e - 1);
		}
		public static bool NegativeMinusOne_2() {
			EnumI4 e = EnumI4.Negative50;
			return EnumI4.Negative51 == e - 1;
		}
		public static bool NegativeShl() {
			EnumI4 e = EnumI4.Negative50;
			return -100 == (int)e << 1;
		}
		public static bool NegativeShr() {
			EnumI4 e = EnumI4.Negative50;
			return -25 == (int)e >> 1;
		}
		public static bool NegativeMul2() {
			EnumI4 e = EnumI4.Negative50;
			return -100 == (int)e * 2;
		}
		public static bool NegativeDiv2() {
			EnumI4 e = EnumI4.Negative50;
			return -25 == (int)e / 2;
		}
		public static bool NegativeRem2() {
			EnumI4 e = EnumI4.Negative50;
			return 0 == (int)e % 2;
		}
		public static bool NegativeAssignPlusOne() {
			EnumI4 e = EnumI4.Negative50;
			e += 1;
			return -49 == (int)e;
		}
		public static bool NegativeAssignMinusOne() {
			EnumI4 e = EnumI4.Negative50;
			e -= 1;
			return -51 == (int)e;
		}
		public static bool NegativePreincrement() {
			EnumI4 e = EnumI4.Negative50;
			bool retval = -49 == (int)(++e);
			return retval && -49 == (int)e;
		}
		public static bool NegativePredecrement() {
			EnumI4 e = EnumI4.Negative50;
			bool retval = -51 == (int)(--e);
			return retval && -51 == (int)e;
		}
		public static bool NegativePostincrement() {
			EnumI4 e = EnumI4.Negative50;
			bool retval = -50 == (int)(e++);
			return retval && -49 == (int)e;
		}
		public static bool NegativePostdecrement() {
			EnumI4 e = EnumI4.Negative50;
			bool retval = -50 == (int)(e--);
			return retval && -51 == (int)e;
		}
		public static bool NegativeAnd() {
			EnumI4 e = EnumI4.Negative50;
			return 206 == ((int)e & 0xFF);
		}
		public static bool NegativeOr() {
			EnumI4 e = EnumI4.Negative50;
			return -49 == ((int)e | 1);
		}
		public static bool NegativeXOr() {
			EnumI4 e = EnumI4.Negative50;
			return -49 == ((int)e ^ 1);
		}
		private static bool _NegativeEqual(EnumI4 e, int v) {
			return (int)e == v;
		}
		public static bool NegativeEqual1() {
			return (-50 == -50) == _NegativeEqual(EnumI4.Negative50, -50);
		}
		public static bool NegativeEqual2() {
			return (-50 == -49) == _NegativeEqual(EnumI4.Negative50, -49);
		}
		public static bool NegativeEqual3() {
			return (-50 == -51) == _NegativeEqual(EnumI4.Negative50, -51);
		}

		private static bool _NegativeNotEqual(EnumI4 e, int v) {
			return (int)e != v;
		}
		public static bool NegativeNotEqual1() {
			return (-50 != -50) == _NegativeNotEqual(EnumI4.Negative50, -50);
		}
		public static bool NegativeNotEqual2() {
			return (-50 != -49) == _NegativeNotEqual(EnumI4.Negative50, -49);
		}
		public static bool NegativeNotEqual3() {
			return (-50 != -51) == _NegativeNotEqual(EnumI4.Negative50, -51);
		}

		private static bool _NegativeGreaterThan(EnumI4 e, int v) {
			return (int)e > v;
		}
		public static bool NegativeGreaterThan1() {
			return (-50 > -50) == _NegativeGreaterThan(EnumI4.Negative50, -50);
		}
		public static bool NegativeGreaterThan2() {
			return (-50 > -49) == _NegativeGreaterThan(EnumI4.Negative50, -49);
		}
		public static bool NegativeGreaterThan3() {
			return (-50 > -51) == _NegativeGreaterThan(EnumI4.Negative50, -51);
		}

		private static bool _NegativeLessThan(EnumI4 e, int v) {
			return (int)e < v;
		}
		public static bool NegativeLessThan1() {
			return (-50 < -50) == _NegativeLessThan(EnumI4.Negative50, -50);
		}
		public static bool NegativeLessThan2() {
			return (-50 < -49) == _NegativeLessThan(EnumI4.Negative50, -49);
		}
		public static bool NegativeLessThan3() {
			return (-50 < -51) == _NegativeLessThan(EnumI4.Negative50, -51);
		}

		private static bool _NegativeGreaterThanOrEqual(EnumI4 e, int v) {
			return (int)e >= v;
		}
		public static bool NegativeGreaterThanOrEqual1() {
			return (-50 >= -50) == _NegativeGreaterThanOrEqual(EnumI4.Negative50, -50);
		}
		public static bool NegativeGreaterThanOrEqual2() {
			return (-50 >= -49) == _NegativeGreaterThanOrEqual(EnumI4.Negative50, -49);
		}
		public static bool NegativeGreaterThanOrEqual3() {
			return (-50 >= -51) == _NegativeGreaterThanOrEqual(EnumI4.Negative50, -51);
		}

		private static bool _NegativeLessThanOrEqual(EnumI4 e, int v) {
			return (int)e <= v;
		}
		public static bool NegativeLessThanOrEqual1() {
			return (-50 <= -50) == _NegativeLessThanOrEqual(EnumI4.Negative50, -50);
		}
		public static bool NegativeLessThanOrEqual2() {
			return (-50 <= -49) == _NegativeLessThanOrEqual(EnumI4.Negative50, -49);
		}
		public static bool NegativeLessThanOrEqual3() {
			return (-50 <= -51) == _NegativeLessThanOrEqual(EnumI4.Negative50, -51);
		}

	}

	enum EnumI8 : long {
		Negative51 = -51,
		Negative50,
		Negative49,
		Positive49 = 49,
		Positive50,
		Positive51
	}

	public static class TestEnumI8Class {
		public static bool PositiveConversion() {
			EnumI8 e = EnumI8.Positive50;
			return 50 == (long)e;
		}
		public static bool PositivePlusOne_1() {
			EnumI8 e = EnumI8.Positive50;
			return 51 == (long)(e + 1);
		}
		public static bool PositivePlusOne_2() {
			EnumI8 e = EnumI8.Positive50;
			return EnumI8.Positive51 == e + 1;
		}
		public static bool PositiveMinusOne_1() {
			EnumI8 e = EnumI8.Positive50;
			return 49 == (long)(e - 1);
		}
		public static bool PositiveMinusOne_2() {
			EnumI8 e = EnumI8.Positive50;
			return EnumI8.Positive49 == e - 1;
		}
		public static bool PositiveShl() {
			EnumI8 e = EnumI8.Positive50;
			return 100 == (long)e << 1;
		}
		public static bool PositiveShr() {
			EnumI8 e = EnumI8.Positive50;
			return 25 == (long)e >> 1;
		}
		public static bool PositiveMul2() {
			EnumI8 e = EnumI8.Positive50;
			return 100 == (long)e * 2;
		}
		public static bool PositiveDiv2() {
			EnumI8 e = EnumI8.Positive50;
			return 25 == (long)e / 2;
		}
		public static bool PositiveRem2() {
			EnumI8 e = EnumI8.Positive50;
			return 0 == (long)e % 2;
		}
		public static bool PositiveAssignPlusOne() {
			EnumI8 e = EnumI8.Positive50;
			e += 1;
			return 51 == (long)e;
		}
		public static bool PositiveAssignMinusOne() {
			EnumI8 e = EnumI8.Positive50;
			e -= 1;
			return 49 == (long)e;
		}
		public static bool PositivePreincrement() {
			EnumI8 e = EnumI8.Positive50;
			bool retval = 51 == (long)(++e);
			return retval && 51 == (long)e;
		}
		public static bool PositivePredecrement() {
			EnumI8 e = EnumI8.Positive50;
			bool retval = 49 == (long)(--e);
			return retval && 49 == (long)e;
		}
		public static bool PositivePostincrement() {
			EnumI8 e = EnumI8.Positive50;
			bool retval = 50 == (long)(e++);
			return retval && 51 == (long)e;
		}
		public static bool PositivePostdecrement() {
			EnumI8 e = EnumI8.Positive50;
			bool retval = 50 == (long)(e--);
			return retval && 49 == (long)e;
		}
		public static bool PositiveAnd() {
			EnumI8 e = EnumI8.Positive50;
			return 50 == ((long)e & 0xFF);
		}
		public static bool PositiveOr() {
			EnumI8 e = EnumI8.Positive50;
			return 51 == ((long)e | 1);
		}
		public static bool PositiveXOr() {
			EnumI8 e = EnumI8.Positive50;
			return 51 == ((long)e ^ 1);
		}
		private static bool _PositiveEqual(EnumI8 e, long v) {
			return (long)e == v;
		}
		public static bool PositiveEqual1() {
			return (50 == 50) == _PositiveEqual(EnumI8.Positive50, 50);
		}
		public static bool PositiveEqual2() {
			return (50 == 51) == _PositiveEqual(EnumI8.Positive50, 51);
		}
		public static bool PositiveEqual3() {
			return (50 == 49) == _PositiveEqual(EnumI8.Positive50, 49);
		}

		private static bool _PositiveNotEqual(EnumI8 e, long v) {
			return (long)e != v;
		}
		public static bool PositiveNotEqual1() {
			return (50 != 50) == _PositiveNotEqual(EnumI8.Positive50, 50);
		}
		public static bool PositiveNotEqual2() {
			return (50 != 51) == _PositiveNotEqual(EnumI8.Positive50, 51);
		}
		public static bool PositiveNotEqual3() {
			return (50 != 49) == _PositiveNotEqual(EnumI8.Positive50, 49);
		}

		private static bool _PositiveGreaterThan(EnumI8 e, long v) {
			return (long)e > v;
		}
		public static bool PositiveGreaterThan1() {
			return (50 > 50) == _PositiveGreaterThan(EnumI8.Positive50, 50);
		}
		public static bool PositiveGreaterThan2() {
			return (50 > 51) == _PositiveGreaterThan(EnumI8.Positive50, 51);
		}
		public static bool PositiveGreaterThan3() {
			return (50 > 49) == _PositiveGreaterThan(EnumI8.Positive50, 49);
		}

		private static bool _PositiveLessThan(EnumI8 e, long v) {
			return (long)e < v;
		}
		public static bool PositiveLessThan1() {
			return (50 < 50) == _PositiveLessThan(EnumI8.Positive50, 50);
		}
		public static bool PositiveLessThan2() {
			return (50 < 51) == _PositiveLessThan(EnumI8.Positive50, 51);
		}
		public static bool PositiveLessThan3() {
			return (50 < 49) == _PositiveLessThan(EnumI8.Positive50, 49);
		}

		private static bool _PositiveGreaterThanOrEqual(EnumI8 e, long v) {
			return (long)e >= v;
		}
		public static bool PositiveGreaterThanOrEqual1() {
			return (50 >= 50) == _PositiveGreaterThanOrEqual(EnumI8.Positive50, 50);
		}
		public static bool PositiveGreaterThanOrEqual2() {
			return (50 >= 51) == _PositiveGreaterThanOrEqual(EnumI8.Positive50, 51);
		}
		public static bool PositiveGreaterThanOrEqual3() {
			return (50 >= 49) == _PositiveGreaterThanOrEqual(EnumI8.Positive50, 49);
		}

		private static bool _PositiveLessThanOrEqual(EnumI8 e, long v) {
			return (long)e <= v;
		}
		public static bool PositiveLessThanOrEqual1() {
			return (50 <= 50) == _PositiveLessThanOrEqual(EnumI8.Positive50, 50);
		}
		public static bool PositiveLessThanOrEqual2() {
			return (50 <= 51) == _PositiveLessThanOrEqual(EnumI8.Positive50, 51);
		}
		public static bool PositiveLessThanOrEqual3() {
			return (50 <= 49) == _PositiveLessThanOrEqual(EnumI8.Positive50, 49);
		}

		public static bool NegativeConversion() {
			EnumI8 e = EnumI8.Negative50;
			return -50 == (long)e;
		}
		public static bool NegativePlusOne_1() {
			EnumI8 e = EnumI8.Negative50;
			return -49 == (long)(e + 1);
		}
		public static bool NegativePlusOne_2() {
			EnumI8 e = EnumI8.Negative50;
			return EnumI8.Negative49 == e + 1;
		}
		public static bool NegativeMinusOne_1() {
			EnumI8 e = EnumI8.Negative50;
			return -51 == (long)(e - 1);
		}
		public static bool NegativeMinusOne_2() {
			EnumI8 e = EnumI8.Negative50;
			return EnumI8.Negative51 == e - 1;
		}
		public static bool NegativeShl() {
			EnumI8 e = EnumI8.Negative50;
			return -100 == (long)e << 1;
		}
		public static bool NegativeShr() {
			EnumI8 e = EnumI8.Negative50;
			return -25 == (long)e >> 1;
		}
		public static bool NegativeMul2() {
			EnumI8 e = EnumI8.Negative50;
			return -100 == (long)e * 2;
		}
		public static bool NegativeDiv2() {
			EnumI8 e = EnumI8.Negative50;
			return -25 == (long)e / 2;
		}
		public static bool NegativeRem2() {
			EnumI8 e = EnumI8.Negative50;
			return 0 == (long)e % 2;
		}
		public static bool NegativeAssignPlusOne() {
			EnumI8 e = EnumI8.Negative50;
			e += 1;
			return -49 == (long)e;
		}
		public static bool NegativeAssignMinusOne() {
			EnumI8 e = EnumI8.Negative50;
			e -= 1;
			return -51 == (long)e;
		}
		public static bool NegativePreincrement() {
			EnumI8 e = EnumI8.Negative50;
			bool retval = -49 == (long)(++e);
			return retval && -49 == (long)e;
		}
		public static bool NegativePredecrement() {
			EnumI8 e = EnumI8.Negative50;
			bool retval = -51 == (long)(--e);
			return retval && -51 == (long)e;
		}
		public static bool NegativePostincrement() {
			EnumI8 e = EnumI8.Negative50;
			bool retval = -50 == (long)(e++);
			return retval && -49 == (long)e;
		}
		public static bool NegativePostdecrement() {
			EnumI8 e = EnumI8.Negative50;
			bool retval = -50 == (long)(e--);
			return retval && -51 == (long)e;
		}
		public static bool NegativeAnd() {
			EnumI8 e = EnumI8.Negative50;
			return 206 == ((long)e & 0xFF);
		}
		public static bool NegativeOr() {
			EnumI8 e = EnumI8.Negative50;
			return -49 == ((long)e | 1);
		}
		public static bool NegativeXOr() {
			EnumI8 e = EnumI8.Negative50;
			return -49 == ((long)e ^ 1);
		}
		private static bool _NegativeEqual(EnumI8 e, long v) {
			return (long)e == v;
		}
		public static bool NegativeEqual1() {
			return (-50 == -50) == _NegativeEqual(EnumI8.Negative50, -50);
		}
		public static bool NegativeEqual2() {
			return (-50 == -49) == _NegativeEqual(EnumI8.Negative50, -49);
		}
		public static bool NegativeEqual3() {
			return (-50 == -51) == _NegativeEqual(EnumI8.Negative50, -51);
		}

		private static bool _NegativeNotEqual(EnumI8 e, long v) {
			return (long)e != v;
		}
		public static bool NegativeNotEqual1() {
			return (-50 != -50) == _NegativeNotEqual(EnumI8.Negative50, -50);
		}
		public static bool NegativeNotEqual2() {
			return (-50 != -49) == _NegativeNotEqual(EnumI8.Negative50, -49);
		}
		public static bool NegativeNotEqual3() {
			return (-50 != -51) == _NegativeNotEqual(EnumI8.Negative50, -51);
		}

		private static bool _NegativeGreaterThan(EnumI8 e, long v) {
			return (long)e > v;
		}
		public static bool NegativeGreaterThan1() {
			return (-50 > -50) == _NegativeGreaterThan(EnumI8.Negative50, -50);
		}
		public static bool NegativeGreaterThan2() {
			return (-50 > -49) == _NegativeGreaterThan(EnumI8.Negative50, -49);
		}
		public static bool NegativeGreaterThan3() {
			return (-50 > -51) == _NegativeGreaterThan(EnumI8.Negative50, -51);
		}

		private static bool _NegativeLessThan(EnumI8 e, long v) {
			return (long)e < v;
		}
		public static bool NegativeLessThan1() {
			return (-50 < -50) == _NegativeLessThan(EnumI8.Negative50, -50);
		}
		public static bool NegativeLessThan2() {
			return (-50 < -49) == _NegativeLessThan(EnumI8.Negative50, -49);
		}
		public static bool NegativeLessThan3() {
			return (-50 < -51) == _NegativeLessThan(EnumI8.Negative50, -51);
		}

		private static bool _NegativeGreaterThanOrEqual(EnumI8 e, long v) {
			return (long)e >= v;
		}
		public static bool NegativeGreaterThanOrEqual1() {
			return (-50 >= -50) == _NegativeGreaterThanOrEqual(EnumI8.Negative50, -50);
		}
		public static bool NegativeGreaterThanOrEqual2() {
			return (-50 >= -49) == _NegativeGreaterThanOrEqual(EnumI8.Negative50, -49);
		}
		public static bool NegativeGreaterThanOrEqual3() {
			return (-50 >= -51) == _NegativeGreaterThanOrEqual(EnumI8.Negative50, -51);
		}

		private static bool _NegativeLessThanOrEqual(EnumI8 e, long v) {
			return (long)e <= v;
		}
		public static bool NegativeLessThanOrEqual1() {
			return (-50 <= -50) == _NegativeLessThanOrEqual(EnumI8.Negative50, -50);
		}
		public static bool NegativeLessThanOrEqual2() {
			return (-50 <= -49) == _NegativeLessThanOrEqual(EnumI8.Negative50, -49);
		}
		public static bool NegativeLessThanOrEqual3() {
			return (-50 <= -51) == _NegativeLessThanOrEqual(EnumI8.Negative50, -51);
		}

	}

}
