// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// CallDirect
	/// </summary>
	public sealed class CallDirect : BaseTransform
	{
		public CallDirect() : base(IRInstruction.CallDirect, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var operand1 = context.Operand1;

			if (operand1.IsCPURegister || operand1.IsVirtualRegister || operand1.IsResolvedConstant)
			{
				operand1 = ARMv8A32TransformHelper.MoveConstantToRegister(transform, context, operand1);

				context.SetInstruction(ARMv8A32.Add, transform.LinkRegister, transform.ProgramCounter, transform.Constant32_4);
				context.AppendInstruction(ARMv8A32.Mov, transform.ProgramCounter, operand1);
			}
			else
			{
				context.SetInstruction(ARMv8A32.Bl, operand1);
			}
		}
	}
}
