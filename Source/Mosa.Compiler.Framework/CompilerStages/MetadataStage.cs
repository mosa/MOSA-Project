// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
		private uint NativePointerSize;

		private IList<MosaType> Interfaces;

		#endregion Data Members

		protected override void Initialization()
		{
			NativePointerSize = TypeLayout.NativePointerSize;
			NativePatchType = (NativePointerSize == 4) ? PatchType.I32 : PatchType.I64;
		}

		protected override void Finalization()
		{
			Interfaces = TypeLayout.Interfaces;

			CreateDefinitionTables();
		}

		#region Helper Functions

		private LinkerSymbol EmitStringWithLength(string name, string data)
		{
			// Strings are now going to be embedded objects since they are immutable
			var symbol = Linker.DefineSymbol(name, SectionKind.ROData, TypeLayout.NativePointerAlignment, Compiler.ObjectHeaderSize + NativePointerSize + ((uint)data.Length * 2));
			var writer = new BinaryWriter(symbol.Stream);

			// 1. Object Header
			writer.WriteZeroBytes(NativePointerSize);

			// 2. Method Table Pointers
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, symbol, writer.GetPosition(), Metadata.TypeDefinition + "System.String", 0);
			writer.WriteZeroBytes(NativePointerSize);

			// 3. Length
			writer.Write(data.Length, NativePointerSize);

			// 4. Unicode
			writer.Write(Encoding.Unicode.GetBytes(data));

			return symbol;
		}

		private MethodData GetTargetMethodData(MosaMethod method)
		{
			var methodData = Compiler.GetMethodData(method);

			if (methodData.ReplacedBy == null)
				return methodData;

			return Compiler.GetMethodData(methodData.ReplacedBy);
		}

		#endregion Helper Functions

		#region Assembly Tables

		private void CreateDefinitionTables()
		{
			// Emit assembly list
			var assemblyListSymbol = Linker.DefineSymbol(Metadata.AssembliesTable, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new BinaryWriter(assemblyListSymbol.Stream);

			// 1. Number of Assemblies
			writer.Write((uint)TypeSystem.Modules.Count, NativePointerSize);

			// 2. Pointers to Assemblies
			foreach (var module in TypeSystem.Modules)
			{
				var assemblyTableSymbol = CreateAssemblyDefinition(module);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, assemblyListSymbol, writer.GetPosition(), assemblyTableSymbol, 0);
				writer.WriteZeroBytes(NativePointerSize);
			}
		}

		private LinkerSymbol CreateAssemblyDefinition(MosaModule module)
		{
			// Emit assembly name
			var assemblyNameSymbol = EmitStringWithLength(Metadata.NameString + module.Assembly, module.Assembly);

			// Emit assembly table
			var assemblyTableSymbol = Linker.DefineSymbol(Metadata.AssemblyDefinition + module.Assembly, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new BinaryWriter(assemblyTableSymbol.Stream);

			// 1. Pointer to Assembly Name
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, assemblyTableSymbol, writer.GetPosition(), assemblyNameSymbol, (int)Compiler.ObjectHeaderSize);
			writer.WriteZeroBytes(NativePointerSize);

			// 2. Pointer to Custom Attributes
			if (module.CustomAttributes.Count > 0)
			{
				var customAttributeListSymbol = CreateCustomAttributesTable(module);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, assemblyTableSymbol, writer.GetPosition(), customAttributeListSymbol, 0);
			}
			writer.WriteZeroBytes(NativePointerSize);

			// 3. Attributes - IsReflectionOnly (32bit length)
			uint flags = 0x0;
			if (module.IsReflectionOnly) flags |= 0x1;
			writer.Write(flags, NativePointerSize);

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
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, assemblyTableSymbol, writer.GetPosition(), typeTableSymbol, 0);
				writer.WriteZeroBytes(NativePointerSize);

				count++;
			}

			writer.SetPosition(3 * NativePointerSize);
			writer.Write(count, NativePointerSize);

			return assemblyTableSymbol;
		}

		#endregion Assembly Tables

		#region Type Definition

		private LinkerSymbol CreateTypeDefinition(MosaType type, LinkerSymbol assemblyTableSymbol)
		{
			// Emit type table
			var typeNameSymbol = EmitStringWithLength(Metadata.NameString + type.FullName, type.FullName);
			var typeDefinitionSymbol = Linker.DefineSymbol(Metadata.TypeDefinition + type.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new BinaryWriter(typeDefinitionSymbol.Stream);

			// 1. Pointer to Name
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeDefinitionSymbol, writer.GetPosition(), typeNameSymbol, (int)Compiler.ObjectHeaderSize);
			writer.WriteZeroBytes(NativePointerSize);

			// 2. Pointer to Custom Attributes
			if (type.CustomAttributes.Count > 0)
			{
				var customAttributeListSymbol = CreateCustomAttributesTable(type);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeDefinitionSymbol, writer.GetPosition(), customAttributeListSymbol, 0);
			}
			writer.WriteZeroBytes(NativePointerSize);

			// 3. Type Code & Attributes
			writer.Write(((uint)type.TypeCode << 24) + (uint)type.TypeAttributes, NativePointerSize);

			// 4. Size
			writer.Write((uint)TypeLayout.GetTypeSize(type), NativePointerSize);

			// 5. Pointer to Assembly Definition
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeDefinitionSymbol, writer.GetPosition(), assemblyTableSymbol, 0);
			writer.WriteZeroBytes(NativePointerSize);

			// 6. Pointer to Base Type
			if (type.BaseType != null)
			{
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeDefinitionSymbol, writer.GetPosition(), Metadata.TypeDefinition + type.BaseType.FullName, 0);
			}
			writer.WriteZeroBytes(NativePointerSize);

			// 7. Pointer to Declaring Type
			if (type.DeclaringType != null)
			{
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeDefinitionSymbol, writer.GetPosition(), Metadata.TypeDefinition + type.DeclaringType.FullName, 0);
			}
			writer.WriteZeroBytes(NativePointerSize);

			// 8. Pointer to Element Type
			if (type.ElementType != null)
			{
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeDefinitionSymbol, writer.GetPosition(), Metadata.TypeDefinition + type.ElementType.FullName, 0);
			}
			writer.WriteZeroBytes(NativePointerSize);

			// 9. Constructor that accepts no parameters, if any, for this type
			foreach (var method in type.Methods)
			{
				if (!method.IsConstructor || method.Signature.Parameters.Count != 0 || method.HasOpenGenericParams)
					continue;

				var targetMethodData = GetTargetMethodData(method);

				if (targetMethodData.HasCode)
				{
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeDefinitionSymbol, writer.GetPosition(), Metadata.MethodDefinition + targetMethodData.Method.FullName, 0);
				}

				break;
			}
			writer.WriteZeroBytes(NativePointerSize);

			// 10. Properties (if any)
			if (type.Properties.Count > 0)
			{
				var propertiesSymbol = CreatePropertyDefinitions(type);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeDefinitionSymbol, writer.GetPosition(), propertiesSymbol, 0);
			}
			writer.WriteZeroBytes(NativePointerSize);

			LinkerSymbol interfaceSlotTableSymbol = null;

			// 11. Fields Definitions
			if (!type.IsInterface && type.Fields.Count > 0)
			{
				var fieldsSymbol = CreateFieldDefinitions(type);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeDefinitionSymbol, writer.GetPosition(), fieldsSymbol, 0);
			}
			writer.WriteZeroBytes(NativePointerSize);

			var interfaces = type.IsInterface || type.HasOpenGenericParams ? null : GetInterfaces(type);

			if (interfaces != null && interfaces.Count == 0)
				interfaces = null;

			// 12. Pointer to Interface Slots
			if (interfaces != null)
			{
				interfaceSlotTableSymbol = CreateInterfaceSlotTable(type, interfaces);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeDefinitionSymbol, writer.GetPosition(), interfaceSlotTableSymbol, 0);
			}
			writer.WriteZeroBytes(NativePointerSize);

			// 13. Pointer to Interface Bitmap
			if (interfaces != null)
			{
				var interfaceBitmapSymbol = CreateInterfaceBitmap(type, interfaces);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeDefinitionSymbol, writer.GetPosition(), interfaceBitmapSymbol, 0);
			}
			writer.WriteZeroBytes(NativePointerSize);

			var methodList = TypeLayout.GetMethodTable(type);

			// 14. Number of Methods
			writer.Write(methodList == null ? 0 : methodList.Count, NativePointerSize);

			// 15. Method Table
			if (methodList != null)
			{
				foreach (var method in methodList)
				{
					var targetMethodData = GetTargetMethodData(method);

					if (targetMethodData.HasCode)
					{
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeDefinitionSymbol, writer.GetPosition(), targetMethodData.Method.FullName, 0);
					}
					writer.WriteZeroBytes(NativePointerSize);
				}
			}

			// 16. Method Definition Table
			if (!type.IsInterface && methodList != null)
			{
				foreach (var method in methodList)
				{
					// Create definition and get the symbol
					var methodDefinitionSymbol = CreateMethodDefinition(method);

					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, typeDefinitionSymbol, writer.GetPosition(), methodDefinitionSymbol, 0);
					writer.WriteZeroBytes(NativePointerSize);
				}
			}

			return typeDefinitionSymbol;
		}

		#endregion Type Definition

		#region Interface Bitmap

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

			var symbol = Linker.DefineSymbol(Metadata.InterfaceBitmap + type.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, (uint)bitmap.Length);
			symbol.Stream.Write(bitmap);

			return symbol;
		}

		#endregion Interface Bitmap

		#region Interface Tables

		private LinkerSymbol CreateInterfaceSlotTable(MosaType type, List<MosaType> interfaces)
		{
			// Emit interface slot table
			var interfaceSlotTableSymbol = Linker.DefineSymbol(Metadata.InterfaceSlotTable + type.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new BinaryWriter(interfaceSlotTableSymbol.Stream);

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
			writer.Write((uint)slots.Count, NativePointerSize);

			// 2. Pointers to Interface Method Tables
			foreach (var interfaceType in slots)
			{
				if (interfaceType != null)
				{
					var interfaceMethodTableSymbol = CreateInterfaceMethodTable(type, interfaceType);
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, interfaceSlotTableSymbol, writer.GetPosition(), interfaceMethodTableSymbol, 0);
				}
				writer.WriteZeroBytes(NativePointerSize);
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
			var interfaceMethodTableSymbol = Linker.DefineSymbol($"{Metadata.InterfaceMethodTable}{type.FullName}${interfaceType.FullName}", SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new BinaryWriter(interfaceMethodTableSymbol.Stream);

			var interfaceMethodTable = TypeLayout.GetInterfaceTable(type, interfaceType) ?? new MosaMethod[0];

			// 1. Number of Interface Methods
			writer.Write((uint)interfaceMethodTable.Length, NativePointerSize);

			// 2. Pointer to Interface Type
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, interfaceMethodTableSymbol, writer.GetPosition(), Metadata.TypeDefinition + interfaceType.FullName, 0);
			writer.WriteZeroBytes(NativePointerSize);

			// 3. Pointers to Method Definitions
			foreach (var method in interfaceMethodTable)
			{
				// Create definition and get the symbol
				var methodDefinitionSymbol = CreateMethodDefinition(method);

				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, interfaceMethodTableSymbol, writer.GetPosition(), methodDefinitionSymbol, 0);
				writer.WriteZeroBytes(NativePointerSize);
			}

			return interfaceMethodTableSymbol;
		}

		#endregion Interface Tables

		#region Field Definition

		private LinkerSymbol CreateFieldDefinitions(MosaType type)
		{
			// Emit fields table
			var fieldsTableSymbol = Linker.DefineSymbol(Metadata.FieldsTable + type.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new BinaryWriter(fieldsTableSymbol.Stream);

			// 1. Number of Fields
			writer1.Write((uint)type.Fields.Count, NativePointerSize);

			// 2. Pointers to Field Definitions
			foreach (var field in type.Fields)
			{
				// Emit field name
				var fieldNameSymbol = EmitStringWithLength(Metadata.NameString + field.FullName, field.Name);

				// Emit field definition
				var fieldDefSymbol = Linker.DefineSymbol(Metadata.FieldDefinition + field.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
				var writer2 = new BinaryWriter(fieldDefSymbol.Stream);

				// 1. Name
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, fieldDefSymbol, writer2.GetPosition(), fieldNameSymbol, (int)Compiler.ObjectHeaderSize);
				writer2.WriteZeroBytes(NativePointerSize);

				// 2. Pointer to Custom Attributes
				if (field.CustomAttributes.Count > 0)
				{
					var customAttributesTableSymbol = CreateCustomAttributesTable(field);
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, fieldDefSymbol, writer2.GetPosition(), customAttributesTableSymbol, 0);
				}
				writer2.WriteZeroBytes(NativePointerSize);

				// 3. Attributes
				writer2.Write((uint)field.FieldAttributes, NativePointerSize);

				// 4. Pointer to Field Type
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, fieldDefSymbol, writer2.GetPosition(), Metadata.TypeDefinition + field.FieldType.FullName, 0);
				writer2.WriteZeroBytes(NativePointerSize);

				// 5 & 6. Offset / Address + Size
				if (field.IsStatic && !field.IsLiteral && !type.HasOpenGenericParams)
				{
					if (Compiler.MethodScanner.IsFieldAccessed(field))
					{
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, fieldDefSymbol, writer2.GetPosition(), field.FullName, 0);
					}
					writer2.WriteZeroBytes(NativePointerSize);
					writer2.Write(field.Data?.Length ?? 0, NativePointerSize);
				}
				else
				{
					writer2.WriteZeroBytes(NativePointerSize);
					writer2.Write(TypeLayout.GetFieldOffset(field), NativePointerSize);
				}

				// Add pointer to field list
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, fieldsTableSymbol, writer1.GetPosition(), fieldDefSymbol, 0);
				writer1.WriteZeroBytes(NativePointerSize);
			}

			return fieldsTableSymbol;
		}

		#endregion Field Definition

		#region Property Definition

		private LinkerSymbol CreatePropertyDefinitions(MosaType type)
		{
			// Emit properties table
			var propertiesTableSymbol = Linker.DefineSymbol(Metadata.PropertiesTable + type.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new BinaryWriter(propertiesTableSymbol.Stream);

			// 1. Number of Properties
			writer.Write((uint)type.Properties.Count, NativePointerSize);

			// 2. Pointers to Property Definitions
			foreach (var property in type.Properties)
			{
				// Emit field name
				var fieldNameSymbol = EmitStringWithLength(Metadata.NameString + property.FullName, property.Name);

				// Emit property definition
				var propertyDefSymbol = Linker.DefineSymbol(Metadata.PropertyDefinition + property.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
				var writer2 = new BinaryWriter(propertyDefSymbol.Stream);

				// 1. Name
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, propertyDefSymbol, writer2.GetPosition(), fieldNameSymbol, (int)Compiler.ObjectHeaderSize);
				writer2.WriteZeroBytes(NativePointerSize);

				// 2. Pointer to Custom Attributes
				if (property.CustomAttributes.Count > 0)
				{
					var customAttributesTableSymbol = CreateCustomAttributesTable(property);
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, propertyDefSymbol, writer.GetPosition(), customAttributesTableSymbol, 0);
				}
				writer.WriteZeroBytes(NativePointerSize);

				// 3. Attributes
				writer.Write((uint)property.PropertyAttributes, NativePointerSize);

				// 4. Pointer to Property Type
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, propertyDefSymbol, writer2.GetPosition(), Metadata.TypeDefinition + property.PropertyType.FullName, 0);
				writer2.WriteZeroBytes(NativePointerSize);

				// If the type is a interface then skip linking the methods
				if (!type.IsInterface)
				{
					// TODO: Replace .HasImpelement with .HasCode

					// 5. Pointer to Getter Method Definition
					if (property.GetterMethod != null && property.GetterMethod.HasImplementation && !property.GetterMethod.HasOpenGenericParams)
					{
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, propertyDefSymbol, writer2.GetPosition(), Metadata.MethodDefinition + property.GetterMethod.FullName, 0);
					}
					writer2.WriteZeroBytes(NativePointerSize);

					// 6. Pointer to Setter Method Definition
					if (property.SetterMethod != null && property.SetterMethod.HasImplementation && !property.SetterMethod.HasOpenGenericParams)
					{
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, propertyDefSymbol, writer2.GetPosition(), Metadata.MethodDefinition + property.SetterMethod.FullName, 0);
					}
					writer2.WriteZeroBytes(NativePointerSize);
				}
				else
				{
					// Fill 5 and 6 with zeros.
					writer.WriteZeroBytes(NativePointerSize * 2);
				}

				// Add pointer to properties table
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, propertiesTableSymbol, writer.GetPosition(), propertyDefSymbol, 0);
				writer.WriteZeroBytes(NativePointerSize);
			}

			return propertiesTableSymbol;
		}

		#endregion Property Definition

		#region Method Definition

		private LinkerSymbol CreateMethodDefinition(MosaMethod method)
		{
			var symbolName = Metadata.MethodDefinition + method.FullName;
			var methodTableSymbol = Linker.GetSymbol(symbolName);

			if (methodTableSymbol.Size != 0)
				return methodTableSymbol;

			// Emit method name
			var methodNameSymbol = EmitStringWithLength(Metadata.NameString + method.FullName, method.FullName);

			// Emit method table
			methodTableSymbol = Linker.DefineSymbol(symbolName, SectionKind.ROData, TypeLayout.NativePointerAlignment, ((uint)method.Signature.Parameters.Count + 9) * NativePointerSize);
			var writer = new BinaryWriter(methodTableSymbol.Stream);

			// 1. Pointer to Name
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodTableSymbol, writer.GetPosition(), methodNameSymbol, (int)Compiler.ObjectHeaderSize);
			writer.WriteZeroBytes(NativePointerSize);

			// 2. Pointer to Custom Attributes
			if (method.CustomAttributes.Count > 0)
			{
				var customAttributeListSymbol = CreateCustomAttributesTable(method);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodTableSymbol, writer.GetPosition(), customAttributeListSymbol, 0);
			}
			writer.WriteZeroBytes(NativePointerSize);

			// 3. Attributes
			writer.Write((uint)method.MethodAttributes, NativePointerSize);

			var targetMethodData = GetTargetMethodData(method);

			// 4. Local Stack Size (16 Bits) && Parameter Stack Size (16 Bits)
			writer.Write(targetMethodData.LocalMethodStackSize | (targetMethodData.ParameterStackSize << 16), NativePointerSize);

			// 5. Pointer to Method
			if (targetMethodData.HasCode)
			{
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodTableSymbol, writer.GetPosition(), targetMethodData.Method.FullName, 0);
			}
			writer.WriteZeroBytes(NativePointerSize);

			// 6. Pointer to return type
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodTableSymbol, writer.GetPosition(), Metadata.TypeDefinition + method.Signature.ReturnType.FullName, 0);
			writer.WriteZeroBytes(NativePointerSize);

			// 7. Pointer to Exception Handler Table
			if (targetMethodData.HasProtectedRegions && targetMethodData.HasCode)
			{
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodTableSymbol, writer.GetPosition(), Metadata.ProtectedRegionTable + targetMethodData.Method.FullName, 0);
			}
			writer.WriteZeroBytes(NativePointerSize);

			// 8. Pointer to GC Tracking information
			// TODO: This has yet to be designed.
			writer.WriteZeroBytes(NativePointerSize);

			// 9. Number of Parameters
			writer.Write((uint)method.Signature.Parameters.Count, NativePointerSize);

			// 10. Pointers to Parameter Definitions
			foreach (var parameter in method.Signature.Parameters)
			{
				// Create definition and get the symbol
				var parameterDefinitionSymbol = CreateParameterDefinition(parameter);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, methodTableSymbol, writer.GetPosition(), parameterDefinitionSymbol, 0);
				writer.WriteZeroBytes(NativePointerSize);
			}

			return methodTableSymbol;
		}

		#endregion Method Definition

		#region Parameter Definition

		private LinkerSymbol CreateParameterDefinition(MosaParameter parameter)
		{
			// Emit parameter name
			var parameterNameSymbol = EmitStringWithLength(Metadata.NameString + parameter, parameter.FullName);

			// Emit parameter table
			var parameterTableSymbol = Linker.DefineSymbol(Metadata.MethodDefinition + parameter.FullName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new BinaryWriter(parameterTableSymbol.Stream);

			// 1. Pointer to Name
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, parameterTableSymbol, writer.GetPosition(), parameterNameSymbol, (int)Compiler.ObjectHeaderSize);
			writer.WriteZeroBytes(NativePointerSize);

			// 2. Pointer to Custom Attributes
			if (parameter.CustomAttributes.Count > 0)
			{
				var customAttributeListSymbol = CreateCustomAttributesTable(parameter);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, parameterTableSymbol, writer.GetPosition(), customAttributeListSymbol, 0);
			}
			writer.WriteZeroBytes(NativePointerSize);

			// 3. Attributes
			writer.Write((uint)parameter.ParameterAttributes, NativePointerSize);

			// 4. Pointer to Parameter Type
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, parameterTableSymbol, writer.GetPosition(), Metadata.TypeDefinition + parameter.ParameterType.FullName, 0);
			writer.WriteZeroBytes(NativePointerSize);

			return parameterTableSymbol;
		}

		#endregion Parameter Definition

		#region Custom Attributes

		private LinkerSymbol CreateCustomAttributesTable(MosaUnit unit)
		{
			var symbolName = Metadata.CustomAttributesTable + unit.FullName;

			var customAttributesTableSymbol = Linker.GetSymbol(symbolName);

			if (customAttributesTableSymbol.Size != 0)
				return customAttributesTableSymbol;

			// Emit custom attributes table
			customAttributesTableSymbol = Linker.DefineSymbol(symbolName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer = new BinaryWriter(customAttributesTableSymbol.Stream);

			// 1. Number of Custom Attributes
			writer.Write(unit.CustomAttributes.Count, NativePointerSize);

			// 2. Pointers to Custom Attributes
			for (int i = 0; i < unit.CustomAttributes.Count; i++)
			{
				// Get custom attribute
				var ca = unit.CustomAttributes[i];

				// Build definition
				var customAttributeTableSymbol = CreateCustomAttribute(unit, ca, i);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, customAttributesTableSymbol, writer.GetPosition(), customAttributeTableSymbol, 0);
				writer.WriteZeroBytes(NativePointerSize);
			}

			return customAttributesTableSymbol;
		}

		private LinkerSymbol CreateCustomAttribute(MosaUnit unit, MosaCustomAttribute ca, int position)
		{
			// Emit custom attribute list
			string name = $"{unit.FullName}>>{position.ToString()}:{ca.Constructor.DeclaringType.Name}";

			var customAttributeSymbol = Linker.DefineSymbol(Metadata.CustomAttribute + name, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new BinaryWriter(customAttributeSymbol.Stream);

			// 1. Pointer to Attribute Type
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, customAttributeSymbol, writer1.GetPosition(), Metadata.TypeDefinition + ca.Constructor.DeclaringType.FullName, 0);
			writer1.WriteZeroBytes(NativePointerSize);

			// 2. Pointer to Constructor Method Definition
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, customAttributeSymbol, writer1.GetPosition(), Metadata.MethodDefinition + ca.Constructor.FullName, 0);

			writer1.WriteZeroBytes(NativePointerSize);

			// 3. Number of Arguments (Both unnamed and named)
			writer1.Write((uint)(ca.Arguments.Length + ca.NamedArguments.Length), NativePointerSize);

			// 4. Pointers to Custom Attribute Arguments (Both unnamed and named)
			for (int i = 0; i < ca.Arguments.Length; i++)
			{
				// Build definition
				var customAttributeArgumentSymbol = CreateCustomAttributeArgument(name, i, null, ca.Arguments[i], false);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, customAttributeSymbol, writer1.GetPosition(), customAttributeArgumentSymbol, 0);
				writer1.WriteZeroBytes(NativePointerSize);
			}

			foreach (var namedArg in ca.NamedArguments)
			{
				// Build definition
				var customAttributeArgumentSymbol = CreateCustomAttributeArgument(name, 0, namedArg.Name, namedArg.Argument, namedArg.IsField);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, customAttributeSymbol, writer1.GetPosition(), customAttributeArgumentSymbol, 0);
				writer1.WriteZeroBytes(NativePointerSize);
			}

			return customAttributeSymbol;
		}

		private LinkerSymbol CreateCustomAttributeArgument(string name, int count, string argName, MosaCustomAttribute.Argument arg, bool isField)
		{
			var attributeName = $"{name}:{(argName ?? count.ToString())}";
			var symbolName = Metadata.CustomAttributeArgument + attributeName;

			var customAttributeArgumentSymbol = Linker.GetSymbol(symbolName);

			if (customAttributeArgumentSymbol.Size != 0)
				return customAttributeArgumentSymbol;

			customAttributeArgumentSymbol = Linker.DefineSymbol(symbolName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new BinaryWriter(customAttributeArgumentSymbol.Stream);

			// 1. Pointer to name (if named)
			if (argName != null)
			{
				var nameSymbol = EmitStringWithLength(Metadata.NameString + attributeName, argName);
				Linker.Link(LinkType.AbsoluteAddress, NativePatchType, customAttributeArgumentSymbol, writer1.GetPosition(), nameSymbol, (int)Compiler.ObjectHeaderSize);
			}
			writer1.WriteZeroBytes(NativePointerSize);

			// 2. Is Argument A Field
			writer1.Write(isField, NativePointerSize);

			// 3. Argument Type Pointer
			Linker.Link(LinkType.AbsoluteAddress, NativePatchType, customAttributeArgumentSymbol, writer1.GetPosition(), Metadata.TypeDefinition + arg.Type.FullName, 0);
			writer1.WriteZeroBytes(NativePointerSize);

			// 4. Argument Size
			writer1.Write(ComputeArgumentSize(arg.Type, arg.Value), NativePointerSize);

			// 5. Argument Value
			WriteArgument(writer1, customAttributeArgumentSymbol, arg.Type, arg.Value);

			return customAttributeArgumentSymbol;
		}

		private uint ComputeArgumentSize(MosaType type, object value)
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
					uint size = 0;
					foreach (var elem in arr)
						size += ComputeArgumentSize(elem.Type, elem.Value);
					return size;

				// String
				case MosaTypeCode.String:
					return NativePointerSize;

				default:
					if (type.FullName == "System.Type")
					{
						return NativePointerSize;
					}
					else
					{
						throw new NotSupportedException();
					}
			}
		}

		private void WriteArgument(BinaryWriter writer, LinkerSymbol symbol, MosaType type, object value)
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
					writer.Write(arr.Length, NativePointerSize);
					foreach (var elem in arr)
						WriteArgument(writer, symbol, elem.Type, elem.Value);
					break;

				// String
				case MosaTypeCode.String:

					// Since strings are immutable, make it an object that we can just use
					var str = (string)value;
					writer.WriteZeroBytes(NativePointerSize);
					Linker.Link(LinkType.AbsoluteAddress, NativePatchType, symbol, writer.GetPosition(), Metadata.TypeDefinition + "System.String", 0);
					writer.WriteZeroBytes(NativePointerSize);
					writer.Write(str.Length, NativePointerSize);
					writer.Write(Encoding.Unicode.GetBytes(str));
					break;

				default:
					if (type.FullName == "System.Type")
					{
						var valueType = (MosaType)value;
						Linker.Link(LinkType.AbsoluteAddress, NativePatchType, symbol, writer.GetPosition(), Metadata.TypeDefinition + valueType.FullName, 0);
						writer.WriteZeroBytes(NativePointerSize);
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
