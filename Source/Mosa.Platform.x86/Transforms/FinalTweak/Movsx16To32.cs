// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.FinalTweak
{
	/// <summary>
	/// Movsx16To32
	/// </summary>
	public sealed class Movsx16To32 : BaseTransformation
	{
		public Movsx16To32() : base(X86.Movsx16To32, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsCPURegister)
				return false;

			return !(context.Operand1.Register != CPURegister.ESI && context.Operand1.Register != CPURegister.EDI);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Result.IsCPURegister);

			var result = context.Result;
			var source = context.Operand1;

			// Movsx16To32 can not use with ESI or EDI registers as source registers
			var eax = Operand.CreateCPURegister(transform.I4, CPURegister.EAX);

			if (source.Register == result.Register)
			{
				context.SetInstruction2(X86.XChg32, eax, source, source, eax);
				context.AppendInstruction(X86.Movsx16To32, eax, eax);
				context.AppendInstruction2(X86.XChg32, source, eax, eax, source);
			}
			else
			{
				context.SetInstruction2(X86.XChg32, eax, source, source, eax);
				context.AppendInstruction(X86.Movsx16To32, result, eax);
				context.AppendInstruction2(X86.XChg32, source, eax, eax, source);
			}
		}
	}
}
