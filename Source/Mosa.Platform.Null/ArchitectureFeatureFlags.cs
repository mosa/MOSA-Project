/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.Platform.Null
{
	/// <summary>
	/// Determines features provided by an architecture.
	/// </summary>
	[Flags]
	public enum ArchitectureFeatureFlags
	{
		/// <summary>
		/// Auto detect architecture features using the current processor. Not available for cross compilation.
		/// </summary>
		AutoDetect = 0,

		// FIXME: Add more instructions set specific flags
	}
}
