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

using Mono.Cecil;

namespace Mosa.Runtime.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct GenericParamRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private ushort _number;

		/// <summary>
		/// 
		/// </summary>
		private GenericParameterAttributes _flags;

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes _ownerTableIdx;

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes _nameStringIdx;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericParamRow"/> struct.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <param name="flags">The flags.</param>
		/// <param name="ownerTableIdx">The owner table idx.</param>
		/// <param name="nameStringIdx">The name string idx.</param>
		public GenericParamRow(ushort number, GenericParameterAttributes flags, TokenTypes ownerTableIdx, TokenTypes nameStringIdx)
		{
			_number = number;
			_flags = flags;
			_ownerTableIdx = ownerTableIdx;
			_nameStringIdx = nameStringIdx;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the number.
		/// </summary>
		/// <value>The number.</value>
		public ushort Number
		{
			get { return _number; }
		}

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public GenericParameterAttributes Flags
		{
			get { return _flags; }
		}

		/// <summary>
		/// Gets the owner table idx.
		/// </summary>
		/// <value>The owner table idx.</value>
		public TokenTypes OwnerTableIdx
		{
			get { return _ownerTableIdx; }
		}

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
