// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// X86 Optimization Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.Stages.BaseTransformationStage" />
	public sealed class EarlyOptimizationStage : BaseOptimizationStage
	{
		public override string Name { get { return "x86." + GetType().Name; } }

		public EarlyOptimizationStage()
			: base(false)
		{
			//AddTranformations(AutoTransforms.List);

			AddTranformation(new Transform.Manual.Special.Deadcode());

			AddTranformation(new Transform.Manual.Add32ToInc32());
			AddTranformation(new Transform.Manual.Sub32ToDec32());

			AddTranformation(new Transform.Manual.Add32ToLea32());
			AddTranformation(new Transform.Manual.Sub32ToLea32());

			//AddTranformation(new Transform.Manual.Special.Mov32Propagate());

			AddTranformation(new Transform.Manual.Stack.Add32());
		}

		protected override void CustomizeTransformationContract()
		{
		}
	}
}
