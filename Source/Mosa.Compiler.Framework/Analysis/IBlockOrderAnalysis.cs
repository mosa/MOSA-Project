// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Analysis
{
	public interface IBlockOrderAnalysis
	{
		void PerformAnalysis(BasicBlocks basicBlocks);

		#region Properties

		IList<BasicBlock> NewBlockOrder { get; }

		int GetLoopDepth(BasicBlock block);

		int GetLoopIndex(BasicBlock block);

		#endregion Properties
	}
}
