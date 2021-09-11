// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// X86 Post Optimization Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformationStage" />
	public sealed class PostOptimizationStage : BaseOptimizationStage
	{
		public override string Name { get { return "x86." + GetType().Name; } }

		public PostOptimizationStage()
			: base(false)
		{
			//AddTranformations(AutoTransforms.List);

			AddTranformation(new Transform.Manual.Special.Deadcode());
			AddTranformation(new Transform.Manual.Mov32ToXor32());

			AddTranformation(new Transform.Manual.Add32ToInc32());
			AddTranformation(new Transform.Manual.Sub32ToDec32());
			AddTranformation(new Transform.Manual.Lea32ToInc32());
			AddTranformation(new Transform.Manual.Lea32ToDec32());
		}

		protected override void CustomizeTransformationContract()
		{
		}
	}
}
