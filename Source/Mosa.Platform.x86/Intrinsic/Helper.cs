// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Helper
	/// </summary>
	internal static class Helper
	{
		#region Methods

		/// <summary>
		/// Converts the operand1 to constant.
		/// </summary>
		/// <param name="context">The context.</param>
		internal static void FoldOperand1ToConstant(Context context)
		{
			var operand1 = context.Operand1;

			if (operand1.IsConstant)
				return;

			while (!operand1.IsConstant)
			{
				if (operand1.Definitions.Count != 1)
					break;

				var node = operand1.Definitions[0];

				if ((node.Instruction == X86.Mov32 || node.Instruction == IRInstruction.Move32) && node.Operand1.IsConstant)
				{
					operand1 = node.Operand1;
					continue;
				}

				break;
			}

			if (!operand1.IsConstant)
			{
				throw new CompilerException("unable to find constant value");
			}

			context.Operand1 = operand1;
		}

		#endregion Methods
	}
}
