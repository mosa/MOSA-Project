
using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.IR
{
	/// <summary>
	/// RemSigned32
	/// </summary>
	public sealed class RemSigned32 : BaseTransform
	{
		public RemSigned32() : base(IRInstruction.RemSigned32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var v1 = transform.AllocateVirtualRegister32();
			var v2 = transform.AllocateVirtualRegister32();
			var v3 = transform.AllocateVirtualRegister32();

			context.SetInstruction2(X86.Cdq32, v1, v2, operand1);
			context.AppendInstruction2(X86.IDiv32, result, v3, v1, v2, operand2);
		}
	}
}
