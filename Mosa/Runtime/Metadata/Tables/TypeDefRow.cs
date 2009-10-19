/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.Metadata.Tables
{
    /// <summary>
    /// 
    /// </summary>
    public struct TypeDefRow
    {
        #region Data members

        /// <summary>
        /// 
        /// </summary>
        private TypeAttributes _flags;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _typeNameIdx;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _typeNamespaceIdx;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _extends;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _fieldList;

        /// <summary>
        /// 
        /// </summary>
        private TokenTypes _methodList;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDefRow"/> struct.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="typeNameIdx">The type name idx.</param>
        /// <param name="typeNamespaceIdx">The type namespace idx.</param>
        /// <param name="extends">The extends.</param>
        /// <param name="fieldList">The field list.</param>
        /// <param name="methodList">The method list.</param>
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

        /// <summary>
        /// Gets or sets the flags.
        /// </summary>
        /// <value>The flags.</value>
        public TypeAttributes Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }

        /// <summary>
        /// Gets or sets the type name idx.
        /// </summary>
        /// <value>The type name idx.</value>
        public TokenTypes TypeNameIdx
        {
            get { return _typeNameIdx; }
            set { _typeNameIdx = value; }
        }

        /// <summary>
        /// Gets or sets the type namespace idx.
        /// </summary>
        /// <value>The type namespace idx.</value>
        public TokenTypes TypeNamespaceIdx
        {
            get { return _typeNamespaceIdx; }
            set { _typeNamespaceIdx = value; }
        }

        /// <summary>
        /// Gets or sets the extends.
        /// </summary>
        /// <value>The extends.</value>
        public TokenTypes Extends
        {
            get { return _extends; }
            set { _extends = value; }
        }

        /// <summary>
        /// Gets or sets the field list.
        /// </summary>
        /// <value>The field list.</value>
        public TokenTypes FieldList
        {
            get { return _fieldList; }
            set { _fieldList = value; }
        }

        /// <summary>
        /// Gets or sets the method list.
        /// </summary>
        /// <value>The method list.</value>
        public TokenTypes MethodList
        {
            get { return _methodList; }
            set { _methodList = value; }
        }

        #endregion // Properties
    }
}
