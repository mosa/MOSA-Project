// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.Framework.Transform.Auto;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// X86 Post Optimization Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformationStage" />
	public sealed class PostOptimizationStage : BaseOptimizationStage
	{
		public override string Name
		{ get { return "x86." + GetType().Name; } }

		public PostOptimizationStage()
			: base(true, false)
		{
			AddTranformations(AutoTransforms.List);

			AddTranformation(new Transform.Manual.Special.Deadcode());

			AddTranformation(new Transform.Manual.Mov32ToXor32());

			AddTranformation(new Transform.Manual.Add32ToInc32());
			AddTranformation(new Transform.Manual.Sub32ToDec32());
			AddTranformation(new Transform.Manual.Lea32ToInc32());
			AddTranformation(new Transform.Manual.Lea32ToDec32());

			AddTranformation(new Transform.Manual.Cmp32ToZero());
			AddTranformation(new Transform.Manual.Test32ToZero());
			AddTranformation(new Transform.Manual.Cmp32ToTest32());

			AddTranformation(new Transform.Manual.Special.Mov32ConstantReuse());
		}
	}
}
