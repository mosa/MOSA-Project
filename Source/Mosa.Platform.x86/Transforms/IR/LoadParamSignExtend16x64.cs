
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// LoadParamSignExtend16x64
	/// </summary>
	public sealed class LoadParamSignExtend16x64 : BaseTransform
	{
		public LoadParamSignExtend16x64() : base(IRInstruction.LoadParamSignExtend16x64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
			transform.SplitLongOperand(context.Operand1, out var lowOffset, out _);

			context.SetInstruction(X86.MovsxLoad16, resultLow, transform.StackFrame, lowOffset);
			context.AppendInstruction2(X86.Cdq32, resultHigh, resultLow, resultLow);
		}
	}
}
