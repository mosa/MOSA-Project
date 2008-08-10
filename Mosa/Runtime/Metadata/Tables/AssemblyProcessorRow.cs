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
	public struct AssemblyProcessorRow 
    {
		#region Data members

        private uint _processor;

		#endregion // Data members

        #region Construction

        public AssemblyProcessorRow(uint processor)
        {
            _processor = processor;
        }

        #endregion // Construction

        #region Properties

        public uint Processor
        {
            get { return _processor; }
        }

        #endregion // Properties
	}
}
