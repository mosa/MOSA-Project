// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Virtual Registers
/// </summary>
public sealed class VirtualRegisters : IEnumerable<Operand>
{
	#region Data Members

	private readonly List<Operand> virtualRegisters = new();

	#endregion Data Members

	#region Properties

	public int Count => virtualRegisters.Count;

	public Operand this[int index] => virtualRegisters[index];

	public bool Is32BitPlatform { get; private set; }

	#endregion Properties

	/// <summary>
	/// Initializes a new instance of the <see cref="VirtualRegisters" /> class.
	/// </summary>
	public VirtualRegisters(bool is32BitPlatform)
	{
		Is32BitPlatform = is32BitPlatform;
	}

	public Operand Allocate(PrimitiveType primitiveType)
	{
		var operand = Operand.CreateVirtualRegister(primitiveType, Count + 1, null);

		virtualRegisters.Add(operand);

		return operand;
	}

	public Operand Allocate(Operand operand)
	{
		return Allocate(operand.Primitive);
	}

	public Operand Allocate32()
	{
		return Allocate(PrimitiveType.Int32);
	}

	public Operand Allocate64()
	{
		return Allocate(PrimitiveType.Int64);
	}

	public Operand AllocateR4()
	{
		return Allocate(PrimitiveType.R4);
	}

	public Operand AllocateR8()
	{
		return Allocate(PrimitiveType.R8);
	}

	public Operand AllocateObject()
	{
		return Allocate(PrimitiveType.Object);
	}

	public Operand AllocateManagedPointer()
	{
		return Allocate(PrimitiveType.ManagedPointer);
	}

	public Operand AllocateNativeInteger()
	{
		return Is32BitPlatform ? Allocate32() : Allocate64();
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

	internal void Reorder(Operand virtualRegister, int index)
	{
		virtualRegisters[index - 1] = virtualRegister;
		virtualRegister.Reindex(index);
	}

	internal void SwapPosition(Operand a, Operand b)
	{
		if (a == b)
			return;

		virtualRegisters[a.Index - 1] = b;
		virtualRegisters[b.Index - 1] = a;

		var t = a.Index;
		a.Reindex(b.Index);
		b.Reindex(t);
	}

	internal void RemoveUnused()
	{
		var updated = false;

		for (var i = virtualRegisters.Count - 1; i >= 0; i--)
		{
			var virtualRegister = virtualRegisters[i];

			if (virtualRegister.IsVirtualRegisterUsed)
				continue;

			virtualRegisters.RemoveAt(i);
			updated = true;
		}

		if (!updated)
			return;

		for (var i = 0; i < virtualRegisters.Count; i++)
		{
			virtualRegisters[i].Reindex(i + 1);
		}
	}
}
