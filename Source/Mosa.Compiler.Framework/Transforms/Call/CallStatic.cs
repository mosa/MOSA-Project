// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework.Transforms.Call;

/// <summary>
/// CallStatic
/// </summary>
public sealed class CallStatic : BasePlugTransform
{
	public CallStatic() : base(IR.CallStatic, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var call = context.Operand1;
		var result = context.Result;
		var method = call.Method;
		var operands = context.GetOperands();

		Debug.Assert(method != null);

		operands.RemoveAt(0);
		context.Empty();

		MakeCall(transform, context, call, result, operands);

		transform.MethodScanner.MethodDirectInvoked(call.Method, transform.Method);
	}
}
