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

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.Vm
{
    public class RuntimeMethod : RuntimeMember, IEquatable<RuntimeMethod>
    {
        #region Constants

        /// <summary>
        /// Static instance of RuntimeMethod array used for types without methods.
        /// </summary>
        //public static readonly RuntimeMethod[] None = new RuntimeMethod[0];

        #endregion // Constants

        #region Data members

        /// <summary>
        /// Holds the in-memory address of the method.
        /// </summary>
        private IntPtr _address;

        /// <summary>
        /// The implementation attributes of the method.
        /// </summary>
        private MethodImplAttributes _implFlags;

        /// <summary>
        /// Generic attributes of the method.
        /// </summary>
        private MethodAttributes _attributes;

        /// <summary>
        /// Holds the module, which owns the method.
        /// </summary>
        private IMetadataModule _module;

        /// <summary>
        /// The name index of the method.
        /// </summary>
        private TokenTypes _nameStringIdx;

        /// <summary>
        /// The name of the method.
        /// </summary>
        private string _name;

        /// <summary>
        /// Holds the signature of the method.
        /// </summary>
        private MethodSignature _signature;

        /// <summary>
        /// Holds the method signature.
        /// </summary>
        private TokenTypes _signatureBlobIdx;

        /// <summary>
        /// Holds the list of parameters of the method.
        /// </summary>
        private ReadOnlyRuntimeParameterListView _parameters;

        /// <summary>
        /// Holds the rva of the MSIL of the method.
        /// </summary>
        private uint _rva;

        #endregion // Data members

        #region Construction

        public RuntimeMethod(int token, IMetadataModule module, ref MethodDefRow method, TokenTypes maxParam) :
            base(token, null, null)
        {
            _implFlags = method.ImplFlags;
            _attributes = method.Flags;
            _module = module;
            _nameStringIdx = method.NameStringIdx;
            _signatureBlobIdx = method.SignatureBlobIdx;
            _rva = method.Rva;

            if (method.ParamList < maxParam)
            {
                int count = maxParam - method.ParamList;
                int p = (int)(method.ParamList & TokenTypes.RowIndexMask) - 1 + RuntimeBase.Instance.TypeLoader.GetModuleOffset(module).ParameterOffset;
                _parameters = new ReadOnlyRuntimeParameterListView(p, count);
            }
            else
            {
                _parameters = ReadOnlyRuntimeParameterListView.Empty;
            }
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets or sets the method compiler of this method.
        /// </summary>
        public IntPtr Address
        {
            get { return _address; }
            set { _address = value; }
        }

        /// <summary>
        /// Retrieves the method attributes.
        /// </summary>
        public MethodAttributes Attributes
        {
            get { return _attributes; }
        }

        /// <summary>
        /// Determines if the method is a generic method.
        /// </summary>
        public bool IsGeneric
        {
            // FIXME:
            get { return false; }
        }

        /// <summary>
        /// Gets the method implementation attributes.
        /// </summary>
        public MethodImplAttributes ImplAttributes
        {
            get { return _implFlags; }
        }

        /// <summary>
        /// Retrieves the module, which holds this method.
        /// </summary>
        public IMetadataModule Module
        {
            get { return _module; }
        }

        /// <summary>
        /// Retrieves the name of the method.
        /// </summary>
        public string Name
        {
            get
            {
                if (null != _name)
                    return _name;

                _module.Metadata.Read(_nameStringIdx, out _name);
                return _name;
            }
        }

        /// <summary>
        /// Returns the parameter definitions of this method.
        /// </summary>
        public IList<RuntimeParameter> Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Retrieves the signature of the method.
        /// </summary>
        public MethodSignature Signature
        {
            get 
            {
                if (null != _signature)
                    return _signature;

                _signature = MethodSignature.Parse(_module.Metadata, _signatureBlobIdx);
                return _signature;
            }
        }

        /// <summary>
        /// Holds the RVA of the method in the binary.
        /// </summary>
        public uint Rva
        {
            get { return _rva; }
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Sets generic parameters on this method.
        /// </summary>
        /// <param name="gprs">A list of generic parameters to set on the method.</param>
        public void SetGenericParameter(List<GenericParamRow> gprs)
        {
            // TODO: Implement this method
        }

        #endregion // Methods

        #region IEquatable<RuntimeMethod> Members

        public bool Equals(RuntimeMethod other)
        {
            return (_module == other._module && _nameStringIdx == other._nameStringIdx && _signatureBlobIdx == other._signatureBlobIdx);
        }

        #endregion // IEquatable<RuntimeMethod> Members

        #region Object Overrides

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append(this.Name);
            result.Append('(');

            if (0 != this.Parameters.Count)
            {
                MethodSignature sig = this.Signature;
                int i = 0;
                foreach (RuntimeParameter p in this.Parameters)
                {
                    result.AppendFormat("{0} {1},", sig.Parameters[i++].Type, p.Name);
                }
                result.Remove(result.Length - 1, 1);
            }

            result.Append(')');

            return result.ToString();
        }

        #endregion // Object Overrides
    }
}
