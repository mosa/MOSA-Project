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
	public struct ModuleRefRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes _nameStringIdx;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ModuleRefRow"/> struct.
		/// </summary>
		/// <param name="nameStringIdx">The name string idx.</param>
		public ModuleRefRow(TokenTypes nameStringIdx)
		{
			_nameStringIdx = nameStringIdx;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the name string idx.
		/// </summary>
		/// <value>The name string idx.</value>
		public TokenTypes NameStringIdx
		{
			get { return _nameStringIdx; }
		}

		#endregion // Properties
	}
}
