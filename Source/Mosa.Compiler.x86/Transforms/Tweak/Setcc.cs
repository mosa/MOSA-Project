// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.Tweak;

/// <summary>
/// Setcc
/// </summary>
public sealed class Setcc : BaseTransform
{
	public Setcc() : base(X86.Setcc, TransformType.Manual | TransformType.Transform)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Result.IsPhysicalRegister)
			return false;

		return context.Result.Register == CPURegister.ESI || context.Result.Register == CPURegister.EDI;
	}

	public override void Transform(Context context, Transform transform)
	{
		Debug.Assert(context.Result.IsPhysicalRegister);
		Debug.Assert(context.Result.IsPhysicalRegister);

		var result = context.Result;
		var instruction = context.Instruction;

		// SETcc can not use with ESI or EDI registers as source registers
		var condition = context.ConditionCode;

		var eax = transform.PhysicalRegisters.Allocate32(CPURegister.EAX);

		context.SetInstruction2(X86.XChg32, eax, result, result, eax);
		context.AppendInstruction(instruction, condition, eax);
		context.AppendInstruction2(X86.XChg32, result, eax, eax, result);
	}
}
