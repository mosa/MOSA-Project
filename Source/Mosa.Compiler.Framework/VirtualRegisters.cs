/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections;
using System.Collections.Generic;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Contains the layout of the stack
	/// </summary>
	public sealed class VirtualRegisters : IEnumerable<Operand>
	{

		#region Data members

		private List<Operand> virtualRegisters = new List<Operand>();

		#endregion // Data members

		#region Properties

		public int Count { get { return virtualRegisters.Count; } }

		#endregion // Properties

		/// <summary>
		/// Initializes a new instance of the <see cref="VirtualRegisters"/> class.
		/// </summary>
		public VirtualRegisters()
		{
		}

		/// <summary>
		/// Allocates the virtual register.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public Operand Allocate(SigType type)
		{
			Operand virtualRegister = Operand.CreateVirtualRegister(type, virtualRegisters.Count + 1);

			virtualRegisters.Add(virtualRegister);

			return virtualRegister;
		}


		public IEnumerator<Operand> GetEnumerator()
		{
			foreach (var virtualRegister in virtualRegisters)
			{
				yield return virtualRegister;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
