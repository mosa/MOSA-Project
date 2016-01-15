// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;
using System;

namespace Mosa.Compiler.Framework.Intrinsics
{
	[ReplacementTarget("Mosa.Runtime.Intrinsic::GetObjectAddress")]
	[ReplacementTarget("Mosa.Runtime.Intrinsic::GetValueTypeAddress")]
	public sealed class GetObjectAddress : IIntrinsicInternalMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodCompiler">The method compiler.</param>
		void IIntrinsicInternalMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			var result = context.Result;
			var operand1 = context.Operand1;
			if (operand1.IsValueType)
			{
				InstructionNode def = operand1.Definitions[0];
				var replacements = new List<Tuple<InstructionNode, int>>();
				foreach (var use in operand1.Uses)
					for (int i = 0; i < use.OperandCount; i++)
						if (use.GetOperand(i) == operand1)
							replacements.Add(new Tuple<InstructionNode, int>(use, i));

				foreach (var replace in replacements)
					replace.Item1.SetOperand(replace.Item2, def.Operand1);

				operand1 = def.Operand1;
				def.Empty();
				context.SetInstruction(IRInstruction.AddressOf, result, operand1);
				return;
			}
			context.SetInstruction(IRInstruction.Move, result, operand1);
		}
	}
}
