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
	public class ImplMapRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ImplMapRow" /> struct.
		/// </summary>
		/// <param name="mappingFlags">The mapping flags.</param>
		/// <param name="memberForwarded">The member forwarded table.</param>
		/// <param name="importNameString">The import name string.</param>
		/// <param name="importScopeTable">The import scope table.</param>
		public ImplMapRow(PInvokeAttributes mappingFlags, Token memberForwarded,
			HeapIndexToken importNameString, Token importScopeTable)
		{
			MappingFlags = mappingFlags;
			MemberForwarded = memberForwarded;
			ImportNameString = importNameString;
			ImportScopeTable = importScopeTable;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the mapping flags.
		/// </summary>
		/// <value>The mapping flags.</value>
		public PInvokeAttributes MappingFlags { get; private set; }

		/// <summary>
		/// Gets the member forwarded.
		/// </summary>
		/// <value>The member forwarded.</value>
		public Token MemberForwarded { get; private set; }

		/// <summary>
		/// Gets the import name string.
		/// </summary>
		/// <value>
		/// The import name string.
		/// </value>
		public HeapIndexToken ImportNameString { get; private set; }

		/// <summary>
		/// Gets the import scope table.
		/// </summary>
		/// <value>
		/// The import scope table.
		/// </value>
		public Token ImportScopeTable { get; private set; }

		#endregion Properties
	}
}