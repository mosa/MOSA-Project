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
using Mosa.Runtime.Metadata.Blobs;
using Mosa.Runtime.Metadata.Signatures;
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

		/// <summary>
		/// Holds the static instance of the runtime.
		/// </summary>
		protected IModuleTypeSystem moduleTypeSystem;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Populates the <see cref="RuntimeAttribute"/> with the values in <paramref name="car"/>.
		/// </summary>
		/// <param name="moduleTypeSystem">The module type system.</param>
		/// <param name="car">The custom attribute row from metadata.</param>
		public RuntimeAttribute(IModuleTypeSystem moduleTypeSystem, CustomAttributeRow car)
		{
			attribute = null;
			attributeBlob = car.ValueBlobIdx;
			ctor = car.TypeIdx;
			this.moduleTypeSystem = moduleTypeSystem;
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Retrieves the attribute.
		/// </summary>
		/// <returns>An instance of the attribute.</returns>
		public object GetAttribute()
		{
			// Skip over attribute initialization, if we already initialized the attribute
			if (null != attribute)
				return attribute;

			// Retrieve the attribute type
			attribute = CustomAttributeParser.Parse(moduleTypeSystem.MetadataModule, attributeBlob, ctorMethod);
			Debug.Assert(null != attribute, @"Failed to load the attribute.");

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
				if (ctorMethod == null)
					LocateAttributeCtorMethod();

				return ctorMethod.DeclaringType;
			}
		}

		#endregion // Properties

		#region Internals

		/// <summary>
		/// Locates the attribute ctor method.
		/// </summary>
		private void LocateAttributeCtorMethod()
		{
			ctorMethod = moduleTypeSystem.GetMethod(ctor);
			Debug.Assert(null != ctorMethod);
		}

		#endregion // Internals
	}
}
