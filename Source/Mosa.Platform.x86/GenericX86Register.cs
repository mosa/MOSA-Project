/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// Base class for x86 registers.
	/// </summary>
	public abstract class GenericX86Register : Register
	{
		#region Data members

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="GenericX86Register"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="callerSaved">True if this register is caller saved, otherwise false.</param>
		protected GenericX86Register(int index) :
			base(index)
		{
		}

		#endregion // Construction

		#region Properties

		#endregion // Properties
	}
}
