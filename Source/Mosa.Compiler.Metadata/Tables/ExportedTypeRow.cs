/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */



namespace Mosa.Runtime.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct ExportedTypeRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private TypeAttributes flags;

		/// <summary>
		/// 
		/// </summary>
		private HeapIndexToken typeDef;

		/// <summary>
		/// 
		/// </summary>
		private HeapIndexToken typeName;

		/// <summary>
		/// 
		/// </summary>
		private HeapIndexToken typeNamespace;

		/// <summary>
		/// 
		/// </summary>
		private Token implementation;

		#endregion // Data members

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
			this.flags = flags;
			this.typeDef = typeDef;
			this.typeName = typeName;
			this.typeNamespace = typeNamespace;
			this.implementation = implementation;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public TypeAttributes Flags
		{
			get { return flags; }
		}

		/// <summary>
		/// Gets the type def.
		/// </summary>
		/// <value>The type def.</value>
		public HeapIndexToken TypeDef
		{
			get { return typeDef; }
		}

		/// <summary>
		/// Gets the name of the type.
		/// </summary>
		/// <value>The name of the type.</value>
		public HeapIndexToken TypeName
		{
			get { return typeName; }
		}

		/// <summary>
		/// Gets the type namespace.
		/// </summary>
		/// <value>The type namespace.</value>
		public HeapIndexToken TypeNamespace
		{
			get { return typeNamespace; }
		}

		/// <summary>
		/// Gets the implementation.
		/// </summary>
		/// <value>The implementation.</value>
		public Token Implementation
		{
			get { return implementation; }
		}

		#endregion // Properties
	}
}
