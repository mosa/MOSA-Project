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
	public struct ModuleRefRow {
		#region Data members

        private TokenTypes _nameStringIdx;

		#endregion // Data members

        #region Construction

        public ModuleRefRow(TokenTypes nameStringIdx)
        {
            _nameStringIdx = nameStringIdx;
        }

        #endregion // Construction

        #region Properties

        public TokenTypes NameStringIdx
        {
            get { return _nameStringIdx; }
        }

        #endregion // Properties
	}
}
