// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Transforms.Call;

public abstract class BasePlugTransform : BaseTransform
{
	public BasePlugTransform(BaseInstruction instruction, TransformType type, bool log = false)
		: base(instruction, type, log)
	{ }

	#region Helpers

	public static uint CalculateInterfaceMethodTableOffset(Transform transform, MosaMethod invokeTarget)
	{
		var slot = transform.TypeLayout.GetMethodSlot(invokeTarget);

		// Skip the first two entries (TypeDef and Count)
		slot += 2;

		return transform.NativePointerSize * slot;
	}

	public static uint CalculateInterfaceSlotOffset(Transform transform, MosaMethod invokeTarget)
	{
		var slot = CalculateInterfaceSlot(transform, invokeTarget.DeclaringType);

		// Skip the first entry (Count)
		slot += 1;

		return slot * transform.NativePointerSize;
	}

	public static uint CalculateMethodTableOffset(Transform transform, MosaMethod invokeTarget)
	{
		var slot = transform.TypeLayout.GetMethodSlot(invokeTarget);

		return transform.NativePointerSize * (slot + 14); // 14 is the offset into the TypeDef to the start of the MethodTable
	}

	public static uint CalculateParameterStackSize(Transform transform, List<Operand> operands)
	{
		uint stackSize = 0;

		for (var index = operands.Count - 1; index >= 0; index--)
		{
			var operand = operands[index];
			var size = transform.MethodCompiler.GetSize(operand);

			stackSize += Alignment.AlignUp(size, transform.NativePointerSize);
		}

		return stackSize;
	}

	public static void MakeCall(Transform transform, Context context, Operand target, Operand result, List<Operand> operands)
	{
		//var data = TypeLayout.__GetMethodInfo(method);

		var stackSize = CalculateParameterStackSize(transform, operands);
		var returnSize = CalculateReturnSize(transform, result);

		var totalStack = returnSize + stackSize;

		ReserveStackSizeForCall(transform, context, totalStack);
		PushOperands(transform, context, operands, totalStack);

		// the mov/call two-instructions combo is to help facilitate the register allocator
		context.AppendInstruction(IR.CallDirect, null, target);

		GetReturnValue(transform, context, result);
		FreeStackAfterCall(transform, context, totalStack);
	}

	private static uint CalculateInterfaceSlot(Transform transform, MosaType interaceType)
	{
		return transform.TypeLayout.GetInterfaceSlot(interaceType);
	}

	private static uint CalculateReturnSize(Transform transform, Operand result)
	{
		if (result == null)
			return 0;

		if (result.IsPrimitive)
			return 0;

		return transform.MethodCompiler.GetSize(result, true);
	}

	private static void FreeStackAfterCall(Transform transform, Context context, uint stackSize)
	{
		if (stackSize == 0)
			return;

		context.AppendInstruction(transform.AddInstruction, transform.StackPointer, transform.StackPointer, Operand.CreateConstant32(stackSize));
	}

	private static void GetReturnValue(Transform transform, Context context, Operand result)
	{
		if (result == null)
			return;

		if (result.IsObject)
		{
			var returnLow = transform.PhysicalRegisters.AllocateObject(transform.Architecture.ReturnRegister);
			context.AppendInstruction(IR.Gen, returnLow);
			context.AppendInstruction(IR.MoveObject, result, returnLow);
		}
		else if (result.IsInt64 && transform.Is32BitPlatform)
		{
			var returnLow = transform.PhysicalRegisters.Allocate32(transform.Architecture.ReturnRegister);
			var returnHigh = transform.PhysicalRegisters.Allocate32(transform.Architecture.ReturnHighRegister);

			context.AppendInstruction(IR.Gen, returnLow);
			context.AppendInstruction(IR.Gen, returnHigh);
			context.AppendInstruction(IR.To64, result, returnLow, returnHigh);
		}
		else if (result.IsInteger)
		{
			var returnLow = transform.PhysicalRegisters.Allocate(result, transform.Architecture.ReturnRegister);
			context.AppendInstruction(IR.Gen, returnLow);
			context.AppendInstruction(transform.MoveInstruction, result, returnLow);
		}
		else if (result.IsR4)
		{
			var returnFP = transform.PhysicalRegisters.AllocateR4(transform.Architecture.ReturnFloatingPointRegister);
			context.AppendInstruction(IR.Gen, returnFP);
			context.AppendInstruction(IR.MoveR4, result, returnFP);
		}
		else if (result.IsR8)
		{
			var returnFP = transform.PhysicalRegisters.AllocateR8(transform.Architecture.ReturnFloatingPointRegister);
			context.AppendInstruction(IR.Gen, returnFP);
			context.AppendInstruction(IR.MoveR8, result, returnFP);
		}
		else if (result.IsValueType)
		{
			context.AppendInstruction(IR.LoadCompound, result, transform.StackPointer, Operand.Constant32_0);
		}
		else
		{
			var returnLow = transform.PhysicalRegisters.Allocate(result, transform.Architecture.ReturnRegister);
			context.AppendInstruction(IR.Gen, returnLow);
			context.AppendInstruction(transform.MoveInstruction, result, returnLow);
		}
	}

	private static void Push(Transform transform, Context context, Operand operand, uint offset)
	{
		var offsetOperand = Operand.CreateConstant32(offset);

		if (operand.IsObject)
		{
			context.AppendInstruction(IR.StoreObject, null, transform.StackPointer, offsetOperand, operand);
		}
		else if (operand.IsInt32)
		{
			context.AppendInstruction(IR.Store32, null, transform.StackPointer, offsetOperand, operand);
		}
		else if (operand.IsInt64)
		{
			context.AppendInstruction(IR.Store64, null, transform.StackPointer, offsetOperand, operand);
		}
		else if (operand.IsR4)
		{
			context.AppendInstruction(IR.StoreR4, null, transform.StackPointer, offsetOperand, operand);
		}
		else if (operand.IsR8)
		{
			context.AppendInstruction(IR.StoreR8, null, transform.StackPointer, offsetOperand, operand);
		}
		else if (operand.IsValueType)
		{
			context.AppendInstruction(IR.StoreCompound, null, transform.StackPointer, offsetOperand, operand);
		}
		else
		{
			context.AppendInstruction(transform.StoreInstruction, null, transform.StackPointer, offsetOperand, operand);
		}
	}

	private static void PushOperands(Transform transform, Context context, List<Operand> operands, uint offset)
	{
		for (var index = operands.Count - 1; index >= 0; index--)
		{
			var operand = operands[index];
			var size = transform.MethodCompiler.GetSize(operand);

			offset -= Alignment.AlignUp(size, transform.NativePointerSize);

			Push(transform, context, operand, offset);
		}
	}

	private static void ReserveStackSizeForCall(Transform transform, Context context, uint stackSize)
	{
		if (stackSize == 0)
			return;

		context.AppendInstruction(transform.SubInstruction, transform.StackPointer, transform.StackPointer, Operand.CreateConstant32(stackSize));
	}

	#endregion Helpers
}
