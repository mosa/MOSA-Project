using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mosa.Runtime.Linker;
using Mosa.Compiler.Linker;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// 
	/// </summary>
	public class MethodTableBuilderStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage
	{
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

			foreach (var type in this.typeSystem.GetAllTypes())
			{
				if (type.ContainsOpenGenericParameters)
					continue;
				if (type.IsModule || type.IsGeneric || type.IsDelegate)
					continue;
				if (type.IsInterface)
					continue;

				foreach (var method in type.Methods)
					if (this.linker.HasSymbol(method.ToString()))
						table.Add(this.linker.GetSymbol(method.ToString()));
			}

			var size = 3 * table.Count * this.typeLayout.NativePointerSize;
			var offsetPointer = 0;
			var offsets = new Dictionary<LinkerSymbol, long>();

			using (var stream = this.linker.Allocate("methodTableStart", SectionKind.Text, size, this.typeLayout.NativePointerAlignment))
			{
				var position = stream.Position;
				foreach (var entry in table)
				{
					offsets[entry] = offsetPointer;
					this.linker.Link(LinkType.AbsoluteAddress | LinkType.I4, "methodTableStart", offsetPointer, 0, entry.Name, IntPtr.Zero);
					this.linker.Link(LinkType.AbsoluteAddress | LinkType.I4, "methodTableStart", offsetPointer + 8, 0, entry.Name + "$methodDescriptionEntry", IntPtr.Zero);
					offsetPointer += 3 * this.typeLayout.NativePointerSize;
				}

				stream.Position = position;
				foreach (var entry in table)
				{
					stream.Seek(offsets[entry] + 4, System.IO.SeekOrigin.Begin);
					stream.Write(BitConverter.GetBytes(entry.Length), 0, 4);
				}
			}

			foreach (var entry in table)
			{
				using (var stream = this.linker.Allocate(entry.Name + "$methodDescriptionEntry", SectionKind.Text, 4, this.typeLayout.NativePointerAlignment))
				{
					stream.Write(BitConverter.GetBytes(0xFFEEDDCC), 0, 4);
				}
			}
		}
	}
}
