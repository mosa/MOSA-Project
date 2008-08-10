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
	public struct FieldLayoutRow {
		#region Data members

        private uint _offset;

        private TokenTypes _field;

		#endregion // Data members

        #region Construction

        public FieldLayoutRow(uint offset, TokenTypes field)
        {
            _offset = offset;
            _field = field;
        }

        #endregion // Construction

        #region Properties

        public uint Offset
        {
            get { return _offset; }
        }

        public TokenTypes Field
        {
            get { return _field; }
        }

        #endregion // Properties
	}
}
