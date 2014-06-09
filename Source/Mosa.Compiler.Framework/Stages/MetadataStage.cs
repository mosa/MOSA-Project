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

			// 4. Number of Types
			uint count = 0;
			foreach (var type in module.Types.Values)
			{
				if (type.IsModule/* || (type.BaseType == null && !type.IsInterface && type.FullName != "System.Object")*/)   // ghost types like generic params, function ptr, etc.
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

				/*if (type.BaseType == null && !type.IsInterface && type.FullName != "System.Object")   // ghost types like generic params, function ptr, etc.
					continue;*/

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

			// 3. Size
			writer1.Write((uint)TypeLayout.GetTypeSize(type));

			// 4. Pointer to Assembly Definition
			Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, typeTableSymbol, (int)writer1.Position, 0, assemblyTableSymbol, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 4. Pointer to Parent Type
			if (type.BaseType != null)
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, typeTableSymbol, (int)writer1.Position, 0, type.BaseType.FullName + Metadata.TypeDefinition, SectionKind.ROData, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 6. Constructor that accepts no parameters, if any, for this type
			foreach (var method in type.Methods)
			{
				if (!method.Name.Equals(".ctor") || !(method.Signature.Parameters.Count == 0) || method.HasOpenGenericParams)
					continue;

				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, typeTableSymbol, (int)writer1.Position, 0, method.FullName + Metadata.MethodDefinition, SectionKind.ROData, 0);
				break;
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 7. Flag: IsInterface
			writer1.WriteByte((byte)(type.IsInterface ? 1 : 0));

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

				// 10. Number of Methods
				uint count = 0;
				foreach (MosaMethod method in type.Methods)
				{
					if (!method.IsAbstract && !method.IsInternal && method.ExternMethod == null && !method.HasOpenGenericParams) count++;
				}
				writer1.Write(count);

				// 11. Pointer to Method Definitions
				foreach (MosaMethod method in type.Methods)
				{
					if (method.IsAbstract || method.IsInternal || !(method.ExternMethod == null) || method.HasOpenGenericParams)
						continue;

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

			var methodTable = TypeLayout.GetInterfaceTable(type, interfaceType) ?? new MosaMethod[0];

			// 1. Number of Interface Methods
			writer1.Write((uint)methodTable.Length);

			// 2. Pointers to Method Definitions
			foreach (MosaMethod method in methodTable)
			{
				// Link
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, interfaceMethodTableSymbol, (int)writer1.Position, 0, method.FullName + Metadata.MethodDefinition, SectionKind.ROData, 0);
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

			// 3. Pointer to Method
			Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, methodTableSymbol, (int)writer1.Position, 0, method.FullName, SectionKind.Text, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 4. Flag: IsStatic
			writer1.WriteByte((byte)(method.IsStatic ? 1 : 0));

			// 5. Pointer to return Type
			Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, methodTableSymbol, (int)writer1.Position, 0, method.Signature.ReturnType.FullName + Metadata.TypeDefinition, SectionKind.ROData, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 6. Pointer to Parameter Definitions
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
				var customAttributeArgumentSymbol = CreateCustomAttributeArgument(name, i, null, ca.Arguments[i].Item1, ca.Arguments[i].Item2);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, customAttributeTableSymbol, (int)writer1.Position, 0, customAttributeArgumentSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			foreach (Mosa.Compiler.Common.Tuple<string, bool, object, MosaTypeCode> namedArg in ca.NamedArguments)
			{
				// Build definition
				var customAttributeArgumentSymbol = CreateCustomAttributeArgument(name, 0, namedArg.Item1, namedArg.Item3, namedArg.Item4);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, customAttributeTableSymbol, (int)writer1.Position, 0, customAttributeArgumentSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			// Return customAttributeTableSymbol for linker usage
			return customAttributeTableSymbol;
		}

		private LinkerSymbol CreateCustomAttributeArgument(string symbolName, int count, string name, object value, MosaTypeCode typeCode)
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

			// 2. Argument Size (High 16 bits is MosaTypeCode, Low 16 bits is length in bytes)
			uint size = (uint)typeCode << 16;
			switch (typeCode)
			{
				// 1 byte
				case MosaTypeCode.Boolean:
				case MosaTypeCode.U1:
				case MosaTypeCode.I1:
					size |= 1;
					break;

				// 2 byte
				case MosaTypeCode.Char:
				case MosaTypeCode.U2:
				case MosaTypeCode.I2:
					size |= 2;
					break;

				// 4 byte
				case MosaTypeCode.U4:
				case MosaTypeCode.I4:
				case MosaTypeCode.R4:
					size |= 4;
					break;

				// 8 byte
				case MosaTypeCode.U8:
				case MosaTypeCode.I8:
				case MosaTypeCode.R8:
					size |= 8;
					break;

				default:
					break;
			}
			writer1.Write(size);

			// 3. Argument Value
			switch (typeCode)
			{
				// 1 byte
				case MosaTypeCode.Boolean:
					writer1.Write((bool)value);
					break;
				case MosaTypeCode.U1:
					writer1.Write((sbyte)value);
					break;
				case MosaTypeCode.I1:
					writer1.Write((byte)value);
					break;

				// 2 byte
				case MosaTypeCode.Char:
					writer1.Write((char)value);
					break;
				case MosaTypeCode.U2:
					writer1.Write((ushort)value);
					break;
				case MosaTypeCode.I2:
					writer1.Write((short)value);
					break;

				// 4 byte
				case MosaTypeCode.U4:
					writer1.Write((uint)value);
					break;
				case MosaTypeCode.I4:
					writer1.Write((int)value);
					break;
				case MosaTypeCode.R4:
					writer1.Write((float)value);
					break;

				// 8 byte
				case MosaTypeCode.U8:
					writer1.Write((ulong)value);
					break;
				case MosaTypeCode.I8:
					writer1.Write((long)value);
					break;
				case MosaTypeCode.R8:
					writer1.Write((double)value);
					break;

				default:
					break;
			}

			// Return symbol for linker usage
			return symbol;
		}

		#endregion
	}
}