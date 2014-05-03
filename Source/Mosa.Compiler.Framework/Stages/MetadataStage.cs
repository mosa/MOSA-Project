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
			CreateTypeDefinitionTables();
		}

		private void EmitStringWithLength(EndianAwareBinaryWriter stream, string value)
		{
			stream.Write(value.Length);
			stream.Write(System.Text.ASCIIEncoding.ASCII.GetBytes(value));
		}

		private void CreateTypeDefinitionTables()
		{
			foreach (var type in TypeSystem.AllTypes)
			{
				if (type.IsModule)
					continue;

				// For reflection, even types having opening generic params should be written
				//if (type.HasOpenGenericParams)
				//    continue;

				if (type.BaseType == null && !type.IsInterface && type.FullName != "System.Object")   // ghost types like generic params, function ptr, etc.
					continue;

				CreateTypeDefinitionTable(type);
			}
		}

		private void CreateTypeDefinitionTable(MosaType type)
		{
			// Emit type name
			var typeNameSymbol = Linker.CreateSymbol(type + @"$tname", SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer1 = new EndianAwareBinaryWriter(typeNameSymbol.Stream, Architecture.Endianness);

			EmitStringWithLength(writer1, type.FullName);

			var typeTableSymbol = Linker.CreateSymbol(type.FullName + @"$dtable", SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
			var writer2 = new EndianAwareBinaryWriter(typeTableSymbol.Stream, Architecture.Endianness);

			// 1. Size
			writer2.Write((uint)TypeLayout.GetTypeSize(type));

			// 2. Metadata Token
			writer2.Write((uint)0); //TODO?

			// 3. Pointer to Name
			Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, typeTableSymbol, (int)writer2.Position, 0, typeNameSymbol, 0);
			writer2.Position += TypeLayout.NativePointerSize;

			// 4. Pointer to Assembly Definition
			//linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, typeTableSymbol, (int)writer.Position, 0, assemblySymbol, 0);
			writer2.Position += TypeLayout.NativePointerSize;

			// 5. TODO: Constructor that accepts no parameters, if any, for this type
			writer2.WriteZeroBytes(TypeLayout.NativePointerSize);

			// 6. Flag: IsInterface
			writer2.WriteByte((byte)(type.IsInterface ? 1 : 0));

			CreateFieldDefinitions(type);
		}

		private void CreateFieldDefinitions(MosaType type)
		{
			foreach (MosaField field in type.Fields)
			{
				// Emit field name
				var fieldNameSymbol = Linker.CreateSymbol(field.FullName + @"$name", SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
				var writer1 = new EndianAwareBinaryWriter(fieldNameSymbol.Stream, Architecture.Endianness);

				EmitStringWithLength(writer1, field.Name);

				// Emit field descriptor
				var fieldDescSymbol = Linker.CreateSymbol(field.FullName + @"$desc", SectionKind.ROData, TypeLayout.NativePointerAlignment, 0);
				var writer2 = new EndianAwareBinaryWriter(fieldDescSymbol.Stream, Architecture.Endianness);

				// 1. Offset / Address
				if (field.IsStatic && !field.IsLiteral)
				{
					var section = (field.Data != null) ? SectionKind.Data : SectionKind.BSS;

					Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, fieldDescSymbol, (int)writer2.Position, 0, field.FullName, section, 0);
				}
				else
				{
					writer2.Write(TypeLayout.GetFieldOffset(field));
					writer2.Position -= 4;
				}
				writer2.Position += TypeLayout.NativePointerSize;

				// 2. Name
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, fieldDescSymbol, (int)writer2.Position, 0, fieldNameSymbol.Name, SectionKind.ROData, 0);
				writer2.Position += TypeLayout.NativePointerSize;

				// 3. Size
				writer2.Write((uint)TypeLayout.GetFieldSize(field));

				// 4. Metadata Token
				writer2.Write((uint)0); // TODO
			}
		}
	}
}