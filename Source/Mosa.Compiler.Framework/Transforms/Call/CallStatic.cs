// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.Framework.Call
{
	/// <summary>
	/// CallStatic
	/// </summary>
	public sealed class CallStatic : BaseTransform
	{
		public CallStatic() : base(IRInstruction.CallStatic, TransformType.Manual | TransformType.Transform)
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
			var operands = context.GetOperands();

			Debug.Assert(method != null);

			operands.RemoveAt(0);
			context.Empty();

			CallingConvention.MakeCall(transform, context, call, result, operands, method);

			transform.MethodScanner.MethodDirectInvoked(call.Method, transform.Method);
		}
	}
}