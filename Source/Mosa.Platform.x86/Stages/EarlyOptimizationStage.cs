// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;
using Mosa.Platform.x86.Transform.Manual;
using Mosa.Platform.x86.Transform.Manual.Special;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// X86 Optimization Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformationStage" />
	public sealed class EarlyOptimizationStage : BaseOptimizationStage
	{
		public override string Name { get { return "X86." + GetType().Name; } }

		public EarlyOptimizationStage()
			: base(false)
		{
			//AddTranformations(AutoTransforms.List);

			AddTranformation(new Deadcode());

			//AddTranformation(new Add32ToInc32());
			//AddTranformation(new Sub32ToDec32());

			AddTranformation(new Add32ToLea32());
			AddTranformation(new Sub32ToLea32());

			//AddTranformation(new Mov32Propagate());
		}

		protected override void CustomizeTransformationContract()
		{
		}
	}
}
