// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// This stage converts high level IR instructions to VM Calls
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCodeTransformationStage" />
	public sealed class RuntimeCallStage : BaseCodeTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
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

			MethodScanner.MethodInvoked(method, Method);

			return method;
		}

		private void SetVMCall(Context context, VmCall vmcall, Operand result, List<Operand> operands)
		{
			var method = GetVMCallMethod(vmcall);
			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);

			context.SetInstruction(IRInstruction.CallStatic, result, symbol, operands);
		}

		private void MemorySet(Context context)
		{
			SetVMCall(context, VmCall.MemorySet, context.Result, context.GetOperands());
		}

		private void IsInstanceOfType(Context context)
		{
			SetVMCall(context, VmCall.IsInstanceOfType, context.Result, context.GetOperands());
		}

		private void IsInstanceOfInterfaceType(Context context)
		{
			SetVMCall(context, VmCall.IsInstanceOfInterfaceType, context.Result, context.GetOperands());
		}

		private void GetVirtualFunctionPtr(Context context)
		{
			SetVMCall(context, VmCall.GetVirtualFunctionPtr, context.Result, context.GetOperands());
		}

		private void Rethrow(Context context)
		{
			SetVMCall(context, VmCall.Rethrow, context.Result, context.GetOperands());
		}

		private void MemoryCopy(Context context)
		{
			SetVMCall(context, VmCall.MemoryCopy, context.Result, context.GetOperands());
		}

		private void Box(Context context)
		{
			SetVMCall(context, VmCall.Box, context.Result, context.GetOperands());
		}

		private void Box32(Context context)
		{
			SetVMCall(context, VmCall.Box32, context.Result, context.GetOperands());
		}

		private void Box64(Context context)
		{
			SetVMCall(context, VmCall.Box64, context.Result, context.GetOperands());
		}

		private void BoxR4(Context context)
		{
			SetVMCall(context, VmCall.BoxR4, context.Result, context.GetOperands());
		}

		private void BoxR8(Context context)
		{
			SetVMCall(context, VmCall.BoxR8, context.Result, context.GetOperands());
		}

		private void Unbox(Context context)
		{
			SetVMCall(context, VmCall.Unbox, context.Result, context.GetOperands());
		}

		private void Unbox32(Context context)
		{
			SetVMCall(context, VmCall.Unbox32, context.Result, context.GetOperands());
		}

		private void Unbox64(Context context)
		{
			SetVMCall(context, VmCall.Unbox64, context.Result, context.GetOperands());
		}
	}
}
