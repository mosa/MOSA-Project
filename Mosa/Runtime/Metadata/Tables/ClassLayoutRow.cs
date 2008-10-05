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
using System.IO;

namespace Mosa.Runtime.Metadata.Tables
{
    /// <summary>
    /// 
    /// </summary>
	public struct ClassLayoutRow 
    {
		#region Data members

        /// <summary>
        /// 
        /// </summary>
        private ushort _packingSize;

        /// <summary>
        /// 
        /// </summary>
        private uint _classSize;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _parentTypeDefIdx;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassLayoutRow"/> struct.
        /// </summary>
        /// <param name="packingSize">Size of the packing.</param>
        /// <param name="classSize">Size of the class.</param>
        /// <param name="parentTypeDefIdx">The parent type def idx.</param>
        public ClassLayoutRow(ushort packingSize, uint classSize, TokenTypes parentTypeDefIdx)
        {
            _packingSize = packingSize;
            _classSize = classSize;
            _parentTypeDefIdx = parentTypeDefIdx;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the size of the packing.
        /// </summary>
        /// <value>The size of the packing.</value>
        public ushort PackingSize
        {
            get { return _packingSize; }
        }

        /// <summary>
        /// Gets the size of the class.
        /// </summary>
        /// <value>The size of the class.</value>
        public uint ClassSize
        {
            get { return _classSize; }
        }

        /// <summary>
        /// Gets the parent type def idx.
        /// </summary>
        /// <value>The parent type def idx.</value>
        public TokenTypes ParentTypeDefIdx
        {
            get { return _parentTypeDefIdx; }
        }

        #endregion // Properties
	}
}
