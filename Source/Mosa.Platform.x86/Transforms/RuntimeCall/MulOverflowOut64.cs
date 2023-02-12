// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.RuntimeCall;

/// <summary>
/// MulOverflowOut64
/// </summary>
public sealed class MulOverflowOut64 : BaseTransform
{
	public MulOverflowOut64() : base(IRInstruction.MulOverflowOut64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var methodName = "Mul64Overflow";
		var method = transform.GetMethod("Mosa.Runtime.Math.Multiplication", methodName);

		var operand1 = context.Operand1;
		var operand2 = context.Operand2;
		var result = context.Result;
		var result2 = context.Result2;

		var v1 = transform.MethodCompiler.AddStackLocal(result2.Type); // Review
		var v2 = transform.AllocateVirtualRegister32();

		Debug.Assert(method != null, $"Cannot find method: {methodName}");

		var symbol = Operand.CreateSymbolFromMethod(method, transform.TypeSystem);

		context.SetInstruction(IRInstruction.AddressOf, v2, v1);
		context.AppendInstruction(IRInstruction.CallStatic, result, symbol, operand1, operand2, v2);
		context.AppendInstruction(IRInstruction.LoadZeroExtend8x32, result2, v2, transform.Constant32_0);

		transform.MethodCompiler.MethodScanner.MethodInvoked(method, transform.MethodCompiler.Method);
	}
}
