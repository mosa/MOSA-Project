// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.x86.Transforms.AddressMode;
using Mosa.Compiler.x86.Transforms.FixedRegisters;
using Mosa.Compiler.x86.Transforms.Optimizations.Manual.Special;
using Mosa.Compiler.x86.Transforms.Stack;
using Mosa.Compiler.x86.Transforms.Tweak;

namespace Mosa.Compiler.x86.Stages;

/// <summary>
/// Platform Transformation Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class PlatformTransformationStage : Compiler.Framework.Stages.BaseTransformStage
{
	public override string Name => "x86." + GetType().Name;

	public PlatformTransformationStage()
		: base(0)
	{
		AddTranforms(TweakTransforms.List);
		AddTranforms(FixedRegistersTransforms.List);
		AddTranforms(StackTransforms.List);
		AddTranforms(AddressModeTransforms.List);

		AddTranform(new Mov32Unless());
		AddTranform(new Mov32Coalescing());
		AddTranform(new Deadcode());
	}
}
