/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;

namespace Mosa.Test.Quick.Tests
{
	public class AA { }
	public class BB : AA { }
	public class CC { }

	public class Test
	{
		public bool BBisAA()
		{
			object b = new BB();

			if (b is AA)
				return true;

			return false;
		}

		public CC BBasAA()
		{
			object b = new BB();

			CC c = b as CC;

			return c;
		}
	}

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

	public class A<T> { }

	public class B : A<int> { }

	public class C<T> : A<int> { }

	public class Parent<T>
	{
		public T value;

		public T Get() { return value; }
		public void Set(T value) { this.value = value; }

		public class NestedClass
		{
			public T nestedValue;

			public T Get() { return nestedValue; }
			public void Set(T value) { this.nestedValue = value; }
		}

		public class GenericNestedClass<S>
		{
			public S nestedValue;

			public S Get() { return nestedValue; }
			public void Set(S value) { this.nestedValue = value; }
		}

	}

	public class ClosedClass : Parent<int>
	{
		public int integerValue;
	}

	public class K
	{
		public int Kvalue;

		public void SetK(int kValue) { this.Kvalue = kValue; }
	}

	public class KK : K
	{
		public int KKvalue;

		public void SetKK(int kkValue) { this.KKvalue = kkValue; }
	}

	public class KKK : KK
	{
		public int KKKvalue;

		public void SetKKK(int kkkValue) { this.KKKvalue = kkkValue; }
	}

}
