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
using System.Text;
using System.Diagnostics;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem.Cil;
using Mosa.Runtime.TypeSystem.Generic;

namespace Mosa.Runtime.TypeSystem
{
	public class TypeModule : ITypeModule
	{
		#region Data members

		/// <summary>
		/// Holds the type system
		/// </summary>
		private readonly ITypeSystem typeSystem;

		/// <summary>
		/// Holds the metadata provider
		/// </summary>
		private readonly IMetadataProvider metadataProvider;

		/// <summary>
		/// Holds the metadata module
		/// </summary>
		private readonly IMetadataModule metadataModule;

		/// <summary>
		/// Array of loaded runtime type descriptors.
		/// </summary>
		private RuntimeType[] types;

		/// <summary>
		/// Holds all loaded method definitions.
		/// </summary>
		private RuntimeMethod[] methods;

		/// <summary>
		/// Array of loaded runtime typeSpec descriptors.
		/// </summary>
		private RuntimeType[] typeSpecs;

		/// <summary>
		/// Holds all loaded method definitions.
		/// </summary>
		private RuntimeMethod[] methodSpecs;

		/// <summary>
		/// Holds all loaded _stackFrameIndex definitions.
		/// </summary>
		private RuntimeField[] fields;

		/// <summary>
		/// 
		/// </summary>
		private RuntimeMember[] memberRef;

		/// <summary>
		/// 
		/// </summary>
		private RuntimeType[] typeRef;

		/// <summary>
		/// 
		/// </summary>
		private RuntimeType[] genericParamConstraint;

		/// <summary>
		/// 
		/// </summary>
		private Dictionary<MetadataToken, string> externals;

		/// <summary>
		/// 
		/// </summary>
		private Dictionary<TokenTypes, string> strings;

		/// <summary>
		/// 
		/// </summary>
		private Dictionary<TokenTypes, Signature> signatures;

		/// <summary>
		/// 
		/// </summary>
		private int[] tableRows = new int[TableCount];

		private const int TableCount = ((int)MetadataTable.GenericParamConstraint >> 24) + 1;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes static data members of the type loader.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="metadataModule">The metadata module.</param>
		public TypeModule(ITypeSystem typeSystem, IMetadataModule metadataModule)
		{
			Debug.Assert(metadataModule != null);
			Debug.Assert(typeSystem != null);

			this.typeSystem = typeSystem;
			this.metadataModule = metadataModule;
			this.metadataProvider = metadataModule.Metadata;

			RetrieveAllTableSizes();

			this.methods = new RuntimeMethod[GetTableRows(MetadataTable.MethodDef)];
			this.fields = new RuntimeField[GetTableRows(MetadataTable.Field)];
			this.types = new RuntimeType[GetTableRows(MetadataTable.TypeDef)];

			this.typeSpecs = new RuntimeType[GetTableRows(MetadataTable.TypeSpec)];
			this.methodSpecs = new RuntimeMethod[GetTableRows(MetadataTable.MethodSpec)];

			this.memberRef = new RuntimeMember[GetTableRows(MetadataTable.MemberRef)];
			this.typeRef = new RuntimeType[GetTableRows(MetadataTable.TypeRef)];
			this.genericParamConstraint = new RuntimeType[GetTableRows(MetadataTable.GenericParamConstraint)];

			this.externals = new Dictionary<MetadataToken, string>();

			this.signatures = new Dictionary<TokenTypes, Signature>();
			this.strings = new Dictionary<TokenTypes, string>();

			// Load all types from the assembly into the type array
			LoadTypeReferences();
			LoadTypes();
			LoadTypeSpecs();
			LoadMemberReferences();
			LoadCustomAttributes();
			LoadGenericParams();

			LoadInterfaces();
			LoadGenericInterfaces();

			LoadExternals();
		}

		#endregion // Construction

		#region Internals

		private void RetrieveAllTableSizes()
		{
			for (int i = 0; i < TableCount; i++)
			{
				tableRows[i] = metadataProvider.GetRowCount((MetadataTable)(i << 24));
			}
		}

		/// <summary>
		/// Gets the table rows.
		/// </summary>
		/// <param name="tokenType">Type of the token.</param>
		/// <returns></returns>
		private int GetTableRows(MetadataTable tokenType)
		{
			return tableRows[(int)(tokenType) >> 24];
		}

