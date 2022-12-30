
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// Move64
	/// </summary>
	public sealed class Move64 : BaseTransformation
	{
		public Move64() : base(IRInstruction.Move64, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			transform.SplitLongOperand(context.Operand1, out var op1L, out var op1H);

			context.SetInstruction(X86.Mov32, resultLow, op1L);
			context.AppendInstruction(X86.Mov32, resultHigh, op1H);
		}
	}
}
