
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// GetHigh32
	/// </summary>
	public sealed class GetHigh32 : BaseTransformation
	{
		public GetHigh32() : base(IRInstruction.GetHigh32, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Operand1, out var _, out var op0H);

			context.SetInstruction(X86.Mov32, context.Result, op0H);
		}
	}
}
