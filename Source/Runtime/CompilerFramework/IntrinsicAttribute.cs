/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

namespace Mosa.Runtime.CompilerFramework
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
		/// The architecture of the intrinsic implementation.
		/// </summary>
		private Type _architecture;

		/// <summary>
		/// The IR type of this instruction.
		/// </summary>
		private Type _instructionType;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the IntrinsicAttribute type.
		/// </summary>
		/// <param name="architecture">The specific architecture of the intrinsic implementation.</param>
		/// <param name="instructionType">The type of the IR instruction, which generates appropriate native code.</param>
		public IntrinsicAttribute(Type architecture, Type instructionType)
		{
			_architecture = architecture;
			_instructionType = instructionType;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Returns the architecture, which has support for an intrinsic implementation.
		/// </summary>
		public Type Architecture
		{
			get { return _architecture; }
		}

		/// <summary>
		/// Returns the instruction type used to represent the intrinsic.
		/// </summary>
		public Type InstructionType
		{
			get { return _instructionType; }
		}

		#endregion // Properties
	}
}
