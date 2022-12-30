
using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// ArithShiftRight32
	/// </summary>
	public sealed class ArithShiftRight32 : BaseTransform
	{
		public ArithShiftRight32() : base(IRInstruction.ArithShiftRight32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			context.ReplaceInstruction(X64.Sar32);
		}
	}
}
