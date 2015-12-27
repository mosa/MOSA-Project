// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

		public static IEnumerable<object[]> CCC
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.C.Series)
					foreach (var i2 in Mosa.Test.Numbers.C.Series)
						foreach (var i3 in Mosa.Test.Numbers.C.Series)
							yield return new object[] { i1, i2, i3 };
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

		public static IEnumerable<object[]> I1I1I1
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I1.Series)
					foreach (var i2 in Mosa.Test.Numbers.I1.Series)
						foreach (var i3 in Mosa.Test.Numbers.I1.Series)
							yield return new object[] { i1, i2, i3 };
			}
		}

		public static IEnumerable<object[]> I1U1UpTo16
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I1)
					foreach (var i2 in Mosa.Test.Numbers.Series.U1UpTo16)
						yield return new object[] { i1, i2 };
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

		public static IEnumerable<object[]> I2I2I2
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I2.Series)
					foreach (var i2 in Mosa.Test.Numbers.I2.Series)
						foreach (var i3 in Mosa.Test.Numbers.I2.Series)
							yield return new object[] { i1, i2, i3 };
			}
		}

		public static IEnumerable<object[]> I2U1UpTo16
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I2.Series)
					foreach (var i2 in Mosa.Test.Numbers.Series.U1UpTo16)
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

		public static IEnumerable<object[]> I4SmallB
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I4Small)
					foreach (var i2 in Mosa.Test.Numbers.B.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallC
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I4Small)
					foreach (var i2 in Mosa.Test.Numbers.C.Series)
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

		public static IEnumerable<object[]> I4SmallI1
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I4Small)
					foreach (var i2 in Mosa.Test.Numbers.I1.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallI2
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I4Small)
					foreach (var i2 in Mosa.Test.Numbers.I2.Series)
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

		public static IEnumerable<object[]> I4SmallI8
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I4Small)
					foreach (var i2 in Mosa.Test.Numbers.I8.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallU1
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I4Small)
					foreach (var i2 in Mosa.Test.Numbers.U1.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallU2
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I4Small)
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

		public static IEnumerable<object[]> I4SmallU8
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I4Small)
					foreach (var i2 in Mosa.Test.Numbers.U8.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallR4Simple
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I4Small)
					foreach (var i2 in Mosa.Test.Numbers.Series.R4Simple)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallR8Simple
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I4Small)
					foreach (var i2 in Mosa.Test.Numbers.Series.R8Simple)
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

		public static IEnumerable<object[]> I4U1UpTo32
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I4.Series)
					foreach (var i2 in Mosa.Test.Numbers.Series.U1UpTo32)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4I4I4
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I4.Series)
					foreach (var i2 in Mosa.Test.Numbers.I4.Series)
						foreach (var i3 in Mosa.Test.Numbers.I4.Series)
							yield return new object[] { i1, i2, i3 };
			}
		}

		public static IEnumerable<object[]> I4I4I4I4
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I4.Series)
					foreach (var i2 in Mosa.Test.Numbers.I4.Series)
						foreach (var i3 in Mosa.Test.Numbers.I4.Series)
							foreach (var i4 in Mosa.Test.Numbers.I4.Series)
								yield return new object[] { i1, i2, i3, i4 };
			}
		}

		public static IEnumerable<object[]> I4MiniI4MiniI4Mini
		{
			get
			{
				foreach (var i1 in Series.I4Mini)
					foreach (var i2 in Series.I4Mini)
						foreach (var i3 in Series.I4Mini)
							yield return new object[] { i1, i2, i3 };
			}
		}

		public static IEnumerable<object[]> I4MiniI4MiniI4MiniI4Mini
		{
			get
			{
				foreach (var i1 in Series.I4Mini)
					foreach (var i2 in Series.I4Mini)
						foreach (var i3 in Series.I4Mini)
							foreach (var i4 in Series.I4Mini)
								yield return new object[] { i1, i2, i3, i4 };
			}
		}

		public static IEnumerable<object[]> I4SmallI4SmallI4SmallI4SmallI4SmallI4SmallI4Small
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I4Small)
					foreach (var i2 in Mosa.Test.Numbers.Series.I4Small)
						foreach (var i3 in Mosa.Test.Numbers.Series.I4Small)
							foreach (var i4 in Mosa.Test.Numbers.Series.I4Small)
								foreach (var i5 in Mosa.Test.Numbers.Series.I4Small)
									foreach (var i6 in Mosa.Test.Numbers.Series.I4Small)
										foreach (var i7 in Mosa.Test.Numbers.Series.I4Small)
											yield return new object[] { i1, i2, i3, i4, i5, i6, i7 };
			}
		}

		public static IEnumerable<object[]> I4MiniI4MiniI4MiniI4MiniI4MiniI4MiniI4Mini
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.I4Mini)
					foreach (var i2 in Mosa.Test.Numbers.Series.I4Mini)
						foreach (var i3 in Mosa.Test.Numbers.Series.I4Mini)
							foreach (var i4 in Mosa.Test.Numbers.Series.I4Mini)
								foreach (var i5 in Mosa.Test.Numbers.Series.I4Mini)
									foreach (var i6 in Mosa.Test.Numbers.Series.I4Mini)
										foreach (var i7 in Mosa.Test.Numbers.Series.I4Mini)
											yield return new object[] { i1, i2, i3, i4, i5, i6, i7 };
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

		public static IEnumerable<object[]> I8MiniI8MiniI8Mini
		{
			get
			{
				foreach (var i1 in Series.I8Mini)
					foreach (var i2 in Series.I8Mini)
						foreach (var i3 in Series.I8Mini)
							yield return new object[] { i1, i2, i3 };
			}
		}

		public static IEnumerable<object[]> I8I8I8I8
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I8.Series)
					foreach (var i2 in Mosa.Test.Numbers.I8.Series)
						foreach (var i3 in Mosa.Test.Numbers.I8.Series)
							foreach (var i4 in Mosa.Test.Numbers.I8.Series)
								yield return new object[] { i1, i2, i3, i4 };
			}
		}

		public static IEnumerable<object[]> I8MiniI8MiniI8MiniI8Mini
		{
			get
			{
				foreach (var i1 in Series.I8Mini)
					foreach (var i2 in Series.I8Mini)
						foreach (var i3 in Series.I8Mini)
							foreach (var i4 in Series.I8Mini)
								yield return new object[] { i1, i2, i3, i4 };
			}
		}

		public static IEnumerable<object[]> I8U1UpTo32
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.I8.Series)
					foreach (var i2 in Mosa.Test.Numbers.Series.U1UpTo32)
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

		public static IEnumerable<object[]> U1U1U1
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.U1.Series)
					foreach (var i2 in Mosa.Test.Numbers.U1.Series)
						foreach (var i3 in Mosa.Test.Numbers.U1.Series)
							yield return new object[] { i1, i2, i3 };
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

		public static IEnumerable<object[]> U2U2U2
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.U2.Series)
					foreach (var i2 in Mosa.Test.Numbers.U2.Series)
						foreach (var i3 in Mosa.Test.Numbers.U2.Series)
							yield return new object[] { i1, i2, i3 };
			}
		}

		public static IEnumerable<object[]> U1U1UpTo16
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.U1.Series)
					foreach (var i2 in Mosa.Test.Numbers.Series.U1UpTo16)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> U2U1UpTo16
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.U2.Series)
					foreach (var i2 in Mosa.Test.Numbers.Series.U1UpTo16)
						yield return new object[] { i1, i2 };
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

		public static IEnumerable<object[]> U4MiniU4MiniU4Mini
		{
			get
			{
				foreach (var i1 in Series.U4Mini)
					foreach (var i2 in Series.U4Mini)
						foreach (var i3 in Series.U4Mini)
							yield return new object[] { i1, i2, i3 };
			}
		}

		public static IEnumerable<object[]> U4U8U8U8
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.U4.Series)
					foreach (var i2 in Mosa.Test.Numbers.U8.Series)
						foreach (var i3 in Mosa.Test.Numbers.U8.Series)
							foreach (var i4 in Mosa.Test.Numbers.U8.Series)
								yield return new object[] { i1, i2, i3, i4 };
			}
		}

		public static IEnumerable<object[]> U4MiniU8MiniU8MiniU8Mini
		{
			get
			{
				foreach (var i1 in Series.U4Mini)
					foreach (var i2 in Series.U8Mini)
						foreach (var i3 in Series.U8Mini)
							foreach (var i4 in Series.U8Mini)
								yield return new object[] { i1, i2, i3, i4 };
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

		public static IEnumerable<object[]> U4U1UpTo32
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.U4.Series)
					foreach (var i2 in Mosa.Test.Numbers.Series.U1UpTo32)
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

		public static IEnumerable<object[]> U8U8U8U8
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.U8.Series)
					foreach (var i2 in Mosa.Test.Numbers.U8.Series)
						foreach (var i3 in Mosa.Test.Numbers.U8.Series)
							foreach (var i4 in Mosa.Test.Numbers.U8.Series)
								yield return new object[] { i1, i2, i3, i4 };
			}
		}

		public static IEnumerable<object[]> U8MiniU8MiniU8Mini
		{
			get
			{
				foreach (var i1 in Series.U8Mini)
					foreach (var i2 in Series.U8Mini)
						foreach (var i3 in Series.U8Mini)
							yield return new object[] { i1, i2, i3 };
			}
		}

		public static IEnumerable<object[]> U8MiniU8MiniU8MiniU8Mini
		{
			get
			{
				foreach (var i1 in Series.U8Mini)
					foreach (var i2 in Series.U8Mini)
						foreach (var i3 in Series.U8Mini)
							foreach (var U8 in Series.U8Mini)
								yield return new object[] { i1, i2, i3, U8 };
			}
		}

		public static IEnumerable<object[]> U8U1UpTo32
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.U8.Series)
					foreach (var i2 in Mosa.Test.Numbers.Series.U1UpTo32)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> R4
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.R4.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> R4NotNaN
		{
			get
			{
				foreach (var i1 in Series.R4NotNaN)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> R4R4
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.R4.Series)
					foreach (var i2 in Mosa.Test.Numbers.R4.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> R4MiniR4MiniR4Mini
		{
			get
			{
				foreach (var i1 in Series.R4Mini)
					foreach (var i2 in Series.R4Mini)
						foreach (var i3 in Series.R4Mini)
							yield return new object[] { i1, i2, i3 };
			}
		}

		public static IEnumerable<object[]> R4SimpleR4Simple
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.R4Simple)
					foreach (var i2 in Mosa.Test.Numbers.Series.R4Simple)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> R8
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.R8.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> R8NotNaN
		{
			get
			{
				foreach (var i1 in Series.R8NotNaN)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> R8R8
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.R8.Series)
					foreach (var i2 in Mosa.Test.Numbers.R8.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> R8SimpleR8Simple
		{
			get
			{
				foreach (var i1 in Mosa.Test.Numbers.Series.R8Simple)
					foreach (var i2 in Mosa.Test.Numbers.Series.R8Simple)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> R8MiniR8MiniR8Mini
		{
			get
			{
				foreach (var i1 in Series.R8Mini)
					foreach (var i2 in Series.R8Mini)
						foreach (var i3 in Series.R8Mini)
							yield return new object[] { i1, i2, i3 };
			}
		}
	}
}
