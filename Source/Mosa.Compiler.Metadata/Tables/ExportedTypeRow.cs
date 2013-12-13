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
	public class ExportedTypeRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ExportedTypeRow"/> struct.
		/// </summary>
		/// <param name="flags">The flags.</param>
		/// <param name="typeDef">The type def.</param>
		/// <param name="typeName">Name of the type.</param>
		/// <param name="typeNamespace">The type namespace.</param>
		/// <param name="implementation">The implementation.</param>
		public ExportedTypeRow(TypeAttributes flags, HeapIndexToken typeDef, HeapIndexToken typeName,
								HeapIndexToken typeNamespace, Token implementation)
		{
			Flags = flags;
			TypeDef = typeDef;
			TypeName = typeName;
			TypeNamespace = typeNamespace;
			Implementation = implementation;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public TypeAttributes Flags { get; private set; }

		/// <summary>
		/// Gets the type def.
		/// </summary>
		/// <value>The type def.</value>
		public HeapIndexToken TypeDef { get; private set; }

		/// <summary>
		/// Gets the name of the type.
		/// </summary>
		/// <value>The name of the type.</value>
		public HeapIndexToken TypeName { get; private set; }

		/// <summary>
		/// Gets the type namespace.
		/// </summary>
		/// <value>The type namespace.</value>
		public HeapIndexToken TypeNamespace { get; private set; }

		/// <summary>
		/// Gets the implementation.
		/// </summary>
		/// <value>The implementation.</value>
		public Token Implementation { get; private set; }

		#endregion Properties
	}
}