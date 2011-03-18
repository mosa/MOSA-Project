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
	public struct ExportedTypeRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private TypeAttributes _flags;

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes _typeDefTableIdx;

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes _typeNameStringIdx;

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes _typeNamespaceStringIdx;

		/// <summary>
		/// 
		/// </summary>
		private TokenTypes _implementationTableIdx;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ExportedTypeRow"/> struct.
		/// </summary>
		/// <param name="flags">The flags.</param>
		/// <param name="typeDefTableIdx">The type def table idx.</param>
		/// <param name="typeNameStringIdx">The type name string idx.</param>
		/// <param name="typeNamespaceStringIdx">The type namespace string idx.</param>
		/// <param name="implementationTableIdx">The implementation table idx.</param>
		public ExportedTypeRow(TypeAttributes flags, TokenTypes typeDefTableIdx, TokenTypes typeNameStringIdx,
								TokenTypes typeNamespaceStringIdx, TokenTypes implementationTableIdx)
		{
			_flags = flags;
			_typeDefTableIdx = typeDefTableIdx;
			_typeNameStringIdx = typeNameStringIdx;
			_typeNamespaceStringIdx = typeNamespaceStringIdx;
			_implementationTableIdx = implementationTableIdx;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public TypeAttributes Flags
		{
			get { return _flags; }
		}

		/// <summary>
		/// Gets the type def table idx.
		/// </summary>
		/// <value>The type def table idx.</value>
		public TokenTypes TypeDefTableIdx
		{
			get { return _typeDefTableIdx; }
		}

		/// <summary>
		/// Gets the type name string idx.
		/// </summary>
		/// <value>The type name string idx.</value>
		public TokenTypes TypeNameStringIdx
		{
			get { return _typeNameStringIdx; }
		}

		/// <summary>
		/// Gets the type namespace string idx.
		/// </summary>
		/// <value>The type namespace string idx.</value>
		public TokenTypes TypeNamespaceStringIdx
		{
			get { return _typeNamespaceStringIdx; }
		}

		/// <summary>
		/// Gets the implementation table idx.
		/// </summary>
		/// <value>The implementation table idx.</value>
		public TokenTypes ImplementationTableIdx
		{
			get { return _implementationTableIdx; }
		}

		#endregion // Properties
	}
}
