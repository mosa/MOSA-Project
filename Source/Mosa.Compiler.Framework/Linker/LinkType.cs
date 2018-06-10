// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework.Linker
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

		/// <summary>
		/// The size of the object
		/// </summary>
		Size,
	}
}
