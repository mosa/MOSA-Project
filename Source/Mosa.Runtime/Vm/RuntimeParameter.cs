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

namespace Mosa.Runtime.Vm
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
		private ParameterAttributes _attributes;

		/// <summary>
		/// The metadata module, which holds the parameter.
		/// </summary>
		private IMetadataModule _module;

		/// <summary>
		/// Holds the token of the parameter name string.
		/// </summary>
		private TokenTypes _nameIdx;

		/// <summary>
		/// Cached name of the parameter. This value is filled upon first request of the name.
		/// </summary>
		private string _name;

		/// <summary>
		/// Holds the parameter token.
		/// </summary>
		private TokenTypes _token;

		// <summary>
		// Holds the type of the parameter.
		// </summary>
		//private TokenTypes _elementType;

		/// <summary>
		/// Holds the parameter index.
		/// </summary>
		private int _position;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeParameter"/> class.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="name">The name.</param>
		/// <param name="position">The position.</param>
		/// <param name="attributes">The attributes.</param>
		public RuntimeParameter(IMetadataModule module, string name, int position, ParameterAttributes attributes)
		{
			_module = module;
			_nameIdx = (TokenTypes)0;
			_token = (TokenTypes)0;
			_attributes = attributes;
			_name = name;
			_position = position;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeParameter"/> class.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="param">The param.</param>
		public RuntimeParameter(IMetadataModule module, ParamRow param)
		{
			_module = module;
			_attributes = param.Flags;
			_nameIdx = param.NameIdx;
			_position = param.Sequence;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Returns the parameter flags.
		/// </summary>
		public ParameterAttributes Attributes
		{
			get { return _attributes; }
		}

		/// <summary>
		/// Determines if this is an input parameter.
		/// </summary>
		public bool IsIn
		{
			get { return (ParameterAttributes.In == (ParameterAttributes.In & _attributes)); }
		}

		/// <summary>
		/// Determines if this is an output parameter.
		/// </summary>
		public bool IsOut
		{
			get { return (ParameterAttributes.Out == (ParameterAttributes.Out & _attributes)); }
		}

		/// <summary>
		/// Retrieves the name of the parameter.
		/// </summary>
		public string Name
		{
			get
			{
				if (null != _name)
					return _name;

				// Load the name
				_name = _module.Metadata.ReadString(_nameIdx);
				return _name;
			}
		}

		/// <summary>
		/// Retrieves the metadata token of this parameter.
		/// </summary>
		public TokenTypes MetadataToken
		{
			get { return _token; }
		}
		/*
				/// <summary>
				/// Retrieves the type of the parameter.
				/// </summary>
				public TokenTypes ParameterType
				{
					get { return _elementType; }
				}
		*/
		/// <summary>
		/// Retrieves the parameter position.
		/// </summary>
		public int Position
		{
			get { return _position; }
		}

		#endregion // Properties

		#region Methods

		#endregion // Methods

		#region IEquatable<RuntimeParameter> Members

		/// <summary>
		/// Gibt an, ob das aktuelle Objekt gleich einem anderen Objekt des gleichen Typs ist.
		/// </summary>
		/// <param name="other">Ein Objekt, das mit diesem Objekt verglichen werden soll.</param>
		/// <returns>
		/// true, wenn das aktuelle Objekt gleich dem <paramref name="other"/>-Parameter ist, andernfalls false.
		/// </returns>
		public bool Equals(RuntimeParameter other)
		{
			return (_module == other._module && _nameIdx == other._nameIdx && _position == other._position);
		}

		#endregion // IEquatable<RuntimeParameter> Members
	}
}
