/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */


namespace Mosa.Compiler.Framework.CIL
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
			: base(opCode, 1)
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
			switch (opcode)
			{
				case OpCode.Ldloc:
					locIdx = decoder.DecodeUShort();
					break;

				case OpCode.Ldloc_s:
					locIdx = decoder.DecodeByte();
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
					throw new System.NotImplementedException();
			}

			// Push the loaded value onto the evaluation stack
			Operand localVariableOperand = decoder.Compiler.GetLocalOperand(locIdx);
			Operand result = LoadInstruction.CreateResultOperand(decoder, localVariableOperand.StackType, localVariableOperand.Type);

			ctx.Operand1 = localVariableOperand;
			ctx.Result = result;
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


		#endregion Methods

	}
}
