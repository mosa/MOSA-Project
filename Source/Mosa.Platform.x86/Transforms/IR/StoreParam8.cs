
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// StoreParam8
	/// </summary>
	public sealed class StoreParam8 : BaseTransform
	{
		public StoreParam8() : base(IRInstruction.StoreParam8, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(X86.MovStore8, null, transform.StackFrame, context.Operand1, context.Operand2);
		}
	}
}
