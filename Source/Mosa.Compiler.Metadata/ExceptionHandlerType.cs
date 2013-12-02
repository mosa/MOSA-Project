/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Metadata
{
	/// <summary>
	///
	/// </summary>
	public enum ExceptionHandlerType : byte
	{
		/// <summary>
		/// A typed exception handler clause.
		/// </summary>
		Exception = 0,

		/// <summary>
		/// An exception filter and handler clause.
		/// </summary>
		Filter = 1,

		/// <summary>
		/// A finally clause.
		/// </summary>
		Finally = 2,

		/// <summary>
		/// A fault clause. This is similar to finally, except its only executed if an exception is/was processed.
		/// </summary>
		Fault = 4
	}
}