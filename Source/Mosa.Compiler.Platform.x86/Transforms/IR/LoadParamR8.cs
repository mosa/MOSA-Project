// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.IR
{
	/// <summary>
	/// LoadParamR8
	/// </summary>
	public sealed class LoadParamR8 : BaseTransform
	{
		public LoadParamR8() : base(IRInstruction.LoadParamR8, TransformType.Manual | TransformType.Transform)
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
