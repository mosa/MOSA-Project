// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.ARM32.Transforms.BaseIR;

namespace Mosa.Compiler.ARM32.Stages;

/// <summary>
/// ARM32 IR Transformation Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class IRTransformationStage : Compiler.Framework.Stages.BaseTransformStage
{
	public override string Name => "ARM32." + GetType().Name;

	public IRTransformationStage()
		: base()
	{
		AddTranforms(IRTransforms.List);
	}
}
