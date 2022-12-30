
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// Store64
	/// </summary>
	public sealed class Store64 : BaseTransformation
	{
		public Store64() : base(IRInstruction.Store64, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Operand2, out var op2L, out _);
			transform.SplitLongOperand(context.Operand3, out var op3L, out var op3H);

			var address = context.Operand1;
			var offset = context.Operand2;

			context.SetInstruction(X86.MovStore32, null, address, op2L, op3L);

			if (offset.IsResolvedConstant)
			{
				context.AppendInstruction(X86.MovStore32, null, address, transform.CreateConstant32(offset.Offset + transform.NativePointerSize), op3H);
			}
			else
			{
				var offset4 = transform.AllocateVirtualRegister32();

				context.AppendInstruction(X86.Add32, offset4, op2L, transform.Constant32_4);
				context.AppendInstruction(X86.MovStore32, null, address, offset4, op3H);
			}
		}
	}
}
