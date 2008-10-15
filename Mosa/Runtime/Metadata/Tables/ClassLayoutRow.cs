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
        private short _packingSize;

        /// <summary>
        /// 
        /// </summary>
        private int _classSize;

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
        public ClassLayoutRow(short packingSize, int classSize, TokenTypes parentTypeDefIdx)
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
        public short PackingSize
        {
            get { return _packingSize; }
        }

        /// <summary>
        /// Gets the size of the class.
        /// </summary>
        /// <value>The size of the class.</value>
        public int ClassSize
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
