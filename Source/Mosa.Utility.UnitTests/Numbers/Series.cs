﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.UnitTests.Numbers;

public static class Series
{
	#region Utilities

	public static IEnumerable<int> GetUpTo(int end)
	{
		for (var i = 0; i < end; i++)
			yield return i;
	}

	public static IEnumerable<int> I4Small
	{
		get
		{
			yield return 0;
			yield return 1;
			yield return 2;
			yield return 4;
			yield return 6;
			yield return 7;
			yield return 8;
			yield return 10;
		}
	}

	#endregion Utilities

	#region B Types

	public static IEnumerable<bool> B => Numbers.B.Series;

	#endregion B Types

	#region C Types

	public static IEnumerable<char> C => Numbers.C.Series;

	public static IEnumerable<char> CNotZero
	{
		get { foreach (var a in C) if (a != 0) yield return a; }
	}

	public static IEnumerable<char> CAboveZero
	{
		get { foreach (var a in C) if (a > 0) yield return a; }
	}

	public static IEnumerable<char> CBelowZero
	{
		get { foreach (var a in C) if (a < 0) yield return a; }
	}

	public static IEnumerable<char> CUpTo8
	{
		get { foreach (char a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<char> CUpTo16
	{
		get { foreach (char a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<char> CUpTo32
	{
		get { foreach (char a in GetUpTo(32)) yield return a; }
	}

	#endregion C Types

	#region I1 Types

	public static IEnumerable<sbyte> I1 => Numbers.I1.Series;

	public static IEnumerable<sbyte> I1NotZero
	{
		get { foreach (var a in I1) if (a != 0) yield return a; }
	}

	public static IEnumerable<sbyte> I1AboveZero
	{
		get { foreach (var a in I1) if (a > 0) yield return a; }
	}

	public static IEnumerable<sbyte> I1BelowZero
	{
		get { foreach (var a in I1) if (a < 0) yield return a; }
	}

	public static IEnumerable<sbyte> I1UpTo8
	{
		get { foreach (sbyte a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<sbyte> I1UpTo16
	{
		get { foreach (sbyte a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<sbyte> I1UpTo32
	{
		get { foreach (sbyte a in GetUpTo(32)) yield return a; }
	}

	#endregion I1 Types

	#region U1 Types

	public static IEnumerable<byte> U1 => Numbers.U1.Series;

	public static IEnumerable<byte> U1NotZero
	{
		get { foreach (var a in U1) if (a != 0) yield return a; }
	}

	public static IEnumerable<byte> U1AboveZero
	{
		get { foreach (var a in U1) if (a > 0) yield return a; }
	}

	public static IEnumerable<byte> U1UpTo8
	{
		get { foreach (byte a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<byte> U1UpTo16
	{
		get { foreach (byte a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<byte> U1UpTo32
	{
		get { foreach (byte a in GetUpTo(32)) yield return a; }
	}

	public static IEnumerable<byte> U1UpTo64
	{
		get { foreach (byte a in GetUpTo(32)) yield return a; }
	}

	#endregion U1 Types

	#region I2 Types

	public static IEnumerable<short> I2 => Numbers.I2.Series;

	public static IEnumerable<short> I2NotZero
	{
		get { foreach (var a in I2) if (a != 0) yield return a; }
	}

	public static IEnumerable<short> I2AboveZero
	{
		get { foreach (var a in I2) if (a > 0) yield return a; }
	}

	public static IEnumerable<short> I2BelowZero
	{
		get { foreach (var a in I2) if (a < 0) yield return a; }
	}

	public static IEnumerable<short> I2UpTo8
	{
		get { foreach (short a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<short> I2UpTo16
	{
		get { foreach (short a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<short> I2UpTo32
	{
		get { foreach (short a in GetUpTo(32)) yield return a; }
	}

	#endregion I2 Types

	#region U2 Types

	public static IEnumerable<ushort> U2 => Numbers.U2.Series;

	public static IEnumerable<ushort> U2NotZero
	{
		get { foreach (var a in U2) if (a != 0) yield return a; }
	}

	public static IEnumerable<ushort> U2AboveZero
	{
		get { foreach (var a in U2) if (a > 0) yield return a; }
	}

	public static IEnumerable<ushort> U2UpTo8
	{
		get { foreach (ushort a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<ushort> U2UpTo16
	{
		get { foreach (ushort a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<ushort> U2UpTo32
	{
		get { foreach (ushort a in GetUpTo(32)) yield return a; }
	}

	#endregion U2 Types

	#region I4 Types

	public static IEnumerable<int> I4 => Numbers.I4.Series;

	public static IEnumerable<int> I4NotZero
	{
		get { foreach (var a in I4) if (a != 0) yield return a; }
	}

	public static IEnumerable<int> I4AboveZero
	{
		get { foreach (var a in I4) if (a > 0) yield return a; }
	}

	public static IEnumerable<int> I4BelowZero
	{
		get { foreach (var a in I4) if (a < 0) yield return a; }
	}

	public static IEnumerable<int> I4UpTo8
	{
		get { foreach (var a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<int> I4UpTo16
	{
		get { foreach (var a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<int> I4UpTo32
	{
		get { foreach (var a in GetUpTo(32)) yield return a; }
	}

	public static IEnumerable<int> I4Mini
	{
		get { yield return 0; yield return 1; yield return int.MinValue; yield return int.MaxValue; }
	}

	public static IEnumerable<int> FewScatteredAI4
	{
		get { yield return 1; yield return 51; }
	}

	public static IEnumerable<int> FewScatteredBI4
	{
		get { yield return 101; yield return 9999; }
	}

	public static IEnumerable<int> FewScatteredCI4
	{
		get { yield return 33; yield return 99; }
	}

	#endregion I4 Types

	#region U4 Types

	public static IEnumerable<uint> U4 => Numbers.U4.Series;

	public static IEnumerable<uint> U4NotZero
	{
		get { foreach (var a in U4) if (a != 0) yield return a; }
	}

	public static IEnumerable<uint> U4AboveZero
	{
		get { foreach (var a in U4) if (a > 0) yield return a; }
	}

	public static IEnumerable<uint> U4UpTo16
	{
		get { foreach (uint a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<uint> U4UpTo32
	{
		get { foreach (uint a in GetUpTo(32)) yield return a; }
	}

	public static IEnumerable<uint> U4UpTo8
	{
		get { foreach (uint a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<uint> U4Mini
	{
		get { yield return 0; yield return 1; yield return uint.MinValue; yield return uint.MaxValue; }
	}

	#endregion U4 Types

	#region I8 Types

	public static IEnumerable<long> I8 => Numbers.I8.Series;

	public static IEnumerable<long> I8NotZero
	{
		get { foreach (var a in I8) if (a != 0) yield return a; }
	}

	public static IEnumerable<long> I8AboveZero
	{
		get { foreach (var a in I8) if (a > 0) yield return a; }
	}

	public static IEnumerable<long> I8BelowZero
	{
		get { foreach (var a in I8) if (a < 0) yield return a; }
	}

	public static IEnumerable<long> I8UpTo8
	{
		get { foreach (long a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<long> I8UpTo16
	{
		get { foreach (long a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<long> I8UpTo32
	{
		get { foreach (long a in GetUpTo(32)) yield return a; }
	}

	public static IEnumerable<long> I8Mini
	{
		get
		{
			yield return 0;
			yield return 1;
			yield return long.MinValue;
			yield return long.MaxValue;
		}
	}

	#endregion I8 Types

	#region U8 Types

	public static IEnumerable<ulong> U8 => Numbers.U8.Series;

	public static IEnumerable<ulong> U8NotZero
	{
		get { foreach (var a in U8) if (a != 0) yield return a; }
	}

	public static IEnumerable<ulong> U8AboveZero
	{
		get { foreach (var a in U8) if (a > 0) yield return a; }
	}

	public static IEnumerable<ulong> U8UpTo8
	{
		get { foreach (ulong a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<ulong> U8UpTo16
	{
		get { foreach (ulong a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<ulong> U8UpTo32
	{
		get { foreach (ulong a in GetUpTo(32)) yield return a; }
	}

	public static IEnumerable<ulong> U8Mini
	{
		get { yield return 0; yield return 1; yield return ulong.MinValue; yield return ulong.MaxValue; }
	}

	#endregion U8 Types

	#region R4 Types

	public static IEnumerable<float> R4 => Numbers.R4.Series;

	public static IEnumerable<float> R4Mini
	{
		get
		{
			yield return 0;
			yield return float.MinValue;
			yield return float.MaxValue;
			yield return float.NaN;
			yield return float.NegativeInfinity;
		}
	}

	public static IEnumerable<float> R4NotNaN
	{
		get { foreach (var a in R4) if (!float.IsNaN(a)) yield return a; }
	}

	public static IEnumerable<float> R4Number
	{
		get { foreach (var a in R4) if (!float.IsNaN(a) && !float.IsInfinity(a)) yield return a; }
	}

	public static IEnumerable<float> R4NumberNotZero
	{
		get { foreach (float a in R8) if (a != 0) if (!float.IsNaN(a) && !float.IsInfinity(a)) yield return a; }
	}

	public static IEnumerable<float> R4NumberNoExtremes
	{
		get { foreach (var a in R4Number) if (!(float.MaxValue == a) && !(float.MinValue == a)) yield return a; }
	}

	public static IEnumerable<float> R4NumberNoExtremesOrZero
	{
		get { foreach (var a in R4NumberNoExtremes) if (a != 0) yield return a; }
	}

	public static IEnumerable<float> R4NotZero
	{
		get { foreach (var a in R4) if (a != 0) yield return a; }
	}

	public static IEnumerable<float> R4AboveZero
	{
		get { foreach (var a in R4) if (a > 0) yield return a; }
	}

	public static IEnumerable<float> R4BelowZero
	{
		get { foreach (var a in R4) if (a < 0) yield return a; }
	}

	public static IEnumerable<float> R4UpTo8
	{
		get { foreach (float a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<float> R4UpTo16
	{
		get { foreach (float a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<float> R4UpTo32
	{
		get { foreach (float a in GetUpTo(32)) yield return a; }
	}

	public static IEnumerable<float> R4FitsI4
	{
		get { foreach (var a in R4) if (!float.IsNaN(a) && a <= int.MaxValue && a >= int.MinValue) yield return a; }
	}

	public static IEnumerable<float> R4Simple
	{
		get
		{
			yield return 0f;
			yield return 1f;
			yield return 2f;
			yield return 4f;
			yield return 8f;
			yield return 10f;
		}
	}

	#endregion R4 Types

	#region R8 Types

	public static IEnumerable<double> R8 => Numbers.R8.Series;

	public static IEnumerable<double> R8Mini
	{
		get
		{
			yield return 0;
			yield return double.MinValue;
			yield return double.MaxValue;
			yield return double.NaN;
			yield return double.NegativeInfinity;
		}
	}

	public static IEnumerable<double> R8NotNaN
	{
		get { foreach (var a in R8) if (!double.IsNaN(a)) yield return a; }
	}

	public static IEnumerable<double> R8Number
	{
		get { foreach (var a in R8) if (!double.IsNaN(a) && !double.IsInfinity(a)) yield return a; }
	}

	public static IEnumerable<double> R8NumberNotZero
	{
		get { foreach (var a in R8) if (a != 0) if (!double.IsNaN(a) && !double.IsInfinity(a)) yield return a; }
	}

	public static IEnumerable<double> R8NumberNoExtremes
	{
		get { foreach (var a in R8Number) if (!(double.MaxValue == a) && !(double.MinValue == a)) yield return a; }
	}

	public static IEnumerable<double> R8NumberNoExtremesOrZero
	{
		get { foreach (var a in R8NumberNoExtremes) if (a != 0) yield return a; }
	}

	public static IEnumerable<double> R8NotZero
	{
		get { foreach (var a in R8) if (a != 0) yield return a; }
	}

	public static IEnumerable<double> R8AboveZero
	{
		get { foreach (var a in R8) if (a > 0) yield return a; }
	}

	public static IEnumerable<double> R8BelowZero
	{
		get { foreach (var a in R8) if (a < 0) yield return a; }
	}

	public static IEnumerable<double> R8UpTo8
	{
		get { foreach (double a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<double> R8UpTo16
	{
		get { foreach (double a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<double> R8UpTo32
	{
		get { foreach (double a in GetUpTo(32)) yield return a; }
	}

	public static IEnumerable<double> R8FitsI4
	{
		get { foreach (var a in R8) if (!double.IsNaN(a) && a <= int.MaxValue && a >= int.MinValue) yield return a; }
	}

	public static IEnumerable<double> R8Simple
	{
		get
		{
			yield return 0d;
			yield return 1d;
			yield return 2d;
			yield return 4d;
			yield return 8d;
			yield return 10d;
		}
	}

	#endregion R8 Types
}
