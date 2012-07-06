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
	public sealed class GetIDTJumpLocation : IIntrinsicMethod
	{
		#region Methods

		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void IIntrinsicMethod.ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem, IList<RuntimeParameter> parameters)
		{
			Context loadContext = new Context(context.InstructionSet, context.Operand1.Definitions[0]);

			Debug.Assert(loadContext.Operand1.IsConstant);

			int irq = -1;

			object obj = loadContext.Operand1.Value;

			if ((obj is int) || (obj is uint))
				irq = (int)obj;
			else if (obj is sbyte)
				irq = (sbyte)obj;

			if ((irq > 256) || (irq < 0))
				throw new InvalidOperationException();

			context.SetInstruction(IR.IRInstruction.Move, context.Result, Operand.CreateSymbol(BuiltInSigType.Ptr, @"Mosa.Tools.Compiler.LinkerGenerated.<$>InterruptISR" + irq.ToString() + "()"));
		}

		#endregion // Methods
	}
}
