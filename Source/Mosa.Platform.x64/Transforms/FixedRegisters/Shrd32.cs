using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.FixedRegisters
{
	/// <summary>
	/// Shrd32
	/// </summary>
	public sealed class Shrd32 : BaseTransform
	{
		public Shrd32() : base(X64.Shrd32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (context.Operand3.IsConstant)
				return false;

			if (context.Operand3.Register == CPURegister.RCX)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;
			var result = context.Result;

			var rcx = Operand.CreateCPURegister(transform.I4, CPURegister.RCX);

			context.SetInstruction(X64.Mov64, rcx, operand3);
			context.AppendInstruction(X64.Shrd32, result, operand1, operand2, rcx);
		}
	}
}
