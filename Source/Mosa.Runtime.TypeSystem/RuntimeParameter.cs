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

using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Tables;

namespace Mosa.Runtime.TypeSystem
{
	/// <summary>
	/// Holds method parameters for reflection and compilation.
	/// </summary>
	public class RuntimeParameter : IEquatable<RuntimeParameter>
	{
		#region Data members

		/// <summary>
		/// Holds the attributes of the parameter.
		/// </summary>
		private ParameterAttributes attributes;

		/// <summary>
		/// Holds the token of the parameter name string.
		/// </summary>
		private TokenTypes nameIdx;

		/// <summary>
		/// Cached name of the parameter. This value is filled upon first request of the name.
		/// </summary>
		private string name;

		/// <summary>
		/// Holds the parameter token.
		/// </summary>
		private TokenTypes token;

		/// <summary>
		/// Holds the parameter index.
		/// </summary>
		private int position;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeParameter"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="position">The position.</param>
		/// <param name="attributes">The attributes.</param>
		public RuntimeParameter(string name, int position, ParameterAttributes attributes)
		{
			this.nameIdx = (TokenTypes)0;
			this.token = (TokenTypes)0;
			this.attributes = attributes;
			this.name = name;
			this.position = position;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeParameter"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="param">The param.</param>
		public RuntimeParameter(string name, ParamRow param)
		{
			this.attributes = param.Flags;
			this.nameIdx = param.NameIdx;
			this.position = param.Sequence;
			this.name = name;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Returns the parameter flags.
		/// </summary>
		public ParameterAttributes Attributes
		{
			get { return attributes; }
		}

		/// <summary>
		/// Determines if this is an input parameter.
		/// </summary>
		public bool IsIn
		{
			get { return (ParameterAttributes.In == (ParameterAttributes.In & attributes)); }
		}

		/// <summary>
		/// Determines if this is an output parameter.
		/// </summary>
		public bool IsOut
		{
			get { return (ParameterAttributes.Out == (ParameterAttributes.Out & attributes)); }
		}

		/// <summary>
		/// Retrieves the name of the parameter.
		/// </summary>
		public string Name
		{
			get { return name; }
		}

		/// <summary>
		/// Retrieves the metadata token of this parameter.
		/// </summary>
		public TokenTypes MetadataToken
		{
			get { return token; }
		}
		
		/// <summary>
		/// Retrieves the parameter position.
		/// </summary>
		public int Position
		{
			get { return position; }
		}

		#endregion // Properties

		#region Methods

		#endregion // Methods

		#region IEquatable<RuntimeParameter> Members

		/// <summary>
		/// Equalses the specified other.
		/// </summary>
		/// <param name="other">The other.</param>
		/// <returns></returns>
		public bool Equals(RuntimeParameter other)
		{
			//TODO
			//return (module == other.module && nameIdx == other.nameIdx && position == other.position);
			return (nameIdx == other.nameIdx && position == other.position);
		}

		#endregion // IEquatable<RuntimeParameter> Members
	}
}
