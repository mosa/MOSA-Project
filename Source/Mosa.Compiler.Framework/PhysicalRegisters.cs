// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Diagnostics;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Physical Registers
/// </summary>
public sealed class PhysicalRegisters : IEnumerable<Operand>
{
	#region Data Members

	private readonly List<Operand> registers = new();

	#endregion Data Members

	#region Properties

	public int Count => registers.Count;

	public Operand this[int index] => registers[index];

	public bool Is32Platform { get; private set; }

	#endregion Properties

	/// <summary>
	/// Initializes a new instance of the <see cref="PhysicalRegisters" /> class.
	/// </summary>
	public PhysicalRegisters(bool is32Platform)
	{
		Is32Platform = is32Platform;
	}

	public Operand Allocate(PrimitiveType primitiveType, PhysicalRegister register)
	{
		var operand = Operand.CreateCPURegister(primitiveType, register, Count + 1);

		registers.Add(operand);

		return operand;
	}

	public Operand Allocate(Operand operand, PhysicalRegister register)
	{
		return Allocate(operand.Primitive, register);
	}

	public Operand Allocate(Operand operand)
	{
		Debug.Assert(operand.IsPhysicalRegister);
		return Allocate(operand.Primitive, operand.Register);
	}

	public Operand Allocate32(PhysicalRegister register)
	{
		return Allocate(PrimitiveType.Int32, register);
	}

	public Operand Allocate64(PhysicalRegister register)
	{
		return Allocate(PrimitiveType.Int64, register);
	}

	public Operand AllocateR4(PhysicalRegister register)
	{
		return Allocate(PrimitiveType.R4, register);
	}

	public Operand AllocateR8(PhysicalRegister register)
	{
		return Allocate(PrimitiveType.R8, register);
	}

	public Operand AllocateObject(PhysicalRegister register)
	{
		return Allocate(PrimitiveType.Object, register);
	}

	public Operand AllocateManagedPointer(PhysicalRegister register)
	{
		return Allocate(PrimitiveType.ManagedPointer, register);
	}

	public Operand AllocateNativeInteger(PhysicalRegister register)
	{
		return Is32Platform ? Allocate32(register) : Allocate64(register);
	}

	public IEnumerator<Operand> GetEnumerator()
	{
		foreach (var register in registers)
		{
			yield return register;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	internal void Reorder(Operand register, int index)
	{
		registers[index - 1] = register;
		register.Reindex(index);
	}

	internal void SwapPosition(Operand a, Operand b)
	{
		if (a == b)
			return;

		registers[a.Index - 1] = b;
		registers[b.Index - 1] = a;

		var t = a.Index;
		a.Reindex(b.Index);
		b.Reindex(t);
	}

	internal void RemoveUnused()
	{
		var updated = false;

		for (var i = registers.Count - 1; i >= 0; i--)
		{
			var virtualRegister = registers[i];

			if (virtualRegister.IsPhysicalRegister)
				continue;

			registers.RemoveAt(i);
			updated = true;
		}

		if (!updated)
			return;

		for (var i = 0; i < registers.Count; i++)
		{
			registers[i].Reindex(i + 1);
		}
	}
}
