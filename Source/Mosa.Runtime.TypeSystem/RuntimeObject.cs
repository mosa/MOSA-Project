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

using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.TypeSystem
{
	/// <summary>
	/// Base class of all runtime type system objects.
	/// </summary>
	public abstract class RuntimeObject
	{
		#region Data members

		/// <summary>
		/// Holds the token of the object.
		/// </summary>
		private readonly Token token;

		/// <summary>
		/// Holds the module from which this object originated
		/// </summary>
		private readonly ITypeModule module;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="RuntimeObject"/>.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="token">The runtime token of this metadata.</param>
		protected RuntimeObject(ITypeModule module, Token token)
		{
			this.module = module;
			this.token = token;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Retrieves the token of the object.
		/// </summary>
		public Token Token
		{
			get { return token; }
		}

		/// <summary>
		/// Retrieves the module from which this object originated
		/// </summary>
		/// <value>The module.</value>
		public ITypeModule Module
		{
			get { return module; }
		}

		#endregion // Properties
	}
}
