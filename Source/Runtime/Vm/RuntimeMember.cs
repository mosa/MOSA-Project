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
		private RuntimeAttribute[] attributes;

		/// <summary>
		/// Specifies the type, that declares the member.
		/// </summary>
		private RuntimeType declaringType;

		/// <summary>
		/// Holds the (cached) name of the type.
		/// </summary>
		private string name;

		/// <summary>
		/// Holds the module, which owns the method.
		/// </summary>
		private IMetadataModule module;

		/// <summary>
		/// Holds the static instance of the runtime.
		/// </summary>
		protected ITypeSystem typeSystem;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="RuntimeMember"/>.
		/// </summary>
		/// <param name="token">Holds the token of this runtime metadata.</param>
		/// <param name="module">The module.</param>
		/// <param name="declaringType">The declaring type of the member.</param>
		/// <param name="attributes">Holds the attributes of the member.</param>
		protected RuntimeMember(int token, IMetadataModule module, RuntimeType declaringType, RuntimeAttribute[] attributes, ITypeSystem typeSystem) :
			base(token)
		{
			this.module = module;
			this.declaringType = declaringType;
			this.attributes = attributes;
			this.typeSystem = typeSystem;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Retrieves the declaring type of the member.
		/// </summary>
		public RuntimeType DeclaringType
		{
			get { return this.declaringType; }
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name 
		{
			get
			{
				if (this.name == null)
				{
					this.name = GetName();
					Debug.Assert(this.name != null, @"GetName() failed");
				}

				return this.name;
			}

			set
			{
				if (value == null)
					throw new ArgumentNullException(@"value");
				if (this.name != null)
					throw new InvalidOperationException();

				this.name = value;
			}
		}

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
			get { return this.module; }
		}

		#endregion // Properties

		#region Methods

		/// <summary>
		/// Called to retrieve the name of the type.
		/// </summary>
		/// <returns>The name of the type.</returns>
		protected abstract string GetName();

		#endregion // Methods

		#region IRuntimeAttributable Members

		/// <summary>
		/// Gets the custom attributes.
		/// </summary>
		/// <value>The custom attributes.</value>
		public RuntimeAttribute[] CustomAttributes
		{
			get
			{
				return this.attributes;
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
			if (null != this.attributes)
			{
				foreach (RuntimeAttribute attribute in this.attributes)
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
			if (this.attributes != null)
			{
				foreach (RuntimeAttribute attribute in this.attributes)
				{
					if (attributeType.IsAssignableFrom(attribute.Type))
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
			if (null != this.attributes)
				throw new InvalidOperationException(@"Can't set attributes twice.");

			this.attributes = attributes;
		}

		#endregion // IRuntimeAttributable Members
	}
}
