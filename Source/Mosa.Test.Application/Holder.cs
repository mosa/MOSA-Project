/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

namespace Mosa.Test.Application
{
	public class Holder<T>
	{
		public T value;

		public T GetValue()
		{
			return value;
		}

		public void SetValue(T value)
		{
			this.value = value;
		}

		public Holder(T value)
		{
			this.value = value;
		}
	}
}