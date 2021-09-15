// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Transform;
using Mosa.Compiler.Framework.Transform.Auto;
using Mosa.Compiler.Framework.Transform.Manual;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	Optimization Stage
	/// </summary>
	public class OptimizationStage : BaseOptimizationStage
	{
		private readonly bool LowerTo32;

		public OptimizationStage(bool lowerTo32)
			: base(true, true)
		{
			LowerTo32 = lowerTo32;
			AddTranformations(ManualTransforms.List);
			AddTranformations(AutoTransforms.List);
		}

		protected override void CustomizeTransformation()
		{
			TransformContext.SetStageOptions(IsInSSAForm, LowerTo32 && CompilerSettings.LongExpansion && Is32BitPlatform);
		}
	}
}
