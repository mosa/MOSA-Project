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
	public struct ExportedTypeRow {
		#region Data members

        private TypeAttributes _flags;

        private TokenTypes _typeDefTableIdx;

        private TokenTypes _typeNameStringIdx;

        private TokenTypes _typeNamespaceStringIdx;

        private TokenTypes _implementationTableIdx;

		#endregion // Data members

        #region Construction

        public ExportedTypeRow(TypeAttributes flags, TokenTypes typeDefTableIdx, TokenTypes typeNameStringIdx, 
                                TokenTypes typeNamespaceStringIdx, TokenTypes implementationTableIdx)
        {
            _flags = flags;
            _typeDefTableIdx = typeDefTableIdx;
            _typeNameStringIdx = typeNameStringIdx;
            _typeNamespaceStringIdx = typeNamespaceStringIdx;
            _implementationTableIdx = implementationTableIdx;
        }

        #endregion // Construction

        #region Properties

        public TypeAttributes Flags
        {
            get { return _flags; }
        }

        public TokenTypes TypeDefTableIdx
        {
            get { return _typeDefTableIdx; }
        }

        public TokenTypes TypeNameStringIdx
        {
            get { return _typeNameStringIdx; }
        }

        public TokenTypes TypeNamespaceStringIdx
        {
            get { return _typeNamespaceStringIdx; }
        }

        public TokenTypes ImplementationTableIdx
        {
            get { return _implementationTableIdx; }
        }

        #endregion // Properties
	}
}
