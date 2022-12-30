
using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// StoreParam16
	/// </summary>
	public sealed class StoreParam16 : BaseTransform
	{
		public StoreParam16() : base(IRInstruction.StoreParam16, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(X64.MovStore16, null, transform.StackFrame, context.Operand1, context.Operand2);
		}
	}
}
