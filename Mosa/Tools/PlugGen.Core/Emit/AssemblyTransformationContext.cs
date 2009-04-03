/*
 * (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Bruce Markham       < illuminus86@gmail.com >
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using System.CodeDom;
using System.IO;

namespace PlugGen.Emit
{
    /// <summary>
    /// This class implements context-relative transformations for the emission process, and contains context relevant information
    /// for the transform
    /// </summary>
    /// <typeparam name="TCodeDom">The CodeDomProvider type used to generate output code</typeparam>
    public class AssemblyTransformationContext<TCodeDom>
        where TCodeDom : System.CodeDom.Compiler.CodeDomProvider
    {
        private Dictionary<TypeReference, string> _EffectiveNamespaces = new Dictionary<TypeReference, string>();
        private Dictionary<TypeReference, bool> _AffectedTypes = new Dictionary<TypeReference, bool>();
        private NamespaceToFolderTranslationStrategy _NamespaceFolderTranslationStrategy;

        private TCodeDom _CodeDomProvider;
        private string _GlobalNamespacePrefix;
        private string _TranslatedNamespacePrefix;

        private IEnumerable<IR.LoadedAssembly> _Assemblies;

        public AssemblyTransformationContext(IEnumerable<IR.LoadedAssembly> assemblies, TCodeDom codeDomProvider, string translatedNamespacePrefix)
        {
            if (assemblies == null)
                throw new ArgumentNullException("assemblies");
            if (codeDomProvider==null)
                throw new ArgumentNullException("codeDomProvider");
            if (String.IsNullOrEmpty(translatedNamespacePrefix))
                throw new ArgumentNullException("translatedNamespacePrefix");

            this._Assemblies = assemblies;
            this._CodeDomProvider = codeDomProvider;

            this._GlobalNamespacePrefix = this.GetGlobalNamespacePrefix();
            this._TranslatedNamespacePrefix = "MOSA.Internal";
            this._NamespaceFolderTranslationStrategy = new NamespaceToFolderTranslationStrategy();

        }

        public NamespaceToFolderTranslationStrategy NamespaceFolderTranslationStrategy
        {
            get
            {
                return _NamespaceFolderTranslationStrategy;
            }
        }        

        public bool IsAffectedType(TypeReference typeReference)
        {
            if (typeReference == null)
                throw new ArgumentNullException("typeReference");
            if (!_AffectedTypes.ContainsKey(typeReference))
            {
                _AffectedTypes.Add(typeReference,
                (
                from a in this._Assemblies
                where (
                        from tr in a.Externs.Keys
                        where tr.Equals(typeReference)
                        select true
                        ).FirstOrDefault() == true
                select true
                ).FirstOrDefault()
                );
            }
            return _AffectedTypes[typeReference];
        }

        public string GetEffectiveNamespace(TypeReference typeReference)
        {
            if (typeReference == null)
                throw new ArgumentNullException("typeReference");
            if (!_EffectiveNamespaces.ContainsKey(typeReference))
            {
                if (IsAffectedType(typeReference))
                    _EffectiveNamespaces.Add(typeReference, this._TranslatedNamespacePrefix + typeReference.Namespace);
                else
                    _EffectiveNamespaces.Add(typeReference, typeReference.Namespace);
            }
            return _EffectiveNamespaces[typeReference];
        }
        public bool IsEffectiveNamespaceDifferent(TypeReference typeReference)
        {
            if (typeReference == null)
                throw new ArgumentNullException("typeReference");
            if (IsAffectedType(typeReference))
                return true;
            else
                return false;
        }

        public System.CodeDom.CodeAttributeDeclaration CreateOverrideAttribute(TypeDefinition typeDefinition)
        {
            CodeAttributeDeclaration cad = new CodeAttributeDeclaration("TypeOverridePlug");
            cad.Arguments.Add(new CodeAttributeArgument("OriginalType", CreateTypeInstanceReferencingExpression(new CodeTypeReference(typeDefinition.FullName))));
            return cad;
        }

        public System.CodeDom.CodeAttributeDeclaration CreateOverrideAttribute(MethodDefinition methodDefinition)
        {
            return CreateOverrideAttribute(methodDefinition, true);
        }
        public System.CodeDom.CodeAttributeDeclaration CreateOverrideAttribute(MethodDefinition methodDefinition, bool includeOriginalMethodName)
        {
            CodeAttributeDeclaration cad = new CodeAttributeDeclaration("MethodOverridePlug");
            if (includeOriginalMethodName)
            {
                cad.Arguments.Add(new CodeAttributeArgument("OriginalMethod", new CodePrimitiveExpression(methodDefinition.Name)));
            }
            return cad;
        }

        public System.CodeDom.CodeAttributeDeclaration CreateSymmetricalProjectionAttribute(FieldDefinition fieldDefinition)
        {
            return CreateSymmetricalProjectionAttribute(fieldDefinition, true);
        }

        public System.CodeDom.CodeAttributeDeclaration CreateSymmetricalProjectionAttribute(FieldDefinition fieldDefinition, bool includeOriginalFieldName)
        {
            CodeAttributeDeclaration cad = new CodeAttributeDeclaration("SymmetricalProjectionPlug");
            if (includeOriginalFieldName)
            {
                cad.Arguments.Add(new CodeAttributeArgument("BaseFieldName", new CodePrimitiveExpression(fieldDefinition.Name)));
            }
            return cad;
        }

        public System.CodeDom.CodeAttributeDeclaration CreateInverseOverrideAttribute(MethodDefinition methodDefinition)
        {
            return CreateInverseOverrideAttribute(methodDefinition, true);
        }

        public System.CodeDom.CodeAttributeDeclaration CreateInverseOverrideAttribute(MethodDefinition methodDefinition, bool includeOriginalMethodName)
        {
            CodeAttributeDeclaration cad = new CodeAttributeDeclaration("InverseMethodOverridePlug");
            if (includeOriginalMethodName)
            {
                cad.Arguments.Add(new CodeAttributeArgument("OriginalMethod", new CodePrimitiveExpression(methodDefinition.Name)));
            }
            return cad;
        }

        public System.CodeDom.CodeStatement CreateOverrideNotImplementedExceptionThrower()
        {
                        return new CodeThrowExceptionStatement(
                            new CodeObjectCreateExpression("System.NotImplementedException",
                            new CodePrimitiveExpression("Method plug not implemented."))
                                );
        }

        internal CodeExpression CreateTypeInstanceReferencingExpression(CodeTypeReference typeReference)
        {
            string expressedTypeReference = ExpressTypeReference(typeReference);
            if (typeof(Microsoft.CSharp.CSharpCodeProvider).IsAssignableFrom(typeof(TCodeDom)))
                return new CodeSnippetExpression(String.Format("typeof({0})", expressedTypeReference));
            else if (typeof(Microsoft.VisualBasic.VBCodeProvider).IsAssignableFrom(typeof(TCodeDom)))
                return new CodeSnippetExpression(String.Format("GetType({0})", expressedTypeReference));
            else if (this._CodeDomProvider.FileExtension == "h")
                return new CodeSnippetExpression(String.Format("{0}::typeid", expressedTypeReference));
            else
                return new CodeTypeReferenceExpression(typeReference);
        }

        internal string ExpressTypeReference(CodeTypeReference typeReference)
        {
            StringWriter sw = new StringWriter();
            this._CodeDomProvider.GenerateCodeFromExpression(new CodeTypeReferenceExpression(typeReference), sw, new System.CodeDom.Compiler.CodeGeneratorOptions());
            return sw.ToString();
        }

        internal string GetGlobalNamespacePrefix()
        {
            if (typeof(Microsoft.CSharp.CSharpCodeProvider).IsAssignableFrom(typeof(TCodeDom)))
                return "global::";
            else if (typeof(Microsoft.VisualBasic.VBCodeProvider).IsAssignableFrom(typeof(TCodeDom)))
                return "global.";
            else if (this._CodeDomProvider.FileExtension == "h")
                return "global::";
            else
                throw new NotSupportedException();
        }

        public System.CodeDom.CodeStatement CreateInverseOverrideNotMappedException()
        {
            return new CodeThrowExceptionStatement(
                            new CodeObjectCreateExpression("System.Exception",
                            new CodePrimitiveExpression("Inverse method plug not mapped correctly.") )
                                );
        }
    }
}