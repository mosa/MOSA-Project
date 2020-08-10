// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.MosaTypeSystem;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// New Object Stage
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCodeTransformationStage" />
	public sealed class NewObjectStage : BaseCodeTransformationStage
	{
		protected override void PopulateVisitationDictionary()
		{
			AddVisitation(IRInstruction.NewObject, NewObject);
			AddVisitation(IRInstruction.NewArray, NewArray);
		}

		private MosaMethod GetVMCallMethod(VmCall vmcall)
		{
			string methodName = vmcall.ToString();

			var method = InternalRuntimeType.FindMethodByName(methodName) ?? PlatformInternalRuntimeType.FindMethodByName(methodName);

			Debug.Assert(method != null, "Cannot find method: " + methodName);

			MethodScanner.MethodInvoked(method, Method);

			return method;
		}

		private void NewObject(Context context)
		{
			var method = GetVMCallMethod(VmCall.AllocateObject);
			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);
			var classType = context.MosaType;

			context.SetInstruction(IRInstruction.CallStatic, context.Result, symbol, context.GetOperands());

			MethodScanner.TypeAllocated(classType, Method);
		}

		private void NewArray(Context context)
		{
			var method = GetVMCallMethod(VmCall.AllocateArray);
			var symbol = Operand.CreateSymbolFromMethod(method, TypeSystem);
			var arrayType = context.MosaType;

			context.SetInstruction(IRInstruction.CallStatic, context.Result, symbol, context.GetOperands());

			MethodScanner.TypeAllocated(arrayType, method);
		}
	}
}
