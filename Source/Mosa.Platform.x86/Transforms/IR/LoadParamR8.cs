
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// LoadParamR8
	/// </summary>
	public sealed class LoadParamR8 : BaseTransformation
	{
		public LoadParamR8() : base(IRInstruction.LoadParamR8, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Result.IsR8);

			context.SetInstruction(X86.MovsdLoad, context.Result, transform.StackFrame, context.Operand1);
		}
	}
}
