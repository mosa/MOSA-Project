// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

		private uint CalculateMethodTableOffset(MosaMethod invokeTarget)
		{
			var slot = (uint)TypeLayout.GetMethodSlot(invokeTarget);

			return NativePointerSize * (slot + 14); // 14 is the offset into the TypeDef to the start of the MethodTable
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
			Debug.Assert(!method.DeclaringType.IsInterface);

			operands.RemoveAt(0);

			// Offset to TypeDef/MethodTable from thisPtr
			var typeDefOffset = CreateConstant32(-NativePointerSize);

			// Offset to the method pointer on the MethodTable
			var methodPointerOffset = CreateConstant32(CalculateMethodTableOffset(method));

			var typeDef = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);
			var callTarget = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);

			// Get the Method Table pointer
			context.SetInstruction(LoadInstruction, typeDef, thisPtr, typeDefOffset);

			// Get the address of the method
			context.AppendInstruction(LoadInstruction, callTarget, typeDef, methodPointerOffset);

			MakeCall(context, callTarget, result, operands, method);

			MethodScanner.MethodInvoked(method, Method);
		}

		private uint CalculateInterfaceSlot(MosaType interaceType)
		{
			return TypeLayout.GetInterfaceSlot(interaceType);
		}

		private uint CalculateInterfaceSlotOffset(MosaMethod invokeTarget)
		{
			var slot = CalculateInterfaceSlot(invokeTarget.DeclaringType);

			// Skip the first entry (Count)
			slot += 1;

			return slot * NativePointerSize;
		}

		private uint CalculateInterfaceMethodTableOffset(MosaMethod invokeTarget)
		{
			var slot = (uint)TypeLayout.GetMethodSlot(invokeTarget);

			// Skip the first two entries (TypeDef and Count)
			slot += 2;

			return NativePointerSize * slot;
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

			Debug.Assert(method.DeclaringType.IsInterface);

			// FUTURE: This can be optimized to skip Method Definition lookup.

			// Offset to MethodTable from thisPtr
			var typeDefOffset = CreateConstant32(-NativePointerSize);

			// Offset for InterfaceSlotTable in TypeDef/MethodTable
			var interfaceSlotTableOffset = CreateConstant32(NativePointerSize * 11);

			// Offset for InterfaceMethodTable in InterfaceSlotTable
			var interfaceMethodTableOffset = CreateConstant32(CalculateInterfaceSlotOffset(method));

			// Offset for Method Def in InterfaceMethodTable
			var methodDefinitionOffset = CreateConstant32(CalculateInterfaceMethodTableOffset(method));

			// Offset for Method pointer in MethodDef
			var methodPointerOffset = CreateConstant32(NativePointerSize * 4);

			// Operands to hold pointers
			var typeDef = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);
			var callTarget = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);

			var interfaceSlotPtr = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);
			var interfaceMethodTablePtr = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);
			var methodDefinition = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);

			// Get the MethodTable pointer
			context.SetInstruction(LoadInstruction, typeDef, thisPtr, typeDefOffset);

			// Get the InterfaceSlotTable pointer
			context.AppendInstruction(LoadInstruction, interfaceSlotPtr, typeDef, interfaceSlotTableOffset);

			// Get the InterfaceMethodTable pointer
			context.AppendInstruction(LoadInstruction, interfaceMethodTablePtr, interfaceSlotPtr, interfaceMethodTableOffset);

			// Get the MethodDef pointer
			context.AppendInstruction(LoadInstruction, methodDefinition, interfaceMethodTablePtr, methodDefinitionOffset);

			// Get the address of the method
			context.AppendInstruction(LoadInstruction, callTarget, methodDefinition, methodPointerOffset);

			MakeCall(context, callTarget, result, operands, method);

			MethodScanner.InterfaceMethodInvoked(method, Method);
		}

		private void MakeCall(Context context, Operand target, Operand result, List<Operand> operands, MosaMethod method)
		{
			Debug.Assert(method != null);

			//var data = TypeLayout.__GetMethodInfo(method);

			uint stackSize = CalculateParameterStackSize(operands);
			uint returnSize = CalculateReturnSize(result);

			uint totalStack = returnSize + stackSize;

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

		private void ReserveStackSizeForCall(Context context, uint stackSize)
		{
			if (stackSize == 0)
				return;

			context.AppendInstruction(SubInstruction, StackPointer, StackPointer, CreateConstant32(stackSize));
		}

		private void FreeStackAfterCall(Context context, uint stackSize)
		{
			if (stackSize == 0)
				return;

			context.AppendInstruction(AddInstruction, StackPointer, StackPointer, CreateConstant32(stackSize));
		}

		private uint CalculateParameterStackSize(List<Operand> operands)
		{
			uint stackSize = 0;

			for (int index = operands.Count - 1; index >= 0; index--)
			{
				var operand = operands[index];

				var size = GetTypeSize(operand.Type, true);
				stackSize += size;
			}

			return stackSize;
		}

		private uint CalculateReturnSize(Operand result)
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

		private void PushOperands(Context context, List<Operand> operands, uint space)
		{
			for (int index = operands.Count - 1; index >= 0; index--)
			{
				var operand = operands[index];

				var size = GetTypeSize(operand.Type, true);
				space -= size;

				Push(context, operand, space);
			}
		}

		private void Push(Context context, Operand operand, uint offset)
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
