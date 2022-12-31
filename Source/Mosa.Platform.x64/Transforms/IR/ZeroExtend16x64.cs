// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// ZeroExtend16x64
	/// </summary>
	public sealed class ZeroExtend16x64 : BaseTransform
	{
		public ZeroExtend16x64() : base(IRInstruction.ZeroExtend16x64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.ReplaceInstruction(X64.Movzx16To64);
		}
	}
}