		private TokenTypes GetMaxTokenValue(TokenTypes tokenType)
		{
			return (TokenTypes)(tableRows[(int)(tokenType) >> 24]) | (tokenType & TokenTypes.TableMask);
		}

		private MetadataToken GetMaxTokenValue(MetadataTable table)
		{
			return new MetadataToken(table, tableRows[(int)(table) >> 24]);
		}

		private string GetString(TokenTypes stringIdx)
		{
			string value;

			if (strings.TryGetValue(stringIdx, out value))
				return value;

			value = metadataProvider.ReadString(stringIdx);
			strings.Add(stringIdx, value);

			return value;
		}

		private T RetrieveSignature<T>(TokenTypes blobIdx) where T : Signature
		{
			Signature signature;

			if (signatures.TryGetValue(blobIdx, out signature))
				return signature as T;
			else
				return (T)null;
		}

		private T StoreSignature<T>(TokenTypes blobIdx, T signature) where T : Signature
		{
			signatures.Add(blobIdx, signature);
			return signature;
		}

		private MethodSignature GetMethodSignature(TokenTypes blobIdx)
		{
			return (RetrieveSignature<MethodSignature>(blobIdx) ?? StoreSignature<MethodSignature>(blobIdx, new MethodSignature(metadataProvider, blobIdx)));
		}

		private FieldSignature GetFieldSignature(TokenTypes blobIdx)
		{
			return (RetrieveSignature<FieldSignature>(blobIdx) ?? StoreSignature<FieldSignature>(blobIdx, new FieldSignature(metadataProvider, blobIdx)));
		}

		private TypeSpecSignature GetTypeSpecSignature(TokenTypes blobIdx)
		{
			return (RetrieveSignature<TypeSpecSignature>(blobIdx) ?? StoreSignature<TypeSpecSignature>(blobIdx, new TypeSpecSignature(metadataProvider, blobIdx)));
		}

		private Signature GetMemberRefSignature(TokenTypes blobIdx)
		{
			return (RetrieveSignature<Signature>(blobIdx) ?? StoreSignature<Signature>(blobIdx, Signature.FromMemberRefSignatureToken(metadataProvider, blobIdx)));
		}

		private struct TypeInfo
		{
			public TypeDefRow TypeDefRow;
			public MetadataToken NestedClass;
			public MetadataToken EnclosingClass;
			public MetadataToken MaxMethod;
			public MetadataToken MaxField;
			public int Size;
			public short PackingSize;
		}

