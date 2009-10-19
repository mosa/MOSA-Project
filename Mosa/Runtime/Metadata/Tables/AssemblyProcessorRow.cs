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
using System.IO;

namespace Mosa.Runtime.Metadata.Tables
{
    /// <summary>
    /// 
    /// </summary>
	public struct AssemblyProcessorRow 
    {
		#region Data members

        /// <summary>
        /// 
        /// </summary>
        private uint _processor;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyProcessorRow"/> struct.
        /// </summary>
        /// <param name="processor">The processor.</param>
        public AssemblyProcessorRow(uint processor)
        {
            _processor = processor;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Gets the processor.
        /// </summary>
        /// <value>The processor.</value>
        public uint Processor
        {
            get { return _processor; }
        }

        #endregion // Properties
	}
}
