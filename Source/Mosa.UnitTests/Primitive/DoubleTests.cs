// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTests.Primitive
{
	public static class DoubleTests

	{
		[MosaUnitTest(Series = "R8R8")]
		public static double AddR8R8(double first, double second)
		{
			return first + second;
		}

		[MosaUnitTest(Series = "R8R8")]
		public static double SubR8R8(double first, double second)
		{
			return first - second;
		}

		[MosaUnitTest(Series = "R8R8")]
		public static double MulR8R8(double first, double second)
		{
			return first * second;
		}

		[MosaUnitTest(Series = "R8R8NoZero")]
		public static double DivR8R8(double first, double second)
		{
			return first / second;
		}

		//[MosaUnitTest(Series = "R8R8NoZero")]
		public static double RemR8R8(double first, double second)
		{
			return first % second;
		}

		[MosaUnitTest(Series = "R8R8")]
		public static bool CeqR8R8(double first, double second)
		{
			return first.CompareTo(second) == 0;
		}

		[MosaUnitTest(Series = "R8R8")]
		public static bool CneqR8R8(double first, double second)
		{
			return first.CompareTo(second) != 0;
		}

		[MosaUnitTest(Series = "R8R8")]
		public static bool CltR8R8(double first, double second)
		{
			return first.CompareTo(second) < 0;
		}

		[MosaUnitTest(Series = "R8R8")]
		public static bool CgtR8R8(double first, double second)
		{
			return first.CompareTo(second) > 0;
		}

		[MosaUnitTest(Series = "R8R8")]
		public static bool CleR8R8(double first, double second)
		{
			return first.CompareTo(second) <= 0;
		}

		[MosaUnitTest(Series = "R8R8")]
		public static bool CgeR8R8(double first, double second)
		{
			return first.CompareTo(second) >= 0;
		}

		[MosaUnitTest]
		public static bool Newarr()
		{
			double[] arr = new double[0];
			return arr != null;
		}

		[MosaUnitTest(Series = "I4Small")]
		public static bool Ldlen(int length)
		{
			double[] arr = new double[length];
			return arr.Length == length;
		}

		[MosaUnitTest(Series = "I4SmallR8Simple")]
		public static bool Ldelem(int index, double value)
		{
			double[] arr = new double[index + 1];
			arr[index] = value;
			return value == arr[index];
		}

		[MosaUnitTest(Series = "I4SmallR8Simple")]
		public static bool Stelem(int index, double value)
		{
			double[] arr = new double[index + 1];
			arr[index] = value;
			return true;
		}

		[MosaUnitTest(Series = "I4SmallR8Simple")]
		public static bool Ldelema(int index, double value)
		{
			double[] arr = new double[index + 1];
			SetValueInRefValue(ref arr[index], value);
			return arr[index] == value;
		}

		private static void SetValueInRefValue(ref double destination, double value)
		{
			destination = value;
		}

		[MosaUnitTest(Series = "R8")]
		public static bool IsNaN(double value)
		{
			return Double.IsNaN(value);
		}
	}
}
