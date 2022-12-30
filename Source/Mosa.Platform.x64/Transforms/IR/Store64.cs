
using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// Store64
	/// </summary>
	public sealed class Store64 : BaseTransform
	{
		public Store64() : base(IRInstruction.Store64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.OrderOperands(context);

			context.SetInstruction(X64.MovStore64, null, context.Operand1, context.Operand2, context.Operand3);
		}
	}
}
