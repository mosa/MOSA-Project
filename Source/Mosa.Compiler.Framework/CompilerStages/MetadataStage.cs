// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Compiler.Framework.CompilerStages
{
	/// <summary>
	/// Emits metadata for assemblies and types
	/// </summary>
	/// <seealso cref="Mosa.Compiler.Framework.BaseCompilerStage" />
	public sealed class MetadataStage : BaseCompilerStage
	{
		#region Data Members

		private PatchType NativePatchType;

		#endregion Data Members

		protected override void Setup()
		{
			NativePatchType = (TypeLayout.NativePointerSize == 4) ? PatchType.I4 : NativePatchType = PatchType.I8;
		}

		protected override void RunPostCompile()
		{
			CreateDefinitionTables();
		}

		#region Helper Functions

		private LinkerSymbol EmitStringWithLength(string name, string value)
		{
			// Strings are now going to be embedded objects since they are immutable
			var symbol = Linker.DefineSymbol(name, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var stream = new EndianAwareBinaryWriter(symbol.Stream, Architecture.Endianness);
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, symbol, (int)stream.Position, "System.String" + Metadata.TypeDefinition, 0);
			stream.WriteZeroBytes(TypeLayout.NativePointerSize * 2);
			stream.Write(value.Length, TypeLayout.NativePointerSize);
			stream.Write(Encoding.Unicode.GetBytes(value));
			return symbol;
		}

		#endregion Helper Functions

		#region Assembly Tables

		private void CreateDefinitionTables()
		{
			// Emit assembly list
			var assemblyListSymbol = Linker.DefineSymbol(Metadata.AssembliesTable, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(assemblyListSymbol.Stream, Architecture.Endianness);

			// 1. Number of Assemblies
			writer1.Write((uint)TypeSystem.Modules.Count, TypeLayout.NativePointerSize);

			// 2. Pointers to Assemblies
			// Create the definitions along the way
			foreach (var module in TypeSystem.Modules)
			{
				var assemblyTableSymbol = CreateAssemblyDefinition(module);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, assemblyListSymbol, (int)writer1.Position, assemblyTableSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}
		}

		private LinkerSymbol CreateAssemblyDefinition(MosaModule module)
		{
			// Emit assembly name
			var assemblyNameSymbol = EmitStringWithLength(module.Assembly + Metadata.NameString, module.Assembly);

			// Emit assembly table
			var assemblyTableSymbol = Linker.DefineSymbol(module.Assembly + Metadata.AssemblyDefinition, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(assemblyTableSymbol.Stream, Architecture.Endianness);

			// 1. Pointer to Assembly Name
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, assemblyTableSymbol, (int)writer1.Position, assemblyNameSymbol, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 2. Pointer to Custom Attributes
			if (module.CustomAttributes.Count > 0)
			{
				var customAttributeListSymbol = CreateCustomAttributesTable(module);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, assemblyTableSymbol, (int)writer1.Position, customAttributeListSymbol, 0);
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 3. Attributes - IsReflectionOnly (32bit length)
			uint flags = 0x0;
			if (module.IsReflectionOnly) flags |= 0x1;
			writer1.Write(flags, TypeLayout.NativePointerSize);

			// 4. Number of Types
			uint count = 0;
			foreach (var type in module.Types.Values)
			{
				if (type.IsModule)
					continue;

				count++;
			}
			writer1.Write(count, TypeLayout.NativePointerSize);

			// 5. Pointers to Types
			// Create the definitions along the way
			foreach (var type in module.Types.Values)
			{
				if (type.IsModule)
					continue;

				// Run the type through the TypeLayout system
				//TypeLayout.ResolveType(type);

				var typeTableSymbol = CreateTypeDefinition(type, assemblyTableSymbol);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, assemblyTableSymbol, (int)writer1.Position, typeTableSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			// Return assemblyTableSymbol
			return assemblyTableSymbol;
		}

		#endregion Assembly Tables

		#region TypeDefinition

		private LinkerSymbol CreateTypeDefinition(MosaType type, LinkerSymbol assemblyTableSymbol)
		{
			// Emit type name
			var typeNameSymbol = EmitStringWithLength(type.FullName + Metadata.NameString, type.FullName);

			// Emit type table
			var typeTableSymbol = Linker.DefineSymbol(type.FullName + Metadata.TypeDefinition, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(typeTableSymbol.Stream, Architecture.Endianness);

			// 1. Pointer to Name
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, (int)writer1.Position, typeNameSymbol, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 2. Pointer to Custom Attributes
			if (type.CustomAttributes.Count > 0)
			{
				var customAttributeListSymbol = CreateCustomAttributesTable(type);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, (int)writer1.Position, customAttributeListSymbol, 0);
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 3. Type Code & Attributes
			writer1.Write(((uint)type.TypeCode << 24) + (uint)type.TypeAttributes, TypeLayout.NativePointerSize);

			// 4. Size
			writer1.Write((uint)TypeLayout.GetTypeSize(type), TypeLayout.NativePointerSize);

			// 5. Pointer to Assembly Definition
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, (int)writer1.Position, assemblyTableSymbol, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 6. Pointer to Parent Type
			if (type.BaseType != null)
			{
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, (int)writer1.Position, type.BaseType.FullName + Metadata.TypeDefinition, 0);
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 7. Pointer to Declaring Type
			if (type.DeclaringType != null)
			{
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, (int)writer1.Position, type.DeclaringType.FullName + Metadata.TypeDefinition, 0);
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 8. Pointer to Element Type
			if (type.ElementType != null)
			{
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, (int)writer1.Position, type.ElementType.FullName + Metadata.TypeDefinition, 0);
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 9. Constructor that accepts no parameters, if any, for this type
			foreach (var method in type.Methods)
			{
				if (!method.Name.Equals(".ctor") || method.Signature.Parameters.Count != 0 || method.HasOpenGenericParams)
					continue;

				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, (int)writer1.Position, method.FullName + Metadata.MethodDefinition, 0);
				break;
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 10. Properties (if any)
			if (type.Properties.Count > 0)
			{
				var propertiesSymbol = CreatePropertyDefinitions(type);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, (int)writer1.Position, propertiesSymbol, 0);
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// If the type is not an interface continue, otherwise just pad until the end
			if (!type.IsInterface)
			{
				// 11. Fields (if any)
				if (type.Fields.Count > 0)
				{
					var fieldsSymbol = CreateFieldDefinitions(type);
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, (int)writer1.Position, fieldsSymbol, 0);
				}
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

				var interfaces = GetInterfaces(type);

				// If the type doesn't use interfaces then skip 9 and 10
				if (interfaces.Count > 0)
				{
					// 12. Pointer to Interface Slots
					var interfaceSlotTableSymbol = CreateInterfaceSlotTable(type, interfaces);
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, (int)writer1.Position, interfaceSlotTableSymbol, 0);
					writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

					// 13. Pointer to Interface Bitmap
					var interfaceBitmapSymbol = CreateInterfaceBitmap(type, interfaces);
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, (int)writer1.Position, interfaceBitmapSymbol, 0);
					writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
				}
				else
				{
					// Fill 12 and 13 with zeros
					writer1.WriteZeroBytes(TypeLayout.NativePointerSize * 2);
				}

				// For the next part we'll need to get the list of methods from the MosaTypeLayout
				var methodList = TypeLayout.GetMethodTable(type) ?? new List<MosaMethod>();

				// 14. Number of Methods
				writer1.Write(methodList.Count, TypeLayout.NativePointerSize);

				// 15. Pointer to Methods
				foreach (var method in methodList)
				{
					if ((!(!method.HasImplementation && method.IsAbstract)) && !method.HasOpenGenericParams && !method.DeclaringType.HasOpenGenericParams)
					{
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, (int)writer1.Position, method.FullName, 0);
					}
					writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
				}

				// 16. Pointer to Method Definitions
				foreach (var method in methodList)
				{
					if ((!(!method.HasImplementation && method.IsAbstract)) && !method.HasOpenGenericParams && !method.DeclaringType.HasOpenGenericParams)
					{
						// Create definition and get the symbol
						var methodDefinitionSymbol = CreateMethodDefinition(method);

						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, (int)writer1.Position, methodDefinitionSymbol, 0);
					}

					writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
				}
			}
			else
			{
				// Fill 11, 12, 13, 14 with zeros, 15 & 16 can be left out.
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize * 4);
			}

			// Return typeTableSymbol for linker usage
			return typeTableSymbol;
		}

		#endregion TypeDefinition

		#region Interface Bitmap and Tables

		private LinkerSymbol CreateInterfaceBitmap(MosaType type, List<MosaType> interfaces)
		{
			var bitmap = new byte[((TypeLayout.Interfaces.Count - 1) / 8) + 1];

			int at = 0;
			byte bit = 0;
			foreach (var interfaceType in TypeLayout.Interfaces)
			{
				if (interfaces.Contains(interfaceType))
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

			var symbol = Linker.DefineSymbol(type.FullName + Metadata.InterfaceBitmap, SectionKind.ROData, TypeLayout.NativePointerAlignment, bitmap.Length);
			symbol.Stream.Write(bitmap);

			// Return symbol for linker usage
			return symbol;
		}

		private LinkerSymbol CreateInterfaceSlotTable(MosaType type, List<MosaType> interfaces)
		{
			// Emit interface slot table
			var interfaceSlotTableSymbol = Linker.DefineSymbol(type.FullName + Metadata.InterfaceSlotTable, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(interfaceSlotTableSymbol.Stream, Architecture.Endianness);

			var slots = new List<MosaType>(TypeLayout.Interfaces.Count);

			foreach (var interfaceType in TypeLayout.Interfaces)
			{
				if (interfaces.Contains(interfaceType))
				{
					slots.Add(interfaceType);
				}
				else
				{
					slots.Add(null);
				}
			}

			// 1. Number of Interface slots
			writer1.Write((uint)slots.Count, TypeLayout.NativePointerSize);

			// 2. Pointers to Interface Method Tables
			foreach (var interfaceType in slots)
			{
				if (interfaceType != null)
				{
					var interfaceMethodTableSymbol = CreateInterfaceMethodTable(type, interfaceType);

					// Link
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, interfaceSlotTableSymbol, (int)writer1.Position, interfaceMethodTableSymbol, 0);
				}
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			// Return interfaceSlotTableSymbol for linker usage
			return interfaceSlotTableSymbol;
		}

		private static List<MosaType> GetInterfaces(MosaType type)
		{
			var interfaces = new List<MosaType>();
			var baseType = type;

			while (baseType != null)
			{
				foreach (var interfaceType in baseType.Interfaces)
				{
					interfaces.AddIfNew(interfaceType);
				}

				baseType = baseType.BaseType;
			}

			return interfaces;
		}

		private LinkerSymbol CreateInterfaceMethodTable(MosaType type, MosaType interfaceType)
		{
			// Emit interface method table
			var interfaceMethodTableSymbol = Linker.DefineSymbol(type.FullName + Metadata.InterfaceMethodTable + interfaceType.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(interfaceMethodTableSymbol.Stream, Architecture.Endianness);

			var interfaceMethodTable = TypeLayout.GetInterfaceTable(type, interfaceType) ?? new MosaMethod[0];

			// 1. Number of Interface Methods
			writer1.Write((uint)interfaceMethodTable.Length, TypeLayout.NativePointerSize);

			// 2. Pointer to Interface Type
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, interfaceMethodTableSymbol, (int)writer1.Position, interfaceType.FullName + Metadata.TypeDefinition, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 3. Pointers to Method Definitions
			foreach (var method in interfaceMethodTable)
			{
				// Create definition and get the symbol
				var methodDefinitionSymbol = CreateMethodDefinition(method);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, interfaceMethodTableSymbol, (int)writer1.Position, methodDefinitionSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			// Return interfaceMethodTableSymbol for linker usage
			return interfaceMethodTableSymbol;
		}

		#endregion Interface Bitmap and Tables

		#region FieldDefinition

		private LinkerSymbol CreateFieldDefinitions(MosaType type)
		{
			// Emit fields table
			var fieldsTableSymbol = Linker.DefineSymbol(type.FullName + Metadata.FieldsTable, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(fieldsTableSymbol.Stream, Architecture.Endianness);

			// 1. Number of Fields
			writer1.Write((uint)type.Fields.Count, TypeLayout.NativePointerSize);

			// 2. Pointers to Field Definitions
			foreach (MosaField field in type.Fields)
			{
				// Emit field name
				var fieldNameSymbol = EmitStringWithLength(field.FullName + Metadata.NameString, field.Name);

				// Emit field definition
				var fieldDefSymbol = Linker.DefineSymbol(field.FullName + Metadata.FieldDefinition, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
				var writer2 = new EndianAwareBinaryWriter(fieldDefSymbol.Stream, Architecture.Endianness);

				// 1. Name
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, fieldDefSymbol, (int)writer2.Position, fieldNameSymbol, 0);
				writer2.WriteZeroBytes(TypeLayout.NativePointerSize);

				// 2. Pointer to Custom Attributes
				if (field.CustomAttributes.Count > 0)
				{
					var customAttributesTableSymbol = CreateCustomAttributesTable(field);
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, fieldDefSymbol, (int)writer2.Position, customAttributesTableSymbol, 0);
				}
				writer2.WriteZeroBytes(TypeLayout.NativePointerSize);

				// 3. Attributes
				writer2.Write((uint)field.FieldAttributes, TypeLayout.NativePointerSize);

				// 4. Pointer to Field Type
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, fieldDefSymbol, (int)writer2.Position, field.FieldType.FullName + Metadata.TypeDefinition, 0);
				writer2.WriteZeroBytes(TypeLayout.NativePointerSize);

				// 5 & 6. Offset / Address + Size
				if (field.IsStatic && !field.IsLiteral)
				{
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, fieldDefSymbol, (int)writer2.Position, field.FullName, 0);
					writer2.WriteZeroBytes(TypeLayout.NativePointerSize);
					writer2.Write(field.Data?.Length ?? 0, TypeLayout.NativePointerSize);
				}
				else
				{
					writer2.WriteZeroBytes(TypeLayout.NativePointerSize);
					writer2.Write(TypeLayout.GetFieldOffset(field), TypeLayout.NativePointerSize);
				}

				// Create another symbol with field data if any
				if (field.IsStatic)
				{
					// Assign a memory slot to the static & initialize it, if there's initial data set
					// Determine the size of the type & alignment requirements
					//Architecture.GetTypeRequirements(TypeLayout, field.FieldType, out int size, out int alignment);

					int size = TypeLayout.GetFieldSize(field);

					// The linker section to move this field into
					var section = field.Data != null ? SectionKind.ROData : SectionKind.BSS;

					var symbol = Compiler.Linker.DefineSymbol(field.FullName, section, Architecture.NativeAlignment, size);

					if (field.Data != null)
					{
						symbol.Stream.Write(field.Data, 0, size);
					}
				}

				// Add pointer to field list
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, fieldsTableSymbol, (int)writer1.Position, fieldDefSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			// Return fieldsTableSymbol for linker usage
			return fieldsTableSymbol;
		}

		#endregion FieldDefinition

		#region PropertyDefinition

		private LinkerSymbol CreatePropertyDefinitions(MosaType type)
		{
			// Emit properties table
			var propertiesTableSymbol = Linker.DefineSymbol(type.FullName + Metadata.PropertiesTable, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(propertiesTableSymbol.Stream, Architecture.Endianness);

			// 1. Number of Properties
			writer1.Write((uint)type.Properties.Count, TypeLayout.NativePointerSize);

			// 2. Pointers to Property Definitions
			foreach (MosaProperty property in type.Properties)
			{
				// Emit field name
				var fieldNameSymbol = EmitStringWithLength(property.FullName + Metadata.NameString, property.Name);

				// Emit property definition
				var propertyDefSymbol = Linker.DefineSymbol(property.FullName + Metadata.PropertyDefinition, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
				var writer2 = new EndianAwareBinaryWriter(propertyDefSymbol.Stream, Architecture.Endianness);

				// 1. Name
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, propertyDefSymbol, (int)writer2.Position, fieldNameSymbol, 0);
				writer2.WriteZeroBytes(TypeLayout.NativePointerSize);

				// 2. Pointer to Custom Attributes
				if (property.CustomAttributes.Count > 0)
				{
					var customAttributesTableSymbol = CreateCustomAttributesTable(property);
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, propertyDefSymbol, (int)writer1.Position, customAttributesTableSymbol, 0);
				}
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

				// 3. Attributes
				writer1.Write((uint)property.PropertyAttributes, TypeLayout.NativePointerSize);

				// 4. Pointer to Property Type
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, propertyDefSymbol, (int)writer2.Position, property.PropertyType.FullName + Metadata.TypeDefinition, 0);
				writer2.WriteZeroBytes(TypeLayout.NativePointerSize);

				// If the type is a property then skip linking the methods
				if (!type.IsInterface)
				{
					// 5. Pointer to Getter Method Definition
					if (property.GetterMethod != null && property.GetterMethod.HasImplementation && !property.GetterMethod.HasOpenGenericParams)
					{
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, propertyDefSymbol, (int)writer2.Position, property.GetterMethod.FullName + Metadata.MethodDefinition, 0);
					}

					writer2.WriteZeroBytes(TypeLayout.NativePointerSize);

					// 6. Pointer to Setter Method Definition
					if (property.SetterMethod != null && property.SetterMethod.HasImplementation && !property.SetterMethod.HasOpenGenericParams)
					{
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, propertyDefSymbol, (int)writer2.Position, property.SetterMethod.FullName + Metadata.MethodDefinition, 0);
					}

					writer2.WriteZeroBytes(TypeLayout.NativePointerSize);
				}
				else
				{
					// Fill 5 and 6 with zeros.
					writer1.WriteZeroBytes(TypeLayout.NativePointerSize * 2);
				}

				// Add pointer to properties table
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, propertiesTableSymbol, (int)writer1.Position, propertyDefSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			// Return fieldListTableSymbol for linker usage
			return propertiesTableSymbol;
		}

		#endregion PropertyDefinition

		#region MethodDefinition

		private LinkerSymbol CreateMethodDefinition(MosaMethod method)
		{
			// Emit method name
			var methodNameSymbol = EmitStringWithLength(method.FullName + Metadata.NameString, method.FullName);

			// Emit method table
			var methodTableSymbol = Linker.DefineSymbol(method.FullName + Metadata.MethodDefinition, SectionKind.ROData, TypeLayout.NativePointerAlignment, (method.Signature.Parameters.Count + 9) * TypeLayout.NativePointerSize);
			var writer1 = new EndianAwareBinaryWriter(methodTableSymbol.Stream, Architecture.Endianness);

			// 1. Pointer to Name
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodTableSymbol, (int)writer1.Position, methodNameSymbol, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 2. Pointer to Custom Attributes
			if (method.CustomAttributes.Count > 0)
			{
				var customAttributeListSymbol = CreateCustomAttributesTable(method);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodTableSymbol, (int)writer1.Position, customAttributeListSymbol, 0);
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 3. Attributes
			writer1.Write((uint)method.MethodAttributes, TypeLayout.NativePointerSize);

			// 4. Local Stack Size (16 Bits) && Parameter Stack Size (16 Bits)
			var methodData = Compiler.CompilerData.GetCompilerMethodData(method);
			int value = methodData.LocalMethodStackSize | (methodData.ParameterStackSize << 16);
			writer1.Write(value, TypeLayout.NativePointerSize);

			// 5. Pointer to Method
			if (method.HasImplementation && !method.HasOpenGenericParams && !method.DeclaringType.HasOpenGenericParams)
			{
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodTableSymbol, (int)writer1.Position, method.FullName, 0);
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 6. Pointer to return type
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodTableSymbol, (int)writer1.Position, method.Signature.ReturnType.FullName + Metadata.TypeDefinition, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 7. Pointer to Exception Handler Table
			if (method.ExceptionHandlers.Count != 0)
			{
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodTableSymbol, (int)writer1.Position, method.FullName + Metadata.ProtectedRegionTable, 0);
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 8. Pointer to GC Tracking information
			// TODO: This has yet to be designed.
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 9. Number of Parameters
			writer1.Write((uint)method.Signature.Parameters.Count, TypeLayout.NativePointerSize);

			// 10. Pointers to Parameter Definitions
			foreach (var parameter in method.Signature.Parameters)
			{
				// Create definition and get the symbol
				var parameterDefinitionSymbol = CreateParameterDefinition(parameter);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodTableSymbol, (int)writer1.Position, parameterDefinitionSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			// Return methodTableSymbol for linker usage
			return methodTableSymbol;
		}

		#endregion MethodDefinition

		#region ParameterDefinition

		private LinkerSymbol CreateParameterDefinition(MosaParameter parameter)
		{
			// Emit parameter name
			var parameterNameSymbol = EmitStringWithLength(parameter + Metadata.NameString, parameter.FullName);

			// Emit parameter table
			var parameterTableSymbol = Linker.DefineSymbol(parameter.FullName + Metadata.MethodDefinition, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(parameterTableSymbol.Stream, Architecture.Endianness);

			// 1. Pointer to Name
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, parameterTableSymbol, (int)writer1.Position, parameterNameSymbol, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 2. Pointer to Custom Attributes
			if (parameter.CustomAttributes.Count > 0)
			{
				var customAttributeListSymbol = CreateCustomAttributesTable(parameter);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, parameterTableSymbol, (int)writer1.Position, customAttributeListSymbol, 0);
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 3. Attributes
			writer1.Write((uint)parameter.ParameterAttributes, TypeLayout.NativePointerSize);

			// 4. Pointer to Parameter Type
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, parameterTableSymbol, (int)writer1.Position, parameter.ParameterType.FullName + Metadata.TypeDefinition, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// Return parameterTableSymbol for linker usage
			return parameterTableSymbol;
		}

		#endregion ParameterDefinition

		#region Custom Attributes

		private LinkerSymbol CreateCustomAttributesTable(MosaUnit unit)
		{
			// Emit custom attributes table
			var customAttributesTableSymbol = Linker.DefineSymbol(unit.FullName + Metadata.CustomAttributesTable, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(customAttributesTableSymbol.Stream, Architecture.Endianness);

			// 1. Number of Custom Attributes
			writer1.Write(unit.CustomAttributes.Count, TypeLayout.NativePointerSize);

			// 2. Pointers to Custom Attributes
			// Create the definitions along the way
			for (int i = 0; i < unit.CustomAttributes.Count; i++)
			{
				// Get custom attribute
				var ca = unit.CustomAttributes[i];

				// Build definition
				var customAttributeTableSymbol = CreateCustomAttribute(unit, ca, i);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, customAttributesTableSymbol, (int)writer1.Position, customAttributeTableSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			// Return customAttributesTableSymbol for linker usage
			return customAttributesTableSymbol;
		}

		private LinkerSymbol CreateCustomAttribute(MosaUnit unit, MosaCustomAttribute ca, int position)
		{
			// Emit custom attribute list
			string name = unit.FullName + ">>" + position.ToString() + ":" + ca.Constructor.DeclaringType.Name;
			var customAttributeSymbol = Linker.DefineSymbol(name + Metadata.CustomAttribute, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(customAttributeSymbol.Stream, Architecture.Endianness);

			// 1. Pointer to Attribute Type
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, customAttributeSymbol, (int)writer1.Position, ca.Constructor.DeclaringType.FullName + Metadata.TypeDefinition, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 2. Pointer to Constructor Method Definition
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, customAttributeSymbol, (int)writer1.Position, ca.Constructor.FullName + Metadata.MethodDefinition, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 3. Number of Arguments (Both unnamed and named)
			writer1.Write((uint)(ca.Arguments.Length + ca.NamedArguments.Length), TypeLayout.NativePointerSize);

			// 4. Pointers to Custom Attribute Arguments (Both unnamed and named)
			for (int i = 0; i < ca.Arguments.Length; i++)
			{
				// Build definition
				var customAttributeArgumentSymbol = CreateCustomAttributeArgument(name, i, null, ca.Arguments[i], false);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, customAttributeSymbol, (int)writer1.Position, customAttributeArgumentSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			foreach (var namedArg in ca.NamedArguments)
			{
				// Build definition
				var customAttributeArgumentSymbol = CreateCustomAttributeArgument(name, 0, namedArg.Name, namedArg.Argument, namedArg.IsField);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, customAttributeSymbol, (int)writer1.Position, customAttributeArgumentSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			// Return customAttributeSymbol for linker usage
			return customAttributeSymbol;
		}

		private LinkerSymbol CreateCustomAttributeArgument(string symbolName, int count, string name, MosaCustomAttribute.Argument arg, bool isField)
		{
			string nameForSymbol = name ?? count.ToString();
			nameForSymbol = symbolName + ":" + nameForSymbol;
			var symbol = Linker.DefineSymbol(nameForSymbol + Metadata.CustomAttributeArgument, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(symbol.Stream, Architecture.Endianness);

			// 1. Pointer to name (if named)
			if (name != null)
			{
				var nameSymbol = EmitStringWithLength(nameForSymbol + Metadata.NameString, name);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, symbol, (int)writer1.Position, nameSymbol, 0);
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 2. Is Argument A Field
			writer1.Write(isField, TypeLayout.NativePointerSize);

			// 3. Argument Type Pointer
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, symbol, (int)writer1.Position, arg.Type.FullName + Metadata.TypeDefinition, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 4. Argument Size
			writer1.Write(ComputeArgumentSize(arg.Type, arg.Value), TypeLayout.NativePointerSize);

			// 5. Argument Value
			WriteArgument(writer1, symbol, arg.Type, arg.Value);

			// Return symbol for linker usage
			return symbol;
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

				// SZArray
				case MosaTypeCode.SZArray:
					Debug.Assert(value is MosaCustomAttribute.Argument[]);
					var arr = (MosaCustomAttribute.Argument[])value;
					int size = 0;
					foreach (var elem in arr)
						size += ComputeArgumentSize(elem.Type, elem.Value);
					return size;

				// String
				case MosaTypeCode.String:
					return TypeLayout.NativePointerSize;

				default:
					if (type.FullName == "System.Type")
					{
						return TypeLayout.NativePointerSize;
					}
					else
					{
						throw new NotSupportedException();
					}
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

				// SZArray
				case MosaTypeCode.SZArray:
					Debug.Assert(value is MosaCustomAttribute.Argument[]);
					var arr = (MosaCustomAttribute.Argument[])value;
					writer.Write(arr.Length, TypeLayout.NativePointerSize);
					foreach (var elem in arr)
						WriteArgument(writer, symbol, elem.Type, elem.Value);
					break;

				// String
				case MosaTypeCode.String:

					// Since strings are immutable, make it an object that we can just use
					var str = (string)value;
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, symbol, (int)writer.Position, "System.String" + Metadata.TypeDefinition, 0);
					writer.WriteZeroBytes(TypeLayout.NativePointerSize * 2);
					writer.Write(str.Length, TypeLayout.NativePointerSize);
					writer.Write(System.Text.Encoding.Unicode.GetBytes(str));
					break;

				default:
					if (type.FullName == "System.Type")
					{
						var valueType = (MosaType)value;
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, symbol, (int)writer.Position, valueType.FullName + Metadata.TypeDefinition, 0);
						writer.WriteZeroBytes(TypeLayout.NativePointerSize);
					}
					else
					{
						throw new NotSupportedException();
					}

					break;
			}
		}

		#endregion Custom Attributes
	}
}
