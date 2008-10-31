/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.Vm;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata.Tables;
using System.Diagnostics;

namespace Mosa.Runtime.Metadata.Runtime
{
    /// <summary>
    /// Runtime representation of a CIL type.
    /// </summary>
    sealed class CilRuntimeType : RuntimeType
    {
        #region Data Members

        /// <summary>
        /// Holds the token of the base type of this type.
        /// </summary>
        private TokenTypes baseTypeToken;

        /// <summary>
        /// The metadata module, which owns this type.
        /// </summary>
        private IMetadataModule module;

        /// <summary>
        /// The name index of the defined type.
        /// </summary>
        private TokenTypes nameIdx;

        /// <summary>
        /// The namespace index of the defined type.
        /// </summary>
        private TokenTypes namespaceIdx;

        #endregion // Data Members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CilRuntimeType"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="module">The module.</param>
        /// <param name="typeDefRow">The type def row.</param>
        /// <param name="maxField">The max field.</param>
        /// <param name="maxMethod">The max method.</param>
        /// <param name="packing">The packing.</param>
        /// <param name="size">The size.</param>
        public CilRuntimeType(TokenTypes token, IMetadataModule module, ref TypeDefRow typeDefRow, TokenTypes maxField, TokenTypes maxMethod, int packing, int size) :
            base((int)token, module)
        {
            this.baseTypeToken = typeDefRow.Extends;
            this.module = module;
            this.nameIdx = typeDefRow.TypeNameIdx;
            this.namespaceIdx = typeDefRow.TypeNamespaceIdx;

            base.Attributes = typeDefRow.Flags;
            base.Pack = packing;
            base.Size = size;

            // Load all fields of the type
            int members = maxField - typeDefRow.FieldList;
            if (0 < members)
            {
                int i = (int)(typeDefRow.FieldList & TokenTypes.RowIndexMask) - 1 + RuntimeBase.Instance.TypeLoader.GetModuleOffset(module).FieldOffset;
                base.Fields = new ReadOnlyRuntimeFieldListView(i, members);
            }
            else
            {
                base.Fields = ReadOnlyRuntimeFieldListView.Empty;
            }

            // Load all methods of the type
            members = maxMethod - typeDefRow.MethodList;
            if (0 < members)
            {
                int i = (int)(typeDefRow.MethodList & TokenTypes.RowIndexMask) - 1 + RuntimeBase.Instance.TypeLoader.GetModuleOffset(module).MethodOffset;
                base.Methods = new ReadOnlyRuntimeMethodListView(i, members);
            }
            else
            {
                base.Methods = ReadOnlyRuntimeMethodListView.Empty;
            }
        }

        #endregion // Construction

        #region Methods

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public override bool Equals(RuntimeType other)
        {
            CilRuntimeType crt = other as CilRuntimeType;
            return (crt != null && 
                    this.module == crt.module && 
                    this.nameIdx == crt.nameIdx && 
                    this.namespaceIdx == crt.namespaceIdx && 
                    this.baseTypeToken == crt.baseTypeToken &&
                    base.Equals(other) == true);
        }

        /// <summary>
        /// Gets the base type.
        /// </summary>
        /// <returns>The base type.</returns>
        protected override RuntimeType GetBaseType()
        {
            ITypeSystem typeSystem = RuntimeBase.Instance.TypeLoader;
            return typeSystem.GetType(this.Module, this.baseTypeToken);
        }

        /// <summary>
        /// Called to retrieve the name of the type.
        /// </summary>
        /// <returns>The name of the type.</returns>
        protected override string GetName()
        {
            string name;
            this.module.Metadata.Read(this.nameIdx, out name);
            Debug.Assert(name != null, @"Failed to retrieve CilRuntimeMethod name.");
            return name;
        }

        /// <summary>
        /// Called to retrieve the namespace of the type.
        /// </summary>
        /// <returns>The namespace of the type.</returns>
        protected override string GetNamespace()
        {
            string @namespace;
            this.module.Metadata.Read(this.namespaceIdx, out @namespace);
            Debug.Assert(@namespace != null, @"Failed to retrieve CilRuntimeMethod name.");
            return @namespace;
        }

        #endregion // Methods
    }
}
