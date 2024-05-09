﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
	private readonly ExceptionHandlerOperandManager ExceptionHandlerOperands = new();

	private readonly bool LowerTo32;

	public OptimizationStage(bool lowerTo32)
		: base()
	{
		LowerTo32 = lowerTo32;
		EnableBlockOptimizations = true;

		AddTranforms(ManualTransforms.List);
		AddTranforms(AutoTransforms.List);

		if (LowerTo32)
		{
			AddTranforms(LowerTo32Transforms.List);
		}

		AddTranforms(Transforms.BasicBlocks.BasicBlocksTransforms.List);
	}

	protected override void Setup()
	{
		Transform.AddManager(CodeMotion);
		Transform.AddManager(ExceptionHandlerOperands);

		CodeMotion.Setup(Transform.MethodCompiler);
		ExceptionHandlerOperands.Setup(Transform.MethodCompiler);
	}

	protected override bool SetupPhase(int phase)
	{
		if (Compiler.MosaSettings.ReduceCodeSize)
		{
			Transform.SetStageOption(TransformStageOption.ReduceCodeSize);
		}

		switch (phase)
		{
			case 0:
				return true;

			case 1 when LowerTo32:
				Transform.SetStageOption(TransformStageOption.LowerTo32);
				return true;
		}

		return false;
	}
}
