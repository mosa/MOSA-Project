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
	private readonly CodeMotionManager CodeMotion = new CodeMotionManager();
	private readonly bool LowerTo32;

	public OptimizationStage(bool lowerTo32)
		: base(true, true)
	{
		LowerTo32 = lowerTo32;

		AddTranforms(ManualTransforms.List);
		AddTranforms(AutoTransforms.List);

		if (LowerTo32)
			AddTranforms(LowerTo32Transforms.List);
	}

	protected override void CustomizeTransform(TransformContext transformContext)
	{
		transformContext.SetStageOptions(LowerTo32 && MosaSettings.LongExpansion && Is32BitPlatform);
		transformContext.AddManager(CodeMotion);
	}
}
