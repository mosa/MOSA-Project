/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class DumyInstruction : BaseInstruction
	{
		/// <summary>
		/// 
		/// </summary>
		public static DumyInstruction Instance = new DumyInstruction();

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="DumyInstruction"/> class.
		/// </summary>
		public DumyInstruction()
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IVisitor visitor, Context context)
		{
		}

		#endregion // Methods

	}
}
