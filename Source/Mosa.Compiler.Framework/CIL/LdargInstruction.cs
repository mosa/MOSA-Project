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
	public sealed class LdargInstruction : LoadInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdargInstruction"/> class.
		/// </summary>
		public LdargInstruction(OpCode opCode)
			: base(opCode, 1)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Decodes the specified CIL instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		/// <remarks>
		/// This method is used by instructions to retrieve immediate operands
		/// From the instruction stream.
		/// </remarks>
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			ushort argIdx;

			// Opcode specific handling
			switch (opcode)
			{
				case OpCode.Ldarg:
					argIdx = decoder.DecodeUShort();
					break;

				case OpCode.Ldarg_s:
					argIdx = decoder.DecodeByte();
					break;

				case OpCode.Ldarg_0:
					argIdx = 0;
					break;

				case OpCode.Ldarg_1:
					argIdx = 1;
					break;

				case OpCode.Ldarg_2:
					argIdx = 2;
					break;

				case OpCode.Ldarg_3:
					argIdx = 3;
					break;

				default:
					throw new System.NotImplementedException();
			}

			// Push the loaded value onto the evaluation stack
			Operand parameterOperand = decoder.Compiler.GetParameterOperand(argIdx);
			Operand result = LoadInstruction.CreateResultOperand(decoder, parameterOperand.StackType, parameterOperand.Type);

			ctx.Operand1 = parameterOperand;
			ctx.Result = result;
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldarg(context);
		}

		#endregion Methods
	}
}