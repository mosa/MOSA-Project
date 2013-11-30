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
	public class ModuleRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ModuleRow" /> struct.
		/// </summary>
		/// <param name="generation">The generation.</param>
		/// <param name="nameString">The name string.</param>
		/// <param name="mvidGuid">The mvid unique identifier.</param>
		/// <param name="encIdGuid">The enc identifier unique identifier.</param>
		/// <param name="encBaseIdGuid">The enc base identifier unique identifier.</param>
		public ModuleRow(ushort generation,
							HeapIndexToken nameString,
							HeapIndexToken mvidGuid,
							HeapIndexToken encIdGuid,
							HeapIndexToken encBaseIdGuid)
		{
			Generation = generation;
			NameString = nameString;
			MvidGuid = mvidGuid;
			EncIdGuid = encIdGuid;
			EncBaseIdGuid = encBaseIdGuid;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the enc base identifier unique identifier.
		/// </summary>
		/// <value>
		/// The enc base identifier unique identifier.
		/// </value>
		public HeapIndexToken EncBaseIdGuid { get; private set; }

		/// <summary>
		/// Gets the enc identifier unique identifier.
		/// </summary>
		/// <value>
		/// The enc identifier unique identifier.
		/// </value>
		public HeapIndexToken EncIdGuid { get; private set; }

		/// <summary>
		/// Gets the generation.
		/// </summary>
		/// <value>The generation.</value>
		public ushort Generation { get; private set; }

		/// <summary>
		/// Gets the mvid unique identifier.
		/// </summary>
		/// <value>
		/// The mvid unique identifier.
		/// </value>
		public HeapIndexToken MvidGuid { get; private set; }

		/// <summary>
		/// Gets the name string.
		/// </summary>
		/// <value>
		/// The name string.
		/// </value>
		public HeapIndexToken NameString { get; private set; }

		#endregion Properties
	}
}