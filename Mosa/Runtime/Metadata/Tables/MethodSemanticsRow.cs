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
	public struct MethodSemanticsRow {
		#region Data members

        private MethodSemanticsAttributes _semantics;

        private TokenTypes _methodTableIdx;

        private TokenTypes _associationTableIdx;

        #endregion // Data members

        #region Construction

        public MethodSemanticsRow(MethodSemanticsAttributes semantics, TokenTypes methodTableIdx,
                                    TokenTypes associationTableIdx)
        {
            _semantics = semantics;
            _methodTableIdx = methodTableIdx;
            _associationTableIdx = associationTableIdx;
        }

        #endregion // Construction

        #region Properties

        public MethodSemanticsAttributes Semantics
        {
            get { return _semantics; }
        }

        public TokenTypes MethodTableIdx
        {
            get { return _methodTableIdx; }
        }

        public TokenTypes AssociationTableIdx
        {
            get { return _associationTableIdx; }
        }

        #endregion // Properties
	}
}
