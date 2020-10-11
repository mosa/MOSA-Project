// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// The simple dead code removal stage remove useless instructions
	/// and NOP instructions are moved
	/// and a simple move propagation is performed as well.
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Platform.PlatformDeadCodeRemovalStage" />
	public sealed class SimpleDeadCodeRemovalStage : PlatformDeadCodeRemovalStage
	{
		protected override bool IsMov(BaseInstruction instruction)
		{
			return instruction == X86.Mov32 || instruction == X86.Movsd || instruction == X86.Movss;
		}
	}
}
