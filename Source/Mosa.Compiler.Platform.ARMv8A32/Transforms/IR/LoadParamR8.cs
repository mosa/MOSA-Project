// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.ARMv8A32.Transforms.IR
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
			Debug.Assert(context.Operand1.IsConstant);

			ARMv8A32TransformHelper.TransformLoad(transform, context, ARMv8A32.Ldf, context.Result, transform.StackFrame, context.Operand1);
		}
	}
}
