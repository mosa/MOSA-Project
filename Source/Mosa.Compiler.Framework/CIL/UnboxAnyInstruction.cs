/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	///
	/// </summary>
	public sealed class UnboxAnyInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="UnboxAnyInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public UnboxAnyInstruction(OpCode opcode)
			: base(opcode, 1)
		{
		}

		#endregion Construction

		#region Methods

		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			// Retrieve the provider token to check against
			var token = decoder.DecodeTokenType();
			var type = decoder.TypeModule.GetType(token);

			Operand result;

			switch (type.FullName)
			{
				case "System.Boolean": result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.Boolean); break;
				case "System.SByte": result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.SByte); break;
				case "System.Int16": result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.Int16); break;
				case "System.Int32": result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.Int32); break;
				case "System.Int64": result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.Int64); break;
				case "System.Byte": result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.Byte); break;
				case "System.UInt16": result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.UInt16); break;
				case "System.UInt32": result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.UInt32); break;
				case "System.UInt64": result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.UInt64); break;
				case "System.Single": result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.Single); break;
				case "System.Double": result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.Double); break;
				case "System.Char": result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.Char); break;
				default: throw new System.InvalidOperationException();
			}

			// threat this like a load
			result = LoadInstruction.CreateResultOperand(decoder, result.StackType, result.Type);

			ctx.Result = result;
			ctx.RuntimeType = type;
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.UnboxAny(context);
		}

		#endregion Methods
	}
}