/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.CIL;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using Mosa.Compiler.TypeSystem.Generic;

namespace Mosa.Compiler.Framework.IR
{
	/// <summary>
	/// Transforms CIL instructions into their appropriate IR.
	/// </summary>
	/// <remarks>
	/// This transformation stage transforms CIL instructions into their equivalent IR sequences.
	/// </remarks>
	public sealed class CILTransformationStage : BaseCodeTransformationStage, CIL.ICILVisitor, IPipelineStage
	{

		#region Data members

		#endregion // Data members

		#region IMethodCompilerStage Members

		#endregion // IMethodCompilerStage Members

		#region ICILVisitor

		/// <summary>
		/// Visitation function for Ldarg instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Ldarg(Context context)
		{
			ProcessLoadInstruction(context);
		}

		/// <summary>
		/// Visitation function for Ldarga instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Ldarga(Context context)
		{
			context.ReplaceInstructionOnly(IRInstruction.AddressOf);
		}

		/// <summary>
		/// Visitation function for Ldloc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Ldloc(Context context)
		{
			if (context.Ignore)
			{
				context.Remove();
			}
			else
			{
				ProcessLoadInstruction(context);
			}
		}

		/// <summary>
		/// Visitation function for Ldloca instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Ldloca(Context context)
		{
			context.ReplaceInstructionOnly(IRInstruction.AddressOf);
		}

		/// <summary>
		/// Visitation function for Ldc instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Ldc(Context context)
		{
			ProcessLoadInstruction(context);
		}

		/// <summary>
		/// Visitation function for Ldobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Ldobj(Context context)
		{
			IInstruction loadInstruction = IRInstruction.Load;
			Operand destination = context.Result;
			Operand source = context.Operand1;

			SigType elementType = context.Other as SigType;

			if (elementType == null)
			{
				CIL.LdobjInstruction instruction = (CIL.LdobjInstruction)context.Instruction;
				elementType = instruction.TypeReference;
			}

			// This is actually ldind.* and ldobj - the opcodes have the same meanings
			if (MustSignExtendOnLoad(elementType.Type))
			{
				loadInstruction = IRInstruction.SignExtendedMove;
			}
			else if (MustZeroExtendOnLoad(elementType.Type))
			{
				loadInstruction = IRInstruction.ZeroExtendedMove;
			}

			context.SetInstruction(loadInstruction, destination, source, ConstantOperand.FromValue(0));
			context.Other = elementType;
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
		public void Ldsfld(Context context)
		{
			SigType sigType = context.RuntimeField.SignatureType;
			Operand source = new MemberOperand(context.RuntimeField);
			Operand destination = context.Result;

			IInstruction loadInstruction = IRInstruction.Move;
			if (MustSignExtendOnLoad(sigType.Type))
			{
				loadInstruction = IRInstruction.SignExtendedMove;
			}
			else if (MustZeroExtendOnLoad(sigType.Type))
			{
				loadInstruction = IRInstruction.ZeroExtendedMove;
			}

			context.SetInstruction(loadInstruction, destination, source);
		}

		/// <summary>
		/// Visitation function for Ldsflda instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Ldsflda(Context context)
		{
			context.SetInstruction(IRInstruction.AddressOf, context.Result, new MemberOperand(context.RuntimeField));
			context.SetInstruction(IRInstruction.Move, context.Result, context.Operand1);
		}

		/// <summary>
		/// Visitation function for Ldftn instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldftn(Context context)
		{
			context.SetInstruction(IRInstruction.Move, context.Result, SymbolOperand.FromMethod(context.InvokeTarget));
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
		public void Stloc(Context context)
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
			context.SetInstruction(IRInstruction.Store, context.Operand1, context.Operand1, ConstantOperand.FromValue(0), context.Operand2);
		}

		/// <summary>
		/// Visitation function for Stsfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Stsfld(Context context)
		{
			context.SetInstruction(IRInstruction.Move, new MemberOperand(context.RuntimeField), context.Operand1);
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

			// Create a symbol operand for the invocation target
			RuntimeMethod invokeTarget = context.InvokeTarget;
			SymbolOperand symbolOperand = SymbolOperand.FromMethod(invokeTarget);

			ProcessInvokeInstruction(context, symbolOperand, context.Result, new List<Operand>(context.Operands));
		}

		/// <summary>
		/// Visitation function for Calli instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Calli(Context context)
		{
			Operand destinationOperand = context.GetOperand(context.OperandCount - 1);
			context.OperandCount -= 1;

			ProcessInvokeInstruction(context, destinationOperand, context.Result, new List<Operand>(context.Operands));
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
				var operand = new MemberOperand(type.Fields[0], type.Fields[0].SignatureType, new IntPtr(0));
				context.SetOperand(0, operand);
			}

			if (context.Operand2.Type is ValueTypeSigType)
			{
				var type = methodCompiler.Method.Module.GetType((context.Operand2.Type as ValueTypeSigType).Token);
				var operand = new MemberOperand(type.Fields[0], type.Fields[0].SignatureType, new IntPtr(0));
				context.SetOperand(1, operand);
			}

			switch ((context.Instruction as CIL.BaseCILInstruction).OpCode)
			{
				case CIL.OpCode.And:
					context.SetInstruction(IRInstruction.LogicalAnd, context.Result, context.Operand1, context.Operand2);
					break;
				case CIL.OpCode.Or:
					context.SetInstruction(IRInstruction.LogicalOr, context.Result, context.Operand1, context.Operand2);
					break;
				case CIL.OpCode.Xor:
					context.SetInstruction(IRInstruction.LogicalXor, context.Result, context.Operand1, context.Operand2);
					break;
				case CIL.OpCode.Div_un:
					context.SetInstruction(IRInstruction.DivU, context.Result, context.Operand1, context.Operand2);
					break;
				case CIL.OpCode.Rem_un:
					context.SetInstruction(IRInstruction.RemU, context.Result, context.Operand1, context.Operand2);
					break;
				default:
					throw new NotSupportedException();
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
				case CIL.OpCode.Shl:
					context.SetInstruction(IRInstruction.ShiftLeft, context.Result, context.Operand1, context.Operand2);
					break;
				case CIL.OpCode.Shr:
					context.SetInstruction(IRInstruction.ArithmeticShiftRight, context.Result, context.Operand1, context.Operand2);
					break;
				case CIL.OpCode.Shr_un:
					context.SetInstruction(IRInstruction.ShiftRight, context.Result, context.Operand1, context.Operand2);
					break;
				default:
					throw new NotSupportedException();
			}
		}

