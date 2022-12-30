
using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.IR
{
	/// <summary>
	/// DivUnsigned64
	/// </summary>
	public sealed class DivUnsigned64 : BaseTransform
	{
		public DivUnsigned64() : base(IRInstruction.DivUnsigned64, TransformType.Manual | TransformType.Transform)
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

			var v1 = transform.AllocateVirtualRegister64();
			var v2 = transform.AllocateVirtualRegister64();

			context.SetInstruction(X64.Mov64, v1, transform.ConstantZero64);
			context.AppendInstruction2(X64.Div64, result, v2, v1, operand1, operand2);
		}
	}
}
