/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Runtime.Metadata;

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
		private HeapIndexToken typeNameIdx;

		/// <summary>
		/// An index into the String heap
		/// </summary>
		private HeapIndexToken typeNamespaceIdx;

		/// <summary>
		/// Index into a <see cref="TypeDefRow"/>, <see cref="TypeRefRow"/>, or <see cref="TypeSpecRow"/> table.
		/// </summary>
		private Token extends;

		/// <summary>
		/// An index into the <see cref="FieldRow"/>, it marks the first of a contiguous run of Fields
		/// owned by this type.
		/// </summary>
		private Token fieldList;

		/// <summary>
		/// 
		/// </summary>
		private Token methodList;

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
		public TypeDefRow(TypeAttributes flags, HeapIndexToken typeNameIdx, HeapIndexToken typeNamespaceIdx,
							Token extends, Token fieldList, Token methodList)
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

		/// <summary>
		/// Gets or sets the extends.
		/// </summary>
		/// <value>The extends.</value>
		public Token Extends
		{
			get { return extends; }
		}

		/// <summary>
		/// Gets or sets the field list.
		/// </summary>
		/// <value>The field list.</value>
		public Token FieldList
		{
			get { return fieldList; }
		}

		/// <summary>
		/// Gets or sets the method list.
		/// </summary>
		/// <value>The method list.</value>
		public Token MethodList
		{
			get { return methodList; }
		}

		#endregion // Properties
	}
}
