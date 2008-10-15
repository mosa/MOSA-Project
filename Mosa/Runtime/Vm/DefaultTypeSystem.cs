/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Alex Lyman (<?>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;
using System.Runtime.CompilerServices;

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

        #endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes static data members of the type loader.
		/// </summary>
		public DefaultTypeSystem()
		{
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
        ReadOnlyRuntimeTypeListView ITypeSystem.GetTypesFromModule(IMetadataModule module)
        {
            if (null == module)
                throw new ArgumentNullException(@"module");

            // Determine the offsets of the module
            ModuleOffsets offsets = GetModuleOffset(module);

            // Calculate the number of types defined in this module
            int count = ((int)(TokenTypes.RowIndexMask & module.Metadata.GetMaxTokenValue(TokenTypes.TypeDef)) - 1 + 0);
            // FIXME: (int)(TokenTypes.RowIndexMask & module.Metadata.GetMaxTokenValue(TokenTypes.TypeSpec)));

            return new ReadOnlyRuntimeTypeListView(offsets.TypeOffset, count);
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
                        TypeRefRow typeRef;
                        module.Metadata.Read(token, out typeRef);
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
            AssemblyRefRow arr;
            string ns, name;
            module.Metadata.Read(typeRef.TypeNameIdx, out name);
            module.Metadata.Read(typeRef.TypeNamespaceIdx, out ns);
            module.Metadata.Read(typeRef.ResolutionScopeIdx, out arr);
            IAssemblyLoader loader = RuntimeBase.Instance.AssemblyLoader;
            IMetadataModule dependency = loader.Resolve(module.Metadata, arr);
            ITypeSystem ts = (ITypeSystem)this;

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

        RuntimeType ITypeSystem.GetType(IMetadataModule module, TokenTypes token)
        {
            int typeIdx = _moduleOffsets[module.LoadOrder].TypeOffset;
            TokenTypes table = (TokenTypes.TableMask & token);
            TokenTypes row = (token & TokenTypes.RowIndexMask);
            switch (table)
            {
                case TokenTypes.TypeRef:
                    return ResolveTypeRef(module, row);

                case TokenTypes.TypeDef:
                    typeIdx += (int)row-2;
                    break;

                case TokenTypes.TypeSpec:
                    typeIdx += (int)(module.Metadata.GetMaxTokenValue(TokenTypes.TypeDef) & TokenTypes.RowIndexMask) + (int)row;
                    break;
            }

            return _types[typeIdx];
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

            /* FIXME: Typename could be a fully formatted type name, make sure
             * we support this one day, so we could use the assembly information
             * to reduce the lookup times.
             */
            result = FindType(ns, name, _types);
            if (2 <= names.Length && null == result)
            {
                try
                {
                    IMetadataModule module = RuntimeBase.Instance.AssemblyLoader.Load(names[1].Trim() + ".dll");
                    ITypeSystem ts = (ITypeSystem)this;
                    result = FindType(ns, name, ts.GetTypesFromModule(module));
                }
                catch
                {
                }
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

        RuntimeField ITypeSystem.GetField(IMetadataModule scope, TokenTypes token)
        {
            if (null == scope)
                throw new ArgumentNullException(@"scope");
            if (TokenTypes.Field != (TokenTypes.TableMask & token) && TokenTypes.MemberRef != (TokenTypes.TableMask & token))
                throw new ArgumentException(@"Invalid field token.");

            if (TokenTypes.MemberRef == (TokenTypes.TableMask & token))
            {
                MemberRefRow row;
                scope.Metadata.Read(token, out row);
                throw new NotImplementedException();  
            }

            ModuleOffsets offsets = GetModuleOffset(scope);
            int fieldIndex = (int)(token & TokenTypes.RowIndexMask) - 1;
            RuntimeField result = _fields[offsets.FieldOffset + fieldIndex];
            return result;
        }

        RuntimeMethod ITypeSystem.GetMethod(IMetadataModule scope, TokenTypes token)
        {
            if (null == scope)
                throw new ArgumentNullException(@"scope");

            switch (TokenTypes.TableMask & token)
            {
                case TokenTypes.MethodDef:
                    {
                        ModuleOffsets offsets = GetModuleOffset(scope);
                        return _methods[offsets.MethodOffset + (int)(TokenTypes.RowIndexMask & token) - 1];
                    }

                case TokenTypes.MemberRef:
                    {
                        MemberRefRow row;
                        scope.Metadata.Read(token, out row);
                        RuntimeType type = this.ResolveTypeRef(scope, row.ClassTableIdx);
                        string nameString;
                        scope.Metadata.Read(row.NameStringIdx, out nameString);
                        MethodSignature sig = (MethodSignature)Signature.FromMemberRefSignatureToken(scope.Metadata, row.SignatureBlobIdx);
                        List<RuntimeMethod> methods = new List<RuntimeMethod>(type.Methods);
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
                    throw new NotImplementedException();

                default:
                    throw new NotSupportedException();
            }
        }

        #endregion // ITypeSystem Members

        #region InternalCall Support

        /// <summary>
        /// Holds a map of internal call methods to their implementation.
        /// </summary>
        private Dictionary<RuntimeMethod, RuntimeMethod> internalCallTargets = new Dictionary<RuntimeMethod, RuntimeMethod>();

        /// <summary>
        /// A list of types, which realize internal calls.
        /// </summary>
        private List<RuntimeType> internalTypes = new List<RuntimeType>();

        RuntimeMethod ITypeSystem.GetImplementationForInternalCall(RuntimeMethod internalCall)
        {
            Debug.Assert(internalCall != null, @"internalCall is null.");
            if (null == internalCall)
                throw new ArgumentNullException(@"internalCall");

            // Return value
            RuntimeMethod result = null;

            // Shortcut: If the call was resolved previously, scan it there
            if (true == this.internalCallTargets.TryGetValue(internalCall, out result))
                return result;

            /*
             * FIXME:
             * - The following sequence requires that mscorlib is available in the test runtime.
             * - Maybe we should be smarter about its use though.
             * - Commented out right now, as we don't load assembly dependencies yet.
             */

            ITypeSystem ts = (ITypeSystem)this;
            // FIXME: Include this when we're loading mscorlib
            //RuntimeType rtMia = ts.GetType(@"System.Runtime.CompilerServices.MethodImplAttribute");
            //MethodImplAttribute mia = (MethodImplAttribute)internalCall.GetAttributes(rtMia, true);
            //Debug.Assert(MethodImplOptions.InternalCall == (mia.Value & MethodImplOptions.InternalCall), @"Method is not InternalCall.");
            //if (MethodImplOptions.InternalCall != (mia.Value & MethodImplOptions.InternalCall))
            //    throw new ArgumentException(@"Method not marked as an InternalCall.", @"internalCall");

            RuntimeType callImplType = ts.GetType("Mosa.Runtime.Vm.InternalCallImplAttribute");
            object[] callDefAttrs = internalCall.GetCustomAttributes(callImplType);
            Debug.Assert(0 != callDefAttrs.Length, @"No runtime call definition for icall!");
            Debug.Assert(1 == callDefAttrs.Length, @"Only one call definition for icall supported! Additional ones ignored!");
            InternalCallImplAttribute callDef = (InternalCallImplAttribute)callDefAttrs[0];

            // Scan all known types for icalls
            foreach (RuntimeType rType in internalTypes)
            {
                foreach (RuntimeMethod rMethod in rType.Methods)
                {
                    object[] callImpls = rMethod.GetCustomAttributes(callImplType);
                    foreach (InternalCallImplAttribute callImpl in callImpls)
                    {
                        if (callDef.Matches(callImpl) == true)
                        {
                            // We found the sought icall, save it and return it
                            this.internalCallTargets[internalCall] = rMethod;
                            return rMethod;
                        }
                    }
                }
            }

            throw new NotImplementedException(@"Requested InternalCall not loaded or not implemented.");
        }

        #endregion // InternalCall Support

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

        private RuntimeType rtCallTypeAttribute = null;

        /// <summary>
        /// Loads all types from the given metadata module.
        /// </summary>
        /// <param name="module">The metadata module to load the types from.</param>
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
                md.Read(tokenLayout, out layoutRow);

            TokenTypes token = TokenTypes.TypeDef+2;
            md.Read(token, out typeDefRow);
            do
            {
                /*
                              string name;
                              md.Read(typeDefRow.TypeNameIdx, out name);
                              Debug.WriteLine(name);
                 */
                if (token < maxTypeDef)
                {
                    md.Read(token+1, out nextTypeDefRow);
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
                        md.Read(tokenLayout, out layoutRow);
                }

                // Create and populate the runtime type
                rt = new RuntimeType((int)token, module, ref typeDefRow, maxField, maxMethod, packing, size);
                LoadMethods(module, rt, typeDefRow.MethodList, maxMethod, ref methodOffset);
                LoadFields(module, rt, typeDefRow.FieldList, maxField, ref fieldOffset);
                _types[typeOffset++] = rt;

                Debug.Assert(rt.Name != @"CompilerSupport");

                if (rtCallTypeAttribute == null)
                {
                    if (rt.Name == "InternalCallTypeAttribute" && rt.Namespace == "Mosa.Runtime.Vm")
                    {
                        rtCallTypeAttribute = rt;
                    }
                }

                packing = size = 0;
                typeDefRow = nextTypeDefRow;
            }
            while (token++ < maxTypeDef);

        }

        /// <summary>
        /// Loads all methods from the given metadata module.
        /// </summary>
        /// <param name="module">The metadata module to load methods from.</param>
        /// <param name="declaringType">The type, which declared the method.</param>
        /// <param name="first">The first method token to load.</param>
        /// <param name="last">The last method token to load (non-inclusive.)</param>
        /// <param name="offset">The offset into the method table to start loading methods from.</param>
        private void LoadMethods(IMetadataModule module, RuntimeType declaringType, TokenTypes first, TokenTypes last, ref int offset)
        {
            IMetadataProvider md = module.Metadata;
            MethodDefRow methodDef, nextMethodDef = new MethodDefRow();
            TokenTypes maxParam, maxMethod = md.GetMaxTokenValue(TokenTypes.MethodDef);

            if (first < last)
            {
                md.Read(first, out methodDef);
                for (TokenTypes token = first; token < last; token++)
                {
                    if (token < maxMethod)
                    {
                        md.Read(token + 1, out nextMethodDef);
                        maxParam = nextMethodDef.ParamList;
                    }
                    else
                    {
                        maxParam = md.GetMaxTokenValue(TokenTypes.Param) + 1;
                    }

                    Debug.Assert(offset < _methods.Length, @"Invalid method index.");
                    _methods[offset++] = new RuntimeMethod(offset, module, ref methodDef, maxParam, declaringType);
                    methodDef = nextMethodDef;
                }
            }
        }

        /// <summary>
        /// Loads all parameters from the given metadata module.
        /// </summary>
        /// <param name="module">The metadata module to load methods from.</param>
        /// <param name="offset">The offset into the parameter table to start loading methods from.</param>
        private void LoadParameters(IMetadataModule module, int offset)
        {
            IMetadataProvider md = module.Metadata;
            TokenTypes token, maxParam = md.GetMaxTokenValue(TokenTypes.Param);
            ParamRow paramDef;

            token = TokenTypes.Param + 1;
            while (token <= maxParam)
            {
                md.Read(token++, out paramDef);
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
            uint rva = 0xFFFFFFFF, layout = 0xFFFFFFFF;

            if (TokenTypes.FieldRVA < maxRVA)
                md.Read(tokenRva, out fieldRVA);
            if (TokenTypes.FieldLayout < maxLayout)
                md.Read(tokenLayout, out fieldLayout);

            for (TokenTypes token = first; token < last; token++)
            {
                // Read the _stackFrameIndex
                md.Read(token, out field);

                // Move to the RVA of this field
                while (fieldRVA.FieldTableIdx < token && tokenRva <= maxRVA)
                    md.Read(tokenRva++, out fieldRVA);

                // Does this field have an RVA?
                if (token == fieldRVA.FieldTableIdx && tokenRva <= maxRVA)
                {
                    rva = fieldRVA.Rva;
                    tokenRva++;
                    if (tokenRva < maxRVA)
                    {
                        md.Read(tokenRva, out fieldRVA);
                    }
                }

                // Move to the layout of this field
                while (fieldLayout.Field < token && tokenLayout <= maxLayout)
                    md.Read(tokenLayout++, out fieldLayout);

                // Does this field have layout?
                if (token == fieldLayout.Field && tokenLayout <= maxLayout)
                {
                    layout = fieldLayout.Offset;
                    tokenLayout++;
                    if (tokenLayout < maxLayout)
                    {
                        md.Read(tokenLayout, out fieldLayout);
                    }
                }

                // Load the field metadata
                _fields[offset++] = new RuntimeField(module, ref field, layout, rva, declaringType);
                layout = rva = 0xFFFFFFFF;
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
        /// <param name="module">The metadata module to load generic parameters from.</param>
        /// <param name="typeOffset"></param>
        /// <param name="methodOffset">The module offsets structure.</param>
        private void LoadGenerics(IMetadataModule module, int typeOffset, int methodOffset)
        {
            IMetadataProvider md = module.Metadata;
            TokenTypes token, maxGeneric = md.GetMaxTokenValue(TokenTypes.GenericParam), owner = 0;
            GenericParamRow gpr;
            List<GenericParamRow> gprs = new List<GenericParamRow>();

            typeOffset--;
            methodOffset--;

            token = TokenTypes.GenericParam + 1;
            while (token <= maxGeneric)
            {
                md.Read(token++, out gpr);

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
                else
                {
                    gprs.Add(gpr);
                }
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
            switch (owner & TokenTypes.TableMask)
            {
                case TokenTypes.TypeDef:
                    _types[typeOffset + (int)(TokenTypes.RowIndexMask & owner)].SetGenericParameter(gprs);
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
        /// <param name="module">The module to load attributes from.</param>
        /// <param name="modOffset">The module offset.</param>
        private void LoadCustomAttributes(IMetadataModule module, ModuleOffsets modOffset)
        {
            IMetadataProvider metadata = module.Metadata;
            TokenTypes token, owner = 0, maxAttrs = metadata.GetMaxTokenValue(TokenTypes.CustomAttribute);
            CustomAttributeRow car;
            List<CustomAttributeRow> attributes = new List<CustomAttributeRow>();

            for (token = TokenTypes.CustomAttribute + 1; token <= maxAttrs; token++)
            {
                metadata.Read(token, out car);

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
                ra[i] = new RuntimeAttribute(module, attributes[i]);

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
                    RuntimeType type = ts.GetType(module, owner);
                    type.SetAttributes(ra);
                    if (rtCallTypeAttribute != null)
                    {
                        if (type.IsDefined(rtCallTypeAttribute) == true)
                        {
                            this.internalTypes.Add(type);
                        }
                    }
                    break;

                case TokenTypes.MethodDef:
                    // AttributeTargets.Constructor
                    // AttributeTargets.Method
                    RuntimeMethod method = ts.GetMethod(module, owner);
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
            TypeRefRow row;
            module.Metadata.Read(tokenTypes, out row);
            switch (row.ResolutionScopeIdx & TokenTypes.TableMask)
            {
                case TokenTypes.Module:
                case TokenTypes.ModuleRef:
                case TokenTypes.TypeRef:
                    throw new NotImplementedException();
                case TokenTypes.AssemblyRef:
                    AssemblyRefRow asmRefRow;
                    module.Metadata.Read(row.ResolutionScopeIdx, out asmRefRow);
                    string typeNamespace, typeName;
                    module.Metadata.Read(row.TypeNameIdx, out typeName);
                    module.Metadata.Read(row.TypeNamespaceIdx, out typeNamespace);
                    IMetadataModule resolvedModule = RuntimeBase.Instance.AssemblyLoader.Resolve(module.Metadata, asmRefRow);

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
