// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Call
{
	/// <summary>
	/// CallVirtual
	/// </summary>
	public sealed class CallVirtual : BaseTransform
	{
		public CallVirtual() : base(IRInstruction.CallVirtual, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
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
			var typeDefOffset = transform.CreateConstant32(-transform.NativePointerSize);

			// Offset to the method pointer on the MethodTable
			var methodPointerOffset = transform.CreateConstant32(CallingConvention.CalculateMethodTableOffset(transform, method));

			var typeDef = transform.AllocateVirtualRegister(transform.TypeSystem.BuiltIn.Pointer);
			var callTarget = transform.AllocateVirtualRegister(transform.TypeSystem.BuiltIn.Pointer);

			// Get the Method Table pointer
			context.SetInstruction(transform.LoadInstruction, typeDef, thisPtr, typeDefOffset);

			// Get the address of the method
			context.AppendInstruction(transform.LoadInstruction, callTarget, typeDef, methodPointerOffset);

			CallingConvention.MakeCall(transform, context, callTarget, result, operands, method);

			transform.MethodCompiler.MethodScanner.MethodInvoked(method, transform.Method);
		}
	}
}
