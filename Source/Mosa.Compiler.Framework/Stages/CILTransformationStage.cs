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
	public sealed class CILTransformationStage : BaseCodeTransformationStage, IPipelineStage
	{
		protected override void PopulateVisitationDictionary()
		{
			visitationDictionary[CILInstruction.Nop] = Nop;
			visitationDictionary[CILInstruction.Break] = Break;
			visitationDictionary[CILInstruction.Ldarg_0] = Ldarg;
			visitationDictionary[CILInstruction.Ldarg_1] = Ldarg;
			visitationDictionary[CILInstruction.Ldarg_2] = Ldarg;
			visitationDictionary[CILInstruction.Ldarg_3] = Ldarg;
			visitationDictionary[CILInstruction.Ldloc_0] = Ldloc;
			visitationDictionary[CILInstruction.Ldloc_1] = Ldloc;
			visitationDictionary[CILInstruction.Ldloc_2] = Ldloc;
			visitationDictionary[CILInstruction.Ldloc_3] = Ldloc;
			visitationDictionary[CILInstruction.Stloc_0] = Stloc;
			visitationDictionary[CILInstruction.Stloc_1] = Stloc;
			visitationDictionary[CILInstruction.Stloc_2] = Stloc;
			visitationDictionary[CILInstruction.Stloc_3] = Stloc;
			visitationDictionary[CILInstruction.Ldarg_s] = Ldarg;
			visitationDictionary[CILInstruction.Ldarga_s] = Ldarga;
			visitationDictionary[CILInstruction.Starg_s] = Starg;
			visitationDictionary[CILInstruction.Ldloc_s] = Ldloc;
			visitationDictionary[CILInstruction.Ldloca_s] = Ldloca;
			visitationDictionary[CILInstruction.Stloc_s] = Stloc;
			visitationDictionary[CILInstruction.Ldnull] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_m1] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_0] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_1] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_2] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_3] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_4] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_5] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_6] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_7] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_8] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4_s] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i4] = Ldc;
			visitationDictionary[CILInstruction.Ldc_i8] = Ldc;
			visitationDictionary[CILInstruction.Ldc_r4] = Ldc;
			visitationDictionary[CILInstruction.Ldc_r8] = Ldc;
			visitationDictionary[CILInstruction.Dup] = Dup;
			visitationDictionary[CILInstruction.Pop] = Pop;

			//visitationDictionary[CILInstruction.Jmp] = Jmp;
			visitationDictionary[CILInstruction.Call] = Call;
			visitationDictionary[CILInstruction.Calli] = Calli;
			visitationDictionary[CILInstruction.Ret] = Ret;
			visitationDictionary[CILInstruction.Br_s] = Branch;
			visitationDictionary[CILInstruction.Brfalse_s] = UnaryBranch;
			visitationDictionary[CILInstruction.Brtrue_s] = UnaryBranch;
			visitationDictionary[CILInstruction.Beq_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Bge_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Bgt_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Ble_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Blt_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Bne_un_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Bge_un_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Bgt_un_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Ble_un_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Blt_un_s] = BinaryBranch;
			visitationDictionary[CILInstruction.Br] = Branch;
			visitationDictionary[CILInstruction.Brfalse] = UnaryBranch;
			visitationDictionary[CILInstruction.Brtrue] = UnaryBranch;
			visitationDictionary[CILInstruction.Beq] = BinaryBranch;
			visitationDictionary[CILInstruction.Bge] = BinaryBranch;
			visitationDictionary[CILInstruction.Bgt] = BinaryBranch;
			visitationDictionary[CILInstruction.Ble] = BinaryBranch;
			visitationDictionary[CILInstruction.Blt] = BinaryBranch;
			visitationDictionary[CILInstruction.Bne_un] = BinaryBranch;
			visitationDictionary[CILInstruction.Bge_un] = BinaryBranch;
			visitationDictionary[CILInstruction.Bgt_un] = BinaryBranch;
			visitationDictionary[CILInstruction.Ble_un] = BinaryBranch;
			visitationDictionary[CILInstruction.Blt_un] = BinaryBranch;
			visitationDictionary[CILInstruction.Switch] = Switch;
			visitationDictionary[CILInstruction.Ldind_i1] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_u1] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_i2] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_u2] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_i4] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_u4] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_i8] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_i] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_r4] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_r8] = Ldobj;
			visitationDictionary[CILInstruction.Ldind_ref] = Ldobj;
			visitationDictionary[CILInstruction.Stind_ref] = Stobj;
			visitationDictionary[CILInstruction.Stind_i1] = Stobj;
			visitationDictionary[CILInstruction.Stind_i2] = Stobj;
			visitationDictionary[CILInstruction.Stind_i4] = Stobj;
			visitationDictionary[CILInstruction.Stind_i8] = Stobj;
			visitationDictionary[CILInstruction.Stind_r4] = Stobj;
			visitationDictionary[CILInstruction.Stind_r8] = Stobj;
			visitationDictionary[CILInstruction.Add] = Add;
			visitationDictionary[CILInstruction.Sub] = Sub;
			visitationDictionary[CILInstruction.Mul] = Mul;
			visitationDictionary[CILInstruction.Div] = Div;
			visitationDictionary[CILInstruction.Div_un] = BinaryLogic;
			visitationDictionary[CILInstruction.Rem] = Rem;
			visitationDictionary[CILInstruction.Rem_un] = BinaryLogic;
			visitationDictionary[CILInstruction.And] = BinaryLogic;
			visitationDictionary[CILInstruction.Or] = BinaryLogic;
			visitationDictionary[CILInstruction.Xor] = BinaryLogic;
			visitationDictionary[CILInstruction.Shl] = Shift;
			visitationDictionary[CILInstruction.Shr] = Shift;
			visitationDictionary[CILInstruction.Shr_un] = Shift;
			visitationDictionary[CILInstruction.Neg] = Neg;
			visitationDictionary[CILInstruction.Not] = Not;
			visitationDictionary[CILInstruction.Conv_i1] = Conversion;
			visitationDictionary[CILInstruction.Conv_i2] = Conversion;
			visitationDictionary[CILInstruction.Conv_i4] = Conversion;
			visitationDictionary[CILInstruction.Conv_i8] = Conversion;
			visitationDictionary[CILInstruction.Conv_r4] = Conversion;
			visitationDictionary[CILInstruction.Conv_r8] = Conversion;
			visitationDictionary[CILInstruction.Conv_u4] = Conversion;
			visitationDictionary[CILInstruction.Conv_u8] = Conversion;
			visitationDictionary[CILInstruction.Callvirt] = Callvirt;

			//visitationDictionary[CILInstruction.Cpobj] = Cpobj;
			visitationDictionary[CILInstruction.Ldobj] = Ldobj;
			visitationDictionary[CILInstruction.Ldstr] = Ldstr;
			visitationDictionary[CILInstruction.Newobj] = Newobj;
			visitationDictionary[CILInstruction.Castclass] = Castclass;
			visitationDictionary[CILInstruction.Isinst] = IsInst;
			visitationDictionary[CILInstruction.Conv_r_un] = Conversion;
			visitationDictionary[CILInstruction.Unbox] = Unbox;
			visitationDictionary[CILInstruction.Throw] = Throw;
			visitationDictionary[CILInstruction.Ldfld] = Ldfld;
			visitationDictionary[CILInstruction.Ldflda] = Ldflda;
			visitationDictionary[CILInstruction.Stfld] = Stfld;
			visitationDictionary[CILInstruction.Ldsfld] = Ldsfld;
			visitationDictionary[CILInstruction.Ldsflda] = Ldsflda;
			visitationDictionary[CILInstruction.Stsfld] = Stsfld;
			visitationDictionary[CILInstruction.Stobj] = Stobj;
			visitationDictionary[CILInstruction.Conv_ovf_i1_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_i2_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_i4_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_i8_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u1_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u2_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u4_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u8_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_i_un] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u_un] = Conversion;
			visitationDictionary[CILInstruction.Box] = Box;
			visitationDictionary[CILInstruction.Newarr] = Newarr;
			visitationDictionary[CILInstruction.Ldlen] = Ldlen;
			visitationDictionary[CILInstruction.Ldelema] = Ldelema;
			visitationDictionary[CILInstruction.Ldelem_i1] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_u1] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_i2] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_u2] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_i4] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_u4] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_i8] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_i] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_r4] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_r8] = Ldelem;
			visitationDictionary[CILInstruction.Ldelem_ref] = Ldelem;
			visitationDictionary[CILInstruction.Stelem_i] = Stelem;
			visitationDictionary[CILInstruction.Stelem_i1] = Stelem;
			visitationDictionary[CILInstruction.Stelem_i2] = Stelem;
			visitationDictionary[CILInstruction.Stelem_i4] = Stelem;
			visitationDictionary[CILInstruction.Stelem_i8] = Stelem;
			visitationDictionary[CILInstruction.Stelem_r4] = Stelem;
			visitationDictionary[CILInstruction.Stelem_r8] = Stelem;
			visitationDictionary[CILInstruction.Stelem_ref] = Stelem;
			visitationDictionary[CILInstruction.Ldelem] = Ldelem;
			visitationDictionary[CILInstruction.Stelem] = Stelem;
			visitationDictionary[CILInstruction.Unbox_any] = UnboxAny;
			visitationDictionary[CILInstruction.Conv_ovf_i1] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u1] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_i2] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u2] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_i4] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u4] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_i8] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u8] = Conversion;

			//visitationDictionary[CILInstruction.Refanyval] = Refanyval;
			//visitationDictionary[CILInstruction.Ckfinite] = Ckfinite;
			//visitationDictionary[CILInstruction.Mkrefany] = Mkrefany;
			visitationDictionary[CILInstruction.Ldtoken] = Ldtoken;
			visitationDictionary[CILInstruction.Conv_u2] = Conversion;
			visitationDictionary[CILInstruction.Conv_u1] = Conversion;
			visitationDictionary[CILInstruction.Conv_i] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_i] = Conversion;
			visitationDictionary[CILInstruction.Conv_ovf_u] = Conversion;

			//visitationDictionary[CILInstruction.Add_ovf] = Add_ovf;
			//visitationDictionary[CILInstruction.Add_ovf_un] = Add_ovf_un;
			//visitationDictionary[CILInstruction.Mul_ovf] = Mul_ovf;
			//visitationDictionary[CILInstruction.Mul_ovf_un] = Mul_ovf_un;
			//visitationDictionary[CILInstruction.Sub_ovf] = Sub_ovf;
			//visitationDictionary[CILInstruction.Sub_ovf_un] = Sub_ovf_un;
			visitationDictionary[CILInstruction.Endfinally] = Endfinally;
			visitationDictionary[CILInstruction.Leave] = Leave;
			visitationDictionary[CILInstruction.Leave_s] = Leave;
			visitationDictionary[CILInstruction.Stind_i] = Stobj;
			visitationDictionary[CILInstruction.Conv_u] = Conversion;

			//visitationDictionary[CILInstruction.Arglist] = Arglist;
			visitationDictionary[CILInstruction.Ceq] = BinaryComparison;
			visitationDictionary[CILInstruction.Cgt] = BinaryComparison;
			visitationDictionary[CILInstruction.Cgt_un] = BinaryComparison;
			visitationDictionary[CILInstruction.Clt] = BinaryComparison;
			visitationDictionary[CILInstruction.Clt_un] = BinaryComparison;
			visitationDictionary[CILInstruction.Ldftn] = Ldftn;
			visitationDictionary[CILInstruction.Ldvirtftn] = Ldvirtftn;
			visitationDictionary[CILInstruction.Ldarg] = Ldarg;
			visitationDictionary[CILInstruction.Ldarga] = Ldarga;
			visitationDictionary[CILInstruction.Starg] = Starg;
			visitationDictionary[CILInstruction.Ldloc] = Ldloc;
			visitationDictionary[CILInstruction.Ldloca] = Ldloca;
			visitationDictionary[CILInstruction.Stloc] = Stloc;

			//visitationDictionary[CILInstruction.Localalloc] = Localalloc;
			visitationDictionary[CILInstruction.Endfilter] = Endfilter;

			//visitationDictionary[CILInstruction.PreUnaligned] = PreUnaligned;
			//visitationDictionary[CILInstruction.PreVolatile] = PreVolatile;
			//visitationDictionary[CILInstruction.PreTail] = PreTail;
			visitationDictionary[CILInstruction.InitObj] = InitObj;

			//visitationDictionary[CILInstruction.PreConstrained] = PreConstrained;
			visitationDictionary[CILInstruction.Cpblk] = Cpblk;
			visitationDictionary[CILInstruction.Initblk] = Initblk;

			//visitationDictionary[CILInstruction.PreNo] = PreNo;
			visitationDictionary[CILInstruction.Rethrow] = Rethrow;
			visitationDictionary[CILInstruction.Sizeof] = Sizeof;

			//visitationDictionary[CILInstruction.Refanytype] = Refanytype;
			//visitationDictionary[CILInstruction.PreReadOnly] = PreReadOnly;
		}

		#region Visitation Methods

		/// <summary>
		/// Visitation function for Ldarg instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldarg(Context context)
		{
			ProcessLoadInstruction(context);
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
		/// Visitation function for Ldc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldc(Context context)
		{
			ProcessLoadInstruction(context);
		}

		/// <summary>
		/// Visitation function for Ldobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldobj(Context context)
		{
			Operand destination = context.Result;
			Operand source = context.Operand1;

			var type = context.MosaType;

			// This is actually ldind.* and ldobj - the opcodes have the same meanings

			BaseIRInstruction loadInstruction = IRInstruction.Load;

			if (MustSignExtendOnLoad(type))
			{
				loadInstruction = IRInstruction.LoadSignExtended;
			}
			else if (MustZeroExtendOnLoad(type))
			{
				loadInstruction = IRInstruction.LoadZeroExtended;
			}

			var size = GetInstructionSize(type);
			context.SetInstruction(loadInstruction, size, destination, source, ConstantZero);
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

			BaseIRInstruction loadInstruction = IRInstruction.Load;

			if (MustSignExtendOnLoad(fieldType))
			{
				loadInstruction = IRInstruction.LoadSignExtended;
			}
			else if (MustZeroExtendOnLoad(fieldType))
			{
				loadInstruction = IRInstruction.LoadZeroExtended;
			}

			var size = GetInstructionSize(fieldType);
			context.SetInstruction(loadInstruction, size, destination, Operand.CreateField(context.MosaField), ConstantZero);
			context.MosaType = fieldType;
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
		/// Visitation function for Ldftn instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldftn(Context context)
		{
			context.SetInstruction(IRInstruction.Move, context.Result, Operand.CreateSymbolFromMethod(TypeSystem, context.InvokeMethod));
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
				runtimeHandle = MethodCompiler.CreateVirtualRegister(TypeSystem.GetTypeByName("System", "RuntimeTypeHandle"));
			}
			else if (context.MosaField != null)
			{
				source = Operand.CreateUnmanagedSymbolPointer(TypeSystem, context.MosaField.FullName + Metadata.FieldDefinition);
				runtimeHandle = MethodCompiler.CreateVirtualRegister(TypeSystem.GetTypeByName("System", "RuntimeFieldHandle"));
			}
			else
				throw new NotImplementCompilerException();

			Operand destination = context.Result;
			context.SetInstruction(IRInstruction.Move, runtimeHandle, source);
			context.AppendInstruction(IRInstruction.Move, destination, runtimeHandle);
		}

		/// <summary>
		/// Visitation function for Stloc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Stloc(Context context)
		{
			context.SetInstruction(IRInstruction.Move, context.Result, context.Operand1);
		}

		/// <summary>
		/// Visitation function for Starg instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Starg(Context context)
		{
			context.SetInstruction(IRInstruction.Move, context.Result, context.Operand1);
		}

		/// <summary>
		/// Visitation function for Stobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Stobj(Context context)
		{
			// This is actually stind.* and stobj - the opcodes have the same meanings
			var type = context.MosaType;  // pass thru

			var size = GetInstructionSize(type);

			if (TypeLayout.IsCompoundType(type))
			{
				context.SetInstruction(IRInstruction.CompoundStore, size, null, context.Operand1, ConstantZero, context.Operand2);
			}
			else
			{
				context.SetInstruction(IRInstruction.Store, size, null, context.Operand1, ConstantZero, context.Operand2);
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

			context.SetInstruction(IRInstruction.Store, size, null, Operand.CreateField(field), ConstantZero, context.Operand1);
			context.MosaType = field.FieldType;
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
			if (context.InvokeMethod.IsVirtual &&
				context.Operand1.Type.ElementType != null &&
				context.Operand1.Type.ElementType.IsValueType &&
				context.InvokeMethod.DeclaringType == context.Operand1.Type.ElementType)
			{
				if (OverridesMethod(context.InvokeMethod))
				{
					var before = context.InsertBefore();
					before.SetInstruction(IRInstruction.SubSigned, context.Operand1, context.Operand1, Operand.CreateConstant(TypeSystem, NativePointerSize * 2));
				}
				else
				{
					// Get the value type, size and native alignment
					MosaType type = context.Operand1.Type.ElementType;
					int typeSize = TypeLayout.GetTypeSize(type);
					int alignment = TypeLayout.NativePointerAlignment;
					typeSize += (alignment - (typeSize % alignment)) % alignment;

					// Create a virtual register to hold our boxed value
					var boxedValue = MethodCompiler.CreateVirtualRegister(TypeSystem.BuiltIn.Object);

					// Create a new context before the call and set it as a VmCall
					var before = context.InsertBefore();
					before.SetInstruction(IRInstruction.Nop);
					ReplaceWithVmCall(before, VmCall.Box);

					// Populate the operands for the VmCall and result
					before.SetOperand(1, GetRuntimeTypeHandle(type, before));
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
		/// Visitation function for Calli instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Calli(Context context)
		{
			Operand destinationOperand = context.GetOperand(context.OperandCount - 1);
			context.OperandCount -= 1;

			ProcessInvokeInstruction(context, context.InvokeMethod, context.Result, new List<Operand>(context.Operands));
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

			switch ((context.Instruction as CIL.BaseCILInstruction).OpCode)
			{
				case CIL.OpCode.And: context.SetInstruction(IRInstruction.LogicalAnd, context.Result, context.Operand1, context.Operand2); break;
				case CIL.OpCode.Or: context.SetInstruction(IRInstruction.LogicalOr, context.Result, context.Operand1, context.Operand2); break;
				case CIL.OpCode.Xor: context.SetInstruction(IRInstruction.LogicalXor, context.Result, context.Operand1, context.Operand2); break;
				case CIL.OpCode.Div_un: context.SetInstruction(IRInstruction.DivUnsigned, context.Result, context.Operand1, context.Operand2); break;
				case CIL.OpCode.Rem_un: context.SetInstruction(IRInstruction.RemUnsigned, context.Result, context.Operand1, context.Operand2); break;
				default: throw new InvalidCompilerException();
			}
		}

		/// <summary>
		/// Visitation function for Shift instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Shift(Context context)
		{
			switch ((context.Instruction as CIL.BaseCILInstruction).OpCode)
			{
				case CIL.OpCode.Shl: context.SetInstruction(IRInstruction.ShiftLeft, context.Result, context.Operand1, context.Operand2); break;
				case CIL.OpCode.Shr: context.SetInstruction(IRInstruction.ArithmeticShiftRight, context.Result, context.Operand1, context.Operand2); break;
				case CIL.OpCode.Shr_un: context.SetInstruction(IRInstruction.ShiftRight, context.Result, context.Operand1, context.Operand2); break;
				default: throw new InvalidCompilerException();
			}
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
				Operand zero = Operand.CreateConstant(context.Operand1.Type, 0);
				context.SetInstruction(IRInstruction.SubUnsigned, context.Result, zero, context.Operand1);
			}
			else if (context.Operand1.IsR4)
			{
				Operand minusOne = Operand.CreateConstant(TypeSystem, -1.0f);
				context.SetInstruction(IRInstruction.MulFloat, context.Result, minusOne, context.Operand1);
			}
			else if (context.Operand1.IsR8)
			{
				Operand minusOne = Operand.CreateConstant(TypeSystem, -1.0d);
				context.SetInstruction(IRInstruction.MulFloat, context.Result, minusOne, context.Operand1);
			}
			else
			{
				Operand minusOne = Operand.CreateConstant(context.Operand1.Type, -1);
				context.SetInstruction(IRInstruction.MulSigned, context.Result, minusOne, context.Operand1);
			}
		}

		/// <summary>
		/// Visitation function for Not instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Not(Context context)
		{
			context.SetInstruction(IRInstruction.LogicalNot, context.Result, context.Operand1);
		}

		/// <summary>
		/// Visitation function for Conversion instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Conversion(Context context)
		{
			CheckAndConvertInstruction(context);
		}

		/// <summary>
		/// Visitation function for Callvirt instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Callvirt(Context context)
		{
			if (ProcessExternalCall(context))
				return;

			MosaMethod method = context.InvokeMethod;
			Operand resultOperand = context.Result;
			List<Operand> operands = new List<Operand>(context.Operands);

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
						if (method.IsVirtual &&
							context.Operand1.Type.ElementType != null &&
							context.Operand1.Type.ElementType.IsValueType &&
							method.DeclaringType == context.Operand1.Type.ElementType)
						{
							var before = context.InsertBefore();
							before.SetInstruction(IRInstruction.SubSigned, context.Operand1, context.Operand1, Operand.CreateConstant(TypeSystem, NativePointerSize * 2));
						}
					}
					else
					{
						// Get the value type, size and native alignment
						MosaType elementType = context.Operand1.Type.ElementType;
						int typeSize = TypeLayout.GetTypeSize(elementType);
						int alignment = TypeLayout.NativePointerAlignment;
						typeSize += (alignment - (typeSize % alignment)) % alignment;

						// Create a virtual register to hold our boxed value
						var boxedValue = MethodCompiler.CreateVirtualRegister(TypeSystem.BuiltIn.Object);

						// Create a new context before the call and set it as a VmCall
						var before = context.InsertBefore();
						before.SetInstruction(IRInstruction.Nop);
						ReplaceWithVmCall(before, VmCall.Box);

						// Populate the operands for the VmCall and result
						before.SetOperand(1, GetRuntimeTypeHandle(elementType, before));
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
				Operand thisPtr = context.Operand1;

				Operand typeDefinition = MethodCompiler.CreateVirtualRegister(TypeSystem.BuiltIn.Pointer);
				Operand methodDefinition = MethodCompiler.CreateVirtualRegister(TypeSystem.BuiltIn.Pointer);
				Operand methodPtr = MethodCompiler.CreateVirtualRegister(TypeSystem.BuiltIn.Pointer);

				if (!method.DeclaringType.IsInterface)
				{
					// methodDefinitionOffset is as follows (slot * NativePointerSize) + (NativePointerSize * 14)
					// We use 14 as that is the number of NativePointerSized fields until the start of methodDefinition pointers
					int methodDefinitionOffset = CalculateMethodTableOffset(method) + (NativePointerSize * 14);

					// Same as above except for methodPointer
					int methodPointerOffset = (NativePointerSize * 4);

					// Get the TypeDef pointer
					context.SetInstruction(IRInstruction.Load, NativeInstructionSize, typeDefinition, thisPtr, ConstantZero);

					// Get the MethodDef pointer
					context.AppendInstruction(IRInstruction.Load, NativeInstructionSize, methodDefinition, typeDefinition, Operand.CreateConstant(TypeSystem, methodDefinitionOffset));

					// Get the address of the method
					context.AppendInstruction(IRInstruction.Load, NativeInstructionSize, methodPtr, methodDefinition, Operand.CreateConstant(TypeSystem, methodPointerOffset));
				}
				else
				{
					// Offset for InterfaceSlotTable in TypeDef
					int interfaceSlotTableOffset = (NativePointerSize * 11);

					// Offset for InterfaceMethodTable in InterfaceSlotTable
					int interfaceMethodTableOffset = (NativePointerSize * 1) + CalculateInterfaceSlotOffset(method);

					// Offset for MethodDef in InterfaceMethodTable
					int methodDefinitionOffset = (NativePointerSize * 2) + CalculateMethodTableOffset(method);

					// Offset for Method pointer in MethodDef
					int methodPointerOffset = (NativePointerSize * 4);

					// Operands to hold pointers
					Operand interfaceSlotPtr = MethodCompiler.CreateVirtualRegister(TypeSystem.BuiltIn.Pointer);
					Operand interfaceMethodTablePtr = MethodCompiler.CreateVirtualRegister(TypeSystem.BuiltIn.Pointer);

					// Get the TypeDef pointer
					context.SetInstruction(IRInstruction.Load, NativeInstructionSize, typeDefinition, thisPtr, ConstantZero);

					// Get the Interface Slot Table pointer
					context.AppendInstruction(IRInstruction.Load, NativeInstructionSize, interfaceSlotPtr, typeDefinition, Operand.CreateConstant(TypeSystem, interfaceSlotTableOffset));

					// Get the Interface Method Table pointer
					context.AppendInstruction(IRInstruction.Load, NativeInstructionSize, interfaceMethodTablePtr, interfaceSlotPtr, Operand.CreateConstant(TypeSystem, interfaceMethodTableOffset));

					// Get the MethodDef pointer
					context.AppendInstruction(IRInstruction.Load, NativeInstructionSize, methodDefinition, interfaceMethodTablePtr, Operand.CreateConstant(TypeSystem, methodDefinitionOffset));

					// Get the address of the method
					context.AppendInstruction(IRInstruction.Load, NativeInstructionSize, methodPtr, methodDefinition, Operand.CreateConstant(TypeSystem, methodPointerOffset));
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
				&& (implMethod = type.FindMethodBySignature(method.Name, method.Signature)) != null
			)
			{
				return implMethod;
			}
			return method;
		}

		private bool TypeContainsMethodObjective(MosaType type, MosaMethod method)
		{
			foreach (var m in type.Methods)
				if (((object)m).Equals(method))
					return true;
			return false;
		}

		private int CalculateMethodTableOffset(MosaMethod invokeTarget)
		{
			int slot = TypeLayout.GetMethodTableOffset(invokeTarget);

			return (NativePointerSize * slot);
		}

		private int CalculateInterfaceSlotOffset(MosaMethod invokeTarget)
		{
			return CalculateInterfaceSlot(invokeTarget.DeclaringType) * NativePointerSize;
		}

		private int CalculateInterfaceSlot(MosaType interaceType)
		{
			return TypeLayout.GetInterfaceSlotOffset(interaceType);
		}

		/// <summary>
		/// Visitation function for Newarr instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Newarr(Context context)
		{
			Operand thisReference = context.Result;
			Debug.Assert(thisReference != null, @"Newarr didn't specify class signature?");

			MosaType arrayType = context.Result.Type;
			int elementSize = 0;
			var elementType = arrayType.ElementType;

			int alignment = 0;
			Architecture.GetTypeRequirements(TypeLayout, arrayType.ElementType, out elementSize, out alignment);

			Operand lengthOperand = context.Operand1;

			// HACK: If we can't determine the size now, assume 16 bytes per array element.
			if (elementSize == 0)
			{
				elementSize = 16;
			}

			ReplaceWithVmCall(context, VmCall.AllocateArray);

			context.SetOperand(1, GetRuntimeTypeHandle(arrayType, context));
			context.SetOperand(2, Operand.CreateConstant(TypeSystem, elementSize));
			context.SetOperand(3, lengthOperand);
			context.OperandCount = 4;
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

			Context before = context.InsertBefore();

			if (TypeLayout.IsCompoundType(thisReference.Type))
			{
				var newThis = MethodCompiler.StackLayout.AddStackLocal(thisReference.Type);

				var oldThisReference = thisReference;
				thisReference = MethodCompiler.CreateVirtualRegister(thisReference.Type.ToManagedPointer());
				before.SetInstruction(IRInstruction.AddressOf, thisReference, newThis);

				for (var node = context.Next; !node.IsBlockEndInstruction; node = node.Next)
					if (!node.IsEmpty)
						for (int i = 0; i < node.OperandCount; i++)
							if (node.GetOperand(i) == oldThisReference)
								node.SetOperand(i, newThis);
			}
			else
			{
				ReplaceWithVmCall(before, VmCall.AllocateObject);

				before.SetOperand(1, GetRuntimeTypeHandle(classType, before));
				before.SetOperand(2, Operand.CreateConstant(TypeSystem, TypeLayout.GetTypeSize(classType)));
				before.OperandCount = 3;
				before.Result = thisReference;
				before.ResultCount = 1;
			}

			// Result is the this pointer, now invoke the real constructor
			var operands = new List<Operand>(context.Operands);
			operands.Insert(0, thisReference);

			ProcessInvokeInstruction(context, context.InvokeMethod, null, operands);
		}

		private Operand GetRuntimeTypeHandle(MosaType runtimeType, Context context)
		{
			var typeDef = Operand.CreateUnmanagedSymbolPointer(TypeSystem, runtimeType.FullName + Metadata.TypeDefinition);
			var runtimeTypeHandle = MethodCompiler.CreateVirtualRegister(TypeSystem.GetTypeByName("System", "RuntimeTypeHandle"));
			var before = context.InsertBefore();
			before.SetInstruction(IRInstruction.Move, runtimeTypeHandle, typeDef);
			return runtimeTypeHandle;
		}

		/// <summary>
		/// Visitation function for Castclass instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Castclass(Context context)
		{
			// TODO!
			//ReplaceWithVmCall(context, VmCall.Castclass);
			context.ReplaceInstructionOnly(IRInstruction.Move); // HACK!
		}

		/// <summary>
		/// Visitation function for Isinst instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void IsInst(Context context)
		{
			Operand reference = context.Operand1;
			Operand result = context.Result;

			MosaType classType = result.Type;

			if (!classType.IsInterface)
			{
				ReplaceWithVmCall(context, VmCall.IsInstanceOfType);

				context.SetOperand(1, GetRuntimeTypeHandle(classType, context));
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
				context.ReplaceInstructionOnly(IRInstruction.Move);
				return;
			}

			int typeSize = TypeLayout.GetTypeSize(type);
			int alignment = TypeLayout.NativePointerAlignment;
			typeSize += (alignment - (typeSize % alignment)) % alignment;

			var vmCall = ToVmUnboxCall(typeSize);

			context.SetInstruction(IRInstruction.Nop);
			ReplaceWithVmCall(context, vmCall);

			context.SetOperand(1, value);
			if (vmCall == VmCall.Unbox)
			{
				Operand adr = MethodCompiler.CreateVirtualRegister(type.ToManagedPointer());
				context.InsertBefore().SetInstruction(IRInstruction.AddressOf, adr, MethodCompiler.StackLayout.AddStackLocal(type));

				context.SetOperand(2, adr);
				context.SetOperand(3, Operand.CreateConstant(TypeSystem, typeSize));
				context.OperandCount = 4;
			}
			else
			{
				context.OperandCount = 2;
			}

			Operand tmp = MethodCompiler.CreateVirtualRegister(type.ToManagedPointer());
			context.Result = tmp;
			context.ResultCount = 1;

			var size = GetInstructionSize(tmp.Type);

			context.AppendInstruction(IRInstruction.Load, size, result, tmp, ConstantZero);
			context.MosaType = type;
			return;
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
				context.ReplaceInstructionOnly(IRInstruction.Move);
				return;
			}

			int typeSize = TypeLayout.GetTypeSize(type);
			int alignment = TypeLayout.NativePointerAlignment;
			typeSize += (alignment - (typeSize % alignment)) % alignment;

			VmCall vmCall = VmCall.Box32;

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

			context.SetOperand(1, GetRuntimeTypeHandle(type, context));
			if (vmCall == VmCall.Box)
			{
				Operand adr = MethodCompiler.CreateVirtualRegister(type.ToManagedPointer());
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
		/// Visitation function for BinaryComparison instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void BinaryComparison(Context context)
		{
			var code = ConvertCondition((context.Instruction as CIL.BaseCILInstruction).OpCode);
			var instruction = context.Operand1.IsR ? (BaseInstruction)IRInstruction.FloatCompare : (BaseInstruction)IRInstruction.IntegerCompare;

			context.SetInstruction(instruction, code, context.Result, context.Operand1, context.Operand2);
			context.SetInstruction(instruction, code, context.Result, context.Operand1, context.Operand2);
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
		/// Visitation function for Initblk instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Initblk(Context context)
		{
			ReplaceWithVmCall(context, VmCall.MemorySet);
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
		/// Visitation function for Nop instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Nop(Context context)
		{
			context.SetInstruction(IRInstruction.Nop);
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
		/// Visitation function for Break instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Break(Context context)
		{
			context.SetInstruction(IRInstruction.Break);
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
			 *
			 */

			BaseLinker linker = MethodCompiler.Linker;
			string symbolName = context.Operand1.Name;
			string stringdata = context.Operand1.StringData;

			context.SetInstruction(IRInstruction.Move, context.Result, context.Operand1);

			var symbol = linker.CreateSymbol(symbolName, SectionKind.ROData, NativeAlignment, NativePointerSize * 3 + stringdata.Length * 2);
			var stream = symbol.Stream;

			// Type Definition and sync block
			linker.Link(LinkType.AbsoluteAddress, PatchType.I4, symbol, 0, 0, "System.String" + Metadata.TypeDefinition, SectionKind.ROData, 0);

			stream.WriteZeroBytes(NativePointerSize * 2);

			// String length field
			stream.Write(BitConverter.GetBytes(stringdata.Length), 0, NativePointerSize);

			// String data
			var stringData = Encoding.Unicode.GetBytes(stringdata);
			Debug.Assert(stringData.Length == stringdata.Length * 2, "Byte array of string data doesn't match expected string data length");
			stream.Write(stringData);
		}

		/// <summary>
		/// Visitation function for Ldfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldfld(Context context)
		{
			Operand resultOperand = context.Result;
			Operand objectOperand = context.Operand1;
			MosaField field = context.MosaField;

			if (!objectOperand.IsPointer && objectOperand.Type.IsUserValueType)
			{
				var userStruct = objectOperand;
				if (!userStruct.IsStackLocal)
				{
					var originalOperand = userStruct;
					userStruct = MethodCompiler.StackLayout.AddStackLocal(userStruct.Type);
					context.InsertBefore().SetInstruction(IRInstruction.Move, userStruct, originalOperand);
				}
				objectOperand = MethodCompiler.CreateVirtualRegister(userStruct.Type.ToManagedPointer());
				context.InsertBefore().SetInstruction(IRInstruction.AddressOf, objectOperand, userStruct);
			}

			int offset = TypeLayout.GetFieldOffset(field);
			Operand offsetOperand = Operand.CreateConstant(TypeSystem, offset);

			BaseIRInstruction loadInstruction = IRInstruction.Load;

			if (MustSignExtendOnLoad(field.FieldType))
			{
				loadInstruction = IRInstruction.LoadSignExtended;
			}
			else if (MustZeroExtendOnLoad(field.FieldType))
			{
				loadInstruction = IRInstruction.LoadZeroExtended;
			}

			Debug.Assert(offsetOperand != null);

			var size = GetInstructionSize(field.FieldType);
			context.SetInstruction(loadInstruction, size, resultOperand, objectOperand, offsetOperand);
			context.MosaType = field.FieldType;
		}

		/// <summary>
		/// Visitation function for Ldflda instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldflda(Context context)
		{
			Operand fieldAddress = context.Result;
			Operand objectOperand = context.Operand1;

			int offset = TypeLayout.GetFieldOffset(context.MosaField);
			Operand fixedOffset = Operand.CreateConstant(TypeSystem, offset);

			context.SetInstruction(IRInstruction.AddUnsigned, fieldAddress, objectOperand, fixedOffset);
		}

		/// <summary>
		/// Visitation function for Stfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Stfld(Context context)
		{
			Operand objectOperand = context.Operand1;
			Operand valueOperand = context.Operand2;
			Operand temp = MethodCompiler.CreateVirtualRegister(context.MosaField.FieldType);

			int offset = TypeLayout.GetFieldOffset(context.MosaField);
			Operand offsetOperand = Operand.CreateConstant(TypeSystem, offset);

			MosaType fieldType = context.MosaField.FieldType;

			var size = GetInstructionSize(fieldType);

			context.SetInstruction(IRInstruction.Move, temp, valueOperand);
			context.AppendInstruction(IRInstruction.Store, size, null, objectOperand, offsetOperand, temp);
			context.MosaType = fieldType;
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
		/// Visitation function for UnaryBranch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void UnaryBranch(Context context)
		{
			var target = context.BranchTargets[0];

			Operand first = context.Operand1;
			Operand second = ConstantZero;

			CIL.OpCode opcode = ((CIL.BaseCILInstruction)context.Instruction).OpCode;

			if (opcode == CIL.OpCode.Brtrue || opcode == CIL.OpCode.Brtrue_s)
			{
				context.SetInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.NotEqual, null, first, second);
				context.AddBranchTarget(target);
				return;
			}
			else if (opcode == CIL.OpCode.Brfalse || opcode == CIL.OpCode.Brfalse_s)
			{
				context.SetInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.Equal, null, first, second);
				context.AddBranchTarget(target);
				return;
			}

			throw new NotImplementCompilerException(@"CILTransformationStage.UnaryBranch doesn't support CIL opcode " + opcode);
		}

		/// <summary>
		/// Visitation function for BinaryBranch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void BinaryBranch(Context context)
		{
			var target = context.BranchTargets[0];

			ConditionCode cc = ConvertCondition(((CIL.BaseCILInstruction)context.Instruction).OpCode);
			Operand first = context.Operand1;
			Operand second = context.Operand2;

			if (first.IsR)
			{
				Operand comparisonResult = MethodCompiler.CreateVirtualRegister(TypeSystem.BuiltIn.I4);
				context.SetInstruction(IRInstruction.FloatCompare, cc, comparisonResult, first, second);
				context.AppendInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.Equal, null, comparisonResult, Operand.CreateConstant(TypeSystem, 1));
				context.AddBranchTarget(target);
			}
			else
			{
				context.SetInstruction(IRInstruction.IntegerCompareBranch, cc, null, first, second);
				context.AddBranchTarget(target);
			}
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
		/// Visitation function for Ldlen instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldlen(Context context)
		{
			var offset = Operand.CreateConstant(TypeSystem, NativePointerSize * 2);
			context.SetInstruction(IRInstruction.Load2, InstructionSize.Size32, context.Result, context.Operand1, offset);
		}

		/// <summary>
		/// Visitation function for Ldelema instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldelema(Context context)
		{
			var result = context.Result;
			var arrayOperand = context.Operand1;
			var arrayIndexOperand = context.Operand2;
			var arrayType = arrayOperand.Type;
			Debug.Assert(arrayType.ElementType == result.Type.ElementType);

			// Array bounds check
			AddArrayBoundsCheck(context.InsertBefore(), arrayOperand, arrayIndexOperand);

			//
			// The sequence we're emitting is:
			//
			//      arrayAddress = arrayOperand + 12
			//      offset = arrayIndexOperand * elementSize
			//      result = arrayAddress + offset
			//
			// The array data starts at offset 12 from the array object itself. The 12 is a assumption of x86,
			// which might change for other platforms. This is automatically calculated using the plaform native pointer size.
			//

			var arrayAddress = LoadArrayBaseAddress(context, arrayType, arrayOperand);
			var elementOffset = CalculateArrayElementOffset(context, arrayType, arrayIndexOperand);
			context.SetInstruction(IRInstruction.AddSigned, result, arrayAddress, elementOffset);
		}

		/// <summary>
		/// Visitation function for Ldelem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Ldelem(Context context)
		{
			var result = context.Result;
			var arrayOperand = context.Operand1;
			var arrayIndexOperand = context.Operand2;
			var arraySigType = arrayOperand.Type;

			// Array bounds check
			AddArrayBoundsCheck(context.InsertBefore(), arrayOperand, arrayIndexOperand);

			BaseIRInstruction loadInstruction = IRInstruction.Load;

			if (MustSignExtendOnLoad(arraySigType.ElementType))
			{
				loadInstruction = IRInstruction.LoadSignExtended;
			}
			else if (MustZeroExtendOnLoad(arraySigType.ElementType))
			{
				loadInstruction = IRInstruction.LoadZeroExtended;
			}

			//
			// The sequence we're emitting is:
			//
			//      arrayAddress = arrayOperand + 12
			//      offset = arrayIndexOperand * elementSize
			//      result = *(arrayAddress + offset)
			//
			// The array data starts at offset 12 from the array object itself. The 12 is a assumption of x86,
			// which might change for other platforms. This is automatically calculated using the plaform native pointer size.
			//

			var arrayAddress = LoadArrayBaseAddress(context, arraySigType, arrayOperand);
			var elementOffset = CalculateArrayElementOffset(context, arraySigType, arrayIndexOperand);

			Debug.Assert(elementOffset != null);

			var size = GetInstructionSize(arraySigType.ElementType);

			context.SetInstruction(loadInstruction, size, result, arrayAddress, elementOffset);
			context.MosaType = arraySigType.ElementType;
		}

		/// <summary>
		/// Visitation function for Stelem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Stelem(Context context)
		{
			var arrayOperand = context.Operand1;
			var arrayIndexOperand = context.Operand2;
			var value = context.Operand3;
			var arrayType = arrayOperand.Type;

			// Array bounds check
			AddArrayBoundsCheck(context.InsertBefore(), arrayOperand, arrayIndexOperand);

			var arrayAddress = LoadArrayBaseAddress(context, arrayType, arrayOperand);
			var elementOffset = CalculateArrayElementOffset(context, arrayType, arrayIndexOperand);

			var size = GetInstructionSize(arrayType.ElementType);

			context.SetInstruction(IRInstruction.Store, size, null, arrayAddress, elementOffset, value);
			context.MosaType = arrayType.ElementType;
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
				context.ReplaceInstructionOnly(IRInstruction.Move);
				return;
			}

			int typeSize = TypeLayout.GetTypeSize(type);
			int alignment = TypeLayout.NativePointerAlignment;
			typeSize += (alignment - (typeSize % alignment)) % alignment;

			var vmCall = ToVmUnboxCall(typeSize);

			context.SetInstruction(IRInstruction.Nop);
			ReplaceWithVmCall(context, vmCall);

			context.SetOperand(1, value);
			if (vmCall == VmCall.Unbox)
			{
				Operand adr = MethodCompiler.CreateVirtualRegister(type.ToManagedPointer());
				context.InsertBefore().SetInstruction(IRInstruction.AddressOf, adr, MethodCompiler.StackLayout.AddStackLocal(type));

				context.SetOperand(2, adr);
				context.SetOperand(3, Operand.CreateConstant(TypeSystem, typeSize));
				context.OperandCount = 4;
			}
			else
			{
				context.OperandCount = 2;
			}

			Operand tmp = MethodCompiler.CreateVirtualRegister(type.ToManagedPointer());
			context.Result = tmp;
			context.ResultCount = 1;

			var size = GetInstructionSize(tmp.Type);

			context.AppendInstruction(IRInstruction.Load, size, result, tmp, ConstantZero);
			context.MosaType = type;
			return;
		}

		/// <summary>
		/// Visitation function for Endfinally instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Endfinally(Context context)
		{
			throw new InvalidCompilerException();
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
		/// Visitation function for InitObj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void InitObj(Context context)
		{
			// Get the ptr and clear context
			Operand ptr = context.Operand1;
			context.SetInstruction(IRInstruction.Nop);

			// Setup context for VmCall
			ReplaceWithVmCall(context, VmCall.MemorySet);

			// Set the operands
			context.SetOperand(1, ptr);
			context.SetOperand(2, ConstantZero);
			context.SetOperand(3, Operand.CreateConstant(TypeSystem, TypeLayout.GetTypeSize(ptr.Type.ElementType)));
			context.OperandCount = 4;
		}

		/// <summary>
		/// Visitation function for Sizeof instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Sizeof(Context context)
		{
			var type = context.MosaType;
			context.MosaType = null;
			var size = type.IsPointer ? TypeLayout.NativePointerSize : MethodCompiler.TypeLayout.GetTypeSize(type);
			context.SetInstruction(IRInstruction.Move, context.Result, Operand.CreateConstant(TypeSystem, size));
		}

		/// <summary>
		/// Visitation function for Add instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Add(Context context)
		{
			Replace(context, IRInstruction.AddFloat, IRInstruction.AddSigned, IRInstruction.AddUnsigned);
		}

		/// <summary>
		/// Visitation function for Sub instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Sub(Context context)
		{
			Replace(context, IRInstruction.SubFloat, IRInstruction.SubSigned, IRInstruction.SubUnsigned);
		}

		/// <summary>
		/// Visitation function for Mul instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Mul(Context context)
		{
			Replace(context, IRInstruction.MulFloat, IRInstruction.MulSigned, IRInstruction.MulUnsigned);
		}

		/// <summary>
		/// Visitation function for Div instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Div(Context context)
		{
			Replace(context, IRInstruction.DivFloat, IRInstruction.DivSigned, IRInstruction.DivUnsigned);
		}

		/// <summary>
		/// Visitation function for Rem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		private void Rem(Context context)
		{
			Replace(context, IRInstruction.RemFloat, IRInstruction.RemSigned, IRInstruction.RemUnsigned);
		}

		#endregion Visitation Methods

		#region Internals

		/// <summary>
		/// Calculates the element offset for the specified index.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="arrayType">The array type.</param>
		/// <param name="arrayIndexOperand">The index operand.</param>
		/// <returns>Element offset operand.</returns>
		private Operand CalculateArrayElementOffset(Context context, MosaType arrayType, Operand arrayIndexOperand)
		{
			int elementSizeInBytes = 0, alignment = 0;
			Architecture.GetTypeRequirements(TypeLayout, arrayType.ElementType, out elementSizeInBytes, out alignment);

			var elementOffset = MethodCompiler.CreateVirtualRegister(TypeSystem.BuiltIn.I4);
			var elementSizeOperand = Operand.CreateConstant(TypeSystem, elementSizeInBytes);
			context.InsertBefore().SetInstruction(IRInstruction.MulSigned, elementOffset, arrayIndexOperand, elementSizeOperand);

			return elementOffset;
		}

		/// <summary>
		/// Calculates the base of the array elements.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="arrayType">The array type.</param>
		/// <param name="arrayOperand">The array operand.</param>
		/// <returns>Base address for array elements.</returns>
		private Operand LoadArrayBaseAddress(Context context, MosaType arrayType, Operand arrayOperand)
		{
			var arrayPointer = arrayType.ElementType.ToManagedPointer();

			var arrayAddress = MethodCompiler.CreateVirtualRegister(arrayPointer);
			var fixedOffset = Operand.CreateConstant(TypeSystem, (NativePointerSize * 3));

			context.InsertBefore().SetInstruction(IRInstruction.AddSigned, arrayAddress, arrayOperand, fixedOffset);

			return arrayAddress;
		}

		/// <summary>
		/// Adds bounds check to the array access.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="arrayOperand">The array operand.</param>
		/// <param name="arrayIndexOperand">The index operand.</param>
		private void AddArrayBoundsCheck(Context context, Operand arrayOperand, Operand arrayIndexOperand)
		{
			// First create new block and split current block
			var exceptionContext = CreateNewBlockContexts(1)[0];
			var nextContext = Split(context);

			// Get array length
			var lengthOperand = MethodCompiler.CreateVirtualRegister(TypeSystem.BuiltIn.U4);
			var fixedOffset = Operand.CreateConstant(TypeSystem, (NativePointerSize * 2));
			context.SetInstruction(IRInstruction.Load, lengthOperand, arrayOperand, fixedOffset);

			// Now compare length with index
			// If index is greater than or equal to the length then jump to exception block, otherwise jump to next block
			context.AppendInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.UnsignedGreaterOrEqual, null, arrayIndexOperand, lengthOperand, exceptionContext.Block);
			context.AppendInstruction(IRInstruction.Jmp, nextContext.Block);

			// Build exception block which is just a call to throw exception
			var method = InternalRuntimeType.FindMethodByName("ThrowIndexOutOfRangeException");
			var symbolOperand = Operand.CreateSymbolFromMethod(TypeSystem, method);

			exceptionContext.AppendInstruction(IRInstruction.Call, null, symbolOperand);
			exceptionContext.InvokeMethod = method;
		}

		/// <summary>
		/// Converts the specified opcode.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		/// <returns></returns>
		private static ConditionCode ConvertCondition(CIL.OpCode opcode)
		{
			switch (opcode)
			{
				// Signed
				case CIL.OpCode.Beq_s: return ConditionCode.Equal;
				case CIL.OpCode.Bge_s: return ConditionCode.GreaterOrEqual;
				case CIL.OpCode.Bgt_s: return ConditionCode.GreaterThan;
				case CIL.OpCode.Ble_s: return ConditionCode.LessOrEqual;
				case CIL.OpCode.Blt_s: return ConditionCode.LessThan;

				// Unsigned
				case CIL.OpCode.Bne_un_s: return ConditionCode.NotEqual;
				case CIL.OpCode.Bge_un_s: return ConditionCode.UnsignedGreaterOrEqual;
				case CIL.OpCode.Bgt_un_s: return ConditionCode.UnsignedGreaterThan;
				case CIL.OpCode.Ble_un_s: return ConditionCode.UnsignedLessOrEqual;
				case CIL.OpCode.Blt_un_s: return ConditionCode.UnsignedLessThan;

				// Long form signed
				case CIL.OpCode.Beq: goto case CIL.OpCode.Beq_s;
				case CIL.OpCode.Bge: goto case CIL.OpCode.Bge_s;
				case CIL.OpCode.Bgt: goto case CIL.OpCode.Bgt_s;
				case CIL.OpCode.Ble: goto case CIL.OpCode.Ble_s;
				case CIL.OpCode.Blt: goto case CIL.OpCode.Blt_s;

				// Long form unsigned
				case CIL.OpCode.Bne_un: goto case CIL.OpCode.Bne_un_s;
				case CIL.OpCode.Bge_un: goto case CIL.OpCode.Bge_un_s;
				case CIL.OpCode.Bgt_un: goto case CIL.OpCode.Bgt_un_s;
				case CIL.OpCode.Ble_un: goto case CIL.OpCode.Ble_un_s;
				case CIL.OpCode.Blt_un: goto case CIL.OpCode.Blt_un_s;

				// Compare
				case CIL.OpCode.Ceq: return ConditionCode.Equal;
				case CIL.OpCode.Cgt: return ConditionCode.GreaterThan;
				case CIL.OpCode.Cgt_un: return ConditionCode.UnsignedGreaterThan;
				case CIL.OpCode.Clt: return ConditionCode.LessThan;
				case CIL.OpCode.Clt_un: return ConditionCode.UnsignedLessThan;

				default: throw new NotImplementedException();
			}
		}

		private static void Replace(Context context, BaseInstruction floatingPointInstruction, BaseInstruction signedInstruction, BaseInstruction unsignedInstruction)
		{
			if (context.Result.IsR)
			{
				context.ReplaceInstructionOnly(floatingPointInstruction);
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
		/// Determines if a store is silently truncating the value.
		/// </summary>
		/// <param name="destination">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <returns>True if the store is truncating, otherwise false.</returns>
		private static bool IsTruncating(Operand destination, Operand source)
		{
			if (destination.IsInt)
			{
				return (source.IsLong);
			}
			else if (destination.IsShort || destination.IsChar)
			{
				return (source.IsLong || source.IsInteger);
			}
			else if (destination.IsByte) // UNKNOWN: Add destination.IsBoolean
			{
				return (source.IsLong || source.IsInteger || source.IsShort);
			}

			return false;
		}

		/// <summary>
		/// Determines if the load should sign extend the given source operand.
		/// </summary>
		/// <param name="source">The source operand to determine sign extension for.</param>
		/// <returns>
		/// True if the given operand should be loaded with its sign extended.
		/// </returns>
		private static bool MustSignExtendOnLoad(MosaType source)
		{
			return source.IsI1 || source.IsI2;
		}

		/// <summary>
		/// Determines if the load should sign extend the given source operand.
		/// </summary>
		/// <param name="source">The source operand to determine sign extension for.</param>
		/// <returns>
		/// True if the given operand should be loaded with its sign extended.
		/// </returns>
		private static bool MustZeroExtendOnLoad(MosaType source)
		{
			return source.IsU1 || source.IsU2 || source.IsChar || source.IsBoolean;
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

		private static readonly BaseIRInstruction[][] convTable = new BaseIRInstruction[13][] {
			/* I1 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.Move,
				/* I2 */ IRInstruction.LogicalAnd,
				/* I4 */ IRInstruction.LogicalAnd,
				/* I8 */ IRInstruction.LogicalAnd,
				/* U1 */ IRInstruction.Move,
				/* U2 */ IRInstruction.LogicalAnd,
				/* U4 */ IRInstruction.LogicalAnd,
				/* U8 */ IRInstruction.LogicalAnd,
				/* R4 */ IRInstruction.FloatToIntegerConversion,
				/* R8 */ IRInstruction.FloatToIntegerConversion,
				/* I  */ IRInstruction.LogicalAnd,
				/* U  */ IRInstruction.LogicalAnd,
				/* Ptr*/ IRInstruction.LogicalAnd,
			},
			/* I2 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.SignExtendedMove,
				/* I2 */ IRInstruction.Move,
				/* I4 */ IRInstruction.LogicalAnd,
				/* I8 */ IRInstruction.LogicalAnd,
				/* U1 */ IRInstruction.ZeroExtendedMove,
				/* U2 */ IRInstruction.Move,
				/* U4 */ IRInstruction.LogicalAnd,
				/* U8 */ IRInstruction.LogicalAnd,
				/* R4 */ IRInstruction.FloatToIntegerConversion,
				/* R8 */ IRInstruction.FloatToIntegerConversion,
				/* I  */ IRInstruction.LogicalAnd,
				/* U  */ IRInstruction.LogicalAnd,
				/* Ptr*/ IRInstruction.LogicalAnd,
			},
			/* I4 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.SignExtendedMove,
				/* I2 */ IRInstruction.SignExtendedMove,
				/* I4 */ IRInstruction.Move,
				/* I8 */ IRInstruction.LogicalAnd,
				/* U1 */ IRInstruction.ZeroExtendedMove,
				/* U2 */ IRInstruction.ZeroExtendedMove,
				/* U4 */ IRInstruction.Move,
				/* U8 */ IRInstruction.LogicalAnd,
				/* R4 */ IRInstruction.FloatToIntegerConversion,
				/* R8 */ IRInstruction.FloatToIntegerConversion,
				/* I  */ IRInstruction.LogicalAnd,
				/* U  */ IRInstruction.LogicalAnd,
				/* Ptr*/ IRInstruction.LogicalAnd,
			},
			/* I8 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.SignExtendedMove,
				/* I2 */ IRInstruction.SignExtendedMove,
				/* I4 */ IRInstruction.SignExtendedMove,
				/* I8 */ IRInstruction.Move,
				/* U1 */ IRInstruction.ZeroExtendedMove,
				/* U2 */ IRInstruction.ZeroExtendedMove,
				/* U4 */ IRInstruction.ZeroExtendedMove,
				/* U8 */ IRInstruction.Move,
				/* R4 */ IRInstruction.FloatToIntegerConversion,
				/* R8 */ IRInstruction.FloatToIntegerConversion,
				/* I  */ IRInstruction.LogicalAnd,
				/* U  */ IRInstruction.LogicalAnd,
				/* Ptr*/ IRInstruction.LogicalAnd,
			},
			/* U1 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.Move,
				/* I2 */ IRInstruction.LogicalAnd,
				/* I4 */ IRInstruction.LogicalAnd,
				/* I8 */ IRInstruction.LogicalAnd,
				/* U1 */ IRInstruction.Move,
				/* U2 */ IRInstruction.LogicalAnd,
				/* U4 */ IRInstruction.LogicalAnd,
				/* U8 */ IRInstruction.LogicalAnd,
				/* R4 */ IRInstruction.FloatToIntegerConversion,
				/* R8 */ IRInstruction.FloatToIntegerConversion,
				/* I  */ IRInstruction.LogicalAnd,
				/* U  */ IRInstruction.LogicalAnd,
				/* Ptr*/ IRInstruction.LogicalAnd,
			},
			/* U2 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.ZeroExtendedMove,
				/* I2 */ IRInstruction.Move,
				/* I4 */ IRInstruction.LogicalAnd,
				/* I8 */ IRInstruction.LogicalAnd,
				/* U1 */ IRInstruction.ZeroExtendedMove,
				/* U2 */ IRInstruction.Move,
				/* U4 */ IRInstruction.LogicalAnd,
				/* U8 */ IRInstruction.LogicalAnd,
				/* R4 */ IRInstruction.FloatToIntegerConversion,
				/* R8 */ IRInstruction.FloatToIntegerConversion,
				/* I  */ IRInstruction.LogicalAnd,
				/* U  */ IRInstruction.LogicalAnd,
				/* Ptr*/ IRInstruction.LogicalAnd,
			},
			/* U4 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.ZeroExtendedMove,
				/* I2 */ IRInstruction.ZeroExtendedMove,
				/* I4 */ IRInstruction.Move,
				/* I8 */ IRInstruction.LogicalAnd,
				/* U1 */ IRInstruction.ZeroExtendedMove,
				/* U2 */ IRInstruction.ZeroExtendedMove,
				/* U4 */ IRInstruction.Move,
				/* U8 */ IRInstruction.LogicalAnd,
				/* R4 */ IRInstruction.FloatToIntegerConversion,
				/* R8 */ IRInstruction.FloatToIntegerConversion,
				/* I  */ IRInstruction.LogicalAnd,
				/* U  */ IRInstruction.LogicalAnd,
				/* Ptr*/ IRInstruction.LogicalAnd,
			},
			/* U8 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.ZeroExtendedMove,
				/* I2 */ IRInstruction.ZeroExtendedMove,
				/* I4 */ IRInstruction.ZeroExtendedMove,
				/* I8 */ IRInstruction.Move,
				/* U1 */ IRInstruction.ZeroExtendedMove,
				/* U2 */ IRInstruction.ZeroExtendedMove,
				/* U4 */ IRInstruction.ZeroExtendedMove,
				/* U8 */ IRInstruction.Move,
				/* R4 */ IRInstruction.FloatToIntegerConversion,
				/* R8 */ IRInstruction.FloatToIntegerConversion,
				/* I  */ IRInstruction.LogicalAnd,
				/* U  */ IRInstruction.LogicalAnd,
				/* Ptr*/ IRInstruction.LogicalAnd,
			},
			/* R4 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.IntegerToFloatConversion,
				/* I2 */ IRInstruction.IntegerToFloatConversion,
				/* I4 */ IRInstruction.IntegerToFloatConversion,
				/* I8 */ IRInstruction.IntegerToFloatConversion,
				/* U1 */ IRInstruction.IntegerToFloatConversion,
				/* U2 */ IRInstruction.IntegerToFloatConversion,
				/* U4 */ IRInstruction.IntegerToFloatConversion,
				/* U8 */ IRInstruction.IntegerToFloatConversion,
				/* R4 */ IRInstruction.Move,
				/* R8 */ IRInstruction.Move,
				/* I  */ IRInstruction.IntegerToFloatConversion,
				/* U  */ IRInstruction.IntegerToFloatConversion,
				/* Ptr*/ null,
			},
			/* R8 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.IntegerToFloatConversion,
				/* I2 */ IRInstruction.IntegerToFloatConversion,
				/* I4 */ IRInstruction.IntegerToFloatConversion,
				/* I8 */ IRInstruction.IntegerToFloatConversion,
				/* U1 */ IRInstruction.IntegerToFloatConversion,
				/* U2 */ IRInstruction.IntegerToFloatConversion,
				/* U4 */ IRInstruction.IntegerToFloatConversion,
				/* U8 */ IRInstruction.IntegerToFloatConversion,
				/* R4 */ IRInstruction.Move,
				/* R8 */ IRInstruction.Move,
				/* I  */ IRInstruction.IntegerToFloatConversion,
				/* U  */ IRInstruction.IntegerToFloatConversion,
				/* Ptr*/ null,
			},
			/* I  */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.SignExtendedMove,
				/* I2 */ IRInstruction.SignExtendedMove,
				/* I4 */ IRInstruction.SignExtendedMove,
				/* I8 */ IRInstruction.Move,
				/* U1 */ IRInstruction.ZeroExtendedMove,
				/* U2 */ IRInstruction.ZeroExtendedMove,
				/* U4 */ IRInstruction.ZeroExtendedMove,
				/* U8 */ IRInstruction.ZeroExtendedMove,
				/* R4 */ IRInstruction.FloatToIntegerConversion,
				/* R8 */ IRInstruction.FloatToIntegerConversion,
				/* I  */ IRInstruction.Move,
				/* U  */ IRInstruction.Move,
				/* Ptr*/ IRInstruction.Move,
			},
			/* U  */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.ZeroExtendedMove,
				/* I2 */ IRInstruction.ZeroExtendedMove,
				/* I4 */ IRInstruction.ZeroExtendedMove,
				/* I8 */ IRInstruction.ZeroExtendedMove,
				/* U1 */ IRInstruction.ZeroExtendedMove,
				/* U2 */ IRInstruction.ZeroExtendedMove,
				/* U4 */ IRInstruction.ZeroExtendedMove,
				/* U8 */ IRInstruction.Move,
				/* R4 */ IRInstruction.FloatToIntegerConversion,
				/* R8 */ IRInstruction.FloatToIntegerConversion,
				/* I  */ IRInstruction.Move,
				/* U  */ IRInstruction.Move,
				/* Ptr*/ IRInstruction.Move,
			},
			/* Ptr*/ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.ZeroExtendedMove,
				/* I2 */ IRInstruction.ZeroExtendedMove,
				/* I4 */ IRInstruction.ZeroExtendedMove,
				/* I8 */ IRInstruction.ZeroExtendedMove,
				/* U1 */ IRInstruction.ZeroExtendedMove,
				/* U2 */ IRInstruction.ZeroExtendedMove,
				/* U4 */ IRInstruction.ZeroExtendedMove,
				/* U8 */ IRInstruction.ZeroExtendedMove,
				/* R4 */ null,
				/* R8 */ null,
				/* I  */ IRInstruction.Move,
				/* U  */ IRInstruction.Move,
				/* Ptr*/ IRInstruction.Move,
			},
		};

		private void CheckAndConvertInstruction(Context context)
		{
			var destinationOperand = context.Result;
			var sourceOperand = context.Operand1;

			int destIndex = GetIndex(destinationOperand.Type, NativeInstructionSize == InstructionSize.Size32);
			int srcIndex = GetIndex(sourceOperand.Type, NativeInstructionSize == InstructionSize.Size32);

			var type = convTable[destIndex][srcIndex];

			if (type == null)
				throw new InvalidCompilerException();

			uint mask = 0xFFFFFFFF;
			var instruction = ComputeExtensionTypeAndMask(destinationOperand.Type, ref mask);

			if (type == IRInstruction.LogicalAnd)
			{
				if (mask == 0)
				{
					// TODO: May not be correct
					context.SetInstruction(IRInstruction.Move, destinationOperand, sourceOperand);
				}
				else
				{
					if (sourceOperand.IsLong)
					{
						Operand temp = AllocateVirtualRegister(destinationOperand.Type);

						context.SetInstruction(IRInstruction.Move, temp, sourceOperand);
						context.AppendInstruction(type, destinationOperand, temp, Operand.CreateConstant(TypeSystem, (int)mask));
					}
					else
					{
						context.SetInstruction(type, destinationOperand, sourceOperand, Operand.CreateConstant(TypeSystem, (int)mask));
					}
				}
			}
			else
			{
				context.SetInstruction(type, destinationOperand, sourceOperand);
			}
		}

		private BaseInstruction ComputeExtensionTypeAndMask(MosaType destinationType, ref uint mask)
		{
			if (destinationType.IsUI1)
			{
				mask = 0xFF;
				return (destinationType.IsSigned ? (BaseInstruction)IRInstruction.SignExtendedMove : (BaseInstruction)IRInstruction.ZeroExtendedMove);
			}
			else if (destinationType.IsUI2)
			{
				mask = 0xFFFF;
				return destinationType.IsSigned ? (BaseInstruction)IRInstruction.SignExtendedMove : (BaseInstruction)IRInstruction.ZeroExtendedMove;
			}
			else if (destinationType.IsUI4)
			{
				mask = 0xFFFFFFFF;
			}
			else if (destinationType.IsUI8)
			{
				mask = 0x0;
			}

			return null;
		}

		/// <summary>
		/// Processes external method calls.
		/// </summary>
		/// <param name="context">The transformation context.</param>
		/// <returns>
		/// 	<c>true</c> if the method was replaced by an intrinsic; <c>false</c> otherwise.
		/// </returns>
		/// <remarks>
		/// This method checks if the call target has an Intrinsic-Attribute applied with
		/// the current architecture. If it has, the method call is replaced by the specified
		/// native instruction.
		/// </remarks>
		private bool ProcessExternalCall(Context context)
		{
			string external = context.InvokeMethod.ExternMethod;
			bool isInternal = context.InvokeMethod.IsInternal;

			Type intrinsicType = null;

			if (external != null)
			{
				intrinsicType = Type.GetType(external);
			}
			else if (isInternal)
			{
				MethodCompiler.Compiler.IntrinsicTypes.TryGetValue(context.InvokeMethod.FullName, out intrinsicType);
				if (intrinsicType == null)
					MethodCompiler.Compiler.IntrinsicTypes.TryGetValue(context.InvokeMethod.DeclaringType.FullName + "::" + context.InvokeMethod.Name, out intrinsicType);

				Debug.Assert(intrinsicType != null, "Method is internal but no processor found: " + context.InvokeMethod.FullName);
			}

			if (intrinsicType == null)
				return false;

			var instance = Activator.CreateInstance(intrinsicType);

			var instanceMethod = instance as IIntrinsicInternalMethod;
			if (instanceMethod != null)
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

		private bool CanSkipDueToRecursiveSystemObjectCtorCall(Context context)
		{
			var currentMethod = MethodCompiler.Method;
			var invokeTarget = context.InvokeMethod;

			// Skip recursive System.Object ctor calls.
			if (currentMethod.DeclaringType.FullName == @"System.Object" &&
				currentMethod.Name == @".ctor" &&
				invokeTarget.DeclaringType.FullName == @"System.Object" &&
				invokeTarget.Name == @".ctor")
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Replaces the IL load instruction by an appropriate IR move instruction or removes it entirely, if
		/// it is a native size.
		/// </summary>
		/// <param name="context">Provides the transformation context.</param>
		private void ProcessLoadInstruction(Context context)
		{
			var source = context.Operand1;
			var destination = context.Result;
			var size = GetInstructionSize(source.Type);

			BaseIRInstruction instruction = IRInstruction.Move;

			if (MustSignExtendOnLoad(source.Type))
			{
				instruction = IRInstruction.SignExtendedMove;
			}
			else if (MustZeroExtendOnLoad(source.Type))
			{
				instruction = IRInstruction.ZeroExtendedMove;
			}

			context.SetInstruction(instruction, size, destination, source);
		}

		/// <summary>
		/// Replaces the instruction with an internal call.
		/// </summary>
		/// <param name="context">The transformation context.</param>
		/// <param name="internalCallTarget">The internal call target.</param>
		private void ReplaceWithVmCall(Context context, VmCall internalCallTarget)
		{
			var method = InternalRuntimeType.FindMethodByName(internalCallTarget.ToString());

			if (method == null)
			{
				method = PlatformInternalRuntimeType.FindMethodByName(internalCallTarget.ToString());
			}

			Debug.Assert(method != null, "Cannot find method: " + internalCallTarget.ToString());

			context.ReplaceInstructionOnly(IRInstruction.Call);
			context.SetOperand(0, Operand.CreateSymbolFromMethod(TypeSystem, method));
			context.OperandCount = 1;
			context.InvokeMethod = method;
		}

		private bool ReplaceWithInternalCall(Context context)
		{
			var method = context.InvokeMethod;

			if (!method.IsInternal)
				return false;

			string replacementMethod = BuildInternalCallName(method);

			method = method.DeclaringType.FindMethodByNameAndParameters(replacementMethod, method.Signature.Parameters);

			Operand result = context.Result;

			var operands = new List<Operand>(context.Operands);

			ProcessInvokeInstruction(context, method, result, operands);

			return true;
		}

		private string BuildInternalCallName(MosaMethod method)
		{
			string name = method.Name;
			if (name == @".ctor")
			{
				name = @"Create" + method.DeclaringType.Name;
			}
			else
			{
				name = @"Internal" + name;
			}

			return name;
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
