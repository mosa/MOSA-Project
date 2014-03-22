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
using System.IO;

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
			string typeNameSymbol = type + @"$tname";

			// Emit type name
			using (Stream stream = Linker.Allocate(typeNameSymbol, SectionKind.ROData, 0, TypeLayout.NativePointerAlignment))
			{
				using (EndianAwareBinaryWriter writer = new EndianAwareBinaryWriter(stream, Architecture.Endianness))
				{
					EmitStringWithLength(writer, type.FullName);
				}
			}

			string typeTableSymbol = type.FullName + @"$dtable";

			using (Stream stream = Linker.Allocate(typeTableSymbol, SectionKind.ROData, 0, TypeLayout.NativePointerAlignment))
			{
				using (EndianAwareBinaryWriter writer = new EndianAwareBinaryWriter(stream, Architecture.Endianness))
				{
					// 1. Size
					writer.Write((uint)TypeLayout.GetTypeSize(type));

					// 2. Metadata Token
					//writer.Write((uint)type.Token.ToUInt32());
					writer.Write((uint)0); //FIXME! ^^^

					// 3. Pointer to Name
					Linker.Link(LinkType.AbsoluteAddress | LinkType.I4, BuiltInPatch.I4, typeTableSymbol, (int)writer.Position, 0, typeNameSymbol, 0);
					writer.Position += TypeLayout.NativePointerSize;

					// 4. Pointer to Assembly Definition
					//linker.Link(LinkType.AbsoluteAddress | LinkType.I4, BuiltInPatch.I4, typeTableSymbol, (int)writer.Position, 0, assemblySymbol, 0);
					writer.Position += TypeLayout.NativePointerSize;

					// 5. TODO: Constructor that accepts no parameters, if any, for this type
					writer.WriteZeroBytes(TypeLayout.NativePointerSize);

					// 6. Flag: IsInterface
					writer.WriteByte((byte)(type.IsInterface ? 1 : 0));
				}
			}

			CreateFieldDefinitions(type);
		}

		private void CreateFieldDefinitions(MosaType type)
		{
			foreach (MosaField field in type.Fields)
			{
				string fieldNameSymbol = field.FullName + @"$name";

				// Emit field name
				using (Stream stream = Linker.Allocate(fieldNameSymbol, SectionKind.ROData, 0, TypeLayout.NativePointerAlignment))
				{
					using (EndianAwareBinaryWriter writer = new EndianAwareBinaryWriter(stream, Architecture.Endianness))
					{
						EmitStringWithLength(writer, field.Name);
					}
				}

				string fieldDescSymbol = field.FullName + @"$desc";

				// Emit field descriptor
				using (Stream stream = Linker.Allocate(fieldDescSymbol, SectionKind.ROData, 0, TypeLayout.NativePointerAlignment))
				{
					using (EndianAwareBinaryWriter writer = new EndianAwareBinaryWriter(stream, Architecture.Endianness))
					{
						// 1. Offset / Address
						if (field.IsStatic && !field.IsLiteral)
						{
							Linker.Link(LinkType.AbsoluteAddress | LinkType.I4, BuiltInPatch.I4, fieldDescSymbol, (int)writer.Position, 0, field.FullName, 0);
						}
						else
						{
							writer.Write(TypeLayout.GetFieldOffset(field));
							writer.Position -= 4;
						}
						writer.Position += TypeLayout.NativePointerSize;

						// 2. Name
						Linker.Link(LinkType.AbsoluteAddress | LinkType.I4, BuiltInPatch.I4, fieldDescSymbol, (int)writer.Position, 0, fieldNameSymbol, 0);
						writer.Position += TypeLayout.NativePointerSize;

						// 3. Size
						writer.Write((uint)TypeLayout.GetFieldSize(field));

						// 4. Metadata Token
						writer.Write((uint)0); //FIXME!
					}
				}
			}
		}
	}
}