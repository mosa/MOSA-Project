
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// LoadSignExtend16x32
	/// </summary>
	public sealed class LoadSignExtend16x32 : BaseTransformation
	{
		public LoadSignExtend16x32() : base(IRInstruction.LoadSignExtend16x32, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.OrderOperands(context);

			context.SetInstruction(X86.MovsxLoad16, context.Result, context.Operand1, context.Operand2);
		}
	}
}
