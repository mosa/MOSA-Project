// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.FixedRegisters
{
	/// <summary>
	/// Out16
	/// </summary>
	public sealed class Out16 : BaseTransform
	{
		public Out16() : base(X86.Out16, TransformType.Manual | TransformType.Transform)
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
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;

			var eax = Operand.CreateCPURegister(operand2.Type, CPURegister.EAX);
			var edx = Operand.CreateCPURegister(operand1.Type, CPURegister.EDX);

			context.SetInstruction(X86.Mov32, edx, operand1);
			context.AppendInstruction(X86.Mov32, eax, operand2);
			context.AppendInstruction(X86.Out16, null, edx, eax);
		}
	}
}
