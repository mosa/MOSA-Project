// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;

namespace Mosa.Platform.ARMv8A32.Stages
{
	/// <summary>
	/// The simple dead code removal stage remove useless instructions
	/// and NOP instructions are moved
	/// and a simple move propagation is performed as well.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Platform.PlatformDeadCodeRemovalStage" />
	public sealed class SimpleDeadCodeRemovalStage : BasePlatformDeadCodeRemovalStage
	{
		protected override bool IsMov(BaseInstruction instruction)
		{
			return instruction == ARMv8A32.Mov;
		}
	}
}
