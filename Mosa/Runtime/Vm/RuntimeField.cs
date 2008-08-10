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

using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.Metadata.Signatures;

namespace Mosa.Runtime.Vm
{
    public class RuntimeField : IEquatable<RuntimeField>
    {
        #region Static constants

        /// <summary>
        /// Static array instance used for all types, which don't have fields.
        /// </summary>
        public static readonly RuntimeField[] None = new RuntimeField[0];

        #endregion // Static constants

        #region Data members

        /// <summary>
        /// The module, which defines the RuntimeField.
        /// </summary>
        private IMetadataModule _module;

        /// <summary>
        /// Holds the attributes of the RuntimeField.
        /// </summary>
        private FieldAttributes _fieldAttributes;

        /// <summary>
        /// Holds the name index of the RuntimeField.
        /// </summary>
        private TokenTypes _nameIdx;

        /// <summary>
        /// Signature identifier of this RuntimeField.
        /// </summary>
        private int _sig;

	private uint _offset;
	private uint _rva;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Loads a _stackFrameIndex definition from metadata.
        /// </summary>
        /// <param name="module">The module to load the _stackFrameIndex from.</param>
        /// <param name="_stackFrameIndex">The _stackFrameIndex metadata row.</param>
		/// <param name="offset">Holds the offset of the _stackFrameIndex in the owner type.</param>
		/// <param name="rva">The RVA of the initialization data</param>
        public RuntimeField(IMetadataModule module, ref FieldRow field, uint offset, uint rva)
        {
            _module = module;
            _fieldAttributes = field.Flags;
            _nameIdx = field.NameStringIdx;
		  _offset = offset;
		  _rva = rva;

            // FIXME: Load the signature of the _stackFrameIndex
        }

        #endregion // Construction

        #region Properties

        public IntPtr Address
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public SigType Type
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion // Properties

        #region IEquatable<RuntimeField> Members

        public bool Equals(RuntimeField other)
        {
            return (_module == other._module && _fieldAttributes == other._fieldAttributes && _nameIdx == other._nameIdx && _sig == other._sig);
        }

        #endregion // IEquatable<RuntimeField> Members
    }
}
