using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem.Cil;

namespace Mosa.Runtime.TypeSystem
{
	public class TypeModule : ITypeModule
	{
		#region Data members

		/// <summary>
		/// Holds the type system
		/// </summary>
		private readonly ITypeSystem TypeSystem;

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
		/// Holds all parameter information elements.
		/// </summary>
		private RuntimeParameter[] parameters;

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
		Dictionary<TokenTypes, string> strings;

		/// <summary>
		/// 
		/// </summary>
		Dictionary<TokenTypes, Signature> signatures;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes static data members of the type loader.
		/// </summary>
		/// <param name="metadataProvider">The metadata provider.</param>
		public TypeModule(ITypeSystem TypeSystem, IMetadataModule metadataModule)
		{
			Debug.Assert(metadataProvider != null);
			Debug.Assert(TypeSystem != null);

			this.TypeSystem = TypeSystem;
			this.metadataModule = metadataModule;
			this.metadataProvider = metadataModule.Metadata;

			methods = new RuntimeMethod[GetTableRows(TokenTypes.MethodDef)];
			fields = new RuntimeField[GetTableRows(TokenTypes.Field)];
			types = new RuntimeType[GetTableRows(TokenTypes.TypeDef)];
			parameters = new RuntimeParameter[GetTableRows(TokenTypes.Param)];

			typeSpecs = new RuntimeType[GetTableRows(TokenTypes.TypeSpec)];
			methodSpecs = new RuntimeMethod[GetTableRows(TokenTypes.MethodSpec)];

			memberRef = new RuntimeMember[GetTableRows(TokenTypes.MemberRef)];
			typeRef = new RuntimeType[GetTableRows(TokenTypes.TypeRef)];

			signatures = new Dictionary<TokenTypes, Signature>();
			strings = new Dictionary<TokenTypes, string>();

			// Load all types from the assembly into the type array
			LoadTypes();
			//LoadGenerics();
			//LoadTypeSpecs();
			LoadParameters();
			//LoadCustomAttributes();
			LoadInterfaces();
			LoadTypeReferences();	
			LoadMemberReferences();
		}

		#endregion // Construction

		#region Internals

		private string GetString(TokenTypes stringIdx)
		{
			string value;

			if (strings.TryGetValue(stringIdx, out value))
				return value;

			value = metadataProvider.ReadString(stringIdx);
			strings.Add(stringIdx, value);

			return value;
		}

		private Signature RetrieveSignature(TokenTypes blobIdx)
		{
			Signature signature;

			if (signatures.TryGetValue(blobIdx, out signature))
				return signature;
			else
				return null;
		}

		private Signature StoreSignature(TokenTypes blobIdx, Signature signature)
		{
			signatures.Add(blobIdx, signature);
			return signature;
		}

		private MethodSignature GetMethodSignature(TokenTypes blobIdx)
		{
			return (RetrieveSignature(blobIdx) ?? StoreSignature(blobIdx, new MethodSignature(metadataProvider, blobIdx))) as MethodSignature;
		}

		private FieldSignature GetFieldSignature(TokenTypes blobIdx)
		{
			return (RetrieveSignature(blobIdx) ?? StoreSignature(blobIdx, new FieldSignature(metadataProvider, blobIdx))) as FieldSignature;
		}

		/// <summary>
		/// Gets the table rows.
		/// </summary>
		/// <param name="tokenType">Type of the token.</param>
		/// <returns></returns>
		private int GetTableRows(TokenTypes tokenType)
		{
			return (int)(TokenTypes.RowIndexMask & metadataProvider.GetMaxTokenValue(tokenType));
		}

