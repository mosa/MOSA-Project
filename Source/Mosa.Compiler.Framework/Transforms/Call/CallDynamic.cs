// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Call;

/// <summary>
/// CallDynamic
/// </summary>
public sealed class CallDynamic : BasePlugTransform
{
	public CallDynamic() : base(IRInstruction.CallDynamic, TransformType.Manual | TransformType.Transform)
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
		var operands = context.GetOperands();

		operands.RemoveAt(0);
		context.Empty();

		MakeCall(transform, context, call, result, operands);

		//transform.MethodScanner.MethodInvoked(call.Method, method);
	}
}
