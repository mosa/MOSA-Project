/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// 
	/// </summary>
	public interface IBasicBlockOrder
	{
		/// <summary>
		/// Gets the ordered blocks.
		/// </summary>
		/// <value>The ordered blocks.</value>
		int[] OrderedBlocks { get; }
	}
}
