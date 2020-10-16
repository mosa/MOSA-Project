// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	This stage removes critical edges by inserting empty basic blocks. Some SSA optimizations and flow
	///	control resolution in the register allocator require that all critical edges are removed.
	/// </summary>
	public class EdgeSplitStage : BaseEdgeSplitStage
	{
	}
}