		/// <summary>
		/// Visitation function for Neg instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Neg(Context context)
		{
			if (IsUnsigned(context.Operand1))
			{
				ConstantOperand zero = new ConstantOperand(context.Operand1.Type, 0UL);
				context.SetInstruction(IRInstruction.SubU, context.Result, zero, context.Operand1);
			}
			else if (context.Operand1.Type.Type == CilElementType.R4)
			{
				ConstantOperand minusOne = new ConstantOperand(context.Operand1.Type, -1.0f);
				context.SetInstruction(IRInstruction.MulF, context.Result, minusOne, context.Operand1);
			}
			else if (context.Operand1.Type.Type == CilElementType.R8)
			{
				ConstantOperand minusOne = new ConstantOperand(context.Operand1.Type, -1.0);
				context.SetInstruction(IRInstruction.MulF, context.Result, minusOne, context.Operand1);
			}
			else
			{
				ConstantOperand minusOne = new ConstantOperand(context.Operand1.Type, -1L);
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
			ProcessConversionInstruction(context);
		}

		/// <summary>
		/// Visitation function for Callvirt instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Callvirt(Context context)
		{
			RuntimeMethod invokeTarget = context.InvokeTarget;

			////if (invokeTarget.DeclaringType.BaseType != null && invokeTarget.DeclaringType.BaseType.FullName == "System.MulticastDelegate")
			//if (invokeTarget.DeclaringType.IsDelegate)
			//{
			//    typeSystem.DelegateTypePatcher.PatchType(invokeTarget.DeclaringType);

			//    //InternalTypeModule internalTypeModule = typeSystem.InternalTypeModule as InternalTypeModule;

			//    //internalTypeModule.AddType(invokeTarget.DeclaringType);
			//    //foreach (var method in invokeTarget.DeclaringType.Methods)
			//    //    internalTypeModule.AddMethod(method);
			//}

			Operand resultOperand = context.Result;
			var operands = new List<Operand>(context.Operands);

			if (context.Previous.Instruction is ConstrainedPrefixInstruction)
			{
				var type = context.Previous.Other as RuntimeType;

				foreach (var method in type.Methods)
				{
					if (method.Name == invokeTarget.Name)
					{
						if (method.Signature.Matches(invokeTarget.Signature))
						{
							invokeTarget = method;
							break;
						}
					}
				}

				context.Previous.ReplaceInstructionOnly(IRInstruction.Nop);

				SymbolOperand symbolOperand = SymbolOperand.FromMethod(invokeTarget);
				ProcessInvokeInstruction(context, symbolOperand, resultOperand, operands);

				return;
			}

			if ((invokeTarget.Attributes & MethodAttributes.Virtual) == MethodAttributes.Virtual)
			{
				Operand thisPtr = context.Operand1;

				Operand methodTable = methodCompiler.CreateVirtualRegister(BuiltInSigType.IntPtr);
				Operand methodPtr = methodCompiler.CreateVirtualRegister(BuiltInSigType.IntPtr);

				if (!invokeTarget.DeclaringType.IsInterface)
				{
					int methodTableOffset = CalculateMethodTableOffset(invokeTarget) + (nativePointerSize * 5);
					context.SetInstruction(IRInstruction.Load, methodTable, thisPtr, ConstantOperand.FromValue(0));
					context.AppendInstruction(IRInstruction.Load, methodPtr, methodTable, new ConstantOperand(BuiltInSigType.Int32, methodTableOffset));
				}
				else
				{
					int methodTableOffset = CalculateMethodTableOffset(invokeTarget);
					int slotOffset = CalculateInterfaceSlotOffset(invokeTarget);

					Operand interfaceSlotPtr = methodCompiler.CreateVirtualRegister(BuiltInSigType.IntPtr);
					Operand interfaceMethodTablePtr = methodCompiler.CreateVirtualRegister(BuiltInSigType.IntPtr);

					context.SetInstruction(IRInstruction.Load, methodTable, thisPtr, ConstantOperand.FromValue(0));
					context.AppendInstruction(IRInstruction.Load, interfaceSlotPtr, methodTable, ConstantOperand.FromValue(0));
					context.AppendInstruction(IRInstruction.Load, interfaceMethodTablePtr, interfaceSlotPtr, new ConstantOperand(BuiltInSigType.Int32, slotOffset));
					context.AppendInstruction(IRInstruction.Load, methodPtr, interfaceMethodTablePtr, new ConstantOperand(BuiltInSigType.Int32, methodTableOffset));
				}

				context.AppendInstruction(IRInstruction.Nop);
				ProcessInvokeInstruction(context, methodPtr, resultOperand, operands);
			}
			else
			{
				// FIXME: Callvirt imposes a null-check. For virtual calls this is done implicitly, but for non-virtual calls
				// we have to make this explicitly somehow.

				// Create a symbol operand for the invocation target
				SymbolOperand symbolOperand = SymbolOperand.FromMethod(invokeTarget);
				ProcessInvokeInstruction(context, symbolOperand, resultOperand, operands);
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
		public void Newarr(Context context)
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

			context.SetOperand(1, new ConstantOperand(BuiltInSigType.IntPtr, 0));
			context.SetOperand(2, new ConstantOperand(BuiltInSigType.Int32, elementSize));
			context.SetOperand(3, lengthOperand);
			context.OperandCount = 4;
		}

		/// <summary>
		/// Visitation function for Newobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Newobj(Context context)
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
					classType = new CilGenericType(classType.Module, classType.Token, classType, thisReference.Type as GenericInstSigType);
				classType = methodCompiler.AssemblyCompiler.GenericTypePatcher.PatchType(this.typeModule, methodCompiler.Method.DeclaringType as CilGenericType, classType as CilGenericType);
			}

			List<Operand> ctorOperands = new List<Operand>(context.Operands);
			RuntimeMethod ctorMethod = context.InvokeTarget;

			if (!ReplaceWithInternalCall(context, ctorMethod))
			{
				Context before = context.InsertBefore();
				before.SetInstruction(IRInstruction.Nop);

				ReplaceWithVmCall(before, VmCall.AllocateObject);

				SymbolOperand methodTableSymbol = GetMethodTableSymbol(classType);

				before.SetOperand(1, methodTableSymbol);
				before.SetOperand(2, new ConstantOperand(BuiltInSigType.Int32, typeLayout.GetTypeSize(classType)));
				before.OperandCount = 2;
				before.Result = thisReference;

				// Result is the this pointer, now invoke the real constructor
				SymbolOperand symbolOperand = SymbolOperand.FromMethod(ctorMethod);

				ctorOperands.Insert(0, thisReference);
				ProcessInvokeInstruction(context, symbolOperand, null, ctorOperands);
			}
		}

