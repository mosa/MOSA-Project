// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.Framework.Transform;
using Mosa.Compiler.Framework.Transform.Auto;
using Mosa.Compiler.Framework.Transform.Manual;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///	Optimization Stage
	/// </summary>
	public class OptimizationStage : BaseOptimizationStage
	{
		private bool LowerTo32;

		public OptimizationStage(bool lowerTo32)
			: base(true)
		{
			LowerTo32 = lowerTo32;
			CreateTransformationList(ManualTransforms.List);
			CreateTransformationList(AutoTransforms.List);
		}

		protected override void CustomizeTransformationContract()
		{
			TransformContext.SetStageOptions(IsInSSAForm, LowerTo32 && CompilerSettings.LongExpansion && Is32BitPlatform);
		}
	}
}
