// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System.IO;
using System.Text;

namespace Mosa.Platform.Intel.CompilerStages
{
	/// <summary>
	/// Writes a multiboot v0.6.95 header into the generated binary.
	/// </summary>
	/// <remarks>
	/// This compiler stage writes a multiboot header into the
	/// the data section of the binary file and also creates a multiboot
	/// compliant entry point into the binary.<para/>
	/// The header and entry point written by this stage is compliant with
	/// the specification at
	/// http://www.gnu.org/software/grub/manual/multiboot/multiboot.html.
	/// </remarks>
	public abstract class MultibootV1Stage : BaseCompilerStage
	{
		protected abstract void CreateMultibootMethod();

		#region Constants

		/// <summary>
		/// This address is the top of the initial kernel stack.
		/// </summary>
		protected const uint STACK_ADDRESS = 0x00A00000 - 8;

		/// <summary>
		/// Magic value in the multiboot header.
		/// </summary>
		protected const uint HEADER_MB_MAGIC = 0x1BADB002U;

		/// <summary>
		/// Multiboot flag, which indicates that additional modules must be
		/// loaded on page boundaries.
		/// </summary>
		protected const uint HEADER_MB_FLAG_MODULES_PAGE_ALIGNED = 0x00000001U;

		/// <summary>
		/// Multiboot flag, which indicates if memory information must be
		/// provided in the boot information structure.
		/// </summary>
		protected const uint HEADER_MB_FLAG_MEMORY_INFO_REQUIRED = 0x00000002U;

		/// <summary>
		/// Multiboot flag, which indicates that the supported video mode
		/// table must be provided in the boot information structure.
		/// </summary>
		protected const uint HEADER_MB_FLAG_VIDEO_MODES_REQUIRED = 0x00000004U;

		/// <summary>
		/// Multiboot flag, which indicates a non-elf binary to boot and that
		/// settings for the executable file should be read From the boot header
		/// instead of the executable header.
		/// </summary>
		protected const uint HEADER_MB_FLAG_NON_ELF_BINARY = 0x00010000U;

		#endregion Constants

		#region Data Members

		/// <summary>
		/// The multiboot method
		/// </summary>
		protected MosaMethod multibootMethod;

		/// <summary>
		/// The multiboot header
		/// </summary>
		protected LinkerSymbol multibootHeader;

		#endregion Data Members

		public bool HasVideo { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int Depth { get; set; }

		protected override void Initialization()
		{
			HasVideo = CompilerSettings.Settings.GetValue("Multiboot.Video", false);
			Width = CompilerSettings.Settings.GetValue("Multiboot.Video.Width", 0);
			Height = CompilerSettings.Settings.GetValue("Multiboot.Video.Geight", 0);
			Depth = CompilerSettings.Settings.GetValue("Multiboot.Video.Depth", 0);
		}

		protected override void Setup()
		{
			multibootHeader = Linker.DefineSymbol(MultibootHeaderSymbolName, SectionKind.Text, 1, 0x30);

			Linker.DefineSymbol(MultibootEAX, SectionKind.BSS, Architecture.NativeAlignment, Architecture.NativePointerSize);
			Linker.DefineSymbol(MultibootEBX, SectionKind.BSS, Architecture.NativeAlignment, Architecture.NativePointerSize);

			multibootMethod = Compiler.CreateLinkerMethod("MultibootInit");

			Linker.EntryPoint = Linker.GetSymbol(multibootMethod.FullName);

			Compiler.GetMethodData(multibootMethod).DoNotInline = true;
			MethodScanner.MethodInvoked(multibootMethod, multibootMethod);

			var startUpType = TypeSystem.GetTypeByName("Mosa.Runtime", "StartUp");
			var initializeMethod = startUpType.FindMethodByName("Initialize");

			Compiler.GetMethodData(initializeMethod).DoNotInline = true;

			MethodScanner.MethodInvoked(initializeMethod, multibootMethod);
		}

		protected override void Finalization()
		{
			CreateMultibootMethod();

			WriteMultibootHeader();
		}

		#region Internals

		private const string MultibootHeaderSymbolName = "<$>mosa-multiboot-header";
		public const string MultibootEAX = "<$>mosa-multiboot-eax";
		public const string MultibootEBX = "<$>mosa-multiboot-ebx";

		/// <summary>
		/// Writes the multiboot header.
		/// </summary>
		/// <param name="entryPoint">The virtualAddress of the multiboot compliant entry point.</param>
		protected void WriteMultibootHeader()
		{
			// According to the multiboot specification this header must be within the first 8K of the kernel binary.
			Linker.SetFirst(multibootHeader);

			var stream = multibootHeader.Stream;

			var writer = new BinaryWriter(stream, Encoding.ASCII);

			// flags - multiboot flags
			uint flags = HEADER_MB_FLAG_MEMORY_INFO_REQUIRED | HEADER_MB_FLAG_MODULES_PAGE_ALIGNED;

			if (HasVideo)
				flags |= HEADER_MB_FLAG_VIDEO_MODES_REQUIRED;

			const uint load_addr = 0;

			// magic
			writer.Write(HEADER_MB_MAGIC);

			// flags
			writer.Write(flags);

			// checksum
			writer.Write(unchecked(0U - HEADER_MB_MAGIC - flags));

			// header_addr - load address of the multiboot header
			Linker.Link(LinkType.AbsoluteAddress, PatchType.I32, multibootHeader, (int)stream.Position, multibootHeader, 0);
			writer.Write(0);

			// load_addr - address of the binary in memory
			writer.Write(load_addr);

			// load_end_addr - address past the last byte to load from the image
			writer.Write(0);

			// bss_end_addr - address of the last byte to be zeroed out
			writer.Write(0);

			// entry_addr - address of the entry point to invoke
			Linker.Link(LinkType.AbsoluteAddress, PatchType.I32, multibootHeader, (int)stream.Position, Linker.EntryPoint, 0);
			writer.Write(0);

			// Write video settings if video has been specified, otherwise pad
			if (HasVideo)
			{
				writer.Write(0); // Mode, 0 = linear
				writer.Write(Width); // Width, 1280px
				writer.Write(Height); // Height, 720px
				writer.Write(Depth); // Depth, 24px
			}
			else
			{
				writer.Write(0);
				writer.Write(0);
				writer.Write(0);
				writer.Write(0);
			}
		}

		#endregion Internals
	}
}
