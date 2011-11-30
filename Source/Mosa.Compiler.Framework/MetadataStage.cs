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

namespace Mosa.Compiler.Framework
{
	/// <summary>
	/// Emits metadata for assemblies and types
	/// </summary>
	public sealed class MetadataStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage
	{
		#region Data members

		private IAssemblyLinker linker;

		private readonly DataConverter LittleEndianBitConverter = DataConverter.LittleEndian;

		#endregion // Data members

		#region IAssemblyCompilerStage members

		void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
		{
			base.Setup(compiler);
			this.linker = RetrieveAssemblyLinkerFromCompiler();
		}

		void IAssemblyCompilerStage.Run()
		{
			CreateAssemblyListTable();
		}

		#endregion // IAssemblyCompilerStage members

		private void EmitStringWithLength(Stream stream, string value)
		{
			stream.Write(LittleEndianBitConverter.GetBytes(value.Length));
			stream.Write(System.Text.ASCIIEncoding.ASCII.GetBytes(value));
		}

		private void EmitInteger(Stream stream, uint value)
		{
			stream.Write(LittleEndianBitConverter.GetBytes(value));
		}

		private void CreateAssemblyListTable()
		{
			string assemblyListSymbol = @"$assembly-list";

			using (Stream stream = linker.Allocate(assemblyListSymbol, SectionKind.ROData, 0, typeLayout.NativePointerAlignment))
			{
				// 1. Number of assemblies (modules)
				EmitInteger(stream, (uint)typeSystem.TypeModules.Count);

				// 2. Pointers to assemblies
				foreach (var module in typeSystem.TypeModules)
				{
					linker.Link(LinkType.AbsoluteAddress | LinkType.I4, assemblyListSymbol, (int)stream.Position, 0, module.Name + "$atable", IntPtr.Zero);
					stream.Position += typeLayout.NativePointerSize;
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
				EmitStringWithLength(stream, typeModule.Name);
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
				// 1. Pointer to Assembly Name
				linker.Link(LinkType.AbsoluteAddress | LinkType.I4, assemblyTableSymbol, 0, 0, assemblyNameSymbol, IntPtr.Zero);
				stream.Position += typeLayout.NativePointerSize;

				// 2. Number of types
				EmitInteger(stream, moduleTypes);

				// 3. Pointer to list of types
				foreach (var type in typeModule.GetAllTypes())
				{
					if (!type.IsModule && !(type.Module is InternalTypeModule))
						linker.Link(LinkType.AbsoluteAddress | LinkType.I4, assemblyTableSymbol, (int)stream.Position, 0, type.FullName + @"$dtable", IntPtr.Zero);
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
				EmitStringWithLength(stream, type.FullName);
			}

			string typeTableSymbol = type.FullName + @"$dtable";

			using (Stream stream = linker.Allocate(typeTableSymbol, SectionKind.ROData, 0, typeLayout.NativePointerAlignment))
			{
				// 1. Size
				EmitInteger(stream, (uint)typeLayout.GetTypeSize(type));

				// 2. Metadata Token
				EmitInteger(stream, (uint)type.Token.ToUInt32());

				// 3. Pointer to Name
				linker.Link(LinkType.AbsoluteAddress | LinkType.I4, typeTableSymbol, (int)stream.Position, 0, typeNameSymbol, IntPtr.Zero);
				stream.Position = +typeLayout.NativePointerSize;

				// 4. Pointer to Assembly Definition
				linker.Link(LinkType.AbsoluteAddress | LinkType.I4, typeTableSymbol, (int)stream.Position, 0, assemblySymbol, IntPtr.Zero);
				stream.Position = +typeLayout.NativePointerSize;

				// 5. isInterface
				stream.WriteByte((byte)(type.IsInterface ? 1 : 0));

				// 6. Constructor that accept no parameters, if any, for this type
				// TODO
			}

		}


	}
}
