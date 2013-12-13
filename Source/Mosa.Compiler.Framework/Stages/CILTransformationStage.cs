/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using Mosa.Compiler.TypeSystem.Cil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Transforms CIL instructions into their appropriate IR.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms CIL instructions into their equivalent IR sequences.
	/// </remarks>
	public sealed class CILTransformationStage : BaseCodeTransformationStage, CIL.ICILVisitor, IPipelineStage
	{
		#region ICILVisitor

		/// <summary>
		/// Visitation function for Ldarg instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldarg(Context context)
		{
			ProcessLoadInstruction(context);
		}

		/// <summary>
		/// Visitation function for Ldarga instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldarga(Context context)
		{
			context.ReplaceInstructionOnly(IRInstruction.AddressOf);
		}

		/// <summary>
		/// Visitation function for Ldloc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldloc(Context context)
		{
			ProcessLoadInstruction(context);
		}

		/// <summary>
		/// Visitation function for Ldloca instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldloca(Context context)
		{
			context.ReplaceInstructionOnly(IRInstruction.AddressOf);
		}

		/// <summary>
		/// Visitation function for Ldc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldc(Context context)
		{
			ProcessLoadInstruction(context);
		}

		/// <summary>
		/// Visitation function for Ldobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldobj(Context context)
		{
			Operand destination = context.Result;
			Operand source = context.Operand1;

			SigType elementType = context.SigType as SigType;

			// This is actually ldind.* and ldobj - the opcodes have the same meanings

			BaseInstruction loadInstruction = IRInstruction.Load;
			if (MustSignExtendOnLoad(elementType.Type))
			{
				loadInstruction = IRInstruction.LoadSignExtended;
			}
			else if (MustZeroExtendOnLoad(elementType.Type))
			{
				loadInstruction = IRInstruction.LoadZeroExtended;
			}

			context.SetInstruction(loadInstruction, destination, source, Operand.CreateConstantSignedInt(0));
			context.SigType = elementType;
		}

		private SigType GetElementTypeFromSigType(SigType sigType)
		{
			RefSigType refSigType = sigType as RefSigType;
			if (refSigType != null)
				return refSigType.ElementType;

			PtrSigType ptrSigType = sigType as PtrSigType;
			if (ptrSigType != null)
				return ptrSigType.ElementType;

			throw new NotSupportedException(@"Invalid signature type: " + sigType.GetType());
		}

		/// <summary>
		/// Visitation function for Ldsfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldsfld(Context context)
		{
			SigType sigType = context.RuntimeField.SigType;
			Operand source = Operand.CreateRuntimeMember(context.RuntimeField);
			Operand destination = context.Result;

			if (MustSignExtendOnLoad(sigType.Type))
			{
				context.SetInstruction(IRInstruction.SignExtendedMove, destination, source);
			}
			else if (MustZeroExtendOnLoad(sigType.Type))
			{
				context.SetInstruction(IRInstruction.ZeroExtendedMove, destination, source);
			}
			else
			{
				context.SetInstruction(IRInstruction.Move, destination, source);
			}
		}

		/// <summary>
		/// Visitation function for Ldsflda instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldsflda(Context context)
		{
			context.SetInstruction(IRInstruction.AddressOf, context.Result, Operand.CreateRuntimeMember(context.RuntimeField));

			//context.AppendInstruction(IRInstruction.Move, context.Result, context.Operand1);
		}

		/// <summary>
		/// Visitation function for Ldftn instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldftn(Context context)
		{
			context.SetInstruction(IRInstruction.Move, context.Result, Operand.CreateSymbolFromMethod(context.InvokeMethod));
		}

		/// <summary>
		/// Visitation function for Ldvirtftn instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldvirtftn(Context context)
		{
			ReplaceWithVmCall(context, VmCall.GetVirtualFunctionPtr);
		}

		/// <summary>
		/// Visitation function for Ldtoken instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldtoken(Context context)
		{
			ReplaceWithVmCall(context, VmCall.GetHandleForToken);
		}

		/// <summary>
		/// Visitation function for Stloc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Stloc(Context context)
		{
			ProcessStoreInstruction(context);
		}

		/// <summary>
		/// Visitation function for Starg instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Starg(Context context)
		{
			ProcessStoreInstruction(context);
		}

		/// <summary>
		/// Visitation function for Stobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Stobj(Context context)
		{
			// This is actually stind.* and stobj - the opcodes have the same meanings
			context.SetInstruction(IRInstruction.Store, null, context.Operand1, Operand.CreateConstantSignedInt(0), context.Operand2);
		}

		/// <summary>
		/// Visitation function for Stsfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Stsfld(Context context)
		{
			context.SetInstruction(IRInstruction.Move, Operand.CreateRuntimeMember(context.RuntimeField), context.Operand1);
		}

		/// <summary>
		/// Visitation function for Dup instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Dup(Context context)
		{
			// We don't need the dup anymore.
			context.Remove();
		}

		/// <summary>
		/// Visitation function for Call instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Call(Context context)
		{
			if (this.CanSkipDueToRecursiveSystemObjectCtorCall(context))
			{
				context.Remove();
				return;
			}

			if (ProcessExternalCall(context))
				return;

			ProcessInvokeInstruction(context, context.InvokeMethod, context.Result, new List<Operand>(context.Operands));
		}

		/// <summary>
		/// Visitation function for Calli instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Calli(Context context)
		{
			Operand destinationOperand = context.GetOperand(context.OperandCount - 1);
			context.OperandCount -= 1;

			ProcessInvokeInstruction(context, context.InvokeMethod, context.Result, new List<Operand>(context.Operands));
		}

		/// <summary>
		/// Visitation function for Ret instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ret(Context context)
		{
			context.ReplaceInstructionOnly(IRInstruction.Return);
		}

		/// <summary>
		/// Visitation function for BinaryLogic instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.BinaryLogic(Context context)
		{
			if (context.Operand1.Type is ValueTypeSigType)
			{
				var type = methodCompiler.Method.Module.GetType((context.Operand1.Type as ValueTypeSigType).Token);
				var operand = Operand.CreateRuntimeMember(type.Fields[0].SigType, type.Fields[0], 0);
				context.SetOperand(0, operand);
			}

			if (context.Operand2.Type is ValueTypeSigType)
			{
				var type = methodCompiler.Method.Module.GetType((context.Operand2.Type as ValueTypeSigType).Token);
				var operand = Operand.CreateRuntimeMember(type.Fields[0].SigType, type.Fields[0], 0);
				context.SetOperand(1, operand);
			}

			switch ((context.Instruction as CIL.BaseCILInstruction).OpCode)
			{
				case CIL.OpCode.And: context.SetInstruction(IRInstruction.LogicalAnd, context.Result, context.Operand1, context.Operand2); break;
				case CIL.OpCode.Or: context.SetInstruction(IRInstruction.LogicalOr, context.Result, context.Operand1, context.Operand2); break;
				case CIL.OpCode.Xor: context.SetInstruction(IRInstruction.LogicalXor, context.Result, context.Operand1, context.Operand2); break;
				case CIL.OpCode.Div_un: context.SetInstruction(IRInstruction.DivU, context.Result, context.Operand1, context.Operand2); break;
				case CIL.OpCode.Rem_un: context.SetInstruction(IRInstruction.RemU, context.Result, context.Operand1, context.Operand2); break;
				default: throw new InvalidCompilerException();
			}
		}

		/// <summary>
		/// Visitation function for Shift instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Shift(Context context)
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
		void CIL.ICILVisitor.Neg(Context context)
		{
			if (context.Operand1.IsUnsigned)
			{
				Operand zero = Operand.CreateConstant(context.Operand1.Type, 0);
				context.SetInstruction(IRInstruction.SubU, context.Result, zero, context.Operand1);
			}
			else if (context.Operand1.IsSingle)
			{
				Operand minusOne = Operand.CreateConstantFloat(-1.0f);
				context.SetInstruction(IRInstruction.MulF, context.Result, minusOne, context.Operand1);
			}
			else if (context.Operand1.IsDouble)
			{
				Operand minusOne = Operand.CreateConstantDouble(-1.0d);
				context.SetInstruction(IRInstruction.MulF, context.Result, minusOne, context.Operand1);
			}
			else
			{
				Operand minusOne = Operand.CreateConstant(context.Operand1.Type, -1);
				context.SetInstruction(IRInstruction.MulS, context.Result, minusOne, context.Operand1);
			}
		}

		/// <summary>
		/// Visitation function for Not instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Not(Context context)
		{
			context.SetInstruction(IRInstruction.LogicalNot, context.Result, context.Operand1);
		}

		/// <summary>
		/// Visitation function for Conversion instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Conversion(Context context)
		{
			CheckAndConvertInstruction(context);
		}

		/// <summary>
		/// Visitation function for Callvirt instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Callvirt(Context context)
		{
			RuntimeMethod method = context.InvokeMethod;
			Operand resultOperand = context.Result;
			List<Operand> operands = new List<Operand>(context.Operands);

			if (context.Previous.Instruction is ConstrainedPrefixInstruction)
			{
				var type = context.Previous.RuntimeType;

				foreach (var m in type.Methods)
				{
					if (method.Name == method.Name)
					{
						if (method.Matches(method))
						{
							method = m;
							break;
						}
					}
				}

				context.Previous.Remove();

				ProcessInvokeInstruction(context, method, resultOperand, operands);

				return;
			}

			if ((method.Attributes & MethodAttributes.Virtual) == MethodAttributes.Virtual)
			{
				Operand thisPtr = context.Operand1;

				Operand methodTable = methodCompiler.CreateVirtualRegister(BuiltInSigType.IntPtr);
				Operand methodPtr = methodCompiler.CreateVirtualRegister(BuiltInSigType.IntPtr);

				if (!method.DeclaringType.IsInterface)
				{
					int methodTableOffset = CalculateMethodTableOffset(method) + (nativePointerSize * 5);
					context.SetInstruction(IRInstruction.Load, methodTable, thisPtr, Operand.CreateConstantSignedInt(0));
					context.AppendInstruction(IRInstruction.Load, methodPtr, methodTable, Operand.CreateConstantSignedInt((int)methodTableOffset));
				}
				else
				{
					int methodTableOffset = CalculateMethodTableOffset(method);
					int slotOffset = CalculateInterfaceSlotOffset(method);

					Operand interfaceSlotPtr = methodCompiler.CreateVirtualRegister(BuiltInSigType.IntPtr);
					Operand interfaceMethodTablePtr = methodCompiler.CreateVirtualRegister(BuiltInSigType.IntPtr);

					context.SetInstruction(IRInstruction.Load, methodTable, thisPtr, Operand.CreateConstantSignedInt(0));
					context.AppendInstruction(IRInstruction.Load, interfaceSlotPtr, methodTable, Operand.CreateConstantSignedInt(0));
					context.AppendInstruction(IRInstruction.Load, interfaceMethodTablePtr, interfaceSlotPtr, Operand.CreateConstantSignedInt((int)slotOffset));
					context.AppendInstruction(IRInstruction.Load, methodPtr, interfaceMethodTablePtr, Operand.CreateConstantSignedInt((int)methodTableOffset));
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

		private int CalculateMethodTableOffset(RuntimeMethod invokeTarget)
		{
			int slot = typeLayout.GetMethodTableOffset(invokeTarget);

			return (nativePointerSize * slot);
		}

		private int CalculateInterfaceSlotOffset(RuntimeMethod invokeTarget)
		{
			return CalculateInterfaceSlot(invokeTarget.DeclaringType) * nativePointerSize;
		}

		private int CalculateInterfaceSlot(RuntimeType interaceType)
		{
			return typeLayout.GetInterfaceSlotOffset(interaceType);
		}

		/// <summary>
		/// Visitation function for Newarr instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Newarr(Context context)
		{
			Operand thisReference = context.Result;
			Debug.Assert(thisReference != null, @"Newobj didn't specify class signature?");

			SZArraySigType arrayType = (SZArraySigType)context.Result.Type;
			int elementSize = 0;
			var elementSigType = arrayType.ElementType;

			// Handle builtin types
			if (elementSigType is BuiltInSigType)
			{
				var builtInSigType = elementSigType as BuiltInSigType;
				var alignment = 0;
				this.architecture.GetTypeRequirements(builtInSigType, out elementSize, out alignment);
			}

			// Handle classes and structs
			else if (elementSigType is ClassSigType)
			{
				var classSigType = elementSigType as ClassSigType;
				var module = this.typeModule;
				if (methodCompiler.Method.DeclaringType is CilGenericType)
					module = (methodCompiler.Method.DeclaringType as CilGenericType).InstantiationModule;
				var elementType = module.GetType(classSigType.Token);
				elementSize = typeLayout.GetTypeSize(elementType);
			}

			Operand lengthOperand = context.Operand1;

			// HACK: If we can't determine the size now, assume 16 bytes per array element.
			if (elementSize == 0)
			{
				elementSize = 16;
			}

			ReplaceWithVmCall(context, VmCall.AllocateArray);

			context.SetOperand(1, Operand.CreateConstantSignedInt((int)0));
			context.SetOperand(2, Operand.CreateConstantSignedInt((int)elementSize));
			context.SetOperand(3, lengthOperand);
			context.OperandCount = 4;
		}

		/// <summary>
		/// Visitation function for Newobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Newobj(Context context)
		{
			if (!ReplaceWithInternalCall(context))
			{
				Operand thisReference = context.Result;
				Debug.Assert(thisReference != null, @"Newobj didn't specify class signature?");
				RuntimeType classType = null;

				if (thisReference.Type is ClassSigType)
				{
					var classSigType = thisReference.Type as ClassSigType;
					classType = typeModule.GetType(classSigType.Token);
				}
				else if (thisReference.Type is GenericInstSigType)
				{
					var genericInstSigType = thisReference.Type as GenericInstSigType;
					var baseSigType = genericInstSigType.BaseType as ValueTypeSigType;
					classType = typeModule.GetType(baseSigType.Token);
				}

				if (classType.ContainsOpenGenericParameters)
				{
					if (!(classType is CilGenericType))
					{
						string name = CilGenericType.GetGenericTypeName(classType.Module, classType, (thisReference.Type as GenericInstSigType).GenericArguments);

						classType = new CilGenericType(classType.Module, classType.Token, name, classType, (thisReference.Type as GenericInstSigType).GenericArguments);
					}

					classType = methodCompiler.Compiler.GenericTypePatcher.PatchType(this.typeModule, methodCompiler.Method.DeclaringType as CilGenericType, classType as CilGenericType);
				}

				Context before = context.InsertBefore();

				ReplaceWithVmCall(before, VmCall.AllocateObject);

				Operand methodTableSymbol = GetMethodTableSymbol(classType);

				before.SetOperand(1, methodTableSymbol);
				before.SetOperand(2, Operand.CreateConstantSignedInt((int)typeLayout.GetTypeSize(classType)));
				before.OperandCount = 3;
				before.Result = thisReference;

				// Result is the this pointer, now invoke the real constructor
				List<Operand> operands = new List<Operand>(context.Operands);
				operands.Insert(0, thisReference);

				ProcessInvokeInstruction(context, context.InvokeMethod, null, operands);
			}
		}

		private Operand GetMethodTableSymbol(RuntimeType runtimeType)
		{
			return Operand.CreateSymbol(BuiltInSigType.IntPtr, runtimeType.FullName + @"$mtable");
		}

		private RuntimeMethod FindConstructor(RuntimeType classType, List<Operand> ctorOperands)
		{
			foreach (RuntimeMethod method in classType.Methods)
			{
				if (method.Name == @".ctor" && this.DoCtorParametersMatch(method, ctorOperands))
				{
					return method;
				}
			}

			throw new CompilationException(@"Failed to find constructor matching given argument list.");
		}

		private bool DoCtorParametersMatch(RuntimeMethod method, List<Operand> ctorOperands)
		{
			if (method.Parameters.Count != ctorOperands.Count)
				return false;

			for (int index = 0; index < ctorOperands.Count; index++)
			{
				if (!ctorOperands[index].Type.Matches(method.SigParameters[index]))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Visitation function for Castclass instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Castclass(Context context)
		{
			// TODO!
			//ReplaceWithVmCall(context, VmCall.Castclass);
			context.ReplaceInstructionOnly(IRInstruction.Move); // HACK!
		}

		/// <summary>
		/// Visitation function for Isinst instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.IsInst(Context context)
		{
			Operand reference = context.Operand1;
			Operand result = context.Result;

			ClassSigType classSigType = (ClassSigType)result.Type;
			RuntimeType classType = typeModule.GetType(classSigType.Token);
			Operand methodTableSymbol = GetMethodTableSymbol(classType);

			if (!classType.IsInterface)
			{
				ReplaceWithVmCall(context, VmCall.IsInstanceOfType);

				context.SetOperand(1, methodTableSymbol);
				context.SetOperand(2, reference);
				context.OperandCount = 3;
				context.ResultCount = 1;
			}
			else
			{
				int slot = CalculateInterfaceSlot(classType);

				ReplaceWithVmCall(context, VmCall.IsInstanceOfInterfaceType);

				context.SetOperand(1, Operand.CreateConstantUnsignedInt((uint)slot));
				context.SetOperand(2, reference);
				context.OperandCount = 3;
				context.ResultCount = 1;
			}
		}

		/// <summary>
		/// Visitation function for Unbox instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Unbox(Context context)
		{
			throw new NotImplementCompilerException();

			//ReplaceWithVmCall(context, VmCall.Unbox);
		}

		/// <summary>
		/// Visitation function for Throw instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Throw(Context context)
		{
			context.SetInstruction(IRInstruction.Throw, context.Result, context.Operand1);
		}

		/// <summary>
		/// Visitation function for Box instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Box(Context context)
		{
			var value = context.Operand1;
			var result = context.Result;
			var type = context.RuntimeType;

			// this type resolution is a hack
			if (type == null)
			{
				var sigType = result.Type as ValueTypeSigType;

				if (sigType != null)
				{
					type = typeModule.GetType(sigType.Token);
				}
			}

			if (type == null || !type.IsValueType)
			{
				context.ReplaceInstructionOnly(IRInstruction.Move);
				return;
			}

			var vmCall = TypeToVmBoxCall(type);

			if (!vmCall.HasValue)
			{
				context.ReplaceInstructionOnly(IRInstruction.Move);
				return;
			}

			context.SetInstruction(IRInstruction.Nop);
			ReplaceWithVmCall(context, vmCall.Value);

			var methodTableSymbol = GetMethodTableSymbol(type);
			var classSize = typeLayout.GetTypeSize(type);

			context.SetOperand(1, methodTableSymbol);
			context.SetOperand(2, Operand.CreateConstantUnsignedInt((uint)classSize));
			context.SetOperand(3, value);
			context.OperandCount = 4;
			context.Result = result;
			return;
		}

		/// <summary>
		/// Visitation function for BinaryComparison instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.BinaryComparison(Context context)
		{
			ConditionCode code = ConvertCondition((context.Instruction as CIL.BaseCILInstruction).OpCode);

			if (context.Operand1.StackType == StackTypeCode.F)
			{
				context.SetInstruction(IRInstruction.FloatingPointCompare, code, context.Result, context.Operand1, context.Operand2);
			}
			else
			{
				context.SetInstruction(IRInstruction.IntegerCompare, code, context.Result, context.Operand1, context.Operand2);
			}
		}

		/// <summary>
		/// Visitation function for Cpblk instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Cpblk(Context context)
		{
			ReplaceWithVmCall(context, VmCall.Memcpy);
		}

		/// <summary>
		/// Visitation function for Initblk instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Initblk(Context context)
		{
			ReplaceWithVmCall(context, VmCall.Memset);
		}

		/// <summary>
		/// Visitation function for Rethrow instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Rethrow(Context context)
		{
			ReplaceWithVmCall(context, VmCall.Rethrow);
		}

		/// <summary>
		/// Visitation function for Nop instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Nop(Context context)
		{
			context.SetInstruction(IRInstruction.Nop);
		}

		/// <summary>
		/// Visitation function for Pop instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Pop(Context context)
		{
			context.Remove();
		}

		#endregion ICILVisitor

		#region ICILVisitor - Unused

		/// <summary>
		/// Visitation function for Break instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Break(Context context)
		{
			context.SetInstruction(IRInstruction.Break);
		}

		/// <summary>
		/// Visitation function for Ldstr instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldstr(Context context)
		{
			/*
			 * This requires a special memory layout for strings as they are interned by the compiler
			 * into the generated image. This won't work this way forever: As soon as we'll support
			 * a real AppDomain and real string interning, this code will have to go away and will
			 * be replaced by a proper VM call.
			 *
			 */

			ILinker linker = methodCompiler.Linker;
			IMetadataModule assembly = methodCompiler.Assembly;

			string referencedString = assembly.Metadata.ReadUserString(context.TokenType);

			string symbolName = @"$ldstr$" + assembly.Name + "$" + context.TokenType.ToString("x");

			if (linker.GetSymbol(symbolName) == null)
			{
				using (Stream stream = linker.Allocate(symbolName, SectionKind.ROData, 0, nativePointerAlignment))
				{
					// Method table and sync block
					linker.Link(LinkType.AbsoluteAddress | LinkType.I4, BuiltInPatch.I4, symbolName, 0, 0, @"System.String$mtable", 0);
					stream.WriteZeroBytes(8);

					// String length field
					stream.Write(BitConverter.GetBytes(referencedString.Length), 0, nativePointerSize);

					// String data
					byte[] stringData = Encoding.Unicode.GetBytes(referencedString);
					Debug.Assert(stringData.Length == referencedString.Length * 2, @"Byte array of string data doesn't match expected string data length");
					stream.Write(stringData);
				}
			}

			Operand source = Operand.CreateSymbol(BuiltInSigType.String, symbolName);
			Operand destination = context.Result;

			context.SetInstruction(IRInstruction.Move, destination, source);
		}

		/// <summary>
		/// Visitation function for Ldfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldfld(Context context)
		{
			Operand resultOperand = context.Result;
			Operand objectOperand = context.Operand1;
			Operand result = methodCompiler.CreateVirtualRegister(context.RuntimeField.SigType);
			RuntimeField field = context.RuntimeField;

			int offset = typeLayout.GetFieldOffset(field);
			Operand offsetOperand = Operand.CreateConstantIntPtr(offset);

			BaseInstruction loadInstruction = IRInstruction.Load;
			if (MustSignExtendOnLoad(field.SigType.Type))
			{
				loadInstruction = IRInstruction.LoadSignExtended;
			}
			else if (MustZeroExtendOnLoad(field.SigType.Type))
			{
				loadInstruction = IRInstruction.LoadZeroExtended;
			}

			Debug.Assert(offsetOperand != null);

			context.SetInstruction(loadInstruction, result, objectOperand, offsetOperand);
			context.SigType = field.SigType;
			context.AppendInstruction(IRInstruction.Move, resultOperand, result);
		}

		/// <summary>
		/// Visitation function for Ldflda instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldflda(Context context)
		{
			Operand fieldAddress = context.Result;
			Operand objectOperand = context.Operand1;

			int offset = typeLayout.GetFieldOffset(context.RuntimeField);
			Operand fixedOffset = Operand.CreateConstantSignedInt((int)offset);

			context.SetInstruction(IRInstruction.AddU, fieldAddress, objectOperand, fixedOffset);
		}

		/// <summary>
		/// Visitation function for Stfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Stfld(Context context)
		{
			Operand objectOperand = context.Operand1;
			Operand valueOperand = context.Operand2;
			Operand temp = methodCompiler.CreateVirtualRegister(context.RuntimeField.SigType);

			int offset = typeLayout.GetFieldOffset(context.RuntimeField);
			Operand offsetOperand = Operand.CreateConstantIntPtr(offset);

			context.SetInstruction(IRInstruction.Move, temp, valueOperand);
			context.AppendInstruction(IRInstruction.Store, null, objectOperand, offsetOperand, temp);
		}

		/// <summary>
		/// Visitation function for Jmp instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Jmp(Context context)
		{
			throw new NotImplementCompilerException();
		}

		/// <summary>
		/// Visitation function for Branch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Branch(Context context)
		{
			context.ReplaceInstructionOnly(IRInstruction.Jmp);
		}

		/// <summary>
		/// Visitation function for UnaryBranch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.UnaryBranch(Context context)
		{
			int target = context.BranchTargets[0];

			Operand first = context.Operand1;
			Operand second = Operand.CreateConstantSignedInt((int)0);

			CIL.OpCode opcode = ((CIL.BaseCILInstruction)context.Instruction).OpCode;

			if (opcode == CIL.OpCode.Brtrue || opcode == CIL.OpCode.Brtrue_s)
			{
				context.SetInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.NotEqual, null, first, second);
				context.SetBranch(target);
				return;
			}
			else if (opcode == CIL.OpCode.Brfalse || opcode == CIL.OpCode.Brfalse_s)
			{
				context.SetInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.Equal, null, first, second);
				context.SetBranch(target);
				return;
			}

			throw new NotSupportedException(@"CILTransformationStage.UnaryBranch doesn't support CIL opcode " + opcode);
		}

		/// <summary>
		/// Visitation function for BinaryBranch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.BinaryBranch(Context context)
		{
			int target = context.BranchTargets[0];

			ConditionCode cc = ConvertCondition(((CIL.BaseCILInstruction)context.Instruction).OpCode);
			Operand first = context.Operand1;
			Operand second = context.Operand2;

			if (first.StackType == StackTypeCode.F)
			{
				Operand comparisonResult = methodCompiler.CreateVirtualRegister(BuiltInSigType.Int32);
				context.SetInstruction(IRInstruction.FloatingPointCompare, cc, comparisonResult, first, second);
				context.AppendInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.Equal, null, comparisonResult, Operand.CreateConstantIntPtr(1));
				context.SetBranch(target);
			}
			else
			{
				context.SetInstruction(IRInstruction.IntegerCompareBranch, cc, null, first, second);
				context.SetBranch(target);
			}
		}

		/// <summary>
		/// Visitation function for Switch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Switch(Context context)
		{
			context.ReplaceInstructionOnly(IRInstruction.Switch);
		}

		/// <summary>
		/// Visitation function for Cpobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Cpobj(Context context)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Visitation function for Ldlen instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldlen(Context context)
		{
			Operand arrayOperand = context.Operand1;
			Operand arrayLength = context.Result;
			Operand constantOffset = Operand.CreateConstantSignedInt(8);

			Operand arrayAddress = methodCompiler.CreateVirtualRegister(new PtrSigType(BuiltInSigType.Int32));
			context.SetInstruction(IRInstruction.Move, arrayAddress, arrayOperand);
			context.AppendInstruction(IRInstruction.Load, arrayLength, arrayAddress, constantOffset);
		}

		/// <summary>
		/// Visitation function for Ldelema instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldelema(Context context)
		{
			Operand result = context.Result;
			Operand arrayOperand = context.Operand1;
			Operand arrayIndexOperand = context.Operand2;

			SZArraySigType arraySigType = arrayOperand.Type as SZArraySigType;
			if (arraySigType == null)
			{
				throw new CompilationException(@"Array operation performed on non-array operand.");
			}

			Operand arrayAddress = this.LoadArrayBaseAddress(context, arraySigType, arrayOperand);
			Operand elementOffset = this.CalculateArrayElementOffset(context, arraySigType, arrayIndexOperand);
			context.AppendInstruction(IRInstruction.AddS, result, arrayAddress, elementOffset);
		}

		private Operand CalculateArrayElementOffset(Context context, SZArraySigType arraySignatureType, Operand arrayIndexOperand)
		{
			int elementSizeInBytes = 0, alignment = 0;
			architecture.GetTypeRequirements(arraySignatureType.ElementType, out elementSizeInBytes, out alignment);

			//
			// The sequence we're emitting is:
			//
			//      offset = arrayIndexOperand * elementSize
			//      temp = offset + 12
			//      result = *(arrayOperand * temp)
			//
			// The array data starts at offset 12 from the array object itself. The 12 is a hardcoded assumption
			// of x86, which might change for other platforms. We need to refactor this into some helper classes.
			//

			Operand elementOffset = methodCompiler.CreateVirtualRegister(BuiltInSigType.Int32);
			Operand elementSizeOperand = Operand.CreateConstantSignedInt((int)elementSizeInBytes);
			context.AppendInstruction(IRInstruction.MulS, elementOffset, arrayIndexOperand, elementSizeOperand);

			return elementOffset;
		}

		private Operand LoadArrayBaseAddress(Context context, SZArraySigType arraySignatureType, Operand arrayOperand)
		{
			Operand arrayAddress = methodCompiler.CreateVirtualRegister(new PtrSigType(arraySignatureType.ElementType));
			Operand fixedOffset = Operand.CreateConstantSignedInt((int)12);
			context.SetInstruction(IRInstruction.AddS, arrayAddress, arrayOperand, fixedOffset);
			return arrayAddress;
		}

		/// <summary>
		/// Visitation function for Ldelem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldelem(Context context)
		{
			Operand result = context.Result;
			Operand arrayOperand = context.Operand1;
			Operand arrayIndexOperand = context.Operand2;

			SZArraySigType arraySigType = arrayOperand.Type as SZArraySigType;

			if (arraySigType == null)
			{
				throw new CompilationException(@"Array operation performed on non-array operand.");
			}

			BaseInstruction loadInstruction = IRInstruction.Load;
			if (MustSignExtendOnLoad(arraySigType.ElementType.Type))
			{
				loadInstruction = IRInstruction.LoadSignExtended;
			}
			else if (MustZeroExtendOnLoad(arraySigType.ElementType.Type))
			{
				loadInstruction = IRInstruction.LoadZeroExtended;
			}

			Operand arrayAddress = LoadArrayBaseAddress(context, arraySigType, arrayOperand);
			Operand elementOffset = CalculateArrayElementOffset(context, arraySigType, arrayIndexOperand);

			Debug.Assert(elementOffset != null);

			context.AppendInstruction(loadInstruction, result, arrayAddress, elementOffset);
			context.SigType = arraySigType.ElementType;
		}

		/// <summary>
		/// Visitation function for Stelem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Stelem(Context context)
		{
			Operand arrayOperand = context.Operand1;
			Operand arrayIndexOperand = context.Operand2;
			Operand value = context.Operand3;
			SZArraySigType arraySigType = arrayOperand.Type as SZArraySigType;
			if (arraySigType == null)
			{
				throw new CompilationException(@"Array operation performed on non-array operand.");
			}

			Operand arrayAddress = this.LoadArrayBaseAddress(context, arraySigType, arrayOperand);
			Operand elementOffset = this.CalculateArrayElementOffset(context, arraySigType, arrayIndexOperand);
			context.AppendInstruction(IRInstruction.Store, null, arrayAddress, elementOffset, value);
		}

		/// <summary>
		/// Visitation function for UnboxAny instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.UnboxAny(Context context)
		{
			var value = context.Operand1;
			var type = context.RuntimeType;
			var result = context.Result;

			if (!type.IsValueType)
			{
				context.ReplaceInstructionOnly(IRInstruction.Move);
				return;
			}

			ReplaceWithVmCall(context, TypeToVmUnboxCall(type));

			context.SetOperand(1, value);
			context.OperandCount = 2;
			context.Result = result;
		}

		/// <summary>
		/// Visitation function for Refanyval instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Refanyval(Context context)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Visitation function for UnaryArithmetic instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.UnaryArithmetic(Context context)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Mkrefany(Context context)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Visitation function for ArithmeticOverflow instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.ArithmeticOverflow(Context context)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Visitation function for Endfinally instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Endfinally(Context context)
		{
			context.SetInstruction(IRInstruction.InternalReturn);
		}

		private ExceptionHandlingClause FindImmediateClause(Context context)
		{
			ExceptionHandlingClause innerClause = null;
			int label = context.Label;

			foreach (var clause in methodCompiler.ExceptionHandlingClauses)
			{
				if (clause.IsLabelWithinTry(label) || clause.IsLabelWithinHandler(label))
				{
					return clause;
				}
			}

			return null;
		}

		/// <summary>
		/// Visitation function for Leave instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Leave(Context context)
		{
			// Find enclosing finally clause
			ExceptionHandlingClause clause = FindImmediateClause(context);

			if (clause.IsLabelWithinTry(context.Label))
			{
				if (clause != null)
				{
					// Find finally block
					BasicBlock finallyBlock = basicBlocks.GetByLabel(clause.HandlerOffset);

					Context before = context.InsertBefore();
					before.SetInstruction(IRInstruction.Call, finallyBlock);
				}
			}
			else if (clause.IsLabelWithinHandler(context.Label))
			{
				// nothing!
			}
			else
			{
				throw new Exception("can not find leave clause");
			}

			context.ReplaceInstructionOnly(IRInstruction.Jmp);
		}

		/// <summary>
		/// Visitation function for Arglist instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Arglist(Context context)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Visitation function for Localalloc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Localalloc(Context context)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Visitation function for Endfilter instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Endfilter(Context context)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Visitation function for InitObj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.InitObj(Context context)
		{
			// TODO: Not implemented!
		}

		/// <summary>
		/// Visitation function for Prefix instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Prefix(Context context)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Visitation function for Sizeof instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Sizeof(Context context)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Visitation function for Refanytype instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Refanytype(Context context)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Visitation function for Add instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Add(Context context)
		{
			Replace(context, IRInstruction.AddF, IRInstruction.AddS, IRInstruction.AddU);
		}

		/// <summary>
		/// Visitation function for Sub instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Sub(Context context)
		{
			Replace(context, IRInstruction.SubF, IRInstruction.SubS, IRInstruction.SubU);
		}

		/// <summary>
		/// Visitation function for Mul instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Mul(Context context)
		{
			Replace(context, IRInstruction.MulF, IRInstruction.MulS, IRInstruction.MulU);
		}

		/// <summary>
		/// Visitation function for Div instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Div(Context context)
		{
			Replace(context, IRInstruction.DivF, IRInstruction.DivS, IRInstruction.DivU);
		}

		/// <summary>
		/// Visitation function for Rem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Rem(Context context)
		{
			Replace(context, IRInstruction.RemF, IRInstruction.RemS, IRInstruction.RemU);
		}

		#endregion ICILVisitor - Unused

		#region Internals

		/// <summary>
		/// Converts the specified opcode.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		/// <returns></returns>
		public static ConditionCode ConvertCondition(CIL.OpCode opcode)
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
			if (context.Result.IsFloatingPoint)
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
		/// <returns>True if the given operand should be loaded with its sign extended.</returns>
		private static bool IsSignExtending(Operand source)
		{
			return MustSignExtendOnLoad(source.Type.Type);
		}

		private static bool MustSignExtendOnLoad(CilElementType elementType)
		{
			return (elementType == CilElementType.I1 || elementType == CilElementType.I2);
		}

		/// <summary>
		/// Determines if the load should sign extend the given source operand.
		/// </summary>
		/// <param name="source">The source operand to determine sign extension for.</param>
		/// <returns>True if the given operand should be loaded with its sign extended.</returns>
		private static bool IsZeroExtending(Operand source)
		{
			return MustZeroExtendOnLoad(source.Type.Type);
		}

		private static bool MustZeroExtendOnLoad(CilElementType elementType)
		{
			return (elementType == CilElementType.U1 || elementType == CilElementType.U2 || elementType == CilElementType.Char);
		}

		/// <summary>
		/// Determines the implicit result type of the load instruction.
		/// </summary>
		/// <param name="sigType">The signature type of the source operand.</param>
		/// <returns>The signature type of the result.</returns>
		/// <remarks>
		/// This method performs the implicit type conversion mandated by the CIL spec
		/// for load instructions.
		/// </remarks>
		private SigType GetImplicitResultSigType(SigType sigType)
		{
			SigType result;

			switch (sigType.Type)
			{
				case CilElementType.I1: goto case CilElementType.I2;
				case CilElementType.I2: result = BuiltInSigType.Int32; break;
				case CilElementType.U1: goto case CilElementType.U2;
				case CilElementType.U2: result = BuiltInSigType.UInt32; break;
				case CilElementType.Char: goto case CilElementType.U2;
				case CilElementType.Boolean: goto case CilElementType.U2;
				default: result = sigType; break;
			}

			return result;
		}

		/// <summary>
		/// Gets the index of the cil type.
		/// </summary>
		/// <param name="cilElementType">Type of the cil element.</param>
		/// <returns></returns>
		/// <exception cref="InvalidCompilerException"></exception>
		private int GetCilTypeIndex(CilElementType cilElementType)
		{
			switch (cilElementType)
			{
				case CilElementType.Char: return 5;
				case CilElementType.I1: return 0;
				case CilElementType.I2: return 1;
				case CilElementType.I4: return 2;
				case CilElementType.I8: return 3;
				case CilElementType.U1: return 4;
				case CilElementType.U2: return 5;
				case CilElementType.U4: return 6;
				case CilElementType.U8: return 7;
				case CilElementType.R4: return 8;
				case CilElementType.R8: return 9;
				case CilElementType.I: return 10;
				case CilElementType.U: return 11;
				case CilElementType.Ptr: return 12;
				case CilElementType.ByRef: return 12;
				default: break;
			}

			throw new InvalidCompilerException();
		}

		private static readonly BaseIRInstruction[][] s_convTable = new BaseIRInstruction[13][] {
			/* I1 */ new BaseIRInstruction[13] {
				/* I1 */ IRInstruction.Move,
				/* I2 */ IRInstruction.LogicalAnd,
				/* I4 */ IRInstruction.LogicalAnd,
				/* I8 */ IRInstruction.LogicalAnd,
				/* U1 */ IRInstruction.Move,
				/* U2 */ IRInstruction.LogicalAnd,
				/* U4 */ IRInstruction.LogicalAnd,
				/* U8 */ IRInstruction.LogicalAnd,
				/* R4 */ IRInstruction.FloatingPointToIntegerConversion,
				/* R8 */ IRInstruction.FloatingPointToIntegerConversion,
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
				/* R4 */ IRInstruction.FloatingPointToIntegerConversion,
				/* R8 */ IRInstruction.FloatingPointToIntegerConversion,
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
				/* R4 */ IRInstruction.FloatingPointToIntegerConversion,
				/* R8 */ IRInstruction.FloatingPointToIntegerConversion,
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
				/* R4 */ IRInstruction.FloatingPointToIntegerConversion,
				/* R8 */ IRInstruction.FloatingPointToIntegerConversion,
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
				/* R4 */ IRInstruction.FloatingPointToIntegerConversion,
				/* R8 */ IRInstruction.FloatingPointToIntegerConversion,
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
				/* R4 */ IRInstruction.FloatingPointToIntegerConversion,
				/* R8 */ IRInstruction.FloatingPointToIntegerConversion,
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
				/* R4 */ IRInstruction.FloatingPointToIntegerConversion,
				/* R8 */ IRInstruction.FloatingPointToIntegerConversion,
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
				/* R4 */ IRInstruction.FloatingPointToIntegerConversion,
				/* R8 */ IRInstruction.FloatingPointToIntegerConversion,
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
				/* I8 */ IRInstruction.SignExtendedMove,
				/* U1 */ IRInstruction.ZeroExtendedMove,
				/* U2 */ IRInstruction.ZeroExtendedMove,
				/* U4 */ IRInstruction.ZeroExtendedMove,
				/* U8 */ IRInstruction.ZeroExtendedMove,
				/* R4 */ IRInstruction.FloatingPointToIntegerConversion,
				/* R8 */ IRInstruction.FloatingPointToIntegerConversion,
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
				/* U8 */ IRInstruction.ZeroExtendedMove,
				/* R4 */ IRInstruction.FloatingPointToIntegerConversion,
				/* R8 */ IRInstruction.FloatingPointToIntegerConversion,
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
			Operand destinationOperand = context.Result;
			Operand sourceOperand = context.Operand1;

			int destIndex = GetCilTypeIndex(destinationOperand.Type.Type);
			int srcIndex = GetCilTypeIndex(sourceOperand.Type.Type);

			BaseIRInstruction type = s_convTable[destIndex][srcIndex];

			if (type == null)
				throw new InvalidCompilerException();

			uint mask = 0xFFFFFFFF;
			BaseInstruction instruction = ComputeExtensionTypeAndMask(destinationOperand.Type.Type, ref mask);

			if (type == IRInstruction.LogicalAnd)
			{
				if (mask == 0)
				{
					// TODO: May not be correct
					context.SetInstruction(IRInstruction.Move, destinationOperand, sourceOperand);
				}
				else
				{
					if (sourceOperand.Type.Type == CilElementType.I8 || sourceOperand.Type.Type == CilElementType.U8)
					{
						Operand temp = AllocateVirtualRegister(destinationOperand.Type);

						context.SetInstruction(IRInstruction.Move, temp, sourceOperand);
						context.AppendInstruction(type, destinationOperand, temp, Operand.CreateConstantUnsignedInt((uint)mask));
					}
					else
					{
						context.SetInstruction(type, destinationOperand, sourceOperand, Operand.CreateConstantUnsignedInt((uint)mask));
					}
				}
			}
			else
			{
				context.SetInstruction(type, destinationOperand, sourceOperand);
			}
		}

		private BaseInstruction ComputeExtensionTypeAndMask(CilElementType destinationType, ref uint mask)
		{
			switch (destinationType)
			{
				case CilElementType.I1: mask = 0xFF; return IRInstruction.SignExtendedMove;
				case CilElementType.I2: mask = 0xFFFF; return IRInstruction.SignExtendedMove;
				case CilElementType.I4: mask = 0xFFFFFFFF; break;
				case CilElementType.I8: mask = 0x0; break;
				case CilElementType.U1: mask = 0xFF; return IRInstruction.ZeroExtendedMove;
				case CilElementType.U2: mask = 0xFFFF; return IRInstruction.ZeroExtendedMove;
				case CilElementType.U4: mask = 0xFFFFFFFF; break;
				case CilElementType.U8: mask = 0x0; break;
				case CilElementType.R4: break;
				case CilElementType.R8: break;
				case CilElementType.I: break;
				case CilElementType.U: break;
				case CilElementType.Ptr: break;
				default: throw new InvalidCompilerException();
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
			if ((context.InvokeMethod.Attributes & MethodAttributes.PInvokeImpl) != MethodAttributes.PInvokeImpl)
				return false;

			string external = context.InvokeMethod.Module.GetExternalName(context.InvokeMethod.Token);

			//TODO: Verify!

			Type intrinsicType = Type.GetType(external);

			if (intrinsicType == null)
				return false;

			var instance = Activator.CreateInstance(intrinsicType);

			var instanceMethod = instance as IIntrinsicInternalMethod;
			if (instanceMethod != null)
			{
				instanceMethod.ReplaceIntrinsicCall(context, methodCompiler);
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
		private void ProcessInvokeInstruction(Context context, RuntimeMethod method, Operand resultOperand, List<Operand> operands)
		{
			Operand symbolOperand = Operand.CreateSymbolFromMethod(method);
			ProcessInvokeInstruction(context, method, symbolOperand, resultOperand, operands);
		}

		/// <summary>
		/// Processes a method call instruction.
		/// </summary>
		/// <param name="context">The transformation context.</param>
		/// <param name="method">The method.</param>
		/// <param name="resultOperand">The result operand.</param>
		/// <param name="operands">The operands.</param>
		private void ProcessInvokeInstruction(Context context, RuntimeMethod method, Operand symbolOperand, Operand resultOperand, List<Operand> operands)
		{
			Debug.Assert(method != null);

			context.SetInstruction(IRInstruction.Call, (byte)(operands.Count + 1), (byte)(resultOperand == null ? 0 : 1));
			context.InvokeMethod = method;

			if (resultOperand != null)
			{
				context.SetResult(resultOperand);
			}

			int index = 0;
			context.SetOperand(index++, symbolOperand);
			foreach (Operand op in operands)
			{
				context.SetOperand(index++, op);
			}
		}

		private bool CanSkipDueToRecursiveSystemObjectCtorCall(Context context)
		{
			RuntimeMethod currentMethod = methodCompiler.Method;
			RuntimeMethod invokeTarget = context.InvokeMethod;

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
			Operand source = context.Operand1;
			Operand destination = context.Result;

			BaseInstruction extension = null;
			SigType extendedType = null;

			if (IsSignExtending(source))
			{
				extension = IRInstruction.SignExtendedMove;
				extendedType = BuiltInSigType.Int32;
			}
			else if (IsZeroExtending(source))
			{
				extension = IRInstruction.ZeroExtendedMove;
				extendedType = BuiltInSigType.UInt32;
			}

			if (extension != null)
			{
				context.SetInstruction(extension, destination, source);
				return;
			}

			context.ReplaceInstructionOnly(IRInstruction.Move);
		}

		/// <summary>
		/// Replaces the IL store instruction by an appropriate IR move  instruction.
		/// </summary>
		/// <param name="context">Provides the transformation context.</param>
		private void ProcessStoreInstruction(Context context)
		{
			context.SetInstruction(IRInstruction.Move, context.Result, context.Operand1);
		}

		/// <summary>
		/// Replaces the instruction with an internal call.
		/// </summary>
		/// <param name="context">The transformation context.</param>
		/// <param name="internalCallTarget">The internal call target.</param>
		private void ReplaceWithVmCall(Context context, VmCall internalCallTarget)
		{
			string runtime = "Mosa.Platform.Internal." + methodCompiler.Architecture.PlatformName + ".Runtime";

			RuntimeType runtimeType = typeSystem.GetType(runtime);
			Debug.Assert(runtimeType != null, "Cannot find " + runtime);

			RuntimeMethod method = runtimeType.FindMethod(internalCallTarget.ToString());
			Debug.Assert(method != null, "Cannot find method: " + internalCallTarget.ToString());

			context.ReplaceInstructionOnly(IRInstruction.Call);
			context.SetOperand(0, Operand.CreateSymbolFromMethod(method));
			context.OperandCount = 1;
			context.InvokeMethod = method;
		}

		private bool ReplaceWithInternalCall(Context context)
		{
			var method = context.InvokeMethod;

			bool internalCall = ((method.ImplAttributes & MethodImplAttributes.InternalCall) == MethodImplAttributes.InternalCall);

			if (internalCall)
			{
				string replacementMethod = this.BuildInternalCallName(method);

				method = method.DeclaringType.FindMethod(replacementMethod);

				Operand result = context.Result;
				List<Operand> operands = new List<Operand>(context.Operands);

				ProcessInvokeInstruction(context, method, result, operands);
			}

			return internalCall;
		}

		private string BuildInternalCallName(RuntimeMethod method)
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

		private static VmCall? TypeToVmBoxCall(RuntimeType type)
		{
			Debug.Assert(type.IsValueType);

			switch (type.FullName)
			{
				case "System.Char": return VmCall.BoxChar;
				case "System.Boolean": return VmCall.BoxChar;
				case "System.SByte": return VmCall.BoxInt8;
				case "System.Byte": return VmCall.BoxUInt8;
				case "System.Int16": return VmCall.BoxInt16;
				case "System.UInt16": return VmCall.BoxUInt16;
				case "System.Int32": return VmCall.BoxInt32;
				case "System.UInt32": return VmCall.BoxUInt32;
				case "System.Int64": return VmCall.BoxInt64;
				case "System.UInt64": return VmCall.BoxUInt64;
				case "System.Single": return VmCall.BoxSingle;
				case "System.Double": return VmCall.BoxDouble;
				default: return null;
			}
		}

		private static VmCall TypeToVmUnboxCall(RuntimeType type)
		{
			Debug.Assert(type.IsValueType);

			switch (type.FullName)
			{
				case "System.Char": return VmCall.UnboxChar;
				case "System.Boolean": return VmCall.UnboxChar;
				case "System.SByte": return VmCall.UnboxInt8;
				case "System.Byte": return VmCall.UnboxUInt8;
				case "System.Int16": return VmCall.UnboxInt16;
				case "System.UInt16": return VmCall.UnboxUInt16;
				case "System.Int32": return VmCall.UnboxInt32;
				case "System.UInt32": return VmCall.UnboxUInt32;
				case "System.Int64": return VmCall.UnboxInt64;
				case "System.UInt64": return VmCall.UnboxUInt64;
				case "System.Single": return VmCall.UnboxSingle;
				case "System.Double": return VmCall.UnboxDouble;
				default: throw new InvalidProgramException();
			}
		}

		#endregion Internals
	}
}