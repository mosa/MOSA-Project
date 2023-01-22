// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x64.Transforms.Tweak
{
	/// <summary>
	/// Movzx16To64
	/// </summary>
	public sealed class Movzx16To64 : BaseTransform
	{
		public Movzx16To64() : base(X64.Movzx16To64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return !(context.Operand1.Register != CPURegister.RSI && context.Operand1.Register != CPURegister.RDI);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Result.IsCPURegister);

			// Movzx8To64 can not use with RSI or RDI registers
			var result = context.Result;
			var source = context.Operand1;

			if (source.Register != result.Register)
			{
				context.SetInstruction(X64.Mov64, result, source);
				context.AppendInstruction(X64.And64, result, result, transform.CreateConstant32(0xFFFF));
			}
			else
			{
				context.SetInstruction(X64.And64, result, result, transform.CreateConstant32(0xFFFF));
			}
		}
	}
}
