// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.ARM32.Transforms.Optimizations.Auto;
using Mosa.Compiler.ARM32.Transforms.Optimizations.Manual;

namespace Mosa.Compiler.ARM32.Stages;

/// <summary>
/// ARM32 Optimization Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class OptimizationStage : Compiler.Framework.Stages.BaseTransformStage
{
	public override string Name => "ARM32." + GetType().Name;

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
