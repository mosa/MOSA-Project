// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// StoreR4
	/// </summary>
	public sealed class StoreR4 : BaseTransform
	{
		public StoreR4() : base(IRInstruction.StoreR4, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			ARMv8A32TransformHelper.TransformStore(transform, context, ARMv8A32.Stf, context.Operand1, context.Operand2, context.Operand3);
		}
	}
}
