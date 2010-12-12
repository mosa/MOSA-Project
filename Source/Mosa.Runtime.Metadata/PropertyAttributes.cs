/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Metadata
{
	/// <summary>
	/// 
	/// </summary>
	public enum PropertyAttributes
	{
		/// <summary>
		/// 
		/// </summary>
		SpecialName = 0x0200,
		/// <summary>
		/// 
		/// </summary>
		RTSpecialName = 0x0400,
		/// <summary>
		/// 
		/// </summary>
		HasDefault = 0x1000,
		/// <summary>
		/// 
		/// </summary>
		Unused = 0xe9ff
	}
}
