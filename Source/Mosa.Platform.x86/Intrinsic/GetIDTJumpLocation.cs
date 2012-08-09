/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.TypeSystem;
using IR = Mosa.Compiler.Framework.IR;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations a jump to the global interrupt handler.
	/// </summary>
	public sealed class GetIDTJumpLocation : IIntrinsicPlatformMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicPlatformMethod.ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			var operand = context.Operand1;

			if (!operand.IsConstant)
			{
				// try to find the constant - a bit of a hack
				Context def = new Context(context.InstructionSet, operand.Definitions[0]);

				if (def.Instruction is IR.Move && def.Operand1.IsConstant)
					operand = def.Operand1;
			}

			Debug.Assert(operand.IsConstant);

			int irq = (int)operand.ValueAsLongInteger;

			if ((irq > 256) || (irq < 0))
				throw new InvalidOperationException();

			context.SetInstruction(IR.IRInstruction.Move, context.Result, Operand.CreateSymbol(BuiltInSigType.Ptr, @"Mosa.Tools.Compiler.LinkerGenerated.<$>InterruptISR" + irq.ToString() + "()"));
		}

		#endregion // Methods
	}
}
