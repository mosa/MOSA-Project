// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Special;

namespace Mosa.Compiler.x86.Stages;

/// <summary>
/// X86 Advance IR Transformation Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class AdvanceIRTransformStage : Framework.Stages.BaseTransformStage
{
	public override string Name => "x86." + GetType().Name;

	public AdvanceIRTransformStage()
		: base()
	{
		//AddTranforms(Transforms.BaseIR.Auto.AutoTransforms.List);
		AddTranform(new Deadcode());
	}
}
