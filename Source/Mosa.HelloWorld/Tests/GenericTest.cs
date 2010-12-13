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
			PrintResult(GenericTest4());
			PrintResult(GenericTest5());
			PrintResult(GenericTest6());
		}

		public static bool GenericTest1()
		{
			GenericClassTest<int> genericObject = new GenericClassTest<int>();

			genericObject.value = 10;

			return genericObject.value == 10;
		}

		public static bool GenericTest2()
		{
			GenericClassTest<object> genericObject = new GenericClassTest<object>();

			genericObject.value = new object();

			return true;
		}

		public static bool GenericTest3()
		{
			GenericClassTest<int> genericObject = new GenericClassTest<int>();

			genericObject.value = 10;

			return genericObject.GetValue() == 10;
		}

		public static bool GenericTest4()
		{
			GenericClassTest<TestObject> genericObject = new GenericClassTest<TestObject>();

			genericObject.value = new TestObject(232, 1231);

			if (genericObject.GetValue().A != 232)
				return false;

			if (genericObject.GetValue().B != 1231)
				return false;

			if (genericObject.value.A != 232)
				return false;

			if (genericObject.value.B != 1231)
				return false;

			return true;
		}

		public static bool GenericTest5()
		{
			GenericClassTest<TestObject> genericObject = new GenericClassTest<TestObject>();

			genericObject.SetValue(new TestObject(232, 1231));

			if (genericObject.GetValue().A != 232)
				return false;

			if (genericObject.GetValue().B != 1231)
				return false;

			if (genericObject.value.A != 232)
				return false;

			if (genericObject.value.B != 1231)
				return false;

			return true;
		}

		public static bool GenericTest6()
		{
			GenericClassTest<int> genericObject = new GenericClassTest<int>();

			return (genericObject.Return10() == 10);
		}
	}
	public interface GenericInterface<T>
	{
		int Return10();
	}

	public class GenericClassTest<T> : GenericInterface<T>
	{
		public T value;

		public T GetValue() { return value; }
		public void SetValue(T value) { this.value = value; }

		public int Return10() { return 10; }
	}

	public class TestObject
	{
		public int A;
		public int B;

		public TestObject(int a, int b)
		{
			A = a;
			B = b;
		}
	}

}
