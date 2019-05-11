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

		private IList<MosaType> Interfaces;

		#endregion Data Members

		protected override void Initialization()
		{
			NativePatchType = (TypeLayout.NativePointerSize == 4) ? PatchType.I4 : NativePatchType = PatchType.I8;
		}

		protected override void Finalization()
		{
			Interfaces = TypeLayout.Interfaces;

			CreateDefinitionTables();
		}

		#region Helper Functions

		private LinkerSymbol EmitStringWithLength(string name, string value)
		{
			// Strings are now going to be embedded objects since they are immutable
			var symbol = Linker.DefineSymbol(name, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new EndianAwareBinaryWriter(symbol.Stream, Architecture.Endianness);
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, symbol, writer.Position, Metadata.TypeDefinition + "System.String", 0);
			writer.WriteZeroBytes(TypeLayout.NativePointerSize * 2);
			writer.Write(value.Length, TypeLayout.NativePointerSize);
			writer.Write(Encoding.Unicode.GetBytes(value));
			return symbol;
		}

		#endregion Helper Functions

		#region Assembly Tables

		private void CreateDefinitionTables()
		{
			// Emit assembly list
			var assemblyListSymbol = Linker.DefineSymbol(Metadata.AssembliesTable, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new EndianAwareBinaryWriter(assemblyListSymbol.Stream, Architecture.Endianness);

			// 1. Number of Assemblies
			writer.Write((uint)TypeSystem.Modules.Count, TypeLayout.NativePointerSize);

			// 2. Pointers to Assemblies
			foreach (var module in TypeSystem.Modules)
			{
				var assemblyTableSymbol = CreateAssemblyDefinition(module);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, assemblyListSymbol, writer.Position, assemblyTableSymbol, 0);
				writer.WriteZeroBytes(TypeLayout.NativePointerSize);
			}
		}

		private LinkerSymbol CreateAssemblyDefinition(MosaModule module)
		{
			// Emit assembly name
			var assemblyNameSymbol = EmitStringWithLength(Metadata.NameString + module.Assembly, module.Assembly);

			// Emit assembly table
			var assemblyTableSymbol = Linker.DefineSymbol(Metadata.AssemblyDefinition + module.Assembly, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new EndianAwareBinaryWriter(assemblyTableSymbol.Stream, Architecture.Endianness);

			// 1. Pointer to Assembly Name
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, assemblyTableSymbol, writer.Position, assemblyNameSymbol, 0);
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 2. Pointer to Custom Attributes
			if (module.CustomAttributes.Count > 0)
			{
				var customAttributeListSymbol = CreateCustomAttributesTable(module);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, assemblyTableSymbol, writer.Position, customAttributeListSymbol, 0);
			}
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 3. Attributes - IsReflectionOnly (32bit length)
			uint flags = 0x0;
			if (module.IsReflectionOnly) flags |= 0x1;
			writer.Write(flags, TypeLayout.NativePointerSize);

			// 4. Number of Types
			uint count = 0;
			writer.WriteZeroBytes(4);

			// 5. Pointers to Types
			foreach (var type in module.Types.Values)
			{
				if (type.IsModule)
					continue;

				var typeTableSymbol = CreateTypeDefinition(type, assemblyTableSymbol);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, assemblyTableSymbol, writer.Position, typeTableSymbol, 0);
				writer.WriteZeroBytes(TypeLayout.NativePointerSize);

				count++;
			}

			writer.Position = 3 * TypeLayout.NativePointerSize;
			writer.Write(count, TypeLayout.NativePointerSize);

			return assemblyTableSymbol;
		}

		#endregion Assembly Tables

		#region TypeDefinition

		private LinkerSymbol CreateTypeDefinition(MosaType type, LinkerSymbol assemblyTableSymbol)
		{
			// Emit type table
			var typeNameSymbol = EmitStringWithLength(Metadata.NameString + type.FullName, type.FullName);
			var typeTableSymbol = Linker.DefineSymbol(Metadata.TypeDefinition + type.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new EndianAwareBinaryWriter(typeTableSymbol.Stream, Architecture.Endianness);

			// 1. Pointer to Name
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, writer.Position, typeNameSymbol, 0);
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 2. Pointer to Custom Attributes
			if (type.CustomAttributes.Count > 0)
			{
				var customAttributeListSymbol = CreateCustomAttributesTable(type);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, writer.Position, customAttributeListSymbol, 0);
			}
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 3. Type Code & Attributes
			writer.Write(((uint)type.TypeCode << 24) + (uint)type.TypeAttributes, TypeLayout.NativePointerSize);

			// 4. Size
			writer.Write((uint)TypeLayout.GetTypeSize(type), TypeLayout.NativePointerSize);

			// 5. Pointer to Assembly Definition
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, writer.Position, assemblyTableSymbol, 0);
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 6. Pointer to Base Type
			if (type.BaseType != null)
			{
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, writer.Position, Metadata.TypeDefinition + type.BaseType.FullName, 0);
			}
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 7. Pointer to Declaring Type
			if (type.DeclaringType != null)
			{
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, writer.Position, Metadata.TypeDefinition + type.DeclaringType.FullName, 0);
			}
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 8. Pointer to Element Type
			if (type.ElementType != null)
			{
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, writer.Position, Metadata.TypeDefinition + type.ElementType.FullName, 0);
			}
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 9. Constructor that accepts no parameters, if any, for this type
			foreach (var method in type.Methods)
			{
				if (!method.IsConstructor || method.Signature.Parameters.Count != 0 || method.HasOpenGenericParams)
					continue;

				//if (!Compiler.MethodScanner.IsMethodInvoked(method))
				//	break;

				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, writer.Position, Metadata.MethodDefinition + method.FullName, 0);

				break;
			}
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 10. Properties (if any)
			if (type.Properties.Count > 0)
			{
				var propertiesSymbol = CreatePropertyDefinitions(type);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, writer.Position, propertiesSymbol, 0);
			}
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// If the type is not an interface continue, otherwise just pad until the end
			if (!type.IsInterface)
			{
				// 11. Fields (if any)
				if (type.Fields.Count > 0)
				{
					var fieldsSymbol = CreateFieldDefinitions(type);
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, writer.Position, fieldsSymbol, 0);
				}
				writer.WriteZeroBytes(TypeLayout.NativePointerSize);

				var interfaces = type.HasOpenGenericParams ? null : GetInterfaces(type);

				// If the type doesn't use interfaces then skip 9 and 10
				if (interfaces != null && interfaces.Count > 0)
				{
					// 12. Pointer to Interface Slots
					var interfaceSlotTableSymbol = CreateInterfaceSlotTable(type, interfaces);
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, writer.Position, interfaceSlotTableSymbol, 0);
					writer.WriteZeroBytes(TypeLayout.NativePointerSize);

					// 13. Pointer to Interface Bitmap
					var interfaceBitmapSymbol = CreateInterfaceBitmap(type, interfaces);
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, writer.Position, interfaceBitmapSymbol, 0);
					writer.WriteZeroBytes(TypeLayout.NativePointerSize);
				}
				else
				{
					// Fill 12 and 13 with zeros
					writer.WriteZeroBytes(TypeLayout.NativePointerSize * 2);
				}

				// For the next part we'll need to get the list of methods from the MosaTypeLayout
				var methodList = TypeLayout.GetMethodTable(type) ?? new List<MosaMethod>();

				// 14. Number of Methods
				writer.Write(methodList.Count, TypeLayout.NativePointerSize);

				// 15. Pointer to Methods
				foreach (var method in methodList)
				{
					if ((!(!method.HasImplementation && method.IsAbstract)) && !method.HasOpenGenericParams && !method.DeclaringType.HasOpenGenericParams)
					{
						if (Compiler.MethodScanner.IsMethodInvoked(method))
						{
							Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, writer.Position, method.FullName, 0);
						}
					}
					writer.WriteZeroBytes(TypeLayout.NativePointerSize);
				}

				// 16. Pointer to Method Definitions
				foreach (var method in methodList)
				{
					if ((!(!method.HasImplementation && method.IsAbstract)) && !method.HasOpenGenericParams && !method.DeclaringType.HasOpenGenericParams)
					{
						// Create definition and get the symbol
						var methodDefinitionSymbol = CreateMethodDefinition(method);

						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeTableSymbol, writer.Position, methodDefinitionSymbol, 0);
					}

					writer.WriteZeroBytes(TypeLayout.NativePointerSize);
				}
			}
			else
			{
				// Fill 11, 12, 13, 14 with zeros, 15 & 16 can be left out.
				writer.WriteZeroBytes(TypeLayout.NativePointerSize * 4);
			}

			return typeTableSymbol;
		}

		#endregion TypeDefinition

		#region Interface Bitmap and Tables

		private LinkerSymbol CreateInterfaceBitmap(MosaType type, List<MosaType> interfaces)
		{
			var bitmap = new byte[((Interfaces.Count - 1) / 8) + 1];

			int at = 0;
			byte bit = 0;
			foreach (var interfaceType in Interfaces)
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

			var symbol = Linker.DefineSymbol(Metadata.InterfaceBitmap + type.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, bitmap.Length);
			symbol.Stream.Write(bitmap);

			return symbol;
		}

		private LinkerSymbol CreateInterfaceSlotTable(MosaType type, List<MosaType> interfaces)
		{
			// Emit interface slot table
			var interfaceSlotTableSymbol = Linker.DefineSymbol(Metadata.InterfaceSlotTable + type.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new EndianAwareBinaryWriter(interfaceSlotTableSymbol.Stream, Architecture.Endianness);

			var slots = new List<MosaType>(Interfaces.Count);

			foreach (var interfaceType in Interfaces)
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
			writer.Write((uint)slots.Count, TypeLayout.NativePointerSize);

			// 2. Pointers to Interface Method Tables
			foreach (var interfaceType in slots)
			{
				if (interfaceType != null)
				{
					var interfaceMethodTableSymbol = CreateInterfaceMethodTable(type, interfaceType);
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, interfaceSlotTableSymbol, writer.Position, interfaceMethodTableSymbol, 0);
				}
				writer.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

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
			var interfaceMethodTableSymbol = Linker.DefineSymbol(Metadata.InterfaceMethodTable + type.FullName + "$" + interfaceType.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new EndianAwareBinaryWriter(interfaceMethodTableSymbol.Stream, Architecture.Endianness);

			var interfaceMethodTable = TypeLayout.GetInterfaceTable(type, interfaceType) ?? new MosaMethod[0];

			// 1. Number of Interface Methods
			writer.Write((uint)interfaceMethodTable.Length, TypeLayout.NativePointerSize);

			// 2. Pointer to Interface Type
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, interfaceMethodTableSymbol, writer.Position, Metadata.TypeDefinition + interfaceType.FullName, 0);
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 3. Pointers to Method Definitions
			foreach (var method in interfaceMethodTable)
			{
				// Create definition and get the symbol
				var methodDefinitionSymbol = CreateMethodDefinition(method);

				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, interfaceMethodTableSymbol, writer.Position, methodDefinitionSymbol, 0);
				writer.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			return interfaceMethodTableSymbol;
		}

		#endregion Interface Bitmap and Tables

		#region FieldDefinition

		private LinkerSymbol CreateFieldDefinitions(MosaType type)
		{
			// Emit fields table
			var fieldsTableSymbol = Linker.DefineSymbol(Metadata.FieldsTable + type.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(fieldsTableSymbol.Stream, Architecture.Endianness);

			// 1. Number of Fields
			writer1.Write((uint)type.Fields.Count, TypeLayout.NativePointerSize);

			// 2. Pointers to Field Definitions
			foreach (var field in type.Fields)
			{
				// Emit field name
				var fieldNameSymbol = EmitStringWithLength(Metadata.NameString + field.FullName, field.Name);

				// Emit field definition
				var fieldDefSymbol = Linker.DefineSymbol(Metadata.FieldDefinition + field.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
				var writer2 = new EndianAwareBinaryWriter(fieldDefSymbol.Stream, Architecture.Endianness);

				// 1. Name
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, fieldDefSymbol, writer2.Position, fieldNameSymbol, 0);
				writer2.WriteZeroBytes(TypeLayout.NativePointerSize);

				// 2. Pointer to Custom Attributes
				if (field.CustomAttributes.Count > 0)
				{
					var customAttributesTableSymbol = CreateCustomAttributesTable(field);
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, fieldDefSymbol, writer2.Position, customAttributesTableSymbol, 0);
				}
				writer2.WriteZeroBytes(TypeLayout.NativePointerSize);

				// 3. Attributes
				writer2.Write((uint)field.FieldAttributes, TypeLayout.NativePointerSize);

				// 4. Pointer to Field Type
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, fieldDefSymbol, writer2.Position, Metadata.TypeDefinition + field.FieldType.FullName, 0);
				writer2.WriteZeroBytes(TypeLayout.NativePointerSize);

				// 5 & 6. Offset / Address + Size
				if (field.IsStatic && !field.IsLiteral && !type.HasOpenGenericParams)
				{
					if (Compiler.MethodScanner.IsFieldAccessed(field))
					{
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, fieldDefSymbol, writer2.Position, field.FullName, 0);
					}
					writer2.WriteZeroBytes(TypeLayout.NativePointerSize);
					writer2.Write(field.Data?.Length ?? 0, TypeLayout.NativePointerSize);
				}
				else
				{
					writer2.WriteZeroBytes(TypeLayout.NativePointerSize);
					writer2.Write(TypeLayout.GetFieldOffset(field), TypeLayout.NativePointerSize);
				}

				// Add pointer to field list
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, fieldsTableSymbol, writer1.Position, fieldDefSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			return fieldsTableSymbol;
		}

		#endregion FieldDefinition

		#region PropertyDefinition

		private LinkerSymbol CreatePropertyDefinitions(MosaType type)
		{
			// Emit properties table
			var propertiesTableSymbol = Linker.DefineSymbol(Metadata.PropertiesTable + type.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new EndianAwareBinaryWriter(propertiesTableSymbol.Stream, Architecture.Endianness);

			// 1. Number of Properties
			writer.Write((uint)type.Properties.Count, TypeLayout.NativePointerSize);

			// 2. Pointers to Property Definitions
			foreach (var property in type.Properties)
			{
				// Emit field name
				var fieldNameSymbol = EmitStringWithLength(Metadata.NameString + property.FullName, property.Name);

				// Emit property definition
				var propertyDefSymbol = Linker.DefineSymbol(Metadata.PropertyDefinition + property.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
				var writer2 = new EndianAwareBinaryWriter(propertyDefSymbol.Stream, Architecture.Endianness);

				// 1. Name
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, propertyDefSymbol, writer2.Position, fieldNameSymbol, 0);
				writer2.WriteZeroBytes(TypeLayout.NativePointerSize);

				// 2. Pointer to Custom Attributes
				if (property.CustomAttributes.Count > 0)
				{
					var customAttributesTableSymbol = CreateCustomAttributesTable(property);
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, propertyDefSymbol, writer.Position, customAttributesTableSymbol, 0);
				}
				writer.WriteZeroBytes(TypeLayout.NativePointerSize);

				// 3. Attributes
				writer.Write((uint)property.PropertyAttributes, TypeLayout.NativePointerSize);

				// 4. Pointer to Property Type
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, propertyDefSymbol, writer2.Position, Metadata.TypeDefinition + property.PropertyType.FullName, 0);
				writer2.WriteZeroBytes(TypeLayout.NativePointerSize);

				// If the type is a property then skip linking the methods
				if (!type.IsInterface)
				{
					// 5. Pointer to Getter Method Definition
					if (property.GetterMethod != null && property.GetterMethod.HasImplementation && !property.GetterMethod.HasOpenGenericParams)
					{
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, propertyDefSymbol, writer2.Position, Metadata.MethodDefinition + property.GetterMethod.FullName, 0);
					}

					writer2.WriteZeroBytes(TypeLayout.NativePointerSize);

					// 6. Pointer to Setter Method Definition
					if (property.SetterMethod != null && property.SetterMethod.HasImplementation && !property.SetterMethod.HasOpenGenericParams)
					{
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, propertyDefSymbol, writer2.Position, Metadata.MethodDefinition + property.SetterMethod.FullName, 0);
					}

					writer2.WriteZeroBytes(TypeLayout.NativePointerSize);
				}
				else
				{
					// Fill 5 and 6 with zeros.
					writer.WriteZeroBytes(TypeLayout.NativePointerSize * 2);
				}

				// Add pointer to properties table
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, propertiesTableSymbol, writer.Position, propertyDefSymbol, 0);
				writer.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			return propertiesTableSymbol;
		}

		#endregion PropertyDefinition

		#region MethodDefinition

		private LinkerSymbol CreateMethodDefinition(MosaMethod method)
		{
			// Emit method name
			var methodNameSymbol = EmitStringWithLength(Metadata.NameString + method.FullName, method.FullName);

			// Emit method table
			var methodTableSymbol = Linker.DefineSymbol(Metadata.MethodDefinition + method.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, (method.Signature.Parameters.Count + 9) * TypeLayout.NativePointerSize);
			var writer = new EndianAwareBinaryWriter(methodTableSymbol.Stream, Architecture.Endianness);

			// 1. Pointer to Name
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodTableSymbol, writer.Position, methodNameSymbol, 0);
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 2. Pointer to Custom Attributes
			if (method.CustomAttributes.Count > 0)
			{
				var customAttributeListSymbol = CreateCustomAttributesTable(method);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodTableSymbol, writer.Position, customAttributeListSymbol, 0);
			}
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 3. Attributes
			writer.Write((uint)method.MethodAttributes, TypeLayout.NativePointerSize);

			// 4. Local Stack Size (16 Bits) && Parameter Stack Size (16 Bits)
			var methodData = Compiler.CompilerData.GetMethodData(method);
			int value = methodData.LocalMethodStackSize | (methodData.ParameterStackSize << 16);
			writer.Write(value, TypeLayout.NativePointerSize);

			// 5. Pointer to Method
			if (method.HasImplementation && !method.HasOpenGenericParams && !method.DeclaringType.HasOpenGenericParams)
			{
				if (Compiler.MethodScanner.IsMethodInvoked(method))
				{
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodTableSymbol, writer.Position, method.FullName, 0);
				}
			}
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 6. Pointer to return type
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodTableSymbol, writer.Position, Metadata.TypeDefinition + method.Signature.ReturnType.FullName, 0);
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 7. Pointer to Exception Handler Table
			if (method.ExceptionHandlers.Count != 0 && Compiler.MethodScanner.IsMethodInvoked(method))
			{
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodTableSymbol, writer.Position, Metadata.ProtectedRegionTable + method.FullName, 0);
			}
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 8. Pointer to GC Tracking information
			// TODO: This has yet to be designed.
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 9. Number of Parameters
			writer.Write((uint)method.Signature.Parameters.Count, TypeLayout.NativePointerSize);

			// 10. Pointers to Parameter Definitions
			foreach (var parameter in method.Signature.Parameters)
			{
				// Create definition and get the symbol
				var parameterDefinitionSymbol = CreateParameterDefinition(parameter);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodTableSymbol, writer.Position, parameterDefinitionSymbol, 0);
				writer.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			return methodTableSymbol;
		}

		#endregion MethodDefinition

		#region ParameterDefinition

		private LinkerSymbol CreateParameterDefinition(MosaParameter parameter)
		{
			// Emit parameter name
			var parameterNameSymbol = EmitStringWithLength(Metadata.NameString + parameter, parameter.FullName);

			// Emit parameter table
			var parameterTableSymbol = Linker.DefineSymbol(Metadata.MethodDefinition + parameter.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new EndianAwareBinaryWriter(parameterTableSymbol.Stream, Architecture.Endianness);

			// 1. Pointer to Name
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, parameterTableSymbol, writer.Position, parameterNameSymbol, 0);
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 2. Pointer to Custom Attributes
			if (parameter.CustomAttributes.Count > 0)
			{
				var customAttributeListSymbol = CreateCustomAttributesTable(parameter);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, parameterTableSymbol, writer.Position, customAttributeListSymbol, 0);
			}
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 3. Attributes
			writer.Write((uint)parameter.ParameterAttributes, TypeLayout.NativePointerSize);

			// 4. Pointer to Parameter Type
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, parameterTableSymbol, writer.Position, Metadata.TypeDefinition + parameter.ParameterType.FullName, 0);
			writer.WriteZeroBytes(TypeLayout.NativePointerSize);

			return parameterTableSymbol;
		}

		#endregion ParameterDefinition

		#region Custom Attributes

		private LinkerSymbol CreateCustomAttributesTable(MosaUnit unit)
		{
			// Emit custom attributes table
			var customAttributesTableSymbol = Linker.DefineSymbol(Metadata.CustomAttributesTable + unit.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new EndianAwareBinaryWriter(customAttributesTableSymbol.Stream, Architecture.Endianness);

			// 1. Number of Custom Attributes
			writer.Write(unit.CustomAttributes.Count, TypeLayout.NativePointerSize);

			// 2. Pointers to Custom Attributes
			for (int i = 0; i < unit.CustomAttributes.Count; i++)
			{
				// Get custom attribute
				var ca = unit.CustomAttributes[i];

				// Build definition
				var customAttributeTableSymbol = CreateCustomAttribute(unit, ca, i);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, customAttributesTableSymbol, writer.Position, customAttributeTableSymbol, 0);
				writer.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			return customAttributesTableSymbol;
		}

		private LinkerSymbol CreateCustomAttribute(MosaUnit unit, MosaCustomAttribute ca, int position)
		{
			// Emit custom attribute list
			string name = unit.FullName + ">>" + position.ToString() + ":" + ca.Constructor.DeclaringType.Name;
			var customAttributeSymbol = Linker.DefineSymbol(Metadata.CustomAttribute + name, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(customAttributeSymbol.Stream, Architecture.Endianness);

			// 1. Pointer to Attribute Type
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, customAttributeSymbol, writer1.Position, Metadata.TypeDefinition + ca.Constructor.DeclaringType.FullName, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 2. Pointer to Constructor Method Definition
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, customAttributeSymbol, writer1.Position, Metadata.MethodDefinition + ca.Constructor.FullName, 0);

			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 3. Number of Arguments (Both unnamed and named)
			writer1.Write((uint)(ca.Arguments.Length + ca.NamedArguments.Length), TypeLayout.NativePointerSize);

			// 4. Pointers to Custom Attribute Arguments (Both unnamed and named)
			for (int i = 0; i < ca.Arguments.Length; i++)
			{
				// Build definition
				var customAttributeArgumentSymbol = CreateCustomAttributeArgument(name, i, null, ca.Arguments[i], false);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, customAttributeSymbol, writer1.Position, customAttributeArgumentSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			foreach (var namedArg in ca.NamedArguments)
			{
				// Build definition
				var customAttributeArgumentSymbol = CreateCustomAttributeArgument(name, 0, namedArg.Name, namedArg.Argument, namedArg.IsField);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, customAttributeSymbol, writer1.Position, customAttributeArgumentSymbol, 0);
				writer1.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			return customAttributeSymbol;
		}

		private LinkerSymbol CreateCustomAttributeArgument(string symbolName, int count, string name, MosaCustomAttribute.Argument arg, bool isField)
		{
			string nameForSymbol = name ?? count.ToString();
			nameForSymbol = symbolName + ":" + nameForSymbol;
			var symbol = Linker.DefineSymbol(Metadata.CustomAttributeArgument + nameForSymbol, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(symbol.Stream, Architecture.Endianness);

			// 1. Pointer to name (if named)
			if (name != null)
			{
				var nameSymbol = EmitStringWithLength(Metadata.NameString + nameForSymbol, name);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, symbol, writer1.Position, nameSymbol, 0);
			}
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 2. Is Argument A Field
			writer1.Write(isField, TypeLayout.NativePointerSize);

			// 3. Argument Type Pointer
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, symbol, writer1.Position, Metadata.TypeDefinition + arg.Type.FullName, 0);
			writer1.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 4. Argument Size
			writer1.Write(ComputeArgumentSize(arg.Type, arg.Value), TypeLayout.NativePointerSize);

			// 5. Argument Value
			WriteArgument(writer1, symbol, arg.Type, arg.Value);

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
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, symbol, writer.Position, Metadata.TypeDefinition + "System.String", 0);
					writer.WriteZeroBytes(TypeLayout.NativePointerSize * 2);
					writer.Write(str.Length, TypeLayout.NativePointerSize);
					writer.Write(System.Text.Encoding.Unicode.GetBytes(str));
					break;

				default:
					if (type.FullName == "System.Type")
					{
						var valueType = (MosaType)value;
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, symbol, writer.Position, Metadata.TypeDefinition + valueType.FullName, 0);
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
