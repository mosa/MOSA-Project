// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Collections.Generic;
using Mosa.Compiler.Common.Exceptions;
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

	public Operand Allocate(PrimitiveType primitiveType, bool isPinned = false, MosaType type = null)
	{
		return primitiveType switch
		{
			PrimitiveType.Int32 => Allocate32(isPinned),
			PrimitiveType.Int64 => Allocate64(isPinned),
			PrimitiveType.R4 => AllocateR4(isPinned),
			PrimitiveType.R8 => AllocateR8(isPinned),
			PrimitiveType.Object => AllocateObject(isPinned),
			PrimitiveType.ManagedPointer => AllocateManagedPointer(isPinned),
			PrimitiveType.ValueType => AllocateValueType(type, isPinned),
			_ => throw new CompilerException($"Cannot allocate a local stack of {primitiveType}"),
		};
	}

	public Operand Allocate(MosaType type, bool pinned = false)
	{
		var local = Operand.CreateStackLocal(type, Count, pinned);
		localStack.Add(local);
		return local;
	}

	public Operand Allocate(Operand operand, bool pinned = false)
	{
		var local = Operand.CreateStackLocal(operand, Count, pinned);
		localStack.Add(local);
		return local;
	}

	public Operand Allocate32(bool pinned = false)
	{
		var local = Operand.CreateStackLocal32(Count, pinned);
		localStack.Add(local);
		return local;
	}

	public Operand Allocate64(bool pinned = false)
	{
		var local = Operand.CreateStackLocal64(Count, pinned);
		localStack.Add(local);
		return local;
	}

	public Operand AllocateR4(bool pinned = false)
	{
		var local = Operand.CreateStackLocalR4(Count, pinned);
		localStack.Add(local);
		return local;
	}

	public Operand AllocateR8(bool pinned = false)
	{
		var local = Operand.CreateStackLocalR8(Count, pinned);
		localStack.Add(local);
		return local;
	}

	public Operand AllocateObject(bool pinned = false)
	{
		var local = Operand.CreateStackLocalObject(Count, pinned);
		localStack.Add(local);
		return local;
	}

	public Operand AllocateManagedPointer(bool pinned = false)
	{
		var local = Operand.CreateStackLocalManagedPointer(Count, pinned);
		localStack.Add(local);
		return local;
	}

	public Operand AllocateValueType(MosaType type, bool pinned = false)
	{
		var local = Operand.CreateStackLocalValueType(Count, pinned, type);
		localStack.Add(local);
		return local;
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
