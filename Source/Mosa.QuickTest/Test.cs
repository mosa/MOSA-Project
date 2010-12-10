/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;

namespace Mosa.HelloWorld.Tests
{

	public class Test
	{
		public static void GoTest()
		{
			GenericTest1();
			GenericTest2();
		}

		public static bool GenericTest1()
		{
			GenericTest<int> genericObject = new GenericTest<int>();

			genericObject.value = 10;

			return genericObject.value == 10;
		}

		public static bool GenericTest2()
		{
			GenericTest<object> genericObject = new GenericTest<object>();

			genericObject.value = new object();

			return true;
		}

		public static bool GenericTest3()
		{
			GenericTest<int> genericObject = new GenericTest<int>();

			genericObject.value = 10;

			return genericObject.GetValue() == 10;
		}

	}


	public class GenericTest<T>
	{
		public T value;

		public T GetValue() { return value; }
		public void SetValue(T value) { this.value = value; }
	}

}
