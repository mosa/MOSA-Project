// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transform.FixedRegisters
{
	/// <summary>
	/// Out8
	/// </summary>
	public sealed class Out8 : BaseTransformation
	{
		public Out8() : base(X86.Out8, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (context.Operand1.IsCPURegister
				&& context.Operand2.IsCPURegister
				&& (context.Operand1.Register == CPURegister.EDX || context.Operand1.IsConstant)
				&& context.Operand2.Register == CPURegister.EAX)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			// TRANSFORM: OUT <= edx, eax && OUT <= imm8, eax

			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var instruction = context.Instruction;

			var eax = Operand.CreateCPURegister(operand2.Type, CPURegister.EAX);
			var edx = Operand.CreateCPURegister(operand1.Type, CPURegister.EDX);

			context.SetInstruction(X86.Mov32, edx, operand1);
			context.AppendInstruction(X86.Mov32, eax, operand2);
			context.AppendInstruction(instruction, null, edx, eax);
		}
	}
}
