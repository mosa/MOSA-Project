
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// LoadParamZeroExtend8x32
	/// </summary>
	public sealed class LoadParamZeroExtend8x32 : BaseTransformation
	{
		public LoadParamZeroExtend8x32() : base(IRInstruction.LoadParamZeroExtend8x32, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(X86.MovzxLoad8, context.Result, transform.StackFrame, context.Operand1);
		}
	}
}
