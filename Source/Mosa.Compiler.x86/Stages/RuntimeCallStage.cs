// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.x86.Transforms.RuntimeCall;

namespace Mosa.Compiler.x86.Stages;

/// <summary>
/// Runtime Call Transformation Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class RuntimeCallStage : Framework.Stages.BaseTransformStage
{
	public override string Name => "x86." + GetType().Name;

	public RuntimeCallStage()
		: base(1)
	{
		AddTranforms(RuntimeCallTransforms.List);
	}
}
