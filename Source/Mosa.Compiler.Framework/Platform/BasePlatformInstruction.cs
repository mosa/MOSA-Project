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

namespace Mosa.Compiler.Framework.Platform
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class BasePlatformInstruction : BaseInstruction
	{

		#region  Data members

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

		#endregion //  Methods
	}
}
