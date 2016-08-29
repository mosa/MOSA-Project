// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.IR;
using System.Diagnostics;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 Lidt instruction.
	/// </summary>
	internal sealed class Lidt : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, BaseMethodCompiler methodCompiler)
		{
			//Debug.Assert(context.Operand1.IsConstant); // only constants are supported
			var operand1 = context.Operand1;

			// HACK --- for when optimizations are turned off
			if (!operand1.IsConstant)
			{
				if (operand1.Definitions.Count == 1)
				{
					var node = operand1.Definitions[0];

					if (node.Instruction == X86.Mov && node.Operand1.IsConstant)
						operand1 = node.Operand1;
				}
			}

			Debug.Assert(operand1.IsConstant); // only constants are supported

			context.SetInstruction(X86.Lidt, null, operand1);
		}

		#endregion Methods
	}
}
