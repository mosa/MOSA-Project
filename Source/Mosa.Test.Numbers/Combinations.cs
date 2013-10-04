/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System.Collections.Generic;

namespace Mosa.Test.Numbers
{
	public static class Combinations
	{
		public static IEnumerable<object[]> B
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.B.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> BB
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.B.Series)
					foreach (var i2 in Mosa.Test.Numbers.B.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> C
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.C.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> CC
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.C.Series)
					foreach (var i2 in Mosa.Test.Numbers.C.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallB
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I4Small)
					foreach (var i2 in Mosa.Test.Numbers.B.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I4.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> I4I4
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I4.Series)
					foreach (var i2 in Mosa.Test.Numbers.I4.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I2
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I2.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> I2I2
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I2.Series)
					foreach (var i2 in Mosa.Test.Numbers.I2.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I1
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I1.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> I1I1
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I1.Series)
					foreach (var i2 in Mosa.Test.Numbers.I1.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I8
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I8.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> I8I8
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I8.Series)
					foreach (var i2 in Mosa.Test.Numbers.I8.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallI4
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I4Small)
					foreach (var i2 in Mosa.Test.Numbers.I4.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4Small
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I4Small)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> I1UpTo32
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I1UpTo32)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> U4
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.U4.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> U4U4
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.U4.Series)
					foreach (var i2 in Mosa.Test.Numbers.U4.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> U2
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.U2.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> U2U2
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.U2.Series)
					foreach (var i2 in Mosa.Test.Numbers.U2.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallU4
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I4Small)
					foreach (var i2 in Mosa.Test.Numbers.U4.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> U1
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.U1.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> U1U1
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.U1.Series)
					foreach (var i2 in Mosa.Test.Numbers.U1.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> U8
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.U8.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> U8U8
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.U8.Series)
					foreach (var i2 in Mosa.Test.Numbers.U8.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> U4I1UpTo32
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.U4.Series)
					foreach (var i2 in Mosa.Test.Numbers.Series.I1UpTo32)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4I1UpTo32
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I4.Series)
					foreach (var i2 in Mosa.Test.Numbers.Series.I1UpTo32)
						yield return new object[] { i1, i2 };
			}
		}


		public static IEnumerable<object[]> U4U1UpTo32
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.U4.Series)
					foreach (var i2 in Mosa.Test.Numbers.Series.U1UpTo32)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4U1UpTo32
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I4.Series)
					foreach (var i2 in Mosa.Test.Numbers.Series.U1UpTo32)
						yield return new object[] { i1, i2 };
			}
		}
	}
}