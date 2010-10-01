/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

namespace Mosa.HelloWorld.Test
{
	class GenericType<T>
	{
		public static T StaticMethodInGenericType(T same)
		{
			return same;
		}
	}

	static class Test
	{
		public static bool TestCallStaticMethodInGenericTypeWith(bool value)
		{
			return value == GenericType<bool>.StaticMethodInGenericType(value);
		}
	}

}
