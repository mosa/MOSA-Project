/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// 
	/// </summary>
	public interface IBasicBlockOrder
	{
		/// <summary>
		/// Gets the ordered Blocks.
		/// </summary>
		/// <value>The ordered Blocks.</value>
		int[] OrderedBlocks { get; }
	}
}
