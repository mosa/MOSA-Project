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
    /// <summary>
    /// 
    /// </summary>
    public class RuntimeField : RuntimeMember, IEquatable<RuntimeField>
    {
        #region Static constants

        /// <summary>
        /// Static array instance used for all types, which don't have fields.
        /// </summary>
        public static readonly RuntimeField[] None = new RuntimeField[0];

        #endregion // Static constants

        #region Data members

        /// <summary>
        /// Holds the attributes of the RuntimeField.
        /// </summary>
        private FieldAttributes _attributes;

        /// <summary>
        /// Holds the name of the field to return.
        /// </summary>
        private string _name;

        /// <summary>
        /// Holds the name index of the RuntimeField.
        /// </summary>
        private TokenTypes _nameIdx;

        /// <summary>
        /// Signature identifier of this RuntimeField.
        /// </summary>
        private int _sig;

        /// <summary>
        /// 
        /// </summary>
        private uint _offset;

        /// <summary>
        /// 
        /// </summary>
        private uint _rva;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Loads a _stackFrameIndex definition from metadata.
        /// </summary>
        /// <param name="module">The module to load the _stackFrameIndex from.</param>
        /// <param name="field">The field metadata row.</param>
        /// <param name="offset">Holds the offset of the _stackFrameIndex in the owner type.</param>
        /// <param name="rva">The RVA of the initialization data</param>
        /// <param name="declaringType">Specifies the type, which contains this field.</param>
        public RuntimeField(IMetadataModule module, ref FieldRow field, uint offset, uint rva, RuntimeType declaringType) :
            base(0, module, declaringType, null)
        {
            _sig = 0;
            _name = Name;
            _attributes = field.Flags;
            _nameIdx = field.NameStringIdx;
            _offset = offset;
            _rva = rva;

            // FIXME: Load the signature of the field
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the attributes of the field.
        /// </summary>
        /// <value>The attributes.</value>
        public FieldAttributes Attributes
        {
            get { return _attributes; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get
            {
                if (null != _name)
                    return _name;

                // FIXME: Load the name of the field

                return _name;
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        public SigType Type
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion // Properties

        #region IEquatable<RuntimeField> Members

        /// <summary>
        /// Gibt an, ob das aktuelle Objekt gleich einem anderen Objekt des gleichen Typs ist.
        /// </summary>
        /// <param name="other">Ein Objekt, das mit diesem Objekt verglichen werden soll.</param>
        /// <returns>
        /// true, wenn das aktuelle Objekt gleich dem <paramref name="other"/>-Parameter ist, andernfalls false.
        /// </returns>
        public bool Equals(RuntimeField other)
        {
            return (Module == other.Module && _attributes == other._attributes && _nameIdx == other._nameIdx && _sig == other._sig);
        }

        #endregion // IEquatable<RuntimeField> Members
    }
}
