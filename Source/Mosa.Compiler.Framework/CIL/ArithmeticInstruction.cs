/*
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
	public class ArithmeticInstruction : BinaryInstruction
	{
		#region Static data members

		/// <summary>
		/// Generic operand validation table. Not used for add and sub.
		/// </summary>
		private static StackTypeCode[][] operandTable = new StackTypeCode[][] {
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.F,       StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
		};

		/// <summary>
		/// Operand validation table for the add instruction.
		/// </summary>
		private static StackTypeCode[][] addTable = new StackTypeCode[][] {
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.F,       StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
		};

		/// <summary>
		/// Operand validation table for the sub instruction.
		/// </summary>
		private static StackTypeCode[][] subTable = new StackTypeCode[][] {
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Int32,   StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Int64,   StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.F,       StackTypeCode.Unknown, StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.Ptr,     StackTypeCode.Unknown, StackTypeCode.N,       StackTypeCode.Unknown },
			new StackTypeCode[] { StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown, StackTypeCode.Unknown },
		};

		#endregion Static data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ArithmeticInstruction"/> class.
		/// </summary>
		public ArithmeticInstruction(OpCode opcode)
			: base(opcode, 1)
		{
		}

		#endregion Construction

		#region Methods

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
				case OpCode.Add: result = addTable[(int)TypeSystem.GetStackType(ctx.Operand1.Type)][(int)TypeSystem.GetStackType(ctx.Operand2.Type)]; break;
				case OpCode.Sub: result = subTable[(int)TypeSystem.GetStackType(ctx.Operand1.Type)][(int)TypeSystem.GetStackType(ctx.Operand2.Type)]; break;
				default: result = operandTable[(int)TypeSystem.GetStackType(ctx.Operand1.Type)][(int)TypeSystem.GetStackType(ctx.Operand2.Type)]; break;
			}

			if (result == StackTypeCode.Unknown)
			{
				throw new InvalidOperationException(@"Invalid operand types passed to " + opcode);
			}

			MosaType resultType = null;

			if (StackTypeCode.Ptr != result)
			{
				resultType = compiler.TypeSystem.GetType(result);
			}
			else
			{
				if (ctx.Operand1.Type.IsUnmanagedPointerType)
				{
					resultType = ctx.Operand1.Type;
				}
				else if (ctx.Operand2.Type.IsUnmanagedPointerType)
				{
					resultType = ctx.Operand2.Type;
				}
				else
					throw new InvalidOperationException(@"Invalid operand types passed to " + opcode);
			}

			ctx.Result = compiler.CreateVirtualRegister(resultType);
		}

		#endregion Methods
	}
}