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
	public struct ModuleRow {
		#region Data members

        private short _generation;

        private TokenTypes _nameStringIdx;

        private TokenTypes _mvidGuidIdx;

        private TokenTypes _encIdGuidIdx;

        private TokenTypes _encBaseIdGuidIdx;

		#endregion // Data members

        #region Construction

        public ModuleRow(short generation, 
                            TokenTypes nameStringIdx, 
                            TokenTypes mvidGuidIdx, 
                            TokenTypes encIdGuidIdx, 
                            TokenTypes encBaseIdGuidIdx)
        {
            _generation = generation;
            _nameStringIdx = nameStringIdx;
            _mvidGuidIdx = mvidGuidIdx;
            _encIdGuidIdx = encIdGuidIdx;
            _encBaseIdGuidIdx = encBaseIdGuidIdx;
        }

        #endregion // Construction

        #region Properties

        public TokenTypes EncBaseIdGuidIdx
        {
            get { return _encBaseIdGuidIdx; }
        }

        public TokenTypes EncIdGuidIdx
        {
            get { return _encIdGuidIdx; }
        }

        public short Generation
        {
            get { return _generation; }
        }

        public TokenTypes MvidGuidIdx
        {
            get { return _mvidGuidIdx; }
        }

        public TokenTypes NameStringIdx
        {
            get { return _nameStringIdx; }
        }

        #endregion // Properties
	}
}
