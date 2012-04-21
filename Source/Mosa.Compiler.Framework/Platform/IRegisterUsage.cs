/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Operands;

namespace Mosa.Compiler.Framework.Platform
{

	/// <summary>
	/// This interface is used to present opcode constraints to the register allocator.
	/// </summary>
	public interface IRegisterUsage
	{

		/// <summary>
		/// Gets the output registers.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		RegisterBitmap GetOutputRegisters(Context context);

		/// <summary>
		/// Gets the input registers.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		RegisterBitmap GetInputRegisters(Context context);

		/// <summary>
		/// Gets a value indicating whether [result is input].
		/// </summary>
		/// <value>
		///   <c>true</c> if [result is input]; otherwise, <c>false</c>.
		/// </value>
		bool ResultIsInput { get; }

		/// <summary>
		/// Gets the additional output registers.
		/// </summary>
		RegisterBitmap AdditionalOutputRegisters { get; }

		/// <summary>
		/// Gets the additional input registers.
		/// </summary>
		RegisterBitmap AdditionalInputRegisters { get; }

	}

}
