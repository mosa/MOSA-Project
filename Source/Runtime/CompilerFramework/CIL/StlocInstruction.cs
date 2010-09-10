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
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class StlocInstruction : StoreInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="StlocInstruction"/> class.
		/// </summary>
		public StlocInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Stloc has a result, but doesn't push it on the stack.
		/// </summary>
		/// <value><c>true</c> if [push result]; otherwise, <c>false</c>.</value>
		public override bool PushResult
		{
			get { return false; }
		}

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			ushort locIdx;

			// Destination depends on the opcode
			switch (_opcode)
			{
				case OpCode.Stloc:
					locIdx = decoder.DecodeUShort();
					break;

				case OpCode.Stloc_s:
					{
						byte loc = decoder.DecodeByte();
						locIdx = loc;
						break;
					}


				case OpCode.Stloc_0:
					locIdx = 0;
					break;

				case OpCode.Stloc_1:
					locIdx = 1;
					break;

				case OpCode.Stloc_2:
					locIdx = 2;
					break;

				case OpCode.Stloc_3:
					locIdx = 3;
					break;

				default:
					throw new NotImplementedException();
			}

			ctx.Result = decoder.Compiler.GetLocalOperand(locIdx);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Stloc(context);
		}

		#endregion Methods

	}
}
