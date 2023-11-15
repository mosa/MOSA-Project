// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.ARM64.Transforms.FixedRegisters;
using Mosa.Compiler.ARM64.Transforms.Stack;
using Mosa.Compiler.ARM64.Transforms.Tweak;

namespace Mosa.Compiler.ARM64.Stages;

/// <summary>
/// Platform Transformation Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class PlatformTransformStage : Compiler.Framework.Stages.BaseTransformStage
{
	public override string Name => "ARM64." + GetType().Name;

	public PlatformTransformStage()
		: base(0)
	{
		AddTranforms(TweakTransforms.List);
		AddTranforms(FixedRegistersTransforms.List);
		AddTranforms(StackTransforms.List);
		//AddTranformations(SpecialTransforms.List);
	}
}
