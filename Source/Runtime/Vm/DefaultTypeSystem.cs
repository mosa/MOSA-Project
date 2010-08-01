/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Alex Lyman (<?>)
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
	/// A default implementation of a type system for the Mosa runtime.
	/// </summary>
	public class DefaultTypeSystem : ITypeSystem
	{
		#region Data members

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
		/// Holds all loaded _stackFrameIndex definitions.
		/// </summary>
		private RuntimeField[] _fields;

		/// <summary>
		/// Holds offsets into arrays, where the metadata of a module start.
		/// </summary>
		private ModuleOffsets[] _moduleOffsets;

		private RuntimeBase runtimeBase;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes static data members of the type loader.
		/// </summary>
		public DefaultTypeSystem(RuntimeBase runtimeBase)
		{
			this.runtimeBase = runtimeBase;
			_methods = new RuntimeMethod[0];
			_fields = new RuntimeField[0];
			_types = new RuntimeType[0];
			_parameters = new RuntimeParameter[0];
			_moduleOffsets = new ModuleOffsets[0];
		}

		#endregion // Construction

		#region ITypeSystem Members

		/// <summary>
		/// Returns an array of all fields loaded in the type system.
		/// </summary>
		/// <value></value>
		RuntimeField[] ITypeSystem.Fields
		{
			get { return _fields; }
		}

		/// <summary>
		/// Returns an array of all methods in the type system.
		/// </summary>
		/// <value></value>
		RuntimeMethod[] ITypeSystem.Methods
		{
			get { return _methods; }
		}

		/// <summary>
		/// Returns an array of all parameters in the type system.
		/// </summary>
		/// <value></value>
		RuntimeParameter[] ITypeSystem.Parameters
		{
			get { return _parameters; }
		}

		/// <summary>
		/// Returns an array of all types in the type system.
		/// </summary>
		/// <value></value>
		RuntimeType[] ITypeSystem.Types
		{
			get { return _types; }
		}

		/// <summary>
		/// Notifies the type system that a CIL module was loaded.
		/// </summary>
		/// <param name="module">The loaded module.</param>
		void ITypeSystem.AssemblyLoaded(IMetadataModule module)
		{
			Debug.Assert(null != module && ((null == _moduleOffsets && 0 == module.LoadOrder) || _moduleOffsets.Length == module.LoadOrder));
			if (null == module)
				throw new ArgumentNullException(@"result");
			if ((null == _moduleOffsets && 0 != module.LoadOrder) || (null != _moduleOffsets && module.LoadOrder < _moduleOffsets.Length))
				throw new ArgumentException(@"Module is late?");

			IMetadataProvider md = module.Metadata;

			if (module.LoadOrder >= _moduleOffsets.Length)
				Array.Resize(ref _moduleOffsets, module.LoadOrder + 1);

			ModuleOffsets modOffset = new ModuleOffsets(
				AdjustMetadataSpace(md, TokenTypes.Field, ref _fields),
				AdjustMetadataSpace(md, TokenTypes.MethodDef, ref _methods),
				AdjustMetadataSpace(md, TokenTypes.Param, ref _parameters),
				AdjustMetadataSpace(md, TokenTypes.TypeDef, ref _types)
			);

			_moduleOffsets[module.LoadOrder] = modOffset;

			// Add space for methodspec and typespec members
			AdjustMetadataSpace(md, TokenTypes.MethodSpec, ref _methods);
			AdjustMetadataSpace(md, TokenTypes.TypeSpec, ref _types);

			// Load all types from the assembly into the type array
			LoadTypes(module, modOffset);
			// LoadTypeSpecs(module, modOffset.TypeOffset);
			// LoadMethods(module, modOffset.MethodOffset);
			LoadGenerics(module, modOffset.TypeOffset, modOffset.MethodOffset);
			//LoadFields(module, modOffset.FieldOffset);
			LoadParameters(module, modOffset.ParameterOffset);
			LoadCustomAttributes(module, modOffset);
		}

		/// <summary>
		/// Gets the types from module.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <returns></returns>
		public ReadOnlyRuntimeTypeListView GetTypesFromModule(IMetadataModule module)
		{
			if (null == module)
				throw new ArgumentNullException(@"module");

			// Determine the offsets of the module
			ModuleOffsets offsets = GetModuleOffset(module);

			// Calculate the number of types defined in this module
			int count = ((int)(TokenTypes.RowIndexMask & module.Metadata.GetMaxTokenValue(TokenTypes.TypeDef)) - 1 + 0);
			// FIXME: (int)(TokenTypes.RowIndexMask & module.Metadata.GetMaxTokenValue(TokenTypes.TypeSpec)));

			return new ReadOnlyRuntimeTypeListView(offsets.TypeOffset, count, this);
		}

		/// <summary>
		/// Gets the module offset.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <returns></returns>
		public ModuleOffsets GetModuleOffset(IMetadataModule module)
		{
			// FIXME: Make sure the IMetadataModule is really loaded in this app domain
			return _moduleOffsets[module.LoadOrder];
		}

		/// <summary>
		/// Finds the type index from token.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public int FindTypeIndexFromToken(IMetadataModule module, TokenTypes token)
		{
			// FIXME: Calculate the index of the sought token:
			// - If it is a typedef, return the module offset + row index.
			// - If it is a typeref, resolve it to the referenced typedef and return its type index
			// - If it is a typespec, resolve it to the referenced typespec
			int result = 0;
			int tokenRow = (int)(token & TokenTypes.RowIndexMask);
			switch (TokenTypes.TableMask & token)
			{
				case TokenTypes.TypeDef:
					if (tokenRow != 0)
						result = _moduleOffsets[module.LoadOrder].TypeOffset + tokenRow - 2;
					break;

				case TokenTypes.TypeRef:
					{
						TypeRefRow typeRef = module.Metadata.ReadTypeRefRow(token);
						switch (typeRef.ResolutionScopeIdx & TokenTypes.TableMask)
						{
							case TokenTypes.Module:
								// FIXME: Invalid for CIL compressed modules, but we'll support it for completeness
								result = -5;
								break;

							case TokenTypes.ModuleRef:
								// FIXME: Use the type from the referenced module
								result = -4;
								break;

							case TokenTypes.AssemblyRef:
								// FIXME: Resolve the assembly and use it
								result = ResolveAssemblyRef(module, ref typeRef);
								break;

							case TokenTypes.TypeRef:
								// Nested type within the current assembly...
								result = FindTypeIndexFromToken(module, typeRef.ResolutionScopeIdx);
								break;

							default:
								throw new ArgumentException(@"Invalid type reference.", @"token");
						}
					}
					break;

				case TokenTypes.TypeSpec:
					result = -2;
					break;

				default:
					throw new ArgumentException(@"Invalid token table.", @"tokenTypes");
			}

			return result;
		}

		private int ResolveAssemblyRef(IMetadataModule module, ref TypeRefRow typeRef)
		{
			string name = module.Metadata.ReadString(typeRef.TypeNameIdx);
			string ns = module.Metadata.ReadString(typeRef.TypeNamespaceIdx);
			AssemblyRefRow arr = module.Metadata.ReadAssemblyRefRow(typeRef.ResolutionScopeIdx);
			IAssemblyLoader loader = runtimeBase.AssemblyLoader; 
			IMetadataModule dependency = loader.Resolve(module.Metadata, arr);

			for (int i = GetModuleOffset(dependency).TypeOffset; i < _types.Length; i++)
			{
				RuntimeType type = _types[i];
				if (null != type && ns.Length == type.Namespace.Length && name.Length == type.Name.Length && name == type.Name && ns == type.Namespace)
				{
					return i;
				}
			}

			throw new TypeLoadException();
		}

		public RuntimeType GetType(ISignatureContext context, IMetadataModule module, TokenTypes token)
		{
			TokenTypes table = (TokenTypes.TableMask & token);
			if (TokenTypes.TypeRef == table)
			{
				return ResolveTypeRef(module, token);
			}
			else
			{
				int typeIdx = _moduleOffsets[module.LoadOrder].TypeOffset;
				int row = (int)(token & TokenTypes.RowIndexMask);
				if (row == 0)
					return null;

				RuntimeType result = null;
				if (table == TokenTypes.TypeDef)
				{
					typeIdx += row - 2;
					result = this._types[typeIdx];
				}
				else if (table == TokenTypes.TypeSpec)
				{
					result = this.ResolveTypeSpec(context, module, token);
				}
				else
				{
					throw new ArgumentException(@"Not a type token.", @"token");
				}

				return result;
			}
		}

		RuntimeType ITypeSystem.GetType(string typeName)
		{
			RuntimeType result = null;
			string[] names = typeName.Split(',');
			typeName = names[0];
			int lastDot = typeName.LastIndexOf('.');
			string ns, name;
			if (-1 == lastDot)
			{
				ns = String.Empty;
				name = typeName;
			}
			else
			{
				ns = typeName.Substring(0, lastDot);
				name = typeName.Substring(lastDot + 1);
			}

			if (names.Length > 1)
			{
				IMetadataModule module2 = runtimeBase.AssemblyLoader.Load(names[1].Trim());
				result = FindType(ns, name, this.GetTypesFromModule(module2));
			}
			else
			{
				result = FindType(ns, name, this._types);
			}

			return result;
		}

		/// <summary>
		/// Finds the type.
		/// </summary>
		/// <param name="ns">The namespace of the type.</param>
		/// <param name="name">The name of the type.</param>
		/// <param name="types">The collection of types to scan.</param>
		/// <returns>The <see cref="RuntimeType"/> or null.</returns>
		private static RuntimeType FindType(string ns, string name, IEnumerable<RuntimeType> types)
		{
			RuntimeType result = null;
			foreach (RuntimeType type in types)
			{
				if (null != type && ns.Length == type.Namespace.Length && name.Length == type.Name.Length && name == type.Name && ns == type.Namespace)
				{
					result = type;
					break;
				}
			}
			return result;
		}

		public RuntimeField GetField(ISignatureContext context, IMetadataModule module, TokenTypes token)
		{
			//Console.WriteLine(@"Retrieving field {0:x} in {1}", token, module.Name);
			if (null == module)
				throw new ArgumentNullException(@"module");
			if (TokenTypes.Field != (TokenTypes.TableMask & token) && TokenTypes.MemberRef != (TokenTypes.TableMask & token))
				throw new ArgumentException(@"Invalid field token.");

			RuntimeField result;

			if (TokenTypes.MemberRef == (TokenTypes.TableMask & token))
			{
				result = GetFieldForMemberReference(context, module, token);
			}
			else
			{
				ModuleOffsets offsets = GetModuleOffset(module);
				int fieldIndex = (int)(token & TokenTypes.RowIndexMask) - 1;
				result = _fields[offsets.FieldOffset + fieldIndex];
			}

			return result;
		}

		protected RuntimeField GetFieldForMemberReference(ISignatureContext context, IMetadataModule module, TokenTypes token)
		{
			IMetadataProvider metadata = module.Metadata;
			MemberRefRow row = metadata.ReadMemberRefRow(token);

			RuntimeType ownerType;

			switch (row.ClassTableIdx & TokenTypes.TableMask)
			{
				case TokenTypes.TypeSpec:
					ownerType = this.ResolveTypeSpec(context, module, row.ClassTableIdx);
					break;

				case TokenTypes.TypeDef: goto case TokenTypes.TypeRef;

				case TokenTypes.TypeRef:
					ownerType = this.GetType(context, module, row.ClassTableIdx);
					break;

				default:
					throw new NotSupportedException(String.Format(@"DefaultTypeSystem.GetFieldForMemberReference does not support Token table {0}", row.ClassTableIdx & TokenTypes.TableMask));
			}

			if (ownerType == null)
				throw new InvalidOperationException(String.Format(@"Failed to retrieve owner type for Token {0:x} (Table {1}) in {2}", row.ClassTableIdx, row.ClassTableIdx & TokenTypes.TableMask, module.Name));

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

		private RuntimeType ResolveTypeSpec(ISignatureContext context, IMetadataModule module, TokenTypes typeSpecToken)
		{
			ModuleOffsets offsets = this._moduleOffsets[module.LoadOrder];
			int typeDefs = (int)(module.Metadata.GetMaxTokenValue(TokenTypes.TypeDef) & TokenTypes.RowIndexMask) - 2;
			int typeSpecIndex = (int)(typeSpecToken & TokenTypes.RowIndexMask);

			int typeIndex = offsets.TypeOffset + typeDefs + typeSpecIndex;
			//Console.WriteLine(@"TypeSpec type index {0} is {1}", typeIndex, this._types[typeIndex]);
			if (this._types[typeIndex] == null)
			{
				TypeSpecRow typeSpec = module.Metadata.ReadTypeSpecRow(typeSpecToken);

				TypeSpecSignature signature = new TypeSpecSignature();
				signature.LoadSignature(context, module.Metadata, typeSpec.SignatureBlobIdx);

				this._types[typeIndex] = this.ResolveSignatureType(context, module, signature.Type);
			}

			return this._types[typeIndex];
		}

		/// <summary>
		/// Resolves the type of the signature.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="module">The module.</param>
		/// <param name="sigType">Type of the signature.</param>
		/// <returns></returns>
		public RuntimeType ResolveSignatureType(ISignatureContext context, IMetadataModule module, SigType sigType)
		{
			RuntimeType result = null;

			switch (sigType.Type)
			{
				case CilElementType.Class:
					{
						TypeSigType typeSigType = (TypeSigType)sigType;
						result = this.GetType(context, module, typeSigType.Token);
						//Console.WriteLine(@"TypeSpec (Class/ValueType) {0} resolves to {1}", sigType, result);
					}
					break;

				case CilElementType.ValueType:
					goto case CilElementType.Class;

				case CilElementType.GenericInst:
					{
						GenericInstSigType genericSigType = (GenericInstSigType)sigType;

						RuntimeType baseType = this.GetType(context, module, genericSigType.BaseType.Token);
						//Console.WriteLine(@"TypeSpec (GenericInst) {0} resolves to base type {1}", sigType, baseType);

						result = new CilGenericType(baseType, module, genericSigType, context, this);
					}
					break;

				default:
					throw new NotSupportedException(String.Format(@"ResolveSignatureType does not support CilElementType.{0}", sigType.Type));
			}

			return result;
		}

		protected TokenTypes DecodeTypeIndex(byte signature)
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

		protected CilElementType GetElementType(byte[] blob, int index)
		{
			return (CilElementType)(blob[4 + index]);
		}

		public RuntimeMethod GetMethod(ISignatureContext context, IMetadataModule scope, TokenTypes token)
		{
			RuntimeMethod result = null;

			if (null == scope)
				throw new ArgumentNullException(@"scope");

			switch (TokenTypes.TableMask & token)
			{
				case TokenTypes.MethodDef:
					{
						ModuleOffsets offsets = GetModuleOffset(scope);
						result = _methods[offsets.MethodOffset + (int)(TokenTypes.RowIndexMask & token) - 1];
					}
					break;

				case TokenTypes.MemberRef:
					{
						MemberRefRow row = scope.Metadata.ReadMemberRefRow(token);
						RuntimeType type = this.GetType(context, scope, row.ClassTableIdx);
						string nameString = scope.Metadata.ReadString(row.NameStringIdx);
						MethodSignature sig = (MethodSignature)Signature.FromMemberRefSignatureToken(type, scope.Metadata, row.SignatureBlobIdx);
						foreach (RuntimeMethod method in type.Methods)
						{
							if (method.Name != nameString)
								continue;
							if (!method.Signature.Matches(sig))
								continue;

							result = method;
							break;
						}

						if (result == null)
						{
							throw new MissingMethodException(type.Name, nameString);
						}
					}
					break;

				case TokenTypes.MethodSpec:
					result = this.DecodeMethodSpec(context, scope, token);
					break;

				default:
					throw new NotSupportedException(@"Can't get method for token " + token.ToString("x"));
			}

			return result;
		}

		private RuntimeMethod DecodeMethodSpec(ISignatureContext context, IMetadataModule scope, TokenTypes token)
		{
			MethodSpecRow methodSpec = scope.Metadata.ReadMethodSpecRow(token);

			CilRuntimeMethod genericMethod = (CilRuntimeMethod)this.GetMethod(context, scope, methodSpec.MethodTableIdx);

			MethodSpecSignature specSignature = new MethodSpecSignature(context);
			specSignature.LoadSignature(context, scope.Metadata, methodSpec.InstantiationBlobIdx);

			MethodSignature signature = new MethodSignature();
			signature.LoadSignature(specSignature, genericMethod.Module.Metadata, genericMethod.Signature.Token);

			return new CilGenericMethod(genericMethod, signature, specSignature, this);
		}

		#endregion // ITypeSystem Members

		#region Internals

		/// <summary>
		/// Properly resizes type information arrays to accomodate for new type information.
		/// </summary>
		/// <typeparam name="T">The type of type information to resize.</typeparam>
		/// <param name="md">The metadata provider, which contains the type metadata.</param>
		/// <param name="tokenType">The token type of the metadata to accomodate.</param>
		/// <param name="items">The array to resize.</param>
		/// <returns>The starting offset of type information for this module in the items array.</returns>
		private static int AdjustMetadataSpace<T>(IMetadataProvider md, TokenTypes tokenType, ref T[] items)
		{
			int length = (null != items ? items.Length : 0);
			int rows = (int)(TokenTypes.RowIndexMask & md.GetMaxTokenValue(tokenType));
			if (rows > 0)
			{
				rows += length;
				Array.Resize<T>(ref items, rows);
			}

			return length;
		}

		/// <summary>
		/// Loads all types from the given metadata module.
		/// </summary>
		/// <param name="module">The metadata module to load the types From.</param>
		/// <param name="moduleOffsets">The offsets into the metadata arrays, of the current module.</param>
		private void LoadTypes(IMetadataModule module, ModuleOffsets moduleOffsets)
		{
			TokenTypes maxTypeDef, maxField, maxMethod, maxLayout, tokenLayout = TokenTypes.ClassLayout + 1;
			TypeDefRow typeDefRow, nextTypeDefRow = new TypeDefRow();
			ClassLayoutRow layoutRow = new ClassLayoutRow();
			IMetadataProvider md = module.Metadata;
			int size = 0x0, packing = 0;
			int typeOffset = moduleOffsets.TypeOffset;
			int methodOffset = moduleOffsets.MethodOffset;
			int fieldOffset = moduleOffsets.FieldOffset;
			RuntimeType rt;

			maxTypeDef = md.GetMaxTokenValue(TokenTypes.TypeDef);
			maxLayout = md.GetMaxTokenValue(TokenTypes.ClassLayout);

			if (TokenTypes.ClassLayout < maxLayout)
				layoutRow = md.ReadClassLayoutRow(tokenLayout);

			TokenTypes token = TokenTypes.TypeDef + 2;
			typeDefRow = md.ReadTypeDefRow(token);
			do
			{
				/*
							  string name;
							  md.Read(typeDefRow.TypeNameIdx, out name);
							  Debug.WriteLine(name);
				 */
				if (token < maxTypeDef)
				{
					nextTypeDefRow = md.ReadTypeDefRow(token + 1);
					maxField = nextTypeDefRow.FieldList;
					maxMethod = nextTypeDefRow.MethodList;
				}
				else
				{
					maxMethod = md.GetMaxTokenValue(TokenTypes.MethodDef) + 1;
					maxField = md.GetMaxTokenValue(TokenTypes.Field) + 1;
				}

				// Is this our layout info?
				if ((layoutRow.ParentTypeDefIdx + 1) == token)
				{
					size = layoutRow.ClassSize;
					packing = layoutRow.PackingSize;

					tokenLayout++;
					if (tokenLayout <= maxLayout)
						layoutRow = md.ReadClassLayoutRow(tokenLayout);
				}

				// Create and populate the runtime type
				rt = new CilRuntimeType(token, module, ref typeDefRow, maxField, maxMethod, packing, size, this);
				LoadMethods(module, rt, typeDefRow.MethodList, maxMethod, ref methodOffset);
				LoadFields(module, rt, typeDefRow.FieldList, maxField, ref fieldOffset);
				_types[typeOffset++] = rt;

				packing = size = 0;
				typeDefRow = nextTypeDefRow;
			}
			while (token++ < maxTypeDef);

		}

		/// <summary>
		/// Loads all methods from the given metadata module.
		/// </summary>
		/// <param name="module">The metadata module to load methods From.</param>
		/// <param name="declaringType">The type, which declared the method.</param>
		/// <param name="first">The first method token to load.</param>
		/// <param name="last">The last method token to load (non-inclusive.)</param>
		/// <param name="offset">The offset into the method table to start loading methods From.</param>
		private void LoadMethods(IMetadataModule module, RuntimeType declaringType, TokenTypes first, TokenTypes last, ref int offset)
		{
			IMetadataProvider md = module.Metadata;
			MethodDefRow methodDef, nextMethodDef = new MethodDefRow();
			TokenTypes maxParam, maxMethod = md.GetMaxTokenValue(TokenTypes.MethodDef);

			if (first < last)
			{
				methodDef = md.ReadMethodDefRow(first);
				for (TokenTypes token = first; token < last; token++)
				{
					if (token < maxMethod)
					{
						nextMethodDef = md.ReadMethodDefRow(token + 1);
						maxParam = nextMethodDef.ParamList;
					}
					else
					{
						maxParam = md.GetMaxTokenValue(TokenTypes.Param) + 1;
					}

					Debug.Assert(offset < _methods.Length, @"Invalid method index.");
					_methods[offset++] = new CilRuntimeMethod(offset, module, ref methodDef, maxParam, declaringType, this);
					methodDef = nextMethodDef;
				}
			}
		}

		/// <summary>
		/// Loads all parameters from the given metadata module.
		/// </summary>
		/// <param name="module">The metadata module to load methods From.</param>
		/// <param name="offset">The offset into the parameter table to start loading methods From.</param>
		private void LoadParameters(IMetadataModule module, int offset)
		{
			IMetadataProvider md = module.Metadata;
			TokenTypes token, maxParam = md.GetMaxTokenValue(TokenTypes.Param);
			ParamRow paramDef;

			token = TokenTypes.Param + 1;
			while (token <= maxParam)
			{
				paramDef = md.ReadParamRow(token++);
				_parameters[offset++] = new RuntimeParameter(module, paramDef);
			}
		}

		/// <summary>
		/// Loads all fields defined in the module.
		/// </summary>
		/// <param name="module">The metadata module to load fields form.</param>
		/// <param name="declaringType">The type, which declared the method.</param>
		/// <param name="first">The first field token to load.</param>
		/// <param name="last">The last field token to load (non-inclusive.)</param>
		/// <param name="offset">The offset in the fields array.</param>
		private void LoadFields(IMetadataModule module, RuntimeType declaringType, TokenTypes first, TokenTypes last, ref int offset)
		{
			IMetadataProvider md = module.Metadata;
			TokenTypes maxRVA = md.GetMaxTokenValue(TokenTypes.FieldRVA),
					   maxLayout = md.GetMaxTokenValue(TokenTypes.FieldLayout),
					   tokenRva = TokenTypes.FieldRVA + 1,
					   tokenLayout = TokenTypes.FieldLayout + 1;
			FieldRow field;
			FieldRVARow fieldRVA = new FieldRVARow();
			FieldLayoutRow fieldLayout = new FieldLayoutRow();
			IntPtr rva, layout;

			if (TokenTypes.FieldRVA < maxRVA)
				fieldRVA = md.ReadFieldRVARow(tokenRva);
			if (TokenTypes.FieldLayout < maxLayout)
				fieldLayout = md.ReadFieldLayoutRow(tokenLayout);

			for (TokenTypes token = first; token < last; token++)
			{
				// Read the _stackFrameIndex
				field = md.ReadFieldRow(token);
				layout = rva = IntPtr.Zero;

				// Static fields have an optional RVA, non-static may have a layout assigned
				if ((field.Flags & FieldAttributes.HasFieldRVA) == FieldAttributes.HasFieldRVA)
				{
					// Move to the RVA of this field
					while (fieldRVA.FieldTableIdx < token && tokenRva <= maxRVA)
						fieldRVA = md.ReadFieldRVARow(tokenRva++);

					// Does this field have an RVA?
					if (token == fieldRVA.FieldTableIdx && tokenRva <= maxRVA)
					{
						rva = new IntPtr(fieldRVA.Rva);
						tokenRva++;
						if (tokenRva < maxRVA)
						{
							fieldRVA = md.ReadFieldRVARow(tokenRva);
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
						fieldLayout = md.ReadFieldLayoutRow(tokenLayout++);

					// Does this field have layout?
					if (token == fieldLayout.Field && tokenLayout <= maxLayout)
					{
						layout = new IntPtr(fieldLayout.Offset);
						tokenLayout++;
						if (tokenLayout < maxLayout)
						{
							fieldLayout = md.ReadFieldLayoutRow(tokenLayout);
						}
					}
				}

				// Load the field metadata
				_fields[offset++] = new CilRuntimeField(module, ref field, layout, rva, declaringType, this);
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
		/// <param name="module">The metadata module to load generic parameters From.</param>
		/// <param name="typeOffset"></param>
		/// <param name="methodOffset">The module offsets structure.</param>
		private void LoadGenerics(IMetadataModule module, int typeOffset, int methodOffset)
		{
			//Debug.WriteLine("Loading generic parameters... {0}", module.Name);

			IMetadataProvider md = module.Metadata;
			TokenTypes token, maxGeneric = md.GetMaxTokenValue(TokenTypes.GenericParam), owner = 0;
			GenericParamRow gpr;
			List<GenericParamRow> gprs = new List<GenericParamRow>();

			typeOffset--;
			methodOffset--;

			token = TokenTypes.GenericParam + 1;

			//Debug.WriteLine("\tFrom {0:x} to {1:x}", token, maxGeneric);

			while (token <= maxGeneric)
			{
				gpr = md.ReadGenericParamRow(token++);

				// Do we need to commit generic parameters?
				if (owner < gpr.OwnerTableIdx)
				{
					// Yes, commit them to the last type
					if (0 != owner && 0 != gprs.Count)
					{
						SetGenericParameters(gprs, owner, typeOffset, methodOffset);
						gprs.Clear();
					}

					owner = gpr.OwnerTableIdx;
				}

				gprs.Add(gpr);
			}

			// Set the generic parameters of the last type, if we have them
			if (0 != gprs.Count)
			{
				SetGenericParameters(gprs, owner, typeOffset, methodOffset);
			}
		}

		/// <summary>
		/// Sets generic parameters on a method or type.
		/// </summary>
		/// <param name="gprs">The list of generic parameters to set.</param>
		/// <param name="owner">The owner object token.</param>
		/// <param name="typeOffset">The type offset for the metadata module.</param>
		/// <param name="methodOffset">The method offset for the metadata module.</param>
		private void SetGenericParameters(List<GenericParamRow> gprs, TokenTypes owner, int typeOffset, int methodOffset)
		{
			//Debug.WriteLine("Setting generic parameters on owner {0:x}...", owner);
			switch (owner & TokenTypes.TableMask)
			{
				case TokenTypes.TypeDef:
					_types[typeOffset + (int)(TokenTypes.RowIndexMask & owner) - 1].SetGenericParameter(gprs);
					break;

				case TokenTypes.MethodDef:
					_methods[methodOffset + (int)(TokenTypes.RowIndexMask & owner)].SetGenericParameter(gprs);
					break;

				default:
					// Unknown owner table type
					throw new InvalidProgramException(@"Invalid owner table of generic parameter token.");
			}
		}

		/// <summary>
		/// Loads all custom attributes from the assembly.
		/// </summary>
		/// <param name="module">The module to load attributes From.</param>
		/// <param name="modOffset">The module offset.</param>
		private void LoadCustomAttributes(IMetadataModule module, ModuleOffsets modOffset)
		{
			IMetadataProvider metadata = module.Metadata;
			TokenTypes token, owner = 0, maxAttrs = metadata.GetMaxTokenValue(TokenTypes.CustomAttribute);

			List<CustomAttributeRow> attributes = new List<CustomAttributeRow>();

			for (token = TokenTypes.CustomAttribute + 1; token <= maxAttrs; token++)
			{
				CustomAttributeRow car = metadata.ReadCustomAttributeRow(token);

				// Do we need to commit generic parameters?
				if (owner != car.ParentTableIdx)
				{
					// Yes, commit them to the last type
					if (0 != owner && 0 != attributes.Count)
					{
						SetAttributes(module, owner, attributes);
						attributes.Clear();
					}

					owner = car.ParentTableIdx;
				}

				// Save this attribute
				attributes.Add(car);
			}

			// Set the generic parameters of the last type, if we have them
			if (0 != attributes.Count)
			{
				SetAttributes(module, owner, attributes);
			}
		}

		/// <summary>
		/// Sets the attributes.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="owner">The owner.</param>
		/// <param name="attributes">The attributes.</param>
		private void SetAttributes(IMetadataModule module, TokenTypes owner, List<CustomAttributeRow> attributes)
		{
			ITypeSystem ts = (ITypeSystem)this;

			// Convert the custom attribute rows to RuntimeAttribute instances
			RuntimeAttribute[] ra = new RuntimeAttribute[attributes.Count];
			for (int i = 0; i < attributes.Count; i++)
				ra[i] = new RuntimeAttribute(module, attributes[i], this);

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
					RuntimeType type = ts.GetType(DefaultSignatureContext.Instance, module, owner);
					type.SetAttributes(ra);
					break;

				case TokenTypes.MethodDef:
					// AttributeTargets.Constructor
					// AttributeTargets.Method
					RuntimeMethod method = ts.GetMethod(DefaultSignatureContext.Instance, module, owner);
					method.SetAttributes(ra);
					break;

				case TokenTypes.Event:
					// AttributeTargets.Event
					break;

				case TokenTypes.Field:
					// AttributeTargets.Field
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
		/// <param name="module">The module.</param>
		/// <param name="tokenTypes">The token types.</param>
		/// <returns></returns>
		private RuntimeType ResolveTypeRef(IMetadataModule module, TokenTypes tokenTypes)
		{
			// MR, 2008-08-26, patch by Alex Lyman (thanks!)
			TypeRefRow row = module.Metadata.ReadTypeRefRow(tokenTypes);
			switch (row.ResolutionScopeIdx & TokenTypes.TableMask)
			{
				case TokenTypes.Module:
				case TokenTypes.ModuleRef:
				case TokenTypes.TypeRef:
					throw new NotImplementedException();
				case TokenTypes.AssemblyRef:
					AssemblyRefRow asmRefRow = module.Metadata.ReadAssemblyRefRow(row.ResolutionScopeIdx);
					string typeName = module.Metadata.ReadString(row.TypeNameIdx);
					string typeNamespace = module.Metadata.ReadString(row.TypeNamespaceIdx);
					IMetadataModule resolvedModule = runtimeBase.AssemblyLoader.Resolve(module.Metadata, asmRefRow);

					// HACK: If there's an easier way to do this without all those string comparisons, I'm all for it
					foreach (RuntimeType type in ((ITypeSystem)this).GetTypesFromModule(resolvedModule))
					{
						if (type.Name != typeName)
							continue;
						if (type.Namespace != typeNamespace)
							continue;
						return type;
					}
					throw new TypeLoadException("Could not find type: " + typeNamespace + Type.Delimiter + typeName);
				default:
					throw new NotSupportedException();
			}
		}

		#endregion // Internals
	}
}
