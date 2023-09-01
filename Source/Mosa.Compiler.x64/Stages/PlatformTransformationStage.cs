// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.x64.Transforms.AddressMode;
using Mosa.Compiler.x64.Transforms.FixedRegisters;
using Mosa.Compiler.x64.Transforms.Optimizations.Manual.Special;
using Mosa.Compiler.x64.Transforms.Stack;
using Mosa.Compiler.x64.Transforms.Tweak;

namespace Mosa.Compiler.x64.Stages;

/// <summary>
/// Platform Transformation Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class PlatformTransformationStage : Compiler.Framework.Stages.BaseTransformStage
{
	public override string Name => "x64." + GetType().Name;

	public PlatformTransformationStage()
		: base(true, false, 0)
	{
		AddTranforms(TweakTransforms.List);
		AddTranforms(FixedRegistersTransforms.List);
		AddTranforms(StackTransforms.List);
		AddTranforms(AddressModeTransforms.List);
		AddTranforms(SpecialTransforms.List);

		AddTranform(new Mov32Unless());
		AddTranform(new Mov64Unless());
		AddTranform(new Mov32Coalescing());
		AddTranform(new Mov64Coalescing());
		AddTranform(new Deadcode());
	}
}
