// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.Tweak
{
	/// <summary>
	/// Cmp32
	/// </summary>
	public sealed class Cmp32 : BaseTransform
	{
		public Cmp32() : base(X64.Cmp32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var left = context.Operand1;

			if (left.IsConstant)
			{
				var v1 = transform.AllocateVirtualRegister(left.Type);

				context.InsertBefore().AppendInstruction(X64.Mov32, v1, left);
				context.Operand1 = v1;
			}
		}
	}
}
