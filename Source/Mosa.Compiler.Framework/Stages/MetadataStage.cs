/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;

using Mosa.Compiler.Common;
using Mosa.Compiler.Linker;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Emits metadata for assemblies and types
	/// </summary>
	public sealed class MetadataStage : BaseCompilerStage, ICompilerStage
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private ILinker linker;

		#endregion // Data members

		#region ICompilerStage members

		void ICompilerStage.Setup(BaseCompiler compiler)
		{
			base.Setup(compiler);
			this.linker = RetrieveLinkerFromCompiler();
		}

		void ICompilerStage.Run()
		{
			CreateAssemblyListTable();
		}

		#endregion // ICompilerStage members

		private void EmitStringWithLength(EndianAwareBinaryWriter stream, string value)
		{
			stream.Write(value.Length);
			stream.Write(System.Text.ASCIIEncoding.ASCII.GetBytes(value));
		}

		private void CreateAssemblyListTable()
		{
			string assemblyListSymbol = @"<$>AssemblyList";

			using (Stream stream = linker.Allocate(assemblyListSymbol, SectionKind.ROData, 0, typeLayout.NativePointerAlignment))
			{
				using (EndianAwareBinaryWriter writer = new EndianAwareBinaryWriter(stream, architecture.IsLittleEndian))
				{
					// 1. Number of assemblies (modules)
					writer.Write((uint)typeSystem.TypeModules.Count);

					// 2. Pointers to assemblies
					foreach (var module in typeSystem.TypeModules)
					{
						linker.Link(LinkType.AbsoluteAddress | LinkType.NativeI4, assemblyListSymbol, (int)writer.Position, 0, module.Name + "$atable", IntPtr.Zero);
						writer.Position += typeLayout.NativePointerSize;
					}
				}
			}

			// Create a table per assembly
			foreach (var module in typeSystem.TypeModules)
			{
				CreateAssemblyDefinitionTable(module);
			}
		}

		private void CreateAssemblyDefinitionTable(ITypeModule typeModule)
		{
			string assemblyNameSymbol = typeModule.Name + @"$aname";

			// Emit assembly name
			using (Stream stream = linker.Allocate(assemblyNameSymbol, SectionKind.ROData, 0, typeLayout.NativePointerAlignment))
			{
				using (EndianAwareBinaryWriter writer = new EndianAwareBinaryWriter(stream, architecture.IsLittleEndian))
				{
					EmitStringWithLength(writer, typeModule.Name);
				}
			}

			uint moduleTypes = 0;

			foreach (RuntimeType type in typeModule.GetAllTypes())
			{
				if (!type.IsModule)
					moduleTypes++;
			}

			string assemblyTableSymbol = typeModule.Name + @"$atable";

			using (Stream stream = linker.Allocate(assemblyTableSymbol, SectionKind.ROData, 0, typeLayout.NativePointerAlignment))
			{
				using (EndianAwareBinaryWriter writer = new EndianAwareBinaryWriter(stream, architecture.IsLittleEndian))
				{
					// 1. Pointer to Assembly Name
					linker.Link(LinkType.AbsoluteAddress | LinkType.NativeI4, assemblyTableSymbol, 0, 0, assemblyNameSymbol, IntPtr.Zero);
					writer.Position += typeLayout.NativePointerSize;

					// 2. Number of types
					writer.Write(moduleTypes);

					// 3. Pointer to list of types
					foreach (var type in typeModule.GetAllTypes())
					{
						if (!type.IsModule && !(type.Module is InternalTypeModule))
							linker.Link(LinkType.AbsoluteAddress | LinkType.NativeI4, assemblyTableSymbol, (int)writer.Position, 0, type.FullName + @"$dtable", IntPtr.Zero);

						writer.Position += typeLayout.NativePointerSize;
					}
				}
			}

			foreach (var type in typeModule.GetAllTypes())
			{
				if (!type.IsModule)
				{
					CreateTypeDefinitionTable(type, assemblyTableSymbol);
				}
			}
		}

		private void CreateTypeDefinitionTable(RuntimeType type, string assemblySymbol)
		{
			string typeNameSymbol = type + @"$tname";

			// Emit type name
			using (Stream stream = linker.Allocate(typeNameSymbol, SectionKind.ROData, 0, typeLayout.NativePointerAlignment))
			{
				using (EndianAwareBinaryWriter writer = new EndianAwareBinaryWriter(stream, architecture.IsLittleEndian))
				{
					EmitStringWithLength(writer, type.FullName);
				}
			}

			string typeTableSymbol = type.FullName + @"$dtable";

			using (Stream stream = linker.Allocate(typeTableSymbol, SectionKind.ROData, 0, typeLayout.NativePointerAlignment))
			{
				using (EndianAwareBinaryWriter writer = new EndianAwareBinaryWriter(stream, architecture.IsLittleEndian))
				{
					// 1. Size
					writer.Write((uint)typeLayout.GetTypeSize(type));

					// 2. Metadata Token
					writer.Write((uint)type.Token.ToUInt32());

					// 3. Pointer to Name
					linker.Link(LinkType.AbsoluteAddress | LinkType.NativeI4, typeTableSymbol, (int)writer.Position, 0, typeNameSymbol, IntPtr.Zero);
					writer.Position += typeLayout.NativePointerSize;

					// 4. Pointer to Assembly Definition
					linker.Link(LinkType.AbsoluteAddress | LinkType.NativeI4, typeTableSymbol, (int)writer.Position, 0, assemblySymbol, IntPtr.Zero);
					writer.Position += typeLayout.NativePointerSize;

					// 5. TODO: Constructor that accept no parameters, if any, for this type
					writer.WriteZeroBytes(typeLayout.NativePointerSize);

					// 6. Flag: IsInterface
					writer.WriteByte((byte)(type.IsInterface ? 1 : 0));
				}
			}

		}


	}
}
