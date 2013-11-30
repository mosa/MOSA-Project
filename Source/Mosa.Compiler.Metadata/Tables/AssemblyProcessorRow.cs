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
	public class AssemblyProcessorRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblyProcessorRow"/> struct.
		/// </summary>
		/// <param name="processor">The processor.</param>
		public AssemblyProcessorRow(uint processor)
		{
			Processor = processor;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the processor.
		/// </summary>
		/// <value>The processor.</value>
		public uint Processor { get; private set; }

		#endregion Properties
	}
}