/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

namespace System
{
	/// <summary>
	/// Interface for "System.ICloneable"
	/// </summary>
	public interface ICloneable
	{
		object Clone();
	}
}