﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;

namespace Mosa.Platform.x86.CompilerStages
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
	public class MethodTableBuilderStage : BaseCompilerStage
	{
		/// <summary>
		/// Executes this stage.
		/// </summary>
		protected override void RunPostCompile()
		{
			CreateTables();
		}

		/// <summary>
		/// Creates the table.
		/// </summary>
		private void CreateTables()
		{
			var table = new List<LinkerSymbol>();
			var methods = new List<MosaMethod>();

			// Collect all methods that we can link to
			foreach (var type in TypeSystem.AllTypes)
			{
				if (type.IsModule || type.HasOpenGenericParams || type.IsInterface)
					continue;

				foreach (var method in type.Methods)
				{
					var symbol = Linker.GetSymbol(method.FullName, SectionKind.Text);

					table.Add(symbol);

					if (!methods.Contains(method))
						methods.Add(method);
				}
			}

			CreateMethodLookupTable(table);
			CreateMethodDescriptionEntries(methods);
		}

		/// <summary>
		/// Creates the method description table.
		/// </summary>
		/// <param name="table">The table.</param>
		private void CreateMethodLookupTable(List<LinkerSymbol> table)
		{
			// Allocate the table and fill it
			var size = 3 * table.Count * TypeLayout.NativePointerSize + TypeLayout.NativePointerSize;

			var methodtable = Linker.CreateSymbol("<$>methodLookupTable", SectionKind.ROData, TypeLayout.NativePointerAlignment, size);
			var stream = methodtable.Stream;

			foreach (var entry in table)
			{
				// 1. Store address (the linker writes the actual entry)
				Linker.Link(LinkType.AbsoluteAddress, PatchType.I4, methodtable, (int)stream.Position, SectionKind.Text, entry.Name, 0);
				stream.Position += TypeLayout.NativePointerSize;

				// 2. Store the length (its copied in by the next loop)
				stream.Write(entry.Size, Endianness.Little);

				// 3. Store the pointer to the method description table (the linker writes the actual entry)
				Linker.Link(LinkType.AbsoluteAddress, PatchType.I4, methodtable, (int)stream.Position, SectionKind.ROData, entry.Name + "$mdtable", 0);
				stream.Position += TypeLayout.NativePointerSize;
			}

			// Mark end of table
			stream.Position += TypeLayout.NativePointerSize;
		}

		/// <summary>
		/// Creates the method description entries.
		/// </summary>
		/// <param name="methods">The methods.</param>
		private void CreateMethodDescriptionEntries(IList<MosaMethod> methods)
		{
			foreach (var method in methods)
			{
				int size = 3 * TypeLayout.NativePointerSize;

				var section = Linker.CreateSymbol(method.FullName + "$mdtable", SectionKind.ROData, TypeLayout.NativePointerAlignment, size);
				var stream = section.Stream;

				// Pointer to Exception Handler Table
				// TODO: If there is no exception clause table, set to 0 and do not involve linker
				Linker.Link(LinkType.AbsoluteAddress, PatchType.I4, section, 0, SectionKind.ROData, method.FullName + "$etable", 0);
				stream.Position += TypeLayout.NativePointerSize;

				// GC tracking info (not implemented yet)
				stream.WriteZeroBytes(TypeLayout.NativePointerSize);

				// Method's Parameter stack size
				stream.Write(DetermineSizeOfMethodParameters(method), Endianness.Little); // FIXME
			}
		}

		protected uint DetermineSizeOfMethodParameters(MosaMethod method)
		{
			// TODO
			return 0;
		}
	}
}
