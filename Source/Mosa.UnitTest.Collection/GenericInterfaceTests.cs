// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.UnitTest.Collection
{
	public interface IInterfaceAA<T>
	{
		T GetValue(T value);
	}

	public interface IInterfaceBB<T>
	{
		T Get(T value);
	}

	public class GenericInterfaceTestClass<T> : IInterfaceAA<T>, IInterfaceBB<T>
	{
		public T GetValue(T value)
		{
			return value;
		}

		T IInterfaceBB<T>.Get(T value)
		{
			return value;
		}
	}

	public static class GenericInterfaceTests
	{
		public static int InterfaceTest1(int value)
		{
			var genericInterfaceTestClass = new GenericInterfaceTestClass<int>();
			return genericInterfaceTestClass.GetValue(value);
		}

		public static int InterfaceTest2(int value)
		{
			IInterfaceAA<int> aa = new GenericInterfaceTestClass<int>();
			return aa.GetValue(value);
		}

		public static int InterfaceTest3(int value)
		{
			IInterfaceBB<int> bb = new GenericInterfaceTestClass<int>();
			return bb.Get(value);
		}

		public static int InterfaceTest4()
		{
			int[] list = new int[] { 1, 3, 5 };
			IList<int> iList = list;

			int result = 0;
			foreach (var i in iList)
				result += i;

			return result;
		}
	}
}
