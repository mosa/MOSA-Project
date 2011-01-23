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
	public struct TypeDefRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private TypeAttributes flags;

		/// <summary>
		/// An index into the String heap
		/// </summary>
		private TokenTypes typeNameIdx;

		/// <summary>
		/// An index into the String heap
		/// </summary>
		private TokenTypes typeNamespaceIdx;

		/// <summary>
		/// Index into a <see cref="TypeDefRow"/>, <see cref="TypeRefRow"/>, or <see cref="TypeSpecRow"/> table.
		/// </summary>
		private TokenTypes extends;

		/// <summary>
		/// An index into the <see cref="FieldRow"/>, it marks the first of a contiguous run of Fields
		/// owned by this type.
		/// </summary>
		private TokenTypes fieldList;

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes methodList;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeDefRow"/> struct.
		/// </summary>
		/// <param name="flags">The flags.</param>
		/// <param name="typeNameIdx">The type name idx.</param>
		/// <param name="typeNamespaceIdx">The type namespace idx.</param>
		/// <param name="extends">The extends.</param>
		/// <param name="fieldList">The field list.</param>
		/// <param name="methodList">The method list.</param>
		public TypeDefRow(TypeAttributes flags, TokenTypes typeNameIdx, TokenTypes typeNamespaceIdx,
							TokenTypes extends, TokenTypes fieldList, TokenTypes methodList)
		{
			this.flags = flags;
			this.typeNameIdx = typeNameIdx;
			this.typeNamespaceIdx = typeNamespaceIdx;
			this.extends = extends;
			this.fieldList = fieldList;
			this.methodList = methodList;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets or sets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public TypeAttributes Flags
		{
			get { return flags; }
			set { flags = value; }
		}

		/// <summary>
		/// Gets or sets the type name idx.
		/// </summary>
		/// <value>The type name idx.</value>
		public TokenTypes TypeNameIdx
		{
			get { return typeNameIdx; }
			set { typeNameIdx = value; }
		}

		/// <summary>
		/// Gets or sets the type namespace idx.
		/// </summary>
		/// <value>The type namespace idx.</value>
		public TokenTypes TypeNamespaceIdx
		{
			get { return typeNamespaceIdx; }
			set { typeNamespaceIdx = value; }
		}

		/// <summary>
		/// Gets or sets the extends.
		/// </summary>
		/// <value>The extends.</value>
		public TokenTypes Extends
		{
			get { return extends; }
			set { extends = value; }
		}

		/// <summary>
		/// Gets or sets the field list.
		/// </summary>
		/// <value>The field list.</value>
		public TokenTypes FieldList
		{
			get { return fieldList; }
			set { fieldList = value; }
		}

		/// <summary>
		/// Gets or sets the method list.
		/// </summary>
		/// <value>The method list.</value>
		public TokenTypes MethodList
		{
			get { return methodList; }
			set { methodList = value; }
		}

		#endregion // Properties
	}
}
