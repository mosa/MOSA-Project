/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;

namespace Mosa.Runtime.Vm 
{
	/// <summary>
	/// Represents an attribute in runtime type information.
	/// </summary>
	public class RuntimeAttribute
    {
		#region Data members

		/// <summary>
		/// The instantiated attribute.
		/// </summary>
		private Attribute _attribute;

		/// <summary>
		/// Holds the ctor of the attribute type to invoke.
		/// </summary>
		private TokenTypes _ctor;

		/// <summary>
		/// Specifies the blob, which contains the attribute initialization.
		/// </summary>
		private TokenTypes _attributeBlob;

		#endregion // Data members

		#region Methods

		/// <summary>
		/// Retrieves the attribute.
		/// </summary>
		/// <param name="module">The module, which defined the attribute.</param>
		/// <returns>An instance of the attribute.</returns>
		public Attribute GetAttribute(IMetadataModule module)
		{
			// Skip over attribute initialization, if we already initialized the attribute
			if (null != _attribute)
				return _attribute;

			//
			// FIXME: Initialize the attribute:
			// - Find the type from the method
			// - Invoke the runtimes newobj handler with the type
			// - Invoke the constructor with parameters from the value blob
			// - Set named parameters via the appropriate property setters
			//

			return _attribute;
		}

		/// <summary>
		/// Populates the <see cref="RuntimeAttribute"/> with the values in <paramref name="car"/>.
		/// </summary>
		/// <param name="car">The custom attribute row from metadata.</param>
		public RuntimeAttribute(CustomAttributeRow car)
		{
			_attribute = null;
			_ctor = car.TypeIdx;
			_attributeBlob = car.ValueBlobIdx;
		}

		#endregion // Methods
	}
}
