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
	public struct ConstantRow 
    {
		#region Data members

        private CilElementType _type; // FIXME: Enum?

        private TokenTypes _parent;

        private TokenTypes _valueBlobIdx;

		#endregion // Data members

        #region Construction

        public ConstantRow(CilElementType type, TokenTypes parent, TokenTypes valueBlobIdx)
        {
            _type = type;
            _parent = parent;
            _valueBlobIdx = valueBlobIdx;
        }

        #endregion // Construction

        #region Properties

        public CilElementType Type
        {
            get { return _type; }
        }

        public TokenTypes Parent
        {
            get { return _parent; }
        }

        public TokenTypes ValueBlobIdx
        {
            get { return _valueBlobIdx; }
        }

        #endregion // Properties
	}
}
