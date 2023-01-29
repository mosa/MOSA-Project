// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.UnitTests.Numbers;

public static class Series2
{
	#region Utilities

	public static IEnumerable<object> GetUpTo(int end)
	{
		for (int i = 0; i < end; i++)
			yield return i;
	}

	public static IEnumerable<object> I4Small
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

	public static IEnumerable<object> B
	{
		get { foreach (bool a in Numbers.B.Series) yield return a; }
	}

	#endregion B Types

	#region C Types

	public static IEnumerable<object> C
	{
		get { foreach (char a in Numbers.C.Series) yield return a; }
	}

	public static IEnumerable<object> CNotZero
	{
		get { foreach (char a in C) if (a != 0) yield return a; }
	}

	public static IEnumerable<object> CAboveZero
	{
		get { foreach (char a in C) if (a > 0) yield return a; }
	}

	public static IEnumerable<object> CBelowZero
	{
		get { foreach (char a in C) if (a < 0) yield return a; }
	}

	public static IEnumerable<object> CUpTo8
	{
		get { foreach (char a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<object> CUpTo16
	{
		get { foreach (char a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<object> CUpTo32
	{
		get { foreach (char a in GetUpTo(32)) yield return a; }
	}

	#endregion C Types

	#region I1 Types

	public static IEnumerable<object> I1
	{
		get { foreach (sbyte a in Numbers.I1.Series) yield return a; }
	}

	public static IEnumerable<object> I1NotZero
	{
		get { foreach (sbyte a in I1) if (a != 0) yield return a; }
	}

	public static IEnumerable<object> I1AboveZero
	{
		get { foreach (sbyte a in I1) if (a > 0) yield return a; }
	}

	public static IEnumerable<object> I1BelowZero
	{
		get { foreach (sbyte a in I1) if (a < 0) yield return a; }
	}

	public static IEnumerable<object> I1UpTo8
	{
		get { foreach (sbyte a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<object> I1UpTo16
	{
		get { foreach (sbyte a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<object> I1UpTo32
	{
		get { foreach (sbyte a in GetUpTo(32)) yield return a; }
	}

	#endregion I1 Types

	#region U1 Types

	public static IEnumerable<object> U1
	{
		get { foreach (byte a in Numbers.U1.Series) yield return a; }
	}

	public static IEnumerable<object> U1NotZero
	{
		get { foreach (byte a in U1) if (a != 0) yield return a; }
	}

	public static IEnumerable<object> U1AboveZero
	{
		get { foreach (byte a in U1) if (a > 0) yield return a; }
	}

	public static IEnumerable<object> U1UpTo8
	{
		get { foreach (byte a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<object> U1UpTo16
	{
		get { foreach (byte a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<object> U1UpTo32
	{
		get { foreach (byte a in GetUpTo(32)) yield return a; }
	}

	public static IEnumerable<object> U1UpTo64
	{
		get { foreach (byte a in GetUpTo(32)) yield return a; }
	}

	#endregion U1 Types

	#region I2 Types

	public static IEnumerable<object> I2
	{
		get { foreach (short a in Numbers.I2.Series) yield return a; }
	}

	public static IEnumerable<object> I2NotZero
	{
		get { foreach (short a in I2) if (a != 0) yield return a; }
	}

	public static IEnumerable<object> I2AboveZero
	{
		get { foreach (short a in I2) if (a > 0) yield return a; }
	}

	public static IEnumerable<object> I2BelowZero
	{
		get { foreach (short a in I2) if (a < 0) yield return a; }
	}

	public static IEnumerable<object> I2UpTo8
	{
		get { foreach (short a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<object> I2UpTo16
	{
		get { foreach (short a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<object> I2UpTo32
	{
		get { foreach (short a in GetUpTo(32)) yield return a; }
	}

	#endregion I2 Types

	#region U2 Types

	public static IEnumerable<object> U2
	{
		get { foreach (ushort a in Numbers.U2.Series) yield return a; }
	}

	public static IEnumerable<object> U2NotZero
	{
		get { foreach (ushort a in U2) if (a != 0) yield return a; }
	}

	public static IEnumerable<object> U2AboveZero
	{
		get { foreach (ushort a in U2) if (a > 0) yield return a; }
	}

	public static IEnumerable<object> U2UpTo8
	{
		get { foreach (ushort a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<object> U2UpTo16
	{
		get { foreach (ushort a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<object> U2UpTo32
	{
		get { foreach (ushort a in GetUpTo(32)) yield return a; }
	}

	#endregion U2 Types

	#region I4 Types

	public static IEnumerable<object> I4
	{
		get { foreach (int a in Numbers.I4.Series) yield return a; }
	}

	public static IEnumerable<object> I4NotZero
	{
		get { foreach (int a in I4) if (a != 0) yield return a; }
	}

	public static IEnumerable<object> I4AboveZero
	{
		get { foreach (int a in I4) if (a > 0) yield return a; }
	}

	public static IEnumerable<object> I4BelowZero
	{
		get { foreach (int a in I4) if (a < 0) yield return a; }
	}

	public static IEnumerable<object> I4UpTo8
	{
		get { foreach (int a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<object> I4UpTo16
	{
		get { foreach (int a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<object> I4UpTo32
	{
		get { foreach (int a in GetUpTo(32)) yield return a; }
	}

	public static IEnumerable<object> I4Mini
	{
		get { yield return 0; yield return 1; yield return int.MinValue; yield return int.MaxValue; }
	}

	public static IEnumerable<object> FewScatteredAI4
	{
		get { yield return 1; yield return 51; }
	}

	public static IEnumerable<object> FewScatteredBI4
	{
		get { yield return 101; yield return 9999; }
	}

	public static IEnumerable<object> FewScatteredCI4
	{
		get { yield return 33; yield return 99; }
	}

	#endregion I4 Types

	#region U4 Types

	public static IEnumerable<object> U4
	{
		get { foreach (uint a in Numbers.U4.Series) yield return a; }
	}

	public static IEnumerable<object> U4NotZero
	{
		get { foreach (uint a in U4) if (a != 0) yield return a; }
	}

	public static IEnumerable<object> U4AboveZero
	{
		get { foreach (uint a in U4) if (a > 0) yield return a; }
	}

	public static IEnumerable<object> U4UpTo16
	{
		get { foreach (uint a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<object> U4UpTo32
	{
		get { foreach (uint a in GetUpTo(32)) yield return a; }
	}

	public static IEnumerable<object> U4UpTo8
	{
		get { foreach (uint a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<object> U4Mini
	{
		get { yield return 0; yield return 1; yield return uint.MinValue; yield return uint.MaxValue; }
	}

	#endregion U4 Types

	#region I8 Types

	public static IEnumerable<object> I8
	{
		get { foreach (long a in Numbers.I8.Series) yield return a; }
	}

	public static IEnumerable<object> I8NotZero
	{
		get { foreach (long a in I8) if (a != 0) yield return a; }
	}

	public static IEnumerable<object> I8AboveZero
	{
		get { foreach (long a in I8) if (a > 0) yield return a; }
	}

	public static IEnumerable<object> I8BelowZero
	{
		get { foreach (long a in I8) if (a < 0) yield return a; }
	}

	public static IEnumerable<object> I8UpTo8
	{
		get { foreach (long a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<object> I8UpTo16
	{
		get { foreach (long a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<object> I8UpTo32
	{
		get { foreach (long a in GetUpTo(32)) yield return a; }
	}

	public static IEnumerable<object> I8Mini
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

	public static IEnumerable<object> U8
	{
		get { foreach (ulong a in Numbers.U8.Series) yield return a; }
	}

	public static IEnumerable<object> U8NotZero
	{
		get { foreach (ulong a in U8) if (a != 0) yield return a; }
	}

	public static IEnumerable<object> U8AboveZero
	{
		get { foreach (ulong a in U8) if (a > 0) yield return a; }
	}

	public static IEnumerable<object> U8UpTo8
	{
		get { foreach (ulong a in GetUpTo(8)) yield return a; }
	}

	public static IEnumerable<object> U8UpTo16
	{
		get { foreach (ulong a in GetUpTo(16)) yield return a; }
	}

	public static IEnumerable<object> U8UpTo32
	{
		get { foreach (ulong a in GetUpTo(32)) yield return a; }
	}

	public static IEnumerable<object> U8Mini
	{
		get { yield return 0; yield return 1; yield return ulong.MinValue; yield return ulong.MaxValue; }
	}

	#endregion U8 Types

	#region R4 Types

	public static IEnumerable<float> R4
	{
		get { foreach (float a in Numbers.R4.Series) yield return a; }
	}

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
		get { foreach (float a in R4) if (!float.IsNaN(a)) yield return a; }
	}

	public static IEnumerable<float> R4Number
	{
		get { foreach (float a in R4) if (!float.IsNaN(a) && !float.IsInfinity(a)) yield return a; }
	}

	public static IEnumerable<float> R4NumberNotZero
	{
		get { foreach (float a in R8) if (a != 0) if (!float.IsNaN(a) && !float.IsInfinity(a)) yield return a; }
	}

	public static IEnumerable<float> R4NumberNoExtremes
	{
		get { foreach (float a in R4Number) if (!(float.MaxValue == a) && !(float.MinValue == a)) yield return a; }
	}

	public static IEnumerable<float> R4NumberNoExtremesOrZero
	{
		get { foreach (float a in R4NumberNoExtremes) if (a != 0) yield return a; }
	}

	public static IEnumerable<float> R4NotZero
	{
		get { foreach (float a in R4) if (a != 0) yield return a; }
	}

	public static IEnumerable<float> R4AboveZero
	{
		get { foreach (float a in R4) if (a > 0) yield return a; }
	}

	public static IEnumerable<float> R4BelowZero
	{
		get { foreach (float a in R4) if (a < 0) yield return a; }
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
		get { foreach (float a in R4) if (!float.IsNaN(a) && a <= int.MaxValue && a >= int.MinValue) yield return a; }
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

	public static IEnumerable<double> R8
	{
		get { foreach (double a in Numbers.R8.Series) yield return a; }
	}

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
		get { foreach (double a in R8) if (!double.IsNaN(a)) yield return a; }
	}

	public static IEnumerable<double> R8Number
	{
		get { foreach (double a in R8) if (!double.IsNaN(a) && !double.IsInfinity(a)) yield return a; }
	}

	public static IEnumerable<double> R8NumberNotZero
	{
		get { foreach (double a in R8) if (a != 0) if (!double.IsNaN(a) && !double.IsInfinity(a)) yield return a; }
	}

	public static IEnumerable<double> R8NumberNoExtremes
	{
		get { foreach (double a in R8Number) if (!(double.MaxValue == a) && !(double.MinValue == a)) yield return a; }
	}

	public static IEnumerable<double> R8NumberNoExtremesOrZero
	{
		get { foreach (double a in R8NumberNoExtremes) if (a != 0) yield return a; }
	}

	public static IEnumerable<double> R8NotZero
	{
		get { foreach (double a in R8) if (a != 0) yield return a; }
	}

	public static IEnumerable<double> R8AboveZero
	{
		get { foreach (double a in R8) if (a > 0) yield return a; }
	}

	public static IEnumerable<double> R8BelowZero
	{
		get { foreach (double a in R8) if (a < 0) yield return a; }
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
		get { foreach (double a in R8) if (!double.IsNaN(a) && a <= int.MaxValue && a >= int.MinValue) yield return a; }
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
