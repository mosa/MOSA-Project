/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

namespace Mosa.Intrinsic
{
	/// <summary>
	/// Used to mark architecture intrinsic methods or properties.
	/// </summary>
	/// <remarks>
	/// Marking a method or property with the IntrinsicAttribute causes the compiler to use the
	/// architecture specific implementation of that language element. Compilers, which do not
	/// support the marked intrinsic emit the provided intermediate language equivalent, if there's
	/// no equivalent in intermediate language the implementation should throw a <see cref="System.NotSupportedException"/>.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
	public sealed class IntrinsicAttribute : Attribute
	{
		#region Data members

		/// <summary>
		/// The IR type of this instruction.
		/// </summary>
		private readonly string instructionType;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the IntrinsicAttribute type.
		/// </summary>
		/// <param name="instructionType">The type used to build the architecture neutral intrinsic.</param>
		public IntrinsicAttribute(string instructionType)
		{
			this.instructionType = instructionType;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Returns the instruction type used to represent the intrinsic.
		/// </summary>
		public string InstructionType
		{
			get { return this.instructionType; }
		}

		#endregion // Properties
	}
}
