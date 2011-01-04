/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 */

using System;

namespace Mosa.Test.Collection
{

	public static class SingleTests
	{

		public static bool AddR4R4(float expected, float first, float second)
		{
			return expected == (first + second);
		}

		public static bool SubR4R4(float expected, float first, float second)
		{
			return expected == (first - second);
		}

		public static bool MulR4R4(float expected, float first, float second)
		{
			return expected == (first * second);
		}

		public static bool DivR4R4(float expected, float first, float second)
		{
			return expected == (first / second);
		}

		public static float AddR4R4(float first, float second)
		{
			return (first + second);
		}

		public static float SubR4R4(float first, float second)
		{
			return (first - second);
		}

		public static float MulR4R4(float first, float second)
		{
			return (first * second);
		}

		public static float DivR4R4(float first, float second)
		{
			return (first / second);
		}

		public static float RemR4R4(float first, float second)
		{
			return (first % second);
		}

		public static bool CeqR4R4(float first, float second)
		{
			return (first.CompareTo(second) == 0);
		}

		public static bool CltR4R4(float first, float second)
		{
			return (first.CompareTo(second) < 0);
		}

		public static bool CgtR4R4(float first, float second)
		{
			return (first.CompareTo(second) > 0);
		}

		public static bool CleR4R4(float first, float second)
		{
			return (first.CompareTo(second) <= 0);
		}

		public static bool CgeR4R4(float first, float second)
		{
			return (first.CompareTo(second) >= 0);
		}

		public static bool Newarr()
		{
			float[] arr = new float[0];
			return arr != null;
		}

		public static bool Ldlen(int length)
		{
			float[] arr = new float[length];
			return arr.Length == length;
		}

		public static bool Ldelem(int index, float value)
		{
			float[] arr = new float[index + 1];
			arr[index] = value;
			return value == arr[index];
		}

		public static bool Stelem(int index, float value)
		{
			float[] arr = new float[index + 1];
			arr[index] = value;
			return true;
		}

		public static bool Ldelema(int index, float value)
		{
			float[] arr = new float[index + 1];
			SetValueInRefValue(ref arr[index], value);
			return arr[index] == value;
		}

		private static void SetValueInRefValue(ref float destination, float value)
		{
			destination = value;
		}
	}
}