// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.x64.Transforms.BaseIR;

namespace Mosa.Compiler.x64.Stages;

/// <summary>
/// x64 IR Transform Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class IRTransformStage : Compiler.Framework.Stages.BaseTransformStage
{
	public override string Name => "x64." + GetType().Name;

	public IRTransformStage()
		: base()
	{
		AddTranforms(IRTransforms.List);
	}
}
