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
    public struct TypeDefRow
    {
        #region Data members

        private TypeAttributes _flags;

        private TokenTypes _typeNameIdx;

        private TokenTypes _typeNamespaceIdx;

        private TokenTypes _extends;

        private TokenTypes _fieldList;

        private TokenTypes _methodList;

        #endregion // Data members

        #region Construction

        public TypeDefRow(TypeAttributes flags, TokenTypes typeNameIdx, TokenTypes typeNamespaceIdx, 
                            TokenTypes extends, TokenTypes fieldList, TokenTypes methodList)
        {
            _flags = flags;
            _typeNameIdx = typeNameIdx;
            _typeNamespaceIdx = typeNamespaceIdx;
            _extends = extends;
            _fieldList = fieldList;
            _methodList = methodList;
        }

        #endregion // Construction

        #region Properties

        public TypeAttributes Flags
        {
            get { return _flags; }
            set { _flags = value; }
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

        public TokenTypes Extends
        {
            get { return _extends; }
            set { _extends = value; }
        }

        public TokenTypes FieldList
        {
            get { return _fieldList; }
            set { _fieldList = value; }
        }

        public TokenTypes MethodList
        {
            get { return _methodList; }
            set { _methodList = value; }
        }

        #endregion // Properties
    }
}
