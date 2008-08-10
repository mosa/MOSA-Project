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
	public struct ClassLayoutRow 
    {
		#region Data members

        private ushort _packingSize;

        private uint _classSize;

        private TokenTypes _parentTypeDefIdx;

		#endregion // Data members

        #region Construction

        public ClassLayoutRow(ushort packingSize, uint classSize, TokenTypes parentTypeDefIdx)
        {
            _packingSize = packingSize;
            _classSize = classSize;
            _parentTypeDefIdx = parentTypeDefIdx;
        }

        #endregion // Construction

        #region Properties

        public ushort PackingSize
        {
            get { return _packingSize; }
        }

        public uint ClassSize
        {
            get { return _classSize; }
        }

        public TokenTypes ParentTypeDefIdx
        {
            get { return _parentTypeDefIdx; }
        }

        #endregion // Properties
	}
}
