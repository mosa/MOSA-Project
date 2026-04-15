// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;

namespace Mosa.Compiler.Framework;

/// <summary>
/// Physical Registers
/// </summary>
public sealed class PhysicalRegisters
{
	#region Properties

	public int Count { get; private set; }

	public bool Is32BitPlatform { get; private set; }

	#endregion Properties

	#region Construction

	/// <summary>
	/// Initializes a new instance of the <see cref="PhysicalRegisters" /> class.
	/// </summary>
	public PhysicalRegisters(bool is32BitPlatform)
	{
		Is32BitPlatform = is32BitPlatform;
	}

	#endregion Construction

	#region Allocation

	public Operand Allocate(PrimitiveType primitiveType, PhysicalRegister register)
	{
		var operand = Operand.CreateCPURegister(primitiveType, register, Count++);

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
		return Is32BitPlatform ? Allocate32(register) : Allocate64(register);
	}

	#endregion Allocation
}
