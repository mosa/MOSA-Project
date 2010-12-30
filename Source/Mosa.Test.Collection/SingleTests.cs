/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com> 
 */

namespace Mosa.Test.Collection
{

	public static class SingleTests
	{

		public static float AddU8U8(float first, float second)
		{
			return (first + second);
		}

		public static float SubU8U8(float first, float second)
		{
			return (first - second);
		}

		public static float MulU8U8(float first, float second)
		{
			return (first * second);
		}

		public static float DivU8U8(float first, float second)
		{
			return (first / second);
		}

		public static float RemU8U8(float first, float second)
		{
			return (first % second);
		}

		public static float RetU8(float first)
		{
			return first;
		}

		public static bool CeqU8U8(float first, float second)
		{
			return (first == second);
		}

		public static bool CltU8U8(float first, float second)
		{
			return (first < second);
		}

		public static bool CgtU8U8(float first, float second)
		{
			return (first > second);
		}

		public static bool CleU8U8(float first, float second)
		{
			return (first <= second);
		}

		public static bool CgeU8U8(float first, float second)
		{
			return (first >= second);
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