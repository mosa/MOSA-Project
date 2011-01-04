/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.Collections.Generic;
using System.Text;

using MbUnit.Framework;

namespace Mosa.Test.Runtime.CompilerFramework.Numbers
{
	public static class Series
	{

		public static IEnumerable<int> GetUpTo(int end)
		{
			for (int i = 0; i < end; i++)
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

		#region B Types

		#endregion

		#region C Types

		public static IEnumerable<char> CNotZero
		{
			get { foreach (char a in C.Series) if (a != 0) yield return a; }
		}

		public static IEnumerable<char> CAboveZero
		{
			get { foreach (char a in C.Series) if (a > 0) yield return a; }
		}

		public static IEnumerable<char> CBelowZero
		{
			get { foreach (char a in C.Series) if (a < 0) yield return a; }
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

		#endregion

		#region I1 Types

		public static IEnumerable<sbyte> I1NotZero
		{
			get { foreach (sbyte a in I1.Series) if (a != 0) yield return a; }
		}

		public static IEnumerable<sbyte> I1AboveZero
		{
			get { foreach (sbyte a in I1.Series) if (a > 0) yield return a; }
		}

		public static IEnumerable<sbyte> I1BelowZero
		{
			get { foreach (sbyte a in I1.Series) if (a < 0) yield return a; }
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

		#endregion

		#region U1 Types

		public static IEnumerable<byte> U1NotZero
		{
			get { foreach (byte a in U1.Series) if (a != 0) yield return a; }
		}

		public static IEnumerable<byte> U1AboveZero
		{
			get { foreach (byte a in U1.Series) if (a > 0) yield return a; }
		}

		public static IEnumerable<byte> U1BelowZero
		{
			get { foreach (byte a in U1.Series) if (a < 0) yield return a; }
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

		#endregion

		#region I2 Types

		public static IEnumerable<short> I2NotZero
		{
			get { foreach (short a in I2.Series) if (a != 0) yield return a; }
		}

		public static IEnumerable<short> I2AboveZero
		{
			get { foreach (short a in I2.Series) if (a > 0) yield return a; }
		}

		public static IEnumerable<short> I2BelowZero
		{
			get { foreach (short a in I2.Series) if (a < 0) yield return a; }
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

		#endregion

		#region U2 Types

		public static IEnumerable<ushort> U2NotZero
		{
			get { foreach (ushort a in U2.Series) if (a != 0) yield return a; }
		}

		public static IEnumerable<ushort> U2AboveZero
		{
			get { foreach (ushort a in U2.Series) if (a > 0) yield return a; }
		}

		public static IEnumerable<ushort> U2BelowZero
		{
			get { foreach (ushort a in U2.Series) if (a < 0) yield return a; }
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

		#endregion

		#region I4 Types

		public static IEnumerable<int> I4NotZero
		{
			get { foreach (int a in I4.Series) if (a != 0) yield return a; }
		}

		public static IEnumerable<int> I4AboveZero
		{
			get { foreach (int a in I4.Series) if (a > 0) yield return a; }
		}

		public static IEnumerable<int> I4BelowZero
		{
			get { foreach (int a in I4.Series) if (a < 0) yield return a; }
		}

		public static IEnumerable<int> I4UpTo8
		{
			get { foreach (int a in GetUpTo(8)) yield return a; }
		}

		public static IEnumerable<int> I4UpTo16
		{
			get { foreach (int a in GetUpTo(16)) yield return a; }
		}

		public static IEnumerable<int> I4UpTo32
		{
			get { foreach (int a in GetUpTo(32)) yield return a; }
		}

		#endregion

		#region U4 Types

		public static IEnumerable<uint> U4NotZero
		{
			get { foreach (uint a in U4.Series) if (a != 0) yield return a; }
		}

		public static IEnumerable<uint> U4AboveZero
		{
			get { foreach (uint a in U4.Series) if (a > 0) yield return a; }
		}

		public static IEnumerable<uint> U4BelowZero
		{
			get { foreach (uint a in U4.Series) if (a < 0) yield return a; }
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
		#endregion

		#region I8 Types

		public static IEnumerable<long> I8NotZero
		{
			get { foreach (long a in I8.Series) if (a != 0) yield return a; }
		}

		public static IEnumerable<long> I8AboveZero
		{
			get { foreach (long a in I8.Series) if (a > 0) yield return a; }
		}

		public static IEnumerable<long> I8BelowZero
		{
			get { foreach (long a in I8.Series) if (a < 0) yield return a; }
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

		#endregion

		#region U8 Types

		public static IEnumerable<ulong> U8NotZero
		{
			get { foreach (ulong a in U8.Series) if (a != 0) yield return a; }
		}

		public static IEnumerable<ulong> U8AboveZero
		{
			get { foreach (ulong a in U8.Series) if (a > 0) yield return a; }
		}

		public static IEnumerable<ulong> U8BelowZero
		{
			get { foreach (ulong a in U8.Series) if (a < 0) yield return a; }
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
		#endregion

		#region R4 Types

		public static IEnumerable<float> R4NotZero
		{
			get { foreach (float a in R4.Series) if (a != 0) yield return a; }
		}

		public static IEnumerable<float> R4AboveZero
		{
			get { foreach (float a in R4.Series) if (a > 0) yield return a; }
		}

		public static IEnumerable<float> R4BelowZero
		{
			get { foreach (float a in R4.Series) if (a < 0) yield return a; }
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

		#endregion

		#region R8 Types

		public static IEnumerable<double> R8NotZero
		{
			get { foreach (double a in R8.Series) if (a != 0) yield return a; }
		}

		public static IEnumerable<double> R8AboveZero
		{
			get { foreach (double a in R8.Series) if (a > 0) yield return a; }
		}

		public static IEnumerable<double> R8BelowZero
		{
			get { foreach (double a in R8.Series) if (a < 0) yield return a; }
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

		#endregion
	}
}
