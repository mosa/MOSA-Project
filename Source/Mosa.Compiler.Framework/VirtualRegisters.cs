// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Collections.Generic;
using Mosa.Compiler.Common.Exceptions;
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

	public Operand Allocate(PrimitiveType primitiveType)
	{
		return primitiveType switch
		{
			PrimitiveType.Int32 => Allocate32(),
			PrimitiveType.Int64 => Allocate64(),
			PrimitiveType.R4 => AllocateR4(),
			PrimitiveType.R8 => AllocateR8(),
			PrimitiveType.Object => AllocateObject(),
			PrimitiveType.ManagedPointer => AllocateManagedPointer(),
			PrimitiveType.ValueType => throw new CompilerException($"Cannot allocate a virtual register to a ValueType"),
			_ => throw new CompilerException($"Cannot allocate a virtual register of {primitiveType}"),
		};
	}

	public Operand Allocate(MosaType type)
	{
		var register = Operand.CreateVirtualRegister(type, virtualRegisters.Count + 1);
		virtualRegisters.Add(register);
		return register;
	}

	public Operand Allocate(Operand operand)
	{
		var register = Operand.CreateVirtualRegister(operand, virtualRegisters.Count + 1);
		virtualRegisters.Add(register);
		return register;
	}

	public Operand Allocate32()
	{
		var register = Operand.CreateVirtual32(virtualRegisters.Count + 1);
		virtualRegisters.Add(register);
		return register;
	}

	public Operand Allocate64()
	{
		var register = Operand.CreateVirtual64(virtualRegisters.Count + 1);
		virtualRegisters.Add(register);
		return register;
	}

	public Operand AllocateR4()
	{
		var register = Operand.CreateVirtualR4(virtualRegisters.Count + 1);
		virtualRegisters.Add(register);
		return register;
	}

	public Operand AllocateR8()
	{
		var register = Operand.CreateVirtualR8(virtualRegisters.Count + 1);
		virtualRegisters.Add(register);
		return register;
	}

	public Operand AllocateObject()
	{
		var register = Operand.CreateVirtualObject(virtualRegisters.Count + 1);
		virtualRegisters.Add(register);
		return register;
	}

	public Operand AllocateManagedPointer()
	{
		var register = Operand.CreateVirtualManagedPointer(virtualRegisters.Count + 1);
		virtualRegisters.Add(register);
		return register;
	}

	public Operand AllocateNativeInteger()
	{
		return Is32Platform ? Allocate32() : Allocate64();
	}

	public void SplitOperand(Operand operand)
	{
		if (operand.Low == null && operand.High == null)
		{
			var low = Operand.CreateLow(operand, virtualRegisters.Count + 1);
			var high = Operand.CreateHigh(operand, virtualRegisters.Count + 2);

			if (operand.IsVirtualRegister)
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
