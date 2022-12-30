
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// ConvertI64ToR4
	/// </summary>
	public sealed class ConvertI64ToR4 : BaseTransformation
	{
		public ConvertI64ToR4() : base(IRInstruction.ConvertI64ToR4, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.SplitLongOperand(context.Result, out var op1Low, out _);

			context.SetInstruction(X86.Cvtsi2ss32, context.Result, op1Low);
		}
	}
}
