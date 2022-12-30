
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// LoadZeroExtend8x32
	/// </summary>
	public sealed class LoadZeroExtend8x32 : BaseTransform
	{
		public LoadZeroExtend8x32() : base(IRInstruction.LoadZeroExtend8x32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.OrderOperands(context);

			context.SetInstruction(X86.MovzxLoad8, context.Result, context.Operand1, context.Operand2);
		}
	}
}
