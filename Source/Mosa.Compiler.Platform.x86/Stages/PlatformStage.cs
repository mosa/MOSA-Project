// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Platform.x86.Transforms.AddressMode;
using Mosa.Compiler.Platform.x86.Transforms.FixedRegisters;
using Mosa.Compiler.Platform.x86.Transforms.Optimizations.Manual.Special;
using Mosa.Compiler.Platform.x86.Transforms.Stack;
using Mosa.Compiler.Platform.x86.Transforms.Tweak;

namespace Mosa.Compiler.Platform.x86.Stages
{
	/// <summary>
	/// Platform Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformStage" />
	public sealed class PlatformStage : Compiler.Framework.Stages.BaseTransformStage
	{
		public override string Name => "x86." + GetType().Name;

		public PlatformStage()
			: base(true, false, 0)
		{
			AddTranformations(TweakTransforms.List);
			AddTranformations(FixedRegistersTransforms.List);
			AddTranformations(StackTransforms.List);
			AddTranformations(AddressModeTransforms.List);

			AddTranformation(new Mov32Unless());
			AddTranformation(new Mov32Propagation());
			AddTranformation(new Deadcode());
		}
	}
}
