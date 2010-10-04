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
		private RuntimeType[] _types;

		/// <summary>
		/// Holds all loaded method definitions.
		/// </summary>
		private RuntimeMethod[] _methods;

		/// <summary>
		/// Holds all parameter information elements.
		/// </summary>
		private RuntimeParameter[] _parameters;

		/// <summary>
		/// Array of loaded runtime type descriptors.
		/// </summary>
		private RuntimeType[] _typespecs;

		/// <summary>
		/// Holds all loaded method definitions.
		/// </summary>
		private RuntimeMethod[] _methodspecs;

		/// <summary>
		/// Holds all loaded _stackFrameIndex definitions.
		/// </summary>
		private RuntimeField[] _fields;

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
		RuntimeType[] IModuleTypeSystem.Types { get { return _types; } }

		/// <summary>
		/// Holds all loaded method definitions.
		/// </summary>
		RuntimeMethod[] IModuleTypeSystem.Methods { get { return _methods; } }

		/// <summary>
		/// Holds all parameter information elements.
		/// </summary>
		RuntimeParameter[] IModuleTypeSystem.Parameters { get { return _parameters; } }

		/// <summary>
		/// Holds all loaded _stackFrameIndex definitions.
		/// </summary>
		RuntimeField[] IModuleTypeSystem.Fields { get { return _fields; } }

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

			_methods = new RuntimeMethod[GetTableRows(TokenTypes.MethodDef)];
			_fields = new RuntimeField[GetTableRows(TokenTypes.Field)];
			_types = new RuntimeType[GetTableRows(TokenTypes.TypeDef)];
			_parameters = new RuntimeParameter[GetTableRows(TokenTypes.Param)];

			_typespecs = new RuntimeType[GetTableRows(TokenTypes.TypeSpec)];
			_methodspecs = new RuntimeMethod[GetTableRows(TokenTypes.MethodSpec)];

			// Load all types from the assembly into the type array
			LoadTypes();
			LoadGenerics();
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

			_methods = new RuntimeMethod[0];
			_fields = new RuntimeField[0];
			_types = new RuntimeType[0];
			_parameters = new RuntimeParameter[0];
		}

		#endregion // Construction

		#region ITypeSystem Members

		/// <summary>
		/// Gets the types from module.
		/// </summary>
		/// <returns></returns>
		ReadOnlyRuntimeTypeListView IModuleTypeSystem.GetTypes()
		{
			return new ReadOnlyRuntimeTypeListView(this, 0, _types.Length);
		}

		/// <summary>
		/// Gets all types from module.
		/// </summary>
		/// <returns></returns>
		IEnumerable<RuntimeType> IModuleTypeSystem.GetAllTypes()
		{
			foreach (RuntimeType type in ((IModuleTypeSystem)this).GetTypes())
				yield return type;
		}

		RuntimeType IModuleTypeSystem.GetType(ISignatureContext context, TokenTypes token)
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
					return this._types[row - 1];
				}
				else if (table == TokenTypes.TypeSpec)
				{
					return ResolveTypeSpec(context, token);
				}
				else
				{
					throw new ArgumentException(@"Not a type token.", @"token");
				}
			}
		}

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
			Array.Resize<RuntimeType>(ref _types, _types.Length + 1);
			_types[_types.Length - 1] = type;
		}

		/// <summary>
		/// Retrieves the stackFrameIndex definition identified by the given token in the scope.
		/// </summary>
		/// <param name="context">The generic parameter resolution context.</param>
		/// <param name="token">The token of the _stackFrameIndex to retrieve.</param>
		/// <returns></returns>
		RuntimeField IModuleTypeSystem.GetField(ISignatureContext context, TokenTypes token)
		{
			if (TokenTypes.Field != (TokenTypes.TableMask & token) && TokenTypes.MemberRef != (TokenTypes.TableMask & token))
				throw new ArgumentException(@"Invalid field token.");

			RuntimeField result;

			if (TokenTypes.MemberRef == (TokenTypes.TableMask & token))
			{
				result = GetFieldForMemberReference(context, token);
			}
			else
			{
				int fieldIndex = (int)(token & TokenTypes.RowIndexMask) - 1;
				result = _fields[fieldIndex];
			}

			return result;
		}

		/// <summary>
		/// Resolves the type of the signature.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="sigType">Type of the signature.</param>
		/// <returns></returns>
		RuntimeType IModuleTypeSystem.ResolveSignatureType(ISignatureContext context, SigType sigType)
		{
			switch (sigType.Type)
			{
				case CilElementType.Class:
					{
						TypeSigType typeSigType = (TypeSigType)sigType;
						return ((IModuleTypeSystem)this).GetType(context, typeSigType.Token);
					}

				case CilElementType.ValueType:
					goto case CilElementType.Class;

				case CilElementType.GenericInst:
					{
						GenericInstSigType genericSigType = (GenericInstSigType)sigType;

						RuntimeType baseType = ((IModuleTypeSystem)this).GetType(context, genericSigType.BaseType.Token);

						return new CilGenericType(this, baseType, genericSigType, context);
					}

				default:
					throw new NotSupportedException(String.Format(@"ResolveSignatureType does not support CilElementType.{0}", sigType.Type));
			}
		}

		/// <summary>
		/// Retrieves the method definition identified by the given token in the scope.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="token">The token of the method to retrieve.</param>
		/// <returns></returns>
		RuntimeMethod IModuleTypeSystem.GetMethod(ISignatureContext context, TokenTypes token)
		{
			switch (TokenTypes.TableMask & token)
			{
				case TokenTypes.MethodDef:
					{
						return _methods[(int)(TokenTypes.RowIndexMask & token) - 1];
					}

				case TokenTypes.MemberRef:
					{
						MemberRefRow row = metadata.ReadMemberRefRow(token);
						string nameString = metadata.ReadString(row.NameStringIdx);
						RuntimeType type = ((IModuleTypeSystem)this).GetType(context, row.ClassTableIdx);

						MethodSignature sig = (MethodSignature)Signature.FromMemberRefSignatureToken(type, metadata, row.SignatureBlobIdx);
						foreach (RuntimeMethod method in type.Methods)
						{
							if (method.Name != nameString)
								continue;
							if (!method.Signature.Matches(sig))
								continue;

							return method;
						}

						throw new MissingMethodException(type.Name, nameString);
					}

				case TokenTypes.MethodSpec:
					return this.DecodeMethodSpec(context, token);

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
			foreach (RuntimeType type in _types)
			{
				if (null != type && nameSpace.Length == type.Namespace.Length && typeName.Length == type.Name.Length && typeName == type.Name && nameSpace == type.Namespace)
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
		/// <param name="context">The context.</param>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		private RuntimeMethod DecodeMethodSpec(ISignatureContext context, TokenTypes token)
		{
			MethodSpecRow methodSpec = metadata.ReadMethodSpecRow(token);

			CilRuntimeMethod genericMethod = (CilRuntimeMethod)((IModuleTypeSystem)this).GetMethod(context, methodSpec.MethodTableIdx);

			MethodSpecSignature specSignature = new MethodSpecSignature(context);
			specSignature.LoadSignature(context, metadata, methodSpec.InstantiationBlobIdx);

			MethodSignature signature = new MethodSignature();
			signature.LoadSignature(specSignature, genericMethod.MetadataModule.Metadata, genericMethod.Signature.Token);

			return new CilGenericMethod(this, genericMethod, signature, specSignature);
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

		private TokenTypes DecodeTypeIndex(byte signature)
		{
			TokenTypes result = TokenTypes.TypeDef;
			if ((signature & 0x3) == 0x00)
				result = TokenTypes.TypeDef;
			else if ((signature & 0x03) == 0x01)
				result = TokenTypes.TypeRef;
			else if ((signature & 0x03) == 0x02)
				result = TokenTypes.TypeSpec;

			result |= (TokenTypes)(signature >> 2);
			return result;
		}

		private CilElementType GetElementType(byte[] blob, int index)
		{
			return (CilElementType)(blob[4 + index]);
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

					// Crazy!
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
				_types[typeOffset++] = rt;

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

				Debug.Assert(offset < _methods.Length, @"Invalid method index.");
				_methods[offset++] = new CilRuntimeMethod(this, offset, methodDef, maxParam, declaringType);

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
				_parameters[offset++] = new RuntimeParameter(metadataModule, paramDef);
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
				ulong rva = 0;
				ulong layout = 0;

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
				_fields[offset++] = new CilRuntimeField(this, ref field, layout, rva, declaringType);
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
					if (0 != owner && 0 != gprs.Count)
					{
						SetGenericParameters(gprs, owner);
						gprs.Clear();
					}

					owner = gpr.OwnerTableIdx;
				}

				gprs.Add(gpr);
			}

			// Set the generic parameters of the last type, if we have them
			if (0 != gprs.Count)
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
					_types[(int)(TokenTypes.RowIndexMask & owner) - 1].SetGenericParameter(gprs);
					break;

				case TokenTypes.MethodDef:
					_methods[(int)(TokenTypes.RowIndexMask & owner) - 1].SetGenericParameter(gprs);
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
					RuntimeType type = ((IModuleTypeSystem)this).GetType(DefaultSignatureContext.Instance, owner);
					type.SetAttributes(ra);
					break;

				case TokenTypes.MethodDef:
					// AttributeTargets.Constructor
					// AttributeTargets.Method
					RuntimeMethod method = ((IModuleTypeSystem)this).GetMethod(DefaultSignatureContext.Instance, owner);
					method.SetAttributes(ra);
					break;

				case TokenTypes.Event:
					// AttributeTargets.Event
					break;

				case TokenTypes.Field:
					// AttributeTargets.Field
                    RuntimeField field = ((IModuleTypeSystem)this).GetField(DefaultSignatureContext.Instance, owner);
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

		private RuntimeField GetFieldForMemberReference(ISignatureContext context, TokenTypes token)
		{
			MemberRefRow row = metadata.ReadMemberRefRow(token);

			RuntimeType ownerType;

			switch (row.ClassTableIdx & TokenTypes.TableMask)
			{
				case TokenTypes.TypeSpec:
					ownerType = this.ResolveTypeSpec(context, row.ClassTableIdx);
					break;

				case TokenTypes.TypeDef: goto case TokenTypes.TypeRef;

				case TokenTypes.TypeRef:
					ownerType = ((IModuleTypeSystem)this).GetType(context, row.ClassTableIdx);
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
		/// Resolves the type specification
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSpecToken">The type spec token.</param>
		/// <returns></returns>
		private RuntimeType ResolveTypeSpec(ISignatureContext context, TokenTypes typeSpecToken)
		{
			int typeSpecIndex = (int)(typeSpecToken & TokenTypes.RowIndexMask) - 1;

			if (_typespecs[typeSpecIndex] == null)
			{
				TypeSpecRow typeSpec = metadata.ReadTypeSpecRow(typeSpecToken);

				TypeSpecSignature signature = new TypeSpecSignature();
				signature.LoadSignature(context, metadata, typeSpec.SignatureBlobIdx);

				_typespecs[typeSpecIndex] = ((IModuleTypeSystem)this).ResolveSignatureType(context, signature.Type);
			}

			return _typespecs[typeSpecIndex];
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
