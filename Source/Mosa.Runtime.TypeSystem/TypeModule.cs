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

using Mono.Cecil;

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
		private Dictionary<TokenTypes, string> strings;

		/// <summary>
		/// 
		/// </summary>
		private Dictionary<TokenTypes, Signature> signatures;

		/// <summary>
		/// 
		/// </summary>
		private int[] tableRows = new int[(int)TokenTypes.MaxTable >> 24];

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

			this.methods = new RuntimeMethod[GetTableRows(TokenTypes.MethodDef)];
			this.fields = new RuntimeField[GetTableRows(TokenTypes.Field)];
			this.types = new RuntimeType[GetTableRows(TokenTypes.TypeDef)];

			this.typeSpecs = new RuntimeType[GetTableRows(TokenTypes.TypeSpec)];
			this.methodSpecs = new RuntimeMethod[GetTableRows(TokenTypes.MethodSpec)];

			this.memberRef = new RuntimeMember[GetTableRows(TokenTypes.MemberRef)];
			this.typeRef = new RuntimeType[GetTableRows(TokenTypes.TypeRef)];
			this.genericParamConstraint = new RuntimeType[GetTableRows(TokenTypes.GenericParamConstraint)];

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
		}

		#endregion // Construction

		#region Internals

		private void RetrieveAllTableSizes()
		{
			for (int i = 0; i < (int)TokenTypes.MaxTable >> 24; i++)
			{
				tableRows[i] = (int)(metadataProvider.GetMaxTokenValue((TokenTypes)(i << 24)) & TokenTypes.RowIndexMask);
			}
		}

		/// <summary>
		/// Gets the table rows.
		/// </summary>
		/// <param name="tokenType">Type of the token.</param>
		/// <returns></returns>
		private int GetTableRows(TokenTypes tokenType)
		{
			return tableRows[(int)(tokenType) >> 24];
		}

		private TokenTypes GetMaxTokenValue(TokenTypes tokenType)
		{
			return (TokenTypes)(tableRows[(int)(tokenType) >> 24]) | (tokenType & TokenTypes.TableMask);
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
			public TokenTypes NestedClassTableIdx;
			public TokenTypes EnclosingClassTableIdx;
			public TokenTypes MaxMethod;
			public TokenTypes MaxField;
			public int Size;
			public short PackingSize;
		}

		/// <summary>
		/// Loads all types from the given metadata module.
		/// </summary>
		private void LoadTypes()
		{
			List<TypeInfo> typeInfos = new List<TypeInfo>(GetTableRows(TokenTypes.TypeDef));

			TokenTypes maxTypeDef = GetMaxTokenValue(TokenTypes.TypeDef);
			TokenTypes maxMethod = GetMaxTokenValue(TokenTypes.MethodDef);
			TokenTypes maxField = GetMaxTokenValue(TokenTypes.Field);
			TokenTypes maxLayout = GetMaxTokenValue(TokenTypes.ClassLayout);
			TokenTypes maxNestedClass = GetMaxTokenValue(TokenTypes.NestedClass);

			TokenTypes tokenLayout = TokenTypes.ClassLayout + 1;
			ClassLayoutRow layoutRow = (TokenTypes.ClassLayout < maxLayout) ? metadataProvider.ReadClassLayoutRow(tokenLayout) : new ClassLayoutRow();

			TokenTypes tokenNested = TokenTypes.NestedClass + 1;
			NestedClassRow nestedRow = (TokenTypes.NestedClass < maxNestedClass) ? metadataProvider.ReadNestedClassRow(tokenNested) : new NestedClassRow();

			TypeDefRow nextTypeDefRow = new TypeDefRow();
			TypeDefRow typeDefRow = metadataProvider.ReadTypeDefRow(TokenTypes.TypeDef + 1);

			for (TokenTypes token = TokenTypes.TypeDef + 1; token <= maxTypeDef; token++)
			{
				TypeInfo info = new TypeInfo();

				info.TypeDefRow = typeDefRow;
				info.NestedClassTableIdx = (nestedRow.NestedClassTableIdx == token) ? nestedRow.NestedClassTableIdx : 0;
				info.EnclosingClassTableIdx = (nestedRow.NestedClassTableIdx == token) ? nestedRow.EnclosingClassTableIdx : 0;
				info.Size = (layoutRow.ParentTypeDefIdx == token) ? layoutRow.ClassSize : 0;
				info.PackingSize = (layoutRow.ParentTypeDefIdx == token) ? layoutRow.PackingSize : (short)0;

				if (token < maxTypeDef)
				{
					nextTypeDefRow = metadataProvider.ReadTypeDefRow(token + 1);

					info.MaxField = nextTypeDefRow.FieldList;
					info.MaxMethod = nextTypeDefRow.MethodList;

					if (info.MaxMethod > maxMethod)
						info.MaxMethod = maxMethod + 1;

					if (info.MaxField > maxField)
						info.MaxField = maxField + 1;
				}
				else
				{
					info.MaxMethod = maxMethod + 1;
					info.MaxField = maxField + 1;
				}

				typeInfos.Add(info);

				if (layoutRow.ParentTypeDefIdx == token)
				{
					tokenLayout++;
					if (tokenLayout <= maxLayout)
						layoutRow = metadataProvider.ReadClassLayoutRow(tokenLayout);
				}

				if (nestedRow.NestedClassTableIdx == token)
				{
					tokenNested++;
					if (tokenNested <= maxNestedClass)
						nestedRow = metadataProvider.ReadNestedClassRow(tokenNested);
				}

				typeDefRow = nextTypeDefRow;
			}

			for (TokenTypes token = TokenTypes.TypeDef + 1; token <= maxTypeDef; token++)
			{
				LoadType(token, typeInfos);
			}
		}

		private void LoadType(TokenTypes token, IList<TypeInfo> typeInfos)
		{
			int index = (int)(token & TokenTypes.RowIndexMask) - 1;
			TypeInfo info = typeInfos[index];

			if (types[index] != null)
				return;

			if ((info.TypeDefRow.Extends & TokenTypes.RowIndexMask) != 0)
			{
				if ((info.TypeDefRow.Extends & TokenTypes.TableMask) == TokenTypes.TypeDef)
				{
					LoadType(info.TypeDefRow.Extends, typeInfos);
				}
				else if ((info.TypeDefRow.Extends & TokenTypes.TableMask) != TokenTypes.TypeRef)
				{
					throw new ArgumentException(@"unexpected token type.", @"extends");
				}
			}

			RuntimeType baseType = GetType(info.TypeDefRow.Extends);
			RuntimeType enclosingType = (info.NestedClassTableIdx == token) ? types[(int)(info.EnclosingClassTableIdx & TokenTypes.RowIndexMask) - 1] : null;

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
			types[index] = type;
		}

		/// <summary>
		/// Loads all methods from the given metadata module.
		/// </summary>
		/// <param name="declaringType">The type, which declared the method.</param>
		/// <param name="first">The first method token to load.</param>
		/// <param name="last">The last method token to load (non-inclusive.)</param>
		private void LoadMethods(RuntimeType declaringType, TokenTypes first, TokenTypes last)
		{
			if (first >= last)
				return;

			TokenTypes maxMethod = GetMaxTokenValue(TokenTypes.MethodDef);
			MethodDefRow methodDef = metadataProvider.ReadMethodDefRow(first);
			MethodDefRow nextMethodDef = new MethodDefRow();

			for (TokenTypes token = first; token < last; token++)
			{
				TokenTypes maxParam;

				if (token < maxMethod)
				{
					nextMethodDef = metadataProvider.ReadMethodDefRow(token + 1);
					maxParam = nextMethodDef.ParamList;
				}
				else
				{
					maxParam = GetMaxTokenValue(TokenTypes.Param) + 1;
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

				methods[(int)(token & TokenTypes.RowIndexMask) - 1] = method;
				methodDef = nextMethodDef;
			}
		}

		/// <summary>
		/// Loads the parameters.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <param name="first">The first.</param>
		/// <param name="max">The max.</param>
		private void LoadParameters(RuntimeMethod method, TokenTypes first, TokenTypes max)
		{
			for (TokenTypes token = first; token < max; token++)
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
		private void LoadFields(RuntimeType declaringType, TokenTypes first, TokenTypes last)
		{
			TokenTypes maxRVA = GetMaxTokenValue(TokenTypes.FieldRVA);
			TokenTypes maxLayout = GetMaxTokenValue(TokenTypes.FieldLayout);
			TokenTypes tokenRva = TokenTypes.FieldRVA + 1;
			TokenTypes tokenLayout = TokenTypes.FieldLayout + 1;

			FieldRVARow fieldRVA = new FieldRVARow();
			FieldLayoutRow fieldLayout = new FieldLayoutRow();

			if (TokenTypes.FieldRVA < maxRVA)
			{
				fieldRVA = metadataProvider.ReadFieldRVARow(tokenRva);
			}

			if (TokenTypes.FieldLayout < maxLayout)
			{
				fieldLayout = metadataProvider.ReadFieldLayoutRow(tokenLayout);
			}

			for (TokenTypes token = first; token < last; token++)
			{
				FieldRow fieldRow = metadataProvider.ReadFieldRow(token);
				uint rva = 0;
				uint layout = 0;

				// Static fields have an optional RVA, non-static may have a layout assigned
				if ((fieldRow.Flags & FieldAttributes.HasFieldRVA) == FieldAttributes.HasFieldRVA)
				{
					// Move to the RVA of this field
					while (fieldRVA.FieldTableIdx < token && tokenRva <= maxRVA)
					{
						fieldRVA = metadataProvider.ReadFieldRVARow(tokenRva++);
					}

					// Does this field have an RVA?
					if (token == fieldRVA.FieldTableIdx && tokenRva <= maxRVA)
					{
						rva = fieldRVA.Rva;
						tokenRva++;
						if (tokenRva < maxRVA)
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
					while (fieldLayout.Field < token && tokenLayout <= maxLayout)
						fieldLayout = metadataProvider.ReadFieldLayoutRow(tokenLayout++);

					// Does this field have layout?
					if (token == fieldLayout.Field && tokenLayout <= maxLayout)
					{
						layout = fieldLayout.Offset;
						tokenLayout++;
						if (tokenLayout < maxLayout)
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
				fields[(int)(token & TokenTypes.RowIndexMask) - 1] = field;
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
			TokenTypes maxToken = GetMaxTokenValue(TokenTypes.InterfaceImpl);
			for (TokenTypes token = TokenTypes.InterfaceImpl + 1; token <= maxToken; token++)
			{
				InterfaceImplRow row = metadataProvider.ReadInterfaceImplRow(token);

				RuntimeType declaringType = types[(int)(row.ClassTableIdx & TokenTypes.RowIndexMask) - 1];
				RuntimeType interfaceType;

				if ((row.InterfaceTableIdx & TokenTypes.TableMask) == TokenTypes.TypeSpec)
				{
					interfaceType = typeSpecs[(int)(row.InterfaceTableIdx & TokenTypes.RowIndexMask) - 1];
				}
				else
				{
					interfaceType = types[(int)(row.InterfaceTableIdx & TokenTypes.RowIndexMask) - 1];
				}

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
			TokenTypes maxToken = GetMaxTokenValue(TokenTypes.MemberRef);
			for (TokenTypes token = TokenTypes.MemberRef + 1; token <= maxToken; token++)
			{
				MemberRefRow row = metadataProvider.ReadMemberRefRow(token);
				string name = GetString(row.NameStringIdx);

				RuntimeType ownerType = null;

				switch (row.ClassTableIdx & TokenTypes.TableMask)
				{
					case TokenTypes.TypeDef:
						ownerType = types[(int)(row.ClassTableIdx & TokenTypes.RowIndexMask) - 1];
						break;

					case TokenTypes.TypeRef:
						ownerType = typeRef[(int)(row.ClassTableIdx & TokenTypes.RowIndexMask) - 1];
						break;

					case TokenTypes.TypeSpec:
						ownerType = typeSpecs[(int)(row.ClassTableIdx & TokenTypes.RowIndexMask) - 1];
						break;

					default:
						throw new NotSupportedException(String.Format(@"LoadMemberReferences() does not support token table {0}", row.ClassTableIdx & TokenTypes.TableMask));
				}

				if (ownerType == null)
					throw new InvalidOperationException(String.Format(@"Failed to retrieve owner type for Token {0:x} (Table {1})", row.ClassTableIdx, row.ClassTableIdx & TokenTypes.TableMask));

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

				memberRef[(int)(token & TokenTypes.RowIndexMask) - 1] = runtimeMember;
			}
		}

		/// <summary>
		/// Loads the interfaces.
		/// </summary>
		protected void LoadTypeReferences()
		{
			TokenTypes maxToken = GetMaxTokenValue(TokenTypes.TypeRef);
			for (TokenTypes token = TokenTypes.TypeRef + 1; token <= maxToken; token++)
			{
				RuntimeType runtimeType = null;

				TypeRefRow row = metadataProvider.ReadTypeRefRow(token);
				string typeName = GetString(row.TypeNameIdx);
				//string typenamespace = GetString(row.TypeNamespaceIdx);
				if (row.ResolutionScopeIdx == 0)
					throw new NotImplementedException();

				switch (row.ResolutionScopeIdx & TokenTypes.TableMask)
				{
					case TokenTypes.Module:
						goto case TokenTypes.TypeRef;

					case TokenTypes.TypeDef:
						throw new NotImplementedException();

					case TokenTypes.TypeRef:
						{
							TypeRefRow row2 = metadataProvider.ReadTypeRefRow(row.ResolutionScopeIdx);
							string typeName2 = GetString(row2.TypeNameIdx);
							string typeNamespace2 = GetString(row2.TypeNamespaceIdx) + "." + typeName2;

							AssemblyRefRow asmRefRow = metadataProvider.ReadAssemblyRefRow(row2.ResolutionScopeIdx);
							string assemblyName = GetString(asmRefRow.NameIdx);
							ITypeModule module = typeSystem.ResolveModuleReference(assemblyName);
							runtimeType = module.GetType(typeNamespace2, typeName);

							if (runtimeType == null)
								throw new TypeLoadException("Could not find type: " + typeNamespace2 + Type.Delimiter + typeName);

							break;
						}

					case TokenTypes.TypeSpec:
						throw new NotImplementedException();

					case TokenTypes.ModuleRef:
						throw new NotImplementedException();

					case TokenTypes.AssemblyRef:
						{
							string typeNamespace = GetString(row.TypeNamespaceIdx);

							AssemblyRefRow asmRefRow = metadataProvider.ReadAssemblyRefRow(row.ResolutionScopeIdx);
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

				typeRef[(int)(token & TokenTypes.RowIndexMask) - 1] = runtimeType;
			}
		}

		/// <summary>
		/// Loads all custom attributes from the assembly.
		/// </summary>
		private void LoadCustomAttributes()
		{
			TokenTypes maxToken = GetMaxTokenValue(TokenTypes.CustomAttribute);
			for (TokenTypes token = TokenTypes.CustomAttribute + 1; token <= maxToken; token++)
			{
				CustomAttributeRow row = metadataProvider.ReadCustomAttributeRow(token);
				TokenTypes owner = row.ParentTableIdx;

				RuntimeMethod ctorMethod;

				switch (row.TypeIdx & TokenTypes.TableMask)
				{
					case TokenTypes.MethodDef:
						ctorMethod = methods[(int)(row.TypeIdx & TokenTypes.RowIndexMask) - 1];
						break;

					case TokenTypes.MemberRef:
						ctorMethod = memberRef[(int)(row.TypeIdx & TokenTypes.RowIndexMask) - 1] as RuntimeMethod;
						break;

					default:
						throw new NotImplementedException();
				}

				//byte[] blob = metadataProvider.ReadBlob(row.ValueBlobIdx);
				RuntimeAttribute runtimeAttribute = new RuntimeAttribute(this, row.TypeIdx, ctorMethod, row.ValueBlobIdx);

				// The following switch matches the AttributeTargets enumeration against
				// metadata tables, which make valid targets for an attribute.
				switch (owner & TokenTypes.TableMask)
				{
					case TokenTypes.Assembly:
						// AttributeTargets.Assembly
						break;

					case TokenTypes.TypeDef:
						// AttributeTargets.Class
						// AttributeTargets.Delegate
						// AttributeTargets.Enum
						// AttributeTargets.Interface
						// AttributeTargets.Struct
						types[(int)(owner & TokenTypes.RowIndexMask) - 1].CustomAttributes.Add(runtimeAttribute);
						break;

					case TokenTypes.MethodDef:
						// AttributeTargets.Constructor
						// AttributeTargets.Method
						methods[(int)(owner & TokenTypes.RowIndexMask) - 1].CustomAttributes.Add(runtimeAttribute);
						break;

					case TokenTypes.Event:
						// AttributeTargets.Event
						break;

					case TokenTypes.Field:
						// AttributeTargets.Field
						fields[(int)(owner & TokenTypes.RowIndexMask) - 1].CustomAttributes.Add(runtimeAttribute);
						break;

					case TokenTypes.GenericParam:
						// AttributeTargets.GenericParameter
						break;

					case TokenTypes.Module:
						// AttributeTargets.Module
						break;

					case TokenTypes.Param:
						// AttributeTargets.Parameter
						// AttributeTargets.ReturnValue
						break;

					case TokenTypes.Property:
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
			TokenTypes maxToken = GetMaxTokenValue(TokenTypes.GenericParam);
			for (TokenTypes token = TokenTypes.GenericParam + 1; token <= maxToken; token++)
			{
				GenericParamRow row = metadataProvider.ReadGenericParamRow(token);
				string name = GetString(row.NameStringIdx);

				// The following switch matches the AttributeTargets enumeration against
				// metadata tables, which make valid targets for an attribute.
				switch (row.OwnerTableIdx & TokenTypes.TableMask)
				{
					case TokenTypes.TypeDef:
						types[(int)(row.OwnerTableIdx & TokenTypes.RowIndexMask) - 1].GenericParameters.Add(new GenericParameter(name, row.Flags));
						break;

					case TokenTypes.MethodDef:
						methods[(int)(row.OwnerTableIdx & TokenTypes.RowIndexMask) - 1].GenericParameters.Add(new GenericParameter(name, row.Flags));
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
			TokenTypes maxToken = GetMaxTokenValue(TokenTypes.TypeSpec);
			for (TokenTypes token = TokenTypes.TypeSpec + 1; token <= maxToken; token++)
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
							genericType = types[(int)typeSigType.Token];
							break;

						case CilElementType.GenericInst:
							GenericInstSigType genericSigType2 = (GenericInstSigType)sigType;
							RuntimeType genericBaseType = types[(int)(genericSigType2.BaseType.Token & TokenTypes.RowIndexMask) - 1];
							genericType = new CilGenericType(this, genericBaseType, genericSigType, token, this);
							break;

						default:
							throw new NotSupportedException(String.Format(@"LoadTypeSpecs does not support CilElementType.{0}", genericSigType.Type));
					}

					typeSpecs[(int)(token & TokenTypes.RowIndexMask) - 1] = genericType;
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
		/// Retrieves the runtime type for a given metadata token.
		/// </summary>
		/// <param name="token">The token of the type to load. This can represent a typeref, typedef or typespec token.</param>
		/// <returns>The runtime type of the specified token.</returns>
		private RuntimeType GetType(TokenTypes token)
		{
			int index = (int)(token & TokenTypes.RowIndexMask);

			if (index == 0)
				return null;

			switch (token & TokenTypes.TableMask)
			{
				case TokenTypes.TypeDef:
					return types[index - 1];

				case TokenTypes.TypeRef:
					return typeRef[index - 1];

				case TokenTypes.TypeSpec:
					return typeSpecs[index - 1];

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
		RuntimeType ITypeModule.GetType(TokenTypes token)
		{
			return GetType(token);
		}

		/// <summary>
		/// Retrieves the field definition identified by the given token in the scope.
		/// </summary>
		/// <param name="token">The token of the field to retrieve.</param>
		/// <returns></returns>
		RuntimeField ITypeModule.GetField(TokenTypes token)
		{
			switch (TokenTypes.TableMask & token)
			{
				case TokenTypes.Field:
					return fields[(int)(token & TokenTypes.RowIndexMask) - 1];

				case TokenTypes.MemberRef:
					return memberRef[(int)(token & TokenTypes.RowIndexMask) - 1] as RuntimeField;

				default:
					throw new NotSupportedException(@"Can't get method for token " + token.ToString("x"));
			}
		}

		/// <summary>
		/// Retrieves the method definition identified by the given token in the scope.
		/// </summary>
		/// <param name="token">The token of the method to retrieve.</param>
		/// <returns></returns>
		RuntimeMethod ITypeModule.GetMethod(TokenTypes token)
		{
			switch (TokenTypes.TableMask & token)
			{
				case TokenTypes.MethodDef:
					return methods[(int)(TokenTypes.RowIndexMask & token) - 1];

				case TokenTypes.MemberRef:
					return memberRef[(int)(token & TokenTypes.RowIndexMask) - 1] as RuntimeMethod;

				case TokenTypes.MethodSpec:
					//TODO
					//    return DecodeMethodSpec(token);
					//    break;
					return null;

				default:
					throw new NotSupportedException(@"Can't get method for token " + token.ToString("x"));
			}
		}

		/// <summary>
		/// Gets the method.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <param name="callingType">Type of the calling.</param>
		/// <returns></returns>
		RuntimeMethod ITypeModule.GetMethod(TokenTypes token, RuntimeType callingType)
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
