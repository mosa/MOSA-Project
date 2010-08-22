/*
 * (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mosa.Tools.Compiler.Metadata
{
	/// <summary>
	/// Stores positional information about a metadata stream.
	/// </summary>
	public struct MetadataStreamPosition
	{
		/// <summary>
		/// Holds the position of the stream relative to the start of the metadata symbol stream.
		/// </summary>
		public long Position;

		/// <summary>
		/// Holds the size of the stream in bytes.
		/// </summary>
		public long Size;
	}
}
