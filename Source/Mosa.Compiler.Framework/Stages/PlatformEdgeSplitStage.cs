// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	This stage removes critical edges by inserting empty basic blocks. Some SSA optimizations and the flow
	///	control resolution in the register allocator require that all critical edges are removed.
	/// </summary>
	public class PlatformEdgeSplitStage : BaseEdgeSplitStage
	{
		/// <summary>
		/// Inserts the jump instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="block">The block.</param>
		protected override void InsertJumpInstruction(Context context, BasicBlock block)
		{
			Architecture.InsertJumpInstruction(context, block);
		}
	}
}
