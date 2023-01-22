// Copyright (c) MOSA Project. Licensed under the New BSD License.

/*
 * (c) 2021 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;

namespace Mosa.Demo.TestWorld.x86.Tests
{
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
			MyGenericClass<Object> genericObject = new MyGenericClass<Object>();
			genericObject.value = new object();
		}

		public static void Test2()
		{
			MyGenericClass<Int32> genericObject = new MyGenericClass<Int32>();
			genericObject.value = 10;
		}

		public static void Test3()
		{
			MyGenericClass<MyObject> genericObject = new MyGenericClass<MyObject>();
			genericObject.value = new MyObject(1);
		}

		public static void Test4()
		{
			IMyInterface<Int32> genericInterface = new MyGenericClass<Int32>();
		}

		public static void Test5()
		{
			MyGenericClass<Int32> genericObject = new MyGenericClass<Int32>();
			genericObject.value = 10;
			IMyInterface<Int32> genericInterface = genericObject;
		}
	}
}
