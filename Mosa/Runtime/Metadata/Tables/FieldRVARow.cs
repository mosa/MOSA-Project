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
using System.IO;

namespace Mosa.Runtime.Metadata.Tables 
{
    /// <summary>
    /// 
    /// </summary>
	public struct FieldRVARow {
		#region Data members

        /// <summary>
        /// 
        /// </summary>
        private uint _rva;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _fieldTableIdx;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldRVARow"/> struct.
        /// </summary>
        /// <param name="rva">The rva.</param>
        /// <param name="fieldTableIdx">The field table idx.</param>
        public FieldRVARow(uint rva, TokenTypes fieldTableIdx)
        {
            _rva = rva;
            _fieldTableIdx = fieldTableIdx;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the rva.
        /// </summary>
        /// <value>The rva.</value>
        public uint Rva
        {
            get { return _rva; }
        }


        /// <summary>
        /// Gets the field table idx.
        /// </summary>
        /// <value>The field table idx.</value>
        public TokenTypes FieldTableIdx
        {
            get { return _fieldTableIdx; }
        }

        #endregion // Properties
	}
}
