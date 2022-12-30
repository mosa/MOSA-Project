
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// LoadSignExtend8x32
	/// </summary>
	public sealed class LoadSignExtend8x32 : BaseTransformation
	{
		public LoadSignExtend8x32() : base(IRInstruction.LoadSignExtend8x32, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.OrderOperands(context);

			context.SetInstruction(X86.MovsxLoad8, context.Result, context.Operand1, context.Operand2);
		}
	}
}