		private SymbolOperand GetMethodTableSymbol(RuntimeType runtimeType)
		{
			return new SymbolOperand(BuiltInSigType.IntPtr, runtimeType.FullName + @"$mtable");
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
			bool result = false;

			if (method.Parameters.Count == ctorOperands.Count)
			{
				result = true;

				for (int index = 0; result && index < ctorOperands.Count; index++)
				{
					result = ctorOperands[index].Type.Matches(method.Signature.Parameters[index]);
				}
			}

			return result;
		}


		/// <summary>
		/// Visitation function for Castclass instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Castclass(Context context)
		{
			// We don't need to check the result, if the icall fails, it'll happily throw
			// the InvalidCastException.
			context.ReplaceInstructionOnly(IRInstruction.Nop);
			//ReplaceWithVmCall(context, VmCall.Castclass);
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
			SymbolOperand methodTableSymbol = GetMethodTableSymbol(classType);

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

				context.SetOperand(1, new ConstantOperand(BuiltInSigType.UInt32, slot));
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
			throw new NotSupportedException();
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

			RuntimeType type;
			VmCall vmCall;

			if (context.Operand1.Type.Type == CilElementType.Char)
			{
				type = typeSystem.GetType("mscorlib", "System", "Char");
				vmCall = VmCall.BoxChar;
			}
			else if (context.Operand1.Type.Type == CilElementType.Boolean)
			{
				type = typeSystem.GetType("mscorlib", "System", "Boolean");
				vmCall = VmCall.BoxBool;
			}
			else if (context.Operand1.Type.Type == CilElementType.I1)
			{
				type = typeSystem.GetType("mscorlib", "System", "SByte");
				vmCall = VmCall.BoxInt8;
			}
			else if (context.Operand1.Type.Type == CilElementType.U1)
			{
				type = typeSystem.GetType("mscorlib", "System", "Byte");
				vmCall = VmCall.BoxUInt8;
			}
			else if (context.Operand1.Type.Type == CilElementType.I2)
			{
				type = typeSystem.GetType("mscorlib", "System", "Int16");
				vmCall = VmCall.BoxInt16;
			}
			else if (context.Operand1.Type.Type == CilElementType.U2)
			{
				type = typeSystem.GetType("mscorlib", "System", "UInt16");
				vmCall = VmCall.BoxUInt16;
			}
			else if (context.Operand1.Type.Type == CilElementType.I4)
			{
				type = typeSystem.GetType("mscorlib", "System", "Int32");
				vmCall = VmCall.BoxInt32;
			}
			else if (context.Operand1.Type.Type == CilElementType.U4)
			{
				type = typeSystem.GetType("mscorlib", "System", "UInt32");
				vmCall = VmCall.BoxUInt32;
			}
			else if (context.Operand1.Type.Type == CilElementType.I8)
			{
				type = typeSystem.GetType("mscorlib", "System", "Int64");
				vmCall = VmCall.BoxInt64;
			}
			else if (context.Operand1.Type.Type == CilElementType.U8)
			{
				type = typeSystem.GetType("mscorlib", "System", "UInt64");
				vmCall = VmCall.BoxUInt64;
			}
			else if (context.Operand1.Type.Type == CilElementType.R4)
			{
				type = typeSystem.GetType("mscorlib", "System", "Single");
				vmCall = VmCall.BoxSingle;
			}
			else if (context.Operand1.Type.Type == CilElementType.R8)
			{
				type = typeSystem.GetType("mscorlib", "System", "Double");
				vmCall = VmCall.BoxDouble;
			}
			else
			{
				context.ReplaceInstructionOnly(IRInstruction.Move);
				return;
			}

			context.SetInstruction(IRInstruction.Nop);
			ReplaceWithVmCall(context, vmCall);

			var methodTableSymbol = GetMethodTableSymbol(type);
			var classSize = typeLayout.GetTypeSize(type);

			context.SetOperand(1, methodTableSymbol);
			context.SetOperand(2, new ConstantOperand(BuiltInSigType.UInt32, classSize));
			context.SetOperand(3, value);
			context.OperandCount = 4;
			context.Result = result;
			return;
		}

