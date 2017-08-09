// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Transforms CIL instructions into their appropriate IR.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms CIL instructions into their equivalent IR sequences.
	/// </remarks>
	public sealed class CILTransformationStage : BaseCodeTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			visitationDictionary[CILInstruction.Add] = Add;
			visitationDictionary[CILInstruction.And] = BinaryLogic;
			visitationDictionary[CILInstruction.Beq] = BinaryBranch;
			visitationDictionary[CILInstruction.Beq_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Bge] = BinaryBranch;
			visitationDictionary[CILInstruction.Bge_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Bge_un] = BinaryBranch;
			visitationDictionary[CILInstruction.Bge_un_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Bgt] = BinaryBranch;
			visitationDictionary[CILInstruction.Bgt_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Bgt_un] = BinaryBranch;
			visitationDictionary[CILInstruction.Bgt_un_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Ble] = BinaryBranch;
			visitationDictionary[CILInstruction.Ble_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Ble_un] = BinaryBranch;
			visitationDictionary[CILInstruction.Ble_un_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Blt] = BinaryBranch;
			visitationDictionary[CILInstruction.Blt_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Blt_un] = BinaryBranch;
			visitationDictionary[CILInstruction.Blt_un_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Bne_un] = BinaryBranch;
			visitationDictionary[CILInstruction.Bne_un_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Box] = Box;
			visitationDictionary[CILInstruction.Br] = Branch;
			visitationDictionary[CILInstruction.Br_s] = Branch;
			visitationDictionary[CILInstruction.Break] = Break;
			visitationDictionary[CILInstruction.Brfalse] = UnaryBranch;
			visitationDictionary[CILInstruction.Brfalse_s] = UnaryBranch;
			visitationDictionary[CILInstruction.Brtrue] = UnaryBranch;
			visitationDictionary[CILInstruction.Brtrue_s] = UnaryBranch;
			visitationDictionary[CILInstruction.Call] = Call;
			visitationDictionary[CILInstruction.Calli] = Calli;
			visitationDictionary[CILInstruction.Callvirt] = Callvirt;
			visitationDictionary[CILInstruction.Castclass] = Castclass;
			visitationDictionary[CILInstruction.Ceq] = BinaryComparison;
			visitationDictionary[CILInstruction.Cgt] = BinaryComparison;
			visitationDictionary[CILInstruction.Cgt_un] = BinaryComparison;
			visitationDictionary[CILInstruction.Clt] = BinaryComparison;
			visitationDictionary[CILInstruction.Clt_un] = BinaryComparison;
			visitationDictionary[CILInstruction.Conv_i] = Conversion;
			visitationDictionary[CILInstruction.Conv_i1] = Conversion;
			visitationDictionary[CILInstruction.Conv_i2] = Conversion;
			visitationDictionary[CILInstruction.Conv_i4] = Conversion;
			visitationDictionary[CILInstruction.Conv_i8] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_i] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_i_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_i1] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_i1_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_i2] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_i2_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_i4] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_i4_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_i8] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_i8_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u1] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u1_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u2] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u2_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u4] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u4_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u8] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u8_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_r_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_r4] = Conversion;
			visitationDictionary[CILInstruction.Conv_r8] = Conversion;
			visitationDictionary[CILInstruction.Conv_u] = Conversion;
			visitationDictionary[CILInstruction.Conv_u1] = Conversion;
			visitationDictionary[CILInstruction.Conv_u2] = Conversion;
			visitationDictionary[CILInstruction.Conv_u4] = Conversion;
			visitationDictionary[CILInstruction.Conv_u8] = Conversion;
			visitationDictionary[CILInstruction.Cpblk] = Cpblk;
			visitationDictionary[CILInstruction.Div] = Div;
			visitationDictionary[CILInstruction.Div_un] = BinaryLogic;
			visitationDictionary[CILInstruction.Dup] = Dup;
			visitationDictionary[CILInstruction.Endfilter] = Endfilter;
			visitationDictionary[CILInstruction.Endfinally] = Endfinally;
			visitationDictionary[CILInstruction.Initblk] = Initblk;
			visitationDictionary[CILInstruction.InitObj] = InitObj;
			visitationDictionary[CILInstruction.Isinst] = IsInst;
			visitationDictionary[CILInstruction.Ldarg] = Ldarg;
			visitationDictionary[CILInstruction.Ldarg_0] = Ldarg;
			visitationDictionary[CILInstruction.Ldarg_1] = Ldarg;
			visitationDictionary[CILInstruction.Ldarg_2] = Ldarg;
			visitationDictionary[CILInstruction.Ldarg_3] = Ldarg;
			visitationDictionary[CILInstruction.Ldarg_s] = Ldarg;
			visitationDictionary[CILInstruction.Ldarga] = Ldarga;
			visitationDictionary[CILInstruction.Ldarga_s] = Ldarga;
			visitationDictionary[CILInstruction.Ldc_i4] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_0] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_1] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_2] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_3] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_4] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_5] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_6] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_7] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_8] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_m1] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_s] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i8] = Ldc;
			visitationDictionary[CILInstruction.Ldc_r4] = Ldc;
			visitationDictionary[CILInstruction.Ldc_r8] = Ldc;
			visitationDictionary[CILInstruction.Ldelem] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_i] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_i1] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_i2] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_i4] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_i8] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_r4] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_r8] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_ref] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_u1] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_u2] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_u4] = Ldelem;
			visitationDictionary[CILInstruction.Ldelema] = Ldelema;
			visitationDictionary[CILInstruction.Ldfld] = Ldfld;
			visitationDictionary[CILInstruction.Ldflda] = Ldflda;
			visitationDictionary[CILInstruction.Ldftn] = Ldftn;
			visitationDictionary[CILInstruction.Ldind_i] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_i1] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_i2] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_i4] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_i8] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_r4] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_r8] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_ref] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_u1] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_u2] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_u4] = Ldobj;
			visitationDictionary[CILInstruction.Ldlen] = Ldlen;
			visitationDictionary[CILInstruction.Ldloc] = Ldloc;
			visitationDictionary[CILInstruction.Ldloc_0] = Ldloc;
			visitationDictionary[CILInstruction.Ldloc_1] = Ldloc;
			visitationDictionary[CILInstruction.Ldloc_2] = Ldloc;
			visitationDictionary[CILInstruction.Ldloc_3] = Ldloc;
			visitationDictionary[CILInstruction.Ldloc_s] = Ldloc;
			visitationDictionary[CILInstruction.Ldloca] = Ldloca;
			visitationDictionary[CILInstruction.Ldloca_s] = Ldloca;
			visitationDictionary[CILInstruction.Ldnull] = Ldc;
			visitationDictionary[CILInstruction.Ldobj] = Ldobj;
			visitationDictionary[CILInstruction.Ldsfld] = Ldsfld;
			visitationDictionary[CILInstruction.Ldsflda] = Ldsflda;
			visitationDictionary[CILInstruction.Ldstr] = Ldstr;
			visitationDictionary[CILInstruction.Ldtoken] = Ldtoken;
			visitationDictionary[CILInstruction.Ldvirtftn] = Ldvirtftn;
			visitationDictionary[CILInstruction.Leave] = Leave;
			visitationDictionary[CILInstruction.Leave_s] = Leave;
			visitationDictionary[CILInstruction.Mul] = Mul;
			visitationDictionary[CILInstruction.Neg] = Neg;
			visitationDictionary[CILInstruction.Newarr] = Newarr;
			visitationDictionary[CILInstruction.Newobj] = Newobj;
			visitationDictionary[CILInstruction.Nop] = Nop;
			visitationDictionary[CILInstruction.Not] = Not;
			visitationDictionary[CILInstruction.Or] = BinaryLogic;
			visitationDictionary[CILInstruction.Pop] = Pop;
			visitationDictionary[CILInstruction.Rem] = Rem;
			visitationDictionary[CILInstruction.Rem_un] = BinaryLogic;
			visitationDictionary[CILInstruction.Ret] = Ret;
			visitationDictionary[CILInstruction.Rethrow] = Rethrow;
			visitationDictionary[CILInstruction.Shl] = Shift;
			visitationDictionary[CILInstruction.Shr] = Shift;
			visitationDictionary[CILInstruction.Shr_un] = Shift;
			visitationDictionary[CILInstruction.Sizeof] = Sizeof;
			visitationDictionary[CILInstruction.Starg] = Starg;
			visitationDictionary[CILInstruction.Starg_s] = Starg;
			visitationDictionary[CILInstruction.Stelem] = Stelem;
			visitationDictionary[CILInstruction.Stelem_i] = Stelem;
			visitationDictionary[CILInstruction.Stelem_i1] = Stelem;
			visitationDictionary[CILInstruction.Stelem_i2] = Stelem;
			visitationDictionary[CILInstruction.Stelem_i4] = Stelem;
			visitationDictionary[CILInstruction.Stelem_i8] = Stelem;
			visitationDictionary[CILInstruction.Stelem_r4] = Stelem;
			visitationDictionary[CILInstruction.Stelem_r8] = Stelem;
			visitationDictionary[CILInstruction.Stelem_ref] = Stelem;
			visitationDictionary[CILInstruction.Stfld] = Stfld;
			visitationDictionary[CILInstruction.Stind_i] = Stobj;
			visitationDictionary[CILInstruction.Stind_i1] = Stobj;
			visitationDictionary[CILInstruction.Stind_i2] = Stobj;
			visitationDictionary[CILInstruction.Stind_i4] = Stobj;
			visitationDictionary[CILInstruction.Stind_i8] = Stobj;
			visitationDictionary[CILInstruction.Stind_r4] = Stobj;
			visitationDictionary[CILInstruction.Stind_r8] = Stobj;
			visitationDictionary[CILInstruction.Stind_ref] = Stobj;
			visitationDictionary[CILInstruction.Stloc] = Stloc;
			visitationDictionary[CILInstruction.Stloc_0] = Stloc;
			visitationDictionary[CILInstruction.Stloc_1] = Stloc;
			visitationDictionary[CILInstruction.Stloc_2] = Stloc;
			visitationDictionary[CILInstruction.Stloc_3] = Stloc;
			visitationDictionary[CILInstruction.Stloc_s] = Stloc;
			visitationDictionary[CILInstruction.Stobj] = Stobj;
			visitationDictionary[CILInstruction.Stsfld] = Stsfld;
			visitationDictionary[CILInstruction.Sub] = Sub;
			visitationDictionary[CILInstruction.Switch] = Switch;
			visitationDictionary[CILInstruction.Throw] = Throw;
			visitationDictionary[CILInstruction.Unbox] = Unbox;
			visitationDictionary[CILInstruction.Unbox_any] = UnboxAny;
			visitationDictionary[CILInstruction.Xor] = BinaryLogic;

			//visitationDictionary[CILInstruction.Add_ovf] = Add_ovf;
			//visitationDictionary[CILInstruction.Add_ovf_un] = Add_ovf_un;
			//visitationDictionary[CILInstruction.Arglist] = Arglist;
			//visitationDictionary[CILInstruction.Ckfinite] = Ckfinite;
			//visitationDictionary[CILInstruction.Cpobj] = Cpobj;
			//visitationDictionary[CILInstruction.Jmp] = Jmp;
			//visitationDictionary[CILInstruction.Localalloc] = Localalloc;
			//visitationDictionary[CILInstruction.Mkrefany] = Mkrefany;
			//visitationDictionary[CILInstruction.Mul_ovf] = Mul_ovf;
			//visitationDictionary[CILInstruction.Mul_ovf_un] = Mul_ovf_un;
			//visitationDictionary[CILInstruction.PreConstrained] = PreConstrained;
			//visitationDictionary[CILInstruction.PreNo] = PreNo;
			//visitationDictionary[CILInstruction.PreReadOnly] = PreReadOnly;
			//visitationDictionary[CILInstruction.PreTail] = PreTail;
			//visitationDictionary[CILInstruction.PreUnaligned] = PreUnaligned;
			//visitationDictionary[CILInstruction.PreVolatile] = PreVolatile;
			//visitationDictionary[CILInstruction.Refanytype] = Refanytype;
			//visitationDictionary[CILInstruction.Refanyval] = Refanyval;
			//visitationDictionary[CILInstruction.Sub_ovf] = Sub_ovf;
			//visitationDictionary[CILInstruction.Sub_ovf_un] = Sub_ovf_un;
		}

		#region Visitation Methods

		/// <summary>
		/// Visitation function for Add instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Add(Context context)
		{
			Replace(context, IRInstruction.AddFloatR4, IRInstruction.AddFloatR8, IRInstruction.AddSigned, IRInstruction.AddUnsigned);
		}

		/// <summary>
		/// Visitation function for BinaryBranch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void BinaryBranch(Context context)
		{
			var target = context.BranchTargets[0];

			var cc = ConvertCondition(((BaseCILInstruction)context.Instruction).OpCode);
			var first = context.Operand1;
			var second = context.Operand2;

			if (first.IsR)
			{
				var result = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
				var instruction = (first.IsR4) ? (BaseInstruction)IRInstruction.CompareFloatR4 : IRInstruction.CompareFloatR8;

				context.SetInstruction(instruction, cc, result, first, second);
				context.AppendInstruction(IRInstruction.CompareIntegerBranch, ConditionCode.Equal, null, result, Operand.CreateConstant(TypeSystem, 1));
			}
			else
			{
				context.SetInstruction(IRInstruction.CompareIntegerBranch, cc, null, first, second);
			}

			context.AddBranchTarget(target);
		}

		/// <summary>
		/// Visitation function for BinaryComparison instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void BinaryComparison(Context context)
		{
			var code = ConvertCondition((context.Instruction as BaseCILInstruction).OpCode);
			var first = context.Operand1;
			var second = context.Operand2;
			var result = context.Result;

			BaseInstruction instruction = IRInstruction.CompareInteger;
			if (first.IsR4)
				instruction = IRInstruction.CompareFloatR4;
			else if (first.IsR8)
				instruction = IRInstruction.CompareFloatR8;

			context.SetInstruction(instruction, code, result, first, second);
		}

		/// <summary>
		/// Visitation function for BinaryLogic instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void BinaryLogic(Context context)
		{
			if (context.Operand1.Type.IsEnum)
			{
				var type = context.Operand1.Type;
				var operand = Operand.CreateField(type.Fields[0]);
				context.SetOperand(0, operand);
			}

			if (context.Operand2.Type.IsEnum)
			{
				var type = context.Operand2.Type;
				var operand = Operand.CreateField(type.Fields[0]);
				context.SetOperand(1, operand);
			}

			switch ((context.Instruction as BaseCILInstruction).OpCode)
			{
				case OpCode.And: context.SetInstruction(IRInstruction.LogicalAnd, context.Result, context.Operand1, context.Operand2); break;
				case OpCode.Or: context.SetInstruction(IRInstruction.LogicalOr, context.Result, context.Operand1, context.Operand2); break;
				case OpCode.Xor: context.SetInstruction(IRInstruction.LogicalXor, context.Result, context.Operand1, context.Operand2); break;
				case OpCode.Div_un: context.SetInstruction(IRInstruction.DivUnsigned, context.Result, context.Operand1, context.Operand2); break;
				case OpCode.Rem_un: context.SetInstruction(IRInstruction.RemUnsigned, context.Result, context.Operand1, context.Operand2); break;
				default: throw new InvalidCompilerException();
			}
		}

		/// <summary>
		/// Visitation function for Box instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Box(Context context)
		{
			var value = context.Operand1;
			var result = context.Result;
			var type = context.MosaType;

			if (!type.IsValueType)
			{
				Debug.Assert(result.IsVirtualRegister);
				Debug.Assert(value.IsVirtualRegister);

				var moveInstruction = GetMoveInstruction(type);
				context.ReplaceInstructionOnly(moveInstruction);
				return;
			}

			int typeSize = TypeLayout.GetTypeSize(type);

			typeSize = Alignment.AlignUp(typeSize, TypeLayout.NativePointerAlignment);

			var vmCall = VmCall.Box32;

			if (type.IsR4)
				vmCall = VmCall.BoxR4;
			else if (type.IsR8)
				vmCall = VmCall.BoxR8;
			else if (typeSize <= 4)
				vmCall = VmCall.Box32;
			else if (typeSize == 8)
				vmCall = VmCall.Box64;
			else
				vmCall = VmCall.Box;

			context.SetInstruction(IRInstruction.Nop);
			ReplaceWithVmCall(context, vmCall);

			context.SetOperand(1, GetRuntimeTypeHandle(type));

			if (vmCall == VmCall.Box)
			{
				var adr = AllocateVirtualRegister(type.ToManagedPointer());
				context.InsertBefore().SetInstruction(IRInstruction.AddressOf, adr, value);

				context.SetOperand(2, adr);
				context.SetOperand(3, Operand.CreateConstant(TypeSystem, typeSize));
				context.OperandCount = 4;
			}
			else
			{
				context.SetOperand(2, value);
				context.OperandCount = 3;
			}
			context.Result = result;
			context.ResultCount = 1;
		}

		/// <summary>
		/// Visitation function for Branch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Branch(Context context)
		{
			context.ReplaceInstructionOnly(IRInstruction.Jmp);
		}

		/// <summary>
		/// Visitation function for Break instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Break(Context context)
		{
			context.SetInstruction(IRInstruction.Break);
		}

		private int CalculateInterfaceSlot(MosaType interaceType)
		{
			return TypeLayout.GetInterfaceSlotOffset(interaceType);
		}

		private int CalculateInterfaceSlotOffset(MosaMethod invokeTarget)
		{
			return CalculateInterfaceSlot(invokeTarget.DeclaringType) * NativePointerSize;
		}

		private int CalculateMethodTableOffset(MosaMethod invokeTarget)
		{
			int slot = TypeLayout.GetMethodTableOffset(invokeTarget);

			return NativePointerSize * slot;
		}

		/// <summary>
		/// Visitation function for Call instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Call(Context context)
		{
			if (CanSkipDueToRecursiveSystemObjectCtorCall(context))
			{
				context.Empty();
				return;
			}

			if (ProcessExternalCall(context))
				return;

			// If the method being called is a virtual method then we need to box the value type
			if (context.InvokeMethod.IsVirtual
				&& context.Operand1.Type.ElementType != null
				&& context.Operand1.Type.ElementType.IsValueType
				&& context.InvokeMethod.DeclaringType == context.Operand1.Type.ElementType)
			{
				if (OverridesMethod(context.InvokeMethod))
				{
					var before = context.InsertBefore();
					before.SetInstruction(IRInstruction.SubSigned, context.Operand1, context.Operand1, Operand.CreateConstant(TypeSystem, NativePointerSize * 2));
				}
				else
				{
					// Get the value type, size and native alignment
					var type = context.Operand1.Type.ElementType;
					int typeSize = TypeLayout.GetTypeSize(type);
					int alignment = TypeLayout.NativePointerAlignment;

					typeSize = Alignment.AlignUp(typeSize, alignment);

					// Create a virtual register to hold our boxed value
					var boxedValue = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);

					// Create a new context before the call and set it as a VmCall
					var before = context.InsertBefore();
					before.SetInstruction(IRInstruction.Nop);
					ReplaceWithVmCall(before, VmCall.Box);

					// Populate the operands for the VmCall and result
					before.SetOperand(1, GetRuntimeTypeHandle(type));
					before.SetOperand(2, context.Operand1);
					before.SetOperand(3, Operand.CreateConstant(TypeSystem, typeSize));
					before.OperandCount = 4;
					before.Result = boxedValue;
					before.ResultCount = 1;

					// Now replace the value type pointer with the boxed value virtual register
					context.Operand1 = boxedValue;
				}
			}

			ProcessInvokeInstruction(context, context.InvokeMethod, context.Result, new List<Operand>(context.Operands));
		}

		/// <summary>
		/// Visitation function for Calli instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Calli(Context context)
		{
			//todo: not yet implemented
			throw new NotImplementCompilerException();

			//var destinationOperand = context.GetOperand(context.OperandCount - 1);
			//context.OperandCount--;
			//ProcessInvokeInstruction(context, context.InvokeMethod, context.Result, new List<Operand>(context.Operands));
		}

		/// <summary>
		/// Visitation function for Callvirt instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Callvirt(Context context)
		{
			if (ProcessExternalCall(context))
				return;

			var method = context.InvokeMethod;
			var resultOperand = context.Result;

			var operands = new List<Operand>(context.Operands);

			if (context.Previous.Instruction is ConstrainedPrefixInstruction)
			{
				var type = context.Previous.MosaType;

				context.Previous.Empty();

				if (type.IsValueType)
				{
					method = GetMethodOrOverride(type, method);

					if (type.Methods.Contains(method))
					{
						// If the method being called is a virtual method then we need to box the value type
						if (method.IsVirtual
							&& context.Operand1.Type.ElementType != null
							&& context.Operand1.Type.ElementType.IsValueType
							&& method.DeclaringType == context.Operand1.Type.ElementType)
						{
							var before = context.InsertBefore();
							before.SetInstruction(IRInstruction.SubSigned, context.Operand1, context.Operand1, Operand.CreateConstant(TypeSystem, NativePointerSize * 2));
						}
					}
					else
					{
						// Get the value type, size and native alignment
						var elementType = context.Operand1.Type.ElementType;
						int typeSize = TypeLayout.GetTypeSize(elementType);
						int alignment = TypeLayout.NativePointerAlignment;

						typeSize = Alignment.AlignUp(typeSize, alignment);

						// Create a virtual register to hold our boxed value
						var boxedValue = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);

						// Create a new context before the call and set it as a VmCall
						var before = context.InsertBefore();
						before.SetInstruction(IRInstruction.Nop);
						ReplaceWithVmCall(before, VmCall.Box);

						// Populate the operands for the VmCall and result
						before.SetOperand(1, GetRuntimeTypeHandle(elementType));
						before.SetOperand(2, context.Operand1);
						before.SetOperand(3, Operand.CreateConstant(TypeSystem, typeSize));
						before.OperandCount = 4;
						before.Result = boxedValue;
						before.ResultCount = 1;

						// Now replace the value type pointer with the boxed value virtual register
						context.Operand1 = boxedValue;
					}

					ProcessInvokeInstruction(context, method, resultOperand, operands);
					return;
				}
			}

			if (method.IsVirtual)
			{
				var thisPtr = context.Operand1;

				var typeDefinition = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);
				var methodPtr = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);

				if (!method.DeclaringType.IsInterface)
				{
					// Same as above except for methodPointer
					int methodPointerOffset = CalculateMethodTableOffset(method) + (NativePointerSize * 14);

					// Get the TypeDef pointer
					context.SetInstruction(IRInstruction.LoadInteger, NativeInstructionSize, typeDefinition, thisPtr, ConstantZero);

					// Get the address of the method
					context.AppendInstruction(IRInstruction.LoadInteger, NativeInstructionSize, methodPtr, typeDefinition, Operand.CreateConstant(TypeSystem, methodPointerOffset));
				}
				else
				{
					var methodDefinition = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);

					// Offset for InterfaceSlotTable in TypeDef
					int interfaceSlotTableOffset = (NativePointerSize * 11);

					// Offset for InterfaceMethodTable in InterfaceSlotTable
					int interfaceMethodTableOffset = (NativePointerSize * 1) + CalculateInterfaceSlotOffset(method);

					// Offset for MethodDef in InterfaceMethodTable
					int methodDefinitionOffset = (NativePointerSize * 2) + CalculateMethodTableOffset(method);

					// Offset for Method pointer in MethodDef
					int methodPointerOffset = (NativePointerSize * 4);

					// Operands to hold pointers
					var interfaceSlotPtr = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);
					var interfaceMethodTablePtr = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);

					// Get the TypeDef pointer
					context.SetInstruction(IRInstruction.LoadInteger, NativeInstructionSize, typeDefinition, thisPtr, ConstantZero);

					// Get the Interface Slot Table pointer
					context.AppendInstruction(IRInstruction.LoadInteger, NativeInstructionSize, interfaceSlotPtr, typeDefinition, Operand.CreateConstant(TypeSystem, interfaceSlotTableOffset));

					// Get the Interface Method Table pointer
					context.AppendInstruction(IRInstruction.LoadInteger, NativeInstructionSize, interfaceMethodTablePtr, interfaceSlotPtr, Operand.CreateConstant(TypeSystem, interfaceMethodTableOffset));

					// Get the MethodDef pointer
					context.AppendInstruction(IRInstruction.LoadInteger, NativeInstructionSize, methodDefinition, interfaceMethodTablePtr, Operand.CreateConstant(TypeSystem, methodDefinitionOffset));

					// Get the address of the method
					context.AppendInstruction(IRInstruction.LoadInteger, NativeInstructionSize, methodPtr, methodDefinition, Operand.CreateConstant(TypeSystem, methodPointerOffset));
				}

				context.AppendInstruction(IRInstruction.Nop);
				ProcessInvokeInstruction(context, method, methodPtr, resultOperand, operands);
			}
			else
			{
				// FIXME: Callvirt imposes a null-check. For virtual calls this is done implicitly, but for non-virtual calls
				// we have to make this explicitly somehow.
				ProcessInvokeInstruction(context, method, resultOperand, operands);
			}
		}

		/// <summary>
		/// Visitation function for Castclass instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Castclass(Context context)
		{
			// TODO!
			//ReplaceWithVmCall(context, VmCall.Castclass);
			context.ReplaceInstructionOnly(IRInstruction.MoveInteger); // HACK!
		}

		private static BaseInstruction ComputeExtensionTypeAndMask(MosaType type, ref uint mask)
		{
			if (type.IsUI1)
			{
				mask = 0xFF;
				return type.IsSigned ? (BaseInstruction)IRInstruction.MoveSignExtended : IRInstruction.MoveZeroExtended;
			}
			else if (type.IsUI2)
			{
				mask = 0xFFFF;
				return type.IsSigned ? (BaseInstruction)IRInstruction.MoveSignExtended : IRInstruction.MoveZeroExtended;
			}
			else if (type.IsUI4)
			{
				mask = 0xFFFFFFFF;
			}
			else if (type.IsUI8)
			{
				mask = 0x0;
			}

			return null;
		}

		/// <summary>
		/// Visitation function for Conversion instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Conversion(Context context)
		{
			var result = context.Result;
			var source = context.Operand1;

			int destIndex = GetIndex(result.Type, NativeInstructionSize == InstructionSize.Size32);
			int srcIndex = GetIndex(source.Type, NativeInstructionSize == InstructionSize.Size32);

			var instruction = convTable[destIndex][srcIndex];
			var size = GetInstructionSize(result.Type);

			Debug.Assert(instruction != null);

			uint mask = 0xFFFFFFFF;
			ComputeExtensionTypeAndMask(result.Type, ref mask);

			if (instruction == IRInstruction.ConversionIntegerToFloatR8 && result.IsR4)
			{
				context.SetInstruction(IRInstruction.ConversionIntegerToFloatR4, size, result, source);
				return;
			}

			if (instruction != IRInstruction.LogicalAnd)
			{
				context.SetInstruction(instruction, size, result, source);
				return;
			}

			if (mask == 0)
			{
				Debug.Assert(result.IsInteger);

				// TODO: May not be correct
				context.SetInstruction(IRInstruction.MoveInteger, size, result, source);
				return;
			}

			if (source.IsLong)
			{
				var temp = AllocateVirtualRegister(result.Type);

				context.SetInstruction(IRInstruction.MoveInteger, size, temp, source);
				context.AppendInstruction(instruction, size, result, temp, Operand.CreateConstant(TypeSystem, (int)mask));
				return;
			}

			context.SetInstruction(instruction, size, result, source, Operand.CreateConstant(TypeSystem, (int)mask));
		}

		/// <summary>
		/// Visitation function for Cpblk instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Cpblk(Context context)
		{
			ReplaceWithVmCall(context, VmCall.MemoryCopy);
		}

		/// <summary>
		/// Visitation function for Div instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Div(Context context)
		{
			Replace(context, IRInstruction.DivFloatR4, IRInstruction.DivFloatR8, IRInstruction.DivSigned, IRInstruction.DivUnsigned);
		}

		/// <summary>
		/// Visitation function for Dup instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Dup(Context context)
		{
			Debug.Assert(false); // should never get here

			// We don't need the dup anymore.
			context.Empty();
		}

		/// <summary>
		/// Visitation function for Endfilter instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Endfilter(Context context)
		{
			throw new InvalidCompilerException();

			// Move this transformation to ProtectedRegionStage
			//context.SetInstruction(IRInstruction.FilterEnd, context.Operand1);
		}

		/// <summary>
		/// Visitation function for Endfinally instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Endfinally(Context context)
		{
			throw new InvalidCompilerException();
		}

		private MosaMethod GetMethodOrOverride(MosaType type, MosaMethod method)
		{
			MosaMethod implMethod = null;

			if (type.Methods.Contains(method)
				&& (implMethod = type.FindMethodBySignature(method.Name, method.Signature)) != null)
			{
				return implMethod;
			}

			if (method.DeclaringType.Module == TypeSystem.CorLib
				&& (method.DeclaringType.Name.Equals("ValueType")
					|| method.DeclaringType.Name.Equals("Object")
					|| method.DeclaringType.Name.Equals("Enum"))
				&& (implMethod = type.FindMethodBySignature(method.Name, method.Signature)) != null)
			{
				return implMethod;
			}

			return method;
		}

		private Operand GetRuntimeTypeHandle(MosaType runtimeType)
		{
			return Operand.CreateSymbol(TypeSystem.GetTypeByName("System", "RuntimeTypeHandle"), runtimeType.FullName + Metadata.TypeDefinition);
		}

		private Operand GetRuntimeTypeHandle(MosaType runtimeType)
		{
			return Operand.CreateSymbol(TypeSystem.GetTypeByName("System", "RuntimeTypeHandle"), runtimeType.FullName + Metadata.TypeDefinition);
		}

		/// <summary>
		/// Visitation function for Initblk instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Initblk(Context context)
		{
			ReplaceWithVmCall(context, VmCall.MemorySet);
		}

		/// <summary>
		/// Visitation function for InitObj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void InitObj(Context context)
		{
			// Get the ptr and clear context
			var ptr = context.Operand1;

			// According to ECMA Spec, if the pointer element type is a reference type then
			// this instruction is the equivalent of ldnull followed by stind.ref

			var type = ptr.Type.ElementType;
			if (type.IsReferenceType)
			{
				var size = GetInstructionSize(type);
				context.SetInstruction(IRInstruction.StoreInteger, size, null, ptr, ConstantZero, Operand.GetNull(TypeSystem));
				context.MosaType = type;
			}
			else
			{
				context.SetInstruction(IRInstruction.Nop);

				// Setup context for VmCall
				ReplaceWithVmCall(context, VmCall.MemorySet);

				// Set the operands
				context.SetOperand(1, ptr);
				context.SetOperand(2, ConstantZero);
				context.SetOperand(3, Operand.CreateConstant(TypeSystem, TypeLayout.GetTypeSize(type)));
				context.OperandCount = 4;
			}
		}

		/// <summary>
		/// Visitation function for Isinst instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void IsInst(Context context)
		{
			var reference = context.Operand1;
			var result = context.Result;

			var classType = context.MosaType;

			if (!classType.IsInterface)
			{
				ReplaceWithVmCall(context, VmCall.IsInstanceOfType);

				context.SetOperand(1, GetRuntimeTypeHandle(classType));
				context.SetOperand(2, reference);
				context.OperandCount = 3;
				context.ResultCount = 1;
			}
			else
			{
				int slot = CalculateInterfaceSlot(classType);

				ReplaceWithVmCall(context, VmCall.IsInstanceOfInterfaceType);

				context.SetOperand(1, Operand.CreateConstant(TypeSystem, slot));
				context.SetOperand(2, reference);
				context.OperandCount = 3;
				context.ResultCount = 1;
			}
		}

		/// <summary>
		/// Visitation function for Ldarg instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldarg(Context context)
		{
			Debug.Assert(context.Operand1.IsParameter);

			if (MosaTypeLayout.IsStoredOnStack(context.Operand1.Type))
			{
				context.SetInstruction(IRInstruction.LoadParameterCompound, context.Result, context.Operand1);
				context.MosaType = context.Operand1.Type;
			}
			else
			{
				var loadInstruction = GetLoadParameterInstruction(context.Operand1.Type);
				var size = GetInstructionSize(context.Operand1.Type);

				context.SetInstruction(loadInstruction, size, context.Result, context.Operand1);
			}
		}

		/// <summary>
		/// Visitation function for Ldarga instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldarga(Context context)
		{
			context.ReplaceInstructionOnly(IRInstruction.AddressOf);
		}

		/// <summary>
		/// Visitation function for Ldc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldc(Context context)
		{
			Debug.Assert(context.Operand1.IsConstant || context.Operand1.IsVirtualRegister);

			var source = context.Operand1;
			var destination = context.Result;
			var size = GetInstructionSize(source.Type);

			Debug.Assert(!MosaTypeLayout.IsStoredOnStack(destination.Type));
			var moveInstruction = GetMoveInstruction(destination.Type);
			context.SetInstruction(moveInstruction, size, destination, source);
		}

		/// <summary>
		/// Visitation function for Ldelem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldelem(Context context)
		{
			var result = context.Result;
			var array = context.Operand1;
			var arrayIndex = context.Operand2;
			var arrayType = array.Type;

			// Array bounds check
			AddArrayBoundsCheck(context, array, arrayIndex);

			var arrayAddress = LoadArrayBaseAddress(context, array);
			var elementOffset = CalculateArrayElementOffset(context, arrayType, arrayIndex);

			Debug.Assert(elementOffset != null);

			if (MosaTypeLayout.IsStoredOnStack(arrayType.ElementType))
			{
				context.SetInstruction(IRInstruction.LoadCompound, result, arrayAddress, elementOffset);
				context.MosaType = arrayType.ElementType;
			}
			else
			{
				var loadInstruction = GetLoadInstruction(arrayType.ElementType);
				var size = GetInstructionSize(arrayType.ElementType);

				context.SetInstruction(loadInstruction, size, result, arrayAddress, elementOffset);
			}
		}

		/// <summary>
		/// Visitation function for Ldelema instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldelema(Context context)
		{
			var result = context.Result;
			var array = context.Operand1;
			var arrayIndex = context.Operand2;
			var arrayType = array.Type;

			Debug.Assert(arrayType.ElementType == result.Type.ElementType);

			// Array bounds check
			AddArrayBoundsCheck(context, array, arrayIndex);

			var arrayAddress = LoadArrayBaseAddress(context, array);
			var elementOffset = CalculateArrayElementOffset(context, arrayType, arrayIndex);

			context.SetInstruction(IRInstruction.AddSigned, result, arrayAddress, elementOffset);
		}

		/// <summary>
		/// Visitation function for Ldfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldfld(Context context)
		{
			var result = context.Result;
			var operand = context.Operand1;
			var field = context.MosaField;

			int offset = TypeLayout.GetFieldOffset(field);
			bool isPointer = operand.IsPointer || operand.Type == TypeSystem.BuiltIn.I || operand.Type == TypeSystem.BuiltIn.U;

			if (!result.IsOnStack && !MosaTypeLayout.IsStoredOnStack(operand.Type) && !operand.IsReferenceType && isPointer)
			{
				var loadInstruction = GetLoadInstruction(field.FieldType);
				var size = GetInstructionSize(field.FieldType);
				var fixedOffset = Operand.CreateConstant(TypeSystem, offset);

				context.SetInstruction(loadInstruction, size, result, operand, fixedOffset);

				return;
			}

			if (!result.IsOnStack && !MosaTypeLayout.IsStoredOnStack(operand.Type) && !operand.IsReferenceType && !isPointer)
			{
				// simple move
				Debug.Assert(result.IsVirtualRegister);

				var moveInstruction = GetMoveInstruction(field.FieldType);
				var size = GetInstructionSize(field.FieldType);

				context.SetInstruction(moveInstruction, size, result, operand);

				return;
			}

			if (!MosaTypeLayout.IsStoredOnStack(result.Type) && operand.IsOnStack)
			{
				var loadInstruction = GetLoadInstruction(field.FieldType);
				var size = GetInstructionSize(field.FieldType);
				var address = MethodCompiler.CreateVirtualRegister(operand.Type.ToUnmanagedPointer());
				var fixedOffset = Operand.CreateConstant(TypeSystem, offset);

				context.SetInstruction(IRInstruction.AddressOf, address, operand);
				context.AppendInstruction(loadInstruction, size, result, address, fixedOffset);

				return;
			}

			if (!MosaTypeLayout.IsStoredOnStack(result.Type) && !operand.IsOnStack)
			{
				var loadInstruction = GetLoadInstruction(field.FieldType);
				var size = GetInstructionSize(field.FieldType);
				var fixedOffset = Operand.CreateConstant(TypeSystem, offset);

				context.SetInstruction(loadInstruction, size, result, operand, fixedOffset);

				return;
			}

			if (result.IsOnStack && !operand.IsOnStack)
			{
				var size = GetInstructionSize(field.FieldType);
				var fixedOffset = Operand.CreateConstant(TypeSystem, offset);

				context.SetInstruction(IRInstruction.LoadCompound, size, result, operand, fixedOffset);
				context.MosaType = field.FieldType;

				return;
			}

			if (result.IsOnStack && operand.IsOnStack)
			{
				var size = GetInstructionSize(field.FieldType);
				var address = MethodCompiler.CreateVirtualRegister(operand.Type.ToUnmanagedPointer());
				var fixedOffset = Operand.CreateConstant(TypeSystem, offset);

				context.SetInstruction(IRInstruction.AddressOf, address, operand);
				context.AppendInstruction(IRInstruction.LoadCompound, size, result, address, fixedOffset);

				return;
			}

			throw new CompilerException("Error transforming CIL.Ldfld");
		}

		/// <summary>
		/// Visitation function for Ldflda instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldflda(Context context)
		{
			var fieldAddress = context.Result;
			var objectOperand = context.Operand1;

			int offset = TypeLayout.GetFieldOffset(context.MosaField);
			var fixedOffset = Operand.CreateConstant(TypeSystem, offset);

			context.SetInstruction(IRInstruction.AddUnsigned, fieldAddress, objectOperand, fixedOffset);
		}

		/// <summary>
		/// Visitation function for Ldftn instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldftn(Context context)
		{
			context.SetInstruction(IRInstruction.MoveInteger, context.Result, Operand.CreateSymbolFromMethod(TypeSystem, context.InvokeMethod));
		}

		/// <summary>
		/// Visitation function for Ldlen instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldlen(Context context)
		{
			var offset = Operand.CreateConstant(TypeSystem, NativePointerSize * 2);
			context.SetInstruction(IRInstruction.LoadInteger, InstructionSize.Size32, context.Result, context.Operand1, offset);
		}

		/// <summary>
		/// Visitation function for Ldloc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldloc(Context context)
		{
			Debug.Assert(context.MosaType == null);
			ProcessLoadInstruction(context);
		}

		/// <summary>
		/// Visitation function for Ldloca instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldloca(Context context)
		{
			context.ReplaceInstructionOnly(IRInstruction.AddressOf);
		}

		/// <summary>
		/// Visitation function for Ldobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldobj(Context context)
		{
			var destination = context.Result;
			var source = context.Operand1;

			var type = context.MosaType;

			// This is actually ldind.* and ldobj - the opcodes have the same meanings

			if (MosaTypeLayout.IsStoredOnStack(type))
			{
				context.SetInstruction(IRInstruction.LoadCompound, destination, source, ConstantZero);
			}
			else
			{
				var loadInstruction = GetLoadInstruction(type);
				var size = GetInstructionSize(type);

				context.SetInstruction(loadInstruction, size, destination, source, ConstantZero);
			}

			context.MosaType = type;
		}

		/// <summary>
		/// Visitation function for Ldsfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldsfld(Context context)
		{
			var fieldType = context.MosaField.FieldType;
			var destination = context.Result;

			var size = GetInstructionSize(fieldType);
			var fieldOperand = Operand.CreateField(context.MosaField);

			if (MosaTypeLayout.IsStoredOnStack(fieldType))
			{
				context.SetInstruction(IRInstruction.LoadCompound, destination, fieldOperand, ConstantZero);
				context.MosaType = fieldType;
			}
			else
			{
				var loadInstruction = GetLoadInstruction(fieldType);
				context.SetInstruction(loadInstruction, size, destination, fieldOperand, ConstantZero);
				context.MosaType = fieldType;
			}
		}

		/// <summary>
		/// Visitation function for Ldsflda instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldsflda(Context context)
		{
			context.SetInstruction(IRInstruction.AddressOf, context.Result, Operand.CreateField(context.MosaField));
		}

		/// <summary>
		/// Visitation function for Ldstr instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldstr(Context context)
		{
			/*
			 * This requires a special memory layout for strings as they are interned by the compiler
			 * into the generated image. This won't work this way forever: As soon as we'll support
			 * a real AppDomain and real string interning, this code will have to go away and will
			 * be replaced by a proper VM call.
			 */

			var linker = MethodCompiler.Linker;
			string symbolName = context.Operand1.Name;
			string stringdata = context.Operand1.StringData;

			context.SetInstruction(IRInstruction.MoveInteger, context.Result, context.Operand1);

			var symbol = linker.CreateSymbol(symbolName, SectionKind.ROData, NativeAlignment, (NativePointerSize * 3) + (stringdata.Length * 2));
			var stream = symbol.Stream;

			// Type Definition and sync block
			linker.Link(LinkType.AbsoluteAddress, PatchType.I4, symbol, 0, SectionKind.ROData, "System.String" + Metadata.TypeDefinition, 0);

			stream.WriteZeroBytes(NativePointerSize * 2);

			// String length field
			stream.Write(BitConverter.GetBytes(stringdata.Length), 0, NativePointerSize);

			// String data
			var stringData = Encoding.Unicode.GetBytes(stringdata);
			Debug.Assert(stringData.Length == stringdata.Length * 2, "Byte array of string data doesn't match expected string data length");
			stream.Write(stringData);
		}

		/// <summary>
		/// Visitation function for Ldtoken instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldtoken(Context context)
		{
			// TODO: remove VmCall.GetHandleForToken?

			Operand source;
			Operand runtimeHandle;

			if (context.MosaType != null)
			{
				source = Operand.CreateUnmanagedSymbolPointer(TypeSystem, context.MosaType.FullName + Metadata.TypeDefinition);
				runtimeHandle = AllocateVirtualRegister(TypeSystem.GetTypeByName("System", "RuntimeTypeHandle"));
			}
			else if (context.MosaField != null)
			{
				source = Operand.CreateUnmanagedSymbolPointer(TypeSystem, context.MosaField.FullName + Metadata.FieldDefinition);
				runtimeHandle = AllocateVirtualRegister(TypeSystem.GetTypeByName("System", "RuntimeFieldHandle"));
			}
			else
			{
				throw new NotImplementCompilerException();
			}

			var destination = context.Result;
			context.SetInstruction(IRInstruction.MoveInteger, runtimeHandle, source);
			context.AppendInstruction(IRInstruction.MoveInteger, destination, runtimeHandle);
		}

		/// <summary>
		/// Visitation function for Ldvirtftn instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldvirtftn(Context context)
		{
			ReplaceWithVmCall(context, VmCall.GetVirtualFunctionPtr);
		}

		/// <summary>
		/// Visitation function for Leave instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Leave(Context context)
		{
			throw new InvalidCompilerException();
		}

		/// <summary>
		/// Visitation function for Mul instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Mul(Context context)
		{
			Replace(context, IRInstruction.MulFloatR4, IRInstruction.MulFloatR8, IRInstruction.MulSigned, IRInstruction.MulUnsigned);
		}

		/// <summary>
		/// Visitation function for Neg instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Neg(Context context)
		{
			//FUTURE: Add IRInstruction.Negate
			if (context.Operand1.IsUnsigned)
			{
				var zero = Operand.CreateConstant(context.Operand1.Type, 0);
				context.SetInstruction(IRInstruction.SubUnsigned, context.Result, zero, context.Operand1);
			}
			else if (context.Operand1.IsR4)
			{
				var minusOne = Operand.CreateConstant(TypeSystem, -1.0f);
				context.SetInstruction(IRInstruction.MulFloatR4, context.Result, minusOne, context.Operand1);
			}
			else if (context.Operand1.IsR8)
			{
				var minusOne = Operand.CreateConstant(TypeSystem, -1.0d);
				context.SetInstruction(IRInstruction.MulFloatR8, context.Result, minusOne, context.Operand1);
			}
			else
			{
				var minusOne = Operand.CreateConstant(context.Operand1.Type, -1);
				context.SetInstruction(IRInstruction.MulSigned, context.Result, minusOne, context.Operand1);
			}
		}

		/// <summary>
		/// Visitation function for Newarr instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Newarr(Context context)
		{
			var result = context.Result;
			var arrayType = result.Type;
			var elements = context.Operand1;

			Architecture.GetTypeRequirements(TypeLayout, arrayType.ElementType, out int elementSize, out int alignment);

			Debug.Assert(elementSize != 0);

			var runtimeTypeHandle = GetRuntimeTypeHandle(arrayType);
			var size = Operand.CreateConstant(TypeSystem, elementSize);
			context.SetInstruction(IRInstruction.NewArray, result, runtimeTypeHandle, size, elements);
		}

		/// <summary>
		/// Visitation function for Newobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Newobj(Context context)
		{
			if (ReplaceWithInternalCall(context))
				return;

			var classType = context.InvokeMethod.DeclaringType;
			var thisReference = context.Result;

			var operands = new List<Operand>(context.Operands);

			var before = context.InsertBefore();

			// If the type is value type we don't need to call AllocateObject
			if (MosaTypeLayout.IsStoredOnStack(thisReference.Type))
			{
				Debug.Assert(thisReference.Uses.Count <= 1, "Usages too high");

				var newThis = MethodCompiler.CreateVirtualRegister(thisReference.Type.ToManagedPointer());
				before.SetInstruction(IRInstruction.AddressOf, newThis, thisReference);

				operands.Insert(0, newThis);
			}
			else if (thisReference.Type.IsValueType)
			{
				Debug.Assert(thisReference.Uses.Count <= 1, "Usages too high");

				var newThis = MethodCompiler.AddStackLocal(thisReference.Type);
				var newThisReference = MethodCompiler.CreateVirtualRegister(thisReference.Type.ToManagedPointer());
				before.SetInstruction(IRInstruction.AddressOf, newThisReference, newThis);

				var size = GetInstructionSize(newThis.Type);
				var loadInstruction = GetLoadInstruction(newThis.Type);

				context.InsertAfter().SetInstruction(loadInstruction, thisReference, StackFrame, newThis);

				operands.Insert(0, newThisReference);
			}
			else
			{
				Debug.Assert(thisReference.Type.IsReferenceType, $"VmCall.AllocateObject only needs to be called for reference types. Type: {thisReference.Type}");

				var runtimeTypeHandle = GetRuntimeTypeHandle(classType);
				var size = Operand.CreateConstant(TypeSystem, TypeLayout.GetTypeSize(classType));
				before.SetInstruction(IRInstruction.NewObject, thisReference, runtimeTypeHandle, size);

				operands.Insert(0, thisReference);
			}

			ProcessInvokeInstruction(context, context.InvokeMethod, null, operands);
		}

		/// <summary>
		/// Visitation function for Nop instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Nop(Context context)
		{
			context.SetInstruction(IRInstruction.Nop);
		}

		/// <summary>
		/// Visitation function for Not instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Not(Context context)
		{
			context.SetInstruction(IRInstruction.LogicalNot, context.Result, context.Operand1);
		}

		private bool OverridesMethod(MosaMethod method)
		{
			if (method.Overrides == null)
				return false;
			if (method.DeclaringType.BaseType.Name.Equals("ValueType"))
				return true;
			if (method.DeclaringType.BaseType.Name.Equals("Object"))
				return true;
			if (method.DeclaringType.BaseType.Name.Equals("Enum"))
				return true;
			return false;
		}

		/// <summary>
		/// Visitation function for Pop instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Pop(Context context)
		{
			context.Empty();
		}

		/// <summary>
		/// Visitation function for Rem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Rem(Context context)
		{
			Replace(context, IRInstruction.RemFloatR4, IRInstruction.RemFloatR8, IRInstruction.RemSigned, IRInstruction.RemUnsigned);
		}

		/// <summary>
		/// Visitation function for Ret instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ret(Context context)
		{
			context.ReplaceInstructionOnly(IRInstruction.Return);
		}

		/// <summary>
		/// Visitation function for Rethrow instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Rethrow(Context context)
		{
			ReplaceWithVmCall(context, VmCall.Rethrow);
		}

		/// <summary>
		/// Visitation function for Shift instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <exception cref="InvalidCompilerException"></exception>
		private void Shift(Context context)
		{
			switch ((context.Instruction as BaseCILInstruction).OpCode)
			{
				case OpCode.Shl: context.SetInstruction(IRInstruction.ShiftLeft, context.Result, context.Operand1, context.Operand2); break;
				case OpCode.Shr: context.SetInstruction(IRInstruction.ArithmeticShiftRight, context.Result, context.Operand1, context.Operand2); break;
				case OpCode.Shr_un: context.SetInstruction(IRInstruction.ShiftRight, context.Result, context.Operand1, context.Operand2); break;
				default: throw new InvalidCompilerException();
			}
		}

		/// <summary>
		/// Visitation function for Sizeof instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Sizeof(Context context)
		{
			var type = context.MosaType;
			var size = type.IsPointer ? NativePointerSize : MethodCompiler.TypeLayout.GetTypeSize(type);
			context.SetInstruction(IRInstruction.MoveInteger, context.Result, Operand.CreateConstant(TypeSystem, size));
		}

		/// <summary>
		/// Visitation function for Starg instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Starg(Context context)
		{
			Debug.Assert(context.Result.IsParameter);

			if (MosaTypeLayout.IsStoredOnStack(context.Operand1.Type))
			{
				context.SetInstruction(IRInstruction.StoreParameterCompound, context.Size, null, context.Result, context.Operand1);
				context.MosaType = context.Result.Type; // may not be necessary
			}
			else
			{
				var storeInstruction = GetStoreParameterInstruction(context.Operand1.Type);
				context.SetInstruction(storeInstruction, context.Size, null, context.Result, context.Operand1);
			}
		}

		/// <summary>
		/// Visitation function for Stelem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Stelem(Context context)
		{
			var array = context.Operand1;
			var arrayIndex = context.Operand2;
			var value = context.Operand3;
			var arrayType = array.Type;

			// Array bounds check
			AddArrayBoundsCheck(context, array, arrayIndex);

			var arrayAddress = LoadArrayBaseAddress(context, array);
			var elementOffset = CalculateArrayElementOffset(context, arrayType, arrayIndex);

			if (MosaTypeLayout.IsStoredOnStack(value.Type))
			{
				context.SetInstruction(IRInstruction.StoreCompound, null, arrayAddress, elementOffset, value);
				context.MosaType = arrayType.ElementType;
			}
			else
			{
				var storeInstruction = GetStoreInstruction(value.Type);
				var size = GetInstructionSize(arrayType.ElementType);

				context.SetInstruction(storeInstruction, size, null, arrayAddress, elementOffset, value);
			}
		}

		/// <summary>
		/// Visitation function for Stfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Stfld(Context context)
		{
			var objectOperand = context.Operand1;
			var valueOperand = context.Operand2;
			var fieldType = context.MosaField.FieldType;

			int offset = TypeLayout.GetFieldOffset(context.MosaField);
			var offsetOperand = Operand.CreateConstant(TypeSystem, offset);

			var size = GetInstructionSize(fieldType);

			if (MosaTypeLayout.IsStoredOnStack(fieldType))
			{
				context.SetInstruction(IRInstruction.StoreCompound, size, null, objectOperand, offsetOperand, valueOperand);
				context.MosaType = fieldType;
			}
			else
			{
				var storeInstruction = GetStoreInstruction(fieldType);
				context.SetInstruction(storeInstruction, size, null, objectOperand, offsetOperand, valueOperand);
				context.MosaType = fieldType;
			}
		}

		/// <summary>
		/// Visitation function for Stloc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Stloc(Context context)
		{
			var type = context.Operand1.Type;
			var size = GetInstructionSize(type);

			if (context.Result.IsVirtualRegister && context.Operand1.IsVirtualRegister)
			{
				var moveInstruction = GetMoveInstruction(context.Result.Type);

				context.SetInstruction(moveInstruction, size, context.Result, context.Operand1);
				return;
			}

			if (MosaTypeLayout.IsStoredOnStack(type))
			{
				Debug.Assert(!context.Result.IsVirtualRegister);
				context.SetInstruction(IRInstruction.MoveCompound, context.Result, context.Operand1);
			}
			else if (context.Operand1.IsVirtualRegister)
			{
				var storeInstruction = GetStoreInstruction(type);

				context.SetInstruction(storeInstruction, size, null, StackFrame, context.Result, context.Operand1);
			}

			context.MosaType = type;
		}

		/// <summary>
		/// Visitation function for Stobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Stobj(Context context)
		{
			// This is actually stind.* and stobj - the opcodes have the same meanings
			var type = context.MosaType;  // pass thru

			if (MosaTypeLayout.IsStoredOnStack(type))
			{
				context.SetInstruction(IRInstruction.StoreCompound, null, context.Operand1, ConstantZero, context.Operand2);
			}
			else
			{
				var size = GetInstructionSize(type);
				var storeInstruction = GetStoreInstruction(type);

				context.SetInstruction(storeInstruction, size, null, context.Operand1, ConstantZero, context.Operand2);
			}

			context.MosaType = type;
		}

		/// <summary>
		/// Visitation function for Stsfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Stsfld(Context context)
		{
			var field = context.MosaField;
			var size = GetInstructionSize(field.FieldType);
			var fieldOperand = Operand.CreateField(field);

			if (MosaTypeLayout.IsStoredOnStack(field.FieldType))
			{
				context.SetInstruction(IRInstruction.StoreCompound, size, null, fieldOperand, ConstantZero, context.Operand1);
				context.MosaType = field.FieldType;
			}
			else
			{
				var storeInstruction = GetStoreInstruction(context.Operand1.Type);

				context.SetInstruction(storeInstruction, size, null, fieldOperand, ConstantZero, context.Operand1);
				context.MosaType = field.FieldType;
			}
		}

		/// <summary>
		/// Visitation function for Sub instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Sub(Context context)
		{
			Replace(context, IRInstruction.SubFloatR4, IRInstruction.SubFloatR8, IRInstruction.SubSigned, IRInstruction.SubUnsigned);
		}

		/// <summary>
		/// Visitation function for Switch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Switch(Context context)
		{
			context.ReplaceInstructionOnly(IRInstruction.Switch);
		}

		/// <summary>
		/// Visitation function for Throw instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Throw(Context context)
		{
			throw new InvalidCompilerException();
		}

		/// <summary>
		/// Visitation function for UnaryBranch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void UnaryBranch(Context context)
		{
			var target = context.BranchTargets[0];
			var first = context.Operand1;
			var second = ConstantZero;
			var opcode = ((BaseCILInstruction)context.Instruction).OpCode;

			if (opcode == OpCode.Brtrue || opcode == OpCode.Brtrue_s)
			{
				context.SetInstruction(IRInstruction.CompareIntegerBranch, ConditionCode.NotEqual, null, first, second);
				context.AddBranchTarget(target);
				return;
			}
			else if (opcode == OpCode.Brfalse || opcode == OpCode.Brfalse_s)
			{
				context.SetInstruction(IRInstruction.CompareIntegerBranch, ConditionCode.Equal, null, first, second);
				context.AddBranchTarget(target);
				return;
			}

			throw new NotImplementCompilerException("CILTransformationStage.UnaryBranch doesn't support CIL opcode " + opcode);
		}

		/// <summary>
		/// Visitation function for Unbox instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Unbox(Context context)
		{
			var value = context.Operand1;
			var result = context.Result;
			var type = context.MosaType;

			if (!type.IsValueType)
			{
				var moveInstruction = GetMoveInstruction(type);

				context.ReplaceInstructionOnly(moveInstruction);
				return;
			}

			int typeSize = TypeLayout.GetTypeSize(type);
			typeSize = Alignment.AlignUp(typeSize, TypeLayout.NativePointerAlignment);

			var vmCall = ToVmUnboxCall(typeSize);

			context.SetInstruction(IRInstruction.Nop);
			ReplaceWithVmCall(context, vmCall);

			context.SetOperand(1, value);
			if (vmCall == VmCall.Unbox)
			{
				var adr = AllocateVirtualRegister(type.ToManagedPointer());
				context.InsertBefore().SetInstruction(IRInstruction.AddressOf, adr, MethodCompiler.AddStackLocal(type));

				context.SetOperand(2, adr);
				context.SetOperand(3, Operand.CreateConstant(TypeSystem, typeSize));
				context.OperandCount = 4;
			}
			else
			{
				context.OperandCount = 2;
			}

			var tmp = AllocateVirtualRegister(type.ToManagedPointer());
			context.Result = tmp;
			context.ResultCount = 1;

			if (MosaTypeLayout.IsStoredOnStack(type))
			{
				context.AppendInstruction(IRInstruction.LoadCompound, result, tmp, ConstantZero);
				context.MosaType = type;
			}
			else
			{
				var loadInstruction = GetLoadInstruction(type);
				var size = GetInstructionSize(type);

				context.AppendInstruction(loadInstruction, size, result, tmp, ConstantZero);
				context.MosaType = type;
			}
		}

		/// <summary>
		/// Visitation function for Unbox.Any instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void UnboxAny(Context context)
		{
			var value = context.Operand1;
			var result = context.Result;
			var type = context.MosaType;

			if (!type.IsValueType)
			{
				var moveInstruction = GetMoveInstruction(type);

				context.ReplaceInstructionOnly(moveInstruction);
				return;
			}

			int typeSize = TypeLayout.GetTypeSize(type);
			typeSize = Alignment.AlignUp(typeSize, TypeLayout.NativePointerAlignment);

			var vmCall = ToVmUnboxCall(typeSize);

			context.SetInstruction(IRInstruction.Nop);
			ReplaceWithVmCall(context, vmCall);

			context.SetOperand(1, value);
			if (vmCall == VmCall.Unbox)
			{
				var adr = AllocateVirtualRegister(type.ToManagedPointer());
				context.InsertBefore().SetInstruction(IRInstruction.AddressOf, adr, MethodCompiler.AddStackLocal(type));

				context.SetOperand(2, adr);
				context.SetOperand(3, Operand.CreateConstant(TypeSystem, typeSize));
				context.OperandCount = 4;
			}
			else
			{
				context.OperandCount = 2;
			}

			var tmp = AllocateVirtualRegister(type.ToManagedPointer());
			context.Result = tmp;
			context.ResultCount = 1;

			var size = GetInstructionSize(type);

			if (MosaTypeLayout.IsStoredOnStack(type))
			{
				context.AppendInstruction(IRInstruction.LoadCompound, result, tmp, ConstantZero);
				context.MosaType = type;
			}
			else
			{
				var loadInstruction = GetLoadInstruction(type);
				context.AppendInstruction(loadInstruction, size, result, tmp, ConstantZero);
				context.MosaType = type;
			}
		}

		#endregion Visitation Methods

		#region Internals

		private static readonly BaseIRInstruction[][] convTable = new BaseIRInstruction[13][] {
			/* I1 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.MoveInteger,
				/* I2 */ IRInstruction.LogicalAnd,
				/* I4 */ IRInstruction.LogicalAnd,
				/* I8 */ IRInstruction.LogicalAnd,
				/* U1 */ IRInstruction.MoveInteger,
				/* U2 */ IRInstruction.LogicalAnd,
				/* U4 */ IRInstruction.LogicalAnd,
				/* U8 */ IRInstruction.LogicalAnd,
				/* R4 */ IRInstruction.ConversionFloatR4ToInteger,
				/* R8 */ IRInstruction.ConversionFloatR8ToInteger,
				/* I  */ IRInstruction.LogicalAnd,
				/* U  */ IRInstruction.LogicalAnd,
				/* Ptr*/ IRInstruction.LogicalAnd,
			},
			/* I2 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.MoveSignExtended,
				/* I2 */ IRInstruction.MoveInteger,
				/* I4 */ IRInstruction.LogicalAnd,
				/* I8 */ IRInstruction.LogicalAnd,
				/* U1 */ IRInstruction.MoveZeroExtended,
				/* U2 */ IRInstruction.MoveInteger,
				/* U4 */ IRInstruction.LogicalAnd,
				/* U8 */ IRInstruction.LogicalAnd,
				/* R4 */ IRInstruction.ConversionFloatR4ToInteger,
				/* R8 */ IRInstruction.ConversionFloatR8ToInteger,
				/* I  */ IRInstruction.LogicalAnd,
				/* U  */ IRInstruction.LogicalAnd,
				/* Ptr*/ IRInstruction.LogicalAnd,
			},
			/* I4 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.MoveSignExtended,
				/* I2 */ IRInstruction.MoveSignExtended,
				/* I4 */ IRInstruction.MoveInteger,
				/* I8 */ IRInstruction.LogicalAnd,
				/* U1 */ IRInstruction.MoveZeroExtended,
				/* U2 */ IRInstruction.MoveZeroExtended,
				/* U4 */ IRInstruction.MoveInteger,
				/* U8 */ IRInstruction.LogicalAnd,
				/* R4 */ IRInstruction.ConversionFloatR4ToInteger,
				/* R8 */ IRInstruction.ConversionFloatR8ToInteger,
				/* I  */ IRInstruction.LogicalAnd,
				/* U  */ IRInstruction.LogicalAnd,
				/* Ptr*/ IRInstruction.LogicalAnd,
			},
			/* I8 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.MoveSignExtended,
				/* I2 */ IRInstruction.MoveSignExtended,
				/* I4 */ IRInstruction.MoveSignExtended,
				/* I8 */ IRInstruction.MoveInteger,
				/* U1 */ IRInstruction.MoveZeroExtended,
				/* U2 */ IRInstruction.MoveZeroExtended,
				/* U4 */ IRInstruction.MoveZeroExtended,
				/* U8 */ IRInstruction.MoveInteger,
				/* R4 */ IRInstruction.ConversionFloatR4ToInteger,
				/* R8 */ IRInstruction.ConversionFloatR8ToInteger,
				/* I  */ IRInstruction.LogicalAnd,
				/* U  */ IRInstruction.LogicalAnd,
				/* Ptr*/ IRInstruction.LogicalAnd,
			},
			/* U1 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.MoveInteger,
				/* I2 */ IRInstruction.LogicalAnd,
				/* I4 */ IRInstruction.LogicalAnd,
				/* I8 */ IRInstruction.LogicalAnd,
				/* U1 */ IRInstruction.MoveInteger,
				/* U2 */ IRInstruction.LogicalAnd,
				/* U4 */ IRInstruction.LogicalAnd,
				/* U8 */ IRInstruction.LogicalAnd,
				/* R4 */ IRInstruction.ConversionFloatR4ToInteger,
				/* R8 */ IRInstruction.ConversionFloatR8ToInteger,
				/* I  */ IRInstruction.LogicalAnd,
				/* U  */ IRInstruction.LogicalAnd,
				/* Ptr*/ IRInstruction.LogicalAnd,
			},
			/* U2 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.MoveZeroExtended,
				/* I2 */ IRInstruction.MoveInteger,
				/* I4 */ IRInstruction.LogicalAnd,
				/* I8 */ IRInstruction.LogicalAnd,
				/* U1 */ IRInstruction.MoveZeroExtended,
				/* U2 */ IRInstruction.MoveInteger,
				/* U4 */ IRInstruction.LogicalAnd,
				/* U8 */ IRInstruction.LogicalAnd,
				/* R4 */ IRInstruction.ConversionFloatR4ToInteger,
				/* R8 */ IRInstruction.ConversionFloatR8ToInteger,
				/* I  */ IRInstruction.LogicalAnd,
				/* U  */ IRInstruction.LogicalAnd,
				/* Ptr*/ IRInstruction.LogicalAnd,
			},
			/* U4 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.MoveZeroExtended,
				/* I2 */ IRInstruction.MoveZeroExtended,
				/* I4 */ IRInstruction.MoveInteger,
				/* I8 */ IRInstruction.LogicalAnd,
				/* U1 */ IRInstruction.MoveZeroExtended,
				/* U2 */ IRInstruction.MoveZeroExtended,
				/* U4 */ IRInstruction.MoveInteger,
				/* U8 */ IRInstruction.LogicalAnd,
				/* R4 */ IRInstruction.ConversionFloatR4ToInteger,
				/* R8 */ IRInstruction.ConversionFloatR8ToInteger,
				/* I  */ IRInstruction.LogicalAnd,
				/* U  */ IRInstruction.LogicalAnd,
				/* Ptr*/ IRInstruction.LogicalAnd,
			},
			/* U8 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.MoveZeroExtended,
				/* I2 */ IRInstruction.MoveZeroExtended,
				/* I4 */ IRInstruction.MoveZeroExtended,
				/* I8 */ IRInstruction.MoveInteger,
				/* U1 */ IRInstruction.MoveZeroExtended,
				/* U2 */ IRInstruction.MoveZeroExtended,
				/* U4 */ IRInstruction.MoveZeroExtended,
				/* U8 */ IRInstruction.MoveInteger,
				/* R4 */ IRInstruction.ConversionFloatR4ToInteger,
				/* R8 */ IRInstruction.ConversionFloatR8ToInteger,
				/* I  */ IRInstruction.LogicalAnd,
				/* U  */ IRInstruction.LogicalAnd,
				/* Ptr*/ IRInstruction.LogicalAnd,
			},
			/* R4 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.ConversionIntegerToFloatR8,
				/* I2 */ IRInstruction.ConversionIntegerToFloatR8,
				/* I4 */ IRInstruction.ConversionIntegerToFloatR8,
				/* I8 */ IRInstruction.ConversionIntegerToFloatR8,
				/* U1 */ IRInstruction.ConversionIntegerToFloatR8,
				/* U2 */ IRInstruction.ConversionIntegerToFloatR8,
				/* U4 */ IRInstruction.ConversionIntegerToFloatR8,
				/* U8 */ IRInstruction.ConversionIntegerToFloatR8,
				/* R4 */ IRInstruction.MoveFloatR4,
				/* R8 */ IRInstruction.ConversionFloatR4ToFloatR8,
				/* I  */ IRInstruction.ConversionIntegerToFloatR8,
				/* U  */ IRInstruction.ConversionIntegerToFloatR8,
				/* Ptr*/ null,
			},
			/* R8 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.ConversionIntegerToFloatR8,
				/* I2 */ IRInstruction.ConversionIntegerToFloatR8,
				/* I4 */ IRInstruction.ConversionIntegerToFloatR8,
				/* I8 */ IRInstruction.ConversionIntegerToFloatR8,
				/* U1 */ IRInstruction.ConversionIntegerToFloatR8,
				/* U2 */ IRInstruction.ConversionIntegerToFloatR8,
				/* U4 */ IRInstruction.ConversionIntegerToFloatR8,
				/* U8 */ IRInstruction.ConversionIntegerToFloatR8,
				/* R4 */ IRInstruction.ConversionFloatR8ToFloatR4,
				/* R8 */ IRInstruction.MoveFloatR8,
				/* I  */ IRInstruction.ConversionIntegerToFloatR8,
				/* U  */ IRInstruction.ConversionIntegerToFloatR8,
				/* Ptr*/ null,
			},
			/* I  */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.MoveSignExtended,
				/* I2 */ IRInstruction.MoveSignExtended,
				/* I4 */ IRInstruction.MoveSignExtended,
				/* I8 */ IRInstruction.MoveInteger,
				/* U1 */ IRInstruction.MoveZeroExtended,
				/* U2 */ IRInstruction.MoveZeroExtended,
				/* U4 */ IRInstruction.MoveZeroExtended,
				/* U8 */ IRInstruction.MoveZeroExtended,
				/* R4 */ IRInstruction.ConversionFloatR4ToInteger,
				/* R8 */ IRInstruction.ConversionFloatR8ToInteger,
				/* I  */ IRInstruction.MoveInteger,
				/* U  */ IRInstruction.MoveInteger,
				/* Ptr*/ IRInstruction.MoveInteger,
			},
			/* U  */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.MoveZeroExtended,
				/* I2 */ IRInstruction.MoveZeroExtended,
				/* I4 */ IRInstruction.MoveZeroExtended,
				/* I8 */ IRInstruction.MoveZeroExtended,
				/* U1 */ IRInstruction.MoveZeroExtended,
				/* U2 */ IRInstruction.MoveZeroExtended,
				/* U4 */ IRInstruction.MoveZeroExtended,
				/* U8 */ IRInstruction.MoveInteger,
				/* R4 */ IRInstruction.ConversionFloatR4ToInteger,
				/* R8 */ IRInstruction.ConversionFloatR8ToInteger,
				/* I  */ IRInstruction.MoveInteger,
				/* U  */ IRInstruction.MoveInteger,
				/* Ptr*/ IRInstruction.MoveInteger,
			},
			/* Ptr*/ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.MoveZeroExtended,
				/* I2 */ IRInstruction.MoveZeroExtended,
				/* I4 */ IRInstruction.MoveZeroExtended,
				/* I8 */ IRInstruction.MoveZeroExtended,
				/* U1 */ IRInstruction.MoveZeroExtended,
				/* U2 */ IRInstruction.MoveZeroExtended,
				/* U4 */ IRInstruction.MoveZeroExtended,
				/* U8 */ IRInstruction.MoveZeroExtended,
				/* R4 */ null,
				/* R8 */ null,
				/* I  */ IRInstruction.MoveInteger,
				/* U  */ IRInstruction.MoveInteger,
				/* Ptr*/ IRInstruction.MoveInteger,
			},
		};

		/// <summary>
		/// Converts the specified opcode.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		/// <returns></returns>
		/// <exception cref="InvalidProgramException"></exception>
		private static ConditionCode ConvertCondition(OpCode opcode)
		{
			switch (opcode)
			{
				// Signed
				case OpCode.Beq_s: return ConditionCode.Equal;
				case OpCode.Bge_s: return ConditionCode.GreaterOrEqual;
				case OpCode.Bgt_s: return ConditionCode.GreaterThan;
				case OpCode.Ble_s: return ConditionCode.LessOrEqual;
				case OpCode.Blt_s: return ConditionCode.LessThan;

				// Unsigned
				case OpCode.Bne_un_s: return ConditionCode.NotEqual;
				case OpCode.Bge_un_s: return ConditionCode.UnsignedGreaterOrEqual;
				case OpCode.Bgt_un_s: return ConditionCode.UnsignedGreaterThan;
				case OpCode.Ble_un_s: return ConditionCode.UnsignedLessOrEqual;
				case OpCode.Blt_un_s: return ConditionCode.UnsignedLessThan;

				// Long form signed
				case OpCode.Beq: goto case OpCode.Beq_s;
				case OpCode.Bge: goto case OpCode.Bge_s;
				case OpCode.Bgt: goto case OpCode.Bgt_s;
				case OpCode.Ble: goto case OpCode.Ble_s;
				case OpCode.Blt: goto case OpCode.Blt_s;

				// Long form unsigned
				case OpCode.Bne_un: goto case OpCode.Bne_un_s;
				case OpCode.Bge_un: goto case OpCode.Bge_un_s;
				case OpCode.Bgt_un: goto case OpCode.Bgt_un_s;
				case OpCode.Ble_un: goto case OpCode.Ble_un_s;
				case OpCode.Blt_un: goto case OpCode.Blt_un_s;

				// Compare
				case OpCode.Ceq: return ConditionCode.Equal;
				case OpCode.Cgt: return ConditionCode.GreaterThan;
				case OpCode.Cgt_un: return ConditionCode.UnsignedGreaterThan;
				case OpCode.Clt: return ConditionCode.LessThan;
				case OpCode.Clt_un: return ConditionCode.UnsignedLessThan;

				default: throw new InvalidProgramException();
			}
		}

		/// <summary>
		/// Determines if a store is silently truncating the value.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <returns>True if the store is truncating, otherwise false.</returns>
		private static bool IsTruncating(Operand destination, Operand source)
		{
			if (destination.IsInt)
			{
				return source.IsLong;
			}
			else if (destination.IsShort || destination.IsChar)
			{
				return source.IsLong || source.IsInteger;
			}
			else if (destination.IsByte) // UNKNOWN: Add destination.IsBoolean
			{
				return source.IsLong || source.IsInteger || source.IsShort;
			}

			return false;
		}

		private static void Replace(Context context, BaseInstruction floatingPointR4Instruction, BaseInstruction floatingPointR8Instruction, BaseInstruction signedInstruction, BaseInstruction unsignedInstruction)
		{
			if (context.Result.IsR4)
			{
				context.ReplaceInstructionOnly(floatingPointR4Instruction);
			}
			else if (context.Result.IsR8)
			{
				context.ReplaceInstructionOnly(floatingPointR8Instruction);
			}
			else if (context.Result.IsUnsigned)
			{
				context.ReplaceInstructionOnly(unsignedInstruction);
			}
			else
			{
				context.ReplaceInstructionOnly(signedInstruction);
			}
		}

		/// <summary>
		/// Adds bounds check to the array access.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="arrayOperand">The array operand.</param>
		/// <param name="arrayIndexOperand">The index operand.</param>
		private void AddArrayBoundsCheck(Context context, Operand arrayOperand, Operand arrayIndexOperand)
		{
			var before = context.InsertBefore();

			// First create new block and split current block
			var exceptionContext = CreateNewBlockContexts(1)[0];
			var nextContext = Split(before);

			// Get array length
			var lengthOperand = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
			var fixedOffset = Operand.CreateConstant(TypeSystem, NativePointerSize * 2);

			before.SetInstruction(IRInstruction.LoadInteger, lengthOperand, arrayOperand, fixedOffset);

			// Now compare length with index
			// If index is greater than or equal to the length then jump to exception block, otherwise jump to next block
			before.AppendInstruction(IRInstruction.CompareIntegerBranch, ConditionCode.UnsignedGreaterOrEqual, null, arrayIndexOperand, lengthOperand, exceptionContext.Block);
			before.AppendInstruction(IRInstruction.Jmp, nextContext.Block);

			// Build exception block which is just a call to throw exception
			var method = InternalRuntimeType.FindMethodByName("ThrowIndexOutOfRangeException");
			var symbolOperand = Operand.CreateSymbolFromMethod(TypeSystem, method);

			exceptionContext.AppendInstruction(IRInstruction.Call, null, symbolOperand);
			exceptionContext.InvokeMethod = method;
		}

		/// <summary>
		/// Calculates the element offset for the specified index.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="arrayType">The array type.</param>
		/// <param name="index">The index operand.</param>
		/// <returns>Element offset operand.</returns>
		private Operand CalculateArrayElementOffset(Context context, MosaType arrayType, Operand index)
		{
			Architecture.GetTypeRequirements(TypeLayout, arrayType.ElementType, out int size, out int alignment);

			var elementOffset = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);
			var elementSize = Operand.CreateConstant(TypeSystem, size);

			var before = context.InsertBefore();

			before.AppendInstruction(IRInstruction.MulSigned, elementOffset, index, elementSize);

			return elementOffset;
		}

		/// <summary>
		/// Calculates the base of the array elements.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="array">The array.</param>
		/// <returns>
		/// Base address for array elements.
		/// </returns>
		private Operand LoadArrayBaseAddress(Context context, Operand array)
		{
			var fixedOffset = Operand.CreateConstant(TypeSystem, NativePointerSize * 3);
			var arrayElement = AllocateVirtualRegister(TypeSystem.BuiltIn.I4);

			context.InsertBefore().AppendInstruction(IRInstruction.AddSigned, arrayElement, array, fixedOffset);

			return arrayElement;
		}

		private bool CanSkipDueToRecursiveSystemObjectCtorCall(Context context)
		{
			var currentMethod = MethodCompiler.Method;
			var invokeTarget = context.InvokeMethod;

			// Skip recursive System.Object ctor calls.
			return currentMethod.DeclaringType.FullName == "System.Object"
				&& currentMethod.Name == ".ctor"
				&& invokeTarget.DeclaringType.FullName == "System.Object"
				&& invokeTarget.Name == ".ctor";
		}

		/// <summary>
		/// Gets the index.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="Platform32Bit">if set to <c>true</c> [platform32 bit].</param>
		/// <returns></returns>
		/// <exception cref="InvalidCompilerException"></exception>
		private int GetIndex(MosaType type, bool Platform32Bit)
		{
			if (type.IsChar) return 5;
			else if (type.IsI1) return 0;
			else if (type.IsI2) return 1;
			else if (type.IsI4) return 2;
			else if (type.IsI8) return 3;
			else if (type.IsU1) return 4;
			else if (type.IsU2) return 5;
			else if (type.IsU4) return 6;
			else if (type.IsU8) return 7;
			else if (type.IsR4) return 8;
			else if (type.IsR8) return 9;
			else if (type.IsI) return Platform32Bit ? 2 : 10;
			else if (type.IsU) return Platform32Bit ? 6 : 11;
			else if (type.IsPointer) return 12;
			else if (!type.IsValueType) return 12;

			throw new InvalidCompilerException();
		}

		/// <summary>
		/// Processes external method calls.
		/// </summary>
		/// <param name="context">The transformation context.</param>
		/// <returns>
		///   <c>true</c> if the method was replaced by an intrinsic; <c>false</c> otherwise.
		/// </returns>
		/// <remarks>
		/// This method checks if the call target has an Intrinsic-Attribute applied with
		/// the current architecture. If it has, the method call is replaced by the specified
		/// native instruction.
		/// </remarks>
		private bool ProcessExternalCall(Context context)
		{
			Type intrinsicType = null;

			if (context.InvokeMethod.ExternMethod != null)
			{
				intrinsicType = Type.GetType(context.InvokeMethod.ExternMethod);
			}
			else if (context.InvokeMethod.IsInternal)
			{
				MethodCompiler.Compiler.IntrinsicTypes.TryGetValue(context.InvokeMethod.FullName, out intrinsicType);

				if (intrinsicType == null)
				{
					MethodCompiler.Compiler.IntrinsicTypes.TryGetValue(context.InvokeMethod.DeclaringType.FullName + "::" + context.InvokeMethod.Name, out intrinsicType);
				}

				Debug.Assert(intrinsicType != null, "Method is internal but no processor found: " + context.InvokeMethod.FullName);
			}

			if (intrinsicType == null)
				return false;

			var instance = Activator.CreateInstance(intrinsicType);

			if (instance is IIntrinsicInternalMethod instanceMethod)
			{
				instanceMethod.ReplaceIntrinsicCall(context, MethodCompiler);
				return true;
			}
			else if (instance is IIntrinsicPlatformMethod)
			{
				context.ReplaceInstructionOnly(IRInstruction.IntrinsicMethodCall);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Processes the invoke instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="method">The method.</param>
		/// <param name="resultOperand">The result operand.</param>
		/// <param name="operands">The operands.</param>
		private void ProcessInvokeInstruction(Context context, MosaMethod method, Operand resultOperand, List<Operand> operands)
		{
			var symbolOperand = Operand.CreateSymbolFromMethod(TypeSystem, method);
			ProcessInvokeInstruction(context, method, symbolOperand, resultOperand, operands);
		}

		/// <summary>
		/// Processes a method call instruction.
		/// </summary>
		/// <param name="context">The transformation context.</param>
		/// <param name="method">The method.</param>
		/// <param name="symbolOperand">The symbol operand.</param>
		/// <param name="resultOperand">The result operand.</param>
		/// <param name="operands">The operands.</param>
		private void ProcessInvokeInstruction(Context context, MosaMethod method, Operand symbolOperand, Operand resultOperand, List<Operand> operands)
		{
			Debug.Assert(method != null);

			context.SetInstruction(IRInstruction.Call, (byte)(operands.Count + 1), (byte)(resultOperand == null ? 0 : 1));
			context.InvokeMethod = method;

			if (resultOperand != null)
			{
				context.Result = resultOperand;
			}

			int index = 0;
			context.SetOperand(index++, symbolOperand);
			foreach (var operand in operands)
			{
				context.SetOperand(index++, operand);
			}
		}

		/// <summary>
		/// Replaces the IL load instruction by an appropriate IR move instruction or removes it entirely, if
		/// it is a native size.
		/// </summary>
		/// <param name="context">Provides the transformation context.</param>
		private void ProcessLoadInstruction(Context context)
		{
			var destination = context.Result;
			var source = context.Operand1;
			var size = GetInstructionSize(source.Type);

			if (MosaTypeLayout.IsStoredOnStack(source.Type))
			{
				context.SetInstruction(IRInstruction.MoveCompound, destination, source);
			}
			else if (!source.IsVirtualRegister)
			{
				var loadInstruction = GetLoadInstruction(source.Type);

				context.SetInstruction(loadInstruction, size, destination, StackFrame, source);
			}
			else
			{
				var moveInstruction = GetMoveInstruction(source.Type);

				context.SetInstruction(moveInstruction, size, destination, source);
			}
		}

		private bool ReplaceWithInternalCall(Context context)
		{
			var method = context.InvokeMethod;

			if (!method.IsInternal)
				return false;

			string replacementMethod = (method.Name == ".ctor") ? "Create" + method.DeclaringType.Name : "Internal" + method.Name;

			method = method.DeclaringType.FindMethodByNameAndParameters(replacementMethod, method.Signature.Parameters);

			var result = context.Result;

			var operands = new List<Operand>(context.Operands);

			ProcessInvokeInstruction(context, method, result, operands);

			return true;
		}

		/// <summary>
		/// Replaces the instruction with an internal call.
		/// </summary>
		/// <param name="context">The transformation context.</param>
		/// <param name="internalCallTarget">The internal call target.</param>
		private void ReplaceWithVmCall(Context context, VmCall internalCallTarget)
		{
			var method = InternalRuntimeType.FindMethodByName(internalCallTarget.ToString()) ?? PlatformInternalRuntimeType.FindMethodByName(internalCallTarget.ToString());

			Debug.Assert(method != null, "Cannot find method: " + internalCallTarget.ToString());

			context.ReplaceInstructionOnly(IRInstruction.Call);
			context.SetOperand(0, Operand.CreateSymbolFromMethod(TypeSystem, method));
			context.OperandCount = 1;
			context.InvokeMethod = method;
		}

		private VmCall ToVmUnboxCall(int typeSize)
		{
			if (typeSize <= 4)
				return VmCall.Unbox32;
			else if (typeSize == 8)
				return VmCall.Unbox64;
			else
				return VmCall.Unbox;
		}

		#endregion Internals
	}
}