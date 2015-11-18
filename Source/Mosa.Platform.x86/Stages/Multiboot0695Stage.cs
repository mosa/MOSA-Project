// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;
using Mosa.Compiler.Framework.Stages;
using Mosa.Compiler.Linker;
using Mosa.Compiler.MosaTypeSystem;
using System.IO;
using System.Text;

namespace Mosa.Platform.x86.Stages
{
	/*
	 * FIXME:
	 * - Allow video mode options to be controlled by the command line
	 * - Allow the specification of additional load modules on the command line
	 * - Write the multiboot compliant entry point, which parses the the boot
	 *   information structure and populates the appropriate fields in the
	 *   KernelBoot entry.
	 */

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
	public sealed class Multiboot0695Stage : BaseCompilerStage
	{
		#region Constants

		/// <summary>
		/// Magic value in the multiboot header.
		/// </summary>
		private const uint HEADER_MB_MAGIC = 0x1BADB002U;

		/// <summary>
		/// Multiboot flag, which indicates that additional modules must be
		/// loaded on page boundaries.
		/// </summary>
		private const uint HEADER_MB_FLAG_MODULES_PAGE_ALIGNED = 0x00000001U;

		/// <summary>
		/// Multiboot flag, which indicates if memory information must be
		/// provided in the boot information structure.
		/// </summary>
		private const uint HEADER_MB_FLAG_MEMORY_INFO_REQUIRED = 0x00000002U;

		/// <summary>
		/// Multiboot flag, which indicates that the supported video mode
		/// table must be provided in the boot information structure.
		/// </summary>
		private const uint HEADER_MB_FLAG_VIDEO_MODES_REQUIRED = 0x00000004U;

		/// <summary>
		/// Multiboot flag, which indicates a non-elf binary to boot and that
		/// settings for the executable file should be read From the boot header
		/// instead of the executable header.
		/// </summary>
		private const uint HEADER_MB_FLAG_NON_ELF_BINARY = 0x00010000U;

		#endregion Constants

		#region Data members

		/// <summary>
		/// The multiboot method
		/// </summary>
		private MosaMethod multibootMethod;

		/// <summary>
		/// The multiboot header
		/// </summary>
		private LinkerSymbol multibootHeader;

		#endregion Data members

		protected override void Run()
		{
			if (multibootMethod == null)
			{
				multibootHeader = Linker.CreateSymbol(MultibootHeaderSymbolName, SectionKind.Text, 1, 0x30);

				multibootMethod = Compiler.CreateLinkerMethod("MultibootInit");

				Linker.EntryPoint = Linker.GetSymbol(multibootMethod.FullName, SectionKind.Text);

				WriteMultibootHeader();

				Linker.CreateSymbol(MultibootEAX, SectionKind.BSS, Architecture.NativeAlignment, Architecture.NativePointerSize);
				Linker.CreateSymbol(MultibootEBX, SectionKind.BSS, Architecture.NativeAlignment, Architecture.NativePointerSize);

				return;
			}

			var typeInitializerSchedulerStage = Compiler.PostCompilePipeline.FindFirst<TypeInitializerSchedulerStage>();

			var ecx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.ECX);
			var eax = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EAX);
			var ebx = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EBX);
			var ebp = Operand.CreateCPURegister(TypeSystem.BuiltIn.I4, GeneralPurposeRegister.EBP);
			var multibootEax = Operand.CreateUnmanagedSymbolPointer(TypeSystem, MultibootEAX);
			var multibootEbx = Operand.CreateUnmanagedSymbolPointer(TypeSystem, MultibootEBX);

			var basicBlocks = new BasicBlocks();
			var block = basicBlocks.CreateBlock();
			basicBlocks.AddHeaderBlock(block);
			var ctx = new Context(block);

			// set sentinel on the stack to indicate the start of the stack
			var zero = Operand.CreateConstant(TypeSystem.BuiltIn.I4, 0);
			ctx.AppendInstruction(X86.Mov, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.I4, ebp, 0), zero);

			ctx.AppendInstruction(X86.Mov, ecx, multibootEax);
			ctx.AppendInstruction(X86.Mov, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.I4, ecx, 0), eax);
			ctx.AppendInstruction(X86.Mov, ecx, multibootEbx);
			ctx.AppendInstruction(X86.Mov, Operand.CreateMemoryAddress(TypeSystem.BuiltIn.I4, ecx, 0), ebx);

			// call type initializer
			var entryPoint = Operand.CreateSymbolFromMethod(TypeSystem, typeInitializerSchedulerStage.TypeInitializerMethod);
			ctx.AppendInstruction(X86.Call, null, entryPoint);

			// should never get here
			ctx.AppendInstruction(X86.Ret);

			Compiler.CompileMethod(multibootMethod, basicBlocks, 0);
		}

		#region Internals

		private const string MultibootHeaderSymbolName = @"<$>mosa-multiboot-header";
		public const string MultibootEAX = @"<$>mosa-multiboot-eax";
		public const string MultibootEBX = @"<$>mosa-multiboot-ebx";

		/// <summary>
		/// Writes the multiboot header.
		/// </summary>
		/// <param name="entryPoint">The virtualAddress of the multiboot compliant entry point.</param>
		private void WriteMultibootHeader()
		{
			// HACK: According to the multiboot specification this header must be within the first 8K of the
			// kernel binary. Since the text section is always first, this should take care of the problem.
			//multibootHeader = Linker.GetSymbol(MultibootHeaderSymbolName, SectionKind.Text);
			var stream = multibootHeader.Stream;

			var writer = new BinaryWriter(stream, Encoding.ASCII);

			// flags - multiboot flags
			uint flags = HEADER_MB_FLAG_MEMORY_INFO_REQUIRED | HEADER_MB_FLAG_MODULES_PAGE_ALIGNED;// | HEADER_MB_FLAG_VIDEO_MODES_REQUIRED;

			uint load_addr = 0;

			// magic
			writer.Write(HEADER_MB_MAGIC);

			// flags
			writer.Write(flags);

			// checksum
			writer.Write(unchecked(0U - HEADER_MB_MAGIC - flags));

			// header_addr - load address of the multiboot header
			Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, multibootHeader, (int)stream.Position, 0, multibootHeader, 0);
			writer.Write(0);

			// load_addr - address of the binary in memory
			writer.Write(load_addr);

			// load_end_addr - address past the last byte to load from the image
			writer.Write(0);

			// bss_end_addr - address of the last byte to be zeroed out
			writer.Write(0);

			// entry_addr - address of the entry point to invoke
			Linker.Link(LinkType.AbsoluteAddress, BuiltInPatch.I4, multibootHeader, (int)stream.Position, 0, Linker.EntryPoint, 0);
			writer.Write(0);
		}

		#endregion Internals
	}
}
