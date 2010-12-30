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
	public static class Variations
	{

		public static IEnumerable<int> GetUpTo(int end)
		{
			for (int i = 0; i < end; i++)
				yield return i;
		}

		public static IEnumerable<int> SmallNumbers
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

		public static IEnumerable<object[]> B_B
		{
			get
			{
				foreach (bool a in B.Samples)
					foreach (bool b in B.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> ISmall_B
		{
			get
			{
				foreach (int a in SmallNumbers)
					foreach (bool b in B.Samples)
						yield return new object[2] { a, b };
			}
		}

		#endregion

		#region I1 Types

		public static IEnumerable<object[]> I1_I1
		{
			get
			{
				foreach (sbyte a in I1.Samples)
					foreach (sbyte b in I1.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I1_I1WithoutZero
		{
			get
			{
				foreach (sbyte a in I1.Samples)
					foreach (sbyte b in I1.Samples)
						if (b != 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I1_I1Zero
		{
			get
			{
				foreach (sbyte a in I1.Samples)
					yield return new object[2] { a, (sbyte)0 };
			}
		}

		public static IEnumerable<object[]> I1_I1AboveZero
		{
			get
			{
				foreach (sbyte a in I1.Samples)
					foreach (sbyte b in I1.Samples)
						if (b > 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I1_I1BelowZero
		{
			get
			{
				foreach (sbyte a in I1.Samples)
					foreach (sbyte b in I1.Samples)
						if (b < 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> ISmall_I1
		{
			get
			{
				foreach (int a in SmallNumbers)
					foreach (sbyte b in I1.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I1_U1UpTo8
		{
			get
			{
				foreach (byte a in I1.Samples)
					foreach (byte b in GetUpTo(8))
						yield return new object[2] { a, b };
			}
		}

		#endregion

		#region U1 Types

		public static IEnumerable<object[]> U1_U1
		{
			get
			{
				foreach (byte a in U1.Samples)
					foreach (byte b in U1.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> U1_U1WithoutZero
		{
			get
			{
				foreach (byte a in U1.Samples)
					foreach (byte b in U1.Samples)
						if (b != 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> U1_U1Zero
		{
			get
			{
				foreach (byte a in U1.Samples)
					yield return new object[2] { a, (byte)0 };
			}
		}

		public static IEnumerable<object[]> ISmall_U1
		{
			get
			{
				foreach (int a in SmallNumbers)
					foreach (byte b in U1.Samples)
						yield return new object[2] { (int)a, b };
			}
		}

		public static IEnumerable<object[]> U1_U1UpTo8
		{
			get
			{
				foreach (byte a in U1.Samples)
					foreach (byte b in GetUpTo(8))
						yield return new object[2] { a, b };
			}
		}

		#endregion

		#region I2 Types

		public static IEnumerable<object[]> I2_I2
		{
			get
			{
				foreach (short a in I2.Samples)
					foreach (short b in I2.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I2_I2WithoutZero
		{
			get
			{
				foreach (short a in I2.Samples)
					foreach (short b in I2.Samples)
						if (b != 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I2_I2Zero
		{
			get
			{
				foreach (short a in I2.Samples)
					yield return new object[2] { a, (short)0 };
			}
		}

		public static IEnumerable<object[]> I2_I2AboveZero
		{
			get
			{
				foreach (short a in I2.Samples)
					foreach (short b in I2.Samples)
						if (b > 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I2_I2BelowZero
		{
			get
			{
				foreach (short a in I2.Samples)
					foreach (short b in I2.Samples)
						if (b < 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> ISmall_I2
		{
			get
			{
				foreach (int a in SmallNumbers)
					foreach (short b in I2.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I2_I2UpTo16
		{
			get
			{
				foreach (short a in I2.Samples)
					foreach (short b in GetUpTo(16))
						yield return new object[2] { a, b };
			}
		}

		#endregion

		#region U2 Types

		public static IEnumerable<object[]> U2_U2
		{
			get
			{
				foreach (ushort a in U2.Samples)
					foreach (ushort b in U2.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> U2_U2WithoutZero
		{
			get
			{
				foreach (ushort a in U2.Samples)
					foreach (ushort b in U2.Samples)
						if (b != 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> U2_U2Zero
		{
			get
			{
				foreach (ushort a in U2.Samples)
					yield return new object[2] { a, (ushort)0 };
			}
		}

		public static IEnumerable<object[]> ISmall_U2
		{
			get
			{
				foreach (int a in SmallNumbers)
					foreach (ushort b in U2.Samples)
						yield return new object[2] { (int)a, b };
			}
		}

		public static IEnumerable<object[]> U2_I1UpTo16
		{
			get
			{
				foreach (ushort a in I2.Samples)
					foreach (byte b in GetUpTo(16))
						yield return new object[2] { a, b };
			}
		}

		#endregion

		#region I4 Types

		public static IEnumerable<object[]> I4_I4
		{
			get
			{
				foreach (int a in I4.Samples)
					foreach (int b in I4.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I4_I4WithoutZero
		{
			get
			{
				foreach (int a in I4.Samples)
					foreach (int b in I4.Samples)
						if (b != 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I4_I4Zero
		{
			get
			{
				foreach (int a in I4.Samples)
					yield return new object[2] { a, (int)0 };
			}
		}

		public static IEnumerable<object[]> I4_I4AboveZero
		{
			get
			{
				foreach (int a in I4.Samples)
					foreach (int b in I4.Samples)
						if (b > 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I4_I4BelowZero
		{
			get
			{
				foreach (int a in I4.Samples)
					foreach (int b in I4.Samples)
						if (b < 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> ISmall_I4
		{
			get
			{
				foreach (int a in SmallNumbers)
					foreach (int b in I4.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I4_U1UpTo32
		{
			get
			{
				foreach (int a in I4.Samples)
					foreach (byte b in GetUpTo(32))
						yield return new object[2] { a, b };
			}
		}

		#endregion

		#region U4 Types

		public static IEnumerable<object[]> U4_U4
		{
			get
			{
				foreach (uint a in U4.Samples)
					foreach (uint b in U4.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> U4_U4WithoutZero
		{
			get
			{
				foreach (uint a in U4.Samples)
					foreach (uint b in U4.Samples)
						if (b != 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> U4_U4Zero
		{
			get
			{
				foreach (uint a in U4.Samples)
					yield return new object[2] { a, (uint)0 };
			}
		}

		public static IEnumerable<object[]> ISmall_U4
		{
			get
			{
				foreach (int a in SmallNumbers)
					foreach (uint b in U4.Samples)
						yield return new object[2] { (int)a, b };
			}
		}

		public static IEnumerable<object[]> U4_U1UpTo32
		{
			get
			{
				foreach (uint a in I2.Samples)
					foreach (byte b in GetUpTo(32))
						yield return new object[2] { a, b };
			}
		}

		#endregion

		#region I8 Types

		public static IEnumerable<object[]> I8_I8
		{
			get
			{
				foreach (long a in I8.Samples)
					foreach (long b in I8.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I8_I8WithoutZero
		{
			get
			{
				foreach (long a in I8.Samples)
					foreach (long b in I8.Samples)
						if (b != 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I8_I8Zero
		{
			get
			{
				foreach (long a in I8.Samples)
					yield return new object[2] { a, (long)0 };
			}
		}

		public static IEnumerable<object[]> I8_I8AboveZero
		{
			get
			{
				foreach (long a in I8.Samples)
					foreach (long b in I8.Samples)
						if (b > 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I8_I8BelowZero
		{
			get
			{
				foreach (long a in I8.Samples)
					foreach (long b in I8.Samples)
						if (b < 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> ISmall_I8
		{
			get
			{
				foreach (int a in SmallNumbers)
					foreach (long b in I8.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> I8_U1UpTo64
		{
			get
			{
				foreach (long a in I8.Samples)
					foreach (byte b in GetUpTo(64))
						yield return new object[2] { a, b };
			}
		}

		#endregion

		#region U8 Types

		public static IEnumerable<object[]> U8_U8
		{
			get
			{
				foreach (ulong a in U8.Samples)
					foreach (ulong b in U8.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> U8_U8WithoutZero
		{
			get
			{
				foreach (ulong a in U8.Samples)
					foreach (ulong b in U8.Samples)
						if (b != 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> U8_U8Zero
		{
			get
			{
				foreach (ulong a in U8.Samples)
					yield return new object[2] { a, (ulong)0 };
			}
		}

		public static IEnumerable<object[]> ISmall_U8
		{
			get
			{
				foreach (int a in SmallNumbers)
					foreach (ulong b in U8.Samples)
						yield return new object[2] { (int)a, b };
			}
		}

		public static IEnumerable<object[]> U8_U1UpTo64
		{
			get
			{
				foreach (ulong a in I8.Samples)
					foreach (byte b in GetUpTo(64))
						yield return new object[2] { a, b };
			}
		}

		#endregion

		#region C Types

		public static IEnumerable<object[]> C_C
		{
			get
			{
				foreach (char a in C.Samples)
					foreach (char b in C.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> C_CWithoutZero
		{
			get
			{
				foreach (char a in C.Samples)
					foreach (char b in C.Samples)
						if (b != 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> C_CZero
		{
			get
			{
				foreach (char a in C.Samples)
					yield return new object[2] { a, (char)0 };
			}
		}

		public static IEnumerable<object[]> ISmall_C
		{
			get
			{
				foreach (int a in SmallNumbers)
					foreach (char b in C.Samples)
						yield return new object[2] { (int)a, b };
			}
		}

		public static IEnumerable<object[]> C_CUpTo16
		{
			get
			{
				foreach (char a in I2.Samples)
					foreach (char b in GetUpTo(16))
						yield return new object[2] { a, b };
			}
		}

		#endregion

		#region R4 Types

		public static IEnumerable<object[]> R4_R4
		{
			get
			{
				foreach (float a in R4.Samples)
					foreach (float b in R4.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> R4_R4WithoutZero
		{
			get
			{
				foreach (float a in R4.Samples)
					foreach (float b in R4.Samples)
						if (b != 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> R4_R4Zero
		{
			get
			{
				foreach (float a in R4.Samples)
					yield return new object[2] { a, (float)0 };
			}
		}

		public static IEnumerable<object[]> ISmall_R4
		{
			get
			{
				foreach (int a in SmallNumbers)
					foreach (float b in R4.Samples)
						yield return new object[2] { (int)a, b };
			}
		}

		#endregion

		#region R8 Types

		public static IEnumerable<object[]> R8_R8
		{
			get
			{
				foreach (double a in R8.Samples)
					foreach (double b in R8.Samples)
						yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> R8_R8WithoutZero
		{
			get
			{
				foreach (double a in R8.Samples)
					foreach (double b in R8.Samples)
						if (b != 0)
							yield return new object[2] { a, b };
			}
		}

		public static IEnumerable<object[]> R8_R8Zero
		{
			get
			{
				foreach (double a in R8.Samples)
					yield return new object[2] { a, (double)0 };
			}
		}

		public static IEnumerable<object[]> ISmall_R8
		{
			get
			{
				foreach (int a in SmallNumbers)
					foreach (double b in R8.Samples)
						yield return new object[2] { (int)a, b };
			}
		}

		#endregion
	}
}
