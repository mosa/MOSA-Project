// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
///	This stage removes dead and empty blocks.
/// </summary>
public class DeadBlockStage : BaseTransformStage
{
	public DeadBlockStage() : base()
	{
		AddTranforms(Transforms.BasicBlocks.BasicBlocksTransforms.List);

		EnableBlockOptimizations = true;
	}
}
