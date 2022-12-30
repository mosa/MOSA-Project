
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// Load64
	/// </summary>
	public sealed class Load64 : BaseTransformation
	{
		public Load64() : base(IRInstruction.Load64, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);

			var address = context.Operand1;
			var offset = context.Operand2;

			context.SetInstruction(X86.MovLoad32, resultLow, address, offset);

			if (offset.IsResolvedConstant)
			{
				var offset2 = offset.IsConstantZero ? transform.Constant32_4 : transform.CreateConstant32(offset.Offset + transform.NativePointerSize);
				context.AppendInstruction(X86.MovLoad32, resultHigh, address, offset2);
				return;
			}

			transform.SplitLongOperand(offset, out var op2L, out _);

			var v1 = transform.AllocateVirtualRegister32();

			context.AppendInstruction(X86.Add32, v1, op2L, transform.Constant32_4);
			context.AppendInstruction(X86.MovLoad32, resultHigh, address, v1);
		}
	}
}
