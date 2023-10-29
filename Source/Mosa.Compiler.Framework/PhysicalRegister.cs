// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework;

/// <summary>
/// Represents a machine specific abstract register.
/// </summary>
public sealed class PhysicalRegister
{
	#region Properties

	/// <summary>
	/// Retrieves the numeric index of this register in its architecture.
	/// </summary>
	public readonly int Index;

	/// <summary>
	/// Gets the name of the register
	/// </summary>
	public readonly string Name;

	/// <summary>
	/// Holds the machine specific index or code of the register.
	/// </summary>
	public readonly uint RegisterCode;

	/// <summary>
	/// Determines if this is a floating point register.
	/// </summary>
	public readonly bool IsFloatingPoint;

	/// <summary>
	/// Determines if this is a integer register.
	/// </summary>
	public readonly bool IsInteger;

	/// <summary>
	/// Gets a value indicating whether this register is special register that the
	/// register allocator should not consider.
	/// </summary>
	public bool IsSpecial => !(IsInteger || IsFloatingPoint);

	#endregion Properties

	#region Construction

	/// <summary>
	/// Initializes a new instance of <see cref="PhysicalRegister"/>.
	/// </summary>
	/// <param name="index">The numeric index of the register.</param>
	public PhysicalRegister(int index, uint registerCode, string name, bool isInteger, bool isFloatingPoint)
	{
		Index = index;
		Name = name;
		RegisterCode = registerCode;
		IsInteger = isInteger;
		IsFloatingPoint = isFloatingPoint;
	}

	#endregion Construction

	#region Methods

	public override string ToString() => Name;

	#endregion Methods
}
