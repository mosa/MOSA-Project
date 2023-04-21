// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Parameters
/// </summary>
public sealed class Parameters : IEnumerable<Operand>
{
	#region Data Members

	private readonly List<Operand> parameters = new List<Operand>();

	#endregion Data Members

	#region Properties

	public int Count => parameters.Count;

	public Operand this[int index] => parameters[index];

	public bool Is32Platform { get; private set; }

	#endregion Properties

	/// <summary>
	/// Initializes a new instance of the <see cref="Parameters" /> class.
	/// </summary>
	public Parameters(bool is32Platform)
	{
		Is32Platform = is32Platform;
	}

	public Operand Allocate(PrimitiveType primitiveType, MosaType type = null)
	{
		Debug.Assert(type == null && primitiveType != PrimitiveType.ValueType);
		Debug.Assert(type != null && primitiveType == PrimitiveType.ValueType);

		var operand = Operand.CreateVirtualRegister(primitiveType, Count, type);

		parameters.Add(operand);

		return operand;
	}

	public Operand Allocate(Operand operand)
	{
		return Allocate(operand.Primitive, operand.Type);
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

	public Operand AllocateValueType(MosaType type)
	{
		return Allocate(PrimitiveType.ValueType, type);
	}

	public Operand AllocateNativeInteger()
	{
		return Is32Platform ? Allocate32() : Allocate64();
	}

	public void SplitOperand(Operand operand)
	{
		if (operand.Low == null && operand.High == null)
		{
			var low = Operand.CreateLow(operand, parameters.Count + 1);
			var high = Operand.CreateHigh(operand, parameters.Count + 2);

			if (operand.IsVirtualRegister)
			{
				parameters.Add(low);
				parameters.Add(high);
			}
		}
	}

	public IEnumerator<Operand> GetEnumerator()
	{
		foreach (var virtualRegister in parameters)
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
		parameters[index - 1] = virtualRegister;
		virtualRegister.RenameIndex(index);
	}
}
