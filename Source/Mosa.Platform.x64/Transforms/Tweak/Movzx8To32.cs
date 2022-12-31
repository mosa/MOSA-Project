using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.Tweak
{
	/// <summary>
	/// Movzx8To32
	/// </summary>
	public sealed class Movzx8To32 : BaseTransform
	{
		public Movzx8To32() : base(X64.Movzx8To32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return !(context.Operand1.Register != CPURegister.RSI && context.Operand1.Register != CPURegister.RDI);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Result.IsCPURegister);

			// Movzx8To32 can not use with RSI or RDI registers
			var result = context.Result;
			var source = context.Operand1;

			if (source.Register != result.Register)
			{
				context.SetInstruction(X64.Mov32, result, source);
				context.AppendInstruction(X64.And32, result, result, transform.CreateConstant32(0xFF));
			}
			else
			{
				context.SetInstruction(X64.And32, result, result, transform.CreateConstant32(0xFF));
			}
		}
	}
}
