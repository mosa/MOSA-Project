// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Platform;
using System;
using System.Diagnostics;

namespace Mosa.Platform.ARMv8A32.Stages
{
	/// <summary>
	/// Transforms 64-bit arithmetic to 32-bit operations.
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	/// <remarks>
	/// This stage translates all 64-bit operations to appropriate 32-bit operations on
	/// architectures without appropriate 64-bit integral operations.
	/// </remarks>
	public sealed class ImmediateConstantStage : BaseCodeTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(ARMv8A32.Add, Add);
			AddVisitation(ARMv8A32.Adc, Adc);
			AddVisitation(ARMv8A32.Mov, Mov);
			AddVisitation(ARMv8A32.Mov, Mvn);
			AddVisitation(ARMv8A32.Mvn, Orr);
			AddVisitation(ARMv8A32.Mov, Eor);
		}

		#region Visitation Methods

		private void Eor(Context context)
		{
			CheckOperand(context, 2, ARMv8A32.EorImm);
		}

		private void Orr(Context context)
		{
			CheckOperand(context, 2, ARMv8A32.OrrImm);
		}

		private void Mvn(Context context)
		{
			CheckOperand(context, 2, ARMv8A32.MvnImm);
		}

		private void Mov(Context context)
		{
			CheckOperand(context, 2, ARMv8A32.MovImm);
		}

		private void Adc(Context context)
		{
			CheckOperand(context, 2, ARMv8A32.AdcImm);
		}

		private void Add(Context context)
		{
			CheckOperand(context, 2, ARMv8A32.AddImm);
		}

		#endregion Visitation Methods

		private void CheckOperand(Context context, int index, BaseInstruction replacement)
		{
			var operand = context.GetOperand(index);

			if (!operand.IsConstant)
				return;

			if (operand.IsResolvedConstant)
			{
				if (ARMHelper.CalculateImmediateValue(operand.ConstantUnsignedInteger, out uint immediate, out _, out _))
				{
					context.ReplaceInstruction(replacement);
					context.SetOperand(index, CreateConstant(immediate));
				}
				else
				{
					var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
					context.SetOperand(index, v1);

					context.GotoPrevious();

					context.AppendInstruction(ARMv8A32.MovwImm, CreateConstant(operand.ConstantUnsignedInteger & 0xFFFF));
					context.AppendInstruction(ARMv8A32.MovtImm, CreateConstant(operand.ConstantUnsignedInteger >> 16));
				}
			}
			else
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.SetOperand(index, v1);

				context.GotoPrevious();

				// FIXME ---
				context.AppendInstruction(ARMv8A32.Mov, v1, operand);
			}
		}
	}
}
