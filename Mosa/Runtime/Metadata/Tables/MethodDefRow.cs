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
using System.Reflection;

namespace Mosa.Runtime.Metadata.Tables
{
	public struct MethodDefRow {
		#region Data members

        private uint _rva;

        private MethodImplAttributes _implFlags;

        private MethodAttributes _flags;

        private TokenTypes _nameStringIdx;

        private TokenTypes _signatureBlobIdx;

        private TokenTypes _paramList;

		#endregion // Data members

        #region Construction

        public MethodDefRow(uint rva, MethodImplAttributes implFlags, MethodAttributes flags, TokenTypes nameStringIdx, 
                                TokenTypes signatureBlobIdx, TokenTypes paramList)
        {
            _rva = rva;
            _implFlags = implFlags;
            _flags = flags;
            _nameStringIdx = nameStringIdx;
            _signatureBlobIdx = signatureBlobIdx;
            _paramList = paramList;
        }

        #endregion // Construction

        #region Properties

        public uint Rva
        {
            get { return _rva; }
        }

        public MethodImplAttributes ImplFlags
        {
            get { return _implFlags; }
        }

        public MethodAttributes Flags
        {
            get { return _flags; }
        }

        public TokenTypes NameStringIdx
        {
            get { return _nameStringIdx; }
        }

        public TokenTypes SignatureBlobIdx
        {
            get { return _signatureBlobIdx; }
        }

        public TokenTypes ParamList
        {
            get { return _paramList; }
        }

        #endregion // Properties
	}
}
