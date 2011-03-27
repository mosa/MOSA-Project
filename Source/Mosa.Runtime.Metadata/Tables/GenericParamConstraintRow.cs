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
	public struct GenericParamConstraintRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private MetadataToken _owner;

		/// <summary>
		/// 
		/// </summary>
		private MetadataToken _constraint;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericParamConstraintRow"/> struct.
		/// </summary>
		/// <param name="owner">The owner table idx.</param>
		/// <param name="constraint">The constraint table idx.</param>
		public GenericParamConstraintRow(MetadataToken owner, MetadataToken constraint)
		{
			_owner = owner;
			_constraint = constraint;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the owner table idx.
		/// </summary>
		/// <value>The owner table idx.</value>
		public MetadataToken Owner
		{
			get { return _owner; }
		}

		/// <summary>
		/// Gets the constraint table idx.
		/// </summary>
		/// <value>The constraint table idx.</value>
		public MetadataToken Constraint
		{
			get { return _constraint; }
		}

		#endregion // Properties

	}
}
