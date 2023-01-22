// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Platform.x86.Transforms.IR
{
	/// <summary>
	/// DivSigned32
	/// </summary>
	public sealed class DivSigned32 : BaseTransform
	{
		public DivSigned32() : base(IRInstruction.DivSigned32, TransformType.Manual | TransformType.Transform)
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

			var v1 = transform.AllocateVirtualRegister32();

			var eax = Operand.CreateCPURegister(transform.I4, CPURegister.EAX);
			var edx = Operand.CreateCPURegister(transform.I4, CPURegister.EDX);

			context.SetInstruction(X86.Mov32, eax, operand1);
			context.AppendInstruction(X86.Cdq32, edx, eax);
			context.AppendInstruction2(X86.IDiv32, v1, result, edx, eax, operand2);
		}
	}
}
