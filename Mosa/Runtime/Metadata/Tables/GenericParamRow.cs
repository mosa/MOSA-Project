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
	public struct GenericParamRow {
		#region Data members

        private ushort _number;

        private GenericParamAttributes _flags;

        private TokenTypes _ownerTableIdx;

        private TokenTypes _nameStringIdx;

		#endregion // Data members

        #region Construction

        public GenericParamRow(ushort number, GenericParamAttributes flags, TokenTypes ownerTableIdx, 
            TokenTypes nameStringIdx)
        {
            _number = number;
            _flags = flags;
            _ownerTableIdx = ownerTableIdx;
            _nameStringIdx = nameStringIdx;
        }

        #endregion // Construction

        #region Properties

        public ushort Number
        {
            get { return _number; }
        }

        public GenericParamAttributes Flags
        {
            get { return _flags; }
        }

        public TokenTypes OwnerTableIdx
        {
            get { return _ownerTableIdx; }
        }

        public TokenTypes NameStringIdx
        {
            get { return _nameStringIdx; }
        }

        #endregion // Properties
	}
}
