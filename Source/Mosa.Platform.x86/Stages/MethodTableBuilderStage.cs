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
using Mosa.Compiler.MosaTypeSystem;
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
	public class MethodTableBuilderStage : BaseCompilerStage
	{
		/// <summary>
		/// Executes this stage.
		/// </summary>
		protected override void Run()
		{
			CreateTables();
		}

		/// <summary>
		/// Creates the table.
		/// </summary>
		private void CreateTables()
		{
			var table = new List<LinkerObject>();
			var methods = new List<MosaMethod>();

			// Collect all methods that we can link to
			foreach (var type in TypeSystem.AllTypes)
			{
				if (type.IsModule || type.HasOpenGenericParams || type.IsInterface)
					continue;

				foreach (var method in type.Methods)
				{
					var symbol = Linker.GetLinkerObject(method.FullName, SectionKind.Text);

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
		private void CreateMethodLookupTable(IList<LinkerObject> table)
		{
			// Allocate the table and fill it
			var size = 3 * table.Count * TypeLayout.NativePointerSize + TypeLayout.NativePointerSize;

			var methodtable = Linker.AllocateLinkerObject("<$>methodLookupTable", SectionKind.ROData, size, TypeLayout.NativePointerAlignment);
			var stream = methodtable.Stream;

			foreach (var entry in table)
			{
				// 1. Store address (the linker writes the actual entry)
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, methodtable, (int)stream.Position, 0, entry.Name, SectionKind.Text, 0);
				stream.Position += TypeLayout.NativePointerSize;

				// 2. Store the length (its copied in by the next loop)
				stream.Write((uint)entry.Size, Endianness.Little);

				// 3. Store the pointer to the method description table (the linker writes the actual entry)
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, methodtable, (int)stream.Position, 0, entry.Name + "$mdtable", SectionKind.ROData, 0);
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

				var section = Linker.AllocateLinkerObject(method.FullName + "$mdtable", SectionKind.ROData, size, TypeLayout.NativePointerAlignment);
				var stream = section.Stream;

				// Pointer to Exception Handler Table
				// TODO: If there is no exception clause table, set to 0 and do not involve linker
				Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, section, 0, 0, method.FullName + "$etable", SectionKind.ROData, 0);
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