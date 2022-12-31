// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// AddR4
	/// </summary>
	public sealed class AddR4 : BaseTransform
	{
		public AddR4() : base(IRInstruction.AddR4, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.MoveConstantRight(context);

			ARMv8A32TransformHelper.Translate(transform, context, ARMv8A32.Adf, true);
		}
	}
}