		/// <summary>
		/// Loads all types from the given metadata module.
		/// </summary>
		private void LoadTypes()
		{
			int typeOffset = 0;
			int methodOffset = 0;
			int fieldOffset = 0;
			int size = 0;
			int packing = 0;

			TokenTypes maxTypeDef = metadataProvider.GetMaxTokenValue(TokenTypes.TypeDef);
			TokenTypes maxLayout = metadataProvider.GetMaxTokenValue(TokenTypes.ClassLayout);
			TokenTypes maxMethod = metadataProvider.GetMaxTokenValue(TokenTypes.MethodDef);
			TokenTypes maxField = metadataProvider.GetMaxTokenValue(TokenTypes.Field);

			TokenTypes tokenLayout = TokenTypes.ClassLayout + 1;
			ClassLayoutRow layoutRow = new ClassLayoutRow();

			if (TokenTypes.ClassLayout < maxLayout)
			{
				layoutRow = metadataProvider.ReadClassLayoutRow(tokenLayout);
			}

			TypeDefRow typeDefRow = metadataProvider.ReadTypeDefRow(TokenTypes.TypeDef + 1);
			TypeDefRow nextTypeDefRow = new TypeDefRow();

			for (TokenTypes token = TokenTypes.TypeDef + 1; token <= maxTypeDef; token++)
			{
				TokenTypes maxNextMethod, maxNextField;
				string name = metadataProvider.ReadString(typeDefRow.TypeNameIdx);

				if (token < maxTypeDef)
				{
					nextTypeDefRow = metadataProvider.ReadTypeDefRow(token + 1);
					maxNextField = nextTypeDefRow.FieldList;
					maxNextMethod = nextTypeDefRow.MethodList;

					if (maxNextMethod > maxMethod)
						maxNextMethod = maxMethod + 1;
					if (maxNextField > maxField)
						maxNextField = maxField + 1;
				}
				else
				{
					maxNextMethod = maxMethod + 1;
					maxNextField = maxField + 1;
				}

				// Is this our layout info?
				if (layoutRow.ParentTypeDefIdx == token)
				{
					size = layoutRow.ClassSize;
					packing = layoutRow.PackingSize;

					tokenLayout++;
					if (tokenLayout <= maxLayout)
						layoutRow = metadataProvider.ReadClassLayoutRow(tokenLayout);
				}

				RuntimeType baseType = (typeDefRow.Extends != TokenTypes.TypeDef) ? types[(int)(typeDefRow.Extends & TokenTypes.RowIndexMask)] : null;

				// Create and populate the runtime type
				CilRuntimeType type = new CilRuntimeType(
					GetString(typeDefRow.TypeNameIdx),
					GetString(typeDefRow.TypeNamespaceIdx),
					packing,
					size,
					token,
					baseType,
					typeDefRow
				);

				LoadMethods(type, typeDefRow.MethodList, maxNextMethod, ref methodOffset);
				LoadFields(type, typeDefRow.FieldList, maxNextField, ref fieldOffset);
				types[typeOffset++] = type;

				typeDefRow = nextTypeDefRow;
				packing = 0;
				size = 0;
			}

		}

		/// <summary>
		/// Loads all methods from the given metadata module.
		/// </summary>
		/// <param name="declaringType">The type, which declared the method.</param>
		/// <param name="first">The first method token to load.</param>
		/// <param name="last">The last method token to load (non-inclusive.)</param>
		/// <param name="offset">The offset into the method table to start loading methods From.</param>
		private void LoadMethods(RuntimeType declaringType, TokenTypes first, TokenTypes last, ref int offset)
		{
			if (first >= last)
				return;

			MethodDefRow nextMethodDef = new MethodDefRow();

			TokenTypes maxParam, maxMethod = metadataProvider.GetMaxTokenValue(TokenTypes.MethodDef);
			MethodDefRow methodDef = metadataProvider.ReadMethodDefRow(first);

			for (TokenTypes token = first; token < last; token++)
			{
				if (token < maxMethod)
				{
					nextMethodDef = metadataProvider.ReadMethodDefRow(token + 1);
					maxParam = nextMethodDef.ParamList;
				}
				else
				{
					maxParam = metadataProvider.GetMaxTokenValue(TokenTypes.Param) + 1;
				}

				Debug.Assert(offset < methods.Length, @"Invalid method index.");

				CilRuntimeMethod method = new CilRuntimeMethod(GetString(methodDef.NameStringIdx), GetMethodSignature(methodDef.SignatureBlobIdx), (TokenTypes)offset, declaringType, methodDef);

				declaringType.Methods.Add(method);

				methods[offset++] = method;
				methodDef = nextMethodDef;
			}

		}

		/// <summary>
		/// Loads all parameters from the given metadata module.
		/// </summary>
		private void LoadParameters()
		{
			TokenTypes maxParam = metadataProvider.GetMaxTokenValue(TokenTypes.Param);
			TokenTypes token = TokenTypes.Param + 1;

			int offset = 0;

			while (token <= maxParam)
			{
				ParamRow paramDef = metadataProvider.ReadParamRow(token++);
				parameters[offset++] = new RuntimeParameter(metadataProvider, paramDef);
			}
		}

		/// <summary>
		/// Loads all fields defined in the module.
		/// </summary>
		/// <param name="declaringType">The type, which declared the method.</param>
		/// <param name="first">The first field token to load.</param>
		/// <param name="last">The last field token to load (non-inclusive.)</param>
		/// <param name="offset">The offset in the fields array.</param>
		private void LoadFields(RuntimeType declaringType, TokenTypes first, TokenTypes last, ref int offset)
		{
			TokenTypes maxRVA = metadataProvider.GetMaxTokenValue(TokenTypes.FieldRVA);
			TokenTypes maxLayout = metadataProvider.GetMaxTokenValue(TokenTypes.FieldLayout);
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
				// Read the stackFrameIndex
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
					GetString(fieldRow.NameStringIdx),
					GetFieldSignature(fieldRow.SignatureBlobIdx),
					layout,
					rva,
					declaringType,
					fieldRow
				);

