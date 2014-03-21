/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Framework
{
	public interface IBlockOrderAnalysis
	{
		void PerformAnalysis(BasicBlocks basicBlocks);
		
		#region Properties

		BasicBlock[] NewBlockOrder { get; }

		int GetLoopDepth(BasicBlock block);

		int GetLoopIndex(BasicBlock block);

		#endregion Properties
	}
}