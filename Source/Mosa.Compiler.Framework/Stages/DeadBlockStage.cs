// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
///	This stage removes dead and empty blocks.
/// </summary>
public class DeadBlockStage : BaseTransformStage
{
	public DeadBlockStage() : base(false, true)
	{
	}
}
