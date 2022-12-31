// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.RuntimeCall
{
	/// <summary>
	/// DivSigned32
	/// </summary>
	public sealed class DivSigned32 : BaseTransform
	{
		public DivSigned32() : base(IRInstruction.DivSigned32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.ReplaceWithCall(context, "Mosa.Runtime.Math.Division", "sdiv32");
		}
	}
}
