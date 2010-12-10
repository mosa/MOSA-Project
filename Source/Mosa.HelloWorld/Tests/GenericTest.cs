/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */
using Mosa.Platform.x86;
using Mosa.Kernel;
using Mosa.Kernel.x86;
using System;

namespace Mosa.HelloWorld.Tests
{

	public class GenericTest : KernelTest
	{
		public static void Test()
		{
			Screen.Goto(23, 41);
			Screen.Write("Generic Test: ");

			PrintResult(GenericTest1());
			PrintResult(GenericTest2());
			PrintResult(GenericTest3());
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
