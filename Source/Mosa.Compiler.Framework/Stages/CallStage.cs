// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Lower Call Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCodeTransformationStage" />
	public sealed class CallStage : BaseCodeTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.SetReturn, SetReturn);
			AddVisitation(IRInstruction.CallInterface, CallInterface);
			AddVisitation(IRInstruction.CallStatic, CallStatic);
			AddVisitation(IRInstruction.CallVirtual, CallVirtual);
			AddVisitation(IRInstruction.CallDynamic, CallDynamic);
		}

		private void SetReturn(Context context)
		{
			var operand = context.Operand1;

			Debug.Assert(operand != null);

			if (operand.IsR4)
			{
				context.SetInstruction(IRInstruction.MoveFloatR4, Operand.CreateCPURegister(operand.Type, Architecture.ReturnFloatingPointRegister), operand);
			}
			else if (operand.IsR8)
			{
				context.SetInstruction(IRInstruction.MoveFloatR8, Operand.CreateCPURegister(operand.Type, Architecture.ReturnFloatingPointRegister), operand);
			}
			else if (operand.IsLong)
			{
				var v1 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);
				var v2 = AllocateVirtualRegister(TypeSystem.BuiltIn.U4);

				context.SetInstruction2(IRInstruction.Split64, v1, v2, operand);
				context.AppendInstruction(IRInstruction.MoveInteger, InstructionSize.Size32, Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, Architecture.Return32BitRegister), v1);
				context.AppendInstruction(IRInstruction.MoveInteger, InstructionSize.Size32, Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, Architecture.Return64BitRegister), v2);
			}
			else if (MosaTypeLayout.IsStoredOnStack(operand.Type))
			{
				var OffsetOfFirstParameterOperand = CreateConstant(Architecture.OffsetOfFirstParameter);
				context.SetInstruction(IRInstruction.StoreCompound, null, StackFrame, OffsetOfFirstParameterOperand, operand);
			}
			else
			{
				context.SetInstruction(IRInstruction.MoveInteger, InstructionSize.Size32, Operand.CreateCPURegister(operand.Type, Architecture.Return32BitRegister), operand);
			}
		}

		private int CalculateMethodTableOffset(MosaMethod invokeTarget)
		{
			int slot = TypeLayout.GetMethodTableOffset(invokeTarget);

			return NativePointerSize * slot;
		}

		private void CallStatic(InstructionNode node)
		{
			var call = node.Operand1;
			var result = node.Result;
			var method = call.Method;
			var operands = node.GetOperands();

			Debug.Assert(method != null);
			Debug.Assert(method == node.InvokeMethod || node.InvokeMethod == null);

			operands.RemoveAt(0);

			var context = new Context(node);

			context.Empty();

			MakeCall(context, call, result, operands);
		}

		private void CallDynamic(InstructionNode node)
		{
			var call = node.Operand1;
			var result = node.Result;
			var operands = node.GetOperands();

			operands.RemoveAt(0);

			var context = new Context(node);

			context.Empty();

			MakeCall(context, call, result, operands);
		}

		private void CallVirtual(InstructionNode node)
		{
			var call = node.Operand1;
			var result = node.Result;
			var method = call.Method;
			var thisPtr = node.Operand2;
			var operands = node.GetOperands();

			Debug.Assert(method != null);
			Debug.Assert(method == node.InvokeMethod || node.InvokeMethod == null);

			operands.RemoveAt(0);

			var typeDefinition = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);
			var callTarget = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);

			Debug.Assert(!method.DeclaringType.IsInterface);

			var context = new Context(node);

			// Same as above except for methodPointer
			int methodPointerOffset = CalculateMethodTableOffset(method) + (NativePointerSize * 14);

			// Get the TypeDef pointer
			context.SetInstruction(IRInstruction.LoadInteger, NativeInstructionSize, typeDefinition, thisPtr, ConstantZero);

			// Get the address of the method
			context.AppendInstruction(IRInstruction.LoadInteger, NativeInstructionSize, callTarget, typeDefinition, CreateConstant(methodPointerOffset));

			MakeCall(context, callTarget, result, operands);
		}

		private int CalculateInterfaceSlot(MosaType interaceType)
		{
			return TypeLayout.GetInterfaceSlotOffset(interaceType);
		}

		private int CalculateInterfaceSlotOffset(MosaMethod invokeTarget)
		{
			return CalculateInterfaceSlot(invokeTarget.DeclaringType) * NativePointerSize;
		}

		private void CallInterface(InstructionNode node)
		{
			var call = node.Operand1;
			var method = call.Method;
			var result = node.Result;
			var thisPtr = node.Operand2;
			var operands = node.GetOperands();

			Debug.Assert(method != null);
			Debug.Assert(method == node.InvokeMethod || node.InvokeMethod == null);

			operands.RemoveAt(0);

			var typeDefinition = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);
			var callTarget = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);

			Debug.Assert(method.DeclaringType.IsInterface);

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
			var methodDefinition = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);

			var context = new Context(node);

			// Get the TypeDef pointer
			context.SetInstruction(IRInstruction.LoadInteger, NativeInstructionSize, typeDefinition, thisPtr, ConstantZero);

			// Get the Interface Slot Table pointer
			context.AppendInstruction(IRInstruction.LoadInteger, NativeInstructionSize, interfaceSlotPtr, typeDefinition, CreateConstant(interfaceSlotTableOffset));

			// Get the Interface Method Table pointer
			context.AppendInstruction(IRInstruction.LoadInteger, NativeInstructionSize, interfaceMethodTablePtr, interfaceSlotPtr, CreateConstant(interfaceMethodTableOffset));

			// Get the MethodDef pointer
			context.AppendInstruction(IRInstruction.LoadInteger, NativeInstructionSize, methodDefinition, interfaceMethodTablePtr, CreateConstant(methodDefinitionOffset));

			// Get the address of the method
			context.AppendInstruction(IRInstruction.LoadInteger, NativeInstructionSize, callTarget, methodDefinition, CreateConstant(methodPointerOffset));

			MakeCall(context, callTarget, result, operands);
		}

		private void MakeCall(Context context, Operand target, Operand result, List<Operand> operands)
		{
			int stackSize = CalculateParameterStackSize(operands);
			int returnSize = CalculateReturnSize(result);

			int totalStack = returnSize + stackSize;

			ReserveStackSizeForCall(context, totalStack);
			PushOperands(context, operands, totalStack, StackPointer);

			// the mov/call two-instructions combo is to help facilitate the register allocator
			context.AppendInstruction(IRInstruction.CallDirect, null, target);

			GetReturnValue(context, result);
			FreeStackAfterCall(context, totalStack);
		}

		private void ReserveStackSizeForCall(Context context, int stackSize)
		{
			if (stackSize == 0)
				return;

			context.AppendInstruction(Select(IRInstruction.SubSigned32, IRInstruction.SubSigned64), StackPointer, StackPointer, CreateConstant(TypeSystem.BuiltIn.I4, stackSize));
		}

		private void FreeStackAfterCall(Context context, int stackSize)
		{
			if (stackSize == 0)
				return;

			context.AppendInstruction(Select(IRInstruction.AddSigned32, IRInstruction.AddSigned64), StackPointer, StackPointer, CreateConstant(TypeSystem.BuiltIn.I4, stackSize));
		}

		private int CalculateParameterStackSize(List<Operand> operands)
		{
			int stackSize = 0;

			for (int index = operands.Count - 1; index >= 0; index--)
			{
				var operand = operands[index];

				var size = GetTypeSize(operand.Type, true);
				stackSize += size;
			}

			return stackSize;
		}

		private int CalculateReturnSize(Operand result)
		{
			if (result == null)
				return 0;

			var returnType = result.Type;

			if (MosaTypeLayout.IsStoredOnStack(returnType))
			{
				return Alignment.AlignUp(TypeLayout.GetTypeSize(returnType), NativeAlignment);
			}

			return 0;
		}

		private void PushOperands(Context context, List<Operand> operands, int space, Operand scratch)
		{
			for (int index = operands.Count - 1; index >= 0; index--)
			{
				var operand = operands[index];

				var size = GetTypeSize(operand.Type, true);
				space -= size;

				Push(context, operand, space, scratch);
			}
		}

		private void Push(Context context, Operand operand, int offset, Operand scratch)
		{
			var offsetOperand = CreateConstant(offset);

			if (operand.IsInteger)
			{
				var size = GetInstructionSize(operand.Type);
				context.AppendInstruction(IRInstruction.StoreInteger, size, null, scratch, offsetOperand, operand);
			}
			else if (operand.IsR4)
			{
				context.AppendInstruction(IRInstruction.StoreFloatR4, null, scratch, offsetOperand, operand);
			}
			else if (operand.IsR8)
			{
				context.AppendInstruction(IRInstruction.StoreFloatR8, null, scratch, offsetOperand, operand);
			}
			else if (MosaTypeLayout.IsStoredOnStack(operand.Type))
			{
				context.AppendInstruction(IRInstruction.StoreCompound, null, scratch, offsetOperand, operand);
			}
			else
			{
				// note: same for integer logic (above)
				var size = GetInstructionSize(operand.Type);
				context.AppendInstruction(IRInstruction.StoreInteger, size, null, scratch, offsetOperand, operand);
			}
		}

		private void GetReturnValue(Context context, Operand result)
		{
			if (result == null)
				return;

			if (result.Is64BitInteger && Architecture.NativeIntegerSize == 32)
			{
				var returnLow = Operand.CreateCPURegister(result.Type, Architecture.Return32BitRegister);
				var returnHigh = Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, Architecture.Return64BitRegister);

				context.AppendInstruction(IRInstruction.Gen, returnLow);
				context.AppendInstruction(IRInstruction.Gen, returnHigh);
				context.AppendInstruction(IRInstruction.To64, result, returnLow, returnHigh);
			}
			else if (result.IsInteger)
			{
				var returnLow = Operand.CreateCPURegister(result.Type, Architecture.Return32BitRegister);
				context.AppendInstruction(IRInstruction.Gen, returnLow);
				context.AppendInstruction(IRInstruction.MoveInteger, result, returnLow);
			}
			else if (result.IsR4)
			{
				var returnFP = Operand.CreateCPURegister(result.Type, Architecture.ReturnFloatingPointRegister);
				context.AppendInstruction(IRInstruction.Gen, returnFP);
				context.AppendInstruction(IRInstruction.MoveFloatR4, result, returnFP);
			}
			else if (result.IsR8)
			{
				var returnFP = Operand.CreateCPURegister(result.Type, Architecture.ReturnFloatingPointRegister);
				context.AppendInstruction(IRInstruction.Gen, returnFP);
				context.AppendInstruction(IRInstruction.MoveFloatR8, result, returnFP);
			}
			else if (MosaTypeLayout.IsStoredOnStack(result.Type))
			{
				Debug.Assert(result.IsStackLocal);
				context.AppendInstruction(IRInstruction.LoadCompound, result, StackPointer, ConstantZero);
			}
			else
			{
				// note: same for integer logic (above)
				var returnLow = Operand.CreateCPURegister(result.Type, Architecture.Return32BitRegister);
				context.AppendInstruction(IRInstruction.Gen, returnLow);
				context.AppendInstruction(IRInstruction.MoveInteger, result, returnLow);
			}
		}
	}
}
