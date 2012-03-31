/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Text;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class BasePlatformInstruction : BaseInstruction
	{

		#region  Data members

		/// <summary>
		/// Gets the usable result registers.
		/// </summary>
		public virtual Register[] UsableResultRegisters { get { return null; } }

		/// <summary>
		/// Gets the usable operand registers.
		/// </summary>
		public virtual Register[] UsableOperandRegisters { get { return null; } }

		#endregion

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseInstruction"/> class.
		/// </summary>
		public BasePlatformInstruction()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseInstruction"/> class.
		/// </summary>
		/// <param name="operandCount">The operand count.</param>
		public BasePlatformInstruction(byte operandCount)
			: base(operandCount)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseInstruction"/> class.
		/// </summary>
		/// <param name="operandCount">The operand count.</param>
		/// <param name="resultCount">The result count.</param>
		public BasePlatformInstruction(byte operandCount, byte resultCount)
			: base(operandCount, resultCount)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Gets the used registers.
		/// </summary>
		/// <param name="results">The results.</param>
		/// <param name="operand1">The operand1.</param>
		/// <returns></returns>
		public virtual Register[] GetUsedRegisters(Register results, Register operand1)
		{
			return GetUsedRegisters(results, operand1, null);
		}

		/// <summary>
		/// Determines whether [is valid operand] [the specified result].
		/// </summary>
		/// <param name="result">The result.</param>
		/// <returns>
		///   <c>true</c> if [is valid operand] [the specified result]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsValidOperand(Register result)
		{
			foreach (var register in UsableOperandRegisters)
				if (result == register)
					return true;

			return false;
		}

		/// <summary>
		/// Determines whether [is valid result operand] [the specified result].
		/// </summary>
		/// <param name="result">The result.</param>
		/// <returns>
		///   <c>true</c> if [is valid result operand] [the specified result]; otherwise, <c>false</c>.
		/// </returns>
		public bool IsValidResult(Register result)
		{
			foreach (var register in UsableResultRegisters)
				if (result == register)
					return true;

			return false;
		}

		/// <summary>
		/// Gets the used registers.
		/// </summary>
		/// <param name="results">The results.</param>
		/// <param name="operand1">The operand1.</param>
		/// <param name="operand2">The operand2.</param>
		/// <returns></returns>
		public virtual Register[] GetUsedRegisters(Register results, Register operand1, Register operand2)
		{
			return null;
		}

		#endregion //  Methods
	}
}
