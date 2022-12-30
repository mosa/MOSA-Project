
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// StoreParam64
	/// </summary>
	public sealed class StoreParam64 : BaseTransform
	{
		public StoreParam64() : base(IRInstruction.StoreParam64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Operand1, out var op0L, out var op0H);
			transform.SplitLongOperand(context.Operand2, out var op1L, out var op1H);

			context.SetInstruction(X86.MovStore32, null, transform.StackFrame, op0L, op1L);
			context.AppendInstruction(X86.MovStore32, null, transform.StackFrame, op0H, op1H);
		}
	}
}
