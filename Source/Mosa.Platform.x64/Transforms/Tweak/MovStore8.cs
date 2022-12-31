using System.Diagnostics;

using Mosa.Platform.x64;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.x64.Transforms.Tweak
{
	/// <summary>
	/// MovStore8
	/// </summary>
	public sealed class MovStore8 : BaseTransform
	{
		public MovStore8() : base(X64.MovStore8, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return context.Operand3.IsCPURegister && (context.Operand3.Register == CPURegister.RSI || context.Operand3.Register == CPURegister.RDI);
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var value = context.Operand3;
			var dest = context.Operand1;
			var offset = context.Operand2;

			Operand temporaryRegister = null;

			if (dest.Register != CPURegister.RAX && offset.Register != CPURegister.RAX)
			{
				temporaryRegister = Operand.CreateCPURegister(transform.I4, CPURegister.RAX);
			}
			else if (dest.Register != CPURegister.RBX && offset.Register != CPURegister.RBX)
			{
				temporaryRegister = Operand.CreateCPURegister(transform.I4, CPURegister.RBX);
			}
			else if (dest.Register != CPURegister.RAX && offset.Register != CPURegister.RAX)
			{
				temporaryRegister = Operand.CreateCPURegister(transform.I4, CPURegister.RAX);
			}
			else
			{
				temporaryRegister = Operand.CreateCPURegister(transform.I4, CPURegister.RDX);
			}

			context.SetInstruction2(X64.XChg64, temporaryRegister, value, value, temporaryRegister);
			context.AppendInstruction(X64.MovStore8, null, dest, offset, temporaryRegister);
			context.AppendInstruction2(X64.XChg64, value, temporaryRegister, temporaryRegister, value);
		}
	}
}
