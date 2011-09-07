/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */



namespace Mosa.Compiler.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct ParamRow
	{
		#region Data members

		/// <summary>
		/// Holds the flags of the parameter.
		/// </summary>
		private ParameterAttributes flags;

		/// <summary>
		/// The token holding the name of the parameter.
		/// </summary>
		private HeapIndexToken nameIdx;

		/// <summary>
		/// Holds the sequence index of the parameter.
		/// </summary>
		private short sequence;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="ParamRow"/> struct.
		/// </summary>
		/// <param name="flags">The flags.</param>
		/// <param name="sequence">The sequence.</param>
		/// <param name="nameIdx">The name idx.</param>
		public ParamRow(ParameterAttributes flags, short sequence, HeapIndexToken nameIdx)
		{
			this.nameIdx = nameIdx;
			this.sequence = sequence;
			this.flags = flags;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Returns the attributes of this parameter.
		/// </summary>
		/// <value>The flags.</value>
		public ParameterAttributes Flags
		{
			get { return flags; }
		}

		/// <summary>
		/// Retrieves the token of the parameter name.
		/// </summary>
		/// <value>The name idx.</value>
		public HeapIndexToken NameIdx
		{
			get
			{
				return nameIdx;
			}
		}

		/// <summary>
		/// Retrieves the parameter sequence number.
		/// </summary>
		/// <value>The sequence.</value>
		public short Sequence
		{
			get { return sequence; }
		}

		#endregion // Properties
	}
}
