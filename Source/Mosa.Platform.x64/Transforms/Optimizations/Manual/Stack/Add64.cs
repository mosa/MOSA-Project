// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.Optimizations.Manual.Stack;

/// <summary>
/// Add32
/// </summary>
[Transform("x64.Optimizations.Manual.Stack")]
public sealed class Add64 : BaseTransform
{
	public Add64() : base(X64.Add64, TransformType.Manual | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsCPURegister)
			return false;

		if (context.Operand1.Register != CPURegister.RSP)
			return false;

		if (!context.Operand2.IsConstant)
			return false;

		var next = context.Node.NextNonEmpty;

		if (next == null)
			return false;

		if (next.Instruction != X64.Sub64)
			return false;

		if (context.Operand1.Register != CPURegister.RSP)
			return false;

		if (!next.Operand2.IsConstant)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var next = context.Node.NextNonEmpty;

		var value1 = context.Operand2.ConstantSigned32;
		var value2 = next.Operand2.ConstantSigned32;

		var value = value1 - value2;

		next.SetNop();

		if (value > 0)
		{
			context.SetInstruction(X64.Add64, context.Result, context.Operand1, transform.CreateConstant(value));
		}
		else if (value < 0)
		{
			context.SetInstruction(X64.Sub64, context.Result, context.Operand1, transform.CreateConstant(-value));
		}
		else // if (value == 0)
		{
			Debug.Assert(value == 0);
			context.SetNop();
		}
	}
}
