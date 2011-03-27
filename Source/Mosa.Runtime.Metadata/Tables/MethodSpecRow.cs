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
	public struct MethodSpecRow
	{
		#region Data members

		/// <summary>
		/// Holds the index into the method table.
		/// </summary>
		private MetadataToken _method;

		/// <summary>
		/// Holds the index into the blob instantiation.
		/// </summary>
		private TokenTypes _instantiationBlobIdx;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="MethodSpecRow"/>.
		/// </summary>
		/// <param name="method">The method table index of the MethodSpecRow.</param>
		/// <param name="instantiationBlobIdx">The instantiation blob index of the MethodSpecRow.</param>
		public MethodSpecRow(MetadataToken method, TokenTypes instantiationBlobIdx)
		{
			_method = method;
			_instantiationBlobIdx = instantiationBlobIdx;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the method table idx.
		/// </summary>
		/// <value>The method table idx.</value>
		public MetadataToken Method
		{
			get { return _method; }
		}

		/// <summary>
		/// Gets the instantiation BLOB idx.
		/// </summary>
		/// <value>The instantiation BLOB idx.</value>
		public TokenTypes InstantiationBlobIdx
		{
			get { return _instantiationBlobIdx; }
		}

		#endregion // Properties
	}
}
