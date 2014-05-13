/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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

		private void EmitStringWithLength(EndianAwareBinaryWriter stream, string value)
		{
			stream.Write(value.Length);
			stream.Write(System.Text.ASCIIEncoding.ASCII.GetBytes(value));
		}

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
			var assemblyNameSymbol = Linker.CreateSymbol(module.Assembly + Metadata.AssemblyName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(assemblyNameSymbol.Stream, Architecture.Endianness);

			EmitStringWithLength(writer1, module.Assembly);

			// Emit assembly table
			var assemblyTableSymbol = Linker.CreateSymbol(module.Assembly + Metadata.AssemblyDefinition, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer2 = new EndianAwareBinaryWriter(assemblyTableSymbol.Stream, Architecture.Endianness);

			// 1. Pointer to Assembly Name
			Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, assemblyTableSymbol, (int)writer2.Position, 0, assemblyNameSymbol, 0);
			writer2.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 2. Number of Types
			writer2.Write((uint)module.Types.Values.Count); // TODO: fix count

			// 3. Pointers to Types
			// Create the definitions along the way
			foreach (var type in module.Types.Values)
			{
				if (type.IsModule)
					continue;

				// For reflection, even types having opening generic params should be written
				//if (type.HasOpenGenericParams)
				//    continue;

				if (type.BaseType == null && !type.IsInterface && type.FullName != "System.Object")   // ghost types like generic params, function ptr, etc.
					continue;

				var typeTableSymbol = CreateTypeDefinitionTable(type, assemblyTableSymbol);

				// Link
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, assemblyTableSymbol, (int)writer2.Position, 0, typeTableSymbol, 0);
				writer2.WriteZeroBytes(TypeLayout.NativePointerSize);
			}

			// Return assemblyTableSymbol
			return assemblyTableSymbol;
		}

		private LinkerSymbol CreateTypeDefinitionTable(MosaType type, LinkerSymbol assemblyTableSymbol)
		{
			// Emit type name
			var typeNameSymbol = Linker.CreateSymbol(type + Metadata.TypeName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(typeNameSymbol.Stream, Architecture.Endianness);

			EmitStringWithLength(writer1, type.FullName);

			var typeTableSymbol = Linker.CreateSymbol(type.FullName + Metadata.TypeDefinition, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer2 = new EndianAwareBinaryWriter(typeTableSymbol.Stream, Architecture.Endianness);

			// 1. Size
			writer2.Write((uint)TypeLayout.GetTypeSize(type));

			// 2. Metadata Token [to be removed]
			writer2.Write((uint)0);

			// 3. Pointer to Name
			Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, typeTableSymbol, (int)writer2.Position, 0, typeNameSymbol, 0);
			writer2.Position += TypeLayout.NativePointerSize;

			// 4. Pointer to Assembly Definition
			Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, typeTableSymbol, (int)writer2.Position, 0, assemblyTableSymbol, 0);
			writer2.Position += TypeLayout.NativePointerSize;

			// 5. Constructor that accepts no parameters, if any, for this type
			MosaMethod paramlessConstructor = null;

			foreach (var method in type.Methods)
			{
				if (!method.Name.Equals(".ctor") || !(method.Signature.Parameters.Count == 0) || method.HasOpenGenericParams)
					continue;

				paramlessConstructor = method;
				break;
			}

			if (paramlessConstructor == null)
			{
				writer2.WriteZeroBytes(TypeLayout.NativePointerSize);
			}
			else
			{
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, typeTableSymbol, (int)writer2.Position, 0, paramlessConstructor.FullName, SectionKind.Text, 0);
				writer2.Position += TypeLayout.NativePointerSize;
			}

			// 6. Flag: IsInterface
			writer2.WriteByte((byte)(type.IsInterface ? 1 : 0));

			CreateFieldDefinitions(type);

			// Return typeTableSymbol for linker usage
			return typeTableSymbol;
		}

		private void CreateFieldDefinitions(MosaType type)
		{
			foreach (MosaField field in type.Fields)
			{
				// Emit field name
				var fieldNameSymbol = Linker.CreateSymbol(field.FullName + Metadata.FieldName, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
				var writer1 = new EndianAwareBinaryWriter(fieldNameSymbol.Stream, Architecture.Endianness);

				EmitStringWithLength(writer1, field.Name);

				// Emit field definition
				var fieldDefSymbol = Linker.CreateSymbol(field.FullName + Metadata.FieldDefinition, SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
				var writer2 = new EndianAwareBinaryWriter(fieldDefSymbol.Stream, Architecture.Endianness);

				// 1. Name
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, fieldDefSymbol, (int)writer2.Position, 0, fieldNameSymbol.Name, SectionKind.ROData, 0);
				writer2.Position += TypeLayout.NativePointerSize;

				// 2 & 3. Offset / Address + Size
				if (field.IsStatic && !field.IsLiteral)
				{
					var section = (field.Data != null) ? SectionKind.ROData : SectionKind.BSS;
					Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, fieldDefSymbol, (int)writer2.Position, 0, field.FullName, section, 0);
					writer2.Position += TypeLayout.NativePointerSize;
					writer2.Write((field.Data != null) ? field.Data.Length : 0);
				}
				else
				{
					writer2.Write(TypeLayout.GetFieldOffset(field));
				}

			}
		}
	}
}