// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;
using Mosa.Platform.x86.Transform.Manual;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// X86 Post Optimization Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformationStage" />
	public sealed class PostOptimizationStage : BaseOptimizationStage
	{
		public override string Name { get { return "X86." + GetType().Name; } }

		public PostOptimizationStage()
			: base(false)
		{
			AddTranformation(new Mov32ToXor32());
		}

		protected override void CustomizeTransformationContract()
		{
		}
	}
}
