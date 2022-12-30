
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// LoadParamZeroExtend16x32
	/// </summary>
	public sealed class LoadParamZeroExtend16x32 : BaseTransformation
	{
		public LoadParamZeroExtend16x32() : base(IRInstruction.LoadParamZeroExtend16x32, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.SetInstruction(X86.MovzxLoad16, context.Result, transform.StackFrame, context.Operand1);
		}
	}
}
