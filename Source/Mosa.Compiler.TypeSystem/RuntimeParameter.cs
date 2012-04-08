/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Metadata;

namespace Mosa.Compiler.TypeSystem
{
	/// <summary>
	/// Holds method parameters for reflection and compilation.
	/// </summary>
	public class RuntimeParameter
	{
		#region Data members

		/// <summary>
		/// Holds the attributes of the parameter.
		/// </summary>
		private readonly ParameterAttributes attributes;

		/// <summary>
		/// Cached name of the parameter. This value is filled upon first request of the name.
		/// </summary>
		private readonly string name;

		/// <summary>
		/// Holds the parameter token.
		/// </summary>
		private readonly HeapIndexToken token;

		/// <summary>
		/// Holds the parameter index.
		/// </summary>
		private readonly int position;

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
			this.token = (HeapIndexToken)0;
			this.attributes = attributes;
			this.name = name;
			this.position = position;
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
		public HeapIndexToken MetadataToken
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

	}
}
