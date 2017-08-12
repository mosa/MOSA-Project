// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Lower IR Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCodeTransformationStage" />
	public sealed class LowerCallsStage : BaseCodeTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.CallInterface, CallInterface);
			AddVisitation(IRInstruction.CallStatic, CallStatic);
			AddVisitation(IRInstruction.CallVirtual, CallVirtual);
		}

		private int CalculateMethodTableOffset(MosaMethod invokeTarget)
		{
			int slot = TypeLayout.GetMethodTableOffset(invokeTarget);

			return NativePointerSize * slot;
		}

		private void CallStatic(InstructionNode node)
		{
		}

		private void CallVirtual(InstructionNode node)
		{
			var method = node.InvokeMethod;
			var thisPtr = node.Operand1;
			var resultOperand = node.Result;
			var operands = new List<Operand>(node.Operands);

			var typeDefinition = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);
			var methodPtr = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);

			Debug.Assert(!method.DeclaringType.IsInterface);

			var context = new Context(node);

			// Same as above except for methodPointer
			int methodPointerOffset = CalculateMethodTableOffset(method) + (NativePointerSize * 14);

			// Get the TypeDef pointer
			context.SetInstruction(IRInstruction.LoadInteger, NativeInstructionSize, typeDefinition, thisPtr, ConstantZero);

			// Get the address of the method
			context.AppendInstruction(IRInstruction.LoadInteger, NativeInstructionSize, methodPtr, typeDefinition, Operand.CreateConstant(TypeSystem, methodPointerOffset));

			ProcessInvokeInstruction(context, method, methodPtr, resultOperand, operands);
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
			var method = node.InvokeMethod;
			var thisPtr = node.Operand1;
			var resultOperand = node.Result;
			var operands = new List<Operand>(node.Operands);

			var typeDefinition = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);
			var methodPtr = AllocateVirtualRegister(TypeSystem.BuiltIn.Pointer);

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
			context.AppendInstruction(IRInstruction.LoadInteger, NativeInstructionSize, interfaceSlotPtr, typeDefinition, Operand.CreateConstant(TypeSystem, interfaceSlotTableOffset));

			// Get the Interface Method Table pointer
			context.AppendInstruction(IRInstruction.LoadInteger, NativeInstructionSize, interfaceMethodTablePtr, interfaceSlotPtr, Operand.CreateConstant(TypeSystem, interfaceMethodTableOffset));

			// Get the MethodDef pointer
			context.AppendInstruction(IRInstruction.LoadInteger, NativeInstructionSize, methodDefinition, interfaceMethodTablePtr, Operand.CreateConstant(TypeSystem, methodDefinitionOffset));

			// Get the address of the method
			context.AppendInstruction(IRInstruction.LoadInteger, NativeInstructionSize, methodPtr, methodDefinition, Operand.CreateConstant(TypeSystem, methodPointerOffset));

			ProcessInvokeInstruction(context, method, methodPtr, resultOperand, operands);
		}

		private void ProcessInvokeInstruction(Context context, MosaMethod method, Operand symbolOperand, Operand resultOperand, List<Operand> operands)
		{
			Debug.Assert(method != null);

			context.AppendInstruction(IRInstruction.Call, (byte)(operands.Count + 1), (byte)(resultOperand == null ? 0 : 1));
			context.InvokeMethod = method;

			if (resultOperand != null)
			{
				context.Result = resultOperand;
			}

			int index = 0;
			context.SetOperand(index++, symbolOperand);
			foreach (var operand in operands)
			{
				context.SetOperand(index++, operand);
			}
		}
	}
}
