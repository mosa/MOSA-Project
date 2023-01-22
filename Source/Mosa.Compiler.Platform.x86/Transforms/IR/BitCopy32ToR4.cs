// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.IR
{
	/// <summary>
	/// BitCopy32ToR4
	/// </summary>
	public sealed class BitCopy32ToR4 : BaseTransform
	{
		public BitCopy32ToR4() : base(IRInstruction.BitCopy32ToR4, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.ReplaceInstruction(X86.Movdi32ss);
		}
	}
}
