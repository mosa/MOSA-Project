using System;
using System.Collections.Generic;
using System.IO;
using Mosa.Compiler.Common;
using Mosa.Compiler.Linker;
using Mosa.Compiler.TypeSystem;

namespace Mosa.Compiler.Framework
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
		/// Setups the specified compiler.
		/// </summary>
		/// <param name="compiler">The compiler.</param>
		void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
		{
			base.Setup(compiler);
			this.linker = this.RetrieveAssemblyLinkerFromCompiler();
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

			CreateMethodDescriptionTable(table);
			CreateMethodDescriptionEntries(methods);
		}

		/// <summary>
		/// Creates the method description table.
		/// </summary>
		/// <param name="table">The table.</param>
		private void CreateMethodDescriptionTable(IList<LinkerSymbol> table)
		{
			// Allocate the table and fill it
			var size = 3 * table.Count * typeLayout.NativePointerSize + typeLayout.NativePointerSize;
			var offsetPointer = 0;
			var offsets = new Dictionary<LinkerSymbol, long>();

			using (var stream = this.linker.Allocate("methodTableStart", SectionKind.Text, size, typeLayout.NativePointerAlignment))
			{
				var position = stream.Position;
				foreach (var entry in table)
				{
					// Store the offset pointer
					offsets[entry] = offsetPointer;

					// Store address and length of the method
					this.linker.Link(LinkType.AbsoluteAddress | LinkType.I4, "methodTableStart", offsetPointer, 0, entry.Name, IntPtr.Zero);
					this.linker.Link(LinkType.AbsoluteAddress | LinkType.I4, "methodTableStart", offsetPointer + 8, 0, entry.Name + "$mdtable", IntPtr.Zero);
					offsetPointer += 3 * typeLayout.NativePointerSize;
				}

				// Store pointers to method description entries
				stream.Position = position;
				foreach (var entry in table)
				{
					stream.Seek(offsets[entry] + 4, SeekOrigin.Begin);
					stream.Write(LittleEndianBitConverter.GetBytes(entry.Length), 0, 4);
				}

				// Mark end of table
				stream.Seek(size - typeLayout.NativePointerSize, SeekOrigin.Begin);
				stream.Write(new byte[] { 0, 0, 0, 0 }, 0, 4);
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
				using (var stream = this.linker.Allocate(method.FullName + "$mdtable", SectionKind.Text, 2 * typeLayout.NativePointerSize, typeLayout.NativePointerAlignment))
				{
					// Pointer to exception clause table
					linker.Link(LinkType.AbsoluteAddress | LinkType.I4, method.FullName + "$mdtable", 0, 0, method.FullName + "$etable", IntPtr.Zero);
					// GC tracking info
					stream.Seek(1 * typeLayout.NativePointerSize, SeekOrigin.Begin);
					stream.Write(LittleEndianBitConverter.GetBytes(0x00000000), 0, 4);
				}
			}
		}

	}
}
