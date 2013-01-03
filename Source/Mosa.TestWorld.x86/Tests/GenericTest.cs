/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

namespace Mosa.TestWorld.x86.Tests
{

	public class GenericTest : KernelTest
	{
		public GenericTest() : base("Gen-T") 
		{
			testMethods.Add(GenericTest1);
			testMethods.Add(GenericTest2);
			testMethods.Add(GenericTest3);
			testMethods.Add(GenericTest4);
			testMethods.Add(GenericTest5);
			testMethods.Add(GenericTest6);
			testMethods.Add(GenericTest7);
			testMethods.Add(GenericTest8);
			testMethods.Add(GenericTest9);
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

		public static bool GenericTest7()
		{
			IGenericInterface<int> genericInterface = new GenericClassTest<int>();

			return (genericInterface.Return10() == 10);
		}

		public static bool GenericTest8()
		{
			GenericClassTest<int> genericObject = new GenericClassTest<int>();

			genericObject.value = 10;

			IGenericInterface<int> genericInterface = genericObject;

			return (genericInterface.ReturnIt() == 10);
		}

		public static bool GenericTest9()
		{
			GenericClassTest<int> genericObject = new GenericClassTest<int>();

			genericObject.value = 10;

			return (genericObject.ReturnIt() == 10);
		}
	}

	public interface IGenericInterface<T>
	{
		int Return10();
		T ReturnIt();
	}

	public class GenericClassTest<T> : IGenericInterface<T>
	{
		public T value;

		public T GetValue() { return value; }
		public void SetValue(T value) { this.value = value; }

		public int Return10() { return 10; }
		public T ReturnIt() { return value; }
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
