// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Transforms
{
	public static class CallingConventionTransformHelper
	{
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

				var size = GetTypeSize(transform, operand.Type, true);
				stackSize += size;
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

			var returnType = result.Type;

			if (!MosaTypeLayout.CanFitInRegister(returnType))
			{
				return Alignment.AlignUp(transform.TypeLayout.GetTypeSize(returnType), transform.Architecture.NativeAlignment);
			}

			return 0;
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

			if (result.IsReferenceType)
			{
				var returnLow = Operand.CreateCPURegister(result.Type, transform.Architecture.ReturnRegister);
				context.AppendInstruction(IRInstruction.Gen, returnLow);
				context.AppendInstruction(IRInstruction.MoveObject, result, returnLow);
			}
			else if (result.IsInteger64 && transform.Is32BitPlatform)
			{
				var returnLow = Operand.CreateCPURegister(result.Type, transform.Architecture.ReturnRegister);
				var returnHigh = Operand.CreateCPURegister(transform.I4, transform.Architecture.ReturnHighRegister);

				context.AppendInstruction(IRInstruction.Gen, returnLow);
				context.AppendInstruction(IRInstruction.Gen, returnHigh);
				context.AppendInstruction(IRInstruction.To64, result, returnLow, returnHigh);
			}
			else if (result.IsInteger)
			{
				var returnLow = Operand.CreateCPURegister(result.Type, transform.Architecture.ReturnRegister);
				context.AppendInstruction(IRInstruction.Gen, returnLow);
				context.AppendInstruction(transform.MoveInstruction, result, returnLow);
			}
			else if (result.IsR4)
			{
				var returnFP = Operand.CreateCPURegister(result.Type, transform.Architecture.ReturnFloatingPointRegister);
				context.AppendInstruction(IRInstruction.Gen, returnFP);
				context.AppendInstruction(IRInstruction.MoveR4, result, returnFP);
			}
			else if (result.IsR8)
			{
				var returnFP = Operand.CreateCPURegister(result.Type, transform.Architecture.ReturnFloatingPointRegister);
				context.AppendInstruction(IRInstruction.Gen, returnFP);
				context.AppendInstruction(IRInstruction.MoveR8, result, returnFP);
			}
			else if (!MosaTypeLayout.CanFitInRegister(result.Type))
			{
				Debug.Assert(result.IsStackLocal);
				context.AppendInstruction(IRInstruction.LoadCompound, result, transform.StackPointer, transform.Constant32_0);
			}
			else
			{
				// note: same for integer logic (above)
				var returnLow = Operand.CreateCPURegister(result.Type, transform.Architecture.ReturnRegister);
				context.AppendInstruction(IRInstruction.Gen, returnLow);
				context.AppendInstruction(transform.MoveInstruction, result, returnLow);
			}
		}

		/// <summary>
		/// Gets the size of the type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="align">if set to <c>true</c> [align].</param>
		/// <returns></returns>
		private static uint GetTypeSize(TransformContext transform, MosaType type, bool align)
		{
			return transform.MethodCompiler.GetReferenceOrTypeSize(type, align);
		}

		private static void Push(TransformContext transform, Context context, Operand operand, uint offset)
		{
			var offsetOperand = transform.CreateConstant32(offset);

			if (operand.IsReferenceType)
			{
				context.AppendInstruction(IRInstruction.StoreObject, null, transform.StackPointer, offsetOperand, operand);
			}
			else if (operand.IsInteger32)
			{
				context.AppendInstruction(IRInstruction.Store32, null, transform.StackPointer, offsetOperand, operand);
			}
			else if (operand.IsInteger64)
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
			else if (!MosaTypeLayout.CanFitInRegister(operand.Type))
			{
				context.AppendInstruction(IRInstruction.StoreCompound, null, transform.StackPointer, offsetOperand, operand);
			}
			else
			{
				context.AppendInstruction(transform.StoreInstruction, null, transform.StackPointer, offsetOperand, operand);
			}
		}

		private static void PushOperands(TransformContext transform, Context context, List<Operand> operands, uint space)
		{
			for (var index = operands.Count - 1; index >= 0; index--)
			{
				var operand = operands[index];

				var size = GetTypeSize(transform, operand.Type, true);
				space -= size;

				Push(transform, context, operand, space);
			}
		}

		private static void ReserveStackSizeForCall(TransformContext transform, Context context, uint stackSize)
		{
			if (stackSize == 0)
				return;

			context.AppendInstruction(transform.SubInstruction, transform.StackPointer, transform.StackPointer, transform.CreateConstant32(stackSize));
		}
	}
}
