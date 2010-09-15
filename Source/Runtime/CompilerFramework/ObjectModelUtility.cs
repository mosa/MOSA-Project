using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Vm;

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
		/// <param name="context">The context.</param>
		/// <param name="token">The token.</param>
		/// <param name="moduleTypeSystem">The module type system.</param>
		/// <param name="architecture">The architecture.</param>
		/// <returns></returns>
		public static int ComputeTypeSize(ISignatureContext context, TokenTypes token, IModuleTypeSystem moduleTypeSystem, IArchitecture architecture)
		{
			Metadata.Tables.TypeDefRow followingTypeDefinition = new Mosa.Runtime.Metadata.Tables.TypeDefRow();
			Metadata.Tables.TypeDefRow typeDefinition = moduleTypeSystem.MetadataModule.Metadata.ReadTypeDefRow(token);
			try
			{
				followingTypeDefinition =  moduleTypeSystem.MetadataModule.Metadata.ReadTypeDefRow(token + 1);
			}
			catch (System.Exception)
			{
			}

			int result = 0;
			TokenTypes fieldList = typeDefinition.FieldList;
			TokenTypes last =  moduleTypeSystem.MetadataModule.Metadata.GetMaxTokenValue(TokenTypes.Field);
			while (fieldList != followingTypeDefinition.FieldList && fieldList != last)
				result += FieldSize(context, fieldList++, moduleTypeSystem, architecture);

			return result;
		}

		/// <summary>
		/// Fields the size.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="field">The field.</param>
		/// <param name="moduleTypeSystem">The module type system.</param>
		/// <param name="architecture">The architecture.</param>
		/// <returns></returns>
		public static int FieldSize(ISignatureContext context, TokenTypes field, IModuleTypeSystem moduleTypeSystem, IArchitecture architecture)
		{
			Metadata.Tables.FieldRow fieldRow = moduleTypeSystem.MetadataModule.Metadata.ReadFieldRow(field);
			FieldSignature signature = Signature.FromMemberRefSignatureToken(context, moduleTypeSystem.MetadataModule.Metadata, fieldRow.SignatureBlobIdx) as FieldSignature;

			// If the field is another struct, we have to dig down and compute its size too.
			if (signature.Type.Type == CilElementType.ValueType)
			{
				TokenTypes valueTypeSig = ValueTokenTypeFromSignature(moduleTypeSystem, fieldRow.SignatureBlobIdx);
				return ComputeTypeSize(context, valueTypeSig, moduleTypeSystem, architecture);
			}

			int size, alignment;
			architecture.GetTypeRequirements(signature.Type, out size, out alignment);
			return size;
		}

		/// <summary>
		/// Values the token type from signature.
		/// </summary>
		/// <param name="moduleTypeSystem">The module type system.</param>
		/// <param name="signatureToken">The signature token.</param>
		/// <returns></returns>
		public static TokenTypes ValueTokenTypeFromSignature(IModuleTypeSystem moduleTypeSystem, TokenTypes signatureToken)
		{
			SignatureReader reader = new SignatureReader(moduleTypeSystem.MetadataModule.Metadata.ReadBlob(signatureToken), signatureToken);
			reader.SkipByte();

			// Jump over custom mods
			CustomMod.ParseCustomMods(reader);
			reader.SkipByte();

			return reader.ReadEncodedTypeDefOrRef();
		}

		/// <summary>
		/// Computes the field offset.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="token">The token.</param>
		/// <param name="moduleTypeSystem">The module type system.</param>
		/// <param name="architecture">The architecture.</param>
		/// <returns></returns>
		public static int ComputeFieldOffset(ISignatureContext context, TokenTypes token, IModuleTypeSystem moduleTypeSystem, IArchitecture architecture)
		{
			Metadata.Tables.TypeDefRow typeDefinition = moduleTypeSystem.MetadataModule.Metadata.ReadTypeDefRow(token);
			Metadata.Tables.TypeDefRow followingTypeDefinition = moduleTypeSystem.MetadataModule.Metadata.ReadTypeDefRow(token + 1);

			int result = 0;
			TokenTypes fieldList = typeDefinition.FieldList;
			while (fieldList != token)
				result += FieldSize(context, fieldList++, moduleTypeSystem, architecture);

			return result;
		}
	}
}
