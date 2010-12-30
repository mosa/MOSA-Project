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

	public static class SByteTests
	{

		public static int AddU1U1(sbyte first, sbyte second)
		{
			return (first + second);
		}

		public static int SubU1U1(sbyte first, sbyte second)
		{
			return (first - second);
		}

		public static int MulU1U1(sbyte first, sbyte second)
		{
			return (first * second);
		}

		public static int DivU1U1(sbyte first, sbyte second)
		{
			return (first / second);
		}

		public static int RemU1U1(sbyte first, sbyte second)
		{
			return (first % second);
		}

		public static sbyte RetU1(sbyte first)
		{
			return first;
		}

		public static int AndU1U1(sbyte first, sbyte second)
		{
			return (first & second);
		}

		public static int OrU1U1(sbyte first, sbyte second)
		{
			return (first | second);
		}

		public static int XorU1U1(sbyte first, sbyte second)
		{
			return (first ^ second);
		}

		//public static int CompU1(sbyte first)
		//{
		//    return (~first);
		//}

		//public static int ShiftLeftU1U1(sbyte first, byte second)
		//{
		//    return (first << second);
		//}

		//public static int ShiftRightU1U1(sbyte first, byte second)
		//{
		//    return (first >> second);
		//}

		public static bool CeqU1U1(sbyte first, sbyte second)
		{
			return (first == second);
		}

		public static bool CltU1U1(sbyte first, sbyte second)
		{
			return (first < second);
		}

		public static bool CgtU1U1(sbyte first, sbyte second)
		{
			return (first > second);
		}

		public static bool CleU1U1(sbyte first, sbyte second)
		{
			return (first <= second);
		}

		public static bool CgeU1U1(sbyte first, sbyte second)
		{
			return (first >= second);
		}

		public static bool Newarr()
		{
			byte[] arr = new byte[0];
			return arr != null;
		}

		public static bool Ldlen(int length)
		{
			sbyte[] arr = new sbyte[length];
			return arr.Length == length;
		}

		public static bool Ldelem(int index, sbyte value)
		{
			sbyte[] arr = new sbyte[index + 1];
			arr[index] = value;
			return value == arr[index];
		}

		public static bool Stelem(int index, sbyte value)
		{
			sbyte[] arr = new sbyte[index + 1];
			arr[index] = value;
			return true;
		}

		public static bool Ldelema(int index, sbyte value)
		{
			sbyte[] arr = new sbyte[index + 1];
			SetValueInRefValue(ref arr[index], value);
			return arr[index] == value;
		}

		private static void SetValueInRefValue(ref sbyte destination, sbyte value)
		{
			destination = value;
		}
	}
}