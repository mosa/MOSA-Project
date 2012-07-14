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
using Mosa.Compiler.Metadata;

namespace Mosa.Compiler.TypeSystem
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class RuntimeMember : RuntimeObject
	{
		#region Data members

		/// <summary>
		/// Holds the attributes of the member.
		/// </summary>
		private List<RuntimeAttribute> attributes;

		/// <summary>
		/// Specifies the type, that declares the member.
		/// </summary>
		private readonly RuntimeType declaringType;

		/// <summary>
		/// Holds the (cached) name of the type.
		/// </summary>
		private string name;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="RuntimeMember"/>.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="token">Holds the token of this runtime metadata.</param>
		/// <param name="declaringType">The declaring type of the member.</param>
		protected RuntimeMember(ITypeModule module, Token token, RuntimeType declaringType) :
			base(module, token)
		{
			this.declaringType = declaringType;
			this.attributes = new List<RuntimeAttribute>();
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
			get { return this.name; }
			protected set
			{
				if (value == null)
					throw new ArgumentNullException(@"value");

				this.name = value;
			}
		}

		/// <summary>
		/// Returns the attributes of the type.
		/// </summary>
		/// <value>The attributes.</value>
		public List<RuntimeAttribute> CustomAttributes
		{
			get { return attributes; }
			protected set
			{
				if (value == null)
					throw new ArgumentNullException(@"value");

				attributes = value;
			}
		}

		#endregion // Properties

		#region Methods

		#endregion // Methods

		#region IRuntimeAttributable Members

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
			if (this.attributes != null)
			{
				foreach (var attribute in this.attributes)
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

		#endregion // IRuntimeAttributable Members
	}
}
