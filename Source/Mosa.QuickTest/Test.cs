/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

namespace Mosa.QuickTest
{

	public class GenericTest<T>
	{
		public T value;

		public T GetValue() { return value; }
		public void SetValue(T value) { this.value = value; }
	}

	public static class UseGeneric
	{
		public static void UseGenericInteger()
		{
			GenericTest<int> genericObject = new GenericTest<int>();

			genericObject.value = 10;
		}

		public static void UseGenericObject()
		{
			GenericTest<object> genericObject = new GenericTest<object>();

			genericObject.value = new object();
		}
	}

}
