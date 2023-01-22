// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.IR
{
	/// <summary>
	/// ZeroExtend8x32
	/// </summary>
	public sealed class ZeroExtend8x32 : BaseTransform
	{
		public ZeroExtend8x32() : base(IRInstruction.ZeroExtend8x32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.ReplaceInstruction(X86.Movzx8To32);
		}
	}
}
