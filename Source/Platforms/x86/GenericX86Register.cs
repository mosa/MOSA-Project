/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Runtime.CompilerFramework;

namespace Mosa.Platforms.x86
{
	/// <summary>
	/// Base class for x86 registers.
	/// </summary>
	public abstract class GenericX86Register : Register
	{
		#region Data members

		/// <summary>
		/// Determines if this register is caller-saved.
		/// </summary>
		private readonly bool _isCallerSave;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="GenericX86Register"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="callerSaved">True if this register is caller saved, otherwise false.</param>
		protected GenericX86Register(int index, bool callerSaved) :
			base(index)
		{
			_isCallerSave = callerSaved;
		}

		#endregion // Construction

		#region Properties

		/// <summary>
		/// Gets the caller-save status of this register.
		/// </summary>
		/// <value>True if the register is caller saved; otherwise false.</value>
		public override sealed bool IsCallerSaved
		{
			get { return _isCallerSave; }
		}

		#endregion // Properties
	}
}
