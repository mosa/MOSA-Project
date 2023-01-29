// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.Tweak;

/// <summary>
/// Setcc
/// </summary>
public sealed class Setcc : BaseTransform
{
	public Setcc() : base(X64.Setcc, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		return context.Result.Register == CPURegister.RSI || context.Result.Register == CPURegister.RDI;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		Debug.Assert(context.Result.IsCPURegister);
		Debug.Assert(context.Result.IsCPURegister);

		var result = context.Result;
		var instruction = context.Instruction;
		var condition = context.ConditionCode;

		// SETcc can not use with RSI or RDI registers

		var rax = Operand.CreateCPURegister(transform.I4, CPURegister.RAX);

		context.SetInstruction2(X64.XChg64, rax, result, result, rax);
		context.AppendInstruction(instruction, condition, rax);
		context.AppendInstruction2(X64.XChg64, result, rax, rax, result);
	}
}
