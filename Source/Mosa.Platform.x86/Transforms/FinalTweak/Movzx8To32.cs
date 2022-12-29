// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.FinalTweak
{
	/// <summary>
	/// Movzx8To32
	/// </summary>
	public sealed class Movzx8To32 : BaseTransformation
	{
		public Movzx8To32() : base(X86.Movzx8To32, TransformationType.Manual | TransformationType.Tranformation)
		{
		}

		public override bool Match(Context context, TransformContext transformContext)
		{
			return !(context.Operand1.Register != CPURegister.ESI && context.Operand1.Register != CPURegister.EDI);
		}

		public override void Transform(Context context, TransformContext transformContext)
		{
			Debug.Assert(context.Result.IsCPURegister);

			var result = context.Result;
			var source = context.Operand1;

			// Movzx8To32 can not use with ESI or EDI registers as source registers
			if (source.Register == result.Register)
			{
				context.SetInstruction(X86.And32, result, result, transformContext.CreateConstant32(0xFF));
			}
			else
			{
				context.SetInstruction(X86.Mov32, result, source);
				context.AppendInstruction(X86.And32, result, result, transformContext.CreateConstant32(0xFF));
			}
		}
	}
}
