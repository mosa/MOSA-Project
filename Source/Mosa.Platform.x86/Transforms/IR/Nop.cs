
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// Nop
	/// </summary>
	public sealed class Nop : BaseTransformation
	{
		public Nop() : base(IRInstruction.Nop, TransformationType.Manual | TransformationType.Transform)
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
