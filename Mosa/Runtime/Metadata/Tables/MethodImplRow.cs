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
	public struct MethodImplRow {
		#region Data members

        private TokenTypes _classTableIdx;

        private TokenTypes _methodBodyTableIdx;

        private TokenTypes _methodDeclarationTableIdx;

		#endregion // Data members

        #region Construction

        public MethodImplRow(TokenTypes classTableIdx, TokenTypes methodBodyTableIdx, TokenTypes methodDeclarationTableIdx)
        {
            _classTableIdx = classTableIdx;
            _methodBodyTableIdx = methodBodyTableIdx;
            _methodDeclarationTableIdx = methodDeclarationTableIdx;
        }

        #endregion // Construction

        #region Properties

        public TokenTypes ClassTableIdx
        {
            get { return _classTableIdx; }
        }

        public TokenTypes MethodBodyTableIdx
        {
            get { return _methodBodyTableIdx; }
        }

        public TokenTypes MethodDeclarationTableIdx
        {
            get { return _methodDeclarationTableIdx; }
        }

        #endregion // Properties
	}
}
