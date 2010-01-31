using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.CompilerFramework
{
    /// <summary>
    /// 
    /// </summary>
    public static class ObjectModelUtility
    {
		/// <summary>
		/// Computes the size of the type.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="metadataProvider">The metadata provider.</param>
		/// <param name="architecture">The architecture.</param>
		/// <returns></returns>
        public static int ComputeTypeSize(TokenTypes token, IMetadataProvider metadataProvider, IArchitecture architecture)
        {
            Metadata.Tables.TypeDefRow typeDefinition;
            Metadata.Tables.TypeDefRow followingTypeDefinition = new Mosa.Runtime.Metadata.Tables.TypeDefRow();
            metadataProvider.Read(token, out typeDefinition);
            try
            {
                metadataProvider.Read(token + 1, out followingTypeDefinition);
            }
            catch (System.Exception)
            {
            }

            int result = 0;
            TokenTypes fieldList = typeDefinition.FieldList;
            TokenTypes last = metadataProvider.GetMaxTokenValue(TokenTypes.Field);
            while (fieldList != followingTypeDefinition.FieldList && fieldList != last)
                result += FieldSize(fieldList++, metadataProvider, architecture);

            return result;
        }

		/// <summary>
		/// Fields the size.
		/// </summary>
		/// <param name="field">The field.</param>
		/// <param name="metadataProvider">The metadata provider.</param>
		/// <param name="architecture">The architecture.</param>
		/// <returns></returns>
        public static int FieldSize(TokenTypes field, IMetadataProvider metadataProvider, IArchitecture architecture)
        {
            Metadata.Tables.FieldRow fieldRow;
            metadataProvider.Read(field, out fieldRow);
            FieldSignature signature = Signature.FromMemberRefSignatureToken(metadataProvider, fieldRow.SignatureBlobIdx) as FieldSignature;

            // If the field is another struct, we have to dig down and compute its size too.
            if (signature.Type.Type == CilElementType.ValueType)
            {
                TokenTypes valueTypeSig = ValueTokenTypeFromSignature(metadataProvider, fieldRow.SignatureBlobIdx);
                return ComputeTypeSize(valueTypeSig, metadataProvider, architecture);
            }

            int size, alignment;
            architecture.GetTypeRequirements(signature.Type, out size, out alignment);
            return size;
        }

		/// <summary>
		/// Values the token type from signature.
		/// </summary>
		/// <param name="metadata">The metadata.</param>
		/// <param name="signatureToken">The signature token.</param>
		/// <returns></returns>
        public static TokenTypes ValueTokenTypeFromSignature(IMetadataProvider metadata, TokenTypes signatureToken)
        {
            int index = 1;
            byte[] buffer;
            metadata.Read(signatureToken, out buffer);

            // Jump over custom mods
            CustomMod.ParseCustomMods(buffer, ref index);
            index++;

            return SigType.ReadTypeDefOrRefEncoded(buffer, ref index);
        }

		/// <summary>
		/// Computes the field offset.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="metadataProvider">The metadata provider.</param>
		/// <param name="architecture">The architecture.</param>
		/// <returns></returns>
        public static int ComputeFieldOffset(TokenTypes token, IMetadataProvider metadataProvider, IArchitecture architecture)
        {
            Metadata.Tables.TypeDefRow typeDefinition;
            Metadata.Tables.TypeDefRow followingTypeDefinition;
            metadataProvider.Read(token, out typeDefinition);
            metadataProvider.Read(token + 1, out followingTypeDefinition);

            int result = 0;
            TokenTypes fieldList = typeDefinition.FieldList;
            while (fieldList != token)
                result += FieldSize(fieldList++, metadataProvider, architecture);

            return result;
        }
    }
}
