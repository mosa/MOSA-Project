/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

namespace Mosa.Compiler.Metadata.Tables
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
		private uint processor;

		/// <summary>
		///
		/// </summary>
		private Token assemblyRef;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyRefProcessorRow"/> struct.
		/// </summary>
		/// <param name="processor">The processor.</param>
		/// <param name="assemblyRef">The assembly ref.</param>
		public AssemblyRefProcessorRow(uint processor, Token assemblyRef)
		{
			this.processor = processor;
			this.assemblyRef = assemblyRef;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the processor.
		/// </summary>
		/// <value>The processor.</value>
		public uint Processor
		{
			get { return processor; }
		}

		/// <summary>
		/// Gets the assembly ref.
		/// </summary>
		/// <value>The assembly ref.</value>
		public Token AssemblyRef
		{
			get { return assemblyRef; }
		}

		#endregion Properties
	}
}