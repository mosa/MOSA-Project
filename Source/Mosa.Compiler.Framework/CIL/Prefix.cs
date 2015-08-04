// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// Specifies the possible prefixes of IL instructions.
	/// </summary>
	public enum Prefix
	{
		/// <summary>
		/// Indicates a potentially unaligned, but valid memory access for the instruction.
		/// </summary>
		Unaligned = 0x01,

		/// <summary>
		/// Indicates a volatile memory access, e.g. this memory access should not be optimized away and always
		/// needs to go to memory.
		/// </summary>
		Volatile = 0x02,

		/// <summary>
		/// Subsequent call terminates the method. Can be optimized to remove the current method call frame.
		/// </summary>
		Tail = 0x04,

		/// <summary>
		/// Invoke a member on a value of a variable type.
		/// </summary>
		Constrained = 0x08,

		/// <summary>
		/// Do not perform type, range or null checks on the following instruction.
		/// </summary>
		No = 0x10,

		/// <summary>
		/// Subsequent array address operation performs no type check at runtime and returns a controlled mutability managed pointer.
		/// </summary>
		ReadOnly = 0x20
	}
}
