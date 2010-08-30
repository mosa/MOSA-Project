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
		public static int ComputeTypeSize(ISignatureContext context, TokenTypes token, IMetadataProvider metadataProvider, IArchitecture architecture)
		{
			Metadata.Tables.TypeDefRow followingTypeDefinition = new Mosa.Runtime.Metadata.Tables.TypeDefRow();
			Metadata.Tables.TypeDefRow typeDefinition = metadataProvider.ReadTypeDefRow(token);
			try
			{
				followingTypeDefinition = metadataProvider.ReadTypeDefRow(token + 1);
			}
			catch (System.Exception)
			{
			}

			int result = 0;
			TokenTypes fieldList = typeDefinition.FieldList;
			TokenTypes last = metadataProvider.GetMaxTokenValue(TokenTypes.Field);
			while (fieldList != followingTypeDefinition.FieldList && fieldList != last)
				result += FieldSize(context, fieldList++, metadataProvider, architecture);

			return result;
		}

		/// <summary>
		/// Fields the size.
		/// </summary>
		/// <param name="field">The field.</param>
		/// <param name="metadataProvider">The metadata provider.</param>
		/// <param name="architecture">The architecture.</param>
		/// <returns></returns>
		public static int FieldSize(ISignatureContext context, TokenTypes field, IMetadataProvider metadataProvider, IArchitecture architecture)
		{
			Metadata.Tables.FieldRow fieldRow = metadataProvider.ReadFieldRow(field);
			FieldSignature signature = Signature.FromMemberRefSignatureToken(context, metadataProvider, fieldRow.SignatureBlobIdx) as FieldSignature;

			// If the field is another struct, we have to dig down and compute its size too.
			if (signature.Type.Type == CilElementType.ValueType)
			{
				TokenTypes valueTypeSig = ValueTokenTypeFromSignature(metadataProvider, fieldRow.SignatureBlobIdx);
				return ComputeTypeSize(context, valueTypeSig, metadataProvider, architecture);
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
        public static TokenTypes ValueTokenTypeFromSignature(IMetadataProvider provider, TokenTypes signatureToken)
		{
            SignatureReader reader = new SignatureReader(provider.ReadBlob(signatureToken), provider, signatureToken);
			reader.SkipByte();

			// Jump over custom mods
			CustomMod.ParseCustomMods(reader);
			reader.SkipByte();

			return reader.ReadEncodedTypeDefOrRef(); ;
		}

		/// <summary>
		/// Computes the field offset.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="metadataProvider">The metadata provider.</param>
		/// <param name="architecture">The architecture.</param>
		/// <returns></returns>
		public static int ComputeFieldOffset(ISignatureContext context, TokenTypes token, IMetadataProvider metadataProvider, IArchitecture architecture)
		{
			Metadata.Tables.TypeDefRow typeDefinition = metadataProvider.ReadTypeDefRow(token);
			Metadata.Tables.TypeDefRow followingTypeDefinition = metadataProvider.ReadTypeDefRow(token + 1);

			int result = 0;
			TokenTypes fieldList = typeDefinition.FieldList;
			while (fieldList != token)
				result += FieldSize(context, fieldList++, metadataProvider, architecture);

			return result;
		}
	}
}
