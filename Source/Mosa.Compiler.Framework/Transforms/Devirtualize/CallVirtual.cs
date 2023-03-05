// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Devirtualize;

public sealed class CallVirtual : BaseTransform
{
	public CallVirtual() : base(IRInstruction.CallVirtual, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!transform.Devirtualization)
			return false;

		var method = context.Operand1.Method;

		if (!method.HasImplementation && method.IsAbstract)
			return false;

		var methodData = transform.Compiler.GetMethodData(method);

		if (!methodData.IsDevirtualized)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var method = context.Operand1.Method;

		var operands = context.GetOperands();
		operands.RemoveAt(0);

		var symbol = Operand.CreateSymbolFromMethod(method, transform.TypeSystem);

		context.SetInstruction(IRInstruction.CallStatic, context.Result, symbol, operands);

		transform.MethodScanner.MethodInvoked(method, transform.Method);
	}
}
