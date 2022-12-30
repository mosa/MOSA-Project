
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// ZeroExtend16x32
	/// </summary>
	public sealed class ZeroExtend16x32 : BaseTransformation
	{
		public ZeroExtend16x32() : base(IRInstruction.ZeroExtend16x32, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.ReplaceInstruction(X86.Movzx16To32);
		}
	}
}
