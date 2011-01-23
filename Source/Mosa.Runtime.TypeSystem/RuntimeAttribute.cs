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
		/// The instantiated attribute.
		/// </summary>
		private object attribute;

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
		public RuntimeAttribute(IMetadataProvider metadataProvider, CustomAttributeRow car)
		{
			attribute = null;
			attributeBlob = car.ValueBlobIdx;
			ctor = car.TypeIdx;

			// Retrieve the attribute type
			attribute = CustomAttributeParser.Parse(metadataProvider, attributeBlob, ctorMethod);
			Debug.Assert(null != attribute, @"Failed to load the attribute.");

			//TODO
			//ctorMethod = moduleTypeSystem.GetMethod(ctor);
			ctorMethod = null;
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Retrieves the attribute.
		/// </summary>
		/// <returns>An instance of the attribute.</returns>
		public object GetAttribute()
		{
			return attribute;
		}

		#endregion // Methods

		#region Properties

		/// <summary>
		/// Gets the attribute type.
		/// </summary>
		/// <value>The attribute type.</value>
		public RuntimeType Type
		{
			get
			{
				return ctorMethod.DeclaringType;
			}
		}

		#endregion // Properties

	}
}
