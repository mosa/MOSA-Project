// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.ARM32.Transforms.FloatingTweaks;
using Mosa.Compiler.Framework.Transforms.Optimizations.Manual.Propagate;

namespace Mosa.Compiler.ARM32.Stages;

/// <summary>
/// ARM32 IR Transformation Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class AdvanceTransformStage : Compiler.Framework.Stages.BaseTransformStage
{
	public override string Name => "ARM32." + GetType().Name;

	public AdvanceTransformStage()
		: base()
	{
		AddTranforms(FloatingTweakTransforms.List);

		AddTranform(new Move32PropagateConstant());
		AddTranform(new Move64PropagateConstant());
		AddTranform(new MoveObjectPropagateConstant());
		AddTranform(new MoveManagedPointerPropagateConstant());
	}
}
