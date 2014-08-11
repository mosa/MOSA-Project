/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace System.Reflection
{
	/// <summary>
	/// Represents a type that you can reflect over.
	/// </summary>
	public interface IReflectableType
	{
		/// <summary>
		/// Retrieves an object that represents this type.
		/// </summary>
		/// <returns>An object that represents this type.</returns>
		TypeInfo GetTypeInfo();
	}
}