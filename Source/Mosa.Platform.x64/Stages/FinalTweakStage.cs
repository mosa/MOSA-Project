// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;
using System.Diagnostics;

namespace Mosa.Platform.x64.Stages
{
	/// <summary>
	/// Final Tweak Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.X64.BaseTransformationStage" />
	public sealed class FinalTweakStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(X64.Call, CallReg);
			AddVisitation(X64.Mov32, Mov32);
			AddVisitation(X64.Mov64, Mov64);
			AddVisitation(X64.MovLoad16, MovLoad16);
			AddVisitation(X64.MovLoad8, MovLoad8);
			AddVisitation(X64.Movsd, Movsd);
			AddVisitation(X64.Movss, Movss);
			AddVisitation(X64.MovStore16, MovStore16);
			AddVisitation(X64.MovStore8, MovStore8);
			AddVisitation(X64.Movzx16To32, Movzx16To32);
			AddVisitation(X64.Movzx8To32, Movzx8To32);
			AddVisitation(X64.Nop, Nop);
			AddVisitation(X64.Setcc, Setcc);
		}

		#region Visitation Methods

		public void CallReg(Context context)
		{
			// TODO: this can be written better
			var node = context.Node;

			Debug.Assert(node.Operand1 != null);
			Debug.Assert(node.Operand1.IsCPURegister);

			var before = node.Previous;

			while (before.IsEmpty && !before.IsBlockStartInstruction)
			{
				before = before.Previous;
			}

			if (before == null || before.IsBlockStartInstruction)
				return;

			if (before.Instruction != X64.Mov64)
				return;

			if (!before.Result.IsCPURegister)
				return;

			if (node.Operand1.Register != before.Result.Register)
				return;

			before.SetInstruction(X64.Call, null, before.Operand1);
			node.Empty();
		}

		public void Mov32(Context context)
		{
			var source = context.Operand1;
			var result = context.Result;

			Debug.Assert(result.IsCPURegister);

			if (source.IsCPURegister && result.Register == source.Register)
			{
				context.Empty();
			}
		}

		public void Mov64(Context context)
		{
			var source = context.Operand1;
			var result = context.Result;

			Debug.Assert(result.IsCPURegister);

			if (source.IsCPURegister && result.Register == source.Register)
			{
				context.Empty();
			}
		}

		public void MovLoad16(Context context)
		{
			var result = context.Result;

			Debug.Assert(result.IsCPURegister);

			if (result.Register == GeneralPurposeRegister.ESI || result.Register == GeneralPurposeRegister.EDI)
			{
				var source = context.Operand1;
				var offset = context.Operand2;

				context.SetInstruction(X64.MovLoad32, result, source, offset);
				context.AppendInstruction(X64.And32, result, result, CreateConstant32(0x0000FFFF));
			}
		}

		public void MovLoad8(Context context)
		{
			var result = context.Result;

			Debug.Assert(result.IsCPURegister);

			if (result.Register == GeneralPurposeRegister.ESI || result.Register == GeneralPurposeRegister.EDI)
			{
				var source = context.Operand1;
				var offset = context.Operand2;

				context.SetInstruction(X64.MovLoad32, result, source, offset);
				context.AppendInstruction(X64.And32, result, result, CreateConstant32(0x000000FF));
			}
		}

		public void Movsd(Context context)
		{
			Debug.Assert(context.Result.IsCPURegister);
			Debug.Assert(context.Operand1.IsCPURegister);

			if (context.Result.Register == context.Operand1.Register)
			{
				context.Empty();
			}
		}

		public void Movss(Context context)
		{
			Debug.Assert(context.Result.IsCPURegister);
			Debug.Assert(context.Operand1.IsCPURegister);

			if (context.Result.Register == context.Operand1.Register)
			{
				context.Empty();
			}
		}

		public void MovStore16(Context context)
		{
			var value = context.Operand3;

			if (value.IsCPURegister && (value.Register == GeneralPurposeRegister.ESI || value.Register == GeneralPurposeRegister.EDI))
			{
				var dest = context.Operand1;
				var offset = context.Operand2;

				Operand temporaryRegister = null;

				if (dest.Register != GeneralPurposeRegister.EAX && offset.Register != GeneralPurposeRegister.EAX)
				{
					temporaryRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
				}
				else if (dest.Register != GeneralPurposeRegister.EBX && offset.Register != GeneralPurposeRegister.EBX)
				{
					temporaryRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EBX);
				}
				else if (dest.Register != GeneralPurposeRegister.ECX && offset.Register != GeneralPurposeRegister.ECX)
				{
					temporaryRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ECX);
				}
				else
				{
					temporaryRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);
				}

				context.SetInstruction2(X64.XChg64, temporaryRegister, value, value, temporaryRegister);
				context.AppendInstruction(X64.MovStore16, null, dest, offset, temporaryRegister);
				context.AppendInstruction2(X64.XChg64, value, temporaryRegister, temporaryRegister, value);
			}
		}

		public void MovStore8(Context context)
		{
			var value = context.Operand3;

			if (value.IsCPURegister && (value.Register == GeneralPurposeRegister.ESI || value.Register == GeneralPurposeRegister.EDI))
			{
				var dest = context.Operand1;
				var offset = context.Operand2;

				Operand temporaryRegister = null;

				if (dest.Register != GeneralPurposeRegister.EAX && offset.Register != GeneralPurposeRegister.EAX)
				{
					temporaryRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
				}
				else if (dest.Register != GeneralPurposeRegister.EBX && offset.Register != GeneralPurposeRegister.EBX)
				{
					temporaryRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EBX);
				}
				else if (dest.Register != GeneralPurposeRegister.ECX && offset.Register != GeneralPurposeRegister.ECX)
				{
					temporaryRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ECX);
				}
				else
				{
					temporaryRegister = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EDX);
				}

				context.SetInstruction2(X64.XChg64, temporaryRegister, value, value, temporaryRegister);
				context.AppendInstruction(X64.MovStore8, null, dest, offset, temporaryRegister);
				context.AppendInstruction2(X64.XChg64, value, temporaryRegister, temporaryRegister, value);
			}
		}

		public void Movzx16To32(Context context)
		{
			Debug.Assert(context.Result.IsCPURegister);

			// Movzx8To32 can not use with ESI or EDI registers
			if (context.Operand1.Register != GeneralPurposeRegister.ESI && context.Operand1.Register != GeneralPurposeRegister.EDI)
				return;

			var result = context.Result;
			var source = context.Operand1;

			if (source.Register != result.Register)
			{
				context.SetInstruction(X64.Mov32, result, source);
				context.AppendInstruction(X64.And32, result, result, CreateConstant32(0xFFFF));
			}
			else
			{
				context.SetInstruction(X64.And32, result, result, CreateConstant32(0xFFFF));
			}
		}

		public void Movzx8To32(Context context)
		{
			Debug.Assert(context.Result.IsCPURegister);

			// Movzx8To32 can not use with ESI or EDI registers
			if (context.Operand1.Register != GeneralPurposeRegister.ESI && context.Operand1.Register != GeneralPurposeRegister.EDI)
				return;

			var result = context.Result;
			var source = context.Operand1;

			if (source.Register != result.Register)
			{
				context.SetInstruction(X64.Mov32, result, source);
				context.AppendInstruction(X64.And32, result, result, CreateConstant32(0xFF));
			}
			else
			{
				context.SetInstruction(X64.And32, result, result, CreateConstant32(0xFF));
			}
		}

		public void Nop(Context context)
		{
			context.Empty();
		}

		public void Setcc(Context context)
		{
			Debug.Assert(context.Result.IsCPURegister);

			var result = context.Result;
			var instruction = context.Instruction;

			Debug.Assert(result.IsCPURegister);

			// SETcc can not use with ESI or EDI registers
			if (result.Register == GeneralPurposeRegister.ESI || result.Register == GeneralPurposeRegister.EDI)
			{
				var condition = context.ConditionCode;

				var eax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);

				context.SetInstruction2(X64.XChg64, eax, result, result, eax);
				context.AppendInstruction(instruction, condition, eax);
				context.AppendInstruction2(X64.XChg64, result, eax, eax, result);
			}
		}

		#endregion Visitation Methods
	}
}
