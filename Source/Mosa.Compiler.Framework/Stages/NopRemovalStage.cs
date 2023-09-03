// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// This stages remove Nop instructions.
/// </summary>
public class NopRemovalStage : BaseTransformStage
{
	protected override void Run()
	{
		foreach (var block in BasicBlocks)
		{
			block.RemoveNops();
		}
	}
}
