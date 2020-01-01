// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Platform.Intel;
using System.Diagnostics;

namespace Mosa.Platform.x86.Stages
{
	/// <summary>
	/// Final Tweak Transformation Stage
	/// </summary>
	/// <seealso cref="Mosa.Platform.x86.BaseTransformationStage" />
	public sealed class FinalTweakStage : BaseTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(X86.Call, CallReg);
			AddVisitation(X86.Mov32, Mov32);
			AddVisitation(X86.MovLoad16, MovLoad16);
			AddVisitation(X86.MovLoad8, MovLoad8);
			AddVisitation(X86.Movsd, Movsd);
			AddVisitation(X86.Movss, Movss);
			AddVisitation(X86.MovStore16, MovStore16);
			AddVisitation(X86.MovStore8, MovStore8);
			AddVisitation(X86.Movzx16To32, Movzx16To32);
			AddVisitation(X86.Movsx16To32, Movsx16To32);
			AddVisitation(X86.Movzx8To32, Movzx8To32);
			AddVisitation(X86.Movsx8To32, Movsx8To32);
			AddVisitation(X86.Setcc, Setcc);
		}

		#region Visitation Methods

		public void CallReg(Context context)
		{
			Debug.Assert(context.Operand1 != null);
			Debug.Assert(context.Operand1.IsCPURegister);

			var before = context.Node.Previous;

			while (before.IsEmpty && !before.IsBlockStartInstruction)
			{
				before = before.Previous;
			}

			if (before == null || before.IsBlockStartInstruction)
				return;

			if (before.Instruction != X86.Mov32)
				return;

			if (!before.Result.IsCPURegister)
				return;

			if (context.Operand1.Register != before.Result.Register)
				return;

			before.SetInstruction(X86.Call, null, before.Operand1);
			context.Empty();
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

		public void MovLoad16(Context context)
		{
			var result = context.Result;

			Debug.Assert(result.IsCPURegister);

			if (result.Register == GeneralPurposeRegister.ESI || result.Register == GeneralPurposeRegister.EDI)
			{
				var source = context.Operand1;
				var offset = context.Operand2;

				context.SetInstruction(X86.MovLoad32, result, source, offset);
				context.AppendInstruction(X86.And32, result, result, CreateConstant(0x0000FFFF));
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

				context.SetInstruction(X86.MovLoad32, result, source, offset);
				context.AppendInstruction(X86.And32, result, result, CreateConstant(0x000000FF));
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

				Operand temporaryRegister;

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

				context.SetInstruction2(X86.XChg32, temporaryRegister, value, value, temporaryRegister);
				context.AppendInstruction(X86.MovStore16, null, dest, offset, temporaryRegister);
				context.AppendInstruction2(X86.XChg32, value, temporaryRegister, temporaryRegister, value);
			}
		}

		public void MovStore8(Context context)
		{
			var value = context.Operand3;

			if (value.IsCPURegister && (value.Register == GeneralPurposeRegister.ESI || value.Register == GeneralPurposeRegister.EDI))
			{
				var dest = context.Operand1;
				var offset = context.Operand2;

				Operand temporaryRegister;

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

				context.SetInstruction2(X86.XChg32, temporaryRegister, value, value, temporaryRegister);
				context.AppendInstruction(X86.MovStore8, null, dest, offset, temporaryRegister);
				context.AppendInstruction2(X86.XChg32, value, temporaryRegister, temporaryRegister, value);
			}
		}

		public void Movzx16To32(Context context)
		{
			Debug.Assert(context.Result.IsCPURegister);

			var result = context.Result;
			var source = context.Operand1;

			// Movzx8To32 can not use with ESI or EDI registers as source registers
			if (result.Register != GeneralPurposeRegister.ESI && result.Register != GeneralPurposeRegister.EDI)
				return;

			if (source.Register == result.Register)
			{
				context.SetInstruction(X86.And32, result, result, CreateConstant(0xFFFF));
			}
			else
			{
				context.SetInstruction(X86.Mov32, result, source);
				context.AppendInstruction(X86.And32, result, result, CreateConstant(0xFFFF));
			}
		}

		public void Movsx16To32(Context context)
		{
			Debug.Assert(context.Result.IsCPURegister);

			var result = context.Result;
			var source = context.Operand1;

			// Movsx16To32 can not use with ESI or EDI registers as source registers
			if (source.Register != GeneralPurposeRegister.ESI && source.Register != GeneralPurposeRegister.EDI)
				return;

			var eax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);

			if (source.Register == result.Register)
			{
				context.SetInstruction2(X86.XChg32, eax, source, source, eax);
				context.AppendInstruction(X86.Movsx16To32, eax, eax);
				context.AppendInstruction2(X86.XChg32, source, eax, eax, source);
			}
			else
			{
				context.SetInstruction2(X86.XChg32, eax, source, source, eax);
				context.AppendInstruction(X86.Movsx16To32, result, eax);
				context.AppendInstruction2(X86.XChg32, source, eax, eax, source);
			}
		}

		public void Movzx8To32(Context context)
		{
			Debug.Assert(context.Result.IsCPURegister);

			var result = context.Result;
			var source = context.Operand1;

			// Movzx8To32 can not use with ESI or EDI registers as source registers
			if (source.Register != GeneralPurposeRegister.ESI && source.Register != GeneralPurposeRegister.EDI)
				return;

			if (source.Register == result.Register)
			{
				context.SetInstruction(X86.And32, result, result, CreateConstant(0xFF));
			}
			else
			{
				context.SetInstruction(X86.Mov32, result, source);
				context.AppendInstruction(X86.And32, result, result, CreateConstant(0xFF));
			}
		}

		public void Movsx8To32(Context context)
		{
			Debug.Assert(context.Result.IsCPURegister);

			var result = context.Result;
			var source = context.Operand1;

			// Movsx8To32 can not use with ESI or EDI registers as source registers
			if (source.Register != GeneralPurposeRegister.ESI && source.Register != GeneralPurposeRegister.EDI)
				return;

			var eax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);

			if (source.Register == result.Register)
			{
				context.SetInstruction2(X86.XChg32, eax, source, source, eax);
				context.AppendInstruction(X86.Movsx8To32, eax, eax);
				context.AppendInstruction2(X86.XChg32, source, eax, eax, source);
			}
			else
			{
				context.SetInstruction2(X86.XChg32, eax, source, source, eax);
				context.AppendInstruction(X86.Movsx8To32, result, eax);
				context.AppendInstruction2(X86.XChg32, source, eax, eax, source);
			}
		}

		public void Setcc(Context context)
		{
			Debug.Assert(context.Result.IsCPURegister);

			var result = context.Result;
			var instruction = context.Instruction;

			Debug.Assert(result.IsCPURegister);

			// SETcc can not use with ESI or EDI registers as source registers
			if (result.Register == GeneralPurposeRegister.ESI || result.Register == GeneralPurposeRegister.EDI)
			{
				var condition = context.ConditionCode;

				var eax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);

				context.SetInstruction2(X86.XChg32, eax, result, result, eax);
				context.AppendInstruction(instruction, condition, eax);
				context.AppendInstruction2(X86.XChg32, result, eax, eax, result);
			}
		}

		#endregion Visitation Methods
	}
}
