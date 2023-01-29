// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.x86.Transforms.RuntimeCall;

namespace Mosa.Platform.x86.Stages;

/// <summary>
/// Runtime Call Transformation Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class RuntimeCallStage : Compiler.Framework.Stages.BaseTransformStage
{
	public override string Name => "x86." + GetType().Name;

	public RuntimeCallStage()
		: base(true, false, 1)
	{
		AddTranformations(RuntimeCallTransforms.List);
	}
}
