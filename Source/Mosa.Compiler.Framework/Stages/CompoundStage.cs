// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.Framework.Transforms.Compound;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Transforms compound instructions into lower instructions
/// </summary>
public sealed class CompoundStage : BaseTransformStage
{
	public CompoundStage()
		: base(true, false, 1)
	{
		AddTranformations(CompoundTransforms.List);
	}
}
