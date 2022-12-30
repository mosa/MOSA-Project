
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// LoadParamSignExtend8x32
	/// </summary>
	public sealed class LoadParamSignExtend8x32 : BaseTransformation
	{
		public LoadParamSignExtend8x32() : base(IRInstruction.LoadParamSignExtend8x32, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(X86.MovsxLoad8, context.Result, transform.StackFrame, context.Operand1);
		}
	}
}
