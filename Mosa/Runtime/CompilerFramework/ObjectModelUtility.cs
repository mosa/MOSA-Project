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
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="compiler"></param>
        /// <returns></returns>
        public static int ComputeTypeSize(TokenTypes token, IMethodCompiler compiler)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="compiler"></param>
        /// <returns></returns>
        public static int FieldSize(TokenTypes field, IMethodCompiler compiler)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="signatureToken"></param>
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
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="compiler"></param>
        /// <returns></returns>
        public static int ComputeFieldOffset(TokenTypes token, IMethodCompiler compiler)
        {
            IMetadataProvider metadata = compiler.Assembly.Metadata;
            Metadata.Tables.TypeDefRow typeDefinition;
            Metadata.Tables.TypeDefRow followingTypeDefinition;
            metadata.Read(token, out typeDefinition);
            metadata.Read(token + 1, out followingTypeDefinition);

            int result = 0;
            TokenTypes fieldList = typeDefinition.FieldList;
            while (fieldList != token)
                result += FieldSize(fieldList++, compiler);

            return result;
        }
    }
}
