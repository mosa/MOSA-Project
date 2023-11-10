// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Managers;
using Mosa.Compiler.Framework.Transforms.LowerTo32;
using Mosa.Compiler.Framework.Transforms.Optimizations.Auto;
using Mosa.Compiler.Framework.Transforms.Optimizations.Manual;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
///	Optimization Stage
/// </summary>
public class OptimizationStage : BaseTransformStage
{
	private readonly CodeMotionManager CodeMotion = new();
	private readonly bool LowerTo32;

	public OptimizationStage(bool lowerTo32)
		: base()
	{
		LowerTo32 = lowerTo32;

		AddTranforms(ManualTransforms.List);
		AddTranforms(AutoTransforms.List);

		if (LowerTo32)
		{
			AddTranforms(LowerTo32Transforms.List);
		}

		AddTranforms(Transforms.BasicBlocks.BasicBlocksTransforms.List);

		EnableBlockOptimizations = true;
	}

	protected override void Setup()
	{
		Transform.AddManager(CodeMotion);
	}

	protected override bool SetupPhase(int phase)
	{
		switch (phase)
		{
			case 0:
				return true;

			case 1 when LowerTo32:
				Transform.SetStageOptions(TransformStageOption.LowerTo32);
				return true;
		}

		return false;
	}
}
