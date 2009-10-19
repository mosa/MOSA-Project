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

namespace Mosa.Runtime.Vm
{
    /// <summary>
    /// Contains offset values for a module in the DefaultTypeSystem data structures.
    /// </summary>
    public struct ModuleOffsets
    {
        #region Data members

        /// <summary>
        /// Holds the _stackFrameIndex offsets for a module.
        /// </summary>
        private int _fieldOffset;

        /// <summary>
        /// Holds the method offsets for a module.
        /// </summary>
        private int _methodOffset;

        /// <summary>
        /// Holds the type offsets for a module.
        /// </summary>
        private int _typeOffset;

        /// <summary>
        /// The parameter offsets.
        /// </summary>
        private int _parameterOffset;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="ModuleOffsets"/>.
        /// </summary>
        /// <param name="fieldOffset">The _stackFrameIndex offset in the DefaultTypeSystem.Fields collection.</param>
        /// <param name="methodOffset">The method offset in the DefaultTypeSystem.Methods collection.</param>
        /// <param name="parameterOffset">The parameter offset in the DefaultTypeSystem.Parameters collection.</param>
        /// <param name="typeOffset">The type offset in the DefaultTypeSystem.Types collection.</param>
        public ModuleOffsets(int fieldOffset, int methodOffset, int parameterOffset, int typeOffset)
        {
            _fieldOffset = fieldOffset;
            _methodOffset = methodOffset;
            _parameterOffset = parameterOffset;
            _typeOffset = typeOffset;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Returns the offset into DefaultTypeSystem.Fields for a module.
        /// </summary>
        public int FieldOffset
        {
            get { return _fieldOffset; }
        }

        /// <summary>
        /// Returns the offset into DefaultTypeSystem.Methods for a module.
        /// </summary>
        public int MethodOffset
        {
            get { return _methodOffset; }
        }

        /// <summary>
        /// Returns the offset into DefaultTypeSystem.Parameters for a module.
        /// </summary>
        public int ParameterOffset
        {
            get { return _parameterOffset; }
        }

        /// <summary>
        /// Returns the offset into DefaultTypeSystem.Types for a module.
        /// </summary>
        public int TypeOffset
        {
            get { return _typeOffset; }
        }

        #endregion // Properties
    }
}
