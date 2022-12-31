// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transforms.Tweak
{
	/// <summary>
	/// MovStore8
	/// </summary>
	public sealed class MovStore8 : BaseTransform
	{
		public MovStore8() : base(X86.MovStore8, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return context.Operand3.IsCPURegister && (context.Operand3.Register == CPURegister.ESI || context.Operand3.Register == CPURegister.EDI);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var dest = context.Operand1;
			var offset = context.Operand2;
			var value = context.Operand3;

			Operand temporaryRegister;

			if (dest.Register != CPURegister.EAX && offset.Register != CPURegister.EAX)
			{
				temporaryRegister = Operand.CreateCPURegister(transform.I4, CPURegister.EAX);
			}
			else if (dest.Register != CPURegister.EBX && offset.Register != CPURegister.EBX)
			{
				temporaryRegister = Operand.CreateCPURegister(transform.I4, CPURegister.EBX);
			}
			else if (dest.Register != CPURegister.ECX && offset.Register != CPURegister.ECX)
			{
				temporaryRegister = Operand.CreateCPURegister(transform.I4, CPURegister.ECX);
			}
			else
			{
				temporaryRegister = Operand.CreateCPURegister(transform.I4, CPURegister.EDX);
			}

			context.SetInstruction2(X86.XChg32, temporaryRegister, value, value, temporaryRegister);
			context.AppendInstruction(X86.MovStore8, null, dest, offset, temporaryRegister);
			context.AppendInstruction2(X86.XChg32, value, temporaryRegister, temporaryRegister, value);
		}
	}
}
