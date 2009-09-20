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
	public class LdlocInstruction : LoadInstruction
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
		/// <param name="instruction">The instruction.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(ref InstructionData instruction, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ref instruction, decoder);

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
			instruction.Result = decoder.Compiler.GetLocalOperand(locIdx);
			instruction.Ignore = true;
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor vistor, Context context)
		{
			vistor.Ldloc(context);
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <param name="instruction">The instruction.</param>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString(ref InstructionData instruction)
		{
			return String.Format("{0} ; {1}", base.ToString(), instruction.Result);
		}

		#endregion Methods

	}
}
