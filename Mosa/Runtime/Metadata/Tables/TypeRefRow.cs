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

namespace Mosa.Runtime.Metadata.Tables
{
    public struct TypeRefRow
    {
        #region Data members

        private TokenTypes _resolutionScopeIdx;

        private TokenTypes _typeNameIdx;

        private TokenTypes _typeNamespaceIdx;

        #endregion // Data members

        #region Construction

        public TypeRefRow(TokenTypes resolutionScopeIdx, TokenTypes typeNameIdx, TokenTypes typeNamespaceIdx)
        {
            _resolutionScopeIdx = resolutionScopeIdx;
            _typeNameIdx = typeNameIdx;
            _typeNamespaceIdx = typeNamespaceIdx;
        }

        #endregion // Construction

        #region Properties

        public TokenTypes ResolutionScopeIdx
        {
            get { return _resolutionScopeIdx; }
            set { _resolutionScopeIdx = value; }
        }

        public TokenTypes TypeNameIdx
        {
            get { return _typeNameIdx; }
            set { _typeNameIdx = value; }
        }

        public TokenTypes TypeNamespaceIdx
        {
            get { return _typeNamespaceIdx; }
            set { _typeNamespaceIdx = value; }
        }

        #endregion // Properties
    }
}
