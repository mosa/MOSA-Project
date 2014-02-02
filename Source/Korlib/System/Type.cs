/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */
using System.Reflection;

namespace System
{
	/// <summary>
	/// Implementation of the "System.Type" class.
	/// </summary>
	public abstract class Type
	{
		// TODO

		public static Type GetTypeFromHandle(RuntimeTypeHandle handle)
		{
			return null;
		}

		public static Type GetType(string typeName, bool throwOnError)
		{
			return null;
		}

        public abstract /*override*/ Module Module
        {
            get;
        }
	}
}