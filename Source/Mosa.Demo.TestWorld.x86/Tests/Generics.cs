// Copyright (c) MOSA Project. Licensed under the New BSD License.

/*
 * (c) 2021 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

namespace Mosa.HelloWorld.Tests;

public class Generics
{
	public interface IMyInterface<T>
	{
		T ReturnIt();
	}

	public class MyGenericClass<T> : IMyInterface<T>
	{
		public T value;

		public T ReturnIt()
		{
			return value;
		}
	}

	public class MyObject
	{
		public int A;

		public MyObject(int a)
		{
			A = a;
		}
	}

	public static void Test()
	{
		MyGenericClass<object> genericObject = new MyGenericClass<object>();
		genericObject.value = new object();
	}

	public static void Test2()
	{
		MyGenericClass<int> genericObject = new MyGenericClass<int>();
		genericObject.value = 10;
	}

	public static void Test3()
	{
		MyGenericClass<MyObject> genericObject = new MyGenericClass<MyObject>();
		genericObject.value = new MyObject(1);
	}

	public static void Test4()
	{
		IMyInterface<int> genericInterface = new MyGenericClass<int>();
	}

	public static void Test5()
	{
		MyGenericClass<int> genericObject = new MyGenericClass<int>();
		genericObject.value = 10;
		IMyInterface<int> genericInterface = genericObject;
	}
}