				declaringType.Fields.Add(field);
				fields[offset++] = field;
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
			TokenTypes maxToken = metadataProvider.GetMaxTokenValue(TokenTypes.InterfaceImpl);
			for (TokenTypes token = TokenTypes.InterfaceImpl + 1; token <= maxToken; token++)
			{
				InterfaceImplRow row = metadataProvider.ReadInterfaceImplRow(token);

				RuntimeType interfaceType = types[(int)(row.ClassTableIdx & TokenTypes.RowIndexMask)];
				RuntimeType declaringType = types[(int)(row.InterfaceTableIdx & TokenTypes.RowIndexMask)];

				declaringType.Interfaces.Add(interfaceType);
			}

		}

		/// <summary>
		/// Loads the interfaces.
		/// </summary>
		protected void LoadMemberReferences()
		{
			TokenTypes maxToken = metadataProvider.GetMaxTokenValue(TokenTypes.MemberRef);
			for (TokenTypes token = TokenTypes.MemberRef + 1; token <= maxToken; token++)
			{
				MemberRefRow row = metadataProvider.ReadMemberRefRow(token);
				string name = GetString(row.NameStringIdx);

				RuntimeType ownerType = null;

				switch (row.ClassTableIdx & TokenTypes.TableMask)
				{
					case TokenTypes.TypeDef:
						ownerType = types[(int)row.ClassTableIdx];
						break;

					case TokenTypes.TypeRef:
						//TODO
						//ownerType = ((IModuleTypeSystem)this).GetType(row.ClassTableIdx);
						//break;
						continue;

					case TokenTypes.TypeSpec:
						//TODO
						//ownerType = this.ResolveTypeSpec(row.ClassTableIdx);
						break;

					default:
						throw new NotSupportedException(String.Format(@"LoadMemberReferences() does not support token table {0}", row.ClassTableIdx & TokenTypes.TableMask));
				}

				//MethodSignature signature = new MethodSignature(metadataProvider, row.SignatureBlobIdx);
				//FieldSignature signature = new FieldSignature(metadataProvider, row.SignatureBlobIdx);

				if (ownerType == null)
					throw new InvalidOperationException(String.Format(@"Failed to retrieve owner type for Token {0:x} (Table {1})", row.ClassTableIdx, row.ClassTableIdx & TokenTypes.TableMask));

				RuntimeMember runtimeMember = null;

				foreach (RuntimeField field in ownerType.Fields)
				{
					if (field.Name == name)
					{
						runtimeMember = field;
						break;
					}
				}

				if (runtimeMember == null)
					throw new InvalidOperationException(String.Format(@"Failed to locate field {0}.{1}", ownerType.FullName, name));

				memberRef[(int)token] = runtimeMember;
			}
		}

		/// <summary>
		/// Loads the interfaces.
		/// </summary>
		protected void LoadTypeReferences()
		{
			TokenTypes maxToken = metadataProvider.GetMaxTokenValue(TokenTypes.TypeRef);
			for (TokenTypes token = TokenTypes.TypeRef + 1; token <= maxToken; token++)
			{
				RuntimeType runtimeType = null;

				TypeRefRow row = metadataProvider.ReadTypeRefRow(token);
				string typeName = GetString(row.TypeNameIdx);
				string typenamespace = GetString(row.TypeNamespaceIdx);

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
							ITypeModule module = TypeSystem.ResolveModuleReference(assemblyName);
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
							ITypeModule module = TypeSystem.ResolveModuleReference(assemblyName);
							runtimeType = module.GetType(typeNamespace, typeName);

							if (runtimeType == null)
								throw new TypeLoadException("Could not find type: " + typeNamespace + Type.Delimiter + typeName);

							break;
						}

					default:
						throw new NotImplementedException();
				}

				typeRef[(int)token] = runtimeType;
			}
		}

		#endregion

		#region ITypeLoader interface

		/// <summary>
		/// Gets the metadata module.
		/// </summary>
		/// <value>The metadata module.</value>
		IMetadataModule ITypeModule.MetadataModule { get { return metadataModule; } }

		/// <summary>
		/// Gets all types from module.
		/// </summary>
		/// <returns></returns>
		IEnumerable<RuntimeType> ITypeModule.GetAllTypes()
		{
			foreach (RuntimeType type in types)
				yield return type;

			//foreach (RuntimeType type in typeSpecs)
			//    if (type != null)
			//        yield return type;
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
		/// Retrieves the runtime type for a given metadata token.
		/// </summary>
		/// <param name="token">The token of the type to load. This can represent a typeref, typedef or typespec token.</param>
		/// <returns>The runtime type of the specified token.</returns>
		RuntimeType ITypeModule.GetType(TokenTypes token)
		{
			switch (token & TokenTypes.TableMask)
			{

				case TokenTypes.TypeDef:
					{
						int typeDefRowIndex = (int)(token & TokenTypes.RowIndexMask);

						if (typeDefRowIndex == 0)
							return null;

						return types[typeDefRowIndex - 1];
					}

				//TODO
				//case TokenTypes.TypeRef:
				//    return ResolveTypeRef(token);

				//TODO
				//case TokenTypes.TypeSpec:
				//    return ResolveTypeSpec(token);

				default:
					throw new ArgumentException(@"Not a type token.", @"token");
			}
		}

		#endregion

	}
}
