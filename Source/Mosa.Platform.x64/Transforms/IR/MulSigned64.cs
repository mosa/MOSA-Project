
using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// MulSigned64
	/// </summary>
	public sealed class MulSigned64 : BaseTransform
	{
		public MulSigned64() : base(IRInstruction.MulSigned64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var v1 = transform.AllocateVirtualRegister32();
			context.SetInstruction2(X64.Mul64, v1, context.Result, context.Operand1, context.Operand2);
		}
	}
}
