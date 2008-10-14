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
using System.Diagnostics;

namespace Mosa.Runtime.Vm
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RuntimeMember : RuntimeObject, IRuntimeAttributable
    {
        #region Data members

        /// <summary>
        /// Holds the attributes of the member.
        /// </summary>
        private RuntimeAttribute[] _attributes;

        /// <summary>
        /// Specifies the type, that declares the member.
        /// </summary>
        private RuntimeType _declaringType;

        /// <summary>
        /// Holds the module, which owns the method.
        /// </summary>
        private IMetadataModule _module;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of <see cref="RuntimeMember"/>.
        /// </summary>
        /// <param name="token">Holds the token of this runtime metadata.</param>
        /// <param name="module">The module.</param>
        /// <param name="declaringType">The declaring type of the member.</param>
        /// <param name="attributes">Holds the attributes of the member.</param>
        protected RuntimeMember(int token, IMetadataModule module, RuntimeType declaringType, RuntimeAttribute[] attributes) :
            base(token)
        {
            _module = module;
            _declaringType = declaringType;
            _attributes = attributes;
        }

        #endregion // Construction

        #region Properties

        /// <summary>
        /// Retrieves the declaring type of the member.
        /// </summary>
        public RuntimeType DeclaringType
        {
            get { return _declaringType; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public abstract string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        IntPtr _address;

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public IntPtr Address { 
            get { return _address; } 
            set { _address = value; }
        }

        /// <summary>
        /// Retrieves the module, which holds this member.
        /// </summary>
        public IMetadataModule Module
        {
            get { return _module; }
        }

        #endregion // Properties

        #region IRuntimeAttributable Members

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <value>The custom attributes.</value>
        public RuntimeAttribute[] CustomAttributes
        {
            get
            {
                return _attributes;
            }
        }

        /// <summary>
        /// Determines if the given attribute type is applied.
        /// </summary>
        /// <param name="attributeType">The type of the attribute to check.</param>
        /// <returns>
        /// The return value is true, if the attribute is applied. Otherwise false.
        /// </returns>
        public bool IsDefined(RuntimeType attributeType)
        {
            bool result = false;
            if (null != _attributes)
            {
                foreach (RuntimeAttribute attribute in _attributes)
                {
                    if (attribute.Type.Equals(attributeType) == true || 
                        attribute.Type.IsSubclassOf(attributeType) == true)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Returns an array of custom attributes identified by RuntimeType.
        /// </summary>
        /// <param name="attributeType">Type of attribute to search for. Only attributes that are assignable to this type are returned.</param>
        /// <returns>An array of custom attributes applied to this member, or an array with zero (0) elements if no matching attributes have been applied.</returns>
        public object[] GetCustomAttributes(RuntimeType attributeType)
        {
            List<object> result = new List<object>();
            if (_attributes != null)
            {
                foreach (RuntimeAttribute attribute in _attributes)
                {
                    if (true == attributeType.IsAssignableFrom(attribute.Type))
                        result.Add(attribute.GetAttribute());
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Sets the attributes of this member.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        internal void SetAttributes(RuntimeAttribute[] attributes)
        {
            if (null != _attributes)
                throw new InvalidOperationException(@"Can't set attributes twice.");

            _attributes = attributes;
        }

        #endregion // IRuntimeAttributable Members
    }
}
