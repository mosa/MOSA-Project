// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transform.FixedRegisters
{
	/// <summary>
	/// IDiv32
	/// </summary>
	public sealed class IDiv32 : BaseTransform
	{
		public IDiv32() : base(X86.IDiv32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (context.Result.IsCPURegister
			&& context.Result2.IsCPURegister
			&& context.Operand1.IsCPURegister
			&& context.Operand2.IsCPURegister
			&& context.Result.Register == CPURegister.EDX
			&& context.Result2.Register == CPURegister.EAX
			&& context.Operand1.Register == CPURegister.EDX
			&& context.Operand2.Register == CPURegister.EAX)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var operand3 = context.Operand3;
			var result = context.Result;
			var result2 = context.Result2;

			var eax = Operand.CreateCPURegister(transform.I4, CPURegister.EAX);
			var edx = Operand.CreateCPURegister(transform.I4, CPURegister.EDX);

			context.SetInstruction(X86.Mov32, edx, operand1);
			context.AppendInstruction(X86.Mov32, eax, operand2);

			if (operand3.IsCPURegister)
			{
				context.AppendInstruction2(X86.IDiv32, edx, eax, edx, eax, operand3);
			}
			else
			{
				var v3 = transform.AllocateVirtualRegister32();
				context.AppendInstruction(X86.Mov32, v3, operand3);
				context.AppendInstruction2(X86.IDiv32, edx, eax, edx, eax, v3);
			}

			context.AppendInstruction(X86.Mov32, result2, eax);
			context.AppendInstruction(X86.Mov32, result, edx);
		}
	}
}
