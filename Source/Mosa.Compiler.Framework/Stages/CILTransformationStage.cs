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
			AddVisitation(CILInstruction.Add, Add);
			AddVisitation(CILInstruction.And, BinaryLogic);
			AddVisitation(CILInstruction.Beq, BinaryBranch);
			AddVisitation(CILInstruction.Beq_s, BinaryBranch);
			AddVisitation(CILInstruction.Bge, BinaryBranch);
			AddVisitation(CILInstruction.Bge_s, BinaryBranch);
			AddVisitation(CILInstruction.Bge_un, BinaryBranch);
			AddVisitation(CILInstruction.Bge_un_s, BinaryBranch);
			AddVisitation(CILInstruction.Bgt, BinaryBranch);
			AddVisitation(CILInstruction.Bgt_s, BinaryBranch);
			AddVisitation(CILInstruction.Bgt_un, BinaryBranch);
			AddVisitation(CILInstruction.Bgt_un_s, BinaryBranch);
			AddVisitation(CILInstruction.Ble, BinaryBranch);
			AddVisitation(CILInstruction.Ble_s, BinaryBranch);
			AddVisitation(CILInstruction.Ble_un, BinaryBranch);
			AddVisitation(CILInstruction.Ble_un_s, BinaryBranch);
			AddVisitation(CILInstruction.Blt, BinaryBranch);
			AddVisitation(CILInstruction.Blt_s, BinaryBranch);
			AddVisitation(CILInstruction.Blt_un, BinaryBranch);
			AddVisitation(CILInstruction.Blt_un_s, BinaryBranch);
			AddVisitation(CILInstruction.Bne_un, BinaryBranch);
			AddVisitation(CILInstruction.Bne_un_s, BinaryBranch);
			AddVisitation(CILInstruction.Box, Box);
			AddVisitation(CILInstruction.Br, Branch);
			AddVisitation(CILInstruction.Br_s, Branch);
			AddVisitation(CILInstruction.Break, Break);
			AddVisitation(CILInstruction.Brfalse, UnaryBranch);
			AddVisitation(CILInstruction.Brfalse_s, UnaryBranch);
			AddVisitation(CILInstruction.Brtrue, UnaryBranch);
			AddVisitation(CILInstruction.Brtrue_s, UnaryBranch);
			AddVisitation(CILInstruction.Call, Call);
			AddVisitation(CILInstruction.Calli, Calli);
			AddVisitation(CILInstruction.Callvirt, Callvirt);
			AddVisitation(CILInstruction.Castclass, Castclass);
			AddVisitation(CILInstruction.Ceq, BinaryComparison);
			AddVisitation(CILInstruction.Cgt, BinaryComparison);
			AddVisitation(CILInstruction.Cgt_un, BinaryComparison);
			AddVisitation(CILInstruction.Clt, BinaryComparison);
			AddVisitation(CILInstruction.Clt_un, BinaryComparison);
			AddVisitation(CILInstruction.Conv_i, Conversion);
			AddVisitation(CILInstruction.Conv_i1, Conversion);
			AddVisitation(CILInstruction.Conv_i2, Conversion);
			AddVisitation(CILInstruction.Conv_i4, Conversion);
			AddVisitation(CILInstruction.Conv_i8, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_i, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_i_un, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_i1, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_i1_un, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_i2, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_i2_un, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_i4, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_i4_un, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_i8, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_i8_un, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_u, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_u_un, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_u1, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_u1_un, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_u2, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_u2_un, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_u4, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_u4_un, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_u8, Conversion);
			AddVisitation(CILInstruction.Conv_ovf_u8_un, Conversion);
			AddVisitation(CILInstruction.Conv_r_un, Conversion);
			AddVisitation(CILInstruction.Conv_r4, Conversion);
			AddVisitation(CILInstruction.Conv_r8, Conversion);
			AddVisitation(CILInstruction.Conv_u, Conversion);
			AddVisitation(CILInstruction.Conv_u1, Conversion);
			AddVisitation(CILInstruction.Conv_u2, Conversion);
			AddVisitation(CILInstruction.Conv_u4, Conversion);
			AddVisitation(CILInstruction.Conv_u8, Conversion);
			AddVisitation(CILInstruction.Cpblk, Cpblk);
			AddVisitation(CILInstruction.Div, Div);
			AddVisitation(CILInstruction.Div_un, BinaryLogic);
			AddVisitation(CILInstruction.Dup, Dup);
			AddVisitation(CILInstruction.Endfilter, Endfilter);
			AddVisitation(CILInstruction.Endfinally, Endfinally);
			AddVisitation(CILInstruction.Initblk, Initblk);
			AddVisitation(CILInstruction.InitObj, InitObj);
			AddVisitation(CILInstruction.Isinst, IsInst);
			AddVisitation(CILInstruction.Ldarg, Ldarg);
			AddVisitation(CILInstruction.Ldarg_0, Ldarg);
			AddVisitation(CILInstruction.Ldarg_1, Ldarg);
			AddVisitation(CILInstruction.Ldarg_2, Ldarg);
			AddVisitation(CILInstruction.Ldarg_3, Ldarg);
			AddVisitation(CILInstruction.Ldarg_s, Ldarg);
			AddVisitation(CILInstruction.Ldarga, Ldarga);
			AddVisitation(CILInstruction.Ldarga_s, Ldarga);
			AddVisitation(CILInstruction.Ldc_i4, Ldc);
			AddVisitation(CILInstruction.Ldc_i4_0, Ldc);
			AddVisitation(CILInstruction.Ldc_i4_1, Ldc);
			AddVisitation(CILInstruction.Ldc_i4_2, Ldc);
			AddVisitation(CILInstruction.Ldc_i4_3, Ldc);
			AddVisitation(CILInstruction.Ldc_i4_4, Ldc);
			AddVisitation(CILInstruction.Ldc_i4_5, Ldc);
			AddVisitation(CILInstruction.Ldc_i4_6, Ldc);
			AddVisitation(CILInstruction.Ldc_i4_7, Ldc);
			AddVisitation(CILInstruction.Ldc_i4_8, Ldc);
			AddVisitation(CILInstruction.Ldc_i4_m1, Ldc);
			AddVisitation(CILInstruction.Ldc_i4_s, Ldc);
			AddVisitation(CILInstruction.Ldc_i8, Ldc);
			AddVisitation(CILInstruction.Ldc_r4, Ldc);
			AddVisitation(CILInstruction.Ldc_r8, Ldc);
			AddVisitation(CILInstruction.Ldelem, Ldelem);
			AddVisitation(CILInstruction.Ldelem_i, Ldelem);
			AddVisitation(CILInstruction.Ldelem_i1, Ldelem);
			AddVisitation(CILInstruction.Ldelem_i2, Ldelem);
			AddVisitation(CILInstruction.Ldelem_i4, Ldelem);
			AddVisitation(CILInstruction.Ldelem_i8, Ldelem);
			AddVisitation(CILInstruction.Ldelem_r4, Ldelem);
			AddVisitation(CILInstruction.Ldelem_r8, Ldelem);
			AddVisitation(CILInstruction.Ldelem_ref, Ldelem);
			AddVisitation(CILInstruction.Ldelem_u1, Ldelem);
			AddVisitation(CILInstruction.Ldelem_u2, Ldelem);
			AddVisitation(CILInstruction.Ldelem_u4, Ldelem);
			AddVisitation(CILInstruction.Ldelema, Ldelema);
			AddVisitation(CILInstruction.Ldfld, Ldfld);
			AddVisitation(CILInstruction.Ldflda, Ldflda);
			AddVisitation(CILInstruction.Ldftn, Ldftn);
			AddVisitation(CILInstruction.Ldind_i, Ldobj);
			AddVisitation(CILInstruction.Ldind_i1, Ldobj);
			AddVisitation(CILInstruction.Ldind_i2, Ldobj);
			AddVisitation(CILInstruction.Ldind_i4, Ldobj);
			AddVisitation(CILInstruction.Ldind_i8, Ldobj);
			AddVisitation(CILInstruction.Ldind_r4, Ldobj);
			AddVisitation(CILInstruction.Ldind_r8, Ldobj);
			AddVisitation(CILInstruction.Ldind_ref, Ldobj);
			AddVisitation(CILInstruction.Ldind_u1, Ldobj);
			AddVisitation(CILInstruction.Ldind_u2, Ldobj);
			AddVisitation(CILInstruction.Ldind_u4, Ldobj);
			AddVisitation(CILInstruction.Ldlen, Ldlen);
			AddVisitation(CILInstruction.Ldloc, Ldloc);
			AddVisitation(CILInstruction.Ldloc_0, Ldloc);
			AddVisitation(CILInstruction.Ldloc_1, Ldloc);
			AddVisitation(CILInstruction.Ldloc_2, Ldloc);
			AddVisitation(CILInstruction.Ldloc_3, Ldloc);
			AddVisitation(CILInstruction.Ldloc_s, Ldloc);
			AddVisitation(CILInstruction.Ldloca, Ldloca);
			AddVisitation(CILInstruction.Ldloca_s, Ldloca);
			AddVisitation(CILInstruction.Ldnull, Ldc);
			AddVisitation(CILInstruction.Ldobj, Ldobj);
			AddVisitation(CILInstruction.Ldsfld, Ldsfld);
			AddVisitation(CILInstruction.Ldsflda, Ldsflda);
			AddVisitation(CILInstruction.Ldstr, Ldstr);
			AddVisitation(CILInstruction.Ldtoken, Ldtoken);
			AddVisitation(CILInstruction.Ldvirtftn, Ldvirtftn);
			AddVisitation(CILInstruction.Leave, Leave);
			AddVisitation(CILInstruction.Leave_s, Leave);
			AddVisitation(CILInstruction.Mul, Mul);
			AddVisitation(CILInstruction.Neg, Neg);
			AddVisitation(CILInstruction.Newarr, Newarr);
			AddVisitation(CILInstruction.Newobj, Newobj);
			AddVisitation(CILInstruction.Nop, Nop);
			AddVisitation(CILInstruction.Not, Not);
			AddVisitation(CILInstruction.Or, BinaryLogic);
			AddVisitation(CILInstruction.Pop, Pop);
			AddVisitation(CILInstruction.Rem, Rem);
			AddVisitation(CILInstruction.Rem_un, BinaryLogic);
			AddVisitation(CILInstruction.Ret, Ret);
			AddVisitation(CILInstruction.Rethrow, Rethrow);
			AddVisitation(CILInstruction.Shl, Shift);
			AddVisitation(CILInstruction.Shr, Shift);
			AddVisitation(CILInstruction.Shr_un, Shift);
			AddVisitation(CILInstruction.Sizeof, Sizeof);
			AddVisitation(CILInstruction.Starg, Starg);
			AddVisitation(CILInstruction.Starg_s, Starg);
			AddVisitation(CILInstruction.Stelem, Stelem);
			AddVisitation(CILInstruction.Stelem_i, Stelem);
			AddVisitation(CILInstruction.Stelem_i1, Stelem);
			AddVisitation(CILInstruction.Stelem_i2, Stelem);
			AddVisitation(CILInstruction.Stelem_i4, Stelem);
			AddVisitation(CILInstruction.Stelem_i8, Stelem);
			AddVisitation(CILInstruction.Stelem_r4, Stelem);
			AddVisitation(CILInstruction.Stelem_r8, Stelem);
			AddVisitation(CILInstruction.Stelem_ref, Stelem);
			AddVisitation(CILInstruction.Stfld, Stfld);
			AddVisitation(CILInstruction.Stind_i, Stobj);
			AddVisitation(CILInstruction.Stind_i1, Stobj);
			AddVisitation(CILInstruction.Stind_i2, Stobj);
			AddVisitation(CILInstruction.Stind_i4, Stobj);
			AddVisitation(CILInstruction.Stind_i8, Stobj);
			AddVisitation(CILInstruction.Stind_r4, Stobj);
			AddVisitation(CILInstruction.Stind_r8, Stobj);
			AddVisitation(CILInstruction.Stind_ref, Stobj);
			AddVisitation(CILInstruction.Stloc, Stloc);
			AddVisitation(CILInstruction.Stloc_0, Stloc);
			AddVisitation(CILInstruction.Stloc_1, Stloc);
			AddVisitation(CILInstruction.Stloc_2, Stloc);
			AddVisitation(CILInstruction.Stloc_3, Stloc);
			AddVisitation(CILInstruction.Stloc_s, Stloc);
			AddVisitation(CILInstruction.Stobj, Stobj);
			AddVisitation(CILInstruction.Stsfld, Stsfld);
			AddVisitation(CILInstruction.Sub, Sub);
			AddVisitation(CILInstruction.Switch, Switch);
			AddVisitation(CILInstruction.Throw, Throw);
			AddVisitation(CILInstruction.Unbox, Unbox);
			AddVisitation(CILInstruction.Unbox_any, UnboxAny);
			AddVisitation(CILInstruction.Xor, BinaryLogic);

			//AddVisitation(CILInstruction.Add_ovf,Add_ovf);
			//AddVisitation(CILInstruction.Add_ovf_un,Add_ovf_un);
			//AddVisitation(CILInstruction.Arglist,Arglist);
			//AddVisitation(CILInstruction.Ckfinite,Ckfinite);
			//AddVisitation(CILInstruction.Cpobj,Cpobj);
			//AddVisitation(CILInstruction.Jmp,Jmp);
			//AddVisitation(CILInstruction.Localalloc,Localalloc);
			//AddVisitation(CILInstruction.Mkrefany,Mkrefany);
			//AddVisitation(CILInstruction.Mul_ovf,Mul_ovf);
			//AddVisitation(CILInstruction.Mul_ovf_un,Mul_ovf_un);
			//AddVisitation(CILInstruction.PreConstrained,PreConstrained);
			//AddVisitation(CILInstruction.PreNo,PreNo);
			//AddVisitation(CILInstruction.PreReadOnly,PreReadOnly);
			//AddVisitation(CILInstruction.PreTail,PreTail);
			//AddVisitation(CILInstruction.PreUnaligned,PreUnaligned);
			//AddVisitation(CILInstruction.PreVolatile,PreVolatile);
			//AddVisitation(CILInstruction.Refanytype,Refanytype);
			//AddVisitation(CILInstruction.Refanyval,Refanyval);
			//AddVisitation(CILInstruction.Sub_ovf,Sub_ovf);
			//AddVisitation(CILInstruction.Sub_ovf_un,Sub_ovf_un);
		}

		#region Visitation Methods

		/// <summary>
		/// Visitation function for Add instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Add(InstructionNode node)
		{
			Replace(node, IRInstruction.AddFloatR4, IRInstruction.AddFloatR8, IRInstruction.AddSigned, IRInstruction.AddUnsigned);
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
		/// <param name="node">The node.</param>
		private void BinaryComparison(InstructionNode node)
		{
			var code = ConvertCondition((node.Instruction as BaseCILInstruction).OpCode);
			var first = node.Operand1;
			var second = node.Operand2;
			var result = node.Result;

			BaseInstruction instruction = IRInstruction.CompareInteger;
			if (first.IsR4)
				instruction = IRInstruction.CompareFloatR4;
			else if (first.IsR8)
				instruction = IRInstruction.CompareFloatR8;

			node.SetInstruction(instruction, code, result, first, second);
		}

		/// <summary>
		/// Visitation function for BinaryLogic instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void BinaryLogic(InstructionNode node)
		{
			if (node.Operand1.Type.IsEnum)
			{
				var type = node.Operand1.Type;
				var operand = Operand.CreateField(type.Fields[0]);
				node.SetOperand(0, operand);
			}

			if (node.Operand2.Type.IsEnum)
			{
				var type = node.Operand2.Type;
				var operand = Operand.CreateField(type.Fields[0]);
				node.SetOperand(1, operand);
			}

			switch ((node.Instruction as BaseCILInstruction).OpCode)
			{
				case OpCode.And: node.SetInstruction(IRInstruction.LogicalAnd, node.Result, node.Operand1, node.Operand2); break;
				case OpCode.Or: node.SetInstruction(IRInstruction.LogicalOr, node.Result, node.Operand1, node.Operand2); break;
				case OpCode.Xor: node.SetInstruction(IRInstruction.LogicalXor, node.Result, node.Operand1, node.Operand2); break;
				case OpCode.Div_un: node.SetInstruction(IRInstruction.DivUnsigned, node.Result, node.Operand1, node.Operand2); break;
				case OpCode.Rem_un: node.SetInstruction(IRInstruction.RemUnsigned, node.Result, node.Operand1, node.Operand2); break;
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
				context.ReplaceInstruction(moveInstruction);
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
			ReplaceWithVmCall(context.Node, vmCall);

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
		/// <param name="node">The node.</param>
		private void Branch(InstructionNode node)
		{
			node.ReplaceInstruction(IRInstruction.Jmp);
		}

		/// <summary>
		/// Visitation function for Break instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Break(InstructionNode node)
		{
			node.SetInstruction(IRInstruction.Break);
		}

		private int CalculateInterfaceSlot(MosaType interaceType)
		{
			return TypeLayout.GetInterfaceSlotOffset(interaceType);
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

			var method = context.InvokeMethod;

			// If the method being called is a virtual method then we need to box the value type
			if (method.IsVirtual
				&& context.Operand1.Type.ElementType != null
				&& context.Operand1.Type.ElementType.IsValueType
				&& method.DeclaringType == context.Operand1.Type.ElementType)
			{
				var before = context.InsertBefore();

				if (OverridesMethod(method))
				{
					before.SetInstruction(IRInstruction.SubSigned, context.Operand1, context.Operand1, Operand.CreateConstant(TypeSystem, NativePointerSize * 2));
				}
				else
				{
					// Get the value type, size and native alignment
					var type = context.Operand1.Type.ElementType;
					int typeSize = TypeLayout.GetTypeSize(type);

					typeSize = Alignment.AlignUp(typeSize, TypeLayout.NativePointerAlignment);

					// Create a virtual register to hold our boxed value
					var boxedValue = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);

					// Create a new context before the call and set it as a VmCall
					before.SetInstruction(IRInstruction.Nop);
					ReplaceWithVmCall(before.Node, VmCall.Box);

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

			var result = context.Result;
			var operands = new List<Operand>(context.Operands);
			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			context.SetInstruction(IRInstruction.CallStatic, result, symbol);
			context.AppendOperands(operands);
		}

		/// <summary>
		/// Visitation function for Calli instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Calli(InstructionNode node)
		{
			//todo: not yet implemented
			throw new NotImplementCompilerException();
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
			var result = context.Result;

			var operands = new List<Operand>(context.Operands);

			if (context.Previous.Instruction is ConstrainedPrefixInstruction)
			{
				var type = context.Previous.MosaType;

				// remove constrained prefix
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
						var elementType = context.Operand1.Type.ElementType;
						int typeSize = TypeLayout.GetTypeSize(elementType);

						typeSize = Alignment.AlignUp(typeSize, TypeLayout.NativePointerAlignment);

						// Create a virtual register to hold our boxed value
						var boxedValue = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);

						// Create a new context before the call and set it as a VmCall
						var before = context.InsertBefore();
						before.SetInstruction(IRInstruction.Nop);
						ReplaceWithVmCall(before.Node, VmCall.Box);

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

					var symbol2 = Operand.CreateSymbolFromMethod(TypeSystem, method);
					context.SetInstruction(IRInstruction.CallStatic, result, symbol2);
					context.AppendOperands(operands);
					return;
				}
			}

			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			if (method.IsVirtual)
			{
				if (method.DeclaringType.IsInterface)
				{
					context.SetInstruction(IRInstruction.CallInterface, result, symbol);
				}
				else
				{
					context.SetInstruction(IRInstruction.CallVirtual, result, symbol);
				}
			}
			else
			{
				// FIXME: Callvirt imposes a null-check. For virtual calls this is done implicitly, but for non-virtual calls
				// we have to make this explicitly somehow.
				context.SetInstruction(IRInstruction.CallStatic, result, symbol);
			}

			context.AppendOperands(operands);
		}

		/// <summary>
		/// Visitation function for Castclass instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Castclass(InstructionNode node)
		{
			// TODO!
			//ReplaceWithVmCall(context, VmCall.Castclass);
			node.ReplaceInstruction(IRInstruction.MoveInteger); // HACK!
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
		/// <param name="node">The node.</param>
		private void Cpblk(InstructionNode node)
		{
			ReplaceWithVmCall(node, VmCall.MemoryCopy);
		}

		/// <summary>
		/// Visitation function for Div instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Div(InstructionNode node)
		{
			Replace(node, IRInstruction.DivFloatR4, IRInstruction.DivFloatR8, IRInstruction.DivSigned, IRInstruction.DivUnsigned);
		}

		/// <summary>
		/// Visitation function for Dup instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Dup(InstructionNode node)
		{
			Debug.Assert(false); // should never get here

			// We don't need the dup anymore.
			node.Empty();
		}

		/// <summary>
		/// Visitation function for Endfilter instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Endfilter(InstructionNode node)
		{
			throw new InvalidCompilerException();
		}

		/// <summary>
		/// Visitation function for Endfinally instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Endfinally(InstructionNode node)
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

		/// <summary>
		/// Visitation function for Initblk instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Initblk(InstructionNode node)
		{
			ReplaceWithVmCall(node, VmCall.MemorySet);
		}

		/// <summary>
		/// Visitation function for InitObj instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		private void InitObj(InstructionNode node)
		{
			// Get the ptr and clear context
			var ptr = node.Operand1;

			// According to ECMA Spec, if the pointer element type is a reference type then
			// this instruction is the equivalent of ldnull followed by stind.ref

			var type = ptr.Type.ElementType;
			if (type.IsReferenceType)
			{
				var size = GetInstructionSize(type);
				node.SetInstruction(IRInstruction.StoreInteger, size, null, ptr, ConstantZero, Operand.GetNull(TypeSystem));
				node.MosaType = type;
			}
			else
			{
				node.SetInstruction(IRInstruction.Nop);

				// Setup context for VmCall
				ReplaceWithVmCall(node, VmCall.MemorySet);

				// Set the operands
				node.SetOperand(1, ptr);
				node.SetOperand(2, ConstantZero);
				node.SetOperand(3, Operand.CreateConstant(TypeSystem, TypeLayout.GetTypeSize(type)));
				node.OperandCount = 4;
			}
		}

		/// <summary>
		/// Visitation function for Isinst instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		private void IsInst(InstructionNode node)
		{
			var reference = node.Operand1;
			var result = node.Result;

			var classType = node.MosaType;

			if (!classType.IsInterface)
			{
				ReplaceWithVmCall(node, VmCall.IsInstanceOfType);

				node.SetOperand(1, GetRuntimeTypeHandle(classType));
				node.SetOperand(2, reference);
				node.OperandCount = 3;
				node.ResultCount = 1;
			}
			else
			{
				int slot = CalculateInterfaceSlot(classType);

				ReplaceWithVmCall(node, VmCall.IsInstanceOfInterfaceType);

				node.SetOperand(1, Operand.CreateConstant(TypeSystem, slot));
				node.SetOperand(2, reference);
				node.OperandCount = 3;
				node.ResultCount = 1;
			}
		}

		/// <summary>
		/// Visitation function for Ldarg instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldarg(InstructionNode node)
		{
			Debug.Assert(node.Operand1.IsParameter);

			if (MosaTypeLayout.IsStoredOnStack(node.Operand1.Type))
			{
				node.SetInstruction(IRInstruction.LoadParameterCompound, node.Result, node.Operand1);
				node.MosaType = node.Operand1.Type;
			}
			else
			{
				var loadInstruction = GetLoadParameterInstruction(node.Operand1.Type);
				var size = GetInstructionSize(node.Operand1.Type);

				node.SetInstruction(loadInstruction, size, node.Result, node.Operand1);
			}
		}

		/// <summary>
		/// Visitation function for Ldarga instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldarga(InstructionNode node)
		{
			node.ReplaceInstruction(IRInstruction.AddressOf);
		}

		/// <summary>
		/// Visitation function for Ldc instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldc(InstructionNode node)
		{
			Debug.Assert(node.Operand1.IsConstant || node.Operand1.IsVirtualRegister);

			var source = node.Operand1;
			var destination = node.Result;
			var size = GetInstructionSize(source.Type);

			Debug.Assert(!MosaTypeLayout.IsStoredOnStack(destination.Type));
			var moveInstruction = GetMoveInstruction(destination.Type);
			node.SetInstruction(moveInstruction, size, destination, source);
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
		/// <param name="node">The node.</param>
		private void Ldflda(InstructionNode node)
		{
			var fieldAddress = node.Result;
			var objectOperand = node.Operand1;

			int offset = TypeLayout.GetFieldOffset(node.MosaField);
			var fixedOffset = Operand.CreateConstant(TypeSystem, offset);

			node.SetInstruction(IRInstruction.AddUnsigned, fieldAddress, objectOperand, fixedOffset);
		}

		/// <summary>
		/// Visitation function for Ldftn instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldftn(InstructionNode node)
		{
			node.SetInstruction(IRInstruction.MoveInteger, node.Result, Operand.CreateSymbolFromMethod(TypeSystem, node.InvokeMethod));
		}

		/// <summary>
		/// Visitation function for Ldlen instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldlen(InstructionNode node)
		{
			var offset = Operand.CreateConstant(TypeSystem, NativePointerSize * 2);
			node.SetInstruction(IRInstruction.LoadInteger, InstructionSize.Size32, node.Result, node.Operand1, offset);
		}

		/// <summary>
		/// Visitation function for Ldloc instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldloc(InstructionNode node)
		{
			Debug.Assert(node.MosaType == null);
			ProcessLoadInstruction(node);
		}

		/// <summary>
		/// Visitation function for Ldloca instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldloca(InstructionNode node)
		{
			node.ReplaceInstruction(IRInstruction.AddressOf);
		}

		/// <summary>
		/// Visitation function for Ldobj instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldobj(InstructionNode node)
		{
			var destination = node.Result;
			var source = node.Operand1;

			var type = node.MosaType;

			// This is actually ldind.* and ldobj - the opcodes have the same meanings

			if (MosaTypeLayout.IsStoredOnStack(type))
			{
				node.SetInstruction(IRInstruction.LoadCompound, destination, source, ConstantZero);
			}
			else
			{
				var loadInstruction = GetLoadInstruction(type);
				var size = GetInstructionSize(type);

				node.SetInstruction(loadInstruction, size, destination, source, ConstantZero);
			}

			node.MosaType = type;
		}

		/// <summary>
		/// Visitation function for Ldsfld instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldsfld(InstructionNode node)
		{
			var fieldType = node.MosaField.FieldType;
			var destination = node.Result;

			var size = GetInstructionSize(fieldType);
			var fieldOperand = Operand.CreateField(node.MosaField);

			if (MosaTypeLayout.IsStoredOnStack(fieldType))
			{
				node.SetInstruction(IRInstruction.LoadCompound, destination, fieldOperand, ConstantZero);
				node.MosaType = fieldType;
			}
			else
			{
				var loadInstruction = GetLoadInstruction(fieldType);
				node.SetInstruction(loadInstruction, size, destination, fieldOperand, ConstantZero);
				node.MosaType = fieldType;
			}
		}

		/// <summary>
		/// Visitation function for Ldsflda instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldsflda(InstructionNode node)
		{
			node.SetInstruction(IRInstruction.AddressOf, node.Result, Operand.CreateField(node.MosaField));
		}

		/// <summary>
		/// Visitation function for Ldstr instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldstr(InstructionNode node)
		{
			/*
			 * This requires a special memory layout for strings as they are interned by the compiler
			 * into the generated image. This won't work this way forever: As soon as we'll support
			 * a real AppDomain and real string interning, this code will have to go away and will
			 * be replaced by a proper VM call.
			 */

			var linker = MethodCompiler.Linker;
			string symbolName = node.Operand1.Name;
			string stringdata = node.Operand1.StringData;

			node.SetInstruction(IRInstruction.MoveInteger, node.Result, node.Operand1);

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
		/// <param name="node">The node.</param>
		private void Ldvirtftn(InstructionNode node)
		{
			ReplaceWithVmCall(node, VmCall.GetVirtualFunctionPtr);
		}

		/// <summary>
		/// Visitation function for Leave instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <exception cref="InvalidCompilerException"></exception>
		private void Leave(InstructionNode node)
		{
			throw new InvalidCompilerException();
		}

		/// <summary>
		/// Visitation function for Mul instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Mul(InstructionNode node)
		{
			Replace(node, IRInstruction.MulFloatR4, IRInstruction.MulFloatR8, IRInstruction.MulSigned, IRInstruction.MulUnsigned);
		}

		/// <summary>
		/// Visitation function for Neg instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		private void Neg(InstructionNode node)
		{
			//FUTURE: Add IRInstruction.Negate
			if (node.Operand1.IsUnsigned)
			{
				var zero = Operand.CreateConstant(node.Operand1.Type, 0);
				node.SetInstruction(IRInstruction.SubUnsigned, node.Result, zero, node.Operand1);
			}
			else if (node.Operand1.IsR4)
			{
				var minusOne = Operand.CreateConstant(TypeSystem, -1.0f);
				node.SetInstruction(IRInstruction.MulFloatR4, node.Result, minusOne, node.Operand1);
			}
			else if (node.Operand1.IsR8)
			{
				var minusOne = Operand.CreateConstant(TypeSystem, -1.0d);
				node.SetInstruction(IRInstruction.MulFloatR8, node.Result, minusOne, node.Operand1);
			}
			else
			{
				var minusOne = Operand.CreateConstant(node.Operand1.Type, -1);
				node.SetInstruction(IRInstruction.MulSigned, node.Result, minusOne, node.Operand1);
			}
		}

		/// <summary>
		/// Visitation function for Newarr instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		private void Newarr(InstructionNode node)
		{
			var result = node.Result;
			var arrayType = result.Type;
			var elements = node.Operand1;

			GetTypeRequirements(arrayType.ElementType, out int elementSize, out int alignment);

			Debug.Assert(elementSize != 0);

			var runtimeTypeHandle = GetRuntimeTypeHandle(arrayType);
			var size = Operand.CreateConstant(TypeSystem, elementSize);
			node.SetInstruction(IRInstruction.NewArray, result, runtimeTypeHandle, size, elements);
		}

		/// <summary>
		/// Visitation function for Newobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Newobj(Context context)
		{
			if (ReplaceWithInternalCall(context.Node))
				return;

			var classType = context.InvokeMethod.DeclaringType;
			var result = context.Result;
			var method = context.InvokeMethod;
			var operands = new List<Operand>(context.Operands);

			var before = context.InsertBefore();

			// If the type is value type we don't need to call AllocateObject
			if (MosaTypeLayout.IsStoredOnStack(result.Type))
			{
				Debug.Assert(result.Uses.Count <= 1, "Usages too high");

				var newThis = MethodCompiler.CreateVirtualRegister(result.Type.ToManagedPointer());
				before.SetInstruction(IRInstruction.AddressOf, newThis, result);

				operands.Insert(0, newThis);
			}
			else if (result.Type.IsValueType)
			{
				Debug.Assert(result.Uses.Count <= 1, "Usages too high");

				var newThisLocal = MethodCompiler.AddStackLocal(result.Type);
				var newThis = MethodCompiler.CreateVirtualRegister(result.Type.ToManagedPointer());
				before.SetInstruction(IRInstruction.AddressOf, newThis, newThisLocal);

				var size = GetInstructionSize(newThisLocal.Type);
				var loadInstruction = GetLoadInstruction(newThisLocal.Type);

				context.InsertAfter().SetInstruction(loadInstruction, result, StackFrame, newThisLocal);

				operands.Insert(0, newThis);
			}
			else
			{
				Debug.Assert(result.Type.IsReferenceType, $"VmCall.AllocateObject only needs to be called for reference types. Type: {result.Type}");

				var runtimeTypeHandle = GetRuntimeTypeHandle(classType);
				var size = Operand.CreateConstant(TypeSystem, TypeLayout.GetTypeSize(classType));
				before.SetInstruction(IRInstruction.NewObject, result, runtimeTypeHandle, size);

				operands.Insert(0, result);
			}

			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);
			context.SetInstruction(IRInstruction.CallStatic, null, symbol);
			context.AppendOperands(operands);
		}

		/// <summary>
		/// Visitation function for Nop instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		private void Nop(InstructionNode node)
		{
			node.SetInstruction(IRInstruction.Nop);
		}

		/// <summary>
		/// Visitation function for Not instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		private void Not(InstructionNode node)
		{
			node.SetInstruction(IRInstruction.LogicalNot, node.Result, node.Operand1);
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
		/// <param name="node">The context.</param>
		private void Pop(InstructionNode node)
		{
			node.Empty();
		}

		/// <summary>
		/// Visitation function for Rem instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Rem(InstructionNode node)
		{
			Replace(node, IRInstruction.RemFloatR4, IRInstruction.RemFloatR8, IRInstruction.RemSigned, IRInstruction.RemUnsigned);
		}

		/// <summary>
		/// Visitation function for Ret instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ret(InstructionNode node)
		{
			node.ReplaceInstruction(IRInstruction.Return);
		}

		/// <summary>
		/// Visitation function for Rethrow instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Rethrow(InstructionNode node)
		{
			ReplaceWithVmCall(node, VmCall.Rethrow);
		}

		/// <summary>
		/// Visitation function for Shift instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <exception cref="InvalidCompilerException"></exception>
		private void Shift(InstructionNode node)
		{
			switch ((node.Instruction as BaseCILInstruction).OpCode)
			{
				case OpCode.Shl: node.SetInstruction(IRInstruction.ShiftLeft, node.Result, node.Operand1, node.Operand2); break;
				case OpCode.Shr: node.SetInstruction(IRInstruction.ArithmeticShiftRight, node.Result, node.Operand1, node.Operand2); break;
				case OpCode.Shr_un: node.SetInstruction(IRInstruction.ShiftRight, node.Result, node.Operand1, node.Operand2); break;
				default: throw new InvalidCompilerException();
			}
		}

		/// <summary>
		/// Visitation function for Sizeof instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Sizeof(InstructionNode node)
		{
			var type = node.MosaType;
			var size = type.IsPointer ? NativePointerSize : MethodCompiler.TypeLayout.GetTypeSize(type);
			node.SetInstruction(IRInstruction.MoveInteger, node.Result, Operand.CreateConstant(TypeSystem, size));
		}

		/// <summary>
		/// Visitation function for Starg instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Starg(InstructionNode node)
		{
			Debug.Assert(node.Result.IsParameter);

			if (MosaTypeLayout.IsStoredOnStack(node.Operand1.Type))
			{
				node.SetInstruction(IRInstruction.StoreParameterCompound, node.Size, null, node.Result, node.Operand1);
				node.MosaType = node.Result.Type; // may not be necessary
			}
			else
			{
				var storeInstruction = GetStoreParameterInstruction(node.Operand1.Type);
				node.SetInstruction(storeInstruction, node.Size, null, node.Result, node.Operand1);
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
		/// <param name="node">The node.</param>
		private void Stfld(InstructionNode node)
		{
			var objectOperand = node.Operand1;
			var valueOperand = node.Operand2;
			var fieldType = node.MosaField.FieldType;

			int offset = TypeLayout.GetFieldOffset(node.MosaField);
			var offsetOperand = Operand.CreateConstant(TypeSystem, offset);

			var size = GetInstructionSize(fieldType);

			if (MosaTypeLayout.IsStoredOnStack(fieldType))
			{
				node.SetInstruction(IRInstruction.StoreCompound, size, null, objectOperand, offsetOperand, valueOperand);
				node.MosaType = fieldType;
			}
			else
			{
				var storeInstruction = GetStoreInstruction(fieldType);
				node.SetInstruction(storeInstruction, size, null, objectOperand, offsetOperand, valueOperand);
				node.MosaType = fieldType;
			}
		}

		/// <summary>
		/// Visitation function for Stloc instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Stloc(InstructionNode node)
		{
			var type = node.Operand1.Type;
			var size = GetInstructionSize(type);

			if (node.Result.IsVirtualRegister && node.Operand1.IsVirtualRegister)
			{
				var moveInstruction = GetMoveInstruction(node.Result.Type);

				node.SetInstruction(moveInstruction, size, node.Result, node.Operand1);
				return;
			}

			if (MosaTypeLayout.IsStoredOnStack(type))
			{
				Debug.Assert(!node.Result.IsVirtualRegister);
				node.SetInstruction(IRInstruction.MoveCompound, node.Result, node.Operand1);
			}
			else if (node.Operand1.IsVirtualRegister)
			{
				var storeInstruction = GetStoreInstruction(type);

				node.SetInstruction(storeInstruction, size, null, StackFrame, node.Result, node.Operand1);
			}

			node.MosaType = type;
		}

		/// <summary>
		/// Visitation function for Stobj instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		private void Stobj(InstructionNode node)
		{
			// This is actually stind.* and stobj - the opcodes have the same meanings
			var type = node.MosaType;  // pass thru

			if (MosaTypeLayout.IsStoredOnStack(type))
			{
				node.SetInstruction(IRInstruction.StoreCompound, null, node.Operand1, ConstantZero, node.Operand2);
			}
			else
			{
				var size = GetInstructionSize(type);
				var storeInstruction = GetStoreInstruction(type);

				node.SetInstruction(storeInstruction, size, null, node.Operand1, ConstantZero, node.Operand2);
			}

			node.MosaType = type;
		}

		/// <summary>
		/// Visitation function for Stsfld instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		private void Stsfld(InstructionNode node)
		{
			var field = node.MosaField;
			var size = GetInstructionSize(field.FieldType);
			var fieldOperand = Operand.CreateField(field);

			if (MosaTypeLayout.IsStoredOnStack(field.FieldType))
			{
				node.SetInstruction(IRInstruction.StoreCompound, size, null, fieldOperand, ConstantZero, node.Operand1);
				node.MosaType = field.FieldType;
			}
			else
			{
				var storeInstruction = GetStoreInstruction(node.Operand1.Type);

				node.SetInstruction(storeInstruction, size, null, fieldOperand, ConstantZero, node.Operand1);
				node.MosaType = field.FieldType;
			}
		}

		/// <summary>
		/// Visitation function for Sub instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Sub(InstructionNode node)
		{
			Replace(node, IRInstruction.SubFloatR4, IRInstruction.SubFloatR8, IRInstruction.SubSigned, IRInstruction.SubUnsigned);
		}

		/// <summary>
		/// Visitation function for Switch instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Switch(InstructionNode node)
		{
			node.ReplaceInstruction(IRInstruction.Switch);
		}

		/// <summary>
		/// Visitation function for Throw instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Throw(InstructionNode node)
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

				context.ReplaceInstruction(moveInstruction);
				return;
			}

			int typeSize = TypeLayout.GetTypeSize(type);
			typeSize = Alignment.AlignUp(typeSize, TypeLayout.NativePointerAlignment);

			var vmCall = ToVmUnboxCall(typeSize);

			context.SetInstruction(IRInstruction.Nop);
			ReplaceWithVmCall(context.Node, vmCall);

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

				context.ReplaceInstruction(moveInstruction);
				return;
			}

			int typeSize = TypeLayout.GetTypeSize(type);
			typeSize = Alignment.AlignUp(typeSize, TypeLayout.NativePointerAlignment);

			var vmCall = ToVmUnboxCall(typeSize);

			context.SetInstruction(IRInstruction.Nop);
			ReplaceWithVmCall(context.Node, vmCall);

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

		private static void Replace(InstructionNode node, BaseInstruction floatingPointR4Instruction, BaseInstruction floatingPointR8Instruction, BaseInstruction signedInstruction, BaseInstruction unsignedInstruction)
		{
			if (node.Result.IsR4)
			{
				node.ReplaceInstruction(floatingPointR4Instruction);
			}
			else if (node.Result.IsR8)
			{
				node.ReplaceInstruction(floatingPointR8Instruction);
			}
			else if (node.Result.IsUnsigned)
			{
				node.ReplaceInstruction(unsignedInstruction);
			}
			else
			{
				node.ReplaceInstruction(signedInstruction);
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
			GetTypeRequirements(arrayType.ElementType, out int size, out int alignment);

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
				context.ReplaceInstruction(IRInstruction.IntrinsicMethodCall);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Replaces the IL load instruction by an appropriate IR move instruction or removes it entirely, if
		/// it is a native size.
		/// </summary>
		/// <param name="node">Provides the transformation context.</param>
		private void ProcessLoadInstruction(InstructionNode node)
		{
			var destination = node.Result;
			var source = node.Operand1;
			var size = GetInstructionSize(source.Type);

			if (MosaTypeLayout.IsStoredOnStack(source.Type))
			{
				node.SetInstruction(IRInstruction.MoveCompound, destination, source);
			}
			else if (!source.IsVirtualRegister)
			{
				var loadInstruction = GetLoadInstruction(source.Type);

				node.SetInstruction(loadInstruction, size, destination, StackFrame, source);
			}
			else
			{
				var moveInstruction = GetMoveInstruction(source.Type);

				node.SetInstruction(moveInstruction, size, destination, source);
			}
		}

		private bool ReplaceWithInternalCall(InstructionNode node)
		{
			var method = node.InvokeMethod;

			if (!method.IsInternal)
				return false;

			string replacementMethod = (method.Name == ".ctor") ? "Create" + method.DeclaringType.Name : "Internal" + method.Name;

			method = method.DeclaringType.FindMethodByNameAndParameters(replacementMethod, method.Signature.Parameters);

			var result = node.Result;
			var operands = node.GetOperands();
			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			node.SetInstruction(IRInstruction.CallStatic, result, symbol);
			node.AppendOperands(operands);

			return true;
		}

		/// <summary>
		/// Replaces the instruction with an internal call.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="internalCallTarget">The internal call target.</param>
		private void ReplaceWithVmCall(InstructionNode node, VmCall internalCallTarget)
		{
			var method = InternalRuntimeType.FindMethodByName(internalCallTarget.ToString()) ?? PlatformInternalRuntimeType.FindMethodByName(internalCallTarget.ToString());

			Debug.Assert(method != null, "Cannot find method: " + internalCallTarget.ToString());

			node.ReplaceInstruction(IRInstruction.Call);
			node.SetOperand(0, Operand.CreateSymbolFromMethod(TypeSystem, method));
			node.OperandCount = 1;
			node.InvokeMethod = method;
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
