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
		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <returns></returns>
		public object CreateInstance(Type t)
		{
			return CreateInstance(t, new object[] { });
		}

		/// <summary>
		/// Creates the instance.
		/// </summary>
		/// <param name="t">The t.</param>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		public object CreateInstance(Type t, params object[] args)
		{
			// TODO
			return null;
		}
	}
}
