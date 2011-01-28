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
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata.Tables;

namespace Mosa.Runtime.TypeSystem
{
	/// <summary>
	/// Represents an attribute in runtime type information.
	/// </summary>
	public class RuntimeAttribute
	{
		#region Data members
		
		/// <summary>
		/// Specifies the blob, which contains the attribute initialization.
		/// </summary>
		private TokenTypes attributeBlob;

		/// <summary>
		/// Holds the ctor of the attribute type to invoke.
		/// </summary>
		private TokenTypes ctor;

		/// <summary>
		/// Holds the ctor method of the attribute type.
		/// </summary>
		private RuntimeMethod ctorMethod;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Populates the <see cref="RuntimeAttribute"/> with the values in <paramref name="car"/>.
		/// </summary>
		/// <param name="metadataProvider">The metadata provider.</param>
		/// <param name="car">The custom attribute row from metadata.</param>
		public RuntimeAttribute(CustomAttributeRow customAttributeRow, RuntimeMethod ctorMethod)
		{
			this.ctorMethod = ctorMethod;		
			this.attributeBlob = customAttributeRow.ValueBlobIdx; // NEVER USED
			this.ctor = customAttributeRow.TypeIdx; // NEVER USED
		}

		#endregion // Construction

		#region Methods

		#endregion // Methods

		#region Properties

		/// <summary>
		/// Gets the runtime type.
		/// </summary>
		/// <value>The runtime type.</value>
		public RuntimeType Type { get { return ctorMethod.DeclaringType; } }

		/// <summary>
		/// Gets the ctor method.
		/// </summary>
		/// <value>The ctor method.</value>
		public RuntimeMethod CtorMethod { get { return ctorMethod; } }

		#endregion // Properties

	}
}
