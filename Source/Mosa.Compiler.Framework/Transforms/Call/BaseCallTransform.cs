// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Transforms.Call
{
	public abstract class BasePlugTransform : BaseTransform
	{
		public BasePlugTransform(BaseInstruction instruction, TransformType type, bool log = false)
			: base(instruction, type, log)
		{ }

		#region Helpers

		public static uint CalculateInterfaceMethodTableOffset(TransformContext transform, MosaMethod invokeTarget)
		{
			var slot = transform.TypeLayout.GetMethodSlot(invokeTarget);

			// Skip the first two entries (TypeDef and Count)
			slot += 2;

			return transform.NativePointerSize * slot;
		}

		public static uint CalculateInterfaceSlotOffset(TransformContext transform, MosaMethod invokeTarget)
		{
			var slot = CalculateInterfaceSlot(transform, invokeTarget.DeclaringType);

			// Skip the first entry (Count)
			slot += 1;

			return slot * transform.NativePointerSize;
		}

		public static uint CalculateMethodTableOffset(TransformContext transform, MosaMethod invokeTarget)
		{
			var slot = transform.TypeLayout.GetMethodSlot(invokeTarget);

			return transform.NativePointerSize * (slot + 14); // 14 is the offset into the TypeDef to the start of the MethodTable
		}

		public static uint CalculateParameterStackSize(TransformContext transform, List<Operand> operands)
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

		public static void MakeCall(TransformContext transform, Context context, Operand target, Operand result, List<Operand> operands, MosaMethod method)
		{
			Debug.Assert(method != null);

			//var data = TypeLayout.__GetMethodInfo(method);

			var stackSize = CalculateParameterStackSize(transform, operands);
			var returnSize = CalculateReturnSize(transform, result);

			var totalStack = returnSize + stackSize;

			ReserveStackSizeForCall(transform, context, totalStack);
			PushOperands(transform, context, operands, totalStack);

			// the mov/call two-instructions combo is to help facilitate the register allocator
			context.AppendInstruction(IRInstruction.CallDirect, null, target);

			GetReturnValue(transform, context, result);
			FreeStackAfterCall(transform, context, totalStack);
		}

		private static uint CalculateInterfaceSlot(TransformContext transform, MosaType interaceType)
		{
			return transform.TypeLayout.GetInterfaceSlot(interaceType);
		}

		private static uint CalculateReturnSize(TransformContext transform, Operand result)
		{
			if (result == null)
				return 0;

			if (result.IsPrimitive)
				return 0;

			return transform.MethodCompiler.GetSize(result, true);
		}

		private static void FreeStackAfterCall(TransformContext transform, Context context, uint stackSize)
		{
			if (stackSize == 0)
				return;

			context.AppendInstruction(transform.AddInstruction, transform.StackPointer, transform.StackPointer, transform.CreateConstant32(stackSize));
		}

		private static void GetReturnValue(TransformContext transform, Context context, Operand result)
		{
			if (result == null)
				return;

			if (result.IsObject)
			{
				var returnLow = Operand.CreateCPURegister(result, transform.Architecture.ReturnRegister);
				context.AppendInstruction(IRInstruction.Gen, returnLow);
				context.AppendInstruction(IRInstruction.MoveObject, result, returnLow);
			}
			else if (result.IsInt64 && transform.Is32BitPlatform)
			{
				var returnLow = Operand.CreateCPURegister(result, transform.Architecture.ReturnRegister);
				var returnHigh = Operand.CreateCPURegister32(transform.Architecture.ReturnHighRegister);

				context.AppendInstruction(IRInstruction.Gen, returnLow);
				context.AppendInstruction(IRInstruction.Gen, returnHigh);
				context.AppendInstruction(IRInstruction.To64, result, returnLow, returnHigh);
			}
			else if (result.IsInteger)
			{
				var returnLow = Operand.CreateCPURegister(result, transform.Architecture.ReturnRegister);
				context.AppendInstruction(IRInstruction.Gen, returnLow);
				context.AppendInstruction(transform.MoveInstruction, result, returnLow);
			}
			else if (result.IsR4)
			{
				var returnFP = Operand.CreateCPURegister(result, transform.Architecture.ReturnFloatingPointRegister);
				context.AppendInstruction(IRInstruction.Gen, returnFP);
				context.AppendInstruction(IRInstruction.MoveR4, result, returnFP);
			}
			else if (result.IsR8)
			{
				var returnFP = Operand.CreateCPURegister(result, transform.Architecture.ReturnFloatingPointRegister);
				context.AppendInstruction(IRInstruction.Gen, returnFP);
				context.AppendInstruction(IRInstruction.MoveR8, result, returnFP);
			}
			else if (result.IsValueType)
			{
				context.AppendInstruction(IRInstruction.LoadCompound, result, transform.StackPointer, Operand.Constant32_0);
			}
			else
			{
				var returnLow = Operand.CreateCPURegister(result, transform.Architecture.ReturnRegister);
				context.AppendInstruction(IRInstruction.Gen, returnLow);
				context.AppendInstruction(transform.MoveInstruction, result, returnLow);
			}
		}

		private static void Push(TransformContext transform, Context context, Operand operand, uint offset)
		{
			var offsetOperand = transform.CreateConstant32(offset);

			if (operand.IsObject)
			{
				context.AppendInstruction(IRInstruction.StoreObject, null, transform.StackPointer, offsetOperand, operand);
			}
			else if (operand.IsInt32)
			{
				context.AppendInstruction(IRInstruction.Store32, null, transform.StackPointer, offsetOperand, operand);
			}
			else if (operand.IsInt64)
			{
				context.AppendInstruction(IRInstruction.Store64, null, transform.StackPointer, offsetOperand, operand);
			}
			else if (operand.IsR4)
			{
				context.AppendInstruction(IRInstruction.StoreR4, null, transform.StackPointer, offsetOperand, operand);
			}
			else if (operand.IsR8)
			{
				context.AppendInstruction(IRInstruction.StoreR8, null, transform.StackPointer, offsetOperand, operand);
			}
			else if (operand.IsValueType)
			{
				context.AppendInstruction(IRInstruction.StoreCompound, null, transform.StackPointer, offsetOperand, operand);
			}
			else
			{
				context.AppendInstruction(transform.StoreInstruction, null, transform.StackPointer, offsetOperand, operand);
			}
		}

		private static void PushOperands(TransformContext transform, Context context, List<Operand> operands, uint offset)
		{
			for (var index = operands.Count - 1; index >= 0; index--)
			{
				var operand = operands[index];
				var size = transform.MethodCompiler.GetSize(operand);

				offset -= Alignment.AlignUp(size, transform.NativePointerSize);

				Push(transform, context, operand, offset);
			}
		}

		private static void ReserveStackSizeForCall(TransformContext transform, Context context, uint stackSize)
		{
			if (stackSize == 0)
				return;

			context.AppendInstruction(transform.SubInstruction, transform.StackPointer, transform.StackPointer, transform.CreateConstant32(stackSize));
		}

		#endregion Helpers
	}
}
