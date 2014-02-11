﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.MosaTypeSystem;
using System;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class ArithmeticOverflowInstruction : BinaryInstruction
	{
		#region Static data members

		/// <summary>
		/// Generic operand validation table. Not used for add and sub.
		/// </summary>
		private static StackTypeCode[][] operandTable = new StackTypeCode[][] {
			new StackTypeCode[] { StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
		};

		/// <summary>
		/// Operand validation table for the add instruction.
		/// </summary>
		private static StackTypeCode[][] addovfunTable = new StackTypeCode[][] {
			new StackTypeCode[] { StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
		};

		/// <summary>
		/// Operand validation table for the sub instruction.
		/// </summary>
		private static StackTypeCode[][] subovfunTable = new StackTypeCode[][] {
			new StackTypeCode[] { StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
		};

		#endregion Static data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ArithmeticOverflowInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public ArithmeticOverflowInstruction(OpCode opcode)
			: base(opcode, 1)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.ArithmeticOverflow(context);
		}

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Resolve(Context ctx, BaseMethodCompiler compiler)
		{
			base.Resolve(ctx, compiler);

			StackTypeCode result = StackTypeCode.Unknown;
			
			switch (opcode)
			{
				case OpCode.Add_ovf_un: result = addovfunTable[(int)TypeSystem.GetStackType(ctx.Operand1.Type)][(int)TypeSystem.GetStackType(ctx.Operand2.Type)]; break;
				case OpCode.Sub_ovf_un: result = subovfunTable[(int)TypeSystem.GetStackType(ctx.Operand1.Type)][(int)TypeSystem.GetStackType(ctx.Operand2.Type)]; break;
				default: result = operandTable[(int)TypeSystem.GetStackType(ctx.Operand1.Type)][(int)TypeSystem.GetStackType(ctx.Operand2.Type)]; break;
			}

			if (StackTypeCode.Unknown == result)
			{
				throw new InvalidOperationException(@"Invalid operand types passed to " + opcode);
			}

			ctx.Result = compiler.CreateVirtualRegister(compiler.TypeSystem.GetType(result));
		}

		#endregion Methods
	}
}