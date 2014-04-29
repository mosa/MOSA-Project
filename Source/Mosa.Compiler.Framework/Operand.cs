/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;

using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Operand class
	/// </summary>
	public sealed class Operand
	{
		#region Properties

		/// <summary>
		/// Holds a list of instructions, which define this operand.
		/// </summary>
		public List<int> Definitions { get; private set; }

		/// <summary>
		/// Holds a list of instructions, which use this operand.
		/// </summary>
		public List<int> Uses { get; private set; }

		/// <summary>
		/// Returns the type of the operand.
		/// </summary>
		public MosaType Type { get; private set; }

		/// <summary>
		/// Retrieves the register, where the operand is located.
		/// </summary>
		public Register Register { get; private set; }

		/// <summary>
		/// Retrieves the offset base.
		/// </summary>
		public Operand OffsetBase { get; private set; }

		/// <summary>
		/// Gets the effective base.
		/// </summary>
		/// <value>
		/// The effective base.
		/// </value>
		public Register EffectiveOffsetBase { get { return OffsetBase != null ? OffsetBase.Register : Register; } }

		/// <summary>
		/// Gets the base operand.
		/// </summary>
		public Operand SSAParent { get; private set; }

		/// <summary>
		/// Holds the address offset if used together with a base register or the absolute address, if register is null.
		/// </summary>
		/// <value>
		/// The offset.
		/// </value>
		public long Displacement { get; set; }

		/// <summary>
		/// Retrieves the method.
		/// </summary>
		public MosaMethod Method { get; private set; }

		/// <summary>
		/// Retrieves the field.
		/// </summary>
		public MosaField Field { get; private set; }

		/// <summary>
		/// Gets the ssa version.
		/// </summary>
		public int SSAVersion { get; private set; }

		/// <summary>
		/// Gets or sets the low operand.
		/// </summary>
		/// <value>
		/// The low operand.
		/// </value>
		public Operand Low { get; private set; }

		/// <summary>
		/// Gets or sets the high operand.
		/// </summary>
		/// <value>
		/// The high operand.
		/// </value>
		public Operand High { get; private set; }

		/// <summary>
		/// Gets the split64 parent.
		/// </summary>
		/// <value>
		/// The split64 parent.
		/// </value>
		public Operand SplitParent { get; private set; }

		/// <summary>
		/// Determines if the operand is a constant variable.
		/// </summary>
		public bool IsConstant { get; private set; }

		/// <summary>
		/// Determines if the operand is a symbol operand.
		/// </summary>
		public bool IsSymbol { get; private set; }

		/// <summary>
		/// Determines if the operand is a label operand.
		/// </summary>
		public bool IsLabel { get; private set; }

		/// <summary>
		/// Determines if the operand is a register.
		/// </summary>
		public bool IsRegister { get { return IsVirtualRegister || IsCPURegister; } }

		/// <summary>
		/// Determines if the operand is a virtual register operand.
		/// </summary>
		public bool IsVirtualRegister { get; private set; }

		/// <summary>
		/// Determines if the operand is a cpu register operand.
		/// </summary>
		public bool IsCPURegister { get; private set; }

		/// <summary>
		/// Determines if the operand is a memory operand.
		/// </summary>
		public bool IsMemoryAddress { get; private set; }

		/// <summary>
		/// Determines if the operand is a stack local operand.
		/// </summary>
		public bool IsStackLocal { get; private set; }

		/// <summary>
		/// Determines if the operand is a runtime member operand.
		/// </summary>
		public bool IsField { get; private set; }

		/// <summary>
		/// Determines if the operand is a local variable operand.
		/// </summary>
		public bool IsParameter { get; private set; }

		/// <summary>
		/// Determines if the operand is a ssa operand.
		/// </summary>
		public bool IsSSA { get; private set; }

		/// <summary>
		/// Gets a value indicating whether this instance is split64 child.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is split64 child; otherwise, <c>false</c>.
		/// </value>
		public bool IsSplitChild { get { return SplitParent != null && !IsSSA; } }

		/// <summary>
		/// Determines if the operand is a shift operand.
		/// </summary>
		public bool IsShift { get; private set; }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the index.
		/// </summary>
		public int Index { get; private set; }

		/// <summary>
		/// Gets the constant integer.
		/// </summary>
		/// <value>
		/// The constant integer.
		/// </value>
		public ulong ConstantUnsignedInteger { get; private set; }

		/// <summary>
		/// Gets the constant double float point.
		/// </summary>
		/// <value>
		/// The constant double float point.
		/// </value>
		public double ConstantDoubleFloatingPoint { get; private set; }

		/// <summary>
		/// Gets the single double float point.
		/// </summary>
		/// <value>
		/// The single double float point.
		/// </value>
		public float ConstantSingleFloatingPoint { get; private set; }

		/// <summary>
		/// Gets or sets the constant signed integer.
		/// </summary>
		/// <value>
		/// The constant signed integer.
		/// </value>
		public long ConstantSignedInteger { get { return (long)ConstantUnsignedInteger; } set { ConstantUnsignedInteger = (ulong)value; } }

		/// <summary>
		/// Gets the string data.
		/// </summary>
		/// <value>
		/// The string data.
		/// </value>
		public string StringData { get; private set; }

		/// <summary>
		/// Gets a value indicating whether [is null].
		/// </summary>
		/// <value>
		///   <c>true</c> if [is null]; otherwise, <c>false</c>.
		/// </value>
		public bool IsNull { get; private set; }

		/// <summary>
		/// Gets a value indicating whether [is 64 bit integer].
		/// </summary>
		/// <value>
		///   <c>true</c> if [is signed integer]; otherwise, <c>false</c>.
		/// </value>
		public bool Is64BitInteger { get { return IsLong; } }

		/// <summary>
		/// Gets the type of the shift.
		/// </summary>
		/// <value>
		/// The type of the shift.
		/// </value>
		public ShiftType ShiftType { get; private set; }

		/// <summary>
		/// Gets a value indicating whether [is constant zero].
		/// </summary>
		/// <value>
		///   <c>true</c> if [is constant zero]; otherwise, <c>false</c>.
		/// </value>
		public bool IsConstantZero
		{
			get
			{
				if (IsInteger)
					return ConstantUnsignedInteger == 0;
				else if (IsR8)
					return ConstantDoubleFloatingPoint == 0;
				else if (IsR4)
					return ConstantSingleFloatingPoint == 0;

				throw new InvalidCompilerException();
			}
		}

		/// <summary>
		/// Gets a value indicating whether [is constant one].
		/// </summary>
		/// <value>
		///   <c>true</c> if [is constant one]; otherwise, <c>false</c>.
		/// </value>
		/// <exception cref="InvalidCompilerException"></exception>
		public bool IsConstantOne
		{
			get
			{
				if (IsInteger)
					return ConstantUnsignedInteger == 1;
				else if (IsR8)
					return ConstantDoubleFloatingPoint == 1;
				else if (IsR4)
					return ConstantSingleFloatingPoint == 1;

				throw new InvalidCompilerException();
			}
		}

		public bool IsR { get { return underlyingType.IsR; } }

		public bool IsR8 { get { return underlyingType.IsR8; } }

		public bool IsR4 { get { return underlyingType.IsR4; } }

		public bool IsInteger { get { return underlyingType.IsInteger; } }

		public bool IsSigned { get { return underlyingType.IsSigned; } }

		public bool IsUnsigned { get { return underlyingType.IsUnsigned; } }

		public bool IsU1 { get { return underlyingType.IsU1; } }

		public bool IsI1 { get { return underlyingType.IsI1; } }

		public bool IsU2 { get { return underlyingType.IsU2; } }

		public bool IsI2 { get { return underlyingType.IsI2; } }

		public bool IsU4 { get { return underlyingType.IsU4; } }

		public bool IsI4 { get { return underlyingType.IsI4; } }

		public bool IsU8 { get { return underlyingType.IsU8; } }

		public bool IsI8 { get { return underlyingType.IsI8; } }

		public bool IsByte { get { return underlyingType.IsUI1; } }

		public bool IsShort { get { return underlyingType.IsUI2; } }

		public bool IsChar { get { return underlyingType.IsChar; } }

		public bool IsInt { get { return underlyingType.IsUI4; } }

		public bool IsLong { get { return underlyingType.IsUI8; } }

		public bool IsBoolean { get { return Type.IsBoolean; } }

		public bool IsPointer { get { return Type.IsPointer; } }

		public bool IsValueType { get { return underlyingType.IsValueType; } }

		public bool IsArray { get { return Type.IsArray; } }

		public bool IsI { get { return underlyingType.IsI; } }

		public bool IsU { get { return underlyingType.IsU; } }

		public bool IsReferenceType { get { return Type.IsReferenceType; } }

		private MosaType underlyingType { get { return Type.GetEnumUnderlyingType(); } }

		#endregion Properties

		#region Construction

		private Operand()
		{
			Definitions = new List<int>();
			Uses = new List<int>();
			this.IsParameter = false;
			this.IsStackLocal = false;
			this.IsShift = false;
			this.IsConstant = false;
			this.IsVirtualRegister = false;
			this.IsLabel = false;
			this.IsCPURegister = false;
			this.IsMemoryAddress = false;
			this.IsSSA = false;
			this.IsSymbol = false;
			this.IsField = false;
			this.IsParameter = false;
		}

		/// <summary>
		/// Initializes a new instance of <see cref="Operand"/>.
		/// </summary>
		/// <param name="type">The type of the operand.</param>
		private Operand(MosaType type)
			: this()
		{
			Debug.Assert(type != null);
			Type = type;
		}

		/// <summary>
		/// Prevents a default instance of the <see cref="Operand"/> class from being created.
		/// </summary>
		/// <param name="shiftType">Type of the shift.</param>
		private Operand(ShiftType shiftType)
			: this()
		{
			this.ShiftType = shiftType;
			this.IsShift = true;
		}

		#endregion Construction

		#region Static Factory Constructors

		/// <summary>
		/// Creates a new constant <see cref="Operand" /> for the given integral value.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="value">The value to create the constant operand for.</param>
		/// <returns>
		/// A new operand representing the value <paramref name="value" />.
		/// </returns>
		/// <exception cref="InvalidCompilerException"></exception>
		public static Operand CreateConstant(MosaType type, ulong value)
		{
			var operand = new Operand(type);
			operand.IsConstant = true;

			if (operand.IsUnsigned)
				operand.ConstantUnsignedInteger = value;
			else if (operand.IsSigned)
				operand.ConstantSignedInteger = (int)value;
			else if (operand.IsBoolean)
				operand.ConstantUnsignedInteger = value;
			else if (operand.IsChar)
				operand.ConstantUnsignedInteger = value;
			else
				throw new InvalidCompilerException();

			return operand;
		}

		/// <summary>
		/// Creates a new constant <see cref="Operand" /> for the given integral value.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="value">The value to create the constant operand for.</param>
		/// <returns>
		/// A new operand representing the value <paramref name="value" />.
		/// </returns>
		public static Operand CreateConstant(MosaType type, long value)
		{
			return CreateConstant(type, (ulong)value);
		}

		/// <summary>
		/// Creates a new constant <see cref="Operand" /> for the given integral value.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="value">The value to create the constant operand for.</param>
		/// <returns>
		/// A new operand representing the value <paramref name="value" />.
		/// </returns>
		public static Operand CreateConstant(MosaType type, int value)
		{
			return CreateConstant(type, (long)value);
		}

		/// <summary>
		/// Creates the constant unsigned byte.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static Operand CreateConstantUnsignedByte(TypeSystem typeSystem, byte value)
		{
			var operand = new Operand(typeSystem.BuiltIn.U1);
			operand.IsConstant = true;
			operand.ConstantUnsignedInteger = value;
			return operand;
		}

		/// <summary>
		/// Creates a new constant <see cref="Operand" /> for the given integral value.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="value">The value to create the constant operand for.</param>
		/// <returns>
		/// A new operand representing the value <paramref name="value" />.
		/// </returns>
		public static Operand CreateConstantUnsignedInt(TypeSystem typeSystem, uint value)
		{
			var operand = new Operand(typeSystem.BuiltIn.U4);
			operand.IsConstant = true;
			operand.ConstantUnsignedInteger = value;
			return operand;
		}

		/// <summary>
		/// Creates a new constant <see cref="Operand" /> for the given integral value.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="value">The value to create the constant operand for.</param>
		/// <returns>
		/// A new operand representing the value <paramref name="value" />.
		/// </returns>
		public static Operand CreateConstantSignedInt(TypeSystem typeSystem, int value)
		{
			var operand = new Operand(typeSystem.BuiltIn.I4);
			operand.IsConstant = true;
			operand.ConstantSignedInteger = value;
			return operand;
		}

		/// <summary>
		/// Creates a new constant <see cref="Operand" /> for the given integral value.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="value">The value to create the constant operand for.</param>
		/// <returns>
		/// A new operand representing the value <paramref name="value" />.
		/// </returns>
		public static Operand CreateConstantUnsignedLong(TypeSystem typeSystem, ulong value)
		{
			var operand = new Operand(typeSystem.BuiltIn.U8);
			operand.IsConstant = true;
			operand.ConstantUnsignedInteger = value;
			return operand;
		}

		/// <summary>
		/// Creates a new constant <see cref="Operand" /> for the given integral value.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="value">The value to create the constant operand for.</param>
		/// <returns>
		/// A new operand representing the value <paramref name="value" />.
		/// </returns>
		public static Operand CreateConstantSignedLong(TypeSystem typeSystem, long value)
		{
			var operand = new Operand(typeSystem.BuiltIn.I8);
			operand.IsConstant = true;
			operand.ConstantSignedInteger = value;
			return operand;
		}

		/// <summary>
		/// Creates a new constant <see cref="Operand" /> for the given integral value.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="value">The value to create the constant operand for.</param>
		/// <returns>
		/// A new operand representing the value <paramref name="value" />.
		/// </returns>
		public static Operand CreateConstantSingle(TypeSystem typeSystem, float value)
		{
			var operand = new Operand(typeSystem.BuiltIn.R4);
			operand.IsConstant = true;
			operand.ConstantSingleFloatingPoint = value;
			return operand;
		}

		/// <summary>
		/// Creates a new constant <see cref="Operand" /> for the given integral value.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="value">The value to create the constant operand for.</param>
		/// <returns>
		/// A new operand representing the value <paramref name="value" />.
		/// </returns>
		public static Operand CreateConstantDouble(TypeSystem typeSystem, double value)
		{
			var operand = new Operand(typeSystem.BuiltIn.R8);
			operand.IsConstant = true;
			operand.ConstantDoubleFloatingPoint = value;
			return operand;
		}

		/// <summary>
		/// Gets the null constant <see cref="Operand"/>.
		/// </summary>
		/// <returns></returns>
		public static Operand GetNull(TypeSystem typeSystem)
		{
			var operand = new Operand(typeSystem.BuiltIn.Object);
			operand.IsNull = true;
			operand.IsConstant = true;
			return operand;
		}

		/// <summary>
		/// Creates the symbol.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static Operand CreateUnmanagedSymbolPointer(TypeSystem typeSystem, string name)
		{
			var operand = new Operand(typeSystem.BuiltIn.Pointer);
			operand.IsSymbol = true;
			operand.Name = name;
			return operand;
		}

		/// <summary>
		/// Creates the symbol.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="type">The type.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static Operand CreateManagedSymbolPointer(TypeSystem typeSystem, MosaType type, string name)
		{
			// NOTE: Not being used
			var operand = new Operand(type.ToManagedPointer());
			operand.IsSymbol = true;
			operand.Name = name;
			return operand;
		}

		/// <summary>
		/// Creates the symbol.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="type">The type.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static Operand CreateManagedSymbol(TypeSystem typeSystem, MosaType type, string name)
		{
			var operand = new Operand(type);
			operand.IsSymbol = true;
			operand.Name = name;
			return operand;
		}

		/// <summary>
		/// Creates the string symbol with data.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="name">The name.</param>
		/// <param name="data">The string data.</param>
		/// <returns></returns>
		public static Operand CreateStringSymbol(TypeSystem typeSystem, string name, string data)
		{
			Debug.Assert(data != null);

			var operand = new Operand(typeSystem.BuiltIn.String);
			operand.IsSymbol = true;
			operand.Name = name;
			operand.StringData = data;
			return operand;
		}

		/// <summary>
		/// Creates a new symbol <see cref="Operand" /> for the given symbol name.
		/// </summary>
		/// <param name="typeSystem">The type system.</param>
		/// <param name="method">The method.</param>
		/// <returns></returns>
		public static Operand CreateSymbolFromMethod(TypeSystem typeSystem, MosaMethod method)
		{
			Operand operand = CreateUnmanagedSymbolPointer(typeSystem, method.FullName);
			operand.Method = method;
			return operand;
		}

		/// <summary>
		/// Creates a new virtual register <see cref="Operand" />.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static Operand CreateVirtualRegister(MosaType type, int index)
		{
			var operand = new Operand(type);
			operand.IsVirtualRegister = true;
			operand.Index = index;
			return operand;
		}

		/// <summary>
		/// Creates a new virtual register <see cref="Operand" />.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="index">The index.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static Operand CreateVirtualRegister(MosaType type, int index, string name)
		{
			var operand = new Operand(type);
			operand.IsVirtualRegister = true;
			operand.Name = name;
			operand.Index = index;
			return operand;
		}

		/// <summary>
		/// Creates a new physical register <see cref="Operand" />.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="register">The register.</param>
		/// <returns></returns>
		public static Operand CreateCPURegister(MosaType type, Register register)
		{
			var operand = new Operand(type);
			operand.IsCPURegister = true;
			operand.Register = register;
			return operand;
		}

		/// <summary>
		/// Creates a new memory address <see cref="Operand" />.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="offsetBase">The base register.</param>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		public static Operand CreateMemoryAddress(MosaType type, Operand offsetBase, long offset)
		{
			var operand = new Operand(type);
			operand.IsMemoryAddress = true;
			operand.OffsetBase = offsetBase;
			operand.Displacement = offset;
			return operand;
		}

		/// <summary>
		/// Creates a new symbol <see cref="Operand" /> for the given symbol name.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="label">The label.</param>
		/// <returns></returns>
		public static Operand CreateLabel(MosaType type, string label)
		{
			var operand = new Operand(type);
			operand.IsMemoryAddress = true;
			operand.IsLabel = true;
			operand.Name = label;
			operand.Displacement = 0;
			return operand;
		}
		
		/// <summary>
		/// Creates a new runtime member <see cref="Operand"/>.
		/// </summary>
		/// <param name="field">The field.</param>
		/// <returns></returns>
		public static Operand CreateField(MosaField field)
		{
			var operand = new Operand(field.FieldType);
			operand.IsMemoryAddress = true;
			operand.IsField = true;
			operand.Displacement = 0;
			operand.Field = field;
			return operand;
		}

		/// <summary>
		/// Creates a new local variable <see cref="Operand" />.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="register">The register.</param>
		/// <param name="displacement">The displacement.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static Operand CreateParameter(MosaType type, Register register, int displacement, int index, string name)
		{
			var operand = new Operand(type);
			operand.IsMemoryAddress = true;
			operand.IsParameter = true;
			operand.Register = register;
			operand.Index = index;
			operand.Displacement = displacement;
			operand.Name = name;
			return operand;
		}

		/// <summary>
		/// Creates the stack local.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="register">The register.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static Operand CreateStackLocal(MosaType type, Register register, int index)
		{
			var operand = new Operand(type);
			operand.IsMemoryAddress = true;
			operand.Register = register;
			operand.Index = index;
			operand.IsStackLocal = true;
			return operand;
		}

		/// <summary>
		/// Creates the SSA <see cref="Operand"/>.
		/// </summary>
		/// <param name="ssaOperand">The ssa operand.</param>
		/// <param name="ssaVersion">The ssa version.</param>
		/// <returns></returns>
		public static Operand CreateSSA(Operand ssaOperand, int ssaVersion)
		{
			var operand = new Operand(ssaOperand.Type);
			operand.IsParameter = ssaOperand.IsParameter;
			operand.IsStackLocal = ssaOperand.IsStackLocal;
			operand.IsShift = ssaOperand.IsShift;
			operand.IsConstant = ssaOperand.IsConstant;
			operand.IsVirtualRegister = ssaOperand.IsVirtualRegister;
			operand.IsLabel = ssaOperand.IsLabel;
			operand.IsCPURegister = ssaOperand.IsCPURegister;
			operand.IsMemoryAddress = ssaOperand.IsMemoryAddress;
			operand.IsSymbol = ssaOperand.IsSymbol;
			operand.IsField = ssaOperand.IsField;
			operand.IsParameter = ssaOperand.IsParameter;
			operand.IsSSA = true;
			operand.SSAParent = ssaOperand;
			operand.SSAVersion = ssaVersion;
			return operand;
		}

		/// <summary>
		/// Creates the low 32 bit portion of a 64-bit <see cref="Operand"/>.
		/// </summary>
		/// <param name="longOperand">The long operand.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static Operand CreateLowSplitForLong(TypeSystem typeSystem, Operand longOperand, int offset, int index)
		{
			Debug.Assert(longOperand.IsLong);

			Debug.Assert(longOperand.SplitParent == null);
			Debug.Assert(longOperand.Low == null);

			Operand operand;

			if (longOperand.IsConstant)
			{
				operand = new Operand(typeSystem.BuiltIn.U4);
				operand.IsConstant = true;
				operand.ConstantUnsignedInteger = longOperand.ConstantUnsignedInteger & uint.MaxValue;
			}
			else if (longOperand.IsField)
			{
				operand = new Operand(typeSystem.BuiltIn.U4);
				operand.IsMemoryAddress = true;
				operand.IsField = true;
				operand.Field = longOperand.Field;
				operand.Type = longOperand.Type;
				operand.OffsetBase = longOperand.OffsetBase;
				operand.Displacement = longOperand.Displacement + offset;
				operand.Register = longOperand.Register;
			}
			else if (longOperand.IsMemoryAddress)
			{
				operand = new Operand(typeSystem.BuiltIn.U4);
				operand.IsMemoryAddress = true;
				operand.OffsetBase = longOperand.OffsetBase;
				operand.Displacement = longOperand.Displacement + offset;
				operand.Register = longOperand.Register;
			}
			else
			{
				operand = new Operand(typeSystem.BuiltIn.U4);
				operand.IsVirtualRegister = true;
			}

			operand.SplitParent = longOperand;

			Debug.Assert(longOperand.Low == null);
			longOperand.Low = operand;

			operand.Index = index;

			//operand.sequence = index;
			return operand;
		}

		/// <summary>
		/// Creates the high 32 bit portion of a 64-bit <see cref="Operand"/>.
		/// </summary>
		/// <param name="longOperand">The long operand.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static Operand CreateHighSplitForLong(TypeSystem typeSystem, Operand longOperand, int offset, int index)
		{
			Debug.Assert(longOperand.IsLong);

			Debug.Assert(longOperand.SplitParent == null);
			Debug.Assert(longOperand.High == null);

			Operand operand;

			if (longOperand.IsConstant)
			{
				operand = new Operand(typeSystem.BuiltIn.U4);
				operand.IsConstant = true;
				operand.ConstantUnsignedInteger = ((uint)(longOperand.ConstantUnsignedInteger >> 32)) & uint.MaxValue;
			}
			else if (longOperand.IsField)
			{
				operand = new Operand(typeSystem.BuiltIn.U4);
				operand.IsMemoryAddress = true;
				operand.IsField = true;
				operand.Field = longOperand.Field;
				operand.Type = longOperand.Type;
				operand.OffsetBase = longOperand.OffsetBase;
				operand.Displacement = longOperand.Displacement + offset;
				operand.Register = longOperand.Register;
			}
			else if (longOperand.IsMemoryAddress)
			{
				operand = new Operand(typeSystem.BuiltIn.U4);
				operand.IsMemoryAddress = true;
				operand.OffsetBase = longOperand.OffsetBase;
				operand.Displacement = longOperand.Displacement + offset;
				operand.Register = longOperand.Register;
			}
			else
			{
				operand = new Operand(typeSystem.BuiltIn.U4);
				operand.IsVirtualRegister = true;
			}

			operand.SplitParent = longOperand;

			//operand.SplitParent = longOperand;

			Debug.Assert(longOperand.High == null);
			longOperand.High = operand;

			operand.Index = index;
			return operand;
		}

		/// <summary>
		/// Creates the shifter.
		/// </summary>
		/// <param name="shiftType">Type of the shift.</param>
		/// <returns></returns>
		public static Operand CreateShifter(ShiftType shiftType)
		{
			var operand = new Operand(shiftType);
			return operand;
		}

		#endregion Static Factory Constructors

		#region Object Overrides

		/// <summary>
		/// Returns a string representation of <see cref="Operand"/>.
		/// </summary>
		/// <returns>A string representation of the operand.</returns>
		public override string ToString()
		{
			if (IsSSA)
			{
				string ssa = SSAParent.ToString();
				int pos = ssa.IndexOf(' ');

				if (pos < 0)
					return ssa + "<" + SSAVersion + ">";
				else
					return ssa.Substring(0, pos) + "<" + SSAVersion + ">" + ssa.Substring(pos);
			}

			StringBuilder sb = new StringBuilder();

			if (IsVirtualRegister)
			{
				sb.AppendFormat("V_{0}", Index);
			}
			else if (IsStackLocal && Name == null)
			{
				sb.AppendFormat("T_{0}", Index);
			}
			else if (IsParameter && Name == null)
			{
				sb.AppendFormat("P_{0}", Index);
			}

			if (Name != null)
			{
				sb.Append(Name);
				sb.Append(' ');
			}

			if (IsField)
			{
				sb.Append(' ');
				sb.Append(Field.FullName.ToString());
			}

			if (IsSplitChild)
			{
				sb.Append(' ');

				sb.Append("(" + SplitParent.ToString() + ")");

				if (SplitParent.High == this)
					sb.Append("/high");
				else
					sb.Append("/low");
			}

			if (IsConstant)
			{
				sb.Append(" const {");

				if (IsNull)
					sb.Append("null");
				else if (IsUnsigned || IsBoolean || IsChar)
					sb.AppendFormat("{0}", ConstantUnsignedInteger);
				else if (IsSigned)
					sb.AppendFormat("{0}", ConstantSignedInteger);
				if (IsR8)
					sb.AppendFormat("{0}", ConstantDoubleFloatingPoint);
				else if (IsR4)
					sb.AppendFormat("{0}", ConstantSingleFloatingPoint);

				sb.Append('}');
			}

			if (IsCPURegister)
			{
				sb.AppendFormat(" {0}", Register);
			}
			else if (IsMemoryAddress)
			{
				sb.Append(' ');
				if (OffsetBase != null)
				{
					if (Displacement > 0)
						sb.AppendFormat("[{0}+{1:X}h]", OffsetBase.ToString(), Displacement);
					else
						sb.AppendFormat("[{0}-{1:X}h]", OffsetBase.ToString(), -Displacement);
				}
				else if (Register != null)
				{
					if (Displacement > 0)
						sb.AppendFormat("[{0}+{1:X}h]", Register.ToString(), Displacement);
					else
						sb.AppendFormat("[{0}-{1:X}h]", Register.ToString(), -Displacement);
				}
				else if (IsField && IsSplitChild)
				{
					if (Displacement > 0)
						sb.AppendFormat("+{0:X}h", Displacement);
					else
						sb.AppendFormat("-{0:X}h", -Displacement);
				}
			}

			sb.AppendFormat(" [{0}]", Type.FullName);

			return sb.ToString().Replace("  ", " ").Trim();
		}

		#endregion Object Overrides
	}
}