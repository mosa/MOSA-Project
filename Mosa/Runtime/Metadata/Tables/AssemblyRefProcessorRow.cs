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
	public struct AssemblyRefProcessorRow 
    {
		#region Data members

        private uint _processor;

        private TokenTypes _assemblyRef;

		#endregion // Data members

        #region Construction

        public AssemblyRefProcessorRow(uint processor, TokenTypes assemblyRef)
        {
            _processor = processor;
            _assemblyRef = assemblyRef;
        }

        #endregion // Construction

        #region Properties

        public uint Processor
        {
            get { return _processor; }
        }

        public TokenTypes AssemblyRef
        {
            get { return _assemblyRef; }
        }

        #endregion // Properties
	}
}
