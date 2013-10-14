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
using Mosa.ClassLib;

namespace Mosa.HelloWorld.Tests
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
			public T ReturnIt() { return value; }
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


}