		/// <summary>
		/// Visitation function for BinaryComparison instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void BinaryComparison(Context context)
		{
			IR.ConditionCode code = ConvertCondition((context.Instruction as CIL.BaseCILInstruction).OpCode);

			if (context.Operand1.StackType == StackTypeCode.F)
				context.SetInstruction(IRInstruction.FloatingPointCompare, context.Result, context.Operand1, context.Operand2);
			else
				context.SetInstruction(IRInstruction.IntegerCompare, context.Result, context.Operand1, context.Operand2);

			context.ConditionCode = code;
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

		#endregion // ICILVisitor

		#region ICILVisitor - Unused

		/// <summary>
		/// Visitation function for Break instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Break(Context context)
		{
			context.SetInstruction(IRInstruction.Break);
		}

		/// <summary>
		/// Visitation function for Ldstr instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Ldstr(Context context)
		{
			/*
			 * This requires a special memory layout for strings as they are interned by the compiler
			 * into the generated image. This won't work this way forever: As soon as we'll support
			 * a real AppDomain and real string interning, this code will have to go away and will
			 * be replaced by a proper VM call.
			 * 
			 */

			IAssemblyLinker linker = methodCompiler.Linker;
			IMetadataModule assembly = methodCompiler.Assembly;

			string referencedString = assembly.Metadata.ReadUserString(context.TokenType);

			string symbolName = @"$ldstr$" + assembly.Name + "$String" + context.TokenType.ToString("x");

			if (!linker.HasSymbol(symbolName))
			{
				// HACK: These strings should actually go into .rodata, but we can't link that right now.
				using (Stream stream = linker.Allocate(symbolName, SectionKind.Text, 0, nativePointerAlignment))
				{
					// Method table and sync block
					linker.Link(LinkType.AbsoluteAddress | LinkType.NativeI4, symbolName, 0, 0, @"System.String$mtable", IntPtr.Zero);
					stream.WriteZeroBytes(8);

					// String length field
					stream.Write(BitConverter.GetBytes(referencedString.Length), 0, nativePointerSize);

					// String data
					byte[] stringData = Encoding.Unicode.GetBytes(referencedString);
					Debug.Assert(stringData.Length == referencedString.Length * 2, @"Byte array of string data doesn't match expected string data length");
					stream.Write(stringData);
				}

			}

			Operand source = new SymbolOperand(BuiltInSigType.String, symbolName);
			Operand destination = context.Result;

			context.SetInstruction(IRInstruction.Move, destination, source);
		}

		/// <summary>
		/// Visitation function for Ldfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Ldfld(Context context)
		{
			Operand resultOperand = context.Result;
			Operand objectOperand = context.Operand1;
			Operand temp = methodCompiler.CreateVirtualRegister(context.RuntimeField.SignatureType);
			RuntimeField field = context.RuntimeField;

			int offset = typeLayout.GetFieldOffset(field);
			ConstantOperand offsetOperand = new ConstantOperand(BuiltInSigType.IntPtr, offset);

			IInstruction loadInstruction = IRInstruction.Load;
			if (MustSignExtendOnLoad(field.SignatureType.Type))
			{
				loadInstruction = IRInstruction.SignExtendedMove;
			}
			else if (MustZeroExtendOnLoad(field.SignatureType.Type))
			{
				loadInstruction = IRInstruction.ZeroExtendedMove;
			}

			context.SetInstruction(loadInstruction, temp, objectOperand, offsetOperand);
			context.AppendInstruction(IRInstruction.Move, resultOperand, temp);
		}

		/// <summary>
		/// Visitation function for Ldflda instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Ldflda(Context context)
		{
			Operand fieldAddress = context.Result;
			Operand objectOperand = context.Operand1;

			int offset = typeLayout.GetFieldOffset(context.RuntimeField);
			Operand fixedOffset = new ConstantOperand(BuiltInSigType.Int32, offset);

			context.SetInstruction(IRInstruction.AddU, fieldAddress, objectOperand, fixedOffset);
		}

		/// <summary>
		/// Visitation function for Stfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Stfld(Context context)
		{
			Operand objectOperand = context.Operand1;
			Operand valueOperand = context.Operand2;
			Operand temp = methodCompiler.CreateVirtualRegister(context.RuntimeField.SignatureType);

			int offset = typeLayout.GetFieldOffset(context.RuntimeField);
			ConstantOperand offsetOperand = new ConstantOperand(BuiltInSigType.IntPtr, offset);

			context.SetInstruction(IRInstruction.Move, temp, valueOperand);
			context.AppendInstruction(IRInstruction.Store, objectOperand, objectOperand, offsetOperand, temp);
		}

		/// <summary>
		/// Visitation function for Jmp instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Jmp(Context context)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Visitation function for Branch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Branch(Context context)
		{
			context.ReplaceInstructionOnly(IRInstruction.Jmp);
		}

		/// <summary>
		/// Visitation function for UnaryBranch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void UnaryBranch(Context context)
		{
			int target = context.BranchTargets[0];

			ConditionCode cc;
			Operand first = context.Operand1;
			Operand second = ConstantOperand.I4_0;

			CIL.OpCode opcode = ((CIL.ICILInstruction)context.Instruction).OpCode;
			if (opcode == CIL.OpCode.Brtrue || opcode == CIL.OpCode.Brtrue_s)
			{
				cc = ConditionCode.NotEqual;
			}
			else if (opcode == CIL.OpCode.Brfalse || opcode == CIL.OpCode.Brfalse_s)
			{
				cc = ConditionCode.Equal;
			}
			else
			{
				throw new NotSupportedException(@"CILTransformationStage.UnaryBranch doesn't support CIL opcode " + opcode);
			}

			context.SetInstruction(IRInstruction.IntegerCompareBranch, null, first, second);
			context.ConditionCode = cc;
			context.SetBranch(target);
		}

		/// <summary>
		/// Visitation function for BinaryBranch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void BinaryBranch(Context context)
		{
			int target = context.BranchTargets[0];

			ConditionCode cc = ConvertCondition(((CIL.ICILInstruction)context.Instruction).OpCode);
			Operand first = context.Operand1;
			Operand second = context.Operand2;

			if (first.StackType == StackTypeCode.F)
			{
				Operand comparisonResult = methodCompiler.CreateVirtualRegister(BuiltInSigType.Int32);
				context.SetInstruction(IRInstruction.FloatingPointCompare, comparisonResult, first, second);
				context.ConditionCode = cc;
				context.AppendInstruction(IRInstruction.IntegerCompareBranch, null, comparisonResult, new ConstantOperand(BuiltInSigType.IntPtr, 1));
				context.ConditionCode = ConditionCode.Equal;
				context.SetBranch(target);
			}
			else
			{
				context.SetInstruction(IRInstruction.IntegerCompareBranch, null, first, second);
				context.ConditionCode = cc;
				context.SetBranch(target);
			}
		}

		/// <summary>
		/// Visitation function for Switch instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Switch(Context context)
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
		public void Ldlen(Context context)
		{
			Operand arrayOperand = context.Operand1;
			Operand arrayLength = context.Result;
			ConstantOperand constantOffset = ConstantOperand.FromValue(8);

			Operand arrayAddress = methodCompiler.CreateVirtualRegister(new PtrSigType(BuiltInSigType.Int32));
			context.SetInstruction(IRInstruction.Move, arrayAddress, arrayOperand);
			context.AppendInstruction(IRInstruction.Load, arrayLength, arrayAddress, constantOffset);
		}

		/// <summary>
		/// Visitation function for Ldelema instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Ldelema(Context context)
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
			Operand elementSizeOperand = new ConstantOperand(BuiltInSigType.Int32, elementSizeInBytes);
			context.AppendInstruction(IRInstruction.MulS, elementOffset, arrayIndexOperand, elementSizeOperand);

			return elementOffset;
		}

