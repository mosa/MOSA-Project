
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// SignExtend8x32
	/// </summary>
	public sealed class SignExtend8x32 : BaseTransformation
	{
		public SignExtend8x32() : base(IRInstruction.SignExtend8x32, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.ReplaceInstruction(X86.Movsx8To32);
		}
	}
}
