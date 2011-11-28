/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.Collections.Generic;
using System.IO;
using Mosa.Compiler.Common;
using Mosa.Compiler.Linker;
using Mosa.Compiler.TypeSystem;
using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86
{
	/// <summary>
	/// Builds the method table used for exception handling. The table has the following format:
	///		4 bytes: Pointer to method
	///		4 bytes: Length of method
	///		4 bytes: Pointer to method description entry
	///		
	/// The method description entry has the format:
	///		4 bytes: Pointer to exception clause table
	///		4 bytes: GC tracking info
	/// </summary>
	public class MethodTableBuilderStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage
	{
		/// <summary>
		/// 
		/// </summary>
		private static readonly DataConverter LittleEndianBitConverter = DataConverter.LittleEndian;

		/// <summary>
		/// 
		/// </summary>
		private IAssemblyLinker linker;

		/// <summary>
		/// 
		/// </summary>
		public static readonly byte[] blank4 = new byte[4];

		/// <summary>
		/// Setups the specified compiler.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
		{
			base.Setup(compiler);
			linker = RetrieveAssemblyLinkerFromCompiler();
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IAssemblyCompilerStage.Run()
		{
			this.CreateTable();
		}

		/// <summary>
		/// Creates the table.
		/// </summary>
		private void CreateTable()
		{
			var table = new List<LinkerSymbol>();
			var methods = new List<RuntimeMethod>();

			// Collect all methods that we can link to
			foreach (var type in this.typeSystem.GetAllTypes())
			{
				if (type.ContainsOpenGenericParameters)
					continue;
				if (type.IsModule || type.IsGeneric)
					continue;
				if (type.IsInterface)
					continue;

				foreach (var method in type.Methods)
				{
					if (linker.HasSymbol(method.ToString()))
					{
						table.Add(linker.GetSymbol(method.ToString()));

						if (!methods.Contains(method))
							methods.Add(method);
					}
				}
			}

			CreateMethodLookupTable(table);
			CreateMethodDescriptionEntries(methods);
		}

		/// <summary>
		/// Creates the method description table.
		/// </summary>
		/// <param name="table">The table.</param>
		private void CreateMethodLookupTable(IList<LinkerSymbol> table)
		{
			// Allocate the table and fill it
			var size = 3 * table.Count * typeLayout.NativePointerSize + typeLayout.NativePointerSize;
			var offsetPointer = 0;
			var offsets = new Dictionary<LinkerSymbol, long>();

			using (var stream = linker.Allocate("<$>methodLookupTable", SectionKind.Text, size, typeLayout.NativePointerAlignment))
			{
				foreach (var entry in table)
				{
					// Store the offset pointer
					offsets[entry] = offsetPointer;

					// Store address (the linker writes the actual entry)
					linker.Link(LinkType.AbsoluteAddress | LinkType.I4, "<$>methodLookupTable", offsetPointer, 0, entry.Name, IntPtr.Zero);
					// Store the length (it copied in by the next loop)

					// Store the pointer to the method description table (the linker writes the actual entry)
					linker.Link(LinkType.AbsoluteAddress | LinkType.I4, "<$>methodLookupTable", offsetPointer + 8, 0, entry.Name + "$mdtable", IntPtr.Zero);
					offsetPointer += 3 * typeLayout.NativePointerSize;
				}

				// Mark end of table
				stream.Position = offsetPointer;
				stream.Write(blank4);

				// Store pointers to method description entries
				foreach (var entry in table)
				{
					stream.Position = offsets[entry] + typeLayout.NativePointerSize;
					stream.Write(LittleEndianBitConverter.GetBytes(entry.Length), 0, 4);
				}

			}
		}

		/// <summary>
		/// Creates the method description entries.
		/// </summary>
		/// <param name="methods">The methods.</param>
		private void CreateMethodDescriptionEntries(IList<RuntimeMethod> methods)
		{
			foreach (var method in methods)
			{
				int size = 2 * typeLayout.NativePointerSize;

				using (var stream = linker.Allocate(method.FullName + "$mdtable", SectionKind.Text, size, typeLayout.NativePointerAlignment))
				{
					// Pointer to exception clause table
					linker.Link(LinkType.AbsoluteAddress | LinkType.I4, method.FullName + "$mdtable", 0, 0, method.FullName + "$etable", IntPtr.Zero);
					stream.Position += typeLayout.NativePointerSize;

					// GC tracking info (not implemented yet)
					stream.Write(blank4);
				}
			}
		}

	}
}
