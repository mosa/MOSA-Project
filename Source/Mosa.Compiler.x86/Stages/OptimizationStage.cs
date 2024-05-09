// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.x86.Transforms.Optimizations.Auto;
using Mosa.Compiler.x86.Transforms.Optimizations.Manual;

namespace Mosa.Compiler.x86.Stages;

/// <summary>
/// X86 Optimization Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class OptimizationStage : Framework.Stages.BaseTransformStage
{
	public override string Name => "x86." + GetType().Name;

	public OptimizationStage()
		: base()
	{
	}

	protected override void Initialize()
	{
		base.Initialize();

		AddTranforms(Filter(AutoTransforms.List));
		AddTranforms(Filter(ManualTransforms.List));
	}
}
