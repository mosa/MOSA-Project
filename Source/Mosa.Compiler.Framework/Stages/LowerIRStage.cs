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
			AddVisitation(IRInstruction.CallVirtual, CallVirtual);
			AddVisitation(IRInstruction.CallStatic, CallStatic);
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

			node.SetInstruction(IRInstruction.Call, result, symbol, runtimeHandle, size);
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

			node.SetInstruction(IRInstruction.Call, result, symbol, runtimeHandle, size, elements);
			node.InvokeMethod = method;
		}

		private void CallVirtual(InstructionNode node)
		{
		}

		private void CallStatic(InstructionNode node)
		{
		}
	}
}
