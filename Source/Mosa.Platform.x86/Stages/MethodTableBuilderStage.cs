/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;
using Mosa.Compiler.TypeSystem;
using System.Collections.Generic;

namespace Mosa.Platform.x86.Stages
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
	public class MethodTableBuilderStage : BaseCompilerStage, ICompilerStage
	{
		/// <summary>
		/// Setups the specified compiler.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		void ICompilerStage.Setup(BaseCompiler compiler)
		{
			base.Setup(compiler);
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void ICompilerStage.Run()
		{
			CreateTables();
		}

		/// <summary>
		/// Creates the table.
		/// </summary>
		private void CreateTables()
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
						table.Add(linker.GetSymbol(method.FullName));

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

			string section = "<$>methodLookupTable";

			using (var stream = linker.Allocate(section, SectionKind.ROData, size, typeLayout.NativePointerAlignment))
			{
				foreach (var entry in table)
				{
					// 1. Store address (the linker writes the actual entry)
					linker.Link(LinkType.AbsoluteAddress | LinkType.I4, BuildInPatch.I4, section, (int)stream.Position, 0, entry.Name, 0);
					stream.Position += typeLayout.NativePointerSize;

					// 2. Store the length (its copied in by the next loop)
					stream.Write((uint)entry.Length, Endianness.Little);

					// 3. Store the pointer to the method description table (the linker writes the actual entry)
					linker.Link(LinkType.AbsoluteAddress | LinkType.I4, BuildInPatch.I4, section, (int)stream.Position, 0, entry.Name + "$mdtable", 0);
					stream.Position += typeLayout.NativePointerSize;
				}

				// Mark end of table
				stream.Position += typeLayout.NativePointerSize;
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
				int size = 3 * typeLayout.NativePointerSize;

				string section = method.FullName + "$mdtable";

				using (var stream = linker.Allocate(section, SectionKind.ROData, size, typeLayout.NativePointerAlignment))
				{
					// Pointer to Exception Handler Table
					// TODO: If there is no exception clause table, set to 0 and do not involve linker
					linker.Link(LinkType.AbsoluteAddress | LinkType.I4, BuildInPatch.I4, section, 0, 0, method.FullName + "$etable", 0);
					stream.Position += typeLayout.NativePointerSize;

					// GC tracking info (not implemented yet)
					stream.WriteZeroBytes(typeLayout.NativePointerSize);

					// Method's Parameter stack size
					stream.Write(DetermineSizeOfMethodParameters(method), Endianness.Little); // FIXME
				}
			}
		}

		protected uint DetermineSizeOfMethodParameters(RuntimeMethod method)
		{
			// TODO
			return 0;
		}
	}
}