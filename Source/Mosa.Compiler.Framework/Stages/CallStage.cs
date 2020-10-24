﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
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
			AddVisitation(IRInstruction.SetReturnObject, SetReturnObject);
			AddVisitation(IRInstruction.SetReturn32, SetReturn32);
			AddVisitation(IRInstruction.SetReturn64, SetReturn64);
			AddVisitation(IRInstruction.SetReturnR4, SetReturnR4);
			AddVisitation(IRInstruction.SetReturnR8, SetReturnR8);
			AddVisitation(IRInstruction.SetReturnCompound, SetReturnCompound);
			AddVisitation(IRInstruction.CallInterface, CallInterface);
			AddVisitation(IRInstruction.CallStatic, CallStatic);
			AddVisitation(IRInstruction.CallVirtual, CallVirtual);
			AddVisitation(IRInstruction.CallDynamic, CallDynamic);
		}

		private void SetReturnR4(Context context)
		{
			context.SetInstruction(IRInstruction.MoveR4, Operand.CreateCPURegister(context.Operand1.Type, Architecture.ReturnFloatingPointRegister), context.Operand1);
		}

		private void SetReturnR8(Context context)
		{
			context.SetInstruction(IRInstruction.MoveR8, Operand.CreateCPURegister(context.Operand1.Type, Architecture.ReturnFloatingPointRegister), context.Operand1);
		}

		private void SetReturnObject(Context context)
		{
			context.SetInstruction(IRInstruction.MoveObject, Operand.CreateCPURegister(context.Operand1.Type, Architecture.ReturnRegister), context.Operand1);
		}

		private void SetReturn32(Context context)
		{
			context.SetInstruction(IRInstruction.Move32, Operand.CreateCPURegister(context.Operand1.Type, Architecture.ReturnRegister), context.Operand1);
		}

		private void SetReturn64(Context context)
		{
			var operand = context.Operand1;

			if (Is32BitPlatform)
			{
				context.SetInstruction(IRInstruction.GetLow32, Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, Architecture.ReturnRegister), operand);
				context.AppendInstruction(IRInstruction.GetHigh32, Operand.CreateCPURegister(TypeSystem.BuiltIn.U4, Architecture.ReturnHighRegister), operand);
			}
			else
			{
				context.SetInstruction(IRInstruction.Move64, Operand.CreateCPURegister(TypeSystem.BuiltIn.U8, Architecture.ReturnRegister), context.Operand1);
			}
		}

		private void SetReturnCompound(Context context)
		{
			var OffsetOfFirstParameterOperand = CreateConstant32(Architecture.OffsetOfFirstParameter);
			context.SetInstruction(IRInstruction.StoreCompound, null, StackFrame, OffsetOfFirstParameterOperand, context.Operand1);
		}

		private int CalculateMethodTableOffset(MosaMethod invokeTarget)
		{
			int slot = TypeLayout.GetMethodSlot(invokeTarget);

			return NativePointerSize * slot;
		}

		private void CallStatic(Context context)
		{
			var call = context.Operand1;
			var result = context.Result;
			var method = call.Method;
			var operands = context.GetOperands();

			Debug.Assert(method != null);

			operands.RemoveAt(0);
			context.Empty();

			MakeCall(context, call, result, operands, method);

			MethodScanner.MethodDirectInvoked(call.Method, Method);
		}

		private void CallDynamic(Context context)
		{
			var call = context.Operand1;
			var result = context.Result;
			var method = context.InvokeMethod;
			var operands = context.GetOperands();

			operands.RemoveAt(0);
			context.Empty();

			MakeCall(context, call, result, operands, method);

			MethodScanner.MethodInvoked(call.Method, method);
		}

		private void CallVirtual(Context context)
		{
			var call = context.Operand1;
			var result = context.Result;
			var method = call.Method;
			var thisPtr = context.Operand2;
			var operands = context.GetOperands();

			Debug.Assert(method != null);

			operands.RemoveAt(0);

			var typeDefinition = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);
			var callTarget = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);

			Debug.Assert(!method.DeclaringType.IsInterface);

			// Same as above except for methodPointer
			int methodPointerOffset = CalculateMethodTableOffset(method) + (NativePointerSize * 14);

			// Get the TypeDef pointer
			context.SetInstruction(LoadInstruction, typeDefinition, thisPtr, ConstantZero);

			// Get the address of the method
			context.AppendInstruction(LoadInstruction, callTarget, typeDefinition, CreateConstant32(methodPointerOffset));

			MakeCall(context, callTarget, result, operands, method);

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

		private void CallInterface(Context context)
		{
			var call = context.Operand1;
			var method = call.Method;
			var result = context.Result;
			var thisPtr = context.Operand2;
			var operands = context.GetOperands();

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

			// Get the TypeDef pointer
			context.SetInstruction(LoadInstruction, typeDefinition, thisPtr, ConstantZero);

			// Get the Interface Slot Table pointer
			context.AppendInstruction(LoadInstruction, interfaceSlotPtr, typeDefinition, CreateConstant32(interfaceSlotTableOffset));

			// Get the Interface Method Table pointer
			context.AppendInstruction(LoadInstruction, interfaceMethodTablePtr, interfaceSlotPtr, CreateConstant32(interfaceMethodTableOffset));

			// Get the MethodDef pointer
			context.AppendInstruction(LoadInstruction, methodDefinition, interfaceMethodTablePtr, CreateConstant32(methodDefinitionOffset));

			// Get the address of the method
			context.AppendInstruction(LoadInstruction, callTarget, methodDefinition, CreateConstant32(methodPointerOffset));

			MakeCall(context, callTarget, result, operands, method);

			MethodScanner.InterfaceMethodInvoked(method, Method);
		}

		private void MakeCall(Context context, Operand target, Operand result, List<Operand> operands, MosaMethod method)
		{
			Debug.Assert(method != null);

			//var data = TypeLayout.__GetMethodInfo(method);

			int stackSize = CalculateParameterStackSize(operands);
			int returnSize = CalculateReturnSize(result);

			int totalStack = returnSize + stackSize;

			//if (data.ParameterStackSize != stackSize)
			//{
			//	int apple = 1;
			//}

			ReserveStackSizeForCall(context, totalStack);
			PushOperands(context, operands, totalStack);

			// the mov/call two-instructions combo is to help facilitate the register allocator
			context.AppendInstruction(IRInstruction.CallDirect, null, target);

			GetReturnValue(context, result);
			FreeStackAfterCall(context, totalStack);
		}

		private void ReserveStackSizeForCall(Context context, int stackSize)
		{
			if (stackSize == 0)
				return;

			context.AppendInstruction(SubInstruction, StackPointer, StackPointer, CreateConstant32(stackSize));
		}

		private void FreeStackAfterCall(Context context, int stackSize)
		{
			if (stackSize == 0)
				return;

			context.AppendInstruction(AddInstruction, StackPointer, StackPointer, CreateConstant32(stackSize));
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

			if (!MosaTypeLayout.CanFitInRegister(returnType))
			{
				return Alignment.AlignUp(TypeLayout.GetTypeSize(returnType), NativeAlignment);
			}

			return 0;
		}

		private void PushOperands(Context context, List<Operand> operands, int space)
		{
			for (int index = operands.Count - 1; index >= 0; index--)
			{
				var operand = operands[index];

				var size = GetTypeSize(operand.Type, true);
				space -= size;

				Push(context, operand, space);
			}
		}

		private void Push(Context context, Operand operand, int offset)
		{
			var offsetOperand = CreateConstant32(offset);

			if (operand.IsReferenceType)
			{
				context.AppendInstruction(IRInstruction.StoreObject, null, StackPointer, offsetOperand, operand);
			}
			else if (operand.IsInteger32)
			{
				context.AppendInstruction(IRInstruction.Store32, null, StackPointer, offsetOperand, operand);
			}
			else if (operand.IsInteger64)
			{
				context.AppendInstruction(IRInstruction.Store64, null, StackPointer, offsetOperand, operand);
			}
			else if (operand.IsR4)
			{
				context.AppendInstruction(IRInstruction.StoreR4, null, StackPointer, offsetOperand, operand);
			}
			else if (operand.IsR8)
			{
				context.AppendInstruction(IRInstruction.StoreR8, null, StackPointer, offsetOperand, operand);
			}
			else if (!MosaTypeLayout.CanFitInRegister(operand.Type))
			{
				context.AppendInstruction(IRInstruction.StoreCompound, null, StackPointer, offsetOperand, operand);
			}
			else
			{
				context.AppendInstruction(StoreInstruction, null, StackPointer, offsetOperand, operand);
			}
		}

		private void GetReturnValue(Context context, Operand result)
		{
			if (result == null)
				return;

			if (result.IsReferenceType)
			{
				var returnLow = Operand.CreateCPURegister(result.Type, Architecture.ReturnRegister);
				context.AppendInstruction(IRInstruction.Gen, returnLow);
				context.AppendInstruction(IRInstruction.MoveObject, result, returnLow);
			}
			else if (result.IsInteger64 && Is32BitPlatform)
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
				context.AppendInstruction(MoveInstruction, result, returnLow);
			}
			else if (result.IsR4)
			{
				var returnFP = Operand.CreateCPURegister(result.Type, Architecture.ReturnFloatingPointRegister);
				context.AppendInstruction(IRInstruction.Gen, returnFP);
				context.AppendInstruction(IRInstruction.MoveR4, result, returnFP);
			}
			else if (result.IsR8)
			{
				var returnFP = Operand.CreateCPURegister(result.Type, Architecture.ReturnFloatingPointRegister);
				context.AppendInstruction(IRInstruction.Gen, returnFP);
				context.AppendInstruction(IRInstruction.MoveR8, result, returnFP);
			}
			else if (!MosaTypeLayout.CanFitInRegister(result.Type))
			{
				Debug.Assert(result.IsStackLocal);
				context.AppendInstruction(IRInstruction.LoadCompound, result, StackPointer, ConstantZero);
			}
			else
			{
				// note: same for integer logic (above)
				var returnLow = Operand.CreateCPURegister(result.Type, Architecture.ReturnRegister);
				context.AppendInstruction(IRInstruction.Gen, returnLow);
				context.AppendInstruction(MoveInstruction, result, returnLow);
			}
		}
	}
}
