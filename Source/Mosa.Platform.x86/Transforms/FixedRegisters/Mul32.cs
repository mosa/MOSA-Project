// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

using Mosa.Platform.x86;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x86.Transform.FixedRegisters
{
	/// <summary>
	/// Mul32
	/// </summary>
	public sealed class Mul32 : BaseTransformation
	{
		public Mul32() : base(X86.Mul32, TransformationType.Manual | TransformationType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& !context.Operand2.IsConstant
				&& context.Result.Register == CPURegister.EDX
				&& context.Result2.Register == CPURegister.EAX
				&& context.Operand1.Register == CPURegister.EAX)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var operand1 = context.Operand1;
			var operand2 = context.Operand2;
			var result = context.Result;
			var result2 = context.Result2;

			var eax = Operand.CreateCPURegister(transform.I4, CPURegister.EAX);
			var edx = Operand.CreateCPURegister(transform.I4, CPURegister.EDX);

			context.SetInstruction(X86.Mov32, eax, operand1);

			if (operand2.IsConstant)
			{
				var v3 = transform.AllocateVirtualRegister32();
				context.AppendInstruction(X86.Mov32, v3, operand2);
				operand2 = v3;
			}

			Debug.Assert(operand2.IsCPURegister || operand2.IsVirtualRegister);

			context.AppendInstruction2(X86.Mul32, edx, eax, eax, operand2);
			context.AppendInstruction(X86.Mov32, result, edx);
			context.AppendInstruction(X86.Mov32, result2, eax);
		}
	}
}
