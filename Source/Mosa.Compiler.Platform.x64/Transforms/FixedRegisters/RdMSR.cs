// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x64.Transforms.FixedRegisters
{
	/// <summary>
	/// RdMSR
	/// </summary>
	public sealed class RdMSR : BaseTransform
	{
		public RdMSR() : base(X64.RdMSR, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return !(context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == CPURegister.RAX
				&& context.Result2.Register == CPURegister.RDX
				&& context.Operand1.Register == CPURegister.RCX);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var operand1 = context.Operand1;
			var result = context.Result;
			var result2 = context.Result2;

			var rax = Operand.CreateCPURegister(transform.I8, CPURegister.RAX);
			var rdx = Operand.CreateCPURegister(transform.I8, CPURegister.RDX);
			var rcx = Operand.CreateCPURegister(transform.I8, CPURegister.RCX);

			context.SetInstruction(X64.Mov64, rcx, operand1);
			context.AppendInstruction2(X64.RdMSR, rax, rdx, rcx);
			context.AppendInstruction(X64.Mov64, result, rax);
			context.AppendInstruction(X64.Mov64, result2, rdx);
		}
	}
}
