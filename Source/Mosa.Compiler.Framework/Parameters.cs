// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Collections.Generic;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Parameters
/// </summary>
public sealed class Parameters : IEnumerable<Operand>
{
	#region Data Members

	private readonly List<Operand> parameters;

	#endregion Data Members

	#region Properties

	public int Count => parameters.Count;

	public Operand this[int index] => parameters[index];

	public bool Is32Platform { get; private set; }

	#endregion Properties

	/// <summary>
	/// Initializes a new instance of the <see cref="Parameters" /> class.
	/// </summary>
	public Parameters(bool is32Platform, int count)
	{
		Is32Platform = is32Platform;
		parameters = new List<Operand>(count);
	}

	public Operand Allocate(PrimitiveType primitiveType, ElementType elementType, string name, int offset, uint size, MosaType type = null)
	{
		var operand = Operand.CreateStackParameter(primitiveType, elementType, Count, name, offset, size, type);

		parameters.Add(operand);

		return operand;
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
