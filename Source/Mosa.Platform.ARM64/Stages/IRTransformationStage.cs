// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.ARM64.Stages;

/// <summary>
/// ARM64 IR Transformation Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class IRTransformationStage : Compiler.Framework.Stages.BaseTransformStage
{
	public override string Name => "ARM64." + GetType().Name;

	public IRTransformationStage()
		: base(true, false)
	{
		//AddTranforms(IRTransforms.List);
	}
}
