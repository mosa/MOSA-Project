/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.Vm;
using Mosa.Runtime.Loader;
using System.Diagnostics;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.Metadata.Runtime
{
    /// <summary>
    /// A CIL specialization of <see cref="RuntimeMethod"/>.
    /// </summary>
    sealed class CilRuntimeMethod : RuntimeMethod
    {
        #region Data Members

        /// <summary>
        /// The index of the method name.
        /// </summary>
        private TokenTypes nameIdx;

        /// <summary>
        /// Holds the method signature.
        /// </summary>
        private TokenTypes signatureBlobIdx;

        #endregion // Data Members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="CilRuntimeMethod"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="module">The module.</param>
        /// <param name="method">The method.</param>
        /// <param name="maxParam">The max param.</param>
        /// <param name="declaringType">Type of the declaring.</param>
        public CilRuntimeMethod(int token, IMetadataModule module, ref MethodDefRow method, TokenTypes maxParam, RuntimeType declaringType) :
            base((int)token, module, declaringType)
        {
            this.nameIdx = method.NameStringIdx;
            this.signatureBlobIdx = method.SignatureBlobIdx;
            base.Attributes = method.Flags;
            base.ImplAttributes = method.ImplFlags;
            base.Rva = method.Rva;

            if (method.ParamList < maxParam)
            {
                int count = maxParam - method.ParamList;
                int p = (int)(method.ParamList & TokenTypes.RowIndexMask) - 1 + RuntimeBase.Instance.TypeLoader.GetModuleOffset(module).ParameterOffset;
                base.Parameters = new ReadOnlyRuntimeParameterListView(p, count);
            }
            else
            {
                base.Parameters = ReadOnlyRuntimeParameterListView.Empty;
            }
        }

        #endregion // Construction

        #region RuntimeMethod Overrides

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public override bool Equals(RuntimeMethod other)
        {
            CilRuntimeMethod crm = other as CilRuntimeMethod;
            return (crm != null && this.nameIdx == crm.nameIdx && this.signatureBlobIdx == crm.signatureBlobIdx && base.Equals(other) == true);
        }

        /// <summary>
        /// Gets the method signature.
        /// </summary>
        /// <returns>The method signature.</returns>
        protected override MethodSignature GetMethodSignature()
        {
            return MethodSignature.Parse(this.Module.Metadata, this.signatureBlobIdx);
        }

        /// <summary>
        /// Called to retrieve the name of the type.
        /// </summary>
        /// <returns>The name of the type.</returns>
        protected override string GetName()
        {
            string name;
            this.Module.Metadata.Read(this.nameIdx, out name);
            Debug.Assert(name != null, @"Failed to retrieve CilRuntimeMethod name.");
            return name;
        }

        #endregion // RuntimeMethod Overrides
    }
}
