/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System;

namespace Mosa.Compiler.Metadata
{

	[Flags]
	public enum ManifestResourceAttributes : uint
	{
		VisibilityMask = 0x0007,
		Public = 0x0001,
		Private = 0x0002
	}
}
