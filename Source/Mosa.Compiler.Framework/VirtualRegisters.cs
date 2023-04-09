// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Collections.Generic;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Virtual Registers
/// </summary>
public sealed class VirtualRegisters : IEnumerable<Operand>
{
	#region Data Members

	private readonly List<Operand> virtualRegisters = new List<Operand>();

	#endregion Data Members

	#region Properties

	public int Count => virtualRegisters.Count;

	public Operand this[int index] => virtualRegisters[index];

	public bool Is32Platform { get; private set; }

	#endregion Properties

	/// <summary>
	/// Initializes a new instance of the <see cref="VirtualRegisters" /> class.
	/// </summary>
	public VirtualRegisters(bool is32Platform)
	{
		Is32Platform = is32Platform;
	}

	/// <summary>
	/// Allocates the virtual register.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <returns></returns>
	public Operand Allocate(MosaType type)
	{
		var index = virtualRegisters.Count + 1;

		var virtualRegister = Operand.CreateVirtualRegister(type, index);

		virtualRegisters.Add(virtualRegister);

		return virtualRegister;
	}

	public Operand AllocateOperand(Operand operand)
	{
		var index = virtualRegisters.Count + 1;

		var virtualRegister = Operand.CreateVirtualRegister(operand, index);

		virtualRegisters.Add(virtualRegister);

		return virtualRegister;
	}

	public Operand Allocate32()
	{
		var index = virtualRegisters.Count + 1;

		var virtualRegister = Operand.CreateVirtual32(index);

		virtualRegisters.Add(virtualRegister);

		return virtualRegister;
	}

	public Operand Allocate64()
	{
		var index = virtualRegisters.Count + 1;

		var virtualRegister = Operand.CreateVirtual64(index);

		virtualRegisters.Add(virtualRegister);

		return virtualRegister;
	}

	public Operand AllocateR4()
	{
		var index = virtualRegisters.Count + 1;

		var virtualRegister = Operand.CreateVirtualR4(index);

		virtualRegisters.Add(virtualRegister);

		return virtualRegister;
	}

	public Operand AllocateR8()
	{
		var index = virtualRegisters.Count + 1;

		var virtualRegister = Operand.CreateVirtualR8(index);

		virtualRegisters.Add(virtualRegister);

		return virtualRegister;
	}

	public Operand AllocateObject()
	{
		var index = virtualRegisters.Count + 1;

		var virtualRegister = Operand.CreateVirtualObject(index);

		virtualRegisters.Add(virtualRegister);

		return virtualRegister;
	}

	public Operand AllocateManagedPointer()
	{
		var index = virtualRegisters.Count + 1;

		var virtualRegister = Operand.CreateVirtualManagedPointer(index);

		virtualRegisters.Add(virtualRegister);

		return virtualRegister;
	}

	public Operand AllocateNativeInteger()
	{
		return Is32Platform ? Allocate32() : Allocate64();
	}

	public void SplitLongOperand(TypeSystem typeSystem, Operand longOperand)
	{
		//Debug.Assert(longOperand.IsInteger64 || longOperand.IsParameter);

		if (longOperand.Low == null && longOperand.High == null)
		{
			var low = Operand.CreateLowSplitForLong(longOperand, virtualRegisters.Count + 1, typeSystem);
			var high = Operand.CreateHighSplitForLong(longOperand, virtualRegisters.Count + 2, typeSystem);

			if (longOperand.IsVirtualRegister)
			{
				virtualRegisters.Add(low);
				virtualRegisters.Add(high);
			}
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

	internal void ReOrdered(Operand virtualRegister, int index)
	{
		virtualRegisters[index - 1] = virtualRegister;
		virtualRegister.RenameIndex(index);
	}
}
