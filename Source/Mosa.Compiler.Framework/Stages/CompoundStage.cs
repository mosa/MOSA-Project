// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms.Compound;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Transforms compound instructions into lower instructions
/// </summary>
public sealed class CompoundStage : BaseTransformStage
{
	public CompoundStage()
		: base(1)
	{
		AddTranforms(CompoundTransforms.List);
	}
}
