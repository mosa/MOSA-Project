/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Metadata;

namespace Mosa.Platform.Null
{
	/// <summary>
	/// Implements the CIL default calling convention for null.
	/// </summary>
	sealed class DefaultCallingConvention : ICallingConvention
	{
		#region Data members

		/// <summary>
		/// Holds the architecture of the calling convention.
		/// </summary>
		private IArchitecture architecture;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="DefaultCallingConvention"/>.
		/// </summary>
		/// <param name="architecture">The architecture of the calling convention.</param>
		/// <param name="typeLayout">The type layout.</param>
		public DefaultCallingConvention(IArchitecture architecture)
		{
			if (architecture == null)
				throw new ArgumentNullException(@"architecture");

			this.architecture = architecture;
		}

		#endregion // Construction

		#region ICallingConvention Members

		/// <summary>
		/// Expands the given invoke instruction to perform the method call.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>
		/// A single instruction or an array of instructions, which appropriately represent the method call.
		/// </returns>
		void ICallingConvention.MakeCall(Context context)
		{
			return;
		}

		/// <summary>
		/// Requests the calling convention to create an appropriate move instruction to populate the return
		/// value of a method.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="operand">The operand, that's holding the return value.</param>
		void ICallingConvention.MoveReturnValue(Context context, Operand operand)
		{
			return;
		}

		void ICallingConvention.GetStackRequirements(Operand stackOperand, out int size, out int alignment)
		{
			// Special treatment for some stack types
			// FIXME: Handle the size and alignment requirements of value types
			architecture.GetTypeRequirements(stackOperand.Type, out size, out alignment);
		}

		int ICallingConvention.OffsetOfFirstLocal { get { return -4; } }

		int ICallingConvention.OffsetOfFirstParameter { get { return 8; } }

		/// <summary>
		/// Gets the callee saved registers.
		/// </summary>
		Register[] ICallingConvention.CalleeSavedRegisters
		{
			get { return null; }
		}

		/// <summary>
		/// Gets the return registers.
		/// </summary>
		/// <param name="returnType">Type of the return.</param>
		/// <returns></returns>
		Register[] ICallingConvention.GetReturnRegisters(CilElementType returnType)
		{
			return null;
		}

		#endregion // ICallingConvention Members
	}
}
