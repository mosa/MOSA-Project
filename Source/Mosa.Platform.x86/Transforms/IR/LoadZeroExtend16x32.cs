
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// LoadZeroExtend16x32
	/// </summary>
	public sealed class LoadZeroExtend16x32 : BaseTransform
	{
		public LoadZeroExtend16x32() : base(IRInstruction.LoadZeroExtend16x32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.OrderOperands(context);

			context.SetInstruction(X86.MovzxLoad16, context.Result, context.Operand1, context.Operand2);
		}
	}
}
