/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace Mosa.Compiler.Framework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public class PrefixInstruction : BaseCILInstruction
	{
		#region Properties

		/// <summary>
		/// Gets the prefix flag.
		/// </summary>
		/// <value>A prefix fla</value>
		public Prefix Flags
		{
			get
			{
				switch (OpCode)
				{
					case OpCode.PreConstrained: return Prefix.Constrained;
					case OpCode.PreNo: return Prefix.No;
					case OpCode.PreReadOnly: return Prefix.ReadOnly;
					case OpCode.PreTail: return Prefix.Tail;
					case OpCode.PreUnaligned: return Prefix.Unaligned;
					case OpCode.PreVolatile: return Prefix.Volatile;
					default:
						throw new InvalidOperationException(@"Unknown prefix instruction code.");
				}
			}
		}

		#endregion // Properties

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="PrefixInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		protected PrefixInstruction(OpCode opcode)
			: base(opcode)
		{
		}

		#endregion // Construction


	}
}
