// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// Truncate64x32
	/// </summary>
	public sealed class Truncate64x32 : BaseTransform
	{
		public Truncate64x32() : base(IRInstruction.Truncate64x32, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Operand1.IsInteger64);
			Debug.Assert(!context.Result.IsInteger64);

			transform.SplitLongOperand(context.Result, out var resultLow, out _);
			transform.SplitLongOperand(context.Operand1, out var op1L, out _);

			op1L = ARMv8A32TransformHelper.MoveConstantToRegisterOrImmediate(transform, context, op1L);

			context.SetInstruction(ARMv8A32.Mov, StatusRegister.Set, resultLow, op1L);
		}
	}
}
