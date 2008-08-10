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
	public struct FieldRVARow {
		#region Data members

        private uint _rva;

        private TokenTypes _fieldTableIdx;

        #endregion // Data members

        #region Construction

        public FieldRVARow(uint rva, TokenTypes fieldTableIdx)
        {
            _rva = rva;
            _fieldTableIdx = fieldTableIdx;
        }

        #endregion // Construction

        #region Properties

        public uint Rva
        {
            get { return _rva; }
        }


        public TokenTypes FieldTableIdx
        {
            get { return _fieldTableIdx; }
        }

        #endregion // Properties
	}
}
