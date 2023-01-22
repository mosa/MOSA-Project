// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// Store8
	/// </summary>
	public sealed class Store8 : BaseTransform
	{
		public Store8() : base(IRInstruction.Store8, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			ARMv8A32TransformHelper.TransformStore(transform, context, ARMv8A32.Str8, context.Operand1, context.Operand2, context.Operand3);
		}
	}
}
