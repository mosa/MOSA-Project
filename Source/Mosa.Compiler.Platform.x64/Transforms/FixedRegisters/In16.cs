// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x64.Transforms.FixedRegisters
{
	/// <summary>
	/// In16
	/// </summary>
	public sealed class In16 : BaseTransform
	{
		public In16() : base(X64.In16, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return !(context.Result.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == CPURegister.RAX
				&& (context.Operand1.Register == CPURegister.RDX || context.Operand1.IsConstant));
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			var rax = Operand.CreateCPURegister(transform.I8, CPURegister.RAX);
			var rdx = Operand.CreateCPURegister(operand1.Type, CPURegister.RDX);

			context.SetInstruction(X64.Mov64, rdx, operand1);
			context.AppendInstruction(X64.In16, rax, rdx);
			context.AppendInstruction(X64.Mov64, result, rax);
		}
	}
}
