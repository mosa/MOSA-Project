/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

namespace Mosa.Test.Application
{
	public class Parent<T>
	{
		public T value;

		public Child<T> child;

		public class Child<U>
		{
			public U value;
		}
	}
}