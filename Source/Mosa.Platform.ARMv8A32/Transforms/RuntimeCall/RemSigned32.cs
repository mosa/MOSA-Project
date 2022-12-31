// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.RuntimeCall
{
	/// <summary>
	/// RemSigned32
	/// </summary>
	public sealed class RemSigned32 : BaseTransform
	{
		public RemSigned32() : base(IRInstruction.RemSigned32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Division", "smod32");
		}
	}
}
