// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
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
			AddVisitation(IRInstruction.BoxR4, BoxR8);
			AddVisitation(IRInstruction.Unbox, Unbox);
			AddVisitation(IRInstruction.Unbox32, Unbox32);
			AddVisitation(IRInstruction.Unbox64, Unbox64);
		}

		private MosaMethod GetVMCallMethod(VmCall vmcall)
		{
			string methodName = vmcall.ToString();

			var method = InternalRuntimeType.FindMethodByName(methodName) ?? PlatformInternalRuntimeType.FindMethodByName(methodName);

			Debug.Assert(method != null, "Cannot find method: " + methodName);

			return method;
		}

		private void NewObject(InstructionNode node)
		{
			var method = GetVMCallMethod(VmCall.AllocateObject);
			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			var runtimeHandle = node.Operand1;
			var size = node.Operand2;
			var result = node.Result;

			node.SetInstruction(IRInstruction.CallStatic, result, symbol, runtimeHandle, size);
			node.InvokeMethod = method;
		}

		private void NewArray(InstructionNode node)
		{
			var method = GetVMCallMethod(VmCall.AllocateArray);
			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			var runtimeHandle = node.Operand1;
			var size = node.Operand2;
			var elements = node.Operand3;
			var result = node.Result;

			node.SetInstruction(IRInstruction.CallStatic, result, symbol, runtimeHandle, size, elements);
			node.InvokeMethod = method;
		}

		private void MemorySet(InstructionNode node)
		{
			var method = GetVMCallMethod(VmCall.MemorySet);
			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			//var runtimeHandle = node.Operand1;
			//var size = node.Operand2;
			//var elements = node.Operand3;
			//var result = node.Result;

			// todo

			//node.SetOperand(1, ptr);
			//node.SetOperand(2, ConstantZero);
			//node.SetOperand(3, Operand.CreateConstant(TypeSystem, TypeLayout.GetTypeSize(type)));
		}

		private void IsInstanceOfType(InstructionNode node)
		{
			var method = GetVMCallMethod(VmCall.IsInstanceOfType);
			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			// todo
			//node.SetOperand(1, GetRuntimeTypeHandle(classType));
			//node.SetOperand(2, reference);
		}

		private void IsInstanceOfInterfaceType(InstructionNode node)
		{
			var method = GetVMCallMethod(VmCall.IsInstanceOfInterfaceType);
			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			// todo
			//node.SetOperand(1, Operand.CreateConstant(TypeSystem, slot));
			//node.SetOperand(2, reference);
		}

		private void GetVirtualFunctionPtr(InstructionNode node)
		{
			var method = GetVMCallMethod(VmCall.GetVirtualFunctionPtr);
			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			// TODO
		}

		private void Rethrow(InstructionNode node)
		{
			var method = GetVMCallMethod(VmCall.Rethrow);
			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			// TODO
		}

		private void MemoryCopy(InstructionNode node)
		{
			var method = GetVMCallMethod(VmCall.MemoryCopy);
			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			// TODO
		}

		private void Box(InstructionNode node)
		{
			var method = GetVMCallMethod(VmCall.Box);
			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			// TODO
		}

		private void Box32(InstructionNode node)
		{
			var method = GetVMCallMethod(VmCall.Box32);
			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			// TODO
		}

		private void Box64(InstructionNode node)
		{
			var method = GetVMCallMethod(VmCall.Box64);
			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			// TODO
		}

		private void BoxR4(InstructionNode node)
		{
			var method = GetVMCallMethod(VmCall.BoxR4);
			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			// TODO
		}

		private void BoxR8(InstructionNode node)
		{
			var method = GetVMCallMethod(VmCall.BoxR8);
			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			// TODO
		}

		private void Unbox(InstructionNode node)
		{
			var method = GetVMCallMethod(VmCall.Unbox);
			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			// TODO
		}

		private void Unbox32(InstructionNode node)
		{
			var method = GetVMCallMethod(VmCall.Unbox32);
			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			// TODO
		}

		private void Unbox64(InstructionNode node)
		{
			var method = GetVMCallMethod(VmCall.Unbox64);
			var symbol = Operand.CreateSymbolFromMethod(TypeSystem, method);

			// TODO
		}
	}
}
