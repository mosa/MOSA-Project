// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.ARMv8A32.Stages
{
	/// <summary>
	/// Transforms IR instructions into their appropriate ARMv8A32.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms IR instructions into their equivalent ARMv8A32 sequences.
	/// </remarks>
	public sealed class IRTransformationStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.Add32, Add32);
		}

		#region Visitation Methods

		private void Add32(Context context)
		{
			if (context.Operand2.IsVirtualRegister)
			{
				context.SetInstruction(ARMv8A32.Add32, context.Result, context.Operand1, context.Operand2, ConstantZero32, ConstantZero32);
			}
			else
			{
				context.SetInstruction(ARMv8A32.AddImm32, context.Result, context.Operand1, context.Operand2);
			}
		}

		#endregion Visitation Methods
	}
}
