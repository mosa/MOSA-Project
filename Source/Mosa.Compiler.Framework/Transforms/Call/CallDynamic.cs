// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms.Call;

namespace Mosa.Platform.Framework.Call;

/// <summary>
/// CallDynamic
/// </summary>
public sealed class CallDynamic : BasePlugTransform
{
	public CallDynamic() : base(IRInstruction.CallDynamic, TransformType.Manual | TransformType.Transform)
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
		var method = context.InvokeMethod;
		var operands = context.GetOperands();

		operands.RemoveAt(0);
		context.Empty();

		MakeCall(transform, context, call, result, operands, method);

		transform.MethodScanner.MethodInvoked(call.Method, method);
	}
}
