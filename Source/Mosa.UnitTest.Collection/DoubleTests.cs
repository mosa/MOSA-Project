// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTest.Collection
{
	public static class DoubleTests
	{
		public static double AddR8R8(double first, double second)
		{
			return (first + second);
		}

		public static double SubR8R8(double first, double second)
		{
			return (first - second);
		}

		public static double MulR8R8(double first, double second)
		{
			return (first * second);
		}

		public static double DivR8R8(double first, double second)
		{
			return (first / second);
		}

		public static double RemR8R8(double first, double second)
		{
			return (first % second);
		}

		public static bool CeqR8R8(double first, double second)
		{
			return (first.CompareTo(second) == 0);
		}

		public static bool CneqR8R8(double first, double second)
		{
			return (first.CompareTo(second) != 0);
		}

		public static bool CltR8R8(double first, double second)
		{
			return (first.CompareTo(second) < 0);
		}

		public static bool CgtR8R8(double first, double second)
		{
			return (first.CompareTo(second) > 0);
		}

		public static bool CleR8R8(double first, double second)
		{
			return (first.CompareTo(second) <= 0);
		}

		public static bool CgeR8R8(double first, double second)
		{
			return (first.CompareTo(second) >= 0);
		}

		public static bool Newarr()
		{
			double[] arr = new double[0];
			return arr != null;
		}

		public static bool Ldlen(int length)
		{
			double[] arr = new double[length];
			return arr.Length == length;
		}

		public static bool Ldelem(int index, double value)
		{
			double[] arr = new double[index + 1];
			arr[index] = value;
			return value == arr[index];
		}

		public static bool Stelem(int index, double value)
		{
			double[] arr = new double[index + 1];
			arr[index] = value;
			return true;
		}

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

		public static bool IsNaN(double value)
		{
			return Double.IsNaN(value);
		}
	}
}
