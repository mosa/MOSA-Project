/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class InitObjInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="InitObjInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public InitObjInstruction(OpCode opcode)
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

			// Retrieve the type reference
			TokenTypes token;
			decoder.Decode(out token);

            int size = ComputeTypeSize(token, decoder.Compiler);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.InitObj(context);
		}

        private static int ComputeTypeSize(TokenTypes token, IMethodCompiler compiler)
        {
            IMetadataProvider metadata = compiler.Assembly.Metadata;
            Metadata.Tables.TypeDefRow typeDefinition;
            Metadata.Tables.TypeDefRow followingTypeDefinition;
            metadata.Read(token, out typeDefinition);
            metadata.Read(token + 1, out followingTypeDefinition);

            int result = 0;
            TokenTypes fieldList = typeDefinition.FieldList;
            while (fieldList != followingTypeDefinition.FieldList)
                result += FieldSize(fieldList++, compiler);

            return result;
        }

        private static int FieldSize(TokenTypes field, IMethodCompiler compiler)
        {
            Metadata.Tables.FieldRow fieldRow;
            compiler.Assembly.Metadata.Read(field, out fieldRow);
            FieldSignature signature = Signature.FromMemberRefSignatureToken(compiler.Assembly.Metadata, fieldRow.SignatureBlobIdx) as FieldSignature;

            // If the field is another struct, we have to dig down and compute its size too.
            if (signature.Type.Type == CilElementType.ValueType)
            {
                TokenTypes valueTypeSig = ValueTokenTypeFromSignature(compiler.Assembly.Metadata, fieldRow.SignatureBlobIdx);
                return ComputeTypeSize(valueTypeSig, compiler);
            }

            int size, alignment;
            compiler.Architecture.GetTypeRequirements(signature.Type, out size, out alignment);
            return size;
        }

        private static TokenTypes ValueTokenTypeFromSignature(IMetadataProvider metadata, TokenTypes signatureToken)
        {
            int index = 1;
            byte[] buffer;
            metadata.Read(signatureToken, out buffer);

            // Jump over custom mods
            CustomMod.ParseCustomMods(buffer, ref index);
            index++;

            return SigType.ReadTypeDefOrRefEncoded(buffer, ref index);
        }

		#endregion Methods

	}
}