		private Operand LoadArrayBaseAddress(Context context, SZArraySigType arraySignatureType, Operand arrayOperand)
		{
			Operand arrayAddress = methodCompiler.CreateVirtualRegister(new PtrSigType(arraySignatureType.ElementType));
			Operand fixedOffset = new ConstantOperand(BuiltInSigType.Int32, 12);
			context.SetInstruction(IRInstruction.AddS, arrayAddress, arrayOperand, fixedOffset);
			return arrayAddress;
		}

		/// <summary>
		/// Visitation function for Ldelem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Ldelem(Context context)
		{
			IInstruction loadInstruction = IRInstruction.Load;
			Operand result = context.Result;
			Operand arrayOperand = context.Operand1;
			Operand arrayIndexOperand = context.Operand2;

			SZArraySigType arraySigType = arrayOperand.Type as SZArraySigType;
			if (arraySigType == null)
			{
				throw new CompilationException(@"Array operation performed on non-array operand.");
			}

			if (MustSignExtendOnLoad(arraySigType.ElementType.Type))
			{
				loadInstruction = IRInstruction.SignExtendedMove;
			}
			else if (MustZeroExtendOnLoad(arraySigType.ElementType.Type))
			{
				loadInstruction = IRInstruction.ZeroExtendedMove;
			}

			Operand arrayAddress = LoadArrayBaseAddress(context, arraySigType, arrayOperand);
			Operand elementOffset = CalculateArrayElementOffset(context, arraySigType, arrayIndexOperand);
			context.AppendInstruction(loadInstruction, result, arrayAddress, elementOffset);
		}

