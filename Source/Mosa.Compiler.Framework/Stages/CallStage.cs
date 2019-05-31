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
		private BaseInstruction loadInstruction;
		private BaseInstruction moveInstruction;

		protected override void Setup()
		{
			loadInstruction = Select(IRInstruction.LoadInt32, IRInstruction.LoadInt64);
			moveInstruction = Select(IRInstruction.MoveInt32, IRInstruction.MoveInt64);
		}

		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.SetReturnR4, SetReturnR4);
			AddVisitation(IRInstruction.SetReturnR8, SetReturnR8);
			AddVisitation(IRInstruction.SetReturn32, SetReturn32);
			AddVisitation(IRInstruction.SetReturn64, SetReturn64);
			AddVisitation(IRInstruction.SetReturnCompound, SetReturnCompound);
			AddVisitation(IRInstruction.CallInterface, CallInterface);
			AddVisitation(IRInstruction.CallStatic, CallStatic);
			AddVisitation(IRInstruction.CallVirtual, CallVirtual);
			AddVisitation(IRInstruction.CallDynamic, CallDynamic);
		}

		private void SetReturnR4(Context context)
		{
			context.SetInstruction(IRInstruction.MoveFloatR4, Operand.CreateCPURegister(context.Operand1.Type, Architecture.ReturnFloatingPointRegister), context.Operand1);
		}

		private void SetReturnR8(Context context)
		{
			context.SetInstruction(IRInstruction.MoveFloatR8, Operand.CreateCPURegister(context.Operand1.Type, Architecture.ReturnFloatingPointRegister), context.Operand1);
		}

		private void SetReturn32(Context context)
		{
			context.SetInstruction(IRInstruction.MoveInt32, Operand.CreateCPURegister(context.Operand1.Type, Architecture.ReturnRegister), context.Operand1);
		}

		private void SetReturn64(Context context)
		{
			var operand = context.Operand1;

			if (Is32BitPlatform)
			{
				context.SetInstruction(IRInstruction.GetLow64, Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, Architecture.ReturnRegister), operand);
				context.AppendInstruction(IRInstruction.GetHigh64, Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, Architecture.ReturnHighRegister), operand);
			}
			else
			{
				context.SetInstruction(IRInstruction.MoveInt64, Operand.CreateCPURegister(TypeSystem.BuiltIn.U8, Architecture.ReturnRegister), context.Operand1);
			}
		}

		private void SetReturnCompound(Context context)
		{
			var OffsetOfFirstParameterOperand = CreateConstant(Architecture.OffsetOfFirstParameter);
			context.SetInstruction(IRInstruction.StoreCompound, null, StackFrame, OffsetOfFirstParameterOperand, context.Operand1);
		}

		private int CalculateMethodTableOffset(MosaMethod invokeTarget)
		{
			int slot = TypeLayout.GetMethodSlot(invokeTarget);

			return NativePointerSize * slot;
		}

		private void CallStatic(InstructionNode node)
		{
			var call = node.Operand1;
			var result = node.Result;
			var method = call.Method;
			var operands = node.GetOperands();

			Debug.Assert(method != null);

			if (MethodCompiler.Compiler.CompilerData.IsMethodInlined(method))
			{
				// If above is true, then a race condition occurred between:
				// 1) after inline stage of this method, and
				// 2) theinline evaluation stage of the target method
				// the only best option at this point is to stop compiling this method.
				// Note: It should already be scheduled to be re-compiled!
				//MethodCompiler.Stop();
				//return;
			}

			//Debug.Assert(!MethodCompiler.Compiler.CompilerData.IsMethodInlined(method));

			operands.RemoveAt(0);

			var context = new Context(node);

			context.Empty();

			MakeCall(context, call, result, operands);

			Debug.Assert(method == call.Method);

			MethodScanner.MethodDirectInvoked(call.Method, Method);
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

			if (call.Method != null)
			{
				MethodScanner.MethodInvoked(call.Method, Method);
			}
		}

		private void CallVirtual(InstructionNode node)
		{
			var call = node.Operand1;
			var result = node.Result;
			var method = call.Method;
			var thisPtr = node.Operand2;
			var operands = node.GetOperands();

			Debug.Assert(method != null);

			operands.RemoveAt(0);

			var typeDefinition = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);
			var callTarget = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);

			Debug.Assert(!method.DeclaringType.IsInterface);

			var context = new Context(node);

			// Same as above except for methodPointer
			int methodPointerOffset = CalculateMethodTableOffset(method) + (NativePointerSize * 14);

			// Get the TypeDef pointer
			context.SetInstruction(loadInstruction, typeDefinition, thisPtr, ConstantZero);

			// Get the address of the method
			context.AppendInstruction(loadInstruction, callTarget, typeDefinition, CreateConstant(methodPointerOffset));

			MakeCall(context, callTarget, result, operands);

			MethodScanner.MethodInvoked(method, Method);
		}

		private int CalculateInterfaceSlot(MosaType interaceType)
		{
			return TypeLayout.GetInterfaceSlot(interaceType);
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
			context.SetInstruction(loadInstruction, typeDefinition, thisPtr, ConstantZero);

			// Get the Interface Slot Table pointer
			context.AppendInstruction(loadInstruction, interfaceSlotPtr, typeDefinition, CreateConstant(interfaceSlotTableOffset));

			// Get the Interface Method Table pointer
			context.AppendInstruction(loadInstruction, interfaceMethodTablePtr, interfaceSlotPtr, CreateConstant(interfaceMethodTableOffset));

			// Get the MethodDef pointer
			context.AppendInstruction(loadInstruction, methodDefinition, interfaceMethodTablePtr, CreateConstant(methodDefinitionOffset));

			// Get the address of the method
			context.AppendInstruction(loadInstruction, callTarget, methodDefinition, CreateConstant(methodPointerOffset));

			MakeCall(context, callTarget, result, operands);

			MethodScanner.InterfaceMethodInvoked(method, Method);
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

			context.AppendInstruction(Select(StackPointer, IRInstruction.Sub32, IRInstruction.Sub64), StackPointer, StackPointer, CreateConstant(TypeSystem.BuiltIn.I4, stackSize));
		}

		private void FreeStackAfterCall(Context context, int stackSize)
		{
			if (stackSize == 0)
				return;

			context.AppendInstruction(Select(StackPointer, IRInstruction.Add32, IRInstruction.Add64), StackPointer, StackPointer, CreateConstant(TypeSystem.BuiltIn.I4, stackSize));
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
				context.AppendInstruction(Select(operand, IRInstruction.StoreInt32, IRInstruction.StoreInt64), null, scratch, offsetOperand, operand);
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
				context.AppendInstruction(Select(operand, IRInstruction.StoreInt32, IRInstruction.StoreInt64), null, scratch, offsetOperand, operand);
			}
		}

		private void GetReturnValue(Context context, Operand result)
		{
			if (result == null)
				return;

			if (result.Is64BitInteger && Is32BitPlatform)
			{
				var returnLow = Operand.CreateCPURegister(result.Type, Architecture.ReturnRegister);
				var returnHigh = Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, Architecture.ReturnHighRegister);

				context.AppendInstruction(IRInstruction.Gen, returnLow);
				context.AppendInstruction(IRInstruction.Gen, returnHigh);
				context.AppendInstruction(IRInstruction.To64, result, returnLow, returnHigh);
			}
			else if (result.IsInteger)
			{
				var returnLow = Operand.CreateCPURegister(result.Type, Architecture.ReturnRegister);
				context.AppendInstruction(IRInstruction.Gen, returnLow);
				context.AppendInstruction(moveInstruction, result, returnLow);
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
				var returnLow = Operand.CreateCPURegister(result.Type, Architecture.ReturnRegister);
				context.AppendInstruction(IRInstruction.Gen, returnLow);
				context.AppendInstruction(moveInstruction, result, returnLow);
			}
		}
	}
}
