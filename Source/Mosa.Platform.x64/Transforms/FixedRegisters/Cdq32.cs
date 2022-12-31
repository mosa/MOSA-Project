using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.FixedRegisters
{
	/// <summary>
	/// Cdq32
	/// </summary>
	public sealed class Cdq32 : BaseTransform
	{
		public Cdq32() : base(X64.Cdq32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return !(context.Result.IsCPURegister
				&& context.Result2.IsCPURegister
				&& context.Operand1.IsCPURegister
				&& context.Result.Register == CPURegister.RDX
				&& context.Result2.Register == CPURegister.RAX
				&& context.Operand1.Register == CPURegister.RAX);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var operand1 = context.Operand1;
			var result = context.Result;
			var result2 = context.Result2;

			var rax = Operand.CreateCPURegister(transform.I4, CPURegister.RAX);
			var rdx = Operand.CreateCPURegister(transform.I4, CPURegister.RDX);

			context.SetInstruction(X64.Mov64, rax, operand1);
			context.AppendInstruction2(X64.Cdq32, rdx, rax, rax);
			context.AppendInstruction(X64.Mov64, result, rdx);
			context.AppendInstruction(X64.Mov64, result2, rax);
		}
	}
}
