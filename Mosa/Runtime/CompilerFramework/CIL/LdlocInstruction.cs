/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class LdlocInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdlocInstruction"/> class.
		/// </summary>
		public LdlocInstruction(OpCode opCode)
			: base(opCode)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			// Opcode specific handling
			ushort locIdx;
			switch (_opcode) {
				case OpCode.Ldloc:
					decoder.Decode(out locIdx);
					break;

				case OpCode.Ldloc_s: {
						byte loc;
						decoder.Decode(out loc);
						locIdx = loc;
					}
					break;

				case OpCode.Ldloc_0:
					locIdx = 0;
					break;

				case OpCode.Ldloc_1:
					locIdx = 1;
					break;

				case OpCode.Ldloc_2:
					locIdx = 2;
					break;

				case OpCode.Ldloc_3:
					locIdx = 3;
					break;

				default:
					throw new NotImplementedException();
			}

			// Push the loaded value onto the evaluation stack
			ctx.Result = decoder.Compiler.GetLocalOperand(locIdx);
			ctx.Ignore = true;
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldloc(context);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(Context ctx)
		{
			return String.Format("{0} ; {1}", base.ToString(), ctx.Result);
		}

		#endregion Methods

	}
}
