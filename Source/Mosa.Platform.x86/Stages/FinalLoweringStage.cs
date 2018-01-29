// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Lowering Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	public sealed class FinalLoweringStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(X86.Add32, Add32);
			AddVisitation(X86.Adc32, Adc32);
		}

		#region Visitation Methods

		public void Add32(Context context)
		{
			if (context.Operand2.IsConstant)
			{
				context.SetInstruction(X86.AddConst32, context.Result, context.Operand1, context.Operand2);
			}
		}

		public void Adc32(Context context)
		{
			if (context.Operand2.IsConstant)
			{
				context.SetInstruction(X86.AdcConst32, context.Result, context.Operand1, context.Operand2);
			}
		}

		#endregion Visitation Methods
	}
}
