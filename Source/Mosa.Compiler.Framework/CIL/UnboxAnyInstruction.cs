/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
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
				ctx.Result = decoder.Compiler.CreateTemporary(BuiltInSigType.Boolean);
			else if (type.FullName == "System.SByte")
				ctx.Result = decoder.Compiler.CreateTemporary(BuiltInSigType.SByte);
			else if (type.FullName == "System.Int16")
				ctx.Result = decoder.Compiler.CreateTemporary(BuiltInSigType.Int16);
			else if (type.FullName == "System.Int32")
				ctx.Result = decoder.Compiler.CreateTemporary(BuiltInSigType.Int32);
			else if (type.FullName == "System.Int64")
				ctx.Result = decoder.Compiler.CreateTemporary(BuiltInSigType.Int64);
			else if (type.FullName == "System.Byte")
				ctx.Result = decoder.Compiler.CreateTemporary(BuiltInSigType.Byte);
			else if (type.FullName == "System.UInt16")
				ctx.Result = decoder.Compiler.CreateTemporary(BuiltInSigType.UInt16);
			else if (type.FullName == "System.UInt32")
				ctx.Result = decoder.Compiler.CreateTemporary(BuiltInSigType.UInt32);
			else if (type.FullName == "System.UInt64")
				ctx.Result = decoder.Compiler.CreateTemporary(BuiltInSigType.UInt64);
			else if (type.FullName == "System.Single")
				ctx.Result = decoder.Compiler.CreateTemporary(BuiltInSigType.Single);
			else if (type.FullName == "System.Double")
				ctx.Result = decoder.Compiler.CreateTemporary(BuiltInSigType.Double);
			else if (type.FullName == "System.Char")
				ctx.Result = decoder.Compiler.CreateTemporary(BuiltInSigType.Char);
			else
				Console.WriteLine();

			ctx.Other = type;
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
