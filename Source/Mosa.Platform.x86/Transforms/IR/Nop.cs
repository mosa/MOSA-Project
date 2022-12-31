// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// Nop
	/// </summary>
	public sealed class Nop : BaseTransform
	{
		public Nop() : base(IRInstruction.Nop, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.Empty();

			//context.SetInstruction(X86.Nop);
		}
	}
}
