/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct ConstantRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private CilElementType type; 

		/// <summary>
		/// 
		/// </summary>
		private Token parent;

		/// <summary>
		/// 
		/// </summary>
		private HeapIndexToken value;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ConstantRow"/> struct.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="parent">The parent.</param>
		/// <param name="value">The value BLOB idx.</param>
		public ConstantRow(CilElementType type, Token parent, HeapIndexToken value)
		{
			this.type = type;
			this.parent = parent;
			this.value = value;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>The type.</value>
		public CilElementType Type
		{
			get { return type; }
		}

		/// <summary>
		/// Gets the parent.
		/// </summary>
		/// <value>The parent.</value>
		public Token Parent
		{
			get { return parent; }
		}

		/// <summary>
		/// Gets the value BLOB idx.
		/// </summary>
		/// <value>The value BLOB idx.</value>
		public HeapIndexToken Value
		{
			get { return value; }
		}

		#endregion // Properties
	}
}
