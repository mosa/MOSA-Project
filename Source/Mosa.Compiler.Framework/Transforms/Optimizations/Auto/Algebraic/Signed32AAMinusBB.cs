// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Algebraic
{
	/// <summary>
	/// Signed32AAMinusBB
	/// </summary>
	public sealed class Signed32AAMinusBB : BaseTransformation
	{
		public Signed32AAMinusBB() : base(IRInstruction.Sub32, TransformationType.Auto | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (!context.Operand1.IsVirtualRegister)
				return false;

			if (!context.Operand2.IsVirtualRegister)
				return false;

			if (context.Operand1.Definitions.Count != 1)
				return false;

			if (context.Operand1.Definitions[0].Instruction != IRInstruction.MulSigned32)
				return false;

			if (context.Operand2.Definitions.Count != 1)
				return false;

			if (context.Operand2.Definitions[0].Instruction != IRInstruction.MulSigned32)
				return false;

			if (!AreSame(context.Operand1.Definitions[0].Operand1, context.Operand1.Definitions[0].Operand2))
				return false;

			if (!AreSame(context.Operand2.Definitions[0].Operand1, context.Operand2.Definitions[0].Operand2))
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var result = context.Result;

			var t1 = context.Operand1.Definitions[0].Operand1;
			var t2 = context.Operand2.Definitions[0].Operand1;

			var v1 = transform.AllocateVirtualRegister(transform.I4);
			var v2 = transform.AllocateVirtualRegister(transform.I4);

			context.SetInstruction(IRInstruction.Add32, v1, t1, t2);
			context.AppendInstruction(IRInstruction.Sub32, v2, t1, t2);
			context.AppendInstruction(IRInstruction.MulSigned32, result, v2, v1);
		}
	}
}
