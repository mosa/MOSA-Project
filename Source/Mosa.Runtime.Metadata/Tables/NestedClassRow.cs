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

namespace Mosa.Runtime.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct NestedClassRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private MetadataToken nestedClass;

		/// <summary>
		/// 
		/// </summary>
		private MetadataToken enclosingClass;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="NestedClassRow"/> struct.
		/// </summary>
		/// <param name="nestedClassTableIdx">The nested class table idx.</param>
		/// <param name="enclosingClassTableIdx">The enclosing class table idx.</param>
		public NestedClassRow(MetadataToken nestedClass, MetadataToken enclosingClass)
		{
			this.nestedClass = nestedClass;
			this.enclosingClass = enclosingClass;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the nested class.
		/// </summary>
		/// <value>The nested class.</value>
		public MetadataToken NestedClass
		{
			get { return nestedClass; }
		}

		/// <summary>
		/// Gets the enclosing class.
		/// </summary>
		/// <value>The enclosing class .</value>
		public MetadataToken EnclosingClass
		{
			get { return enclosingClass; }
		}

		#endregion // Properties
	}
}
