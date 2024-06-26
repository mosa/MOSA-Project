// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transforms.LowerTo32;
using Mosa.Compiler.x86.Transforms.AddressMode;
using Mosa.Compiler.x86.Transforms.FixedRegisters;
using Mosa.Compiler.x86.Transforms.Optimizations.Manual.Special;
using Mosa.Compiler.x86.Transforms.Stack;
using Mosa.Compiler.x86.Transforms.Tweak;

namespace Mosa.Compiler.x86.Stages;

/// <summary>
/// Platform Transform Stage
/// </summary>
/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
public sealed class PlatformTransformStage : Framework.Stages.BaseTransformStage
{
	public override string Name => "x86." + GetType().Name;

	public PlatformTransformStage()
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
