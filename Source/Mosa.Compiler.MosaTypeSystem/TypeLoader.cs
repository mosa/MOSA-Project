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
	public class TypeLoader
	{
		#region Data members

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
		/// The externals
		/// </summary>
		private Dictionary<Token, string> externals;

		/// <summary>
		/// The strings
		/// </summary>
		private Dictionary<HeapIndexToken, string> strings;

		/// <summary>
		/// The signatures
		/// </summary>
		private Dictionary<HeapIndexToken, Signature> signatures;

		/// <summary>
		/// The table rows
		/// </summary>
		private int[] tableRows = new int[TableCount];

		/// <summary>
		/// The table count
		/// </summary>
		private const int TableCount = ((int)TableType.GenericParamConstraint >> 24) + 1;

		/// <summary>
		/// The assembly
		/// </summary>
		private MosaAssembly assembly;

		#endregion Data members

		public TypeLoader(MosaTypeResolver resolver)
		{
			this.resolver = resolver;
		}

		public void Load(IMetadataModule metadataModule)
		{
			this.metadataModule = metadataModule;
			this.metadataProvider = metadataModule.Metadata;

			RetrieveAllTableSizes();

			this.externals = new Dictionary<Token, string>();
			this.signatures = new Dictionary<HeapIndexToken, Signature>();
			this.strings = new Dictionary<HeapIndexToken, string>();

			assembly = new MosaAssembly(metadataModule.Name);
			//assembly.Name = metadataModule.Name;

			resolver.AddAssembly(assembly);

			// Load all types from the assembly into the type array
			LoadTypeReferences();
			LoadTypes();
			LoadTypeSpecs();
			LoadMemberReferences();
			//LoadCustomAttributes();
			LoadGenericParams();

			//LoadInterfaces();
			//LoadGenericInterfaces();

			//LoadExternals();

			// release
			this.metadataModule = null;
			this.metadataProvider = null;
			this.externals = null;
			this.signatures = null;
			this.strings = null;
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
					throw new AssemblyLoadException("Could not find type: " + typeName);
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
				info.NestedClass = (nestedRow.NestedClass == token) ? nestedRow.NestedClass : Token.Zero;
				info.EnclosingClass = (nestedRow.NestedClass == token) ? nestedRow.EnclosingClass : Token.Zero;
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

				if (layoutRow.Parent == token)
				{
					tokenLayout = tokenLayout.NextRow;
					if (tokenLayout.RID <= maxLayout.RID)
						layoutRow = metadataProvider.ReadClassLayoutRow(tokenLayout);
				}

				if (nestedRow.NestedClass == token)
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
					default: throw new AssemblyLoadException("unexpected token type.");
				}
			}

			var enclosingType = (info.NestedClass == token) ? resolver.GetTypeByToken(assembly, info.EnclosingClass) : null;

			MosaType type = new MosaType();
			type.Name = GetString(info.TypeDefRow.TypeName);
			type.BaseType = (info.TypeDefRow.Extends.RID == 0) ? null : resolver.GetTypeByToken(assembly, info.TypeDefRow.Extends);
			type.EnclosingType = enclosingType;
			type.IsNested =
				(info.TypeDefRow.Flags & TypeAttributes.NestedPublic) == TypeAttributes.NestedPublic ||
				(info.TypeDefRow.Flags & TypeAttributes.NestedPrivate) == TypeAttributes.NestedPrivate ||
				(info.TypeDefRow.Flags & TypeAttributes.NestedFamily) == TypeAttributes.NestedFamily;
			type.Namespace = type.IsNested ? enclosingType.FullName : GetString(info.TypeDefRow.TypeNamespace);
			type.FullName = type.Namespace + "." + type.Name;
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

				MosaMethod method = new MosaMethod();
				method.Name = GetString(methodDef.NameString);
				method.FullName = declaringType + "." + method.Name;
				method.DeclaringType = declaringType;
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

				LoadParameters(method, methodDef.ParamList, maxParam, signature);

				method.SetMethodName();

				methodDef = nextMethodDef;

				declaringType.Methods.Add(method);
				resolver.AddMethod(assembly, token, method);
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
					while (fieldLayout.Field.RID < token.RID && tokenLayout.RID <= maxLayout.RID)
					{
						fieldLayout = metadataProvider.ReadFieldLayoutRow(tokenLayout);
						tokenLayout = tokenLayout.NextRow;
					}

					// Does this field have layout?
					if (token == fieldLayout.Field && tokenLayout.RID <= maxLayout.RID)
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

		private MosaAssembly internalAssembly = null;

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
				case CilElementType.ByRef: return resolver.GetManagedPointerType(GetMosaType((sigType as PtrSigType).ElementType));
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

				var genericParameter = new MosaGenericParameter(name);

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

			MosaType genericType = new MosaType();

			// TODO: typeSystem.ResolveGenericType(this, signature, token);

			var type = GetMosaType(signature.Type);

			//resolver.AddType(assembly, token, genericType);

			// FIXME:
			resolver.AddType(assembly, token, type);

			return;
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
					throw new TypeLoadException(String.Format("Failed to retrieve owner type for Token {0:x} (Table {1})", row.Class, row.Class.Table));
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

						if (parameterType.IsMVarFlag && ownerType.IsGeneric)
						{
							//parameter = ownerType.GenericParameters[parameterType.VarOrMVarIndex];
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
	}
}