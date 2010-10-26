/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

namespace Mosa.QuickTest
{

	/// <summary>
	/// 
	/// </summary>
	public class GenericTest<T>
	{
		public T value;

		public T GetValue() { return value; }
		public void SetValue(T value) { this.value = value; }

	}

	public class UseGeneric
	{
		public void UseGenericInteger()
		{
			GenericTest<int> genericInt = new GenericTest<int>();

			genericInt.value = 10;
		}
	
		public void UseGenericObject()
		{
			GenericTest<object> genericInt = new GenericTest<object>();

			genericInt.value = new object();
		}
	}

}
