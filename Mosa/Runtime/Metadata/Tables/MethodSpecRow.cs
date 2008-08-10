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
	public struct MethodSpecRow {
		#region Data members

        /// <summary>
        /// Holds the index into the method table.
        /// </summary>
		private TokenTypes _methodTableIdx;

        /// <summary>
        /// Holds the index into the blob instantiation.
        /// </summary>
		private TokenTypes _instantiationBlobIdx;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="MethodSpecRow"/>.
        /// </summary>
        /// <param name="methodTable">The method table index of the MethodSpecRow.</param>
        /// <param name="instantiation">The instantiation blob index of the MethodSpecRow.</param>
        public MethodSpecRow(TokenTypes methodTableIdx, TokenTypes instantiationBlobIdx)
        {
            _methodTableIdx = methodTableIdx;
            _instantiationBlobIdx = instantiationBlobIdx;
        }

        #endregion // Construction

        #region Properties

        public TokenTypes MethodTableIdx
        {
            get { return _methodTableIdx; }
        }

        public TokenTypes InstantiationBlobIdx
        {
            get { return _instantiationBlobIdx; }
        }

        #endregion // Properties
	}
}
