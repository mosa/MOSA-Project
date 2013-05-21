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
	public struct TypeRefRow
	{
		#region Data members

		/// <summary>
		///
		/// </summary>
		private Token resolutionScope;

		/// <summary>
		///
		/// </summary>
		private HeapIndexToken typeNameIdx;

		/// <summary>
		///
		/// </summary>
		private HeapIndexToken typeNamespaceIdx;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeRefRow"/> struct.
		/// </summary>
		/// <param name="resolutionScope">The resolution scope idx.</param>
		/// <param name="typeNameIdx">The type name idx.</param>
		/// <param name="typeNamespaceIdx">The type namespace idx.</param>
		public TypeRefRow(Token resolutionScope, HeapIndexToken typeNameIdx, HeapIndexToken typeNamespaceIdx)
		{
			this.resolutionScope = resolutionScope;
			this.typeNameIdx = typeNameIdx;
			this.typeNamespaceIdx = typeNamespaceIdx;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets or sets the resolution scope idx.
		/// </summary>
		/// <value>The resolution scope idx.</value>
		public Token ResolutionScope
		{
			get { return resolutionScope; }
		}

		/// <summary>
		/// Gets or sets the type name idx.
		/// </summary>
		/// <value>The type name idx.</value>
		public HeapIndexToken TypeNameIdx
		{
			get { return typeNameIdx; }
		}

		/// <summary>
		/// Gets or sets the type namespace idx.
		/// </summary>
		/// <value>The type namespace idx.</value>
		public HeapIndexToken TypeNamespaceIdx
		{
			get { return typeNamespaceIdx; }
		}

		#endregion Properties
	}
}