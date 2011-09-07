/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */



namespace Mosa.Runtime.Metadata.Tables
{
	/// <summary>
	/// 
	/// </summary>
	public struct GenericParamRow
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private ushort number;

		/// <summary>
		/// 
		/// </summary>
		private GenericParameterAttributes flags;

		/// <summary>
		/// 
		/// </summary>
		private Token owner;

		/// <summary>
		/// 
		/// </summary>
		private HeapIndexToken nameStringIdx;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericParamRow"/> struct.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <param name="flags">The flags.</param>
		/// <param name="owner">The owner table idx.</param>
		/// <param name="nameStringIdx">The name string idx.</param>
		public GenericParamRow(ushort number, GenericParameterAttributes flags, Token owner, HeapIndexToken nameStringIdx)
		{
			this.number = number;
			this.flags = flags;
			this.owner = owner;
			this.nameStringIdx = nameStringIdx;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the number.
		/// </summary>
		/// <value>The number.</value>
		public ushort Number
		{
			get { return number; }
		}

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public GenericParameterAttributes Flags
		{
			get { return flags; }
		}

		/// <summary>
		/// Gets the owner table idx.
		/// </summary>
		/// <value>The owner table idx.</value>
		public Token Owner
		{
			get { return owner; }
		}

		/// <summary>
		/// Gets the name string idx.
		/// </summary>
		/// <value>The name string idx.</value>
		public HeapIndexToken NameStringIdx
		{
			get { return nameStringIdx; }
		}

		#endregion // Properties
	}
}
