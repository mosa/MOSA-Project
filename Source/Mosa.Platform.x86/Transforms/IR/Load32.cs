
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// Load32
	/// </summary>
	public sealed class Load32 : BaseTransformation
	{
		public Load32() : base(IRInstruction.Load32, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(!context.Result.IsR4);
			Debug.Assert(!context.Result.IsR8);

			transform.OrderOperands(context);

			context.SetInstruction(X86.MovLoad32, context.Result, context.Operand1, context.Operand2);
		}
	}
}
