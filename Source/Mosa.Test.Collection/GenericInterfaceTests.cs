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

	public class GenericInterfaceTests
	{
		public static int InterfaceTest1(int value)
		{
			GenericInterfaceTestClass<int> gic = new GenericInterfaceTestClass<int>();
			return gic.GetValue(value);
		}

		public static int InterfaceTest2(int value)
		{
			GenericInterfaceTestClass<int> gic = new GenericInterfaceTestClass<int>();
			IInterfaceAA<int> aa = gic;
			return aa.GetValue(value);
		}

		public static int InterfaceTest3(int value)
		{
			GenericInterfaceTestClass<int> gic = new GenericInterfaceTestClass<int>();
			IInterfaceBB<int> bb = gic;
			return bb.Get(value);
		}
	}
}