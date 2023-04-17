// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Collections.Generic;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Local Stack
/// </summary>
public sealed class LocalStack : IEnumerable<Operand>
{
	#region Data Members

	private readonly List<Operand> localStack = new List<Operand>();

	#endregion Data Members

	#region Properties

	public int Count => localStack.Count;

	public Operand this[int index] => localStack[index];

	public bool Is32Platform { get; private set; }

	#endregion Properties

	/// <summary>
	/// Initializes a new instance of the <see cref="VirtualRegisters" /> class.
	/// </summary>
	public LocalStack(bool is32Platform)
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
		var index = localStack.Count + 1;

		var virtualRegister = Operand.CreateVirtualRegister(type, index);

		localStack.Add(virtualRegister);

		return virtualRegister;
	}

	public Operand AllocateOperand(Operand operand)
	{
		var index = localStack.Count + 1;

		var virtualRegister = Operand.CreateVirtualRegister(operand, index);

		localStack.Add(virtualRegister);

		return virtualRegister;
	}

	public Operand Allocate32()
	{
		var index = localStack.Count + 1;

		var virtualRegister = Operand.CreateVirtual32(index);

		localStack.Add(virtualRegister);

		return virtualRegister;
	}

	public Operand Allocate64()
	{
		var index = localStack.Count + 1;

		var virtualRegister = Operand.CreateVirtual64(index);

		localStack.Add(virtualRegister);

		return virtualRegister;
	}

	public Operand AllocateR4()
	{
		var index = localStack.Count + 1;

		var virtualRegister = Operand.CreateVirtualR4(index);

		localStack.Add(virtualRegister);

		return virtualRegister;
	}

	public Operand AllocateR8()
	{
		var index = localStack.Count + 1;

		var virtualRegister = Operand.CreateVirtualR8(index);

		localStack.Add(virtualRegister);

		return virtualRegister;
	}

	public Operand AllocateObject()
	{
		var index = localStack.Count + 1;

		var virtualRegister = Operand.CreateVirtualObject(index);

		localStack.Add(virtualRegister);

		return virtualRegister;
	}

	public Operand AllocateManagedPointer()
	{
		var index = localStack.Count + 1;

		var virtualRegister = Operand.CreateVirtualManagedPointer(index);

		localStack.Add(virtualRegister);

		return virtualRegister;
	}

	public Operand AllocateNativeInteger()
	{
		return Is32Platform ? Allocate32() : Allocate64();
	}

	public void SplitOperand(Operand operand)
	{
		if (operand.Low == null && operand.High == null)
		{
			var low = Operand.CreateLow(operand, localStack.Count + 1);
			var high = Operand.CreateHigh(operand, localStack.Count + 2);

			if (operand.IsVirtualRegister)
			{
				localStack.Add(low);
				localStack.Add(high);
			}
		}
	}

	public IEnumerator<Operand> GetEnumerator()
	{
		foreach (var virtualRegister in localStack)
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
		localStack[index - 1] = virtualRegister;
		virtualRegister.RenameIndex(index);
	}
}
