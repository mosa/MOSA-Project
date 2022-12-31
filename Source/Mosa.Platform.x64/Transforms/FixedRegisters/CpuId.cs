// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.FixedRegisters
{
	/// <summary>
	/// CpuId
	/// </summary>
	public sealed class CpuId : BaseTransform
	{
		public CpuId() : base(X64.CpuId, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return (context.Operand1.IsCPURegister
				&& context.Operand2.IsCPURegister
				&& context.Operand1.Register == CPURegister.RAX
				&& context.Operand2.Register == CPURegister.RCX);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;

			var rax = Operand.CreateCPURegister(transform.I4, CPURegister.RAX);
			var rcx = Operand.CreateCPURegister(transform.I4, CPURegister.RCX);

			context.SetInstruction(X64.Mov64, rax, operand1);
			context.AppendInstruction(X64.Mov64, rcx, operand2);
			context.AppendInstruction(X64.CpuId, rax, rax, rcx);
			context.AppendInstruction(X64.Mov64, result, rax);
		}
	}
}
