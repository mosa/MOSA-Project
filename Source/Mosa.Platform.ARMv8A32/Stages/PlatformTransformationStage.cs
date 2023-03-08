// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Platform.ARMv8A32.Transforms.FixedRegisters;
using Mosa.Platform.ARMv8A32.Transforms.Stack;
using Mosa.Platform.ARMv8A32.Transforms.Tweak;

namespace Mosa.Platform.ARMv8A32.Stages;

/// <summary>
/// Platform Transformation Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class PlatformTransformationStage : Compiler.Framework.Stages.BaseTransformStage
{
	public override string Name => "ARMv8A32." + GetType().Name;

	public PlatformTransformationStage()
		: base(true, false, 0)
	{
		AddTranforms(TweakTransforms.List);
		AddTranforms(FixedRegistersTransforms.List);
		AddTranforms(StackTransforms.List);
		//AddTranformations(SpecialTransforms.List);
	}
}
