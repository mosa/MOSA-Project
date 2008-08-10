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
    public struct StandAloneSigRow 
    {
		#region Data members

        private TokenTypes _signatureBlobIdx;

		#endregion // Data members

        #region Construction

        public StandAloneSigRow(TokenTypes signatureBlobIdx)
        {
            _signatureBlobIdx = signatureBlobIdx;
        }

        #endregion // Construction

        #region Properties

        public TokenTypes SignatureBlobIdx
        {
            get { return _signatureBlobIdx; }
        }

        #endregion // Properties
	}
}
