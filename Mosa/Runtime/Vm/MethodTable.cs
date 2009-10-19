/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

namespace Mosa.Runtime.Vm
{
    /// <summary>
    /// An instance of this table is associated with each runtime type. Actually it has a variable length, where
    /// the first two slots provide type information and all following slots 
    /// </summary>
    internal abstract class MethodTable
    {
        /// <summary>
        /// Holds the type information corresponding to the full type represented by this object.
        /// </summary>
        private RuntimeType _fullType;

        /// <summary>
        /// The type implemented by this method table.
        /// </summary>
        private RuntimeType _thisType;

        /// <summary>
        /// The actual table of method addresses. These are filled with the JIT stub initially and
        /// replaced upon jit compilation with the respective addresses.
        /// </summary>
        private IntPtr[] _methods;

        /// <summary>
        /// Gets or sets the methods.
        /// </summary>
        /// <value>The methods.</value>
        public IntPtr[] Methods
        {
            get { return _methods; }
            set { _methods = value; }
        }

        /// <summary>
        /// Gets or sets the full type.
        /// </summary>
        /// <value>The full type.</value>
        internal RuntimeType FullType
        {
            get { return _fullType; }
            set { _fullType = value; }
        }

        /// <summary>
        /// Gets or sets the type of the this.
        /// </summary>
        /// <value>The type of the this.</value>
        internal RuntimeType ThisType
        {
            get { return _thisType; }
            set { _thisType = value; }
        }
    }
}
