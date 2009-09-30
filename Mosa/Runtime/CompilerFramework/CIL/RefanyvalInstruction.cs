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

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class RefanyvalInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="RefanyvalInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public RefanyvalInstruction(OpCode opcode)
			: base(opcode)
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

			// Retrieve a type reference From the immediate argument
			// FIXME: Limit the token types
			TokenTypes token;
			decoder.Decode(out token);
			throw new NotImplementedException();
			//_typeRef = MetadataTypeReference.FromToken(decoder.Metadata, token);
		}

		/// <summary>
		/// Validates the instruction operands and creates a matching variable for the result.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="compiler">The compiler.</param>
		public override void Validate(Context ctx, IMethodCompiler compiler)
		{
			base.Validate(ctx, compiler);

			// Make sure the base is a typed reference
			throw new NotImplementedException();
			/*
				if (!Object.ReferenceEquals(_operands[0].Type, MetadataTypeReference.FromName(compiler.Assembly.Metadata, @"System", @"TypedReference")))
				{
					Debug.Assert(false);
					throw new InvalidProgramException(@"Invalid stack object.");
				}

				// Push the loaded value
				_results[0] = CreateResultOperand(_typeRef);
			 */
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="vistor">The vistor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor vistor, Context context)
		{
			vistor.Refanyval(context);
		}

		#endregion Methods
	}
}
