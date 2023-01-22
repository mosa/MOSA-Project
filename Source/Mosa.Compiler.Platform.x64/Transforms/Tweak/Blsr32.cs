// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x64.Transforms.Tweak
{
	/// <summary>
	/// Blsr32
	/// </summary>
	public sealed class Blsr32 : BaseTransform
	{
		public Blsr32() : base(X64.Blsr32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			transform.MoveOperand1ToVirtualRegister(context, X64.Mov32);
		}
	}
}
