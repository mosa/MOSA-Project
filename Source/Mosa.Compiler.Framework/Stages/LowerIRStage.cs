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
	public sealed class LowerIRStage : BaseCodeTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.NewObject, NewObject);
			AddVisitation(IRInstruction.NewArray, NewArray);

			AddVisitation(IRInstruction.MemorySet, MemorySet);
			AddVisitation(IRInstruction.MemoryCopy, MemoryCopy);
			AddVisitation(IRInstruction.IsInstanceOfInterfaceType, IsInstanceOfInterfaceType);
			AddVisitation(IRInstruction.IsInstanceOfType, IsInstanceOfType);
			AddVisitation(IRInstruction.GetVirtualFunctionPtr, GetVirtualFunctionPtr);
			AddVisitation(IRInstruction.Rethrow, Rethrow);

			AddVisitation(IRInstruction.Box, Box);
			AddVisitation(IRInstruction.Box32, Box32);
			AddVisitation(IRInstruction.Box64, Box64);
			AddVisitation(IRInstruction.BoxR4, BoxR4);
			AddVisitation(IRInstruction.BoxR8, BoxR8);
			AddVisitation(IRInstruction.Unbox, Unbox);
			AddVisitation(IRInstruction.Unbox32, Unbox32);
			AddVisitation(IRInstruction.Unbox64, Unbox64);
		}

		private MosaMethod GetVMCallMethod(VmCall vmcall)
		{
			string methodName = vmcall.ToString();

			var method = InternalRuntimeType.FindMethodByName(methodName) ?? PlatformInternalRuntimeType.FindMethodByName(methodName);

			Debug.Assert(method != null, "Cannot find method: " + methodName);

			MethodScanner.MethodInvoked(method, this.Method);

			return method;
		}

		private void SetVMCall(InstructionNode node, VmCall vmcall, Operand result, List<Operand> operands)
		{
			var method = GetVMCallMethod(vmcall);
			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			node.SetInstruction(IRInstruction.CallStatic, result, symbol, operands);
		}

		private void NewObject(InstructionNode node)
		{
			var method = GetVMCallMethod(VmCall.AllocateObject);
			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);
			var classType = node.MosaType;

			node.SetInstruction(IRInstruction.CallStatic, node.Result, symbol, node.GetOperands());

			MethodScanner.TypeAllocated(classType, Method);
		}

		private void NewArray(InstructionNode node)
		{
			var method = GetVMCallMethod(VmCall.AllocateArray);
			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);
			var arrayType = node.MosaType;

			node.SetInstruction(IRInstruction.CallStatic, node.Result, symbol, node.GetOperands());

			MethodScanner.TypeAllocated(arrayType, method);
		}

		private void MemorySet(InstructionNode node)
		{
			SetVMCall(node, VmCall.MemorySet, node.Result, node.GetOperands());
		}

		private void IsInstanceOfType(InstructionNode node)
		{
			SetVMCall(node, VmCall.IsInstanceOfType, node.Result, node.GetOperands());
		}

		private void IsInstanceOfInterfaceType(InstructionNode node)
		{
			SetVMCall(node, VmCall.IsInstanceOfInterfaceType, node.Result, node.GetOperands());
		}

		private void GetVirtualFunctionPtr(InstructionNode node)
		{
			SetVMCall(node, VmCall.GetVirtualFunctionPtr, node.Result, node.GetOperands());
		}

		private void Rethrow(InstructionNode node)
		{
			SetVMCall(node, VmCall.Rethrow, node.Result, node.GetOperands());
		}

		private void MemoryCopy(InstructionNode node)
		{
			SetVMCall(node, VmCall.MemoryCopy, node.Result, node.GetOperands());
		}

		private void Box(InstructionNode node)
		{
			SetVMCall(node, VmCall.Box, node.Result, node.GetOperands());
		}

		private void Box32(InstructionNode node)
		{
			SetVMCall(node, VmCall.Box32, node.Result, node.GetOperands());
		}

		private void Box64(InstructionNode node)
		{
			SetVMCall(node, VmCall.Box64, node.Result, node.GetOperands());
		}

		private void BoxR4(InstructionNode node)
		{
			SetVMCall(node, VmCall.BoxR4, node.Result, node.GetOperands());
		}

		private void BoxR8(InstructionNode node)
		{
			SetVMCall(node, VmCall.BoxR8, node.Result, node.GetOperands());
		}

		private void Unbox(InstructionNode node)
		{
			SetVMCall(node, VmCall.Unbox, node.Result, node.GetOperands());
		}

		private void Unbox32(InstructionNode node)
		{
			SetVMCall(node, VmCall.Unbox32, node.Result, node.GetOperands());
		}

		private void Unbox64(InstructionNode node)
		{
			SetVMCall(node, VmCall.Unbox64, node.Result, node.GetOperands());
		}
	}
}
