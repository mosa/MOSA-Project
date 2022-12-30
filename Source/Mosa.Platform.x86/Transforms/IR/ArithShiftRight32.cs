
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// ArithShiftRight32
	/// </summary>
	public sealed class ArithShiftRight32 : BaseTransformation
	{
		public ArithShiftRight32() : base(IRInstruction.ArithShiftRight32, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.ReplaceInstruction(X86.Sar32);
		}
	}
}
