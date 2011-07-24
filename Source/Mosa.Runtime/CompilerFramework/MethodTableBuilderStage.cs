using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mosa.Runtime.Linker;
using Mosa.Compiler.Linker;
using System.IO;
using Mosa.Runtime.TypeSystem;
using Mosa.Compiler.Common;

namespace Mosa.Runtime.CompilerFramework
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
	///		4 bytes: Method metadata token
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
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>
		/// The name of the compilation stage.
		/// </value>
		public string Name
		{
			get { return @"MethodTableBuilderStage"; }
		}

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
				if (type.IsModule || type.IsGeneric || type.IsDelegate)
					continue;
				if (type.IsInterface)
					continue;

				foreach (var method in type.Methods)
				{
					if (this.linker.HasSymbol(method.ToString()))
					{
						table.Add(this.linker.GetSymbol(method.ToString()));
						methods.Add(method);
					}
				}
			}

			this.CreateMethodDescriptionTable(table);
			this.CreateMethodDescriptionEntries(methods);
		}

		/// <summary>
		/// Creates the method description table.
		/// </summary>
		/// <param name="table">The table.</param>
		private void CreateMethodDescriptionTable(IList<LinkerSymbol> table)
		{
			// Allocate the table and fill it
			var size = 3 * table.Count * this.typeLayout.NativePointerSize + this.typeLayout.NativePointerSize;
			var offsetPointer = 0;
			var offsets = new Dictionary<LinkerSymbol, long>();

			using (var stream = this.linker.Allocate("methodTableStart", SectionKind.Text, size, this.typeLayout.NativePointerAlignment))
			{
				var position = stream.Position;
				foreach (var entry in table)
				{
					// Store the offset pointer
					offsets[entry] = offsetPointer;

					// Store address and length of the method
					this.linker.Link(LinkType.AbsoluteAddress | LinkType.I4, "methodTableStart", offsetPointer, 0, entry.Name, IntPtr.Zero);
					this.linker.Link(LinkType.AbsoluteAddress | LinkType.I4, "methodTableStart", offsetPointer + 8, 0, entry.Name + "$mdtable", IntPtr.Zero);
					offsetPointer += 3 * this.typeLayout.NativePointerSize;
				}

				// Store pointers to method description entries
				stream.Position = position;
				foreach (var entry in table)
				{
					stream.Seek(offsets[entry] + 4, SeekOrigin.Begin);
					stream.Write(LittleEndianBitConverter.GetBytes(entry.Length), 0, 4);
				}

				// Mark end of table
				stream.Seek(size - this.typeLayout.NativePointerSize, SeekOrigin.Begin);
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
				using (var stream = this.linker.Allocate(method.ToString() + "$mdtable", SectionKind.Text, 3 * this.typeLayout.NativePointerSize, this.typeLayout.NativePointerAlignment))
				{
					this.CreateMethodDescriptionEntry(stream, method);
				}
			}
		}

		/// <summary>
		/// Creates the method description entry.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <param name="method">The method.</param>
		private void CreateMethodDescriptionEntry(Stream stream, RuntimeMethod method)
		{
			// Pointer to exception clause table
			//TODO: Uncomment when etable is ready

			//this.linker.Link(LinkType.AbsoluteAddress | LinkType.I4, method.ToString() + "$mdtable", 0, 0, method.ToString() + "$etable", IntPtr.Zero);
			// GC tracking info
			stream.Seek(1 * this.typeLayout.NativePointerSize, SeekOrigin.Begin);
			stream.Write(LittleEndianBitConverter.GetBytes(0x00000000), 0, 4);
			// Metadata token
			stream.Seek(2 * this.typeLayout.NativePointerSize, SeekOrigin.Begin);
			stream.Write(LittleEndianBitConverter.GetBytes(method.Token.ToInt32()), 0, 4);
		}
	}
}
