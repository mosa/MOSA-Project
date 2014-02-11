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
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.MosaTypeSystem;
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

			var type = context.MosaType;

			// This is actually ldind.* and ldobj - the opcodes have the same meanings

			BaseInstruction loadInstruction = IRInstruction.Load;
			if (MustSignExtendOnLoad(type))
			{
				loadInstruction = IRInstruction.LoadSignExtended;
			}
			else if (MustZeroExtendOnLoad(type))
			{
				loadInstruction = IRInstruction.LoadZeroExtended;
			}

			context.SetInstruction(loadInstruction, destination, source, Operand.CreateConstantSignedInt(typeSystem, 0));
			context.MosaType = type;
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
			MosaType type = context.MosaField.Type;
			Operand source = Operand.CreateField(context.MosaField);
			Operand destination = context.Result;

			if (MustSignExtendOnLoad(type))
			{
				context.SetInstruction(IRInstruction.SignExtendedMove, destination, source);
			}
			else if (MustZeroExtendOnLoad(type))
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
			context.SetInstruction(IRInstruction.AddressOf, context.Result, Operand.CreateField(context.MosaField));
		}

		/// <summary>
		/// Visitation function for Ldftn instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldftn(Context context)
		{
			context.SetInstruction(IRInstruction.Move, context.Result, Operand.CreateSymbolFromMethod(typeSystem, context.InvokeMethod));
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
			context.SetInstruction(IRInstruction.Store, null, context.Operand1, Operand.CreateConstantSignedInt(methodCompiler.TypeSystem, 0), context.Operand2);
		}

		/// <summary>
		/// Visitation function for Stsfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Stsfld(Context context)
		{
			context.SetInstruction(IRInstruction.Move, Operand.CreateField(context.MosaField), context.Operand1);
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
			if (context.Operand1.IsValueType)
			{
				var type = context.Operand1.Type;
				var operand = Operand.CreateField(type.Fields[0]);
				context.SetOperand(0, operand);
			}

			if (context.Operand2.IsValueType)
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
				Operand minusOne = Operand.CreateConstantSingle(typeSystem, -1.0f);
				context.SetInstruction(IRInstruction.MulF, context.Result, minusOne, context.Operand1);
			}
			else if (context.Operand1.IsDouble)
			{
				Operand minusOne = Operand.CreateConstantDouble(typeSystem, -1.0d);
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
			MosaMethod method = context.InvokeMethod;
			Operand resultOperand = context.Result;
			List<Operand> operands = new List<Operand>(context.Operands);

			if (context.Previous.Instruction is ConstrainedPrefixInstruction)
			{
				var type = context.Previous.MosaType;

				//foreach (var m in type.Methods)
				//{
				//	if (m.Name == method.Name)
				//	{
				//		if (m.Matches(method))
				//		{
				//			method = m;
				//			break;
				//		}
				//	}
				//}

				context.Previous.Remove();

				ProcessInvokeInstruction(context, method, resultOperand, operands);

				return;
			}

			if (method.IsVirtual)
			{
				Operand thisPtr = context.Operand1;

				Operand methodTable = methodCompiler.CreateVirtualRegister(typeSystem.BuiltIn.Ptr);
				Operand methodPtr = methodCompiler.CreateVirtualRegister(typeSystem.BuiltIn.Ptr);

				if (!method.DeclaringType.IsInterface)
				{
					int methodTableOffset = CalculateMethodTableOffset(method) + (nativePointerSize * 5);
					context.SetInstruction(IRInstruction.Load, methodTable, thisPtr, Operand.CreateConstantSignedInt(typeSystem, 0));
					context.AppendInstruction(IRInstruction.Load, methodPtr, methodTable, Operand.CreateConstantSignedInt(typeSystem, (int)methodTableOffset));
				}
				else
				{
					int methodTableOffset = CalculateMethodTableOffset(method);
					int slotOffset = CalculateInterfaceSlotOffset(method);

					Operand interfaceSlotPtr = methodCompiler.CreateVirtualRegister(typeSystem.BuiltIn.Ptr);
					Operand interfaceMethodTablePtr = methodCompiler.CreateVirtualRegister(typeSystem.BuiltIn.Ptr);

					context.SetInstruction(IRInstruction.Load, methodTable, thisPtr, Operand.CreateConstantSignedInt(typeSystem, 0));
					context.AppendInstruction(IRInstruction.Load, interfaceSlotPtr, methodTable, Operand.CreateConstantSignedInt(typeSystem, 0));
					context.AppendInstruction(IRInstruction.Load, interfaceMethodTablePtr, interfaceSlotPtr, Operand.CreateConstantSignedInt(typeSystem, (int)slotOffset));
					context.AppendInstruction(IRInstruction.Load, methodPtr, interfaceMethodTablePtr, Operand.CreateConstantSignedInt(typeSystem, (int)methodTableOffset));
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

		private int CalculateMethodTableOffset(MosaMethod invokeTarget)
		{
			int slot = typeLayout.GetMethodTableOffset(invokeTarget);

			return (nativePointerSize * slot);
		}

		private int CalculateInterfaceSlotOffset(MosaMethod invokeTarget)
		{
			return CalculateInterfaceSlot(invokeTarget.DeclaringType) * nativePointerSize;
		}

		private int CalculateInterfaceSlot(MosaType interaceType)
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

			MosaType arrayType = context.Result.Type;
			int elementSize = 0;
			var elementType = arrayType.ElementType;

			if (elementType.IsBuiltInType)
			{
				// Handle builtin types
				var alignment = 0;
				architecture.GetTypeRequirements(elementType, out elementSize, out alignment);
			}
			else
			{
				// Handle classes and structs
				elementSize = typeLayout.GetTypeSize(elementType);
			}

			Operand lengthOperand = context.Operand1;

			// HACK: If we can't determine the size now, assume 16 bytes per array element.
			if (elementSize == 0)
			{
				elementSize = 16;
			}

			ReplaceWithVmCall(context, VmCall.AllocateArray);

			context.SetOperand(1, Operand.CreateConstantSignedInt(typeSystem, (int)0));
			context.SetOperand(2, Operand.CreateConstantSignedInt(typeSystem, (int)elementSize));
			context.SetOperand(3, lengthOperand);
			context.OperandCount = 4;
		}

		/// <summary>
		/// Visitation function for Newobj instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Newobj(Context context)
		{
			if (ReplaceWithInternalCall(context))
				return;

			var classType = context.InvokeMethod.DeclaringType;
			var thisReference = context.Result;

			Context before = context.InsertBefore();

			ReplaceWithVmCall(before, VmCall.AllocateObject);

			Operand methodTableSymbol = GetMethodTableSymbol(classType);

			before.SetOperand(1, methodTableSymbol);
			before.SetOperand(2, Operand.CreateConstantSignedInt(typeSystem, (int)typeLayout.GetTypeSize(classType)));
			before.OperandCount = 3;
			before.Result = thisReference;

			// Result is the this pointer, now invoke the real constructor
			List<Operand> operands = new List<Operand>(context.Operands);
			operands.Insert(0, thisReference);

			ProcessInvokeInstruction(context, context.InvokeMethod, null, operands);
		}

		private Operand GetMethodTableSymbol(MosaType runtimeType)
		{
			return Operand.CreateUnmanagedSymbolPointer(typeSystem, runtimeType.FullName + "$mtable");
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

			MosaType classType = result.Type;
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

				context.SetOperand(1, Operand.CreateConstantUnsignedInt(typeSystem, (uint)slot));
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
			var type = context.MosaType;

			//FIXME!

			// this type resolution is a hack
			if (type == null)
			{
				type = result.Type;
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
			context.SetOperand(2, Operand.CreateConstantUnsignedInt(typeSystem, (uint)classSize));
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

			if (context.Operand1.IsFloatingPoint)
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
			string symbolName = context.Operand1.Name;
			string stringdata = context.Operand1.StringData;

			context.SetInstruction(IRInstruction.Move, context.Result, context.Operand1);

			if (linker.GetSymbol(symbolName) != null)
				return;

			using (Stream stream = linker.Allocate(symbolName, SectionKind.ROData, 0, nativePointerAlignment))
			{
				// Method table and sync block
				linker.Link(LinkType.AbsoluteAddress | LinkType.I4, BuiltInPatch.I4, symbolName, 0, 0, @"System.String$mtable", 0);
				stream.WriteZeroBytes(8);

				// String length field
				stream.Write(BitConverter.GetBytes(stringdata.Length), 0, nativePointerSize);

				// String data
				byte[] stringData = Encoding.Unicode.GetBytes(stringdata);
				Debug.Assert(stringData.Length == stringdata.Length * 2, @"Byte array of string data doesn't match expected string data length");
				stream.Write(stringData);
			}
		}

		/// <summary>
		/// Visitation function for Ldfld instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.Ldfld(Context context)
		{
			Operand resultOperand = context.Result;
			Operand objectOperand = context.Operand1;
			MosaField field = context.MosaField;

			int offset = typeLayout.GetFieldOffset(field);
			Operand offsetOperand = Operand.CreateConstantSignedInt(typeSystem, offset);

			Operand result = methodCompiler.CreateVirtualRegister(field.Type);

			BaseInstruction loadInstruction = IRInstruction.Load;
			if (MustSignExtendOnLoad(field.Type))
			{
				loadInstruction = IRInstruction.LoadSignExtended;
			}
			else if (MustZeroExtendOnLoad(field.Type))
			{
				loadInstruction = IRInstruction.LoadZeroExtended;
			}

			Debug.Assert(offsetOperand != null);

			context.SetInstruction(loadInstruction, result, objectOperand, offsetOperand);
			context.MosaType = field.Type;
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

			int offset = typeLayout.GetFieldOffset(context.MosaField);
			Operand fixedOffset = Operand.CreateConstantSignedInt(typeSystem, (int)offset);

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
			Operand temp = methodCompiler.CreateVirtualRegister(context.MosaField.Type);

			int offset = typeLayout.GetFieldOffset(context.MosaField);
			Operand offsetOperand = Operand.CreateConstantSignedInt(typeSystem, offset);

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
			Operand second = Operand.CreateConstantSignedInt(typeSystem, (int)0);

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

			if (first.IsFloatingPoint)
			{
				Operand comparisonResult = methodCompiler.CreateVirtualRegister(typeSystem.BuiltIn.I4);
				context.SetInstruction(IRInstruction.FloatingPointCompare, cc, comparisonResult, first, second);
				context.AppendInstruction(IRInstruction.IntegerCompareBranch, ConditionCode.Equal, null, comparisonResult, Operand.CreateConstantSignedInt(typeSystem, 1));
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
			Operand constantOffset = Operand.CreateConstantSignedInt(typeSystem, 8);

			Operand arrayAddress = methodCompiler.CreateVirtualRegister(typeSystem.BuiltIn.I4);
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

			MosaType arrayType = arrayOperand.Type;

			Operand arrayAddress = LoadArrayBaseAddress(context, arrayType, arrayOperand);
			Operand elementOffset = CalculateArrayElementOffset(context, arrayType, arrayIndexOperand);
			context.AppendInstruction(IRInstruction.AddS, result, arrayAddress, elementOffset);
		}

		private Operand CalculateArrayElementOffset(Context context, MosaType arrayType, Operand arrayIndexOperand)
		{
			int elementSizeInBytes = 0, alignment = 0;
			architecture.GetTypeRequirements(arrayType.ElementType, out elementSizeInBytes, out alignment);

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

			Operand elementOffset = methodCompiler.CreateVirtualRegister(typeSystem.BuiltIn.I4);	// FIXME: I4
			Operand elementSizeOperand = Operand.CreateConstantSignedInt(typeSystem, (int)elementSizeInBytes);
			context.AppendInstruction(IRInstruction.MulS, elementOffset, arrayIndexOperand, elementSizeOperand);

			return elementOffset;
		}

		private Operand LoadArrayBaseAddress(Context context, MosaType arrayType, Operand arrayOperand)
		{
			var arrayPointer = typeSystem.Resolver.GetManagedPointerType(arrayType.ElementType);

			Operand arrayAddress = methodCompiler.CreateVirtualRegister(arrayPointer);
			Operand fixedOffset = Operand.CreateConstantSignedInt(typeSystem, (int)12);

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

			MosaType arraySigType = arrayOperand.Type;

			BaseInstruction loadInstruction = IRInstruction.Load;
			if (MustSignExtendOnLoad(arraySigType.ElementType))
			{
				loadInstruction = IRInstruction.LoadSignExtended;
			}
			else if (MustZeroExtendOnLoad(arraySigType.ElementType))
			{
				loadInstruction = IRInstruction.LoadZeroExtended;
			}

			Operand arrayAddress = LoadArrayBaseAddress(context, arraySigType, arrayOperand);
			Operand elementOffset = CalculateArrayElementOffset(context, arraySigType, arrayIndexOperand);

			Debug.Assert(elementOffset != null);

			context.AppendInstruction(loadInstruction, result, arrayAddress, elementOffset);
			context.MosaType = arraySigType.ElementType;
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
			MosaType arrayType = arrayOperand.Type;

			Operand arrayAddress = LoadArrayBaseAddress(context, arrayType, arrayOperand);
			Operand elementOffset = CalculateArrayElementOffset(context, arrayType, arrayIndexOperand);
			context.AppendInstruction(IRInstruction.Store, null, arrayAddress, elementOffset, value);
		}

		/// <summary>
		/// Visitation function for UnboxAny instruction.
		/// </summary>
		/// <param name="context">The context.</param>
		void CIL.ICILVisitor.UnboxAny(Context context)
		{
			var value = context.Operand1;
			var type = context.MosaType;
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

		private ExceptionBlock FindImmediateClause(Context context)
		{
			ExceptionBlock innerClause = null;

			int label = context.Label;

			foreach (var clause in methodCompiler.Method.ExceptionBlocks)
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
			ExceptionBlock clause = FindImmediateClause(context);

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
		/// <returns>
		/// True if the given operand should be loaded with its sign extended.
		/// </returns>
		private static bool MustSignExtendOnLoad(MosaType source)
		{
			return source.IsSignedByte || source.IsSignedShort;
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
			return source.IsUnsignedByte || source.IsUnsignedShort || source.IsChar;
		}

		/// <summary>
		/// Gets the index.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		private int GetIndex(MosaType type)
		{
			if (type.IsChar) return 5;
			else if (type.IsSignedByte) return 0;
			else if (type.IsSignedShort) return 1;
			else if (type.IsSignedInt) return 2;
			else if (type.IsSignedLong) return 3;
			else if (type.IsUnsignedByte) return 4;
			else if (type.IsUnsignedShort) return 5;
			else if (type.IsUnsignedInt) return 6;
			else if (type.IsUnsignedLong) return 7;
			else if (type.IsSingle) return 8;
			else if (type.IsDouble) return 9;
			else if (type.IsNativeSignedInteger) return 10;
			else if (type.IsNativeUnsignedInteger) return 11;
			else if (type.IsPointer) return 12;
			else if (type.IsObject) return 12;

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

			int destIndex = GetIndex(destinationOperand.Type);
			int srcIndex = GetIndex(sourceOperand.Type);

			BaseIRInstruction type = convTable[destIndex][srcIndex];

			if (type == null)
				throw new InvalidCompilerException();

			uint mask = 0xFFFFFFFF;
			BaseInstruction instruction = ComputeExtensionTypeAndMask(destinationOperand.Type, ref mask);

			if (type == IRInstruction.LogicalAnd)
			{
				if (mask == 0)
				{
					// TODO: May not be correct
					context.SetInstruction(IRInstruction.Move, destinationOperand, sourceOperand);
				}
				else
				{
					if (sourceOperand.IsSignedLong || sourceOperand.IsUnsignedLong)
					{
						Operand temp = AllocateVirtualRegister(destinationOperand.Type);

						context.SetInstruction(IRInstruction.Move, temp, sourceOperand);
						context.AppendInstruction(type, destinationOperand, temp, Operand.CreateConstantUnsignedInt(typeSystem, (uint)mask));
					}
					else
					{
						context.SetInstruction(type, destinationOperand, sourceOperand, Operand.CreateConstantUnsignedInt(typeSystem, (uint)mask));
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
			if (destinationType.IsByte)
			{
				mask = 0xFF;
				return (destinationType.IsSigned ? (BaseInstruction)IRInstruction.SignExtendedMove : (BaseInstruction)IRInstruction.ZeroExtendedMove);
			}
			else if (destinationType.IsShort)
			{
				mask = 0xFFFF;
				return destinationType.IsSigned ? (BaseInstruction)IRInstruction.SignExtendedMove : (BaseInstruction)IRInstruction.ZeroExtendedMove;
			}
			else if (destinationType.IsInt)
			{
				mask = 0xFFFFFFFF;
			}
			else if (destinationType.IsLong)
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
			if (!context.InvokeMethod.IsPInvokeImpl)
				return false;

			string external = context.InvokeMethod.ExternalReference;

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
		private void ProcessInvokeInstruction(Context context, MosaMethod method, Operand resultOperand, List<Operand> operands)
		{
			Operand symbolOperand = Operand.CreateSymbolFromMethod(typeSystem, method);
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
			MosaMethod currentMethod = methodCompiler.Method;
			MosaMethod invokeTarget = context.InvokeMethod;

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

			if (MustSignExtendOnLoad(source.Type))
			{
				extension = IRInstruction.SignExtendedMove;
			}
			else if (MustZeroExtendOnLoad(source.Type))
			{
				extension = IRInstruction.ZeroExtendedMove;
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
			var type = typeSystem.GetTypeByName("Mosa.Platform.Internal." + methodCompiler.Architecture.PlatformName, "Runtime");

			Debug.Assert(type != null, "Cannot find platform Runtime type");

			var method = TypeSystem.GetMethodByName(type, internalCallTarget.ToString());

			Debug.Assert(method != null, "Cannot find method: " + internalCallTarget.ToString());

			context.ReplaceInstructionOnly(IRInstruction.Call);
			context.SetOperand(0, Operand.CreateSymbolFromMethod(typeSystem, method));
			context.OperandCount = 1;
			context.InvokeMethod = method;
		}

		private bool ReplaceWithInternalCall(Context context)
		{
			var method = context.InvokeMethod;

			if (!method.IsInternal)
				return false;

			string replacementMethod = BuildInternalCallName(method);

			method = TypeSystem.GetMethodByName(method.DeclaringType, replacementMethod);

			Operand result = context.Result;

			List<Operand> operands = new List<Operand>(context.Operands);

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

		private static VmCall? TypeToVmBoxCall(MosaType type)
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

		private static VmCall TypeToVmUnboxCall(MosaType type)
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