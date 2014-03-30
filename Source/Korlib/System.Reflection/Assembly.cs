/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System.Reflection
{
	public class Assembly
	{
		/// <summary>
		/// Gets the types defined in this assembly.
		/// </summary>
		/// <returns>An array that contains all the types that are defined in this assembly.</returns>
		public virtual Type[] GetTypes()
		{
			throw new NotImplementedException();
		}
	}
}