/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Alex Lyman <mail.alex.lyman@gmail.com>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata.Runtime;

namespace Mosa.Runtime.Vm
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class DefaultModuleTypeSystem : IModuleTypeSystem
	{
		#region Data members

		private ITypeSystem typeSystem;

		/// <summary>
		/// Holds the metadata module
		/// </summary>
		private IMetadataModule metadataModule;

		/// <summary>
		/// Holds the metadata provider
		/// </summary>
		private IMetadataProvider metadata;

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

		#endregion // Data members

		/// <summary>
		/// Holds the runtype system
		/// </summary>
		/// <value></value>
		ITypeSystem IModuleTypeSystem.TypeSystem { get { return typeSystem; } }

		/// <summary>
		/// Holds the metadata module
		/// </summary>
		IMetadataModule IModuleTypeSystem.MetadataModule { get { return metadataModule; } }

		/// <summary>
		/// Array of loaded runtime type descriptors.
		/// </summary>
		RuntimeType[] IModuleTypeSystem.Types { get { return types; } }

		/// <summary>
		/// Holds all loaded method definitions.
		/// </summary>
		RuntimeMethod[] IModuleTypeSystem.Methods { get { return methods; } }

		/// <summary>
		/// Holds all parameter information elements.
		/// </summary>
		RuntimeParameter[] IModuleTypeSystem.Parameters { get { return parameters; } }

		/// <summary>
		/// Holds all loaded _stackFrameIndex definitions.
		/// </summary>
		RuntimeField[] IModuleTypeSystem.Fields { get { return fields; } }

		/// <summary>
		/// Array of loaded runtime typespec descriptors.
		/// </summary>
		RuntimeType[] IModuleTypeSystem.TypeSpecs { get { return typeSpecs; } }

		#region Construction

		/// <summary>
		/// Initializes static data members of the type loader.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="metadataModule">The metadata module.</param>
		public DefaultModuleTypeSystem(ITypeSystem typeSystem, IMetadataModule metadataModule)
		{
			Debug.Assert(typeSystem != null);
			Debug.Assert(metadataModule != null);
			Debug.Assert(metadataModule.Metadata != null);

			this.typeSystem = typeSystem;
			this.metadataModule = metadataModule;
			this.metadata = metadataModule.Metadata;

			methods = new RuntimeMethod[GetTableRows(TokenTypes.MethodDef)];
			fields = new RuntimeField[GetTableRows(TokenTypes.Field)];
			types = new RuntimeType[GetTableRows(TokenTypes.TypeDef)];
			parameters = new RuntimeParameter[GetTableRows(TokenTypes.Param)];

			typeSpecs = new RuntimeType[GetTableRows(TokenTypes.TypeSpec)];
			methodSpecs = new RuntimeMethod[GetTableRows(TokenTypes.MethodSpec)];

			// Load all types from the assembly into the type array
			LoadTypes();
			LoadGenerics();
			LoadTypeSpecs();
			LoadParameters();
			LoadCustomAttributes();
		}

		/// <summary>
		/// Initializes static data members of the type loader.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		public DefaultModuleTypeSystem(ITypeSystem typeSystem)
		{
			Debug.Assert(typeSystem != null);

			this.typeSystem = typeSystem;
			this.metadataModule = null;
			this.metadata = null;

			methods = new RuntimeMethod[0];
			fields = new RuntimeField[0];
			types = new RuntimeType[0];
			parameters = new RuntimeParameter[0];

			typeSpecs = new RuntimeType[0];
			methodSpecs = new RuntimeMethod[0];
		}

		#endregion // Construction

		#region ITypeSystem Members

		/// <summary>
		/// Gets all types from module.
		/// </summary>
		/// <returns></returns>
		IEnumerable<RuntimeType> IModuleTypeSystem.GetAllTypes()
		{
			foreach (RuntimeType type in types)
				yield return type;

			foreach (RuntimeType type in typeSpecs)
				if (type != null)
					yield return type;
		}

		/// <summary>
		/// Retrieves the runtime type for a given metadata token.
		/// </summary>
		/// <param name="token">The token of the type to load. This can represent a typeref, typedef or typespec token.</param>
		/// <returns>The runtime type of the specified token.</returns>
		RuntimeType IModuleTypeSystem.GetType(TokenTypes token)
		{
			TokenTypes table = (TokenTypes.TableMask & token);

			if (TokenTypes.TypeRef == table)
			{
				return ResolveTypeRef(token);
			}
			else
			{
				int row = (int)(token & TokenTypes.RowIndexMask);

				if (row == 0)
					return null;

				if (table == TokenTypes.TypeDef)
				{
					return types[row - 1];
				}
				else if (table == TokenTypes.TypeSpec)
				{
					return ResolveTypeSpec(token);
				}
				else
				{
					throw new ArgumentException(@"Not a type token.", @"token");
				}
			}
		}

		/// <summary>
		/// Retrieves the runtime type for a given type name.
		/// </summary>
		/// <param name="typeName">The name of the type to locate.</param>
		/// <returns>
		/// The located <see cref="RuntimeType"/> or null.
		/// </returns>
		RuntimeType IModuleTypeSystem.GetType(string typeName)
		{
			string[] names = typeName.Split(',');
			typeName = names[0];

			int lastDot = typeName.LastIndexOf('.');

			string nameSpace, name;
			if (lastDot == -1)
			{
				nameSpace = String.Empty;
				name = typeName;
			}
			else
			{
				nameSpace = typeName.Substring(0, lastDot);
				name = typeName.Substring(lastDot + 1);
			}

			RuntimeType result = FindType(name, nameSpace);

			return result;
		}

		RuntimeType IModuleTypeSystem.GetType(string nameSpace, string typeName)
		{
			return FindType(typeName, nameSpace);
		}

		/// <summary>
		/// Adds the internal compiler defined type to the type system
		/// </summary>
		/// <param name="type">The type.</param>
		void IModuleTypeSystem.AddInternalType(RuntimeType type)
		{
			Array.Resize<RuntimeType>(ref types, types.Length + 1);
			types[types.Length - 1] = type;
		}

		/// <summary>
		/// Retrieves the field definition identified by the given token in the scope.
		/// </summary>
		/// <param name="token">The token of the field to retrieve.</param>
		/// <returns></returns>
		RuntimeField IModuleTypeSystem.GetField(TokenTypes token)
		{
			if (TokenTypes.Field != (TokenTypes.TableMask & token) && TokenTypes.MemberRef != (TokenTypes.TableMask & token))
				throw new ArgumentException(@"Invalid field token.");

			RuntimeField result;

			if (TokenTypes.MemberRef == (TokenTypes.TableMask & token))
			{
				result = GetFieldForMemberReference(token);
			}
			else
			{
				int fieldIndex = (int)(token & TokenTypes.RowIndexMask) - 1;
				result = fields[fieldIndex];
			}

			return result;
		}

		/// <summary>
		/// Resolves the type of the signature.
		/// </summary>
		/// <param name="sigType">Type of the signature.</param>
		/// <returns></returns>
		RuntimeType IModuleTypeSystem.ResolveSignatureType(SigType sigType)
		{
			switch (sigType.Type)
			{
				case CilElementType.Class:
					TypeSigType typeSigType = (TypeSigType)sigType;
					return ((IModuleTypeSystem)this).GetType(typeSigType.Token);

				case CilElementType.ValueType:
					goto case CilElementType.Class;

				case CilElementType.GenericInst:
					GenericInstSigType genericSigType = (GenericInstSigType)sigType;
					RuntimeType baseType = ((IModuleTypeSystem)this).GetType(genericSigType.BaseType.Token);
					return new CilGenericType(this, baseType, genericSigType);

				default:
					throw new NotSupportedException(String.Format(@"ResolveSignatureType does not support CilElementType.{0}", sigType.Type));
			}
		}

		/// <summary>
		/// Retrieves the method definition identified by the given token in the scope.
		/// </summary>
		/// <param name="token">The token of the method to retrieve.</param>
		/// <returns></returns>
		RuntimeMethod IModuleTypeSystem.GetMethod(TokenTypes token)
		{
			switch (TokenTypes.TableMask & token)
			{
				case TokenTypes.MethodDef:
					return methods[(int)(TokenTypes.RowIndexMask & token) - 1];

				case TokenTypes.MemberRef:
					MemberRefRow row = metadata.ReadMemberRefRow(token);
					string nameString = metadata.ReadString(row.NameStringIdx);
					RuntimeType type = ((IModuleTypeSystem)this).GetType(row.ClassTableIdx);

					MethodSignature sig = (MethodSignature)Signature.FromMemberRefSignatureToken(metadata, row.SignatureBlobIdx);

					CilGenericType genericType = type as CilGenericType;
					if (genericType != null)
					{
						sig.ApplyGenericType(genericType.GenericArguments);
					}

					foreach (RuntimeMethod method in type.Methods)
					{
						if (method.Name != nameString)
							continue;
						if (!method.Signature.Matches(sig))
							continue;

						return method;
					}

					throw new MissingMethodException(type.Name, nameString);

				case TokenTypes.MethodSpec:
					return this.DecodeMethodSpec(token);

				default:
					throw new NotSupportedException(@"Can't get method for token " + token.ToString("x"));
			}
		}

		#endregion // ITypeSystem Members

		/// <summary>
		/// Finds the type.
		/// </summary>
		/// <param name="typeName">The name of the type.</param>
		/// <param name="nameSpace">The namespace of the type.</param>
		/// <returns>The <see cref="RuntimeType"/> or null.</returns>
		private RuntimeType FindType(string typeName, string nameSpace)
		{
			foreach (RuntimeType type in types)
			{
				if (type != null && typeName == type.Name && nameSpace == type.Namespace)
				{
					return type;
				}
			}
			return null;
		}

		#region Internals

		/// <summary>
		/// Decodes the method specification
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		private RuntimeMethod DecodeMethodSpec(TokenTypes token)
		{
			MethodSpecRow methodSpec = metadata.ReadMethodSpecRow(token);

			CilRuntimeMethod genericMethod = (CilRuntimeMethod)((IModuleTypeSystem)this).GetMethod(methodSpec.MethodTableIdx);

			MethodSpecSignature specSignature = new MethodSpecSignature(metadata, methodSpec.InstantiationBlobIdx);

			MethodSignature signature = new MethodSignature(genericMethod.MetadataModule.Metadata, genericMethod.Signature.Token);

			return new CilGenericMethod(this, genericMethod, signature, genericMethod.DeclaringType);
		}

		/// <summary>
		/// Gets the table rows.
		/// </summary>
		/// <param name="tokenType">Type of the token.</param>
		/// <returns></returns>
		private int GetTableRows(TokenTypes tokenType)
		{
			return (int)(TokenTypes.RowIndexMask & metadata.GetMaxTokenValue(tokenType));
		}

		/// <summary>
		/// Loads all types from the given metadata module.
		/// </summary>
		private void LoadTypes()
		{
			TokenTypes maxTypeDef, maxField, maxMethod, maxLayout, tokenLayout = TokenTypes.ClassLayout + 1;
			TypeDefRow typeDefRow, nextTypeDefRow = new TypeDefRow();
			ClassLayoutRow layoutRow = new ClassLayoutRow();
			int size = 0x0, packing = 0;
			int typeOffset = 0;
			int methodOffset = 0;
			int fieldOffset = 0;
			RuntimeType rt;

			maxTypeDef = metadata.GetMaxTokenValue(TokenTypes.TypeDef);
			maxLayout = metadata.GetMaxTokenValue(TokenTypes.ClassLayout);
			maxMethod = metadata.GetMaxTokenValue(TokenTypes.MethodDef);
			maxField = metadata.GetMaxTokenValue(TokenTypes.Field);

			if (TokenTypes.ClassLayout < maxLayout)
				layoutRow = metadata.ReadClassLayoutRow(tokenLayout);

			TokenTypes token = TokenTypes.TypeDef + 1;
			typeDefRow = metadata.ReadTypeDefRow(token);

			for (; token <= maxTypeDef; token++)
			{
				TokenTypes maxNextMethod, maxNextField;
				string name = metadata.ReadString(typeDefRow.TypeNameIdx);

				//Debug.Write(((uint)token).ToString("X") + ": ");
				//Debug.Write(typeDefRow.TypeNameIdx.ToString("X") + ": ");
				//Debug.Write(metadata.ReadString(typeDefRow.TypeNameIdx));

				if (token < maxTypeDef)
				{
					nextTypeDefRow = metadata.ReadTypeDefRow(token + 1);
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

					//Debug.Write(" [Size: " + size.ToString() + "]");

					tokenLayout++;
					if (tokenLayout <= maxLayout)
						layoutRow = metadata.ReadClassLayoutRow(tokenLayout);
				}
				//Debug.WriteLine(string.Empty);

				// Create and populate the runtime type
				rt = new CilRuntimeType(this, token, typeDefRow, maxNextField, maxNextMethod, packing, size);
				LoadMethods(rt, typeDefRow.MethodList, maxNextMethod, ref methodOffset);
				LoadFields(rt, typeDefRow.FieldList, maxNextField, ref fieldOffset);
				types[typeOffset++] = rt;

				packing = size = 0;
				typeDefRow = nextTypeDefRow;
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

			MethodDefRow methodDef, nextMethodDef = new MethodDefRow();
			TokenTypes maxParam, maxMethod = metadata.GetMaxTokenValue(TokenTypes.MethodDef);

			methodDef = metadata.ReadMethodDefRow(first);
			for (TokenTypes token = first; token < last; token++)
			{
				if (token < maxMethod)
				{
					nextMethodDef = metadata.ReadMethodDefRow(token + 1);
					maxParam = nextMethodDef.ParamList;
				}
				else
				{
					maxParam = metadata.GetMaxTokenValue(TokenTypes.Param) + 1;
				}

				Debug.Assert(offset < methods.Length, @"Invalid method index.");
				methods[offset++] = new CilRuntimeMethod(this, offset, methodDef, maxParam, declaringType);

				//Debug.Write("-> " + ((uint)token).ToString("X") + ": ");
				//Debug.Write(methodDef.NameStringIdx.ToString("X") + ": ");
				//Debug.Write(metadata.ReadString(methodDef.NameStringIdx));
				//Debug.WriteLine(" -  " + methodDef.SignatureBlobIdx.ToString());

				methodDef = nextMethodDef;
			}

		}

		/// <summary>
		/// Loads all parameters from the given metadata module.
		/// </summary>
		private void LoadParameters()
		{
			TokenTypes maxParam = metadata.GetMaxTokenValue(TokenTypes.Param);
			TokenTypes token = TokenTypes.Param + 1;

			int offset = 0;

			while (token <= maxParam)
			{
				ParamRow paramDef = metadata.ReadParamRow(token++);
				parameters[offset++] = new RuntimeParameter(metadataModule, paramDef);
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
			TokenTypes maxRVA = metadata.GetMaxTokenValue(TokenTypes.FieldRVA);
			TokenTypes maxLayout = metadata.GetMaxTokenValue(TokenTypes.FieldLayout);
			TokenTypes tokenRva = TokenTypes.FieldRVA + 1;
			TokenTypes tokenLayout = TokenTypes.FieldLayout + 1;

			FieldRVARow fieldRVA = new FieldRVARow();
			FieldLayoutRow fieldLayout = new FieldLayoutRow();

			if (TokenTypes.FieldRVA < maxRVA)
				fieldRVA = metadata.ReadFieldRVARow(tokenRva);

			if (TokenTypes.FieldLayout < maxLayout)
				fieldLayout = metadata.ReadFieldLayoutRow(tokenLayout);

			for (TokenTypes token = first; token < last; token++)
			{
				// Read the stackFrameIndex
				FieldRow field = metadata.ReadFieldRow(token);
				uint rva = 0;
				uint layout = 0;

				// Static fields have an optional RVA, non-static may have a layout assigned
				if ((field.Flags & FieldAttributes.HasFieldRVA) == FieldAttributes.HasFieldRVA)
				{
					// Move to the RVA of this field
					while (fieldRVA.FieldTableIdx < token && tokenRva <= maxRVA)
						fieldRVA = metadata.ReadFieldRVARow(tokenRva++);

					// Does this field have an RVA?
					if (token == fieldRVA.FieldTableIdx && tokenRva <= maxRVA)
					{
						rva = fieldRVA.Rva;
						tokenRva++;
						if (tokenRva < maxRVA)
						{
							fieldRVA = metadata.ReadFieldRVARow(tokenRva);
						}
					}
				}

				if ((field.Flags & FieldAttributes.HasDefault) == FieldAttributes.HasDefault)
				{
					// FIXME: Has a default value.
					//Debug.Assert(false);
				}

				// Layout only exists for non-static fields
				if ((field.Flags & FieldAttributes.Static) != FieldAttributes.Static)
				{
					// Move to the layout of this field
					while (fieldLayout.Field < token && tokenLayout <= maxLayout)
						fieldLayout = metadata.ReadFieldLayoutRow(tokenLayout++);

					// Does this field have layout?
					if (token == fieldLayout.Field && tokenLayout <= maxLayout)
					{
						layout = fieldLayout.Offset;
						tokenLayout++;
						if (tokenLayout < maxLayout)
						{
							fieldLayout = metadata.ReadFieldLayoutRow(tokenLayout);
						}
					}
				}

				// Load the field metadata
				fields[offset++] = new CilRuntimeField(this, field, layout, rva, declaringType);
			}

			/* FIXME:
			 * Load FieldMarshal tables
			 * as needed afterwards. All Generics have been loaded, fields can receive
			 * their signature in the load method above.
			 */
		}

		/// <summary>
		/// Loads all generic parameter definitions of generic methods and types.
		/// </summary>
		private void LoadGenerics()
		{
			//Debug.WriteLine("Loading generic parameters... {0}", module.Name);

			TokenTypes maxGeneric = metadata.GetMaxTokenValue(TokenTypes.GenericParam);
			TokenTypes token = TokenTypes.GenericParam + 1;

			List<GenericParamRow> gprs = new List<GenericParamRow>();

			//Debug.WriteLine("\tFrom {0:x} to {1:x}", token, maxGeneric);
			TokenTypes owner = 0;

			while (token <= maxGeneric)
			{
				GenericParamRow gpr = metadata.ReadGenericParamRow(token++);

				// Do we need to commit generic parameters?
				if (owner < gpr.OwnerTableIdx)
				{
					// Yes, commit them to the last type
					if (owner != 0 && gprs.Count != 0)
					{
						SetGenericParameters(gprs, owner);
						gprs.Clear();
					}

					owner = gpr.OwnerTableIdx;
				}

				gprs.Add(gpr);
			}

			// Set the generic parameters of the last type, if we have them
			if (gprs.Count != 0)
			{
				SetGenericParameters(gprs, owner);
			}
		}

		/// <summary>
		/// Sets generic parameters on a method or type.
		/// </summary>
		/// <param name="gprs">The list of generic parameters to set.</param>
		/// <param name="owner">The owner object token.</param>
		private void SetGenericParameters(List<GenericParamRow> gprs, TokenTypes owner)
		{
			//Debug.WriteLine("Setting generic parameters on owner {0:x}...", owner);
			switch (owner & TokenTypes.TableMask)
			{
				case TokenTypes.TypeDef:
					types[(int)(TokenTypes.RowIndexMask & owner) - 1].SetGenericParameter(gprs);
					break;

				case TokenTypes.MethodDef:
					methods[(int)(TokenTypes.RowIndexMask & owner) - 1].SetGenericParameter(gprs);
					break;

				default:
					// Unknown owner table type
					throw new InvalidProgramException(@"Invalid owner table of generic parameter token.");
			}
		}

		/// <summary>
		/// Loads all custom attributes from the assembly.
		/// </summary>
		private void LoadCustomAttributes()
		{
			TokenTypes maxAttrs = metadata.GetMaxTokenValue(TokenTypes.CustomAttribute);
			TokenTypes owner = 0;

			List<CustomAttributeRow> attributes = new List<CustomAttributeRow>();

			for (TokenTypes token = TokenTypes.CustomAttribute + 1; token <= maxAttrs; token++)
			{
				CustomAttributeRow car = metadata.ReadCustomAttributeRow(token);

				// Do we need to commit generic parameters?
				if (owner != car.ParentTableIdx)
				{
					// Yes, commit them to the last type
					if (0 != owner && 0 != attributes.Count)
					{
						SetAttributes(owner, attributes);
						attributes.Clear();
					}

					owner = car.ParentTableIdx;
				}

				// Save this attribute
				attributes.Add(car);
			}

			// Set the generic parameters of the last type, if we have them
			if (attributes.Count != 0)
			{
				SetAttributes(owner, attributes);
			}
		}

		/// <summary>
		/// Sets the attributes.
		/// </summary>
		/// <param name="owner">The owner.</param>
		/// <param name="attributes">The attributes.</param>
		private void SetAttributes(TokenTypes owner, List<CustomAttributeRow> attributes)
		{
			// Convert the custom attribute rows to RuntimeAttribute instances
			RuntimeAttribute[] ra = new RuntimeAttribute[attributes.Count];
			for (int i = 0; i < attributes.Count; i++)
				ra[i] = new RuntimeAttribute(this, attributes[i]);

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
					RuntimeType type = ((IModuleTypeSystem)this).GetType(owner);
					type.SetAttributes(ra);
					break;

				case TokenTypes.MethodDef:
					// AttributeTargets.Constructor
					// AttributeTargets.Method
					RuntimeMethod method = ((IModuleTypeSystem)this).GetMethod(owner);
					method.SetAttributes(ra);
					break;

				case TokenTypes.Event:
					// AttributeTargets.Event
					break;

				case TokenTypes.Field:
					// AttributeTargets.Field
					RuntimeField field = ((IModuleTypeSystem)this).GetField(owner);
					field.SetAttributes(ra);
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
			}
		}

		/// <summary>
		/// Resolves the type ref.
		/// </summary>
		/// <param name="tokenTypes">The token types.</param>
		/// <returns></returns>
		private RuntimeType ResolveTypeRef(TokenTypes tokenTypes)
		{
			// MR, 2008-08-26, patch by Alex Lyman (thanks!)
			TypeRefRow row = metadata.ReadTypeRefRow(tokenTypes);

			switch (row.ResolutionScopeIdx & TokenTypes.TableMask)
			{
				case TokenTypes.Module:
				case TokenTypes.ModuleRef:
				case TokenTypes.TypeRef:
					throw new NotImplementedException();
				case TokenTypes.AssemblyRef:
					string typeName = metadata.ReadString(row.TypeNameIdx);
					string typeNamespace = metadata.ReadString(row.TypeNamespaceIdx);

					AssemblyRefRow asmRefRow = metadata.ReadAssemblyRefRow(row.ResolutionScopeIdx);
					IModuleTypeSystem module = typeSystem.ResolveModuleReference(metadata.ReadString(asmRefRow.NameIdx));
					RuntimeType type = module.GetType(typeNamespace, typeName);

					if (type != null)
						return type;

					throw new TypeLoadException("Could not find type: " + typeNamespace + Type.Delimiter + typeName);

				default:
					throw new NotSupportedException();
			}
		}

		private RuntimeField GetFieldForMemberReference(TokenTypes token)
		{
			MemberRefRow row = metadata.ReadMemberRefRow(token);

			RuntimeType ownerType;

			switch (row.ClassTableIdx & TokenTypes.TableMask)
			{
				case TokenTypes.TypeSpec:
					ownerType = this.ResolveTypeSpec(row.ClassTableIdx);
					break;

				case TokenTypes.TypeDef:
					goto case TokenTypes.TypeRef;

				case TokenTypes.TypeRef:
					ownerType = ((IModuleTypeSystem)this).GetType(row.ClassTableIdx);
					break;

				default:
					throw new NotSupportedException(String.Format(@"DefaultTypeSystem.GetFieldForMemberReference does not support Token table {0}", row.ClassTableIdx & TokenTypes.TableMask));
			}

			if (ownerType == null)
				throw new InvalidOperationException(String.Format(@"Failed to retrieve owner type for Token {0:x} (Table {1}) in {2}", row.ClassTableIdx, row.ClassTableIdx & TokenTypes.TableMask, metadataModule.Name));

			string fieldName = metadata.ReadString(row.NameStringIdx);

			foreach (RuntimeField field in ownerType.Fields)
			{
				if (field.Name == fieldName)
				{
					return field;
				}
			}

			throw new InvalidOperationException(String.Format(@"Failed to locate field {0}.{1}", ownerType.FullName, fieldName));
		}

		/// <summary>
		/// Loads all parameters from the given metadata module.
		/// </summary>
		private void LoadTypeSpecs()
		{
			TokenTypes maxParam = metadata.GetMaxTokenValue(TokenTypes.TypeSpec);
			TokenTypes token = TokenTypes.TypeSpec + 1;

			while (token <= maxParam)
			{
				ResolveTypeSpec(token++);
			}
		}

		/// <summary>
		/// Resolves the type specification
		/// </summary>
		/// <param name="typeSpecToken">The type spec token.</param>
		/// <returns></returns>
		private RuntimeType ResolveTypeSpec(TokenTypes typeSpecToken)
		{
			int typeSpecIndex = (int)(typeSpecToken & TokenTypes.RowIndexMask) - 1;

			if (typeSpecs[typeSpecIndex] == null)
			{
				TypeSpecRow typeSpec = metadata.ReadTypeSpecRow(typeSpecToken);

				TypeSpecSignature signature = new TypeSpecSignature(metadata, typeSpec.SignatureBlobIdx);

				if (signature.Type.Type == CilElementType.Class || signature.Type.Type == CilElementType.ValueType || signature.Type.Type == CilElementType.GenericInst)
				{
					GenericInstSigType genericSig = signature.Type as GenericInstSigType;

					if (genericSig != null)
					{
						RuntimeType genericType = ((IModuleTypeSystem)this).ResolveSignatureType(signature.Type);

						typeSpecs[typeSpecIndex] = genericType;
					}
				}
			}

			return typeSpecs[typeSpecIndex];
		}

		public override string ToString()
		{
			if (metadataModule != null)
				return metadataModule.Name;
			else
				return "<internal>";
		}

		#endregion // Internals
	}
}
