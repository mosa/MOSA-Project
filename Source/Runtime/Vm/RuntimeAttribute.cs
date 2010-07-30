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

using Mosa.Runtime.Loader;
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
		private object _attribute;

        /// <summary>
        /// Specifies the blob, which contains the attribute initialization.
        /// </summary>
        private TokenTypes _attributeBlob;

        /// <summary>
		/// Holds the ctor of the attribute type to invoke.
		/// </summary>
		private TokenTypes _ctor;

        /// <summary>
        /// Holds the ctor method of the attribute type.
        /// </summary>
        private RuntimeMethod _ctorMethod;

        /// <summary>
        /// Holds the metadata module defining the attribute instance.
        /// </summary>
        private IMetadataModule _module;

		#endregion // Data members

        #region Construction

        /// <summary>
        /// Populates the <see cref="RuntimeAttribute"/> with the values in <paramref name="car"/>.
        /// </summary>
        /// <param name="module">The metadata module, which defines the attribute.</param>
        /// <param name="car">The custom attribute row from metadata.</param>
        public RuntimeAttribute(IMetadataModule module, CustomAttributeRow car)
        {
            _attribute = null;
            _attributeBlob = car.ValueBlobIdx;
            _ctor = car.TypeIdx;
            _module = module;
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
			if (null != _attribute)
				return _attribute;

            // Retrieve the attribute type
            _attribute = CustomAttributeParser.Parse(_module, _attributeBlob, _ctorMethod);
            Debug.Assert(null != _attribute, @"Failed to load the attribute.");

			return _attribute;
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
                if (null == _ctorMethod)
                    LocateAttributeCtorMethod();

                return _ctorMethod.DeclaringType; 
            }
        }

        #endregion // Properties

        #region Internals

        /// <summary>
        /// Locates the attribute ctor method.
        /// </summary>
        private void LocateAttributeCtorMethod()
        {
            _ctorMethod = RuntimeBase.Instance.TypeLoader.GetMethod(DefaultSignatureContext.Instance, _module, _ctor);
            Debug.Assert(null != _ctorMethod);
        }

        #endregion // Internals
    }
}
