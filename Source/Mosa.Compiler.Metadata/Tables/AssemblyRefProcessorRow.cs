/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Compiler.Metadata.Tables
{
	/// <summary>
	///
	/// </summary>
	public class AssemblyRefProcessorRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyRefProcessorRow"/> struct.
		/// </summary>
		/// <param name="processor">The processor.</param>
		/// <param name="assemblyRef">The assembly ref.</param>
		public AssemblyRefProcessorRow(uint processor, Token assemblyRef)
		{
			Processor = processor;
			AssemblyRef = assemblyRef;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the processor.
		/// </summary>
		/// <value>The processor.</value>
		public uint Processor { get; private set; }

		/// <summary>
		/// Gets the assembly ref.
		/// </summary>
		/// <value>The assembly ref.</value>
		public Token AssemblyRef { get; private set; }

		#endregion Properties
	}
}