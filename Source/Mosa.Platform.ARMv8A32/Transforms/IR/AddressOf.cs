// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Platform.ARMv8A32.Transforms.IR
{
	/// <summary>
	/// AddressOf
	/// </summary>
	public sealed class AddressOf : BaseTransform
	{
		public AddressOf() : base(IRInstruction.AddressOf, TransformType.Manual | TransformType.Transform)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			Debug.Assert(context.Operand1.IsOnStack || context.Operand1.IsStaticField);

			var result = context.Result;
			var operand1 = context.Operand1;

			operand1 = ARMv8A32TransformHelper.MoveConstantToRegisterOrImmediate(transform, context, operand1);

			if (operand1.IsStaticField)
			{
				context.SetInstruction(ARMv8A32.Mov, result, operand1);
			}
			else if (operand1.IsStackLocal)
			{
				context.SetInstruction(ARMv8A32.Add, result, transform.StackFrame, operand1);
			}
			else if (context.Operand1.IsUnresolvedConstant)
			{
				var offset = ARMv8A32TransformHelper.MoveConstantToRegister(transform, context, operand1);

				context.SetInstruction(ARMv8A32.Add, result, transform.StackFrame, offset);
			}
			else
			{
				var offset = transform.CreateConstant32(context.Operand1.Offset);

				offset = ARMv8A32TransformHelper.MoveConstantToRegisterOrImmediate(transform, context, offset);

				context.SetInstruction(ARMv8A32.Add, result, transform.StackFrame, offset);
			}
		}
	}
}
