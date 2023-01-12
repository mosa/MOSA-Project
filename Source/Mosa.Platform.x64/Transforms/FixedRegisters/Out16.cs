// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.FixedRegisters
{
	/// <summary>
	/// Out16
	/// </summary>
	public sealed class Out16 : BaseTransform
	{
		public Out16() : base(X64.Out16, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return !(context.Operand1.IsCPURegister
				&& context.Operand2.IsCPURegister
				&& (context.Operand1.Register == CPURegister.RDX || context.Operand1.IsConstant)
				&& context.Operand2.Register == CPURegister.RAX);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			// TRANSFORM: OUT <= rdx, rax && OUT <= imm8, rax

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var rax = Operand.CreateCPURegister(operand2.Type, CPURegister.RAX);
			var rdx = Operand.CreateCPURegister(operand1.Type, CPURegister.RDX);

			context.SetInstruction(X64.Mov64, rdx, operand1);
			context.AppendInstruction(X64.Mov64, rax, operand2);
			context.AppendInstruction(X64.Out16, null, rdx, rax);
		}
	}
}
