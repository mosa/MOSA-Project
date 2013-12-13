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
	public class TypeRefRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeRefRow"/> class.
		/// </summary>
		/// <param name="resolutionScope">The resolution scope.</param>
		/// <param name="typeName">Name of the type.</param>
		/// <param name="typeNamespace">The type namespace.</param>
		public TypeRefRow(Token resolutionScope, HeapIndexToken typeName, HeapIndexToken typeNamespace)
		{
			ResolutionScope = resolutionScope;
			TypeName = typeName;
			TypeNamespace = typeNamespace;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the resolution scope.
		/// </summary>
		/// <value>
		/// The resolution scope.
		/// </value>
		public Token ResolutionScope { get; private set; }

		/// <summary>
		/// Gets the name of the type.
		/// </summary>
		/// <value>
		/// The name of the type.
		/// </value>
		public HeapIndexToken TypeName { get; private set; }

		/// <summary>
		/// Gets the type namespace.
		/// </summary>
		/// <value>
		/// The type namespace.
		/// </value>
		public HeapIndexToken TypeNamespace { get; private set; }

		#endregion Properties
	}
}