		/// <summary>
		/// Loads all types from the given metadata module.
		/// </summary>
		private void LoadTypes()
		{
			List<TypeInfo> typeInfos = new List<TypeInfo>(GetTableRows(MetadataTable.TypeDef));

			MetadataToken maxTypeDef = GetMaxTokenValue(MetadataTable.TypeDef);
			MetadataToken maxMethod = GetMaxTokenValue(MetadataTable.MethodDef);
			MetadataToken maxField = GetMaxTokenValue(MetadataTable.Field);
			MetadataToken maxLayout = GetMaxTokenValue(MetadataTable.ClassLayout);
			MetadataToken maxNestedClass = GetMaxTokenValue(MetadataTable.NestedClass);

			MetadataToken tokenLayout = new MetadataToken(MetadataTable.ClassLayout, 1);
			ClassLayoutRow layoutRow = (maxLayout.RID != 0) ? metadataProvider.ReadClassLayoutRow(tokenLayout) : new ClassLayoutRow();

			MetadataToken tokenNested = new MetadataToken(MetadataTable.NestedClass, 1);
			NestedClassRow nestedRow = (maxNestedClass.RID != 0) ? metadataProvider.ReadNestedClassRow(tokenNested) : new NestedClassRow();

			TypeDefRow nextTypeDefRow = new TypeDefRow();
			TypeDefRow typeDefRow = metadataProvider.ReadTypeDefRow(new MetadataToken(MetadataTable.TypeDef, 1));

			foreach (MetadataToken token in new MetadataToken(MetadataTable.TypeDef, 1).Upto(maxTypeDef))
			{
				TypeInfo info = new TypeInfo();

				info.TypeDefRow = typeDefRow;
				info.NestedClass = (nestedRow.NestedClass == token) ? nestedRow.NestedClass : MetadataToken.Zero;
				info.EnclosingClass = (nestedRow.NestedClass == token) ? nestedRow.EnclosingClass : MetadataToken.Zero;
				info.Size = (layoutRow.Parent == token) ? layoutRow.ClassSize : 0;
				info.PackingSize = (layoutRow.Parent == token) ? layoutRow.PackingSize : (short)0;

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

			foreach (MetadataToken token in new MetadataToken(MetadataTable.TypeDef, 1).Upto(maxTypeDef))
			{
				LoadType(token, typeInfos);
			}
		}

		private void LoadType(MetadataToken token, IList<TypeInfo> typeInfos)
		{
			TypeInfo info = typeInfos[(int)token.RID - 1];

			if (types[token.RID - 1] != null)
				return;

			if (info.TypeDefRow.Extends.RID != 0)
			{
				if (info.TypeDefRow.Extends.Table == MetadataTable.TypeDef)
				{
					LoadType(info.TypeDefRow.Extends, typeInfos);
				}
				else if (info.TypeDefRow.Extends.Table != MetadataTable.TypeRef)
				{
					throw new ArgumentException(@"unexpected token type.", @"extends");
				}
			}

			RuntimeType baseType = GetType(info.TypeDefRow.Extends);
			RuntimeType enclosingType = (info.NestedClass == token) ? types[info.EnclosingClass.RID - 1] : null;

			// Create and populate the runtime type
			CilRuntimeType type = new CilRuntimeType(
				this,
				GetString(info.TypeDefRow.TypeNameIdx),
				GetString(info.TypeDefRow.TypeNamespaceIdx),
				info.PackingSize,
				info.Size,
				token,
				baseType,
				enclosingType,
				info.TypeDefRow.Flags,
				info.TypeDefRow.Extends
			);

			LoadMethods(type, info.TypeDefRow.MethodList, info.MaxMethod);
			LoadFields(type, info.TypeDefRow.FieldList, info.MaxField);
			types[token.RID - 1] = type;
		}

		/// <summary>
		/// Loads all methods from the given metadata module.
		/// </summary>
		/// <param name="declaringType">The type, which declared the method.</param>
		/// <param name="first">The first method token to load.</param>
		/// <param name="last">The last method token to load (non-inclusive.)</param>
		private void LoadMethods(RuntimeType declaringType, MetadataToken first, MetadataToken last)
		{
			if (first.RID >= last.RID)
				return;

			MetadataToken maxMethod = GetMaxTokenValue(MetadataTable.MethodDef);
			MethodDefRow methodDef = metadataProvider.ReadMethodDefRow(first);
			MethodDefRow nextMethodDef = new MethodDefRow();

			foreach (MetadataToken token in first.Upto(last.PreviousRow))
			{
				MetadataToken maxParam;

				if (token.RID < maxMethod.RID)
				{
					nextMethodDef = metadataProvider.ReadMethodDefRow(token.NextRow);
					maxParam = nextMethodDef.ParamList;
				}
				else
				{
					maxParam = GetMaxTokenValue(MetadataTable.Param).NextRow;
				}

				CilRuntimeMethod method = new CilRuntimeMethod(
					this,
					GetString(methodDef.NameStringIdx),
					GetMethodSignature(methodDef.SignatureBlobIdx),
					token,
					declaringType,
					methodDef.Flags,
					methodDef.ImplFlags,
					methodDef.Rva
				);

				LoadParameters(method, methodDef.ParamList, maxParam);
				declaringType.Methods.Add(method);

				methods[token.RID - 1] = method;
				methodDef = nextMethodDef;
			}
		}

		/// <summary>
		/// Loads the parameters.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <param name="first">The first.</param>
		/// <param name="max">The max.</param>
		private void LoadParameters(RuntimeMethod method, MetadataToken first, MetadataToken max)
		{
			foreach (MetadataToken token in first.Upto(max.PreviousRow))
			//for (TokenTypes token = first; token < max; token++)
			{
				ParamRow paramDef = metadataProvider.ReadParamRow(token);
				method.Parameters.Add(new RuntimeParameter(GetString(paramDef.NameIdx), paramDef.Sequence, paramDef.Flags));
			}
		}

		/// <summary>
		/// Loads all fields defined in the module.
		/// </summary>
		/// <param name="declaringType">The type, which declared the method.</param>
		/// <param name="first">The first field token to load.</param>
		/// <param name="last">The last field token to load (non-inclusive.)</param>
		private void LoadFields(RuntimeType declaringType, MetadataToken first, MetadataToken last)
		{
			MetadataToken maxRVA = GetMaxTokenValue(MetadataTable.FieldRVA);
			MetadataToken maxLayout = GetMaxTokenValue(MetadataTable.FieldLayout);
			MetadataToken tokenRva = new MetadataToken(MetadataTable.FieldRVA, 1);
			MetadataToken tokenLayout = new MetadataToken(MetadataTable.FieldLayout, 1);

			FieldRVARow fieldRVA = (maxRVA.RID != 0) ? metadataProvider.ReadFieldRVARow(tokenRva) : new FieldRVARow();
			FieldLayoutRow fieldLayout = (maxLayout.RID != 0) ? metadataProvider.ReadFieldLayoutRow(tokenLayout) : new FieldLayoutRow();

			foreach (MetadataToken token in first.Upto(last.PreviousRow))
			{
				FieldRow fieldRow = metadataProvider.ReadFieldRow(token);
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

				// Load the field metadata
				CilRuntimeField field = new CilRuntimeField(
					this,
					GetString(fieldRow.NameStringIdx),
					GetFieldSignature(fieldRow.SignatureBlobIdx),
					token,
					layout,
					rva,
					declaringType,
					fieldRow.Flags
				);

				declaringType.Fields.Add(field);
				fields[token.RID - 1] = field;
			}

			/* FIXME:
			 * Load FieldMarshal tables
			 * as needed afterwards. All Generics have been loaded, fields can receive
			 * their signature in the load method above.
			 */
		}

		/// <summary>
		/// Loads the interfaces.
		/// </summary>
		protected void LoadInterfaces()
		{
			MetadataToken maxToken = GetMaxTokenValue(MetadataTable.InterfaceImpl);

			foreach (MetadataToken token in new MetadataToken(MetadataTable.InterfaceImpl, 1).Upto(maxToken))
			{
				InterfaceImplRow row = metadataProvider.ReadInterfaceImplRow(token);

				RuntimeType declaringType = types[row.Class.RID - 1];
				RuntimeType interfaceType;

				if (row.Interface.Table == MetadataTable.TypeSpec)
					interfaceType = typeSpecs[row.Interface.RID - 1];
				else if (row.Interface.Table == MetadataTable.TypeDef)
					interfaceType = types[row.Interface.RID - 1];
				else
					interfaceType = typeRef[row.Interface.RID - 1];

				declaringType.Interfaces.Add(interfaceType);
			}

		}

		/// <summary>
		/// Loads the generic interfaces.
		/// </summary>
		protected void LoadGenericInterfaces()
		{
			foreach (RuntimeType type in typeSpecs)
			{
				CilGenericType genericType = type as CilGenericType;
				if (genericType != null)
				{
					genericType.ResolveInterfaces(this);
				}
			}
		}

		/// <summary>
		/// Loads the interfaces.
		/// </summary>
		protected void LoadMemberReferences()
		{
			MetadataToken maxToken = GetMaxTokenValue(MetadataTable.MemberRef);
			foreach (MetadataToken token in new MetadataToken(MetadataTable.MemberRef, 1).Upto(maxToken))
			{
				MemberRefRow row = metadataProvider.ReadMemberRefRow(token);
				string name = GetString(row.NameStringIdx);

				RuntimeType ownerType = null;

				switch (row.Class.Table)
				{
					case MetadataTable.TypeDef:
						ownerType = types[row.Class.RID - 1];
						break;

					case MetadataTable.TypeRef:
						ownerType = typeRef[row.Class.RID - 1];
						break;

					case MetadataTable.TypeSpec:
						ownerType = typeSpecs[row.Class.RID - 1];
						break;

					default:
						throw new NotSupportedException(String.Format(@"LoadMemberReferences() does not support token table {0}", row.Class.Table));
				}

				if (ownerType == null)
					throw new InvalidOperationException(String.Format(@"Failed to retrieve owner type for Token {0:x} (Table {1})", row.Class, row.Class.Table));

				Signature signature = GetMemberRefSignature(row.SignatureBlobIdx);

				CilGenericType genericOwnerType = ownerType as CilGenericType;

				RuntimeMember runtimeMember = null;
				if (signature is FieldSignature)
				{
					foreach (RuntimeField field in ownerType.Fields)
					{
						if (field.Name == name)
						{
							runtimeMember = field;
							break;
						}
					}
				}
				else
				{
					MethodSignature methodSignature = signature as MethodSignature;
					Debug.Assert(signature is MethodSignature);

					if ((genericOwnerType != null) && (genericOwnerType.GenericArguments.Length != 0))
					{
						methodSignature = new MethodSignature(methodSignature, genericOwnerType.GenericArguments);
					}

					foreach (RuntimeMethod method in ownerType.Methods)
					{
						if (method.Name == name)
						{
							if (method.Signature.Matches(methodSignature))
							{
								runtimeMember = method;
								break;
							}
						}
					}

					// Special case: string.get_Chars is same as string.get_Item
					if (runtimeMember == null && name == "get_Chars" && ownerType.FullName == "System.String")
					{
						name = "get_Item";

						foreach (RuntimeMethod method in ownerType.Methods)
						{
							if (method.Name == name)
							{
								if (method.Signature.Matches(methodSignature))
								{
									runtimeMember = method;
									break;
								}
							}
						}
					}
				}

				if (runtimeMember == null)
					throw new InvalidOperationException(String.Format(@"Failed to locate field {0}.{1}", ownerType.FullName, name));

				memberRef[token.RID - 1] = runtimeMember;
			}
		}

		/// <summary>
		/// Loads the interfaces.
		/// </summary>
		protected void LoadTypeReferences()
		{
			MetadataToken maxToken = GetMaxTokenValue(MetadataTable.TypeRef);
			foreach (MetadataToken token in new MetadataToken(MetadataTable.TypeRef, 1).Upto(maxToken))
			{
				RuntimeType runtimeType = null;

				TypeRefRow row = metadataProvider.ReadTypeRefRow(token);
				string typeName = GetString(row.TypeNameIdx);

				switch (row.ResolutionScope.Table)
				{
					case MetadataTable.Module:
						goto case MetadataTable.TypeRef;

					case MetadataTable.TypeDef:
						throw new NotImplementedException();

					case MetadataTable.TypeRef:
						{
							TypeRefRow row2 = metadataProvider.ReadTypeRefRow(row.ResolutionScope);
							string typeName2 = GetString(row2.TypeNameIdx);
							string typeNamespace2 = GetString(row2.TypeNamespaceIdx) + "." + typeName2;

							AssemblyRefRow asmRefRow = metadataProvider.ReadAssemblyRefRow(row2.ResolutionScope);
							string assemblyName = GetString(asmRefRow.NameIdx);
							ITypeModule module = typeSystem.ResolveModuleReference(assemblyName);
							runtimeType = module.GetType(typeNamespace2, typeName);

							if (runtimeType == null)
								throw new TypeLoadException("Could not find type: " + typeNamespace2 + Type.Delimiter + typeName);

							break;
						}

					case MetadataTable.TypeSpec:
						throw new NotImplementedException();

					case MetadataTable.ModuleRef:
						throw new NotImplementedException();

					case MetadataTable.AssemblyRef:
						{
							string typeNamespace = GetString(row.TypeNamespaceIdx);

							AssemblyRefRow asmRefRow = metadataProvider.ReadAssemblyRefRow(row.ResolutionScope);
							string assemblyName = GetString(asmRefRow.NameIdx);
							ITypeModule module = typeSystem.ResolveModuleReference(assemblyName);
							runtimeType = module.GetType(typeNamespace, typeName);

							if (runtimeType == null)
								throw new TypeLoadException("Could not find type: " + typeNamespace + Type.Delimiter + typeName);

							break;
						}

					default:
						throw new NotImplementedException();
				}

				typeRef[token.RID - 1] = runtimeType;
			}
		}

		/// <summary>
		/// Loads all custom attributes from the assembly.
		/// </summary>
		private void LoadCustomAttributes()
		{
			MetadataToken maxToken = GetMaxTokenValue(MetadataTable.CustomAttribute);
			foreach (MetadataToken token in new MetadataToken(MetadataTable.CustomAttribute, 1).Upto(maxToken))
			{
				CustomAttributeRow row = metadataProvider.ReadCustomAttributeRow(token);
				MetadataToken owner = row.Parent;

				RuntimeMethod ctorMethod;

				switch (row.Type.Table)
				{
					case MetadataTable.MethodDef:
						ctorMethod = methods[row.Type.RID - 1];
						break;

					case MetadataTable.MemberRef:
						ctorMethod = memberRef[row.Type.RID - 1] as RuntimeMethod;
						break;

					default:
						throw new NotImplementedException();
				}

				RuntimeAttribute runtimeAttribute = new RuntimeAttribute(this, row.Type, ctorMethod, row.ValueBlobIdx);

				// The following switch matches the AttributeTargets enumeration against
				// metadata tables, which make valid targets for an attribute.
				switch (owner.Table)
				{
					case MetadataTable.Assembly:
						// AttributeTargets.Assembly
						break;

					case MetadataTable.TypeDef:
						// AttributeTargets.Class
						// AttributeTargets.Delegate
						// AttributeTargets.Enum
						// AttributeTargets.Interface
						// AttributeTargets.Struct
						types[owner.RID - 1].CustomAttributes.Add(runtimeAttribute);
						break;

					case MetadataTable.MethodDef:
						// AttributeTargets.Constructor
						// AttributeTargets.Method
						methods[owner.RID - 1].CustomAttributes.Add(runtimeAttribute);
						break;

					case MetadataTable.Event:
						// AttributeTargets.Event
						break;

					case MetadataTable.Field:
						// AttributeTargets.Field
						fields[owner.RID - 1].CustomAttributes.Add(runtimeAttribute);
						break;

					case MetadataTable.GenericParam:
						// AttributeTargets.GenericParameter
						break;

					case MetadataTable.Module:
						// AttributeTargets.Module
						break;

					case MetadataTable.Param:
						// AttributeTargets.Parameter
						// AttributeTargets.ReturnValue
						break;

					case MetadataTable.Property:
						// AttributeTargets.StackFrameIndex
						break;

					//default:
					//    throw new NotImplementedException();
				}
			}

		}

		/// <summary>
		/// Loads the generic param constraints.
		/// </summary>
		private void LoadGenericParams()
		{
			MetadataToken maxToken = GetMaxTokenValue(MetadataTable.GenericParam);
			foreach (MetadataToken token in new MetadataToken(MetadataTable.GenericParam, 1).Upto(maxToken))
			{
				GenericParamRow row = metadataProvider.ReadGenericParamRow(token);
				string name = GetString(row.NameStringIdx);

				// The following switch matches the AttributeTargets enumeration against
				// metadata tables, which make valid targets for an attribute.
				switch (row.Owner.Table)
				{
					case MetadataTable.TypeDef:
						types[row.Owner.RID - 1].GenericParameters.Add(new GenericParameter(name, row.Flags));
						break;

					case MetadataTable.MethodDef:
						methods[row.Owner.RID - 1].GenericParameters.Add(new GenericParameter(name, row.Flags));
						break;

					default:
						throw new NotImplementedException();
				}
			}
		}

		/// <summary>
		/// Loads the type specs.
		/// </summary>
		private void LoadTypeSpecs()
		{
			MetadataToken maxToken = GetMaxTokenValue(MetadataTable.TypeSpec);
			foreach (MetadataToken token in new MetadataToken(MetadataTable.TypeSpec, 1).Upto(maxToken))
			{
				TypeSpecRow row = metadataProvider.ReadTypeSpecRow(token);
				TypeSpecSignature signature = GetTypeSpecSignature(row.SignatureBlobIdx);

				GenericInstSigType genericSigType = signature.Type as GenericInstSigType;

				if (genericSigType != null)
				{
					RuntimeType genericType = null;
					SigType sigType = genericSigType;

					switch (genericSigType.Type)
					{
						case CilElementType.ValueType:
							goto case CilElementType.Class;

						case CilElementType.Class:
							TypeSigType typeSigType = (TypeSigType)sigType;
							genericType = types[typeSigType.Token.RID];	// NOTE: Should this be -1
							break;

						case CilElementType.GenericInst:
							GenericInstSigType genericSigType2 = (GenericInstSigType)sigType;
							RuntimeType genericBaseType = null;

							if (genericSigType2.BaseType.Token.Table == MetadataTable.TypeDef)
								genericBaseType = types[genericSigType2.BaseType.Token.RID - 1];
							else if (genericSigType2.BaseType.Token.Table == MetadataTable.TypeRef)
								genericBaseType = typeRef[genericSigType2.BaseType.Token.RID - 1];

							genericType = new CilGenericType(this, genericBaseType, genericSigType, token, this);
							break;

						default:
							throw new NotSupportedException(String.Format(@"LoadTypeSpecs does not support CilElementType.{0}", genericSigType.Type));
					}

					typeSpecs[token.RID - 1] = genericType;
				}
				else
				{
					if (signature.Type is MVarSigType)
						continue;
					else if (signature.Type is SZArraySigType)
						continue;
					else
						continue;
				}
			}

		}

		/// <summary>
		/// Loads the externals.
		/// </summary>
		private void LoadExternals()
		{

			MetadataToken maxToken = GetMaxTokenValue(MetadataTable.ImplMap);
			foreach (MetadataToken token in new MetadataToken(MetadataTable.ImplMap, 1).Upto(maxToken))
			{
				ImplMapRow row = metadataProvider.ReadImplMapRow(token);

				//TODO: verify row.ImportScopeTableIdx indexes MethodDef and nothing else

				ModuleRefRow moduleRow = metadataProvider.ReadModuleRefRow(row.ImportScopeTableIdx);

				string external = GetString(moduleRow.NameStringIdx);

				externals.Add(row.MemberForwarded, external);
			}
		}

		/// <summary>
		/// Retrieves the runtime type for a given metadata token.
		/// </summary>
		/// <param name="token">The token of the type to load. This can represent a typeref, typedef or typespec token.</param>
		/// <returns>The runtime type of the specified token.</returns>
		private RuntimeType GetType(MetadataToken token)
		{
			int index = (int)token.RID - 1;

			if (index < 0)
				return null;

			switch (token.Table)
			{
				case MetadataTable.TypeDef:
					return types[index];

				case MetadataTable.TypeRef:
					return typeRef[index];

				case MetadataTable.TypeSpec:
					return typeSpecs[index];

				default:
					throw new ArgumentException(@"Not a type token.", @"token");
			}
		}

		#endregion

		#region ITypeModule interface

		/// <summary>
		/// Gets the type system.
		/// </summary>
		/// <value>The type system.</value>
		ITypeSystem ITypeModule.TypeSystem { get { return typeSystem; } }

		/// <summary>
		/// Gets the metadata module.
		/// </summary>
		/// <value>The metadata module.</value>
		IMetadataModule ITypeModule.MetadataModule { get { return metadataModule; } }

		/// <summary>
		/// Gets the module's name.
		/// </summary>
		/// <value>The module's name.</value>
		string ITypeModule.Name { get { return metadataModule.Name; } }

		/// <summary>
		/// Gets all types from module.
		/// </summary>
		/// <returns></returns>
		IEnumerable<RuntimeType> ITypeModule.GetAllTypes()
		{
			foreach (RuntimeType type in types)
				yield return type;

			foreach (RuntimeType type in typeSpecs)
				if (type != null)
					yield return type;
		}

		/// <summary>
		/// Gets the runtime type for the given type name and namespace
		/// </summary>
		/// <param name="nameSpace">The name space.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		RuntimeType ITypeModule.GetType(string nameSpace, string name)
		{
			foreach (RuntimeType type in types)
			{
				if (type.Name == name && type.Namespace == nameSpace)
				{
					return type;
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the runtime type for the given type name and namespace
		/// </summary>
		/// <param name="fullname">The fullname.</param>
		/// <returns></returns>
		RuntimeType ITypeModule.GetType(string fullname)
		{
			int dot = fullname.LastIndexOf(".");

			if (dot < 0)
				return null;

			return ((ITypeModule)this).GetType(fullname.Substring(0, dot), fullname.Substring(dot + 1));
		}

		/// <summary>
		/// Retrieves the runtime type for a given metadata token.
		/// </summary>
		/// <param name="token">The token of the type to load. This can represent a typeref, typedef or typespec token.</param>
		/// <returns>The runtime type of the specified token.</returns>
		RuntimeType ITypeModule.GetType(MetadataToken token)
		{
			return GetType(token);
		}

		/// <summary>
		/// Retrieves the field definition identified by the given token in the scope.
		/// </summary>
		/// <param name="token">The token of the field to retrieve.</param>
		/// <returns></returns>
		RuntimeField ITypeModule.GetField(MetadataToken token)
		{
			switch (token.Table)
			{
				case MetadataTable.Field:
					return fields[token.RID - 1];

				case MetadataTable.MemberRef:
					return memberRef[token.RID - 1] as RuntimeField;

				default:
					throw new NotSupportedException(@"Can't get method for token " + token.ToString());
			}
		}

		/// <summary>
		/// Retrieves the method definition identified by the given token in the scope.
		/// </summary>
		/// <param name="token">The token of the method to retrieve.</param>
		/// <returns></returns>
		RuntimeMethod ITypeModule.GetMethod(MetadataToken token)
		{
			switch (token.Table)
			{
				case MetadataTable.MethodDef:
					return methods[token.RID - 1];

				case MetadataTable.MemberRef:
					return memberRef[token.RID - 1] as RuntimeMethod;

				case MetadataTable.MethodSpec:
					//TODO
					//    return DecodeMethodSpec(token);
					//    break;
					return null;

				default:
					throw new NotSupportedException(@"Can't get method for token " + token.ToString());
			}
		}

		/// <summary>
		/// Gets the method.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="callingType">Type of the calling.</param>
		/// <returns></returns>
		RuntimeMethod ITypeModule.GetMethod(MetadataToken token, RuntimeType callingType)
		{
			RuntimeMethod calledMethod = (this as ITypeModule).GetMethod(token);

			if (callingType == null)
				return calledMethod;

			if (calledMethod.DeclaringType.Namespace != callingType.Namespace)
				return calledMethod;

			string declaringTypeName = calledMethod.DeclaringType.Name;
			if (declaringTypeName.Contains("<"))
				declaringTypeName = declaringTypeName.Substring(0, declaringTypeName.IndexOf('<'));

			string callingTypeName = callingType.Name;
			if (callingTypeName.Contains("<"))
				callingTypeName = callingTypeName.Substring(0, callingTypeName.IndexOf('<'));

			if (declaringTypeName != callingTypeName)
				return calledMethod;

			foreach (RuntimeMethod m in callingType.Methods)
			{
				if (calledMethod.Name == m.Name)
				{
					return m;
				}
			}

			return calledMethod;
		}

		/// <summary>
		/// Gets the open generic.
		/// </summary>
		/// <param name="baseGenericType">Type of the base generic.</param>
		/// <returns></returns>
		CilGenericType ITypeModule.GetOpenGeneric(RuntimeType baseGenericType)
		{
			if (baseGenericType.IsInterface || baseGenericType.IsModule)
				return null;

			if (baseGenericType.GenericParameters.Count == 0)
				return null;

			foreach (RuntimeType type in typeSpecs)
			{
				CilGenericType genericType = type as CilGenericType;
				if (genericType != null)
				{
					if (genericType.BaseGenericType == baseGenericType)
					{
						if (genericType.ContainsOpenGenericParameters)
						{
							bool open = true;
							foreach (SigType sigType in genericType.GenericArguments)
							{
								if (!sigType.IsOpenGenericParameter)
								{
									open = false;
									break;
								}
							}

							if (open)
								return genericType;
						}
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the name of the external.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		string ITypeModule.GetExternalName(MetadataToken token)
		{
			string name = null;

			externals.TryGetValue(token, out name);

			return name;
		}

		#endregion

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return ((ITypeModule)(this)).Name;
		}
	}
}
