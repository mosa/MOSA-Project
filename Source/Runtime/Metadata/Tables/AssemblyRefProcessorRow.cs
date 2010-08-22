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
	public struct AssemblyRefProcessorRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private uint _processor;

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes _assemblyRef;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyRefProcessorRow"/> struct.
		/// </summary>
		/// <param name="processor">The processor.</param>
		/// <param name="assemblyRef">The assembly ref.</param>
		public AssemblyRefProcessorRow(uint processor, TokenTypes assemblyRef)
		{
			_processor = processor;
			_assemblyRef = assemblyRef;
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

		/// <summary>
		/// Gets the assembly ref.
		/// </summary>
		/// <value>The assembly ref.</value>
		public TokenTypes AssemblyRef
		{
			get { return _assemblyRef; }
		}

		#endregion // Properties
	}
}
