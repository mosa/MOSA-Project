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
using Mosa.Runtime.Vm;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class SizeofInstruction : BaseInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="SizeofInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public SizeofInstruction(OpCode opcode)
			: base(opcode, 0, 1)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		/// <param name="typeSystem">The type system.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder, ITypeSystem typeSystem)
		{
			// Decode base classes first
			base.Decode(ctx, decoder, typeSystem);

			// Get the size type
			// Load the _stackFrameIndex token from the immediate
			TokenTypes token = decoder.DecodeTokenType();

			//throw new NotImplementedException();
			uint size = (uint)ObjectModelUtility.ComputeTypeSize(decoder.Method, token, decoder.Compiler.Assembly.Metadata, decoder.Compiler.Architecture);
			ctx.Result = new Operands.ConstantOperand(new Mosa.Runtime.Metadata.Signatures.SigType(CilElementType.U4), size);
			/*
				TypeReference _typeRef = MetadataTypeReference.FromToken(decoder.Metadata, token);

				// FIXME: Push the size of the type after layout
				ctx.Result = new ConstantOperand(NativeTypeReference.Int32, 0);
			*/
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Sizeof(context);
		}

		#endregion Methods

	}
}
