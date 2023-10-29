// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Tweak;

/// <summary>
/// Setcc
/// </summary>
[Transform("x64.Tweak")]
public sealed class Setcc : BaseTransform
{
	public Setcc() : base(X64.Setcc, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return context.Result.Register == CPURegister.RSI || context.Result.Register == CPURegister.RDI;
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsPhysicalRegister);
		Debug.Assert(context.Result.IsPhysicalRegister);

		var result = context.Result;
		var instruction = context.Instruction;
		var condition = context.ConditionCode;

		// SETcc can not use with RSI or RDI registers

		var rax = transform.PhysicalRegisters.Allocate32(CPURegister.RAX);

		context.SetInstruction2(X64.XChg64, rax, result, result, rax);
		context.AppendInstruction(instruction, condition, rax);
		context.AppendInstruction2(X64.XChg64, result, rax, rax, result);
	}
}
