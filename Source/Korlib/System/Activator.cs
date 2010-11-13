/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System
{
	/// <summary>
	/// Implementation of the "Activator" class.
	/// </summary>
	public class Activator
	{
	
		public static T CreateInstance<T>()
		{
			return (T)CreateInstance(typeof(T));
		}

		public static object CreateInstance(Type type)
		{
			return CreateInstance(type, false);
		}

		public static object CreateInstance(Type type, bool nonPublic)
		{
			// TODO
			return null;
		}
	}
}
