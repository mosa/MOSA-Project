/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Emits metadata for assemblies and types
	/// </summary>
	public sealed class MetadataStage : BaseCompilerStage
	{
		protected override void Run()
		{
			CreateDefinitionTables();
		}

		#region Helper Functions

		private LinkerSymbol EmitStringWithLength(string name, string value)
		{
			var symbol = Linker.CreateSymbol(name, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var stream = new EndianAwareBinaryWriter(symbol.Stream, Architecture.Endianness);
			stream.Write(value.Length);
			stream.Write(System.Text.ASCIIEncoding.ASCII.GetBytes(value));
			return symbol;
		}

		#endregion

		#region Assembly Tables

		private void CreateDefinitionTables()
		{
			// Emit assembly list
			var assemblyListSymbol = Linker.CreateSymbol(Metadata.AssemblyListTable, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(assemblyListSymbol.Stream, Architecture.Endianness);

			// 1. Number of Assemblies
			writer1.Write((uint)TypeSystem.Modules.Count);

			// 2. Pointers to Assemblies
			// Create the definitions along the way
			foreach (var module in TypeSystem.Modules)
			{
				var assemblyTableSymbol = CreateAssemblyDefinitionTable(module);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, assemblyListSymbol, (int)writer1.Position, 0, assemblyTableSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}
		}

		private LinkerSymbol CreateAssemblyDefinitionTable(MosaModule module)
		{
			// Emit assembly name
			var assemblyNameSymbol = EmitStringWithLength(module.Assembly + Metadata.NameString, module.Assembly);

			// Emit assembly table
			var assemblyTableSymbol = Linker.CreateSymbol(module.Assembly + Metadata.AssemblyDefinition, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(assemblyTableSymbol.Stream, Architecture.Endianness);

			// 1. Pointer to Assembly Name
			Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, assemblyTableSymbol, (int)writer1.Position, 0, assemblyNameSymbol, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 2. Pointer to Custom Attributes
			if (module.CustomAttributes.Count > 0)
			{
				var customAttributeListSymbol = CreateCustomAttributeListTable(module);
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, assemblyTableSymbol, (int)writer1.Position, 0, customAttributeListSymbol, 0);
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 3. Flags: IsReflectionOnly (32bit length)
			uint flags = 0x0;
			if (module.IsReflectionOnly) flags |= 0x1;
			writer1.Write(flags);

			// 4. Number of Types
			uint count = 0;
			foreach (var type in module.Types.Values)
			{
				if (type.IsModule)
					continue;

				count++;
			}
			writer1.Write(count);

			// 5. Pointers to Types
			// Create the definitions along the way
			foreach (var type in module.Types.Values)
			{
				if (type.IsModule)
					continue;

				// Run the type through the TypeLayout system
				TypeLayout.ResolveType(type);

				var typeTableSymbol = CreateTypeDefinitionTable(type, assemblyTableSymbol);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, assemblyTableSymbol, (int)writer1.Position, 0, typeTableSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			// Return assemblyTableSymbol
			return assemblyTableSymbol;
		}

		#endregion

		#region TypeDefinition

		private LinkerSymbol CreateTypeDefinitionTable(MosaType type, LinkerSymbol assemblyTableSymbol)
		{
			// Emit type name
			var typeNameSymbol = EmitStringWithLength(type + Metadata.NameString, type.FullName);

			// Emit type table
			var typeTableSymbol = Linker.CreateSymbol(type.FullName + Metadata.TypeDefinition, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(typeTableSymbol.Stream, Architecture.Endianness);

			// 1. Pointer to Name
			Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, typeTableSymbol, (int)writer1.Position, 0, typeNameSymbol, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 2. Pointer to Custom Attributes
			if (type.CustomAttributes.Count > 0)
			{
				var customAttributeListSymbol = CreateCustomAttributeListTable(type);
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, typeTableSymbol, (int)writer1.Position, 0, customAttributeListSymbol, 0);
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 3. Flags: IsInterface, HasGenericParams (32bit length)
			uint flags = 0x0;
			if (type.IsInterface) flags |= 0x1;
			if (type.HasOpenGenericParams) flags |= 0x2;
			writer1.Write(flags);

			// 4. Size
			writer1.Write((uint)TypeLayout.GetTypeSize(type));

			// 5. Pointer to Assembly Definition
			Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, typeTableSymbol, (int)writer1.Position, 0, assemblyTableSymbol, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 6. Pointer to Parent Type
			if (type.BaseType != null)
			{
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, typeTableSymbol, (int)writer1.Position, 0, type.BaseType.FullName + Metadata.TypeDefinition, SectionKind.ROData, 0);
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 7. Constructor that accepts no parameters, if any, for this type
			foreach (var method in type.Methods)
			{
				if (!method.Name.Equals(".ctor") || !(method.Signature.Parameters.Count == 0) || method.HasOpenGenericParams)
					continue;

				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, typeTableSymbol, (int)writer1.Position, 0, method.FullName + Metadata.MethodDefinition, SectionKind.ROData, 0);
				break;
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// If the type is not an interface continue, otherwise just pad until the end
			if (!type.IsInterface)
			{
				// If the type doesn't use interfaces then skip 8 and 9
				if (type.Interfaces.Count > 0)
				{
					// 8. Pointer to Interface Slots
					var interfaceSlotTableSymbol = CreateInterfaceSlotTable(type);
					Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, typeTableSymbol, (int)writer1.Position, 0, interfaceSlotTableSymbol, 0);
					writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

					// 9. Pointer to Interface Bitmap
					var interfaceBitmapSymbol = CreateInterfaceBitmap(type);
					Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, typeTableSymbol, (int)writer1.Position, 0, interfaceBitmapSymbol, 0);
					writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
				}
				else
				{
					// Fill 8 and 9 with zeros
					writer1.WriteZeroBytes(TypeLayout.NativePointerSize * 2);
				}

				// For the next part we'll need to get the list of methods from the MosaTypeLayout
				var methodList = TypeLayout.GetMethodTable(type) ?? new List<MosaMethod>();

				// 10. Number of Methods
				writer1.Write(methodList.Count);

				// 11. Pointer to Method Definitions
				foreach (MosaMethod method in methodList)
				{
					// Create definition and get the symbol
					var methodDefinitionSymbol = CreateMethodDefinitionTable(method);

					// Link
					Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, typeTableSymbol, (int)writer1.Position, 0, methodDefinitionSymbol, 0);
					writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
				}
			}
			else
			{
				// Fill 8, 9, 10 with zeros, 11 can be left out.
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize * 3);
			}

			CreateFieldDefinitions(type);

			// Return typeTableSymbol for linker usage
			return typeTableSymbol;
		}

		#endregion

		#region Interface Bitmap and Tables

		private LinkerSymbol CreateInterfaceBitmap(MosaType type)
		{
			var bitmap = new byte[(((TypeLayout.Interfaces.Count - 1) / 8) + 1)];

			int at = 0;
			byte bit = 0;
			foreach (var interfaceType in TypeLayout.Interfaces)
			{
				if (type.Interfaces.Contains(interfaceType))
				{
					bitmap[at] = (byte)(bitmap[at] | (byte)(1 << bit));
				}

				bit++;
				if (bit == 8)
				{
					bit = 0;
					at++;
				}
			}

			var symbol = Linker.CreateSymbol(type.FullName + Metadata.InterfaceBitmap, SectionKind.ROData, TypeLayout.NativePointerAlignment, bitmap.Length);
			symbol.Stream.Write(bitmap);

			// Return symbol for linker usage
			return symbol;
		}

		private LinkerSymbol CreateInterfaceSlotTable(MosaType type)
		{
			// Emit interface slot table
			var interfaceSlotTableSymbol = Linker.CreateSymbol(type.FullName + Metadata.InterfaceSlotTable, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(interfaceSlotTableSymbol.Stream, Architecture.Endianness);

			var slots = new List<MosaType>(TypeLayout.Interfaces.Count);

			foreach (MosaType interfaceType in TypeLayout.Interfaces)
			{
				if (type.Interfaces.Contains(interfaceType))
					slots.Add(interfaceType);
				else
					slots.Add(null);
			}

			// 1. Number of Interface slots
			writer1.Write((uint)slots.Count);

			// 2. Pointers to Interface Method Tables
			foreach (MosaType interfaceType in slots)
			{
				if (interfaceType != null)
				{
					var interfaceMethodTableSymbol = CreateInterfaceMethodTable(type, interfaceType);

					// Link
					Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, interfaceSlotTableSymbol, (int)writer1.Position, 0, interfaceMethodTableSymbol, 0);
				}
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			// Return interfaceSlotTableSymbol for linker usage
			return interfaceSlotTableSymbol;
		}

		private LinkerSymbol CreateInterfaceMethodTable(MosaType type, MosaType interfaceType)
		{
			// Emit interface method table
			var interfaceMethodTableSymbol = Linker.CreateSymbol(type.FullName + Metadata.InterfaceMethodTable + interfaceType.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(interfaceMethodTableSymbol.Stream, Architecture.Endianness);

			var interfaceMethodTable = TypeLayout.GetInterfaceTable(type, interfaceType) ?? new MosaMethod[0];

			// 1. Number of Interface Methods
			writer1.Write((uint)interfaceMethodTable.Length);

			// 2. Pointers to Method Definitions
			foreach (MosaMethod method in interfaceMethodTable)
			{
				// Create definition and get the symbol
				var methodDefinitionSymbol = CreateMethodDefinitionTable(method);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, interfaceMethodTableSymbol, (int)writer1.Position, 0, methodDefinitionSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			// Return interfaceMethodTableSymbol for linker usage
			return interfaceMethodTableSymbol;
		}

		#endregion

		#region MethodDefinition

		private LinkerSymbol CreateMethodDefinitionTable(MosaMethod method)
		{
			// Emit type name
			var methodNameSymbol = EmitStringWithLength(method + Metadata.NameString, method.FullName);

			// Emit type table
			var methodTableSymbol = Linker.CreateSymbol(method.FullName + Metadata.MethodDefinition, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(methodTableSymbol.Stream, Architecture.Endianness);

			// 1. Pointer to Name
			Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, methodTableSymbol, (int)writer1.Position, 0, methodNameSymbol, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 2. Pointer to Custom Attributes
			if (method.CustomAttributes.Count > 0)
			{
				var customAttributeListSymbol = CreateCustomAttributeListTable(method);
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, methodTableSymbol, (int)writer1.Position, 0, customAttributeListSymbol, 0);
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 3. Flags: IsStatic, IsAbstract, HasGenericParams (32bit length)
			uint flags = 0x0;
			if (method.IsStatic) flags |= 0x1;
			if (method.IsAbstract) flags |= 0x2;
			if (method.HasOpenGenericParams) flags |= 0x4;
			writer1.Write(flags);

			// 4. Local Stack Size (High 16bits) and Parameter Stack Size (Low 16bits)
			uint paramStackSize = method.MaxStack << 16;
			foreach (MosaParameter param in method.Signature.Parameters)
			{
				paramStackSize += (uint)TypeLayout.GetTypeSize(param.Type);
			}
			writer1.Write(paramStackSize);


			// 5. Pointer to Method
			if (!method.IsAbstract)
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, methodTableSymbol, (int)writer1.Position, 0, method.FullName, SectionKind.Text, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 6. Pointer to return Type
			Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, methodTableSymbol, (int)writer1.Position, 0, method.Signature.ReturnType.FullName + Metadata.TypeDefinition, SectionKind.ROData, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 7. Pointer to Exception Hanlder Table
			// TODO: This has yet to be designed.

			// 8. Pointer to GC Tracking information
			// TODO: This has yet to be designed.

			// 9. Pointer to Parameter Definitions
			// TODO: This has yet to be designed.

			// Return methodTableSymbol for linker usage
			return methodTableSymbol;
		}

		#endregion

		#region FieldDefinition

		private void CreateFieldDefinitions(MosaType type)
		{
			foreach (MosaField field in type.Fields)
			{
				// Emit field name
				var fieldNameSymbol = EmitStringWithLength(field.FullName + Metadata.NameString, field.Name);

				// Emit field definition
				var fieldDefSymbol = Linker.CreateSymbol(field.FullName + Metadata.FieldDefinition, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
				var writer1 = new EndianAwareBinaryWriter(fieldDefSymbol.Stream, Architecture.Endianness);

				// 1. Name
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, fieldDefSymbol, (int)writer1.Position, 0, fieldNameSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

				// 2 & 3. Offset / Address + Size
				if (field.IsStatic && !field.IsLiteral)
				{
					var section = (field.Data != null) ? SectionKind.ROData : SectionKind.BSS;
					Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, fieldDefSymbol, (int)writer1.Position, 0, field.FullName, section, 0);
					writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
					writer1.Write((field.Data != null) ? field.Data.Length : 0);
				}
				else
				{
					writer1.Write(TypeLayout.GetFieldOffset(field));
				}

				// Create another symbol with field data if any
				if (field.IsStatic)
				{
					// Assign a memory slot to the static & initialize it, if there's initial data set
					// Determine the size of the type & alignment requirements
					int size, alignment;
					Architecture.GetTypeRequirements(TypeLayout, field.FieldType, out size, out alignment);

					size = (int)TypeLayout.GetFieldSize(field);

					// The linker section to move this field into
					SectionKind section = field.Data != null ? section = SectionKind.ROData : section = SectionKind.BSS;

					var symbol = Compiler.Linker.CreateSymbol(field.FullName, section, alignment, size);

					if (field.Data != null)
					{
						symbol.Stream.Write(field.Data, 0, size);
					}
				}
			}
		}

		#endregion

		#region Custom Attributes

		private LinkerSymbol CreateCustomAttributeListTable(MosaUnit unit)
		{
			// Emit custom attribute list
			var customAttributeListTableSymbol = Linker.CreateSymbol(unit.FullName + Metadata.CustomAttributeListTable, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(customAttributeListTableSymbol.Stream, Architecture.Endianness);

			// 1. Number of Custom Attributes
			writer1.Write(unit.CustomAttributes.Count);

			// 2. Pointers to Custom Attributes
			// Create the definitions along the way
			for (int i = 0; i < unit.CustomAttributes.Count; i++)
			{
				// Get custom attribute
				var ca = unit.CustomAttributes[i];

				// Build definition
				var customAttributeTableSymbol = CreateCustomAttributeTable(unit, ca, i);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, customAttributeListTableSymbol, (int)writer1.Position, 0, customAttributeTableSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			// Return customAttributeListTableSymbol for linker usage
			return customAttributeListTableSymbol;
		}

		private LinkerSymbol CreateCustomAttributeTable(MosaUnit unit, MosaCustomAttribute ca, int position)
		{
			// Emit custom attribute list
			string name = unit.FullName + ">>" + position.ToString() + ":" + ca.Constructor.DeclaringType.Name;
			var customAttributeTableSymbol = Linker.CreateSymbol(name + Metadata.CustomAttributeTable, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(customAttributeTableSymbol.Stream, Architecture.Endianness);

			// 1. Pointer to Attribute Type
			Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, customAttributeTableSymbol, (int)writer1.Position, 0, ca.Constructor.DeclaringType.FullName + Metadata.TypeDefinition, SectionKind.ROData, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 2. Pointer to Constructor Method Definition
			Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, customAttributeTableSymbol, (int)writer1.Position, 0, ca.Constructor.FullName + Metadata.MethodDefinition, SectionKind.ROData, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 3. Number of Arguments (Both unnamed and named)
			writer1.Write((uint)(ca.Arguments.Length + ca.NamedArguments.Length));

			// 4. Pointers to Custom Attribute Arguments (Both unnamed and named)
			for (int i = 0; i < ca.Arguments.Length; i++)
			{
				// Build definition
				var customAttributeArgumentSymbol = CreateCustomAttributeArgument(name, i, null, ca.Arguments[i]);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, customAttributeTableSymbol, (int)writer1.Position, 0, customAttributeArgumentSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			foreach (var namedArg in ca.NamedArguments)
			{
				// Build definition
				var customAttributeArgumentSymbol = CreateCustomAttributeArgument(name, 0, namedArg.Name, namedArg.Argument);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, customAttributeTableSymbol, (int)writer1.Position, 0, customAttributeArgumentSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			// Return customAttributeTableSymbol for linker usage
			return customAttributeTableSymbol;
		}

		private int ComputeArgumentSize(MosaType type, object value)
		{
			if (type.IsEnum)
				type = type.GetEnumUnderlyingType();
			switch (type.TypeCode)
			{
				// 1 byte
				case MosaTypeCode.Boolean:
				case MosaTypeCode.U1:
				case MosaTypeCode.I1:
					return 1;

				// 2 bytes
				case MosaTypeCode.Char:
				case MosaTypeCode.U2:
				case MosaTypeCode.I2:
					return 2;

				// 4 bytes
				case MosaTypeCode.U4:
				case MosaTypeCode.I4:
				case MosaTypeCode.R4:
					return 4;

				// 8 bytes
				case MosaTypeCode.U8:
				case MosaTypeCode.I8:
				case MosaTypeCode.R8:
					return 8;

				default:
					if (type.IsArray)
					{
						Debug.Assert(value is MosaCustomAttribute.Argument[]);
						var arr = (MosaCustomAttribute.Argument[])value;
						int size = 0;
						foreach (var elem in arr)
							size += ComputeArgumentSize(elem.Type, elem.Value);
						return size;
					}
					else if (type.IsReferenceType) // System.String or System.Type
					{
						return TypeLayout.NativePointerSize;
					}
					else
						throw new NotSupportedException();
			}
		}

		private void WriteArgument(EndianAwareBinaryWriter writer, LinkerSymbol symbol, MosaType type, object value)
		{
			if (type.IsEnum)
				type = type.GetEnumUnderlyingType();
			switch (type.TypeCode)
			{
				// 1 byte
				case MosaTypeCode.Boolean:
					writer.Write((bool)value);
					break;
				case MosaTypeCode.U1:
					writer.Write((byte)value);
					break;
				case MosaTypeCode.I1:
					writer.Write((sbyte)value);
					break;

				// 2 bytes
				case MosaTypeCode.Char:
					writer.Write((char)value);
					break;
				case MosaTypeCode.U2:
					writer.Write((ushort)value);
					break;
				case MosaTypeCode.I2:
					writer.Write((short)value);
					break;

				// 4 bytes
				case MosaTypeCode.U4:
					writer.Write((uint)value);
					break;
				case MosaTypeCode.I4:
					writer.Write((int)value);
					break;
				case MosaTypeCode.R4:
					writer.Write((float)value);
					break;

				// 8 bytes
				case MosaTypeCode.U8:
					writer.Write((ulong)value);
					break;
				case MosaTypeCode.I8:
					writer.Write((long)value);
					break;
				case MosaTypeCode.R8:
					writer.Write((double)value);
					break;

				default:
					if (type.IsArray)
					{
						Debug.Assert(value is MosaCustomAttribute.Argument[]);
						var arr = (MosaCustomAttribute.Argument[])value;
						writer.Write(arr.Length);
						foreach (var elem in arr)
							WriteArgument(writer, symbol, elem.Type, elem.Value);
					}
					else if (type.IsReferenceType) // System.String or System.Type
					{
						if (type.TypeCode == MosaTypeCode.String)
						{
							var str = (string)value;
							writer.Write(str.Length);
							writer.Write(System.Text.Encoding.Unicode.GetBytes(str));
						}
						else if (type.FullName == "System.Type")
						{
							var valueType = (MosaType)value;
							Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, symbol, (int)writer.Position, 0, valueType.FullName + Metadata.TypeDefinition, SectionKind.ROData, 0);
							writer.WriteZeroBytes(TypeLayout.NativePointerSize);
						}
						else
							throw new NotSupportedException();
					}
					else
						throw new NotSupportedException();
					break;
			}
		}

		private LinkerSymbol CreateCustomAttributeArgument(string symbolName, int count, string name, MosaCustomAttribute.Argument arg)
		{
			string nameForSymbol = (name == null) ? count.ToString() : name;
			nameForSymbol = symbolName + ":" + nameForSymbol;
			var symbol = Linker.CreateSymbol(nameForSymbol + Metadata.CustomAttributeArgument, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(symbol.Stream, Architecture.Endianness);

			// 1. Pointer to name (if named)
			if (name != null)
			{
				var nameSymbol = EmitStringWithLength(nameForSymbol + Metadata.NameString, name);
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, symbol, (int)writer1.Position, 0, nameSymbol, 0);
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 2. Argument Type Pointer
			Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, symbol, (int)writer1.Position, 0, arg.Type.FullName + Metadata.TypeDefinition, SectionKind.ROData, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 3. Argument Size
			writer1.Write(ComputeArgumentSize(arg.Type, arg.Value));

			// 4. Argument Value
			WriteArgument(writer1, symbol, arg.Type, arg.Value);

			// Return symbol for linker usage
			return symbol;
		}

		#endregion
	}
}