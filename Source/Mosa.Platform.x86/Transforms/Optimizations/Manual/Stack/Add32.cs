// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Transforms;

using System.Diagnostics;

namespace Mosa.Platform.x86.Transforms.Optimizations.Manual.Stack
{
	/// <summary>
	/// Add32
	/// </summary>
	public sealed class Add32 : BaseTransformation
	{
		public Add32() : base(X86.Add32, TransformationType.Manual | TransformationType.Optimization)
		{
		}

		public override bool Match(Context context, TransformContext transform)
		{
			if (transform.AreCPURegistersAllocated)
				return false;

			if (!context.Operand1.IsCPURegister)
				return false;

			if (context.Operand1.Register != CPURegister.ESP)
				return false;

			if (!context.Operand2.IsConstant)
				return false;

			var next = GetNextNode(context);

			if (next == null)
				return false;

			if (next.Instruction != X86.Sub32)
				return false;

			if (context.Operand1.Register != CPURegister.ESP)
				return false;

			if (!next.Operand2.IsConstant)
				return false;

			return true;
		}

		public override void Transform(Context context, TransformContext transform)
		{
			var next = GetNextNode(context);

			var value1 = context.Operand2.ConstantSigned32;
			var value2 = next.Operand2.ConstantSigned32;

			var value = value1 - value2;

			next.SetNop();

			if (value > 0)
			{
				context.SetInstruction(X86.Add32, context.Result, context.Operand1, transform.CreateConstant(value));
			}
			else if (value < 0)
			{
				context.SetInstruction(X86.Sub32, context.Result, context.Operand1, transform.CreateConstant(-value));
			}
			else // if (value == 0)
			{
				Debug.Assert(value == 0);
				context.SetNop();
			}
		}
	}
}
