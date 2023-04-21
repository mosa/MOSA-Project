// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
	/// Initializes a new instance of the <see cref="LocalStack" /> class.
	/// </summary>
	public LocalStack(bool is32Platform)
	{
		Is32Platform = is32Platform;
	}

	public Operand Allocate(PrimitiveType primitiveType, bool isPinned = false, MosaType type = null)
	{
		Debug.Assert(type == null && primitiveType != PrimitiveType.ValueType);
		Debug.Assert(type != null && primitiveType == PrimitiveType.ValueType);

		var operand = Operand.CreateStackLocal(primitiveType, Count, isPinned, type);

		localStack.Add(operand);

		return operand;
	}

	public Operand Allocate(Operand operand, bool isPinned = false)
	{
		return Allocate(operand.Primitive, isPinned, operand.Type);
	}

	public Operand Allocate32(bool pinned = false)
	{
		return Allocate(PrimitiveType.Int32, pinned);
	}

	public Operand Allocate64(bool pinned = false)
	{
		return Allocate(PrimitiveType.Int64, pinned);
	}

	public Operand AllocateR4(bool pinned = false)
	{
		return Allocate(PrimitiveType.R4, pinned);
	}

	public Operand AllocateR8(bool pinned = false)
	{
		return Allocate(PrimitiveType.R8, pinned);
	}

	public Operand AllocateObject(bool pinned = false)
	{
		return Allocate(PrimitiveType.Object, pinned);
	}

	public Operand AllocateManagedPointer(bool pinned = false)
	{
		return Allocate(PrimitiveType.ManagedPointer, pinned);
	}

	public Operand AllocateValueType(MosaType type, bool pinned = false)
	{
		return Allocate(PrimitiveType.ValueType, pinned, type);
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

	internal void Reorder(Operand virtualRegister, int index)
	{
		localStack[index - 1] = virtualRegister;
		virtualRegister.RenameIndex(index);
	}
}
