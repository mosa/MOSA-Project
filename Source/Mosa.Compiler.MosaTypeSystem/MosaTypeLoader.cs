/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Loader;
using Mosa.Compiler.Metadata.Signatures;
using Mosa.Compiler.Metadata.Tables;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.MosaTypeSystem
{
	/// <summary>
	///
	/// </summary>
	public class MosaTypeLoader
	{
		#region Data members

		/// <summary>
		/// The table count
		/// </summary>
		private const int TableCount = ((int)TableType.GenericParamConstraint >> 24) + 1;

		/// <summary>
		/// The resolver
		/// </summary>
		private readonly MosaTypeResolver resolver;

		/// <summary>
		/// Holds the metadata provider
		/// </summary>
		private IMetadataProvider metadataProvider;

		/// <summary>
		/// Holds the metadata module
		/// </summary>
		private IMetadataModule metadataModule;

		/// <summary>
		/// The strings
		/// </summary>
		private Dictionary<HeapIndexToken, string> strings;

		/// <summary>
		/// The signatures
		/// </summary>
		private Dictionary<HeapIndexToken, Signature> signatures;

		/// <summary>
		/// The map generic parameter
		/// </summary>
		private Dictionary<Token, MosaGenericParameter> mapGenericParam;

		/// <summary>
		/// The generic types
		/// </summary>
		private List<MosaType> genericTypes;

		/// <summary>
		/// The internal assembly
		/// </summary>
		private MosaAssembly internalAssembly = null;

		/// <summary>
		/// The table rows
		/// </summary>
		private int[] tableRows = new int[TableCount];

		/// <summary>
		/// The assembly
		/// </summary>
		private MosaAssembly assembly;

		#endregion Data members

		private MosaTypeLoader(MosaTypeResolver resolver)
		{
			this.resolver = resolver;
		}

		public static void Load(IMetadataModule metadataModule, MosaTypeResolver resolver)
		{
			var loader = new MosaTypeLoader(resolver);
			loader.Load(metadataModule);
		}

		private void Load(IMetadataModule metadataModule)
		{
			this.metadataModule = metadataModule;
			this.metadataProvider = metadataModule.Metadata;

			RetrieveAllTableSizes();

			this.signatures = new Dictionary<HeapIndexToken, Signature>();
			this.strings = new Dictionary<HeapIndexToken, string>();
			this.mapGenericParam = new Dictionary<Token, MosaGenericParameter>();
			this.genericTypes = new List<MosaType>();

			assembly = new MosaAssembly();
			assembly.Name = metadataModule.Name;

			resolver.AddAssembly(assembly);

			// Load all types from the assembly into the type array
			LoadTypeReferences();
			LoadTypes();
			LoadTypeSpecs();
			LoadMemberReferences();
			LoadGenericParams();
			LoadCustomAttributes();
			LoadInterfaces();			
			LoadGenericParamContraints();

			// release
			this.metadataModule = null;
			this.metadataProvider = null;
			this.signatures = null;
			this.strings = null;
			this.mapGenericParam = null;
			this.internalAssembly = null;
			this.genericTypes = null;
		}

		#region Internals

		/// <summary>
		///	Computes the size of each table and stores it for faster lookup
		/// </summary>
		private void RetrieveAllTableSizes()
		{
			for (int i = 0; i < TableCount; i++)
			{
				tableRows[i] = metadataProvider.GetRowCount((TableType)(i << 24));
			}
		}

		/// <summary>
		/// Gets the table rows.
		/// </summary>
		/// <param name="tokenType">Type of the token.</param>
		/// <returns></returns>
		private int GetTableRows(TableType tokenType)
		{
			return tableRows[(int)(tokenType) >> 24];
		}

		private HeapIndexToken GetMaxTokenValue(HeapIndexToken tokenType)
		{
			return (HeapIndexToken)(tableRows[(int)(tokenType) >> 24]) | (tokenType & HeapIndexToken.TableMask);
		}

		/// <summary>
		/// Get the last token for the table
		/// </summary>
		/// <param name="table">The table to lookup the token for</param>
		/// <returns>The last token inside the table</returns>
		private Token GetMaxTokenValue(TableType table)
		{
			return new Token(table, tableRows[(int)(table) >> 24]);
		}

		/// <summary>
		/// Loads the string from the heap table for stringIdx
		/// </summary>
		/// <param name="stringIdx">The string's index</param>
		/// <returns>The string at heap[stringIdx]</returns>
		private string GetString(HeapIndexToken stringIdx)
		{
			string value;

			if (strings.TryGetValue(stringIdx, out value))
				return value;

			value = metadataProvider.ReadString(stringIdx);
			strings.Add(stringIdx, value);

			return value;
		}

		private T RetrieveSignature<T>(HeapIndexToken blobIdx) where T : Signature
		{
			Signature signature;

			if (signatures.TryGetValue(blobIdx, out signature))
				return signature as T;
			else
				return (T)null;
		}

		private T StoreSignature<T>(HeapIndexToken blobIdx, T signature) where T : Signature
		{
			signatures.Add(blobIdx, signature);
			return signature;
		}

		private MethodSignature GetMethodSignature(HeapIndexToken blobIdx)
		{
			return (RetrieveSignature<MethodSignature>(blobIdx) ?? StoreSignature<MethodSignature>(blobIdx, new MethodSignature(metadataProvider, blobIdx)));
		}

		private FieldSignature GetFieldSignature(HeapIndexToken blobIdx)
		{
			return (RetrieveSignature<FieldSignature>(blobIdx) ?? StoreSignature<FieldSignature>(blobIdx, new FieldSignature(metadataProvider, blobIdx)));
		}

		private TypeSpecSignature GetTypeSpecSignature(HeapIndexToken blobIdx)
		{
			return (RetrieveSignature<TypeSpecSignature>(blobIdx) ?? StoreSignature<TypeSpecSignature>(blobIdx, new TypeSpecSignature(metadataProvider, blobIdx)));
		}

		private Signature GetMemberRefSignature(HeapIndexToken blobIdx)
		{
			return (RetrieveSignature<Signature>(blobIdx) ?? StoreSignature<Signature>(blobIdx, Signature.GetSignatureFromMemberRef(metadataProvider, blobIdx)));
		}

		private struct TypeInfo
		{
			public TypeDefRow TypeDefRow;
			public Token NestedClass;
			public Token EnclosingClass;
			public Token MaxMethod;
			public Token MaxField;
			public int Size;
			public short PackingSize;
		}

		#endregion Internals

		/// <summary>
		/// Loads the interfaces.
		/// </summary>
		private void LoadTypeReferences()
		{
			var maxToken = GetMaxTokenValue(TableType.TypeRef);
			foreach (var token in new Token(TableType.TypeRef, 1).Upto(maxToken))
			{
				MosaType type = null;

				var row = metadataProvider.ReadTypeRefRow(token);
				var typeName = GetString(row.TypeName);

				switch (row.ResolutionScope.Table)
				{
					case TableType.Module: goto case TableType.TypeRef;
					case TableType.TypeDef: throw new AssemblyLoadException();
					case TableType.TypeRef:
						{
							var row2 = metadataProvider.ReadTypeRefRow(row.ResolutionScope);
							var typeName2 = GetString(row2.TypeName);
							var @namespace = GetString(row2.TypeNamespace) + "." + typeName2;
							var asmRefRow = metadataProvider.ReadAssemblyRefRow(row2.ResolutionScope);
							var sourceAssemblyName = GetString(asmRefRow.Name);
							var sourceAssembly = resolver.GetAssemblyByName(sourceAssemblyName);
							type = resolver.GetTypeByName(sourceAssembly, @namespace, typeName);
							break;
						}
					case TableType.TypeSpec: throw new AssemblyLoadException();
					case TableType.ModuleRef: throw new AssemblyLoadException();
					case TableType.AssemblyRef:
						{
							var @namespace = GetString(row.TypeNamespace);
							var asmRefRow = metadataProvider.ReadAssemblyRefRow(row.ResolutionScope);
							var sourceAssemblyName = GetString(asmRefRow.Name);
							var sourceAssembly = resolver.GetAssemblyByName(sourceAssemblyName);
							type = resolver.GetTypeByName(sourceAssembly, @namespace, typeName);
							break;
						}
					default: throw new AssemblyLoadException();
				}

				if (type == null)
				{
					throw new AssemblyLoadException("Could not find genericBaseType: " + typeName);
				}

				resolver.AddType(assembly, token, type);
			}
		}

		/// <summary>
		/// Loads all types from the given metadata module.
		/// </summary>
		private void LoadTypes()
		{
			var typeInfos = new List<TypeInfo>(GetTableRows(TableType.TypeDef));

			var maxTypeDef = GetMaxTokenValue(TableType.TypeDef);
			var maxMethod = GetMaxTokenValue(TableType.MethodDef);
			var maxField = GetMaxTokenValue(TableType.Field);
			var maxLayout = GetMaxTokenValue(TableType.ClassLayout);
			var maxNestedClass = GetMaxTokenValue(TableType.NestedClass);

			var tokenLayout = new Token(TableType.ClassLayout, 1);
			ClassLayoutRow layoutRow = (maxLayout.RID != 0) ? metadataProvider.ReadClassLayoutRow(tokenLayout) : null;

			var tokenNested = new Token(TableType.NestedClass, 1);
			NestedClassRow nestedRow = (maxNestedClass.RID != 0) ? metadataProvider.ReadNestedClassRow(tokenNested) : null;

			TypeDefRow nextTypeDefRow = null;
			var typeDefRow = metadataProvider.ReadTypeDefRow(new Token(TableType.TypeDef, 1));

			foreach (var token in new Token(TableType.TypeDef, 1).Upto(maxTypeDef))
			{
				var info = new TypeInfo();

				info.TypeDefRow = typeDefRow;
				info.NestedClass = (nestedRow != null && nestedRow.NestedClass == token) ? nestedRow.NestedClass : Token.Zero;
				info.EnclosingClass = (nestedRow != null && nestedRow.NestedClass == token) ? nestedRow.EnclosingClass : Token.Zero;
				info.Size = (layoutRow != null && layoutRow.Parent == token) ? layoutRow.ClassSize : 0;
				info.PackingSize = (layoutRow != null && layoutRow.Parent == token) ? layoutRow.PackingSize : (short)0;

				if (token.RID < maxTypeDef.RID)
				{
					nextTypeDefRow = metadataProvider.ReadTypeDefRow(token.NextRow);

					info.MaxField = nextTypeDefRow.FieldList;
					info.MaxMethod = nextTypeDefRow.MethodList;

					if (info.MaxMethod.RID > maxMethod.RID)
						info.MaxMethod = maxMethod.NextRow;

					if (info.MaxField.RID > maxField.RID)
						info.MaxField = maxField.NextRow;
				}
				else
				{
					info.MaxMethod = maxMethod.NextRow;
					info.MaxField = maxField.NextRow;
				}

				typeInfos.Add(info);

				if (layoutRow != null && layoutRow.Parent == token)
				{
					tokenLayout = tokenLayout.NextRow;
					if (tokenLayout.RID <= maxLayout.RID)
						layoutRow = metadataProvider.ReadClassLayoutRow(tokenLayout);
				}

				if (nestedRow != null && nestedRow.NestedClass == token)
				{
					tokenNested = tokenNested.NextRow;
					if (tokenNested.RID <= maxNestedClass.RID)
						nestedRow = metadataProvider.ReadNestedClassRow(tokenNested);
				}

				typeDefRow = nextTypeDefRow;
			}

			foreach (var token in new Token(TableType.TypeDef, 1).Upto(maxTypeDef))
			{
				LoadType(token, typeInfos);
			}

			foreach (var token in new Token(TableType.TypeDef, 1).Upto(maxTypeDef))
			{
				LoadTypeMethodsAndParameters(token, typeInfos);
			}
		}

		private void LoadType(Token token, IList<TypeInfo> typeInfos)
		{
			if (resolver.CheckTypeExists(assembly, token))
				return;

			var info = typeInfos[(int)token.RID - 1];

			if (info.TypeDefRow.Extends.RID != 0)
			{
				switch (info.TypeDefRow.Extends.Table)
				{
					case TableType.TypeDef: LoadType(info.TypeDefRow.Extends, typeInfos); break;
					case TableType.TypeSpec: LoadTypeSpec(info.TypeDefRow.Extends); break;
					case TableType.TypeRef: break;
					default: throw new AssemblyLoadException("unexpected token genericBaseType.");
				}
			}

			var enclosingType = (info.NestedClass == token) ? resolver.GetTypeByToken(assembly, info.EnclosingClass) : null;

			MosaType type = new MosaType(assembly);
			type.Name = GetString(info.TypeDefRow.TypeName);
			type.BaseType = (info.TypeDefRow.Extends.RID == 0) ? null : resolver.GetTypeByToken(assembly, info.TypeDefRow.Extends);
			type.EnclosingType = enclosingType;
			type.IsNested =
				(info.TypeDefRow.Flags & TypeAttributes.NestedPublic) == TypeAttributes.NestedPublic ||
				(info.TypeDefRow.Flags & TypeAttributes.NestedPrivate) == TypeAttributes.NestedPrivate ||
				(info.TypeDefRow.Flags & TypeAttributes.NestedFamily) == TypeAttributes.NestedFamily;
			type.Namespace = type.IsNested ? enclosingType.FullName : GetString(info.TypeDefRow.TypeNamespace);
			type.FullName = type.Namespace + "." + type.Name;
			type.IsInterface = (info.TypeDefRow.Flags & TypeAttributes.Interface) == TypeAttributes.Interface;
			type.IsExplicitLayout = (info.TypeDefRow.Flags & TypeAttributes.LayoutMask) == TypeAttributes.ExplicitLayout;
			type.PackingSize = info.PackingSize;
			type.Size = info.Size;

			type.SetFlags();

			resolver.AddType(assembly, token, type);
		}

		private void LoadTypeMethodsAndParameters(Token token, IList<TypeInfo> typeInfos)
		{
			var type = resolver.GetTypeByToken(assembly, token);

			var info = typeInfos[(int)token.RID - 1];

			LoadMethods(type, info.TypeDefRow.MethodList, info.MaxMethod);
			LoadFields(type, info.TypeDefRow.FieldList, info.MaxField);
		}

		private void LoadMethods(MosaType declaringType, Token first, Token last)
		{
			if (first.RID >= last.RID)
				return;

			var maxMethod = GetMaxTokenValue(TableType.MethodDef);
			var methodDef = metadataProvider.ReadMethodDefRow(first);
			MethodDefRow nextMethodDef = null;

			foreach (var token in first.Upto(last.PreviousRow))
			{
				Token maxParam;

				if (token.RID < maxMethod.RID)
				{
					nextMethodDef = metadataProvider.ReadMethodDefRow(token.NextRow);
					maxParam = nextMethodDef.ParamList;
				}
				else
				{
					maxParam = GetMaxTokenValue(TableType.Param).NextRow;
				}

				var signature = GetMethodSignature(methodDef.SignatureBlob);

				var method = new MosaMethod();
				method.Name = GetString(methodDef.NameString);
				method.DeclaringType = declaringType;
				//method.FullName = declaringType + "." + method.Name;
				method.ReturnType = GetMosaType(signature.ReturnType);
				method.HasThis = signature.HasThis;
				method.HasExplicitThis = signature.HasExplicitThis;
				method.IsInternal = (methodDef.ImplFlags & MethodImplAttributes.InternalCall) == MethodImplAttributes.InternalCall;
				method.IsNoInlining = (methodDef.ImplFlags & MethodImplAttributes.NoInlining) == MethodImplAttributes.NoInlining;
				method.IsAbstract = (methodDef.Flags & MethodAttributes.Abstract) == MethodAttributes.Abstract;
				method.IsStatic = (methodDef.Flags & MethodAttributes.Static) == MethodAttributes.Static;
				method.IsSpecialName = (methodDef.Flags & MethodAttributes.SpecialName) == MethodAttributes.SpecialName;
				method.IsRTSpecialName = (methodDef.Flags & MethodAttributes.RTSpecialName) == MethodAttributes.RTSpecialName;
				method.IsVirtual = ((methodDef.Flags & MethodAttributes.Virtual) == MethodAttributes.Virtual);
				method.IsPInvokeImpl = ((methodDef.Flags & MethodAttributes.PInvokeImpl) != MethodAttributes.PInvokeImpl);
				method.IsNewSlot = ((methodDef.Flags & MethodAttributes.NewSlot) != MethodAttributes.NewSlot);
				method.IsFinal = ((methodDef.Flags & MethodAttributes.Final) != MethodAttributes.Final);
				method.Rva = methodDef.Rva;

				if (methodDef.Rva != 0)
				{
					var code = metadataModule.GetInstructionStream((long)methodDef.Rva);
					var codeReader = new EndianAwareBinaryReader(code, Endianness.Little);
					var header = new MethodHeader(codeReader);

					if (header.LocalVarSigTok.RID != 0)
					{
						var standAlongSigRow = metadataProvider.ReadStandAloneSigRow(header.LocalVarSigTok);
						var localVariables = new LocalVariableSignature(metadataProvider, standAlongSigRow.SignatureBlob);

						foreach (var local in localVariables.Locals)
						{
							var type = GetMosaType(local.Type);

							method.LocalVariables.Add(type);
						}
					}

					method.Code = codeReader.ReadBytes(header.CodeSize);

					foreach (var handler in header.Clauses)
					{
						ExceptionBlock block = new ExceptionBlock();

						block.TryOffset = handler.TryOffset;
						block.TryLength = handler.TryLength;
						block.HandlerOffset = handler.HandlerOffset;
						block.HandlerLength = handler.HandlerLength;
						block.FilterOffset = handler.FilterOffset;

						block.Type = (handler.ClassToken.Value != 0) ? resolver.GetTypeByToken(assembly, handler.ClassToken) : null;

						switch (handler.ExceptionHandlerType)
						{
							case ExceptionHandlerType.Exception: block.ExceptionHandler = ExceptionBlockType.Exception; break;
							case ExceptionHandlerType.Fault: block.ExceptionHandler = ExceptionBlockType.Fault; break;
							case ExceptionHandlerType.Filter: block.ExceptionHandler = ExceptionBlockType.Filter; break;
							case ExceptionHandlerType.Finally: block.ExceptionHandler = ExceptionBlockType.Finally; break;
						}

						method.ExceptionBlocks.Add(block);
					}
				}

				LoadParameters(method, methodDef.ParamList, maxParam, signature);

				method.SetName();

				declaringType.Methods.Add(method);
				resolver.AddMethod(assembly, token, method);

				methodDef = nextMethodDef;
			}
		}

		private void LoadFields(MosaType declaringType, Token first, Token last)
		{
			var maxRVA = GetMaxTokenValue(TableType.FieldRVA);
			var maxLayout = GetMaxTokenValue(TableType.FieldLayout);
			var tokenRva = new Token(TableType.FieldRVA, 1);
			var tokenLayout = new Token(TableType.FieldLayout, 1);

			FieldRVARow fieldRVA = (maxRVA.RID != 0) ? metadataProvider.ReadFieldRVARow(tokenRva) : null;
			FieldLayoutRow fieldLayout = (maxLayout.RID != 0) ? metadataProvider.ReadFieldLayoutRow(tokenLayout) : null;

			foreach (var token in first.Upto(last.PreviousRow))
			{
				var fieldRow = metadataProvider.ReadFieldRow(token);
				uint rva = 0;
				uint layout = 0;

				// Static fields have an optional RVA, non-static may have a layout assigned
				if ((fieldRow.Flags & FieldAttributes.HasFieldRVA) == FieldAttributes.HasFieldRVA)
				{
					// Move to the RVA of this field
					while (fieldRVA.Field.RID < token.RID && tokenRva.RID <= maxRVA.RID)
					{
						fieldRVA = metadataProvider.ReadFieldRVARow(tokenRva);
						tokenRva = tokenRva.NextRow;
					}

					// Does this field have an RVA?
					if (token == fieldRVA.Field && tokenRva.RID <= maxRVA.RID)
					{
						rva = fieldRVA.Rva;
						tokenRva = tokenRva.NextRow;
						if (tokenRva.RID < maxRVA.RID)
						{
							fieldRVA = metadataProvider.ReadFieldRVARow(tokenRva);
						}
					}
				}

				if ((fieldRow.Flags & FieldAttributes.HasDefault) == FieldAttributes.HasDefault)
				{
					// FIXME: Has a default value.
					//Debug.Assert(false);
				}

				// Layout only exists for non-static fields
				if ((fieldRow.Flags & FieldAttributes.Static) != FieldAttributes.Static)
				{
					// Move to the layout of this field
					while ((fieldLayout == null || fieldLayout.Field.RID < token.RID) && tokenLayout.RID <= maxLayout.RID)
					{
						fieldLayout = metadataProvider.ReadFieldLayoutRow(tokenLayout);
						tokenLayout = tokenLayout.NextRow;
					}

					// Does this field have layout?
					if (fieldLayout != null && fieldLayout.Field == token && tokenLayout.RID <= maxLayout.RID)
					{
						layout = fieldLayout.Offset;
						tokenLayout = tokenLayout.NextRow;
						if (tokenLayout.RID < maxLayout.RID)
						{
							fieldLayout = metadataProvider.ReadFieldLayoutRow(tokenLayout);
						}
					}
				}

				var field = new MosaField();
				field.Name = GetString(fieldRow.Name);
				field.FullName = declaringType.FullName + "." + field.Name;
				field.DeclaringType = declaringType;
				field.IsLiteralField = (fieldRow.Flags & FieldAttributes.Literal) == FieldAttributes.Literal;
				field.IsStaticField = (fieldRow.Flags & FieldAttributes.Static) == FieldAttributes.Static;
				field.RVA = (int)rva;
				field.FieldType = GetMosaType(GetFieldSignature(fieldRow.Signature).Type);

				declaringType.Fields.Add(field);
				resolver.AddField(assembly, token, field);
			}
		}

		private MosaType GetMosaType(SigType sigType)
		{
			switch (sigType.Type)
			{
				case CilElementType.Class: return resolver.GetTypeByToken(assembly, (sigType as TypeSigType).Token);
				case CilElementType.ValueType: goto case CilElementType.Class;
				case CilElementType.GenericInst: return resolver.GetTypeByToken(assembly, (sigType as GenericInstSigType).BaseType.Token);
				case CilElementType.Var: return resolver.GetVarType((sigType as VarSigType).Index);
				case CilElementType.MVar: return resolver.GetMVarType((sigType as MVarSigType).Index);

				case CilElementType.Ptr: return resolver.GetUnmanagedPointerType(GetMosaType((sigType as PtrSigType).ElementType));
				case CilElementType.ByRef: return resolver.GetManagedPointerType(GetMosaType((sigType as RefSigType).ElementType));
				case CilElementType.Array: return resolver.GetArrayType(GetMosaType((sigType as ArraySigType).ElementType));

				case CilElementType.SZArray: return resolver.GetArrayType(GetMosaType((sigType as SZArraySigType).ElementType));
				default: break;
			}

			var builtInSigType = sigType as BuiltInSigType;
			if (builtInSigType != null)
			{
				if (internalAssembly == null)
					internalAssembly = resolver.GetAssemblyByName("@Internal");

				return resolver.GetTypeByToken(internalAssembly, new Token((uint)builtInSigType.Type));
			}

			throw new NotImplementedException(String.Format("SigType of CilElementType.{0} is not supported.", sigType.Type));
		}

		/// <summary>
		/// Loads the generic param constraints.
		/// </summary>
		private void LoadGenericParams()
		{
			var maxToken = GetMaxTokenValue(TableType.GenericParam);
			foreach (var token in new Token(TableType.GenericParam, 1).Upto(maxToken))
			{
				var row = metadataProvider.ReadGenericParamRow(token);
				var name = GetString(row.NameString);

				var genericParameter = new MosaGenericParameter();
				genericParameter.Name = name;
				genericParameter.Index = row.Number;
				genericParameter.IsCovariant = (row.Flags & GenericParameterAttributes.Covariant) == GenericParameterAttributes.Covariant;
				genericParameter.IsContravariant = (row.Flags & GenericParameterAttributes.Contravariant) == GenericParameterAttributes.Contravariant;
				genericParameter.IsNonVariant = (row.Flags & GenericParameterAttributes.NonVariant) == GenericParameterAttributes.NonVariant;

				mapGenericParam.Add(token, genericParameter);

				// The following switch matches the AttributeTargets enumeration against
				// metadata tables, which make valid targets for an attribute.
				switch (row.Owner.Table)
				{
					case TableType.TypeDef:
						var type = resolver.GetTypeByToken(assembly, row.Owner);
						type.GenericParameters.Add(genericParameter);
						break;

					case TableType.MethodDef:
						var method = resolver.GetMethodByToken(assembly, row.Owner);
						method.GenericParameters.Add(genericParameter);
						break;

					default: throw new NotImplementedException();
				}
			}
		}

		private void LoadGenericParamContraints()
		{
			var maxToken = GetMaxTokenValue(TableType.GenericParamConstraint);
			foreach (var token in new Token(TableType.GenericParamConstraint, 1).Upto(maxToken))
			{
				var row = metadataProvider.ReadGenericParamConstraintRow(token);

				var genericParam = mapGenericParam[row.Owner];

				var type = resolver.GetTypeByToken(assembly, row.Constraint);

				genericParam.Constraints.Add(type);
			}
		}

		private void LoadParameters(MosaMethod method, Token first, Token max, MethodSignature signature)
		{
			int index = 0;

			foreach (var token in first.Upto(max.PreviousRow))
			{
				var paramDef = metadataProvider.ReadParamRow(token);
				var parameter = new MosaParameter();

				parameter.Name = GetString(paramDef.Name);
				parameter.IsIn = (paramDef.Flags & ParameterAttributes.In) == ParameterAttributes.In;
				parameter.IsOut = (paramDef.Flags & ParameterAttributes.Out) == ParameterAttributes.Out;
				parameter.Position = paramDef.Sequence;
				parameter.Type = GetMosaType(signature.Parameters[index++]);

				method.Parameters.Add(parameter);
			}
		}

		private void LoadTypeSpecs()
		{
			var maxToken = GetMaxTokenValue(TableType.TypeSpec);
			foreach (var token in new Token(TableType.TypeSpec, 1).Upto(maxToken))
			{
				LoadTypeSpec(token);
			}
		}

		private void LoadTypeSpec(Token token)
		{
			if (resolver.CheckTypeExists(assembly, token))
				return;

			var row = metadataProvider.ReadTypeSpecRow(token);
			var signature = GetTypeSpecSignature(row.SignatureBlob);
			var genericSig = (signature.Type as GenericInstSigType);

			if (genericSig == null)
				return;

			var genericBaseType = GetMosaType(signature.Type);

			List<MosaType> genericParamTypes = new List<MosaType>();

			foreach (var genericParam in genericSig.GenericArguments)
			{
				var genericParamType = GetMosaType(genericParam);
				genericParamTypes.Add(genericParamType);
			}

			var genericType = resolver.ResolveGenericType(genericBaseType, genericParamTypes);

			resolver.AddType(assembly, token, genericType);
			genericTypes.Add(genericType);

			return;
		}

		/// <summary>
		/// Loads the generic interfaces.
		/// </summary>
		private void AssignGenericInterfaces()
		{
			foreach (var genericType in genericTypes)
			{
				foreach (var type in genericType.GenericBaseType.Interfaces)
				{
					if (type.GenericParameterTypes.Count == 0)
					{
						genericType.Interfaces.Add(type);
						continue;
					}

					// -- only needs to search generic type interfaces
					foreach (var interfaceType in resolver.Types)
					{
						if (!interfaceType.IsInterface)
							continue;

						if (interfaceType.GenericBaseType == null)
							continue;

						if (genericType.GenericBaseType == interfaceType.GenericBaseType)
						{
							//if (SigType.Equals(GenericArguments, interfaceType.GenericArguments))
							//{
							//	genericType.Interfaces.Add(interfaceType);
							//	break;
							//}
							continue;
						}
					}
				}
			}
		}

		/// <summary>
		/// Loads the interfaces.
		/// </summary>
		private void LoadMemberReferences()
		{
			var maxToken = GetMaxTokenValue(TableType.MemberRef);
			foreach (var token in new Token(TableType.MemberRef, 1).Upto(maxToken))
			{
				var row = metadataProvider.ReadMemberRefRow(token);
				var name = GetString(row.NameString);

				MosaType ownerType = null;

				if (row.Class.Table == TableType.TypeDef || row.Class.Table == TableType.TypeRef || row.Class.Table == TableType.TypeSpec)
				{
					ownerType = resolver.GetTypeByToken(assembly, row.Class);
				}
				else
				{
					throw new TypeLoadException(String.Format("Failed to retrieve owner genericBaseType for Token {0:x} (Table {1})", row.Class, row.Class.Table));
				}

				var signature = GetMemberRefSignature(row.SignatureBlob);

				if (signature is FieldSignature)
				{
					foreach (var field in ownerType.Fields)
					{
						if (field.Name == name)
						{
							resolver.AddField(assembly, token, field);
							break;
						}
					}
				}
				else
				{
					MethodSignature methodSignature = signature as MethodSignature;
					Debug.Assert(signature is MethodSignature);

					List<MosaType> typeSignature = new List<MosaType>(methodSignature.Parameters.Length);

					for (int index = 0; index < methodSignature.Parameters.Length; index++)
					{
						var parameter = methodSignature.Parameters[index];

						var parameterType = GetMosaType(parameter);

						if (parameterType.IsMVarFlag || parameterType.IsVarFlag)
						{
							parameterType = ownerType.GenericParameterTypes[parameterType.VarOrMVarIndex];
						}

						typeSignature.Add(parameterType);
					}

					MosaMethod targetMethod = null;

					foreach (var method in ownerType.Methods)
					{
						if (method.Name == name)
						{
							if (method.Matches(typeSignature))
							{
								targetMethod = method;
								break;
							}
						}
					}

					// Special case: string.get_Chars is same as string.get_Item
					if (name == "get_Chars" && ownerType.FullName == "System.String")
					{
						name = "get_Item";

						foreach (var method in ownerType.Methods)
						{
							if (method.Matches(typeSignature))
							{
								targetMethod = method;
								break;
							}
						}
					}

					if (targetMethod == null)
					{
						throw new TypeLoadException(String.Format("Failed to retrieve method for Token {0:x} (Table {1})", row.Class, row.Class.Table));
					}

					resolver.AddMethod(assembly, token, targetMethod);
				}
			}
		}

		/// <summary>
		/// Loads the interfaces.
		/// </summary>
		private void LoadInterfaces()
		{
			var maxToken = GetMaxTokenValue(TableType.InterfaceImpl);

			foreach (var token in new Token(TableType.InterfaceImpl, 1).Upto(maxToken))
			{
				var row = metadataProvider.ReadInterfaceImplRow(token);

				var declaringType = resolver.GetTypeByToken(assembly, row.Class);
				var interfaceType = resolver.GetTypeByToken(assembly, row.Interface);

				declaringType.Interfaces.Add(interfaceType);
			}
		}

		/// <summary>
		/// Loads all custom attributes from the assembly.
		/// </summary>
		private void LoadCustomAttributes()
		{
			var maxToken = GetMaxTokenValue(TableType.CustomAttribute);
			foreach (var token in new Token(TableType.CustomAttribute, 1).Upto(maxToken))
			{
				var row = metadataProvider.ReadCustomAttributeRow(token);

				MosaAttribute attribute = new MosaAttribute();
				attribute.CtorMethod = resolver.GetMethodByToken(assembly, row.Type);
				attribute.Blob = metadataProvider.ReadBlob(row.Value);

				// The following switch matches the AttributeTargets enumeration against
				// metadata tables, which make valid targets for an attribute.
				switch (row.Parent.Table)
				{
					case TableType.Assembly:
						// AttributeTargets.Assembly
						break;

					case TableType.AssemblyRef:
						break;

					case TableType.TypeDef:
						// AttributeTargets.Class
						// AttributeTargets.Delegate
						// AttributeTargets.Enum
						// AttributeTargets.Interface
						// AttributeTargets.Struct
						resolver.GetTypeByToken(assembly, row.Parent).CustomAttributes.Add(attribute);
						break;

					case TableType.MethodDef:
						// AttributeTargets.Constructor
						// AttributeTargets.Method
						resolver.GetMethodByToken(assembly, row.Parent).CustomAttributes.Add(attribute);
						break;

					case TableType.Event:
						// AttributeTargets.Event
						break;

					case TableType.Field:
						// AttributeTargets.Field
						resolver.GetFieldByToken(assembly, row.Parent).CustomAttributes.Add(attribute);
						break;

					case TableType.GenericParam:
						// AttributeTargets.GenericParameter
						break;

					case TableType.Module:
						// AttributeTargets.Module
						break;

					case TableType.Param:
						// AttributeTargets.Parameter
						// AttributeTargets.ReturnValue
						break;

					case TableType.Property:
						// AttributeTargets.StackFrameIndex
						break;

					default: throw new AssemblyLoadException();
				}
			}
		}
	}
}