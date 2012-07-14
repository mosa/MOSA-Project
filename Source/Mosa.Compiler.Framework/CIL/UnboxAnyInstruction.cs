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

		#endregion // Construction

		#region Methods

		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			// Decode base classes first
			base.Decode(ctx, decoder);

			// Retrieve the provider token to check against
			var token = decoder.DecodeTokenType();
			var type = decoder.TypeModule.GetType(token);

			if (type.FullName == "System.Boolean")
				ctx.Result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.Boolean);
			else if (type.FullName == "System.SByte")
				ctx.Result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.SByte);
			else if (type.FullName == "System.Int16")
				ctx.Result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.Int16);
			else if (type.FullName == "System.Int32")
				ctx.Result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.Int32);
			else if (type.FullName == "System.Int64")
				ctx.Result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.Int64);
			else if (type.FullName == "System.Byte")
				ctx.Result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.Byte);
			else if (type.FullName == "System.UInt16")
				ctx.Result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.UInt16);
			else if (type.FullName == "System.UInt32")
				ctx.Result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.UInt32);
			else if (type.FullName == "System.UInt64")
				ctx.Result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.UInt64);
			else if (type.FullName == "System.Single")
				ctx.Result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.Single);
			else if (type.FullName == "System.Double")
				ctx.Result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.Double);
			else if (type.FullName == "System.Char")
				ctx.Result = decoder.Compiler.CreateVirtualRegister(BuiltInSigType.Char);
		
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

		#endregion // Methods

	}
}
