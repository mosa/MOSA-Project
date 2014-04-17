/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.Compiler.Linker
{
	/// <summary>
	/// Specifies the type of link to perform by the linker.
	/// </summary>
	[Flags]
	public enum LinkType
	{
		/// <summary>
		/// The link destination receives a relative address.
		/// </summary>
		RelativeOffset,

		/// <summary>
		/// The link destination receives the absolute address.
		/// </summary>
		AbsoluteAddress,

	}
}