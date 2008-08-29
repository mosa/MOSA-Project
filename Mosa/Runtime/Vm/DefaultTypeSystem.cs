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

        RuntimeField[] ITypeSystem.Fields
        {
            get { return _fields; }
        }

        RuntimeMethod[] ITypeSystem.Methods
        {
            get { return _methods; }
        }

        RuntimeParameter[] ITypeSystem.Parameters
        {
            get { return _parameters; }
        }

        RuntimeType[] ITypeSystem.Types
        {
            get { return _types; }
        }

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
            LoadTypes(module, modOffset.TypeOffset);
            // LoadTypeSpecs(module, modOffset.TypeOffset);
            LoadMethods(module, modOffset.MethodOffset);
            LoadGenerics(module, modOffset.TypeOffset, modOffset.MethodOffset);
            LoadFields(module, modOffset.FieldOffset);
            LoadParameters(module, modOffset.ParameterOffset);
            LoadCustomAttributes(module, modOffset);
        }

        ReadOnlyRuntimeTypeListView ITypeSystem.GetTypesFromModule(IMetadataModule module)
        {
            if (null == module)
                throw new ArgumentNullException(@"module");

            // Determine the offsets of the module
            ModuleOffsets offsets = GetModuleOffset(module);

            // Calculate the number of types defined in this module
            int count = ((int)(TokenTypes.RowIndexMask & module.Metadata.GetMaxTokenValue(TokenTypes.TypeDef)) - 1 +
                         0);
            // FIXME: (int)(TokenTypes.RowIndexMask & module.Metadata.GetMaxTokenValue(TokenTypes.TypeSpec)));

            return new ReadOnlyRuntimeTypeListView(offsets.TypeOffset, count);
        }

        public ModuleOffsets GetModuleOffset(IMetadataModule module)
        {
            // FIXME: Make sure the IMetadataModule is really loaded in this app domain
            return _moduleOffsets[module.LoadOrder];
        }

        public int FindTypeIndexFromToken(IMetadataModule module, TokenTypes token)
        {
            // FIXME: Calculate the index of the sought token:
            // - If it is a typedef, return the module offset + row index.
            // - If it is a typeref, resolve it to the referenced typedef and return its type index
            // - If it is a typespec, resolve it to the referenced typespec
            int result;
            switch (TokenTypes.TableMask & token)
            {
                case TokenTypes.TypeDef:
                    result = _moduleOffsets[module.LoadOrder].TypeOffset + (int)((token & TokenTypes.RowIndexMask) - 1);
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
                                result = -3;
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
                    typeIdx += (int)row;
                    break;

                case TokenTypes.TypeSpec:
                    typeIdx += (int)(module.Metadata.GetMaxTokenValue(TokenTypes.TypeDef) & TokenTypes.RowIndexMask) + (int)row;
                    break;
            }

            return _types[typeIdx];
        }

        RuntimeField ITypeSystem.GetField(IMetadataModule scope, TokenTypes token)
        {
            if (null == scope)
                throw new ArgumentNullException(@"scope");
            if (TokenTypes.Field != (TokenTypes.TableMask & token) || TokenTypes.MemberRef != (TokenTypes.TableMask & token))
                throw new ArgumentException(@"Invalid _stackFrameIndex token.");

            if (TokenTypes.MemberRef != (TokenTypes.TableMask & token))
            {
                MemberRefRow row;
                scope.Metadata.Read(token, out row);
                throw new NotImplementedException();
                // token = row
            }

            ModuleOffsets offsets = GetModuleOffset(scope);

            throw new NotImplementedException();
            return null;
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
                        Signature sig = Signature.FromMemberRefSignatureToken(scope.Metadata, row.SignatureBlobIdx);
                        List<RuntimeMethod> methods = new List<RuntimeMethod>(type.Methods);
                        foreach (RuntimeMethod method in type.Methods)
                        {
                            if (method.Name != nameString)
                                continue;
                            // FIXME: Check the signature
                            //if (!method.Signature.Equals(sig))
                            //    continue;
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
                rows += (length + 1);
                Array.Resize<T>(ref items, rows);
            }

            return length;
        }

        /// <summary>
        /// Loads all types from the given metadata module.
        /// </summary>
        /// <param name="module">The metadata module to load the types from.</param>
        /// <param name="typeOffset">The offset into the type array, where we start loading.</param>
        private void LoadTypes(IMetadataModule module, int offset)
        {
            TokenTypes maxTypeDef, maxField, maxMethod, maxLayout, tokenLayout = TokenTypes.ClassLayout + 1;
            TypeDefRow typeDefRow, nextTypeDefRow = new TypeDefRow();
            ClassLayoutRow layoutRow = new ClassLayoutRow();
            IMetadataProvider md = module.Metadata;
            uint size = 0xFFFFFFFF, packing = 0xFFFFFFFF;

            maxTypeDef = md.GetMaxTokenValue(TokenTypes.TypeDef);
            maxLayout = md.GetMaxTokenValue(TokenTypes.ClassLayout);

            if (TokenTypes.ClassLayout < maxLayout)
                md.Read(tokenLayout, out layoutRow);

            TokenTypes token = TokenTypes.TypeDef + 2;
            md.Read(token++, out typeDefRow);
            do
            {
                /*
                              string name;
                              md.Read(typeDefRow.TypeNameIdx, out name);
                              Debug.WriteLine(name);
                 */
                if (token <= maxTypeDef)
                {
                    md.Read(token, out nextTypeDefRow);
                    maxField = nextTypeDefRow.FieldList;
                    maxMethod = nextTypeDefRow.MethodList;
                }
                else
                {
                    maxMethod = md.GetMaxTokenValue(TokenTypes.MethodDef);
                    maxField = md.GetMaxTokenValue(TokenTypes.Field);
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

                _types[offset++] = new RuntimeType(module, ref typeDefRow, maxField, maxMethod, packing, size);
                packing = size = 0xFFFFFFFF;
                typeDefRow = nextTypeDefRow;
            }
            while (token++ <= maxTypeDef);

        }

        /// <summary>
        /// Loads all methods from the given metadata module.
        /// </summary>
        /// <param name="module">The metadata module to load methods from.</param>
        /// <param name="offset">The offset into the method table to start loading methods from.</param>
        private void LoadMethods(IMetadataModule module, int offset)
        {
            IMetadataProvider md = module.Metadata;
            TokenTypes token, maxMethod = md.GetMaxTokenValue(TokenTypes.MethodDef), maxParam;
            MethodDefRow methodDef, nextMethodDef = new MethodDefRow();

            token = TokenTypes.MethodDef + 1;
            md.Read(token++, out methodDef);
            do
            {
                if (token <= maxMethod)
                {
                    md.Read(token, out nextMethodDef);
                    maxParam = nextMethodDef.ParamList;
                }
                else
                {
                    maxParam = md.GetMaxTokenValue(TokenTypes.Param) + 1;
                }

                _methods[offset++] = new RuntimeMethod(offset, module, ref methodDef, maxParam);
                methodDef = nextMethodDef;
            }
            while (token++ <= maxMethod);
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
        /// <param name="offset">The _stackFrameIndex offset in the _stackFrameIndex table.</param>
        private void LoadFields(IMetadataModule module, int offset)
        {
            IMetadataProvider md = module.Metadata;
            TokenTypes maxField = md.GetMaxTokenValue(TokenTypes.Field),
                       maxRVA = md.GetMaxTokenValue(TokenTypes.FieldRVA),
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

            for (TokenTypes token = TokenTypes.Field + 1; token <= maxField; token++)
            {

                // Read the _stackFrameIndex
                md.Read(token, out field);

                // Does this _stackFrameIndex have an RVA?
                if (token == fieldRVA.FieldTableIdx && tokenRva <= maxRVA)
                {
                    rva = fieldRVA.Rva;
                    tokenRva++;
                    if (tokenRva < maxRVA)
                    {
                        md.Read(tokenRva, out fieldRVA);
                    }
                }

                // Does this _stackFrameIndex have layout?
                if (token == fieldLayout.Field && tokenLayout <= maxLayout)
                {
                    layout = fieldLayout.Offset;
                    tokenLayout++;
                    if (tokenLayout < maxLayout)
                    {
                        md.Read(tokenLayout, out fieldLayout);
                    }
                }

                // Load the _stackFrameIndex metadata
                _fields[offset++] = new RuntimeField(module, ref field, layout, rva);
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
        /// <param name="modOffset">The module offsets structure.</param>
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
        /// <param name="metadata">The metadata to load attributes from.</param>
        /// <param name="modOffset">The module offset.</param>
        private void LoadCustomAttributes(IMetadataModule module, ModuleOffsets modOffset)
        {
            IMetadataProvider metadata = module.Metadata;
            TokenTypes token, owner = 0, maxAttrs = metadata.GetMaxTokenValue(TokenTypes.CustomAttribute);
            CustomAttributeRow car;
            List<CustomAttributeRow> attributes = new List<CustomAttributeRow>();
            byte[] caBlob;

            for (token = TokenTypes.CustomAttribute + 1; token <= maxAttrs; token++)
            {
                metadata.Read(token, out car);

                // Do we need to commit generic parameters?
                if (owner < car.ParentTableIdx)
                {
                    // Yes, commit them to the last type
                    if (0 != owner && 0 != attributes.Count)
                    {
                        SetAttributes(module, owner, attributes);
                        attributes.Clear();
                    }

                    owner = car.ParentTableIdx;
                }
                else
                {
                    attributes.Add(car);
                }

                // Set the generic parameters of the last type, if we have them
                if (0 != attributes.Count)
                {
                    SetAttributes(module, owner, attributes);
                }
            }
        }

        private void SetAttributes(IMetadataModule module, TokenTypes owner, List<CustomAttributeRow> attributes)
        {
            // Convert the custom attribute rows to RuntimeAttribute instances
            RuntimeAttribute[] ra = new RuntimeAttribute[attributes.Count];
            for (int i = 0; i < attributes.Count; i++)
                ra[i] = new RuntimeAttribute(attributes[i]);

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
                    break;

                case TokenTypes.MethodDef:
                    // AttributeTargets.Constructor
                    // AttributeTargets.Method
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