		/// <summary>
		/// Visitation function for Stelem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Stelem(Context context)
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
			context.AppendInstruction(IRInstruction.Store, arrayAddress, arrayAddress, elementOffset, value);
		}

		/// <summary>
		/// Visitation function for UnboxAny instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.UnboxAny(Context context)
		{
			var value = context.Operand1;
			var type = context.Other as RuntimeType;
			var result = context.Result;

			if (type.FullName == "System.Boolean")
			{
				ReplaceWithVmCall(context, VmCall.UnboxBool);
			}
			else if (type.FullName == "System.Char")
			{
				ReplaceWithVmCall(context, VmCall.UnboxChar);
			}
			else if (type.FullName == "System.SByte")
			{
				ReplaceWithVmCall(context, VmCall.UnboxInt8);
			}
			else if (type.FullName == "System.Byte")
			{
				ReplaceWithVmCall(context, VmCall.UnboxUInt8);
			}
			else if (type.FullName == "System.Int16")
			{
				//ReplaceWithVmCall(context, VmCall.UnboxInt32); 
				ReplaceWithVmCall(context, VmCall.UnboxInt16);
			}
			else if (type.FullName == "System.UInt16")
			{
				ReplaceWithVmCall(context, VmCall.UnboxUInt32);
			}
			else if (type.FullName == "System.Int32")
			{
				ReplaceWithVmCall(context, VmCall.UnboxInt32);
			}
			else if (type.FullName == "System.UInt32")
			{
				ReplaceWithVmCall(context, VmCall.UnboxUInt32);
			}
			else if (type.FullName == "System.Int64")
			{
				ReplaceWithVmCall(context, VmCall.UnboxInt64);
			}
			else if (type.FullName == "System.UInt64")
			{
				ReplaceWithVmCall(context, VmCall.UnboxUInt64);
			}
			else if (type.FullName == "System.Single")
			{
				ReplaceWithVmCall(context, VmCall.UnboxSingle);
			}
			else if (type.FullName == "System.Double")
			{
				ReplaceWithVmCall(context, VmCall.UnboxDouble);
			}
			else
			{
				context.ReplaceInstructionOnly(IRInstruction.Move);
				return;
			}

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
			context.SetInstruction(IRInstruction.Return);
		}


