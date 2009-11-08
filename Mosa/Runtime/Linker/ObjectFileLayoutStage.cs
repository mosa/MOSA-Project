using System;
using System.Collections.Generic;
using System.Text;
using Mosa.Runtime.CompilerFramework;

namespace Mosa.Runtime.Linker
{
	/// <summary>
	/// Lays out sections and symbols sequentially in an object file.
	/// </summary>
	public class ObjectFileLayoutStage : IAssemblyCompilerStage, IPipelineStage
	{
		#region Methods

		/// <summary>
		/// Lays out the sections in order of appearance.
		/// </summary>
		/// <param name="linker">The linker.</param>
		protected virtual void LayoutSections(IAssemblyLinker linker)
		{
			long fileAlignment = linker.LoadSectionAlignment;
			long sectionAlignment = linker.VirtualSectionAlignment;

			// Reset the size of the image
			long virtualSizeOfImage = sectionAlignment;
			long fileSizeOfImage = fileAlignment;

			// Move all sections to their right positions
			foreach (LinkerSection ls in linker.Sections) {
				// Only use a section with something inside
				if (ls.Length > 0) {
					// Set the section virtualAddress
					ls.VirtualAddress = new IntPtr(linker.BaseAddress + virtualSizeOfImage);
					ls.Offset = fileSizeOfImage;

					// Update the file size
					fileSizeOfImage += ls.Length;
					fileSizeOfImage = AlignValue(fileSizeOfImage, fileAlignment);

					// Update the virtual size
					virtualSizeOfImage += ls.Length;
					virtualSizeOfImage = AlignValue(virtualSizeOfImage, sectionAlignment);
				}
			}
		}

		/// <summary>
		/// Lays the symbols out according to their offset in the section.
		/// </summary>
		/// <param name="linker">The linker.</param>
		protected virtual void LayoutSymbols(IAssemblyLinker linker)
		{
			// Adjust the symbol addresses
			foreach (LinkerSymbol symbol in linker.Symbols) {
				LinkerSection ls = linker.GetSection(symbol.Section);
				symbol.Offset = ls.Offset + symbol.SectionAddress;
				symbol.VirtualAddress = new IntPtr(ls.VirtualAddress.ToInt64() + symbol.SectionAddress);
			}
		}

		/// <summary>
		/// Aligns the value to the requested alignment.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="alignment">The alignment.</param>
		/// <returns>The aligned value.</returns>
		protected long AlignValue(long value, long alignment)
		{
			return (value + (alignment - (value % alignment)));
		}

		#endregion // Methods

		#region IPipelineStage members

		string IPipelineStage.Name { get { return @"Linker Layout Stage"; } }


		private static PipelineStageOrder[] _pipelineOrder = new PipelineStageOrder[] {
				// TODO
			};

		/// <summary>
		/// Gets the pipeline stage order.
		/// </summary>
		/// <value>The pipeline stage order.</value>
		PipelineStageOrder[] IPipelineStage.PipelineStageOrder { get { return _pipelineOrder; } }

		#endregion // IPipelineStage members

		#region IAssemblyCompilerStage Overrides

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public void Run(AssemblyCompiler compiler)
		{
			IAssemblyLinker linker = compiler.Pipeline.Find<IAssemblyLinker>();
			if (linker == null)
				throw new InvalidOperationException(@"ObjectFileLayoutStage needs a linker.");

			LayoutSections(linker);
			LayoutSymbols(linker);
		}

		#endregion // IAssemblyCompilerStage Overrides
	}
}
