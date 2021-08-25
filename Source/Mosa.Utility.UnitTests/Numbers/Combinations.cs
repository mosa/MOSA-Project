﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Utility.UnitTests.Numbers
{
	public static class Combinations
	{
		public static IEnumerable<object[]> B
		{
			get
			{
				foreach (var i1 in Numbers.B.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> BB
		{
			get
			{
				foreach (var i1 in Numbers.B.Series)
					foreach (var i2 in Numbers.B.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> C
		{
			get
			{
				foreach (var i1 in Numbers.C.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> CC
		{
			get
			{
				foreach (var i1 in Numbers.C.Series)
					foreach (var i2 in Numbers.C.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> CCC
		{
			get
			{
				foreach (var i1 in Numbers.C.Series)
					foreach (var i2 in Numbers.C.Series)
						foreach (var i3 in Numbers.C.Series)
							yield return new object[] { i1, i2, i3 };
			}
		}

		public static IEnumerable<object[]> I1
		{
			get
			{
				foreach (var i1 in Numbers.I1.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> I1I1
		{
			get
			{
				foreach (var i1 in Numbers.I1.Series)
					foreach (var i2 in Numbers.I1.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I1I1I1
		{
			get
			{
				foreach (var i1 in Numbers.I1.Series)
					foreach (var i2 in Numbers.I1.Series)
						foreach (var i3 in Numbers.I1.Series)
							yield return new object[] { i1, i2, i3 };
			}
		}

		public static IEnumerable<object[]> I1U1UpTo16
		{
			get
			{
				foreach (var i1 in Series.I1)
					foreach (var i2 in Series.U1UpTo16)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I1UpTo32
		{
			get
			{
				foreach (var i1 in Series.I1UpTo32)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> I2
		{
			get
			{
				foreach (var i1 in Numbers.I2.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> I2I2
		{
			get
			{
				foreach (var i1 in Numbers.I2.Series)
					foreach (var i2 in Numbers.I2.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I2I2I2
		{
			get
			{
				foreach (var i1 in Numbers.I2.Series)
					foreach (var i2 in Numbers.I2.Series)
						foreach (var i3 in Numbers.I2.Series)
							yield return new object[] { i1, i2, i3 };
			}
		}

		public static IEnumerable<object[]> I2U1UpTo16
		{
			get
			{
				foreach (var i1 in Numbers.I2.Series)
					foreach (var i2 in Series.U1UpTo16)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4
		{
			get
			{
				foreach (var i1 in Numbers.I4.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> I4I4
		{
			get
			{
				foreach (var i1 in Numbers.I4.Series)
					foreach (var i2 in Numbers.I4.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4I1UpTo32
		{
			get
			{
				foreach (var i1 in Numbers.I4.Series)
					foreach (var i2 in Series.I1UpTo32)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4NotZeroI4
		{
			get
			{
				foreach (var i1 in Numbers.I4.Series)
					foreach (var i2 in Numbers.I4.Series)
						if (i2 != 0)
							yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallB
		{
			get
			{
				foreach (var i1 in Series.I4Small)
					foreach (var i2 in Numbers.B.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallC
		{
			get
			{
				foreach (var i1 in Series.I4Small)
					foreach (var i2 in Numbers.C.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4Small
		{
			get
			{
				foreach (var i1 in Series.I4Small)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> I4SmallI1
		{
			get
			{
				foreach (var i1 in Series.I4Small)
					foreach (var i2 in Numbers.I1.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallI2
		{
			get
			{
				foreach (var i1 in Series.I4Small)
					foreach (var i2 in Numbers.I2.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallI4
		{
			get
			{
				foreach (var i1 in Series.I4Small)
					foreach (var i2 in Numbers.I4.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallI8
		{
			get
			{
				foreach (var i1 in Series.I4Small)
					foreach (var i2 in Numbers.I8.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallU1
		{
			get
			{
				foreach (var i1 in Series.I4Small)
					foreach (var i2 in Numbers.U1.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallU2
		{
			get
			{
				foreach (var i1 in Series.I4Small)
					foreach (var i2 in Numbers.U2.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallU4
		{
			get
			{
				foreach (var i1 in Series.I4Small)
					foreach (var i2 in Numbers.U4.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallU8
		{
			get
			{
				foreach (var i1 in Series.I4Small)
					foreach (var i2 in Numbers.U8.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallR4Simple
		{
			get
			{
				foreach (var i1 in Series.I4Small)
					foreach (var i2 in Series.R4Simple)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4SmallR8Simple
		{
			get
			{
				foreach (var i1 in Series.I4Small)
					foreach (var i2 in Series.R8Simple)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4U1UpTo32
		{
			get
			{
				foreach (var i1 in Numbers.I4.Series)
					foreach (var i2 in Series.U1UpTo32)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I4I4I4
		{
			get
			{
				foreach (var i1 in Numbers.I4.Series)
					foreach (var i2 in Numbers.I4.Series)
						foreach (var i3 in Numbers.I4.Series)
							yield return new object[] { i1, i2, i3 };
			}
		}

		public static IEnumerable<object[]> I4I4I4I4
		{
			get
			{
				foreach (var i1 in Numbers.I4.Series)
					foreach (var i2 in Numbers.I4.Series)
						foreach (var i3 in Numbers.I4.Series)
							foreach (var i4 in Numbers.I4.Series)
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
				foreach (var i1 in Series.I4Small)
					foreach (var i2 in Series.I4Small)
						foreach (var i3 in Series.I4Small)
							foreach (var i4 in Series.I4Small)
								foreach (var i5 in Series.I4Small)
									foreach (var i6 in Series.I4Small)
										foreach (var i7 in Series.I4Small)
											yield return new object[] { i1, i2, i3, i4, i5, i6, i7 };
			}
		}

		public static IEnumerable<object[]> I4MiniI4MiniI4MiniI4MiniI4MiniI4MiniI4Mini
		{
			get
			{
				foreach (var i1 in Series.I4Mini)
					foreach (var i2 in Series.I4Mini)
						foreach (var i3 in Series.I4Mini)
							foreach (var i4 in Series.I4Mini)
								foreach (var i5 in Series.I4Mini)
									foreach (var i6 in Series.I4Mini)
										foreach (var i7 in Series.I4Mini)
											yield return new object[] { i1, i2, i3, i4, i5, i6, i7 };
			}
		}

		public static IEnumerable<object[]> I8
		{
			get
			{
				foreach (var i1 in Numbers.I8.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> I8I8
		{
			get
			{
				foreach (var i1 in Numbers.I8.Series)
					foreach (var i2 in Numbers.I8.Series)
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
				foreach (var i1 in Numbers.I8.Series)
					foreach (var i2 in Numbers.I8.Series)
						foreach (var i3 in Numbers.I8.Series)
							foreach (var i4 in Numbers.I8.Series)
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
				foreach (var i1 in Numbers.I8.Series)
					foreach (var i2 in Series.U1UpTo32)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> I8U1UpTo64
		{
			get
			{
				foreach (var i1 in Numbers.I8.Series)
					foreach (var i2 in Series.U1UpTo64)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> U1
		{
			get
			{
				foreach (var i1 in Numbers.U1.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> U1U1
		{
			get
			{
				foreach (var i1 in Numbers.U1.Series)
					foreach (var i2 in Numbers.U1.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> U1U1U1
		{
			get
			{
				foreach (var i1 in Numbers.U1.Series)
					foreach (var i2 in Numbers.U1.Series)
						foreach (var i3 in Numbers.U1.Series)
							yield return new object[] { i1, i2, i3 };
			}
		}

		public static IEnumerable<object[]> U2
		{
			get
			{
				foreach (var i1 in Numbers.U2.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> U2U2
		{
			get
			{
				foreach (var i1 in Numbers.U2.Series)
					foreach (var i2 in Numbers.U2.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> U2U2U2
		{
			get
			{
				foreach (var i1 in Numbers.U2.Series)
					foreach (var i2 in Numbers.U2.Series)
						foreach (var i3 in Numbers.U2.Series)
							yield return new object[] { i1, i2, i3 };
			}
		}

		public static IEnumerable<object[]> U1U1UpTo16
		{
			get
			{
				foreach (var i1 in Numbers.U1.Series)
					foreach (var i2 in Series.U1UpTo16)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> U2U1UpTo16
		{
			get
			{
				foreach (var i1 in Numbers.U2.Series)
					foreach (var i2 in Series.U1UpTo16)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> U4
		{
			get
			{
				foreach (var i1 in Numbers.U4.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> U4U4
		{
			get
			{
				foreach (var i1 in Numbers.U4.Series)
					foreach (var i2 in Numbers.U4.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> U4I1UpTo32
		{
			get
			{
				foreach (var i1 in Numbers.U4.Series)
					foreach (var i2 in Series.I1UpTo32)
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
				foreach (var i1 in Numbers.U4.Series)
					foreach (var i2 in Numbers.U8.Series)
						foreach (var i3 in Numbers.U8.Series)
							foreach (var i4 in Numbers.U8.Series)
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

		public static IEnumerable<object[]> U4U1UpTo32
		{
			get
			{
				foreach (var i1 in Numbers.U4.Series)
					foreach (var i2 in Series.U1UpTo32)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> U8
		{
			get
			{
				foreach (var i1 in Numbers.U8.Series)
					yield return new object[] { i1 };
			}
		}

		public static IEnumerable<object[]> U8U8
		{
			get
			{
				foreach (var i1 in Numbers.U8.Series)
					foreach (var i2 in Numbers.U8.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> U8U8U8U8
		{
			get
			{
				foreach (var i1 in Numbers.U8.Series)
					foreach (var i2 in Numbers.U8.Series)
						foreach (var i3 in Numbers.U8.Series)
							foreach (var i4 in Numbers.U8.Series)
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
				foreach (var i1 in Numbers.U8.Series)
					foreach (var i2 in Series.U1UpTo32)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> U8U1UpTo64
		{
			get
			{
				foreach (var i1 in Numbers.U8.Series)
					foreach (var i2 in Series.U1UpTo64)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> R4
		{
			get
			{
				foreach (var i1 in Numbers.R4.Series)
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

		public static IEnumerable<object[]> R4R4NoZero
		{
			get
			{
				foreach (var i1 in Series.R4)
					foreach (var i2 in Numbers.R4.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> R4R4
		{
			get
			{
				foreach (var i1 in Numbers.R4.Series)
					foreach (var i2 in Numbers.R4.Series)
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
				foreach (var i1 in Series.R4Simple)
					foreach (var i2 in Series.R4Simple)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> R8
		{
			get
			{
				foreach (var i1 in Numbers.R8.Series)
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

		public static IEnumerable<object[]> R8R8NoZero
		{
			get
			{
				foreach (var i1 in Series.R8)
					foreach (var i2 in Numbers.R8.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> R8R8
		{
			get
			{
				foreach (var i1 in Numbers.R8.Series)
					foreach (var i2 in Numbers.R8.Series)
						yield return new object[] { i1, i2 };
			}
		}

		public static IEnumerable<object[]> R8SimpleR8Simple
		{
			get
			{
				foreach (var i1 in Series.R8Simple)
					foreach (var i2 in Series.R8Simple)
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
