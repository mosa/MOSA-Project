// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Transforms CIL instructions into their appropriate Manual.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms CIL instructions into their equivalent IR sequences.
	/// </remarks>
	public sealed class CILTransformationStage : BaseCodeTransformationStageLegacy
	{
		private readonly Dictionary<MosaMethod, IntrinsicMethodDelegate> InstrinsicMap = new Dictionary<MosaMethod, IntrinsicMethodDelegate>();

		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(CILInstruction.Add, Add);
			AddVisitation(CILInstruction.Add_ovf, AddOverflow);
			AddVisitation(CILInstruction.Add_ovf_un, AddOverflowUnsigned);
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
			AddVisitation(CILInstruction.Conv_ovf_i, CheckedConversion);
			AddVisitation(CILInstruction.Conv_ovf_i1, CheckedConversion);
			AddVisitation(CILInstruction.Conv_ovf_i2, CheckedConversion);
			AddVisitation(CILInstruction.Conv_ovf_i4, CheckedConversion);
			AddVisitation(CILInstruction.Conv_ovf_i8, CheckedConversion);
			AddVisitation(CILInstruction.Conv_ovf_u, CheckedConversion);
			AddVisitation(CILInstruction.Conv_ovf_u1, CheckedConversion);
			AddVisitation(CILInstruction.Conv_ovf_u2, CheckedConversion);
			AddVisitation(CILInstruction.Conv_ovf_u4, CheckedConversion);
			AddVisitation(CILInstruction.Conv_ovf_u8, CheckedConversion);
			AddVisitation(CILInstruction.Conv_ovf_i_un, CheckedConversionUnsigned);
			AddVisitation(CILInstruction.Conv_ovf_i1_un, CheckedConversionUnsigned);
			AddVisitation(CILInstruction.Conv_ovf_i2_un, CheckedConversionUnsigned);
			AddVisitation(CILInstruction.Conv_ovf_i4_un, CheckedConversionUnsigned);
			AddVisitation(CILInstruction.Conv_ovf_i8_un, CheckedConversionUnsigned);
			AddVisitation(CILInstruction.Conv_ovf_u_un, CheckedConversionUnsigned);
			AddVisitation(CILInstruction.Conv_ovf_u1_un, CheckedConversionUnsigned);
			AddVisitation(CILInstruction.Conv_ovf_u2_un, CheckedConversionUnsigned);
			AddVisitation(CILInstruction.Conv_ovf_u4_un, CheckedConversionUnsigned);
			AddVisitation(CILInstruction.Conv_ovf_u8_un, CheckedConversionUnsigned);
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
			AddVisitation(CILInstruction.Mul_ovf, MulOverflow);
			AddVisitation(CILInstruction.Mul_ovf_un, MulOverflowUnsigned);
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
			AddVisitation(CILInstruction.Sub_ovf, SubOverflow);
			AddVisitation(CILInstruction.Sub_ovf_un, SubOverflowUnsigned);
			AddVisitation(CILInstruction.Switch, Switch);
			AddVisitation(CILInstruction.Throw, Throw);
			AddVisitation(CILInstruction.Unbox, Unbox);
			AddVisitation(CILInstruction.Unbox_any, Unbox);
			AddVisitation(CILInstruction.Xor, BinaryLogic);

			AddVisitation(CILInstruction.PreReadOnly, PreReadOnly);

			//AddVisitation(CILInstruction.Arglist,Arglist);
			//AddVisitation(CILInstruction.Ckfinite,Ckfinite);
			//AddVisitation(CILInstruction.Cpobj,Cpobj);
			//AddVisitation(CILInstruction.Jmp,Jmp);
			//AddVisitation(CILInstruction.Localalloc,Localalloc);
			//AddVisitation(CILInstruction.Mkrefany,Mkrefany);
			//AddVisitation(CILInstruction.PreConstrained,PreConstrained);
			//AddVisitation(CILInstruction.PreNo,PreNo);
			//AddVisitation(CILInstruction.PreReadOnly,PreReadOnly);
			//AddVisitation(CILInstruction.PreTail,PreTail);
			//AddVisitation(CILInstruction.PreUnaligned,PreUnaligned);
			//AddVisitation(CILInstruction.PreVolatile,PreVolatile);
			//AddVisitation(CILInstruction.Refanytype,Refanytype);
			//AddVisitation(CILInstruction.Refanyval,Refanyval);
		}

		protected override void Run()
		{
			if (!MethodCompiler.IsCILStream)
				return;

			base.Run();
		}

		#region Visitation Methods

		/// <summary>
		/// Visitation function for Add instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Add(InstructionNode node)
		{
			Replace(node, IRInstruction.Add32, IRInstruction.Add64, IRInstruction.AddR8, IRInstruction.AddR4);
		}

		/// <summary>
		/// Visitation function for Add Overflow instruction
		/// </summary>
		/// <param name="node">The node.</param>
		private void AddOverflow(InstructionNode node)
		{
			var overflowResult = AllocateVirtualRegister(TypeSystem.BuiltIn.Boolean);

			node.SetInstruction2(Select(node.Result, IRInstruction.AddOverflowOut32, IRInstruction.AddOverflowOut64), node.Result, overflowResult, node.Operand1, node.Operand2);

			AddOverflowCheck(node, overflowResult);
		}

		/// <summary>
		/// Visitation function for Add Overflow Unsigned instruction
		/// </summary>
		/// <param name="node">The node.</param>
		private void AddOverflowUnsigned(InstructionNode node)
		{
			var carryResult = AllocateVirtualRegister(TypeSystem.BuiltIn.Boolean);

			node.SetInstruction2(Select(node.Result, IRInstruction.AddCarryOut32, IRInstruction.AddCarryOut64), node.Result, carryResult, node.Operand1, node.Operand2);

			AddOverflowCheck(node, carryResult);
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

			if (first.IsFloatingPoint)
			{
				var result = Is32BitPlatform ? AllocateVirtualRegisterI32() : AllocateVirtualRegisterI64();
				var instruction = (first.IsR4) ? (BaseInstruction)IRInstruction.CompareR4 : IRInstruction.CompareR8;

				context.SetInstruction(instruction, cc, result, first, second);
				context.AppendInstruction(Select(result, IRInstruction.Branch32, IRInstruction.Branch64), ConditionCode.NotEqual, null, result, ConstantZero32, target); // TODO: Constant should be 64bit
			}
			else
			{
				context.SetInstruction(Select(first, IRInstruction.Branch32, IRInstruction.Branch64), cc, null, first, second, target);
			}

			//context.AddBranchTarget(target);
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

			BaseInstruction instruction = IRInstruction.Compare32x32;

			if (first.IsReferenceType)
				instruction = IRInstruction.CompareObject;
			else if (result.IsInteger64 && first.IsInteger64)
				instruction = IRInstruction.Compare64x64;
			else if (result.IsInteger64 && first.IsInteger32)
				instruction = IRInstruction.Compare32x64;
			else if (result.IsInteger32 && first.IsInteger64)
				instruction = IRInstruction.Compare64x32;
			else if (result.IsInteger32 && first.IsInteger32)
				instruction = IRInstruction.Compare32x32;
			else if (first.IsR4)
				instruction = IRInstruction.CompareR4;
			else if (first.IsR8)
				instruction = IRInstruction.CompareR8;

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
				var operand = Operand.CreateStaticField(type.Fields[0], TypeSystem);
				node.SetOperand(0, operand);
			}

			if (node.Operand2.Type.IsEnum)
			{
				var type = node.Operand2.Type;
				var operand = Operand.CreateStaticField(type.Fields[0], TypeSystem);
				node.SetOperand(1, operand);
			}

			switch ((node.Instruction as BaseCILInstruction).OpCode)
			{
				case OpCode.And: node.SetInstruction(Select(node.Result, IRInstruction.And32, IRInstruction.And64), node.Result, node.Operand1, node.Operand2); break;
				case OpCode.Or: node.SetInstruction(Select(node.Result, IRInstruction.Or32, IRInstruction.Or64), node.Result, node.Operand1, node.Operand2); break;
				case OpCode.Xor: node.SetInstruction(Select(node.Result, IRInstruction.Xor32, IRInstruction.Xor64), node.Result, node.Operand1, node.Operand2); break;
				case OpCode.Div_un: node.SetInstruction(Select(node.Result, IRInstruction.DivUnsigned32, IRInstruction.DivUnsigned64), node.Result, node.Operand1, node.Operand2); break;
				case OpCode.Rem_un: node.SetInstruction(Select(node.Result, IRInstruction.RemUnsigned32, IRInstruction.RemUnsigned64), node.Result, node.Operand1, node.Operand2); break;
				default: throw new CompilerException();
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
				context.SetInstruction(moveInstruction, context.Result, context.Operand1);
				return;
			}

			var typeSize = Alignment.AlignUp((uint)TypeLayout.GetTypeSize(type), TypeLayout.NativePointerAlignment);
			var runtimeType = GetRuntimeTypeHandle(type);

			if (typeSize <= 8 || type.IsR)
			{
				BaseIRInstruction instruction;

				if (type.IsR4)
					instruction = IRInstruction.BoxR4;
				else if (type.IsR8)
					instruction = IRInstruction.BoxR8;
				else if (typeSize <= 4)
					instruction = IRInstruction.Box32;
				else if (typeSize == 8)
					instruction = IRInstruction.Box64;
				else
					throw new InvalidOperationException();

				context.SetInstruction(instruction, result, runtimeType, value);
			}
			else
			{
				var adr = AllocateVirtualRegister(type.ToManagedPointer());

				context.SetInstruction(IRInstruction.AddressOf, adr, value);
				context.AppendInstruction(IRInstruction.Box, result, runtimeType, adr, CreateConstant32(typeSize));
			}
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
		/// <param name="context">The context.</param>
		private void Break(Context context)
		{
			context.Empty();
		}

		private uint CalculateInterfaceSlot(MosaType interaceType)
		{
			return TypeLayout.GetInterfaceSlot(interaceType);
		}

		/// <summary>
		/// Visitation function for Call instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Call(Context context)
		{
			if (ProcessExternalCall(context))
				return;

			var method = context.InvokeMethod;
			var result = context.Result;

			var operands = new List<Operand>(context.Operands);
			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			context.SetInstruction(IRInstruction.CallStatic, result, symbol, operands);
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
			var operands = context.GetOperands();

			var previous = context.Node.Previous;

			if (previous.Instruction is ConstrainedPrefixInstruction)
			{
				var type = context.Node.Previous.MosaType;

				// remove constrained prefix
				previous.Empty();

				if (type.IsValueType)
				{
					method = GetMethodOrOverride(type, method);

					if (!type.Methods.Contains(method))
					{
						var elementType = context.Operand1.Type.ElementType;
						var typeSize = Alignment.AlignUp(TypeLayout.GetTypeSize(elementType), TypeLayout.NativePointerAlignment);

						// Create a virtual register to hold our boxed value
						var boxedValue = AllocateVirtualRegister(TypeSystem.BuiltIn.Object);

						var before = context.InsertBefore();
						before.SetInstruction(IRInstruction.Box, boxedValue, GetRuntimeTypeHandle(type), context.Operand1, CreateConstant32(typeSize));

						// Now replace the value type pointer with the boxed value virtual register
						context.Operand1 = boxedValue;
					}

					var symbol2 = Operand.CreateSymbolFromMethod(method, TypeSystem);
					context.SetInstruction(IRInstruction.CallStatic, result, symbol2, operands);
					return;
				}
			}

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			if (method.IsVirtual)
			{
				if (method.DeclaringType.IsInterface)
				{
					context.SetInstruction(IRInstruction.CallInterface, result, symbol, operands);
				}
				else
				{
					context.SetInstruction(IRInstruction.CallVirtual, result, symbol, operands);
				}
			}
			else
			{
				// FIXME: Callvirt imposes a null-check. For virtual calls this is done implicitly, but for non-virtual calls
				// we have to make this explicitly somehow.
				context.SetInstruction(IRInstruction.CallStatic, result, symbol, operands);
			}
		}

		/// <summary>
		/// Visitation function for Castclass instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Castclass(InstructionNode node)
		{
			// TODO!
			//ReplaceWithVmCall(context, VmCall.Castclass);
			node.ReplaceInstruction(IRInstruction.MoveObject); // HACK!
		}

		/// <summary>
		/// Visitation function for Conversion instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Conversion(Context context)
		{
			var result = context.Result;
			var source = context.Operand1;
			var type = context.MosaType;

			int destIndex = GetIndex(type ?? result.Type);
			int srcIndex = GetIndex(source.Type);

			var conversion = Is32BitPlatform ? ConversionTable32[destIndex][srcIndex] : ConversionTable64[destIndex][srcIndex];

			ulong mask = GetBitMask(conversion.BitsToMask);

			if (mask == 0 && conversion.PostInstruction != null)
			{
				var temp = AllocateVirtualRegister(result);

				context.SetInstruction(conversion.Instruction, temp, source);
				context.AppendInstruction(conversion.PostInstruction, result, temp);
			}
			else if (mask == 0)
			{
				context.SetInstruction(conversion.Instruction, result, source);
			}
			else if (conversion.PostInstruction == null)
			{
				context.SetInstruction(conversion.Instruction, result, source, CreateConstant64(mask));
			}
			else
			{
				var temp = AllocateVirtualRegister(result);

				context.SetInstruction(conversion.Instruction, temp, source);
				context.AppendInstruction(conversion.PostInstruction, result, temp, CreateConstant64(mask));
			}
		}

		/// <summary>
		/// Visitation function for Conversion instruction from signed source.
		/// </summary>
		/// <param name="context">The context.</param>
		private void CheckedConversion(Context context)
		{
			var result = context.Result;
			var source = context.Operand1;
			var type = context.MosaType;

			// First check to see if we have a matching checked conversion function

			var sourceTypeString = (source.Type.IsI4) ? "I4" :
				(source.Type.IsI8) ? "I8" :
				(source.Type.IsR4) ? "R4" :
				(source.Type.IsR8) ? "R8" :
				(source.Type.IsI) ? Is32BitPlatform ? "I4" : "I8" :
				(source.Type.IsPointer) ? Is32BitPlatform ? "I4" : "I8" :
				(!source.Type.IsValueType) ? Is32BitPlatform ? "I4" : "I8" :
				throw new CompilerException();

			var resultTypeString = type.IsU || type.IsI || type.IsPointer ? Is32BitPlatform ? "I4" : "I8" : type.TypeCode.ToString();

			var methodName = $"{sourceTypeString}To{resultTypeString}";
			var method = GetMethod("Mosa.Runtime.Math", "CheckedConversion", methodName);

			Debug.Assert(method != null);

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			context.SetInstruction(IRInstruction.CallStatic, result, symbol, source);

			MethodScanner.MethodInvoked(method, Method);
		}

		/// <summary>
		/// Visitation function for Conversion instruction from unsigned source.
		/// </summary>
		/// <param name="context">The context.</param>
		private void CheckedConversionUnsigned(Context context)
		{
			var result = context.Result;
			var source = context.Operand1;
			var type = context.MosaType;

			// First check to see if we have a matching checked conversion function

			var sourceTypeString = (source.Type.IsI4) ? "U4" :
				(source.Type.IsI8) ? "U8" :
				(source.Type.IsR4) ? "R4" :
				(source.Type.IsR8) ? "R8" :
				(source.Type.IsI) ? Is32BitPlatform ? "U4" : "U8" :
				(source.Type.IsPointer) ? Is32BitPlatform ? "U4" : "U8" :
				(!source.Type.IsValueType) ? Is32BitPlatform ? "U4" : "U8" :
				throw new CompilerException();

			var resultTypeString = type.IsU || type.IsI || type.IsPointer ? Is32BitPlatform ? "U4" : "U8" : type.TypeCode.ToString();

			var methodName = $"{sourceTypeString}To{resultTypeString}";
			var method = GetMethod("Mosa.Runtime.Math", "CheckedConversion", methodName);

			Debug.Assert(method != null);

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			context.SetInstruction(IRInstruction.CallStatic, result, symbol, source);

			MethodScanner.MethodInvoked(method, Method);
		}

		/// <summary>
		/// Visitation function for Cpblk instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Cpblk(InstructionNode node)
		{
			Debug.Assert(node.ResultCount == 0);
			node.ReplaceInstruction(IRInstruction.MemoryCopy);
		}

		/// <summary>
		/// Visitation function for Div instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Div(InstructionNode node)
		{
			Replace(node, IRInstruction.DivSigned32, IRInstruction.DivSigned64, IRInstruction.DivR8, IRInstruction.DivR4);
		}

		/// <summary>
		/// Visitation function for Dup instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Dup(InstructionNode node)
		{
			throw new CompilerException("Dup(): unexpected CIL.DUP instruction");
		}

		/// <summary>
		/// Visitation function for Endfilter instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Endfilter(InstructionNode node)
		{
			throw new CompilerException();
		}

		/// <summary>
		/// Visitation function for Endfinally instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Endfinally(InstructionNode node)
		{
			throw new CompilerException();
		}

		/// <summary>
		/// Visitation function for Initblk instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Initblk(InstructionNode node)
		{
			node.ReplaceInstruction(IRInstruction.MemorySet);
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
				node.SetInstruction(IRInstruction.StoreObject, null, ptr, ConstantZero, Operand.GetNullObject(TypeSystem));
				node.MosaType = type;
			}
			else
			{
				var size = CreateConstant32(TypeLayout.GetTypeSize(type));
				node.SetInstruction(IRInstruction.MemorySet, null, ptr, ConstantZero, size);
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
			var type = result.Type;

			if (type.IsValueType)
			{
				// FIXME:
				var adr = AllocateVirtualRegister(type.ToManagedPointer());

				//var context = new Context(node).InsertBefore();

				//context.SetInstruction(IRInstruction.AddressOf, adr, value);
				//context.AppendInstruction(IRInstruction.Box, result, runtimeType, adr, CreateConstant(typeSize));
			}

			if (!classType.IsInterface)
			{
				node.SetInstruction(IRInstruction.IsInstanceOfType, result, GetRuntimeTypeHandle(classType), reference);
			}
			else
			{
				var slot = CalculateInterfaceSlot(classType);
				node.SetInstruction(IRInstruction.IsInstanceOfInterfaceType, result, CreateConstant32(slot), reference);
			}
		}

		/// <summary>
		/// Visitation function for Ldarg instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldarg(InstructionNode node)
		{
			Debug.Assert(node.Operand1.IsParameter);

			var type = MosaTypeLayout.GetUnderlyingType(node.Operand1.Type);

			if (type != null && MosaTypeLayout.CanFitInRegister(type))
			{
				var loadInstruction = GetLoadParameterInstruction(type);

				node.SetInstruction(loadInstruction, node.Result, node.Operand1);
			}
			else
			{
				node.SetInstruction(IRInstruction.LoadParamCompound, node.Result, node.Operand1);
				node.MosaType = node.Operand1.Type;
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

			Debug.Assert(MosaTypeLayout.CanFitInRegister(destination.Type));
			var moveInstruction = GetMoveInstruction(destination.Type);
			node.SetInstruction(moveInstruction, destination, source);
		}

		/// <summary>
		/// Visitation function for Ldelem instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldelem(InstructionNode node)
		{
			var result = node.Result;
			var array = node.Operand1;
			var arrayIndex = node.Operand2;
			var arrayType = array.Type;

			// Array bounds check
			AddArrayBoundsCheck(node, array, arrayIndex);

			var elementOffset = CalculateArrayElementOffset(node, arrayType, arrayIndex);
			var totalElementOffset = CalculateTotalArrayOffset(node, elementOffset);

			Debug.Assert(elementOffset != null);

			if (MosaTypeLayout.CanFitInRegister(arrayType.ElementType))
			{
				var loadInstruction = GetLoadInstruction(arrayType.ElementType);

				node.SetInstruction(loadInstruction, result, array, totalElementOffset);
			}
			else
			{
				node.SetInstruction(IRInstruction.LoadCompound, result, array, totalElementOffset);
				node.MosaType = arrayType.ElementType;
			}
		}

		/// <summary>
		/// Visitation function for Ldelema instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldelema(InstructionNode node)
		{
			var result = node.Result;
			var array = node.Operand1;
			var arrayIndex = node.Operand2;
			var arrayType = array.Type;

			Debug.Assert(arrayType.ElementType == result.Type.ElementType);

			// Array bounds check
			AddArrayBoundsCheck(node, array, arrayIndex);

			var elementOffset = CalculateArrayElementOffset(node, arrayType, arrayIndex);
			var totalElementOffset = CalculateTotalArrayOffset(node, elementOffset);

			node.SetInstruction(Select(result, IRInstruction.Add32, IRInstruction.Add64), result, array, totalElementOffset);
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

			uint offset = TypeLayout.GetFieldOffset(field);
			bool isPointer = operand.IsPointer || operand.Type == TypeSystem.BuiltIn.I || operand.Type == TypeSystem.BuiltIn.U;

			if (!result.IsOnStack && MosaTypeLayout.CanFitInRegister(operand.Type) && !operand.IsReferenceType && isPointer)
			{
				var loadInstruction = GetLoadInstruction(field.FieldType);
				var fixedOffset = CreateConstant32(offset);

				context.SetInstruction(loadInstruction, result, operand, fixedOffset);

				return;
			}

			if (!result.IsOnStack && MosaTypeLayout.CanFitInRegister(operand.Type) && !operand.IsReferenceType && !isPointer)
			{
				// simple move
				Debug.Assert(result.IsVirtualRegister);

				var moveInstruction = GetMoveInstruction(field.FieldType);

				context.SetInstruction(moveInstruction, result, operand);

				return;
			}

			if (MosaTypeLayout.CanFitInRegister(result.Type) && operand.IsOnStack)
			{
				var loadInstruction = GetLoadInstruction(field.FieldType);
				var address = MethodCompiler.CreateVirtualRegister(operand.Type.ToUnmanagedPointer());
				var fixedOffset = CreateConstant32(offset);

				context.SetInstruction(IRInstruction.AddressOf, address, operand);
				context.AppendInstruction(loadInstruction, result, address, fixedOffset);

				return;
			}

			if (MosaTypeLayout.CanFitInRegister(result.Type) && !operand.IsOnStack)
			{
				var loadInstruction = GetLoadInstruction(field.FieldType);
				var fixedOffset = CreateConstant32(offset);

				context.SetInstruction(loadInstruction, result, operand, fixedOffset);

				return;
			}

			if (result.IsOnStack && !operand.IsOnStack)
			{
				var fixedOffset = CreateConstant32(offset);

				context.SetInstruction(IRInstruction.LoadCompound, result, operand, fixedOffset);
				context.MosaType = field.FieldType;

				return;
			}

			if (result.IsOnStack && operand.IsOnStack)
			{
				var address = MethodCompiler.CreateVirtualRegister(operand.Type.ToUnmanagedPointer());
				var fixedOffset = CreateConstant32(offset);

				context.SetInstruction(IRInstruction.AddressOf, address, operand);
				context.AppendInstruction(IRInstruction.LoadCompound, result, address, fixedOffset);

				return;
			}

			throw new CompilerException("ExpressionEvaluation: Error transforming CIL.Ldfld");
		}

		/// <summary>
		/// Visitation function for Ldflda instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldflda(InstructionNode node)
		{
			var field = node.MosaField;

			MethodScanner.AccessedField(field);

			uint offset = TypeLayout.GetFieldOffset(field);

			var fieldAddress = node.Result;
			var objectOperand = node.Operand1;

			if (offset == 0)
			{
				node.SetInstruction(Select(fieldAddress, IRInstruction.Move32, IRInstruction.Move64), fieldAddress, objectOperand);
			}
			else
			{
				node.SetInstruction(Select(fieldAddress, IRInstruction.Add32, IRInstruction.Add64), fieldAddress, objectOperand, CreateConstant32(offset));
			}
		}

		/// <summary>
		/// Visitation function for Ldftn instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldftn(InstructionNode node)
		{
			var invokedMethod = node.InvokeMethod;

			MethodScanner.MethodInvoked(invokedMethod, Method);

			node.SetInstruction(Select(node.Result, IRInstruction.Move32, IRInstruction.Move64), node.Result, Operand.CreateSymbolFromMethod(invokedMethod, TypeSystem));

			var methodData = MethodCompiler.Compiler.GetMethodData(invokedMethod);

			if (!methodData.HasMethodPointerReferenced)
			{
				methodData.HasMethodPointerReferenced = true;

				MethodScheduler.AddToRecompileQueue(methodData); // FUTURE: Optimize this not to re-schedule when not necessary

				//Debug.WriteLine($" Method Reference: [{MethodData.Version}] {invokedMethod}"); //DEBUGREMOVE
			}
		}

		/// <summary>
		/// Visitation function for Ldlen instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldlen(InstructionNode node)
		{
			node.SetInstruction(Select(node.Result, IRInstruction.Load32, IRInstruction.Load64), node.Result, node.Operand1, ConstantZero);
		}

		/// <summary>
		/// Replaces the IL load instruction by an appropriate IR move instruction or removes it entirely, if
		/// it is a native size.
		/// </summary>
		/// <param name="node">Provides the transformation context.</param>
		private void Ldloc(InstructionNode node)
		{
			var destination = node.Result;
			var source = node.Operand1;

			if (MosaTypeLayout.CanFitInRegister(source.Type))
			{
				if (!source.IsVirtualRegister)
				{
					var loadInstruction = GetLoadInstruction(source.Type);

					node.SetInstruction(loadInstruction, destination, StackFrame, source);
				}
				else
				{
					var moveInstruction = GetMoveInstruction(source.Type);

					node.SetInstruction(moveInstruction, destination, source);
				}
			}
			else
			{
				node.SetInstruction(IRInstruction.MoveCompound, destination, source);
			}
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

			if (MosaTypeLayout.CanFitInRegister(type))
			{
				var loadInstruction = GetLoadInstruction(type);

				node.SetInstruction(loadInstruction, destination, source, ConstantZero);
			}
			else
			{
				node.SetInstruction(IRInstruction.LoadCompound, destination, source, ConstantZero);
			}

			node.MosaType = type;
		}

		/// <summary>
		/// Visitation function for Ldsflda instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldsflda(InstructionNode node)
		{
			var field = node.MosaField;

			MethodScanner.AccessedField(field);

			if (field.FieldType.IsReferenceType)
			{
				// TODO
				node.Result = node.Result;
			}

			var fieldOperand = Operand.CreateStaticField(field, TypeSystem);

			node.SetInstruction(IRInstruction.AddressOf, node.Result, fieldOperand);
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

			var symbolName = node.Operand1.Name;
			var data = node.Operand1.StringData;

			var symbol = Linker.DefineSymbol(symbolName, SectionKind.ROData, NativeAlignment, (uint)(ObjectHeaderSize + NativePointerSize + (data.Length * 2)));
			var writer = new BinaryWriter(symbol.Stream);

			Linker.Link(LinkType.AbsoluteAddress, PatchType.I32, symbol, ObjectHeaderSize - NativePointerSize, Metadata.TypeDefinition + "System.String", 0);

			// 1. Object Header
			writer.WriteZeroBytes(ObjectHeaderSize);

			// 2. Length
			writer.Write(data.Length, NativePointerSize);

			// 3. Unicode
			writer.Write(Encoding.Unicode.GetBytes(data));

			node.SetInstruction(IRInstruction.MoveObject, node.Result, node.Operand1);
		}

		/// <summary>
		/// Visitation function for Ldtoken instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldtoken(Context context)
		{
			// TODO: remove VmCall.GetHandleForToken?

			Operand source;

			if (context.MosaType != null)
			{
				source = Operand.CreateUnmanagedSymbolPointer(Metadata.TypeDefinition + context.MosaType.FullName, TypeSystem);
			}
			else if (context.MosaField != null)
			{
				source = Operand.CreateUnmanagedSymbolPointer(Metadata.FieldDefinition + context.MosaField.FullName, TypeSystem);

				MethodScanner.AccessedField(context.MosaField);
			}
			else
			{
				throw new NotImplementCompilerException();
			}

			context.SetInstruction(Select(context.Result, IRInstruction.Move32, IRInstruction.Move64), context.Result, source);
		}

		/// <summary>
		/// Visitation function for Ldvirtftn instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldvirtftn(InstructionNode node)
		{
			node.ReplaceInstruction(IRInstruction.GetVirtualFunctionPtr);
		}

		/// <summary>
		/// Visitation function for Leave instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <exception cref="CompilerException"></exception>
		private void Leave(InstructionNode node)
		{
			throw new CompilerException();
		}

		/// <summary>
		/// Visitation function for Mul instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Mul(InstructionNode node)
		{
			Replace(node, IRInstruction.MulSigned32, IRInstruction.MulSigned64, IRInstruction.MulR8, IRInstruction.MulR4);
		}

		/// <summary>
		/// Visitation function for Mul Overflow instruction
		/// </summary>
		/// <param name="node">The node.</param>
		private void MulOverflow(InstructionNode node)
		{
			var overflowResult = AllocateVirtualRegister(TypeSystem.BuiltIn.Boolean);

			node.SetInstruction2(Select(node.Result, IRInstruction.MulOverflowOut32, IRInstruction.MulOverflowOut64), node.Result, overflowResult, node.Operand1, node.Operand2);

			AddOverflowCheck(node, overflowResult);
		}

		/// <summary>
		/// Visitation function for Mul Overflow Unsigned instruction
		/// </summary>
		/// <param name="node">The node.</param>
		private void MulOverflowUnsigned(InstructionNode node)
		{
			var carryResult = AllocateVirtualRegister(TypeSystem.BuiltIn.Boolean);

			node.SetInstruction2(Select(node.Result, IRInstruction.MulCarryOut32, IRInstruction.MulCarryOut64), node.Result, carryResult, node.Operand1, node.Operand2);

			AddOverflowCheck(node, carryResult);
		}

		/// <summary>
		/// Visitation function for Neg instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		private void Neg(InstructionNode node)
		{
			//FUTURE: Add IRInstruction.Negate
			if (node.Operand1.IsInteger)
			{
				node.SetInstruction(Select(node.Result, IRInstruction.Sub32, IRInstruction.Sub64), node.Result, ConstantZero, node.Operand1);
			}
			else if (node.Operand1.IsR4)
			{
				var minusOne = CreateConstantR4(-1.0f);
				node.SetInstruction(IRInstruction.MulR4, node.Result, minusOne, node.Operand1);
			}
			else if (node.Operand1.IsR8)
			{
				var minusOne = CreateConstantR8(-1.0d);
				node.SetInstruction(IRInstruction.MulR8, node.Result, minusOne, node.Operand1);
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

			var elementSize = GetTypeSize(arrayType.ElementType, false);

			Debug.Assert(elementSize != 0);

			var methodTable = GetMethodTablePointer(arrayType);
			var size = CreateConstant32(elementSize);
			node.SetInstruction(IRInstruction.NewArray, result, methodTable, size, elements);
			node.MosaType = arrayType;
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
			if (MosaTypeLayout.CanFitInRegister(result.Type))
			{
				if (result.IsValueType)
				{
					var newThisLocal = AddStackLocal(result.Type);
					var newThis = MethodCompiler.CreateVirtualRegister(result.Type.ToManagedPointer());
					before.SetInstruction(IRInstruction.AddressOf, newThis, newThisLocal);

					var loadInstruction = GetLoadInstruction(newThisLocal.Type);

					context.InsertAfter().SetInstruction(loadInstruction, result, StackFrame, newThisLocal);

					operands.Insert(0, newThis);
				}
				else
				{
					Debug.Assert(result.IsReferenceType);

					var methodTable = GetMethodTablePointer(classType);
					var size = CreateConstant32(TypeLayout.GetTypeSize(classType));
					before.SetInstruction(IRInstruction.NewObject, result, methodTable, size);
					before.MosaType = classType;

					operands.Insert(0, result);
				}
			}
			else
			{
				var newThis = MethodCompiler.CreateVirtualRegister(result.Type.ToManagedPointer());
				before.SetInstruction(IRInstruction.AddressOf, newThis, result);
				before.AppendInstruction(IRInstruction.Nop);

				operands.Insert(0, newThis);
			}

			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);
			context.SetInstruction(IRInstruction.CallStatic, null, symbol, operands);
		}

		/// <summary>
		/// Visitation function for Nop instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		private void Nop(InstructionNode node)
		{
			node.SetNop();
		}

		/// <summary>
		/// Visitation function for Not instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		private void Not(InstructionNode node)
		{
			var Not = Select(node.Result, IRInstruction.Not32, IRInstruction.Not64);

			node.SetInstruction(Not, node.Result, node.Operand1);
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

		private void PreReadOnly(Context context)
		{
			context.SetNop();
		}

		/// <summary>
		/// Visitation function for Rem instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Rem(InstructionNode node)
		{
			Replace(node, IRInstruction.RemSigned32, IRInstruction.RemSigned64, IRInstruction.RemR8, IRInstruction.RemR4);
		}

		/// <summary>
		/// Visitation function for Ret instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ret(Context context)
		{
			var operand1 = context.Operand1;

			if (operand1 != null)
			{
				var setReturn = GetSetReturnInstruction(operand1.Type, Is32BitPlatform);

				context.SetInstruction(setReturn, null, operand1);

				context.AppendInstruction(IRInstruction.Jmp, BasicBlocks.EpilogueBlock);
			}
			else
			{
				context.SetInstruction(IRInstruction.Jmp, BasicBlocks.EpilogueBlock);
			}
		}

		/// <summary>
		/// Visitation function for Rethrow instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Rethrow(InstructionNode node)
		{
			node.ReplaceInstruction(IRInstruction.Rethrow);
		}

		/// <summary>
		/// Visitation function for Shift instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <exception cref="CompilerException"></exception>
		private void Shift(InstructionNode node)
		{
			switch ((node.Instruction as BaseCILInstruction).OpCode)
			{
				case OpCode.Shl: node.SetInstruction(Select(node.Result, IRInstruction.ShiftLeft32, IRInstruction.ShiftLeft64), node.Result, node.Operand1, node.Operand2); break;
				case OpCode.Shr: node.SetInstruction(Select(node.Result, IRInstruction.ArithShiftRight32, IRInstruction.ArithShiftRight64), node.Result, node.Operand1, node.Operand2); break;
				case OpCode.Shr_un: node.SetInstruction(Select(node.Result, IRInstruction.ShiftRight32, IRInstruction.ShiftRight64), node.Result, node.Operand1, node.Operand2); break;
				default: throw new CompilerException();
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
			node.SetInstruction(Select(node.Result, IRInstruction.Move32, IRInstruction.Move64), node.Result, CreateConstant32(size));
		}

		/// <summary>
		/// Visitation function for Starg instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Starg(InstructionNode node)
		{
			Debug.Assert(node.Result.IsParameter);

			if (MosaTypeLayout.CanFitInRegister(node.Operand1.Type))
			{
				var storeInstruction = GetStoreParameterInstruction(node.Result.Type);
				node.SetInstruction(storeInstruction, null, node.Result, node.Operand1);
			}
			else
			{
				var result = node.Result;
				node.SetInstruction(IRInstruction.StoreParamCompound, null, result, node.Operand1);
				node.MosaType = result.Type; // may not be necessary
			}
		}

		/// <summary>
		/// Visitation function for Stelem instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Stelem(InstructionNode node)
		{
			var array = node.Operand1;
			var arrayIndex = node.Operand2;
			var value = node.Operand3;
			var arrayType = array.Type;

			// Array bounds check
			AddArrayBoundsCheck(node, array, arrayIndex);

			var elementOffset = CalculateArrayElementOffset(node, arrayType, arrayIndex);
			var totalElementOffset = CalculateTotalArrayOffset(node, elementOffset);

			if (MosaTypeLayout.CanFitInRegister(value.Type))
			{
				var storeInstruction = GetStoreInstruction(arrayType.ElementType);

				node.SetInstruction(storeInstruction, null, array, totalElementOffset, value);
			}
			else
			{
				node.SetInstruction(IRInstruction.StoreCompound, null, array, totalElementOffset, value);
				node.MosaType = arrayType.ElementType;
			}
		}

		/// <summary>
		/// Visitation function for Stfld instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Stfld(InstructionNode node)
		{
			var field = node.MosaField;

			MethodScanner.AccessedField(field);

			uint offset = TypeLayout.GetFieldOffset(field);
			var offsetOperand = CreateConstant32(offset);

			var objectOperand = node.Operand1;
			var valueOperand = node.Operand2;
			var fieldType = field.FieldType;

			if (MosaTypeLayout.CanFitInRegister(fieldType))
			{
				var storeInstruction = GetStoreInstruction(fieldType);
				node.SetInstruction(storeInstruction, null, objectOperand, offsetOperand, valueOperand);
				node.MosaType = fieldType;
			}
			else
			{
				node.SetInstruction(IRInstruction.StoreCompound, null, objectOperand, offsetOperand, valueOperand);
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

			if (node.Result.IsVirtualRegister && node.Operand1.IsVirtualRegister)
			{
				var moveInstruction = GetMoveInstruction(node.Result.Type);

				node.SetInstruction(moveInstruction, node.Result, node.Operand1);

				return;
			}

			if (MosaTypeLayout.CanFitInRegister(type))
			{
				if (node.Operand1.IsVirtualRegister)
				{
					var storeInstruction = GetStoreInstruction(type);

					node.SetInstruction(storeInstruction, null, StackFrame, node.Result, node.Operand1);
				}
			}
			else
			{
				Debug.Assert(!node.Result.IsVirtualRegister);

				node.SetInstruction(IRInstruction.MoveCompound, node.Result, node.Operand1);
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

			if (MosaTypeLayout.CanFitInRegister(type))
			{
				var storeInstruction = GetStoreInstruction(type);

				node.SetInstruction(storeInstruction, null, node.Operand1, ConstantZero, node.Operand2);
			}
			else
			{
				node.SetInstruction(IRInstruction.StoreCompound, null, node.Operand1, ConstantZero, node.Operand2);
			}

			node.MosaType = type;
		}

		/// <summary>
		/// Visitation function for Ldsfld instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Ldsfld(InstructionNode node)
		{
			var field = node.MosaField;

			var fieldType = field.FieldType;
			var result = node.Result;
			var fieldOperand = Operand.CreateStaticField(field, TypeSystem);

			if (MosaTypeLayout.CanFitInRegister(fieldType))
			{
				var loadInstruction = GetLoadInstruction(fieldType);

				if (fieldType.IsReferenceType)
				{
					var symbol = GetStaticSymbol(field);
					var staticReference = Operand.CreateLabel(TypeSystem.BuiltIn.Object, symbol.Name);

					node.SetInstruction(IRInstruction.LoadObject, result, staticReference, ConstantZero);
				}
				else
				{
					node.SetInstruction(loadInstruction, result, fieldOperand, ConstantZero);
					node.MosaType = fieldType;
				}
			}
			else
			{
				// Interesting -- this code appears to never be executed
				node.SetInstruction(IRInstruction.LoadCompound, result, fieldOperand, ConstantZero);
				node.MosaType = fieldType;
			}

			MethodScanner.AccessedField(field);
		}

		/// <summary>
		/// Visitation function for Stsfld instruction.
		/// </summary>
		/// <param name="node">The context.</param>
		private void Stsfld(InstructionNode node)
		{
			var field = node.MosaField;

			var fieldOperand = Operand.CreateStaticField(field, TypeSystem);
			var fieldType = field.FieldType;
			var operand1 = node.Operand1;

			if (MosaTypeLayout.CanFitInRegister(fieldType))
			{
				var storeInstruction = GetStoreInstruction(fieldType);

				if (fieldType.IsReferenceType)
				{
					var symbol = GetStaticSymbol(field);
					var staticReference = Operand.CreateLabel(TypeSystem.BuiltIn.Object, symbol.Name);

					node.SetInstruction(IRInstruction.StoreObject, null, staticReference, ConstantZero, operand1);
				}
				else
				{
					node.SetInstruction(storeInstruction, null, fieldOperand, ConstantZero, operand1);
					node.MosaType = fieldType;
				}
			}
			else
			{
				node.SetInstruction(IRInstruction.StoreCompound, null, fieldOperand, ConstantZero, node.Operand1);
				node.MosaType = fieldType;
			}

			MethodScanner.AccessedField(field);
		}

		/// <summary>
		/// Visitation function for Sub instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void Sub(InstructionNode node)
		{
			Replace(node, IRInstruction.Sub32, IRInstruction.Sub64, IRInstruction.SubR8, IRInstruction.SubR4);
		}

		/// <summary>
		/// Visitation function for Sub Overflow instruction
		/// </summary>
		/// <param name="node">The node.</param>
		private void SubOverflow(InstructionNode node)
		{
			var overflowResult = AllocateVirtualRegister(TypeSystem.BuiltIn.Boolean);

			node.SetInstruction2(Select(node.Result, IRInstruction.SubOverflowOut32, IRInstruction.SubOverflowOut64), node.Result, overflowResult, node.Operand1, node.Operand2);

			AddOverflowCheck(node, overflowResult);
		}

		/// <summary>
		/// Visitation function for Sub Overflow Unsigned instruction
		/// </summary>
		/// <param name="node">The node.</param>
		private void SubOverflowUnsigned(InstructionNode node)
		{
			var carryResult = AllocateVirtualRegister(TypeSystem.BuiltIn.Boolean);

			node.SetInstruction2(Select(node.Result, IRInstruction.SubCarryOut32, IRInstruction.SubCarryOut64), node.Result, carryResult, node.Operand1, node.Operand2);

			AddOverflowCheck(node, carryResult);
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
			throw new CompilerException();
		}

		/// <summary>
		/// Visitation function for UnaryBranch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void UnaryBranch(Context context)
		{
			var target = context.BranchTargets[0];
			var first = context.Operand1;
			var opcode = ((BaseCILInstruction)context.Instruction).OpCode;

			if (opcode == OpCode.Brtrue || opcode == OpCode.Brtrue_s)
			{
				context.SetInstruction(Select(first, IRInstruction.Branch32, IRInstruction.Branch64), ConditionCode.NotEqual, null, first, ConstantZero, target); // TODO: Constant should be 64bit

				//context.AddBranchTarget(target);
				return;
			}
			else if (opcode == OpCode.Brfalse || opcode == OpCode.Brfalse_s)
			{
				context.SetInstruction(Select(first, IRInstruction.Branch32, IRInstruction.Branch64), ConditionCode.Equal, null, first, ConstantZero, target); // TODO: Constant should be 64bit

				//context.AddBranchTarget(target);
				return;
			}

			throw new NotImplementCompilerException("ExpressionEvaluation: CILTransformationStage.UnaryBranch doesn't support CIL opcode " + opcode);
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
				context.SetInstruction(moveInstruction, context.Result, context.Operand1);
				return;
			}

			var typeSize = Alignment.AlignUp(TypeLayout.GetTypeSize(type), TypeLayout.NativePointerAlignment);
			var tmp = AllocateVirtualRegister(type.ToManagedPointer());

			if (typeSize <= 8)
			{
				context.SetInstruction(IRInstruction.Unbox, tmp, value);
			}
			else
			{
				var adr = AllocateVirtualRegister(type.ToManagedPointer());

				context.SetInstruction(IRInstruction.AddressOf, adr, AddStackLocal(type));
				context.AppendInstruction(IRInstruction.UnboxAny, tmp, value, adr, CreateConstant32(typeSize));
			}

			if (MosaTypeLayout.CanFitInRegister(type))
			{
				var loadInstruction = GetLoadInstruction(type);

				context.AppendInstruction(loadInstruction, result, tmp, ConstantZero);
				context.MosaType = type;
			}
			else
			{
				context.AppendInstruction(IRInstruction.LoadCompound, result, tmp, ConstantZero);
				context.MosaType = type;
			}
		}

		#endregion Visitation Methods

		#region Internals

		// [destination]<-[source]
		private static readonly ConversionEntry[][] ConversionTable32 = new ConversionEntry[][] {
		/* I1 */ new ConversionEntry[] {
				/* I1 */ new ConversionEntry(IRInstruction.SignExtend8x32),
				/* I2 */ new ConversionEntry(IRInstruction.SignExtend8x32),
				/* I4 */ new ConversionEntry(IRInstruction.SignExtend8x32),
				/* I8 */ new ConversionEntry(IRInstruction.Truncate64x32, IRInstruction.SignExtend8x32),
				/* U1 */ new ConversionEntry(IRInstruction.Move32),
				/* U2 */ new ConversionEntry(IRInstruction.And32, 8),
				/* U4 */ new ConversionEntry(IRInstruction.And32, 8),
				/* U8 */ new ConversionEntry(IRInstruction.Truncate64x32, IRInstruction.And32, 8),
				/* R4 */ new ConversionEntry(IRInstruction.ConvertR4ToI32, IRInstruction.And32, 8),
				/* R8 */ new ConversionEntry(IRInstruction.ConvertR8ToI32, IRInstruction.And32, 8),
				/* I  */ new ConversionEntry(IRInstruction.And32, 8),
				/* U  */ new ConversionEntry(IRInstruction.And32, 8),
				/* Ptr*/ new ConversionEntry(IRInstruction.And32, 8)
				},
		/* I2 */ new ConversionEntry[] {
				/* I1 */ new ConversionEntry(IRInstruction.SignExtend16x32),
				/* I2 */ new ConversionEntry(IRInstruction.SignExtend16x32),
				/* I4 */ new ConversionEntry(IRInstruction.SignExtend16x32),
				/* I8 */ new ConversionEntry(IRInstruction.Truncate64x32, IRInstruction.SignExtend16x32),
				/* U1 */ new ConversionEntry(IRInstruction.Move32),
				/* U2 */ new ConversionEntry(IRInstruction.Move32),
				/* U4 */ new ConversionEntry(IRInstruction.And32, 16),
				/* U8 */ new ConversionEntry(IRInstruction.Truncate64x32, IRInstruction.And32, 16),
				/* R4 */ new ConversionEntry(IRInstruction.ConvertR4ToI32, IRInstruction.And32, 8),
				/* R8 */ new ConversionEntry(IRInstruction.ConvertR8ToI32, IRInstruction.And32, 8),
				/* I  */ new ConversionEntry(IRInstruction.And32, 16),
				/* U  */ new ConversionEntry(IRInstruction.And32, 16),
				/* Ptr*/ new ConversionEntry(IRInstruction.And32, 16)
				},
		/* I4 */ new ConversionEntry[] {
				/* I1 */ new ConversionEntry(IRInstruction.SignExtend8x32),
				/* I2 */ new ConversionEntry(IRInstruction.SignExtend16x32),
				/* I4 */ new ConversionEntry(IRInstruction.Move32),
				/* I8 */ new ConversionEntry(IRInstruction.Truncate64x32),
				/* U1 */ new ConversionEntry(IRInstruction.Move32),
				/* U2 */ new ConversionEntry(IRInstruction.Move32),
				/* U4 */ new ConversionEntry(IRInstruction.And32, 16),
				/* U8 */ new ConversionEntry(IRInstruction.Truncate64x32),
				/* R4 */ new ConversionEntry(IRInstruction.ConvertR4ToI32),
				/* R8 */ new ConversionEntry(IRInstruction.ConvertR8ToI32),
				/* I  */ new ConversionEntry(IRInstruction.Move32),
				/* U  */ new ConversionEntry(IRInstruction.Move32),
				/* Ptr*/ new ConversionEntry(IRInstruction.Move32)
				},
		/* I8 */ new ConversionEntry[] {
				/* I1 */ new ConversionEntry(IRInstruction.SignExtend8x64),
				/* I2 */ new ConversionEntry(IRInstruction.SignExtend16x64),
				/* I4 */ new ConversionEntry(IRInstruction.SignExtend32x64),
				/* I8 */ new ConversionEntry(IRInstruction.Move64),
				/* U1 */ new ConversionEntry(IRInstruction.SignExtend8x64),
				/* U2 */ new ConversionEntry(IRInstruction.SignExtend16x64),
				/* U4 */ new ConversionEntry(IRInstruction.SignExtend32x64),
				/* U8 */ new ConversionEntry(IRInstruction.Move64),
				/* R4 */ new ConversionEntry(IRInstruction.ConvertR4ToI64),
				/* R8 */ new ConversionEntry(IRInstruction.ConvertR8ToI64),
				/* I  */ new ConversionEntry(IRInstruction.ZeroExtend32x64),
				/* U  */ new ConversionEntry(IRInstruction.ZeroExtend32x64),
				/* Ptr*/ new ConversionEntry(IRInstruction.ZeroExtend32x64)
				},
		/* U1 */ new ConversionEntry[] {
				/* I1 */ new ConversionEntry(IRInstruction.Move32),
				/* I2 */ new ConversionEntry(IRInstruction.And32, 8),
				/* I4 */ new ConversionEntry(IRInstruction.And32, 8),
				/* I8 */ new ConversionEntry(IRInstruction.Truncate64x32, IRInstruction.And32, 8),
				/* U1 */ new ConversionEntry(IRInstruction.Move32),
				/* U2 */ new ConversionEntry(IRInstruction.And32, 8),
				/* U4 */ new ConversionEntry(IRInstruction.And32, 8),
				/* U8 */ new ConversionEntry(IRInstruction.Truncate64x32, IRInstruction.And32, 8),
				/* R4 */ new ConversionEntry(IRInstruction.ConvertR4ToU32, IRInstruction.And32, 8),
				/* R8 */ new ConversionEntry(IRInstruction.ConvertR8ToU32, IRInstruction.And32, 8),
				/* I  */ new ConversionEntry(IRInstruction.And32, 8),
				/* U  */ new ConversionEntry(IRInstruction.And32, 8),
				/* Ptr*/ new ConversionEntry(IRInstruction.And32, 8)
				},
		/* U2 */ new ConversionEntry[] {
				/* I1 */ new ConversionEntry(IRInstruction.And32, 16),
				/* I2 */ new ConversionEntry(IRInstruction.Move32),
				/* I4 */ new ConversionEntry(IRInstruction.And32, 16),
				/* I8 */ new ConversionEntry(IRInstruction.Truncate64x32, IRInstruction.And32, 16),
				/* U1 */ new ConversionEntry(IRInstruction.Move32),
				/* U2 */ new ConversionEntry(IRInstruction.Move32),
				/* U4 */ new ConversionEntry(IRInstruction.And32, 16),
				/* U8 */ new ConversionEntry(IRInstruction.Truncate64x32, IRInstruction.And32, 16),
				/* R4 */ new ConversionEntry(IRInstruction.ConvertR4ToU32, IRInstruction.And32, 16),
				/* R8 */ new ConversionEntry(IRInstruction.ConvertR8ToU32, IRInstruction.And32, 16),
				/* I  */ new ConversionEntry(IRInstruction.And32, 16),
				/* U  */ new ConversionEntry(IRInstruction.And32, 16),
				/* Ptr*/ new ConversionEntry(IRInstruction.And32, 16)
				},
		/* U4 */ new ConversionEntry[] {
				/* I1 */ new ConversionEntry(IRInstruction.ZeroExtend8x32),
				/* I2 */ new ConversionEntry(IRInstruction.ZeroExtend16x32),
				/* I4 */ new ConversionEntry(IRInstruction.Move32),
				/* I8 */ new ConversionEntry(IRInstruction.Truncate64x32),
				/* U1 */ new ConversionEntry(IRInstruction.Move32),
				/* U2 */ new ConversionEntry(IRInstruction.Move32),
				/* U4 */ new ConversionEntry(IRInstruction.Move32),
				/* U8 */ new ConversionEntry(IRInstruction.Truncate64x32),
				/* R4 */ new ConversionEntry(IRInstruction.ConvertR4ToU32),
				/* R8 */ new ConversionEntry(IRInstruction.ConvertR8ToU32),
				/* I  */ new ConversionEntry(IRInstruction.Move32),
				/* U  */ new ConversionEntry(IRInstruction.Move32),
				/* Ptr*/ new ConversionEntry(IRInstruction.Move32)
				},
		/* U8 */ new ConversionEntry[] {
				/* I1 */ new ConversionEntry(IRInstruction.ZeroExtend8x64),
				/* I2 */ new ConversionEntry(IRInstruction.ZeroExtend16x64),
				/* I4 */ new ConversionEntry(IRInstruction.ZeroExtend32x64),
				/* I8 */ new ConversionEntry(IRInstruction.Move64),
				/* U1 */ new ConversionEntry(IRInstruction.ZeroExtend8x64),
				/* U2 */ new ConversionEntry(IRInstruction.ZeroExtend16x64),
				/* U4 */ new ConversionEntry(IRInstruction.ZeroExtend32x64),
				/* U8 */ new ConversionEntry(IRInstruction.Move64),
				/* R4 */ new ConversionEntry(IRInstruction.ConvertR4ToU64),
				/* R8 */ new ConversionEntry(IRInstruction.ConvertR8ToU64),
				/* I  */ new ConversionEntry(IRInstruction.ZeroExtend32x64),
				/* U  */ new ConversionEntry(IRInstruction.ZeroExtend32x64),
				/* Ptr*/ new ConversionEntry(IRInstruction.ZeroExtend32x64)
				},
		/* R4 */ new ConversionEntry[] {
				/* I1 */ new ConversionEntry(IRInstruction.ConvertI32ToR4),
				/* I2 */ new ConversionEntry(IRInstruction.ConvertI32ToR4),
				/* I4 */ new ConversionEntry(IRInstruction.ConvertI32ToR4),
				/* I8 */ new ConversionEntry(IRInstruction.ConvertI64ToR4),
				/* U1 */ new ConversionEntry(IRInstruction.ConvertU32ToR4),
				/* U2 */ new ConversionEntry(IRInstruction.ConvertU32ToR4),
				/* U4 */ new ConversionEntry(IRInstruction.ConvertU32ToR4),
				/* U8 */ new ConversionEntry(IRInstruction.ConvertU64ToR4),
				/* R4 */ new ConversionEntry(IRInstruction.MoveR4),
				/* R8 */ new ConversionEntry(IRInstruction.ConvertR8ToR4),
				/* I  */ new ConversionEntry(IRInstruction.ConvertI32ToR4),
				/* U  */ new ConversionEntry(IRInstruction.ConvertU32ToR4),
				/* Ptr*/ new ConversionEntry(IRInstruction.ConvertI32ToR4)
				},
		/* R8 */ new ConversionEntry[] {
				/* I1 */ new ConversionEntry(IRInstruction.ConvertI32ToR8),
				/* I2 */ new ConversionEntry(IRInstruction.ConvertI32ToR8),
				/* I4 */ new ConversionEntry(IRInstruction.ConvertI32ToR8),
				/* I8 */ new ConversionEntry(IRInstruction.ConvertI64ToR8),
				/* U1 */ new ConversionEntry(IRInstruction.ConvertU32ToR8),
				/* U2 */ new ConversionEntry(IRInstruction.ConvertU32ToR8),
				/* U4 */ new ConversionEntry(IRInstruction.ConvertU32ToR8),
				/* U8 */ new ConversionEntry(IRInstruction.ConvertU64ToR8),
				/* R4 */ new ConversionEntry(IRInstruction.ConvertR4ToR8),
				/* R8 */ new ConversionEntry(IRInstruction.MoveR8),
				/* I  */ new ConversionEntry(IRInstruction.ConvertI32ToR8),
				/* U  */ new ConversionEntry(IRInstruction.ConvertU32ToR8),
				/* Ptr*/ new ConversionEntry(IRInstruction.ConvertI32ToR8)
				},
		/* I */ new ConversionEntry[] {
				/* I1 */ new ConversionEntry(IRInstruction.And32, 8),
				/* I2 */ new ConversionEntry(IRInstruction.And32, 16),
				/* I4 */ new ConversionEntry(IRInstruction.Move32),
				/* I8 */ new ConversionEntry(IRInstruction.Truncate64x32),
				/* U1 */ new ConversionEntry(IRInstruction.Move32),
				/* U2 */ new ConversionEntry(IRInstruction.Move32),
				/* U4 */ new ConversionEntry(IRInstruction.Move32),
				/* U8 */ new ConversionEntry(IRInstruction.Truncate64x32),
				/* R4 */ new ConversionEntry(IRInstruction.ConvertR4ToI32),
				/* R8 */ new ConversionEntry(IRInstruction.ConvertR8ToI32),
				/* I  */ new ConversionEntry(IRInstruction.Move32),
				/* U  */ new ConversionEntry(IRInstruction.Move32),
				/* Ptr*/ new ConversionEntry(IRInstruction.Move32)
				},
		/* U */ new ConversionEntry[] {
				/* I1 */ new ConversionEntry(IRInstruction.And32, 8),
				/* I2 */ new ConversionEntry(IRInstruction.And32, 16),
				/* I4 */ new ConversionEntry(IRInstruction.Move32),
				/* I8 */ new ConversionEntry(IRInstruction.Truncate64x32),
				/* U1 */ new ConversionEntry(IRInstruction.Move32),
				/* U2 */ new ConversionEntry(IRInstruction.Move32),
				/* U4 */ new ConversionEntry(IRInstruction.Move32),
				/* U8 */ new ConversionEntry(IRInstruction.Truncate64x32),
				/* R4 */ new ConversionEntry(IRInstruction.ConvertR4ToU32),
				/* R8 */ new ConversionEntry(IRInstruction.ConvertR8ToU32),
				/* I  */ new ConversionEntry(IRInstruction.Move32),
				/* U  */ new ConversionEntry(IRInstruction.Move32),
				/* Ptr*/ new ConversionEntry(IRInstruction.Move32)
				},
		/* Ptr */ new ConversionEntry[] {
				/* I1 */ new ConversionEntry(IRInstruction.And32, 8),
				/* I2 */ new ConversionEntry(IRInstruction.And32, 16),
				/* I4 */ new ConversionEntry(IRInstruction.Move32),
				/* I8 */ new ConversionEntry(IRInstruction.Truncate64x32),
				/* U1 */ new ConversionEntry(IRInstruction.Move32),
				/* U2 */ new ConversionEntry(IRInstruction.Move32),
				/* U4 */ new ConversionEntry(IRInstruction.Move32),
				/* U8 */ new ConversionEntry(IRInstruction.Truncate64x32),
				/* R4 */ new ConversionEntry(IRInstruction.ConvertR4ToI32),
				/* R8 */ new ConversionEntry(IRInstruction.ConvertR8ToI32),
				/* I  */ new ConversionEntry(IRInstruction.Move32),
				/* U  */ new ConversionEntry(IRInstruction.Move32),
				/* Ptr*/ new ConversionEntry(IRInstruction.Move32)
				},
		};

		// TODO:
		private static readonly ConversionEntry[][] ConversionTable64 = ConversionTable32;

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
				case OpCode.Bgt_s: return ConditionCode.Greater;
				case OpCode.Ble_s: return ConditionCode.LessOrEqual;
				case OpCode.Blt_s: return ConditionCode.Less;

				// Unsigned
				case OpCode.Bne_un_s: return ConditionCode.NotEqual;
				case OpCode.Bge_un_s: return ConditionCode.UnsignedGreaterOrEqual;
				case OpCode.Bgt_un_s: return ConditionCode.UnsignedGreater;
				case OpCode.Ble_un_s: return ConditionCode.UnsignedLessOrEqual;
				case OpCode.Blt_un_s: return ConditionCode.UnsignedLess;

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
				case OpCode.Cgt: return ConditionCode.Greater;
				case OpCode.Cgt_un: return ConditionCode.UnsignedGreater;
				case OpCode.Clt: return ConditionCode.Less;
				case OpCode.Clt_un: return ConditionCode.UnsignedLess;

				default: throw new InvalidProgramException();
			}
		}

		private static void Replace(InstructionNode node, BaseIRInstruction integerInstruction32, BaseIRInstruction integerInstruction64, BaseIRInstruction floatingPointR8Instruction, BaseIRInstruction floatingPointR4Instruction)
		{
			if (node.Result.IsR4)
			{
				node.ReplaceInstruction(floatingPointR4Instruction);
			}
			else if (node.Result.IsR8)
			{
				node.ReplaceInstruction(floatingPointR8Instruction);
			}
			else
			{
				node.ReplaceInstruction(Select(node.Result, integerInstruction32, integerInstruction64));
			}
		}

		private static BaseIRInstruction Select(Operand operand, BaseIRInstruction instruction32, BaseIRInstruction instruction64)
		{
			return Select(operand.IsInteger64, instruction32, instruction64);
		}

		private static BaseIRInstruction Select(bool is64Bit, BaseIRInstruction instruction32, BaseIRInstruction instruction64)
		{
			return !is64Bit ? instruction32 : instruction64;
		}

		/// <summary>
		/// Adds bounds check to the array access.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="arrayOperand">The array operand.</param>
		/// <param name="arrayIndexOperand">The index operand.</param>
		private void AddArrayBoundsCheck(InstructionNode node, Operand arrayOperand, Operand arrayIndexOperand)
		{
			var before = new Context(node).InsertBefore();

			// First create new block and split current block
			var exceptionContext = CreateNewBlockContexts(1, node.Label)[0];
			var nextContext = Split(before);

			// Get array length
			var lengthOperand = AllocateVirtualRegisterI32();

			before.SetInstruction(Select(lengthOperand, IRInstruction.Load32, IRInstruction.Load64), lengthOperand, arrayOperand, ConstantZero);

			// Now compare length with index
			// If index is greater than or equal to the length then jump to exception block, otherwise jump to next block
			before.AppendInstruction(BranchInstruction, ConditionCode.UnsignedGreaterOrEqual, null, arrayIndexOperand, lengthOperand, exceptionContext.Block);
			before.AppendInstruction(IRInstruction.Jmp, nextContext.Block);

			// Build exception block which is just a call to throw exception
			var method = InternalRuntimeType.FindMethodByName("ThrowIndexOutOfRangeException");
			var symbolOperand = Operand.CreateSymbolFromMethod(method, TypeSystem);

			exceptionContext.AppendInstruction(IRInstruction.CallStatic, null, symbolOperand);
		}

		/// <summary>
		/// Adds overflow check using boolean result operand.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="resultOperand">The overflow or carry result operand.</param>
		private void AddOverflowCheck(InstructionNode node, Operand resultOperand)
		{
			var after = new Context(node).InsertAfter();

			// First create new block and split current block
			var exceptionContext = CreateNewBlockContexts(1, node.Label)[0];
			var nextContext = Split(after);

			// If result is equal to true then jump to exception block, otherwise jump to next block
			after.SetInstruction(BranchInstruction, ConditionCode.NotEqual, null, resultOperand, ConstantZero, exceptionContext.Block);
			after.AppendInstruction(IRInstruction.Jmp, nextContext.Block);

			// Build exception block which is just a call to throw exception
			var method = InternalRuntimeType.FindMethodByName("ThrowOverflowException");
			var symbolOperand = Operand.CreateSymbolFromMethod(method, TypeSystem);

			exceptionContext.AppendInstruction(IRInstruction.CallStatic, null, symbolOperand);
		}

		/// <summary>
		/// Calculates the element offset for the specified index.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="arrayType">The array type.</param>
		/// <param name="index">The index operand.</param>
		/// <returns>
		/// Element offset operand.
		/// </returns>
		private Operand CalculateArrayElementOffset(InstructionNode node, MosaType arrayType, Operand index)
		{
			var size = GetTypeSize(arrayType.ElementType, false);

			var elementOffset = Is32BitPlatform ? AllocateVirtualRegisterI32() : AllocateVirtualRegisterI64();
			var elementSize = CreateConstant32(size);

			var context = new Context(node).InsertBefore();

			context.AppendInstruction(Select(elementOffset, IRInstruction.MulUnsigned32, IRInstruction.MulSigned64), elementOffset, index, elementSize);

			return elementOffset;
		}

		/// <summary>
		/// Calculates the base of the array elements.
		/// </summary>
		/// <param name="node">The node.</param>
		/// <param name="elementOffset">The array.</param>
		/// <returns>
		/// Base address for array elements.
		/// </returns>
		private Operand CalculateTotalArrayOffset(InstructionNode node, Operand elementOffset)
		{
			var fixedOffset = CreateConstant32(NativePointerSize);
			var arrayElement = Is32BitPlatform ? AllocateVirtualRegisterI32() : AllocateVirtualRegisterI64();

			var context = new Context(node).InsertBefore();
			context.AppendInstruction(Select(arrayElement, IRInstruction.Add32, IRInstruction.Add64), arrayElement, elementOffset, fixedOffset);

			return arrayElement;
		}

		private ulong GetBitMask(int bits)
		{
			if (bits == 0)
				return 0;
			else if (bits == 8)
				return 0xFF;
			else if (bits == 16)
				return 0xFFFF;
			else if (bits == 32)
				return 0xFFFFFFFF;
			else if (bits == 64)
				return 0xFFFFFFFFFFFFFFFF;

			throw new CompilerException($"GetBitMask(): Invalid parameter: {nameof(bits)} = {bits}");
		}

		/// <summary>
		/// Gets the index.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		/// <exception cref="CompilerException"></exception>
		private int GetIndex(MosaType type)
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
			else if (type.IsI) return 10;
			else if (type.IsU) return 11;
			else if (type.IsPointer) return 12;
			else if (!type.IsValueType) return 12;

			throw new CompilerException();
		}

		private MosaMethod GetMethodOrOverride(MosaType type, MosaMethod method)
		{
			var implMethod = type.FindMethodBySignature(method.Name, method.Signature);

			if (implMethod != null)
				return implMethod;

			return method;
		}

		private Operand GetMethodTablePointer(MosaType runtimeType)
		{
			return Operand.CreateLabel(TypeSystem.BuiltIn.Pointer, Metadata.TypeDefinition + runtimeType.FullName);
		}

		private Operand GetRuntimeTypeHandle(MosaType runtimeType)
		{
			return Operand.CreateLabel(TypeSystem.GetTypeByName("System", "RuntimeTypeHandle"), Metadata.TypeDefinition + runtimeType.FullName);
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
			var intrinsic = ResolveIntrinsicDelegateMethod(context.InvokeMethod);

			if (intrinsic == null)
				return false;

			if (context.InvokeMethod.IsExternal)
			{
				var operands = context.GetOperands();
				operands.Insert(0, Operand.CreateSymbolFromMethod(context.InvokeMethod, TypeSystem));
				context.SetInstruction(IRInstruction.IntrinsicMethodCall, context.Result, operands);

				return true;
			}
			else
			{
				intrinsic(context, MethodCompiler);

				return true;
			}
		}

		private bool ReplaceWithInternalCall(InstructionNode node)
		{
			var method = node.InvokeMethod;

			if (!method.IsInternal || !method.IsConstructor)
				return false;

			var newmethod = method.DeclaringType.FindMethodByNameAndParameters("Ctor", method.Signature.Parameters);

			var result = node.Result;
			var operands = node.GetOperands();
			var symbol = Operand.CreateSymbolFromMethod(newmethod, TypeSystem);

			node.SetInstruction(IRInstruction.CallStatic, result, symbol);
			node.AppendOperands(operands);

			return true;
		}

		private IntrinsicMethodDelegate ResolveIntrinsicDelegateMethod(MosaMethod method)
		{
			IntrinsicMethodDelegate intrinsic = null;

			if (InstrinsicMap.TryGetValue(method, out intrinsic))
			{
				return intrinsic;
			}

			if (method.IsExternal)
			{
				intrinsic = Architecture.GetInstrinsicMethod(method.ExternMethodModule);
			}
			else
			{
				var methodName = $"{method.DeclaringType.FullName}::{method.Name}";

				intrinsic = MethodCompiler.Compiler.GetInstrincMethod(methodName);
			}

			InstrinsicMap.Add(method, intrinsic);

			return intrinsic;
		}

		private struct ConversionEntry
		{
			public int BitsToMask;
			public BaseInstruction Instruction;
			public BaseInstruction PostInstruction;

			public ConversionEntry(BaseInstruction instruction)
			{
				Instruction = instruction;
				PostInstruction = null;
				BitsToMask = 0;
			}

			public ConversionEntry(BaseInstruction instruction, int bitsToMask = 0)
			{
				Instruction = instruction;
				PostInstruction = null;
				BitsToMask = bitsToMask;
			}

			public ConversionEntry(BaseInstruction instruction, BaseInstruction postInstruction = null, int bitsToMask = 0)
			{
				Instruction = instruction;
				PostInstruction = postInstruction;
				BitsToMask = bitsToMask;
			}
		}

		#endregion Internals

		public LinkerSymbol GetStaticSymbol(MosaField field)
		{
			return Linker.DefineSymbol($"{Metadata.StaticSymbolPrefix}{field.DeclaringType}+{field.Name}", SectionKind.BSS, Architecture.NativeAlignment, NativePointerSize);
		}
	}
}
