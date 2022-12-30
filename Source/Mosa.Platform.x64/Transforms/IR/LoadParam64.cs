
using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// LoadParam64
	/// </summary>
	public sealed class LoadParam64 : BaseTransform
	{
		public LoadParam64() : base(IRInstruction.LoadParam64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(X64.MovLoad64, context.Result, transform.StackFrame, context.Operand1);
		}
	}
}
