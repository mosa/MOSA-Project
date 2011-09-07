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
	public struct ImplMapRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private PInvokeAttributes _mappingFlags;

		/// <summary>
		/// 
		/// </summary>
		private Token _memberForwardedTableIdx;

		/// <summary>
		/// 
		/// </summary>
		private HeapIndexToken _importNameStringIdx;

		/// <summary>
		/// 
		/// </summary>
		private Token _importScopeTableIdx;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ImplMapRow"/> struct.
		/// </summary>
		/// <param name="mappingFlags">The mapping flags.</param>
		/// <param name="memberForwardedTableIdx">The member forwarded table idx.</param>
		/// <param name="importNameStringIdx">The import name string idx.</param>
		/// <param name="importScopeTableIdx">The import scope table idx.</param>
		public ImplMapRow(PInvokeAttributes mappingFlags, Token memberForwardedTableIdx,
			HeapIndexToken importNameStringIdx, Token importScopeTableIdx)
		{
			_mappingFlags = mappingFlags;
			_memberForwardedTableIdx = memberForwardedTableIdx;
			_importNameStringIdx = importNameStringIdx;
			_importScopeTableIdx = importScopeTableIdx;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the mapping flags.
		/// </summary>
		/// <value>The mapping flags.</value>
		public PInvokeAttributes MappingFlags
		{
			get { return _mappingFlags; }
		}

		/// <summary>
		/// Gets the member forwarded.
		/// </summary>
		/// <value>The member forwarded.</value>
		public Token MemberForwarded
		{
			get { return _memberForwardedTableIdx; }
		}

		/// <summary>
		/// Gets the import name string idx.
		/// </summary>
		/// <value>The import name string idx.</value>
		public HeapIndexToken ImportNameStringIdx
		{
			get { return _importNameStringIdx; }
		}

		/// <summary>
		/// Gets the import scope table idx.
		/// </summary>
		/// <value>The import scope table idx.</value>
		public Token ImportScopeTableIdx
		{
			get { return _importScopeTableIdx; }
		}

		#endregion // Properties
	}
}
