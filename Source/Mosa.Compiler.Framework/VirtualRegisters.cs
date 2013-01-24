/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class VirtualRegisters : IEnumerable<Operand>
	{

		#region Data members

		private List<Operand> virtualRegisters = new List<Operand>();

		//private int sequenceStart;

		#endregion // Data members

		#region Properties

		public int Count { get { return virtualRegisters.Count; } }

		public Operand this[int index] { get { return virtualRegisters[index]; } }

		#endregion // Properties

		/// <summary>
		/// Initializes a new instance of the <see cref="VirtualRegisters"/> class.
		/// </summary>
		public VirtualRegisters(IArchitecture architecture)
		{
		}

		/// <summary>
		/// Allocates the virtual register.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public Operand Allocate(SigType type)
		{
			return Allocate(type, null);
		}

		/// <summary>
		/// Allocates the virtual register.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public Operand Allocate(SigType type, string name)
		{
			int index = virtualRegisters.Count + 1;

			Operand virtualRegister = name == null ? Operand.CreateVirtualRegister(type, index) : Operand.CreateVirtualRegister(type, index, name); 

			virtualRegisters.Add(virtualRegister);

			return virtualRegister;
		}

		public void SplitLongOperand(Operand longOperand, int highOffset, int lowOffset)
		{
			Debug.Assert(longOperand.StackType == StackTypeCode.Int64);

			if (longOperand.Low == null && longOperand.High == null)
			{
				virtualRegisters.Add(Operand.CreateHighSplitForLong(longOperand, highOffset, virtualRegisters.Count + 1));
				virtualRegisters.Add(Operand.CreateLowSplitForLong(longOperand, lowOffset, virtualRegisters.Count + 1));
			}
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
