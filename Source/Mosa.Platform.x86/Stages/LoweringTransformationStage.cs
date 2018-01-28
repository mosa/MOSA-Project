// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Lowering Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	public sealed class LoweringTransformationStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			//AddVisitation(X86.Adc, Adc);
		}

		#region Visitation Methods

		public void Adc(Context context)
		{
			if (context.Operand2.IsConstant)
			{
				context.SetInstruction(X86.AdcConst, context.Result, context.Operand1, context.Operand2);
			}
		}

		#endregion Visitation Methods
	}
}