		private ExceptionHandlingClause FindImmediateClause(Context context)
		{
			ExceptionHandlingClause innerClause = null;
			int label = context.Label;

			foreach (ExceptionHandlingClause clause in methodCompiler.ExceptionClauseHeader.Clauses)
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
		public void Add(Context context)
		{
			Replace(context, IRInstruction.AddF, IRInstruction.AddS, IRInstruction.AddU);
		}

		/// <summary>
		/// Visitation function for Sub instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Sub(Context context)
		{
			Replace(context, IRInstruction.SubF, IRInstruction.SubS, IRInstruction.SubU);
		}
		/// <summary>
		/// Visitation function for Mul instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Mul(Context context)
		{
			Replace(context, IRInstruction.MulF, IRInstruction.MulS, IRInstruction.MulU);
		}

		/// <summary>
		/// Visitation function for Div instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Div(Context context)
		{
			Replace(context, IRInstruction.DivF, IRInstruction.DivS, IRInstruction.DivU);
		}

		/// <summary>
		/// Visitation function for Rem instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		public void Rem(Context context)
		{
			Replace(context, IRInstruction.RemF, IRInstruction.RemS, IRInstruction.RemU);
		}

		#endregion // ICILVisitor

		#region Internals

		private static void Replace(Context context, IInstruction floatingPointInstruction, IInstruction signedInstruction, IInstruction unsignedInstruction)
		{
			if (IsFloatingPoint(context))
			{
				context.ReplaceInstructionOnly(floatingPointInstruction);
			}
			else if (IsUnsigned(context))
			{
				context.ReplaceInstructionOnly(unsignedInstruction);
			}
			else
			{
				context.ReplaceInstructionOnly(signedInstruction);
			}
		}

		private static bool IsUnsigned(Context context)
		{
			Operand operand = context.Result;
			return IsUnsigned(operand);
		}

		private static bool IsUnsigned(Operand operand)
		{
			CilElementType type = operand.Type.Type;
			return type == CilElementType.U
				   || type == CilElementType.U1
				   || type == CilElementType.U2
				   || type == CilElementType.U4
				   || type == CilElementType.U8;
		}

		private static bool IsFloatingPoint(Context context)
		{
			return context.Result.StackType == StackTypeCode.F;
		}

		/// <summary>
		/// Determines if a store is silently truncating the value.
		/// </summary>
		/// <param name="dest">The destination operand.</param>
		/// <param name="source">The source operand.</param>
		/// <returns>True if the store is truncating, otherwise false.</returns>
		private static bool IsTruncating(Operand dest, Operand source)
		{
			CilElementType cetDest = dest.Type.Type;
			CilElementType cetSource = source.Type.Type;

			if (cetDest == CilElementType.I4 || cetDest == CilElementType.U4)
			{
				return (cetSource == CilElementType.I8 || cetSource == CilElementType.U8);
			}
			if (cetDest == CilElementType.I2 || cetDest == CilElementType.U2 || cetDest == CilElementType.Char)
			{
				return (cetSource == CilElementType.I8 || cetSource == CilElementType.U8 || cetSource == CilElementType.I4 || cetSource == CilElementType.U4);
			}
			if (cetDest == CilElementType.I1 || cetDest == CilElementType.U1)
			{
				return (cetSource == CilElementType.I8 || cetSource == CilElementType.U8 || cetSource == CilElementType.I4 || cetSource == CilElementType.U4 || cetSource == CilElementType.I2 || cetSource == CilElementType.U2 || cetSource == CilElementType.U2);
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
		/// Determines the implicit result type of the load  instruction.
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
				case CilElementType.I2:
					result = BuiltInSigType.Int32;
					break;

				case CilElementType.U1: goto case CilElementType.U2;
				case CilElementType.U2:
					result = BuiltInSigType.UInt32;
					break;

				case CilElementType.Char: goto case CilElementType.U2;
				case CilElementType.Boolean: goto case CilElementType.U2;


				default:
					result = sigType;
					break;
			}

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		private enum ConvType
		{
			I1 = 0,
			I2 = 1,
			I4 = 2,
			I8 = 3,
			U1 = 4,
			U2 = 5,
			U4 = 6,
			U8 = 7,
			R4 = 8,
			R8 = 9,
			I = 10,
			U = 11,
			Ptr = 12
		}

		/// <summary>
		/// Converts a <see cref="CilElementType"/> to <see cref="ConvType"/>
		/// </summary>
		/// <param name="cet">The CilElementType to convert.</param>
		/// <returns>The equivalent ConvType.</returns>
		/// <exception cref="T:System.NotSupportedException"><paramref name="cet"/> can't be converted.</exception>
		private ConvType ConvTypeFromCilType(CilElementType cet)
		{
			switch (cet)
			{
				case CilElementType.Char: return ConvType.U2;
				case CilElementType.I1: return ConvType.I1;
				case CilElementType.I2: return ConvType.I2;
				case CilElementType.I4: return ConvType.I4;
				case CilElementType.I8: return ConvType.I8;
				case CilElementType.U1: return ConvType.U1;
				case CilElementType.U2: return ConvType.U2;
				case CilElementType.U4: return ConvType.U4;
				case CilElementType.U8: return ConvType.U8;
				case CilElementType.R4: return ConvType.R4;
				case CilElementType.R8: return ConvType.R8;
				case CilElementType.I: return ConvType.I;
				case CilElementType.U: return ConvType.U;
				case CilElementType.Ptr: return ConvType.Ptr;
				case CilElementType.ByRef: return ConvType.Ptr;
			}

			// Requested CilElementType is not supported
			throw new NotSupportedException();
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

		/// <summary>
		/// Selects the appropriate IR conversion  instruction.
		/// </summary>
		/// <param name="context">The transformation context.</param>
		private void ProcessConversionInstruction(Context context)
		{
			Operand dest = context.Result;
			Operand src = context.Operand1;

			CheckAndConvertInstruction(context, dest, src);
		}

		private void CheckAndConvertInstruction(Context context, Operand destinationOperand, Operand sourceOperand)
		{
			ConvType ctDest = ConvTypeFromCilType(destinationOperand.Type.Type);
			ConvType ctSrc = ConvTypeFromCilType(sourceOperand.Type.Type);

			BaseIRInstruction type = s_convTable[(int)ctDest][(int)ctSrc];
			if (type == null)
				throw new NotSupportedException();

			uint mask = 0xFFFFFFFF;
			IInstruction instruction = ComputeExtensionTypeAndMask(ctDest, ref mask);

			if (type == IRInstruction.LogicalAnd && mask != 0)
			{
				if (sourceOperand.Type.Type == CilElementType.I8 || sourceOperand.Type.Type == CilElementType.U8)
				{
					context.SetInstruction(IRInstruction.Move, destinationOperand, sourceOperand);
					context.AppendInstruction(type, destinationOperand, sourceOperand, new ConstantOperand(BuiltInSigType.UInt32, mask));
				}
				else
				{
					context.SetInstruction(type, destinationOperand, sourceOperand, new ConstantOperand(BuiltInSigType.UInt32, mask));
				}
			}
			else
			{
				context.SetInstruction(type, destinationOperand, sourceOperand);
			}
		}

		private IInstruction ComputeExtensionTypeAndMask(ConvType destinationType, ref uint mask)
		{
			switch (destinationType)
			{
				case ConvType.I1:
					mask = 0xFF;
					return IRInstruction.SignExtendedMove;
				case ConvType.I2:
					mask = 0xFFFF;
					return IRInstruction.SignExtendedMove;
				case ConvType.I4:
					mask = 0xFFFFFFFF;
					break;
				case ConvType.I8:
					mask = 0x0;
					break;
				case ConvType.U1:
					mask = 0xFF;
					return IRInstruction.ZeroExtendedMove;
				case ConvType.U2:
					mask = 0xFFFF;
					return IRInstruction.ZeroExtendedMove;
				case ConvType.U4:
					mask = 0xFFFFFFFF;
					break;
				case ConvType.U8:
					mask = 0x0;
					break;
				case ConvType.R4:
					break;
				case ConvType.R8:
					break;
				case ConvType.I:
					break;
				case ConvType.U:
					break;
				case ConvType.Ptr:
					break;
				default:
					Debug.Assert(false);
					throw new NotSupportedException();
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
			if ((context.InvokeTarget.Attributes & MethodAttributes.PInvokeImpl) != MethodAttributes.PInvokeImpl)
				return false;

			string external = context.InvokeTarget.Module.GetExternalName(context.InvokeTarget.Token);

			//TODO: Verify!

			Type intrinsicType = Type.GetType(external);

			if (intrinsicType == null)
				return false;

			var instance = Activator.CreateInstance(intrinsicType);

			if (instance is IIntrinsicMethod)
			{
				(instance as IIntrinsicMethod).ReplaceIntrinsicCall(context, typeSystem, methodCompiler.Method.Parameters);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Processes a method call instruction.
		/// </summary>
		/// <param name="context">The transformation context.</param>
		/// <param name="destinationOperand">The operand, which holds the call destination.</param>
		/// <param name="resultOperand"></param>
		/// <param name="operands"></param>
		private void ProcessInvokeInstruction(Context context, Operand destinationOperand, Operand resultOperand, List<Operand> operands)
		{
			context.SetInstruction(IRInstruction.Call, (byte)(operands.Count + 1), (byte)(resultOperand == null ? 0 : 1));

			if (resultOperand != null)
				context.SetResult(resultOperand);

			int operandIndex = 0;
			context.SetOperand(operandIndex++, destinationOperand);
			foreach (Operand op in operands)
			{
				context.SetOperand(operandIndex++, op);
			}
		}

		private bool CanSkipDueToRecursiveSystemObjectCtorCall(Context context)
		{
			RuntimeMethod currentMethod = methodCompiler.Method;
			RuntimeMethod invokeTarget = context.InvokeTarget;

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

			// Is this a sign or zero-extending move?
			if (source != null)
			{
				IInstruction extension = null;
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
					Operand temp = methodCompiler.CreateVirtualRegister(extendedType);
					destination.Replace(temp, context.InstructionSet);
					context.SetInstruction(extension, temp, source);
					return;
				}
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
			RuntimeType type = typeSystem.GetType(@"Mosa.Internal.Runtime");
			Debug.Assert(type != null, "Cannot find Mosa.Internal.Runtime");

			RuntimeMethod method = type.FindMethod(internalCallTarget.ToString());
			Debug.Assert(method != null, "Cannot find method: " + internalCallTarget.ToString());

			context.ReplaceInstructionOnly(IRInstruction.Call);
			context.SetOperand(0, SymbolOperand.FromMethod(method));
			context.OperandCount = 1;
		}

		private bool ReplaceWithInternalCall(Context context, RuntimeMethod method)
		{
			bool internalCall = ((method.ImplAttributes & MethodImplAttributes.InternalCall) == MethodImplAttributes.InternalCall);

			if (internalCall)
			{
				string replacementMethod = this.BuildInternalCallName(method);

				method = method.DeclaringType.FindMethod(replacementMethod);
				context.InvokeTarget = method;

				Operand result = context.Result;
				List<Operand> operands = new List<Operand>(context.Operands);

				ProcessInvokeInstruction(context, SymbolOperand.FromMethod(method), result, operands);
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

		#endregion // Internals
	}
}
