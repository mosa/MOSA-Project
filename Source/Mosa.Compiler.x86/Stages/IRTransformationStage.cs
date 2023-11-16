// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.x86.Transforms.BaseIR;

namespace Mosa.Compiler.x86.Stages;

/// <summary>
/// X86 IR Transform Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class IRTransformationStage : Framework.Stages.BaseTransformStage
{
	public override string Name => "x86." + GetType().Name;

	public IRTransformationStage()
		: base()
	{
		AddTranforms(IRTransforms.List);
	}
}
