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
	public class TypeDefRow
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeDefRow" /> struct.
		/// </summary>
		/// <param name="flags">The flags.</param>
		/// <param name="typeName">Name of the type.</param>
		/// <param name="typeNamespace">The type namespace.</param>
		/// <param name="extends">The extends.</param>
		/// <param name="fieldList">The field list.</param>
		/// <param name="methodList">The method list.</param>
		public TypeDefRow(TypeAttributes flags, HeapIndexToken typeName, HeapIndexToken typeNamespace,
							Token extends, Token fieldList, Token methodList)
		{
			Flags = flags;
			TypeName = typeName;
			TypeNamespace = typeNamespace;
			Extends = extends;
			FieldList = fieldList;
			MethodList = methodList;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets or sets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public TypeAttributes Flags { get; private set; }

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

		/// <summary>
		/// Gets or sets the extends.
		/// </summary>
		/// <value>The extends.</value>
		public Token Extends { get; private set; }

		/// <summary>
		/// Gets or sets the field list.
		/// </summary>
		/// <value>The field list.</value>
		public Token FieldList { get; private set; }

		/// <summary>
		/// Gets or sets the method list.
		/// </summary>
		/// <value>The method list.</value>
		public Token MethodList { get; private set; }

		#endregion Properties
	}
}