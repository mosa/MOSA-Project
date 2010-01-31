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

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class NewarrInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="NewarrInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public NewarrInstruction(OpCode opcode)
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

			// Read the type specification
			TokenTypes arrayEType;
			decoder.Decode(out arrayEType);
            ctx.Token = arrayEType;

            Mosa.Runtime.Vm.RuntimeType type = RuntimeBase.Instance.TypeLoader.GetType(decoder.Compiler.Assembly, arrayEType);
            //ctx.Result = 
			/*
				TypeReference eType = MetadataTypeReference.FromToken(decoder.Metadata, arrayEType);

				// FIXME: If _operands[0] is an integral constant, we can infer the maximum size of the array
				// and instantiate an ArrayTypeSpecification with max. sizes. This way we could eliminate bounds
				// checks in an optimization stage later on, if we find that a value never exceeds the array 
				// bounds.

				// Build a type specification
				ArrayTypeSpecification typeRef = new ArrayTypeSpecification(eType);
				_results[0] = CreateResultOperand(typeRef);
			 */
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Newarr(context);
		}

		#endregion Methods

	}
}
