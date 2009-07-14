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
using System.CodeDom.Compiler;
using Mono.Cecil;
using System.CodeDom;
using System.Runtime.InteropServices;
using PlugGen.Read;

namespace PlugGen.Emit
{
    public class CodeDomEmitter<TCodeDom>
        : ICodeDomEmitter
        where TCodeDom : CodeDomProvider, new()
    {
        #region Helper Fields, Constructor, & Properties

        private static readonly IEnumerable<string> OutputFileHeader = new List<string>
        {
             "(c) 2009 MOSA - The Managed Operating System Alliance",
             "",
             "Licensed under the terms of the New BSD License.",
             "",
             "Authors:",
             " \tBruce Markham \t\t< illuminus86@gmail.com >"
        }.AsReadOnly();

        private IEnumerable<IR.LoadedAssembly> _Assemblies;
        private IEnumerable<TypeDefinition> _AllTypesRequiringEmission;
        private System.CodeDom.Compiler.CodeGeneratorOptions _CodeGeneratorOptions;

        public CodeDomEmitter(IEnumerable<IR.LoadedAssembly> assemblies, string translatedNamespacePrefix)
        {
            if (assemblies == null)
                throw new ArgumentNullException("assemblies");
            this._Assemblies = assemblies.ToList().AsReadOnly();
            this.CodeDomProvider = new TCodeDom();
            
            this.TransformContext = new AssemblyTransformationContext<TCodeDom>(_Assemblies,this.CodeDomProvider, translatedNamespacePrefix);
            
            this._CodeGeneratorOptions = new CodeGeneratorOptions();
            this._CodeGeneratorOptions.IndentString = "\t";
            this._CodeGeneratorOptions.VerbatimOrder = true;
        }

        public AssemblyTransformationContext<TCodeDom> TransformContext { get; private set; }
        public TCodeDom CodeDomProvider { get; private set; }

        private IEnumerable<TypeDefinition> GetAllTypesRequiringEmssion()
        {
            if (_AllTypesRequiringEmission == null)
            {
                _AllTypesRequiringEmission =
                (
                 from a in _Assemblies
                 select a.GetTypesRequiringEmission()
                ).SelectMany(eT => eT);
            }
            return _AllTypesRequiringEmission;
        }

        #endregion

        #region Base Output Generation
        public void EmitAtBaseFolder(string folderPath)
        {
            var outputItems = GenerateOutputFileItems();
            outputItems.ToList().ForEach(oi => EmitItem(System.IO.Path.Combine(folderPath,oi.A), oi.B));
        }
        private void EmitItem(string filePath, CodeNamespace code)
        {
            var file = new System.IO.FileInfo(filePath);
            //if (file.Exists)
            //    throw new InvalidOperationException("The output file already exists");
            if (!file.Directory.Exists)
                RecursivelyCreateDirectory(file.Directory);
            using(var outputWriter = new System.IO.StreamWriter(filePath,false))
            {
                this.CodeDomProvider.GenerateCodeFromNamespace(code, outputWriter, this._CodeGeneratorOptions);
                outputWriter.Flush();
                outputWriter.Close();
            }
        }
        private void RecursivelyCreateDirectory(System.IO.DirectoryInfo directory)
        {
            if (directory.Parent != null && directory.Parent.Exists == false)
                RecursivelyCreateDirectory(directory.Parent);
            directory.Create();
        }
        private IEnumerable<Tuple<string, CodeNamespace>> GenerateOutputFileItems()
        {
            var outputClassFileItems = GenerateOutputClassFileItems();
            var results =
                from ocfi in outputClassFileItems
                select new Tuple<string, CodeNamespace>(
                    ocfi.FilePath,

                            new CodeNamespace(this.TransformContext.GetEffectiveNamespace(ocfi.OriginalTypeDefinition))
                            {
                                 Types= { ocfi.OutputDefinition }
                            }
                );
            return results;
        }
        private IEnumerable<OutputClassFileItem> GenerateOutputClassFileItems()
        {
            return
                (
                from td in GetAllTypesRequiringEmssion()
                select GenerateOutputClassFileItem(td)
                ).Where(x => x != null);
        }
        private OutputClassFileItem GenerateOutputClassFileItem(TypeDefinition typeDefinition)
        {
            if (typeDefinition.DeclaringType != null)
                return null;

            var namespaceFolder = this.TransformContext.NamespaceFolderTranslationStrategy.GetRelativeFolderPath(typeDefinition.Namespace);
            var fileName = @".\" + typeDefinition.Name + "." + this.CodeDomProvider.FileExtension;
            var filePath = System.IO.Path.Combine(namespaceFolder, fileName);
            IEnumerable<TypeDefinition> nestedTypes;
            var cdt = GenerateCodeTypeDeclaration(typeDefinition, out nestedTypes);
            cdt.Comments.AddRange(OutputFileHeader.Select(ofhl=>new CodeCommentStatement(ofhl,false)).ToArray());

            return new OutputClassFileItem(typeDefinition, nestedTypes, cdt, filePath);
        }
        private CodeTypeDeclaration GenerateCodeTypeDeclaration(TypeDefinition typeDefinition, out IEnumerable<TypeDefinition> nestedTypes)
        {
            var originalTypeNameIsValidIdentifier = this.CodeDomProvider.IsValidIdentifier(typeDefinition.Name);
            CodeTypeDeclaration cdt = null;
            if(originalTypeNameIsValidIdentifier)
                cdt = new CodeTypeDeclaration(typeDefinition.Name);
            else
                cdt = new CodeTypeDeclaration(this.CodeDomProvider.CreateValidIdentifier(typeDefinition.Name));

            if (typeDefinition.IsEnum)
                cdt.IsEnum = true;
            if (typeDefinition.IsInterface)
                cdt.IsInterface = true;
            if (typeDefinition.IsClass)
                cdt.IsClass = true;
            if (typeDefinition.GenericParameters.Count > 0)
                cdt.TypeParameters.AddRange(GenerateTypeParameters(typeDefinition.GenericParameters.Cast<GenericParameter>()));
            if (typeDefinition.IsValueType && !typeDefinition.IsEnum)
                cdt.IsStruct = true;
            var typeDefinitionCanonicalName = AssemblyReader.GetCanonicalName(typeDefinition);
            cdt.BaseTypes.AddRange(
                (
                from implementedInterface in typeDefinition.Interfaces.Cast<TypeReference>()
                where AssemblyReader.GetCanonicalName(implementedInterface)!=typeDefinitionCanonicalName
                select TranslateTypeReference(implementedInterface)
                ).ToArray());
            if (typeDefinition.BaseType != null)
                cdt.BaseTypes.Add(TranslateTypeReference(typeDefinition));

            cdt.CustomAttributes.Add(this.TransformContext.CreateOverrideAttribute(typeDefinition));
            // TODO : Attach other attributes

            cdt.TypeAttributes = GenerateReflectionTypeAttributes(typeDefinition);

            cdt.Members.AddRange(GenerateFieldDeclarations(typeDefinition).ToArray());
            cdt.Members.AddRange(GenerateMethodDeclarations(typeDefinition).ToArray());
            cdt.Members.AddRange(GenerateNestedTypeDeclarations(typeDefinition).ToArray());

            nestedTypes = typeDefinition.NestedTypes.Cast<TypeDefinition>();

            return cdt;
        }
        private CodeTypeDeclaration GenerateCodeTypeDeclaration(TypeDefinition typeDefinition)
        {
            IEnumerable<TypeDefinition> nestedTypes;
            return GenerateCodeTypeDeclaration(typeDefinition, out nestedTypes);
        }

        #endregion

        #region Type-Level Operations and Translations
        private System.Reflection.TypeAttributes GenerateReflectionTypeAttributes(TypeDefinition typeDefinition)
        {
            System.Reflection.TypeAttributes result = (System.Reflection.TypeAttributes)0;

            if (!typeDefinition.IsInterface && typeDefinition.IsAbstract)
                result |= System.Reflection.TypeAttributes.Abstract;
            if (typeDefinition.IsAnsiClass)
                result |= System.Reflection.TypeAttributes.AnsiClass;
            if (typeDefinition.IsAutoClass)
                result |= System.Reflection.TypeAttributes.AutoClass;
            if (typeDefinition.IsAutoLayout)
                result = result | System.Reflection.TypeAttributes.AutoLayout;
            if (typeDefinition.IsBeforeFieldInit)
                result |= System.Reflection.TypeAttributes.BeforeFieldInit;
            if (typeDefinition.IsClass)
                result |= System.Reflection.TypeAttributes.Class;
            // TODO : result |= System.Reflection.TypeAttributes.ClassSemanticsMask
            // TODO : result |= System.Reflection.TypeAttributes.CustomFormatClass
            // TODO : result |= System.Reflection.TypeAttributes.CustomFormatMask
            if (typeDefinition.IsExplicitLayout)
                result |= System.Reflection.TypeAttributes.ExplicitLayout;
            // TODO : result |= System.Reflection.TypeAttributes.HasSecurity
            if (typeDefinition.IsImport)
                result |= System.Reflection.TypeAttributes.Import;
            if (typeDefinition.IsInterface)
                result |= System.Reflection.TypeAttributes.Interface;
            if (typeDefinition.HasLayoutInfo)
                result |= System.Reflection.TypeAttributes.LayoutMask;
            if (typeDefinition.IsNestedAssembly)
                result |= System.Reflection.TypeAttributes.NestedAssembly;
            if (typeDefinition.IsNestedFamilyAndAssembly)
                result |= System.Reflection.TypeAttributes.NestedFamANDAssem;
            if (typeDefinition.IsNestedFamily)
                result |= System.Reflection.TypeAttributes.NestedFamily;
            if (typeDefinition.IsNestedFamilyOrAssembly)
                result |= System.Reflection.TypeAttributes.NestedFamORAssem;
            if (typeDefinition.IsNestedPrivate)
                result |= System.Reflection.TypeAttributes.NestedPrivate;
            if (typeDefinition.IsNestedPublic)
                result |= System.Reflection.TypeAttributes.NestedPublic;
            if (typeDefinition.IsNotPublic)
                result |= System.Reflection.TypeAttributes.NotPublic;
            if (typeDefinition.IsPublic)
                result |= System.Reflection.TypeAttributes.Public;
            // TODO : result |= System.Reflection.TypeAttributes.ReservedMask
            if (typeDefinition.IsRuntimeSpecialName)
                result |= System.Reflection.TypeAttributes.RTSpecialName;
            if (typeDefinition.IsSealed)
                result |= System.Reflection.TypeAttributes.Sealed;
            if (typeDefinition.IsSequentialLayout)
                result |= System.Reflection.TypeAttributes.SequentialLayout;
            if (typeDefinition.IsSerializable)
                result |= System.Reflection.TypeAttributes.Serializable;
            if (typeDefinition.IsSpecialName)
                result |= System.Reflection.TypeAttributes.SpecialName;
            // TODO : result |= System.Reflection.TypeAttributes.StringFormatMask
            if (typeDefinition.IsUnicodeClass)
                result |= System.Reflection.TypeAttributes.UnicodeClass;
            // TODO : result |= System.Reflection.TypeAttributes.VisibilityMask

            return result;
        }

        private CodeTypeParameter[] GenerateTypeParameters(IEnumerable<GenericParameter> genericParameters)
        {
            return genericParameters.Select(
                gp =>
                {
                    return GenerateTypeParameter(gp);
                }
            ).ToArray();
        }
        private CodeTypeParameter GenerateTypeParameter(GenericParameter genericParameter)
        {
            var result = new CodeTypeParameter(genericParameter.Name);
            result.Constraints.AddRange(
                (
                from constraint in genericParameter.Constraints.Cast<TypeReference>()
                select TranslateTypeReference(constraint)
                ).ToArray()
                );
            if (genericParameter.HasDefaultConstructorConstraint)
                result.HasConstructorConstraint = true;

            if (genericParameter.HasNotNullableValueTypeConstraint || genericParameter.IsValueType)
                result.Constraints.Add(TranslateTypeReference(new TypeReference("ValueType","System",null,true)));
            if (genericParameter.HasReferenceTypeConstraint)
                result.Constraints.Add(TranslateTypeReference(new TypeReference("Object","System",null,false)));

            return result;
        }

        private IEnumerable<CodeTypeDeclaration> GenerateNestedTypeDeclarations(TypeDefinition parentType)
        {
            IEnumerable<TypeDefinition> nestedTypes = parentType.NestedTypes.Cast<TypeDefinition>();
            return
                from ntd in parentType.NestedTypes.Cast<TypeDefinition>()
                select GenerateCodeTypeDeclaration(ntd);
        }

        private CodeTypeReference TranslateTypeReference(TypeReference typeReference)
        {
            if (typeReference == null)
                throw new ArgumentNullException("typeReference");

            CodeTypeReference result = new CodeTypeReference();

            if (typeReference is ArrayType)
            {
                result.ArrayElementType = TranslateTypeReference((typeReference as ArrayType).ElementType);
                result.ArrayRank = (typeReference as ArrayType).Rank;
            }
            else if (typeReference is GenericParameter)
            {
                result.BaseType = typeReference.Name;
                result.Options = CodeTypeReferenceOptions.GenericTypeParameter;
            }
            else
            {
                result.BaseType = this.TransformContext.GetEffectiveNamespace(typeReference) + '.' + typeReference.Name;
            }

            if (typeReference.DeclaringType != null)
            {
                result.BaseType = TranslateTypeReference(typeReference.DeclaringType) + "." + result.BaseType;
            }

            if (typeReference.GenericParameters.Count > 0)
            {
                result.TypeArguments.AddRange(
                    (
                    from gp in typeReference.GenericParameters.Cast<TypeReference>()
                    select TranslateTypeReference(gp)
                    ).ToArray()
                    );
            }

            return result;
        }
        [Obsolete]
        private CodeTypeReference TranslateTypeReference(string fullTypeName)
        {
            var typeDefinition = LookupTypeReference(fullTypeName);
            if (typeDefinition == null)
                return new CodeTypeReference(fullTypeName);
            else
                return CreateCodeTypeReference(typeDefinition);
        }
        private CodeTypeReference CreateCodeTypeReference(TypeDefinition td)
        {
            if (td == null)
                throw new ArgumentNullException("td");

            if (this._AllTypesRequiringEmission.Contains(td))
            {
                var originalFullTypeName = td.FullName;
                var originalTypeNamespace = td.Namespace;
                var effectiveNamespace = this.TransformContext.GetEffectiveNamespace(td as TypeReference);
                if (effectiveNamespace != originalTypeNamespace)
                {
                    return new CodeTypeReference(effectiveNamespace + "." + originalFullTypeName.Substring(originalTypeNamespace.Length));
                }
                else
                    return new CodeTypeReference(td.FullName);
            }
            else
                return new CodeTypeReference(td.FullName);
        }
        private TypeDefinition LookupTypeReference(TypeReference typeReference)
        {
            var canonicalName = AssemblyReader.GetCanonicalName(typeReference);
            return
                (
                from a in this._Assemblies
                where a.TypeDefinitionMappings.ContainsKey(canonicalName)
                select a.TypeDefinitionMappings[canonicalName]
                ).Union(
                from a in this._Assemblies
                where a.ReferencedTypeDefinitionMappings.ContainsKey(canonicalName)
                select a.ReferencedTypeDefinitionMappings[canonicalName]
                ).FirstOrDefault();
        }

        [Obsolete]
        private TypeDefinition LookupTypeReference(string fullTypeName)
        {
            var result =
                this._Assemblies.SelectMany(a => a.GetAllDefinedTypes().Where(td => td.FullName == fullTypeName))
                .FirstOrDefault();
            return result;
        }
        #endregion

        #region Field Definitions and Declarations
        private IEnumerable<CodeMemberField> GenerateFieldDeclarations(TypeDefinition typeDefinition)
        {
            return
                from f in typeDefinition.Fields.Cast<FieldDefinition>()
                orderby f.Offset
                select GenerateFieldDeclaration(f);
        }
        private CodeMemberField GenerateFieldDeclaration(FieldDefinition fieldDefinition)
        {
            var originalFieldNameIsSyntacticallyCorrect = this.CodeDomProvider.IsValidIdentifier(fieldDefinition.Name);
            CodeMemberField cmf;

            if (originalFieldNameIsSyntacticallyCorrect)
                cmf = new CodeMemberField(TranslateTypeReference(fieldDefinition.FieldType), fieldDefinition.Name);
            else
                cmf = new CodeMemberField(TranslateTypeReference(fieldDefinition.FieldType), this.CodeDomProvider.CreateValidIdentifier(fieldDefinition.Name));
            cmf.Attributes = GenerateCodeDomMemberAttributes(fieldDefinition);
            cmf.CustomAttributes.Add(this.TransformContext.CreateSymmetricalProjectionAttribute(fieldDefinition, !originalFieldNameIsSyntacticallyCorrect));

            if (fieldDefinition.Constant != null)
            {
//                cmf.InitExpression = new CodeFieldReferenceExpression( CreateCodeTypeReference(LookupTypeReference(fieldDefinition.DeclaringType)), fieldDefinition.Name);
            }

            return cmf;
        }
        private System.CodeDom.MemberAttributes GenerateCodeDomMemberAttributes(FieldDefinition fieldDefinition)
        {
            System.CodeDom.MemberAttributes result = (System.CodeDom.MemberAttributes)0;

            // N/A: result |= MemberAttributes.Abstract;
            // TODO: result |= MemberAttributes.AccessMask;
            if (fieldDefinition.IsAssembly)
                result |= MemberAttributes.Assembly;
            if (fieldDefinition.HasConstant)
                result |= MemberAttributes.Const;
            if (fieldDefinition.IsFamily)
                result |= MemberAttributes.Family;
            if (fieldDefinition.IsFamilyAndAssembly)
                result |= MemberAttributes.FamilyAndAssembly;
            if (fieldDefinition.IsFamilyOrAssembly)
                result |= MemberAttributes.FamilyOrAssembly;
            // N/A: result |= MemberAttributes.Final;
            // TODO: result |= MemberAttributes.New;
            // TODO: result |= MemberAttributes.Overloaded;
            // TODO: result |= MemberAttributes.Override;
            if (fieldDefinition.IsPrivate)
                result |= MemberAttributes.Private;
            if (fieldDefinition.IsPublic)
                result |= MemberAttributes.Public;
            // TODO: result |= MemberAttributes.ScopeMask;
            if (fieldDefinition.IsStatic)
                result |= MemberAttributes.Static;
            // TODO: result |= MemberAttributes.VTableMask;

            return result;
        }
        private System.Reflection.FieldAttributes GenerateReflectionFieldAttributes(FieldDefinition fieldDefinition)
        {
            throw new InvalidOperationException();
            //System.Reflection.FieldAttributes result = (System.Reflection.FieldAttributes)0;

            //if (fieldDefinition.IsAssembly)
            //    result |= System.Reflection.FieldAttributes.Assembly;
            //if (fieldDefinition.IsFamilyAndAssembly)
            //    result |= System.Reflection.FieldAttributes.FamANDAssem;
            //if (fieldDefinition.IsFamily)
            //    result |= System.Reflection.FieldAttributes.Family;
            //if (fieldDefinition.IsFamilyOrAssembly)
            //    result |= System.Reflection.FieldAttributes.FamORAssem;
            //// TODO: result |= System.Reflection.FieldAttributes.FieldAccessMask;
            //if (fieldDefinition.HasDefault)
            //    result |= System.Reflection.FieldAttributes.HasDefault;
            //if (fieldDefinition.MarshalSpec != null)
            //    result |= System.Reflection.FieldAttributes.HasFieldMarshal;
            //if (fieldDefinition.RVA != null)
            //    result |= System.Reflection.FieldAttributes.HasFieldRVA;
            //if (fieldDefinition.IsInitOnly)
            //    result |= System.Reflection.FieldAttributes.InitOnly;
            //if (fieldDefinition.IsLiteral)
            //    result |= System.Reflection.FieldAttributes.Literal;
            //if (fieldDefinition.IsNotSerialized)
            //    result |= System.Reflection.FieldAttributes.NotSerialized;
            ///*
            //        if(fieldDefinition.IsPInvokeImpl)
            //            result |= System.Reflection.FieldAttributes.PinvokeImpl;
            // */
            //if (fieldDefinition.IsPrivate)
            //    result |= System.Reflection.FieldAttributes.Private;
            //// TODO: result |= System.Reflection.FieldAttributes.PrivateScope;
            //if (fieldDefinition.IsPublic)
            //    result |= System.Reflection.FieldAttributes.Public;
            //// TODO: result |= System.Reflection.FieldAttributes.ReservedMask;
            //if (fieldDefinition.IsRuntimeSpecialName)
            //    result |= System.Reflection.FieldAttributes.RTSpecialName;
            //if (fieldDefinition.IsSpecialName)
            //    result |= System.Reflection.FieldAttributes.SpecialName;
            //if (fieldDefinition.IsStatic)
            //    result |= System.Reflection.FieldAttributes.Static;

            //return result;
        }
        #endregion

        #region Method Definitions and Declarations
        private IEnumerable<CodeMemberMethod> GenerateMethodDeclarations(TypeDefinition typeDefinition)
        {
            return
                    (
                    from mdInfo in Read.AssemblyReader.GetAllMethods(typeDefinition)
                    orderby 
                              AssemblyReader.IsExterned(mdInfo.Key) descending
                            , mdInfo.Value != null
                            , (mdInfo.Key.IsConstructor) descending
                            , (mdInfo.Value == null ? "" : mdInfo.Value.GetType().FullName)
                            , mdInfo.Key.Name
                    select GenerateMethodDeclaration(mdInfo.Key, mdInfo.Value)
                    );
        }

        private CodeMemberMethod GenerateMethodDeclaration(MethodDefinition methodDefinition, MemberReference associatedMember)
        {
            var isConstructor = methodDefinition.IsConstructor;
            var isStaticConstructor = isConstructor && methodDefinition.IsStatic;
            var originalMethodNameIsSyntacticallyCorrect = this.CodeDomProvider.IsValidIdentifier(methodDefinition.Name);
            
            CodeMemberMethod cmm;
            if (isConstructor && !isStaticConstructor)
                cmm = new CodeConstructor();
            else if (isStaticConstructor)
                cmm = new CodeTypeConstructor();
            else
                cmm = new CodeMemberMethod();

            if (originalMethodNameIsSyntacticallyCorrect && !isConstructor)
                cmm.Name = methodDefinition.Name;
            else if (!isConstructor)
                cmm.Name = this.CodeDomProvider.CreateValidIdentifier(methodDefinition.Name);

            if (Read.AssemblyReader.IsExterned(methodDefinition))
            {
                cmm.CustomAttributes.Add(this.TransformContext.CreateOverrideAttribute(methodDefinition,!originalMethodNameIsSyntacticallyCorrect));
            }
            else
            {
                cmm.CustomAttributes.Add(this.TransformContext.CreateInverseOverrideAttribute(methodDefinition, !originalMethodNameIsSyntacticallyCorrect));
            }

            // TODO: Add other custom attributes (on the return type, on the method itself)

            cmm.TypeParameters.AddRange(GenerateTypeParameters(methodDefinition.GenericParameters.Cast<GenericParameter>()));

            cmm.Parameters.AddRange(GenerateMethodParameters(methodDefinition));

            foreach(var overrideMethodReference in methodDefinition.Overrides.Cast<MethodReference>())
            {
                var overrideMethodDeclaringType = LookupTypeReference(overrideMethodReference.DeclaringType.GetOriginalType());
                if (methodDefinition.IsFamily && overrideMethodDeclaringType.IsInterface)
                {
                    if (cmm.PrivateImplementationType == null)
                    {
                        cmm.PrivateImplementationType = TranslateTypeReference(overrideMethodDeclaringType);
                    }
                    else
                        throw new NotSupportedException("Multiple private implementation types for a method is not supported");
                }
                else
                {
                    cmm.ImplementationTypes.Add(TranslateTypeReference(overrideMethodDeclaringType));
                }
            }

            cmm.ReturnType = TranslateTypeReference(methodDefinition.ReturnType.ReturnType);

            if (Read.AssemblyReader.IsExterned(methodDefinition))
            {
                cmm.Statements.Add(this.TransformContext.CreateOverrideNotImplementedExceptionThrower());
            }
            else
            {
                cmm.Statements.Add(this.TransformContext.CreateInverseOverrideNotMappedException());
            }

            return cmm;
        }
        private System.CodeDom.MemberAttributes GenerateCodeDomMemberAttributes(MethodDefinition methodDefinition)
        {
            System.CodeDom.MemberAttributes result = (System.CodeDom.MemberAttributes)0;

            if (methodDefinition.IsAbstract)
                result |= MemberAttributes.Abstract;
            // TODO: result |= MemberAttributes.AccessMask;
            if (methodDefinition.IsAssembly)
                result |= MemberAttributes.Assembly;
            // N/A: result |= MemberAttributes.Const;
            if (methodDefinition.IsFamily)
                result |= MemberAttributes.Family;
            if (methodDefinition.IsFamilyAndAssembly)
                result |= MemberAttributes.FamilyAndAssembly;
            if (methodDefinition.IsFamilyOrAssembly)
                result |= MemberAttributes.FamilyOrAssembly;
            if (methodDefinition.IsFinal)
                result |= MemberAttributes.Final;
            if (methodDefinition.IsNewSlot)
                result |= MemberAttributes.New;
            if (methodDefinition.IsHideBySig)
                result |= MemberAttributes.Overloaded;
            if (methodDefinition.Overrides.Count>0)
                result |= MemberAttributes.Override;
            if (methodDefinition.IsPrivate)
                result |= MemberAttributes.Private;
            if (methodDefinition.IsPublic)
                result |= MemberAttributes.Public;
            // TODO: result |= MemberAttributes.ScopeMask;
            if (methodDefinition.IsStatic)
                result |= MemberAttributes.Static;
            // TODO: result |= MemberAttributes.VTableMask;

            return result;
        }
        private CodeParameterDeclarationExpression[] GenerateMethodParameters(MethodDefinition methodDefinition)
        {
            return GenerateMethodParameters(methodDefinition.Parameters.Cast<ParameterDefinition>());
        }
        private CodeParameterDeclarationExpression[] GenerateMethodParameters(IEnumerable<ParameterDefinition> parameters)
        {
            return
                (
                from pd in parameters
                select GenerateMethodParameter(pd)
                ).ToArray();
        }
        private CodeParameterDeclarationExpression GenerateMethodParameter(ParameterDefinition parameterDefinition)
        {
            var result = new CodeParameterDeclarationExpression(TranslateTypeReference(parameterDefinition.ParameterType), parameterDefinition.Name);

            // TODO: result.CustomAttributes.AddRange( )

            if (parameterDefinition.IsOut && !parameterDefinition.IsIn)
                result.Direction = FieldDirection.Out;
            else if (parameterDefinition.IsIn && !parameterDefinition.IsOut)
                result.Direction = FieldDirection.In;
            else if (parameterDefinition.IsIn && parameterDefinition.IsOut)
                result.Direction = FieldDirection.Ref;
            else
                result.Direction = FieldDirection.In; //throw new NotImplementedException();

            return result;
        }
        #endregion

        #region ICodeDomEmitter Members

        CodeDomProvider ICodeDomEmitter.CodeDomProvider
        {
            get { return this.CodeDomProvider; }
        }

        #endregion
    }
}
