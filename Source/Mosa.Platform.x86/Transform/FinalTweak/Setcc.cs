// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transform;

namespace Mosa.Platform.x86.Transform.FinalTweak
{
	/// <summary>
	/// Setcc
	/// </summary>
	public sealed class Setcc : BaseTransformation
	{
		public Setcc() : base(X86.Setcc, TransformationType.Manual | TransformationType.Tranformation)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return context.Result.Register == CPURegister.ESI || context.Result.Register == CPURegister.EDI;
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			Debug.Assert(context.Result.IsCPURegister);
			Debug.Assert(context.Result.IsCPURegister);

			var result = context.Result;
			var instruction = context.Instruction;

			// SETcc can not use with ESI or EDI registers as source registers
			var condition = context.ConditionCode;

			var eax = Operand.CreateCPURegister(transformContext.I4, CPURegister.EAX);

			context.SetInstruction2(X86.XChg32, eax, result, result, eax);
			context.AppendInstruction(instruction, condition, eax);
			context.AppendInstruction2(X86.XChg32, result, eax, eax, result);
		}
	}
}
