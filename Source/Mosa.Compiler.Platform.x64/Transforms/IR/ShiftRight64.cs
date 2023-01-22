// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x64.Transforms.IR
{
	/// <summary>
	/// ShiftRight64
	/// </summary>
	public sealed class ShiftRight64 : BaseTransform
	{
		public ShiftRight64() : base(IRInstruction.ShiftRight64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.ReplaceInstruction(X64.Shr64);
		}
	}
}
