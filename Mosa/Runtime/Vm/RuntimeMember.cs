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

namespace Mosa.Runtime.Vm
{
    public abstract class RuntimeMember : RuntimeObject, IRuntimeAttributable
    {
        #region Data members

        /// <summary>
        /// Holds the attributes of the member.
        /// </summary>
        private RuntimeAttribute[] _attributes;

        /// <summary>
        /// Specifies the type, that declares the member.
        /// </summary>
        private RuntimeType _declaringType;

        /// <summary>
        /// Holds the module, which owns the method.
        /// </summary>
        private IMetadataModule _module;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="RuntimeMember"/>.
        /// </summary>
        /// <param name="token">Holds the token of this runtime metadata.</param>
        /// <param name="declaringType">The declaring type of the member.</param>
        /// <param name="attributes">Holds the attributes of the member.</param>
        protected RuntimeMember(int token, IMetadataModule module, RuntimeType declaringType, RuntimeAttribute[] attributes) :
            base(token)
        {
            _module = module;
            _declaringType = declaringType;
            _attributes = attributes;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Retrieves the declaring type of the member.
        /// </summary>
        public RuntimeType DeclaringType
        {
            get { return _declaringType; }
        }

        public abstract string Name { get; }

        IntPtr _address;

        public IntPtr Address { 
            get { return _address; } 
            set { _address = value; }
        }

        /// <summary>
        /// Retrieves the module, which holds this member.
        /// </summary>
        public IMetadataModule Module
        {
            get { return _module; }
        }

        #endregion // Properties

        #region IRuntimeAttributable Members

        public RuntimeAttribute[] CustomAttributes
        {
            get
            {
                return _attributes;
            }
        }

        #endregion // IRuntimeAttributable Members
    }
}
