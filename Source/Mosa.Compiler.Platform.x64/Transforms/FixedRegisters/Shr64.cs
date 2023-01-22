// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x64.Transforms.FixedRegisters
{
	/// <summary>
	/// Shr64
	/// </summary>
	public sealed class Shr64 : BaseTransform
	{
		public Shr64() : base(X64.Shr64, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;

			var rcx = Operand.CreateCPURegister(transform.I8, CPURegister.RCX);

			context.SetInstruction(X64.Mov64, rcx, operand2);
			context.AppendInstruction(X64.Shr64, result, operand1, rcx);
		}
	}
}
