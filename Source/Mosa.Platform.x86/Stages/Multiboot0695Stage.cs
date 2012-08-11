/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Kai P. Reisert <kpreisert@googlemail.com>
 */

using System;
using System.IO;
using System.Text;
using Mosa.Compiler.Framework;
using Mosa.Compiler.Linker;
using Mosa.Compiler.Metadata.Signatures;

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
	public sealed class Multiboot0695Stage : BaseCompilerStage, ICompilerStage, IPipelineStage
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

		#endregion // Constants

		#region Data members

		private ILinker linker;

		/// <summary>
		/// Holds the multiboot video mode.
		/// </summary>
		public uint VideoMode { get; set; }

		/// <summary>
		/// Holds the videoWidth of the screen for the video mode.
		/// </summary>
		public uint VideoWidth { get; set; }

		/// <summary>
		/// Holds the height of the screen for the video mode.
		/// </summary>
		public uint VideoHeight { get; set; }

		/// <summary>
		/// Holds the depth of the video mode in bits per pixel.
		/// </summary>
		public uint VideoDepth { get; set; }

		/// <summary>
		/// Holds true if the second stage is reached
		/// </summary>
		private bool secondStage;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Multiboot0695Stage"/> class.
		/// </summary>
		public Multiboot0695Stage()
		{
			VideoMode = 1;
			VideoWidth = 80;
			VideoHeight = 25;
			VideoDepth = 0;
			secondStage = false;
		}

		#endregion // Construction

		#region ICompilerStage Members

		void ICompilerStage.Setup(BaseCompiler compiler)
		{
			base.Setup(compiler);
			linker = RetrieveLinkerFromCompiler();

			if (compiler.CompilerOptions.Multiboot.VideoDepth.HasValue)
				this.VideoDepth = compiler.CompilerOptions.Multiboot.VideoDepth.Value;
			if (compiler.CompilerOptions.Multiboot.VideoHeight.HasValue)
				this.VideoHeight = compiler.CompilerOptions.Multiboot.VideoHeight.Value;
			if (compiler.CompilerOptions.Multiboot.VideoMode.HasValue)
				this.VideoMode = compiler.CompilerOptions.Multiboot.VideoMode.Value;
			if (compiler.CompilerOptions.Multiboot.VideoWidth.HasValue)
				this.VideoWidth = compiler.CompilerOptions.Multiboot.VideoWidth.Value;
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void ICompilerStage.Run()
		{
			if (!secondStage)
			{
				WriteMultibootHeader();
				secondStage = true;
			}
			else
			{
				ITypeInitializerSchedulerStage typeInitializerSchedulerStage = this.compiler.Pipeline.FindFirst<ITypeInitializerSchedulerStage>();

				SigType I4 = BuiltInSigType.Int32;
				Operand ecx = Operand.CreateCPURegister(I4, GeneralPurposeRegister.ECX);
				Operand eax = Operand.CreateCPURegister(I4, GeneralPurposeRegister.EAX);
				Operand ebx = Operand.CreateCPURegister(I4, GeneralPurposeRegister.EBX);

				InstructionSet instructionSet = new InstructionSet(16);
				Context ctx = new Context(instructionSet);

				ctx.AppendInstruction(X86.Mov, ecx, Operand.CreateConstant(I4, 0x200000));
				ctx.AppendInstruction(X86.Mov, Operand.CreateMemoryAddress(I4, ecx.Register, new IntPtr(0x0)), eax);
				ctx.AppendInstruction(X86.Mov, Operand.CreateMemoryAddress(I4, ecx.Register, new IntPtr(0x4)), ebx);

				Operand entryPoint = Operand.CreateSymbolFromMethod(typeInitializerSchedulerStage.TypeInitializerMethod);

				ctx.AppendInstruction(X86.Call, null, entryPoint);
				ctx.AppendInstruction(X86.Ret);

				LinkerGeneratedMethod method = LinkTimeCodeGenerator.Compile(this.compiler, @"MultibootInit", instructionSet, typeSystem);
				linker.EntryPoint = linker.GetSymbol(method.ToString());
			}
		}

		#endregion // ICompilerStage Members

		#region Internals

		private const string MultibootHeaderSymbolName = @"<$>mosa-multiboot-header";

		/// <summary>
		/// Writes the multiboot header.
		/// </summary>
		/// <param name="entryPoint">The virtualAddress of the multiboot compliant entry point.</param>
		private void WriteMultibootHeader()
		{
			// HACK: According to the multiboot specification this header must be within the first 8K of the
			// kernel binary. Since the text section is always first, this should take care of the problem.
			using (Stream stream = linker.Allocate(MultibootHeaderSymbolName, SectionKind.Text, 64, 4))
			{
				using (BinaryWriter bw = new BinaryWriter(stream, Encoding.ASCII))
				{
					// flags - multiboot flags
					uint flags = /*HEADER_MB_FLAG_VIDEO_MODES_REQUIRED | */HEADER_MB_FLAG_MEMORY_INFO_REQUIRED | HEADER_MB_FLAG_MODULES_PAGE_ALIGNED;
					// The multiboot header checksum 
					uint csum = 0;
					// header_addr is the load virtualAddress of the multiboot header
					uint header_addr = 0;
					// load_addr is the base virtualAddress of the binary in memory
					uint load_addr = 0;
					// load_end_addr holds the virtualAddress past the last byte to load From the image
					uint load_end_addr = 0;
					// bss_end_addr is the virtualAddress of the last byte to be zeroed out
					uint bss_end_addr = 0;
					// entry_point the load virtualAddress of the entry point to invoke

					// entry_point the load virtualAddress of the entry point to invoke
					// Are we linking an ELF binary?
					if (!(linker is Mosa.Compiler.Linker.Elf32.Linker || linker is Mosa.Compiler.Linker.Elf64.Linker))
					{
						// Check the linker layout settings
						if (linker.LoadSectionAlignment != linker.VirtualSectionAlignment)
							throw new LinkerException(@"Load and virtual section alignment must be identical if you are booting non-ELF binaries with a multiboot bootloader.");

						// No, special multiboot treatment required
						flags |= HEADER_MB_FLAG_NON_ELF_BINARY;

						header_addr = (uint)(linker.GetSection(SectionKind.Text).VirtualAddress.ToInt64() + linker.GetSymbol(MultibootHeaderSymbolName).SectionAddress);
						load_addr = (uint)linker.BaseAddress;
						load_end_addr = 0;
						bss_end_addr = 0;
					}

					// Calculate the checksum
					csum = unchecked(0U - HEADER_MB_MAGIC - flags);

					bw.Write(HEADER_MB_MAGIC);
					bw.Write(flags);
					bw.Write(csum);
					bw.Write(header_addr);
					bw.Write(load_addr);
					bw.Write(load_end_addr);
					bw.Write(bss_end_addr);

					linker.Link(LinkType.AbsoluteAddress | LinkType.NativeI4, MultibootHeaderSymbolName, (int)stream.Position, 0, @"Mosa.Tools.Compiler.LinkerGenerated.<$>MultibootInit()", IntPtr.Zero);
					bw.Write((int)0);

					bw.Write(VideoMode);
					bw.Write(VideoWidth);
					bw.Write(VideoHeight);
					bw.Write(VideoDepth);
				}
			}
		}

		#endregion // Internals
	}
}
