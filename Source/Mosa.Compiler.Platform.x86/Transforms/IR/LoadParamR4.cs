// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.IR
{
	/// <summary>
	/// LoadParamR4
	/// </summary>
	public sealed class LoadParamR4 : BaseTransform
	{
		public LoadParamR4() : base(IRInstruction.LoadParamR4, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Result.IsR4);

			context.SetInstruction(X86.MovssLoad, context.Result, transform.StackFrame, context.Operand1);
		}
	}
}
