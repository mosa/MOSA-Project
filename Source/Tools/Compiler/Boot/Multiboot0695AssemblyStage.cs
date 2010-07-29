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
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Collections.Generic;
using NDesk.Options;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.Linker;
using Mosa.Runtime.Linker.Elf32;
using Mosa.Runtime.Linker.Elf64;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Tools.Compiler.LinkTimeCodeGeneration;
using Mosa.Platforms.x86;
using Mosa.Tools.Compiler.TypeInitializers;

using IR = Mosa.Runtime.CompilerFramework.IR;
using CPUx86 = Mosa.Platforms.x86.CPUx86;

namespace Mosa.Tools.Compiler.Boot
{
	using Runtime.Metadata;

	/*
	 * FIXME:
	 * - Allow video mode options to be controlled by the command line
	 * - Allow the specification of additional load modules on the command line
	 * - Write the multiboot compliant entry point, which parses the the boot
	 *   information structure and populates the appropriate fields in the 
	 *   KernelBoot entry.
	 */

	/// <summary>
	/// Writes a multiboot v0.6.95 _header into the generated binary.
	/// </summary>
	/// <remarks>
	/// This assembly compiler stage writes a multiboot _header into the
	/// the data section of the binary file and also creates a multiboot
	/// compliant entry point into the binary.<para/>
	/// The _header and entry point written by this stage is compliant with
	/// the specification at 
	/// http://www.gnu.org/software/grub/manual/multiboot/multiboot.html.
	/// </remarks>
	public sealed class Multiboot0695AssemblyStage : BaseAssemblyCompilerStage, IAssemblyCompilerStage, IHasOptions, IPipelineStage
	{
		#region Constants

		/// <summary>
		/// Magic value in the multiboot _header.
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
		/// settings for the executable file should be read From the boot _header
		/// instead of the executable _header.
		/// </summary>
		private const uint HEADER_MB_FLAG_NON_ELF_BINARY = 0x00010000U;

		#endregion // Constants

		#region Data members
		
		private IAssemblyLinker linker;

		/// <summary>
		/// Holds the multiboot video mode.
		/// </summary>
		private uint videoMode;

		/// <summary>
		/// Holds the videoWidth of the screen for the video mode.
		/// </summary>
		private uint videoWidth;

		/// <summary>
		/// Holds the height of the screen for the video mode.
		/// </summary>
		private uint videoHeight;

		/// <summary>
		/// Holds the depth of the video mode in bits per pixel.
		/// </summary>
		private uint videoDepth;

		/// <summary>
		/// Holds true if the second stage is reached
		/// </summary>
		private bool secondStage;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="Multiboot0695AssemblyStage"/> class.
		/// </summary>
		public Multiboot0695AssemblyStage()
		{
			videoMode = 1;
			videoWidth = 80;
			videoHeight = 25;
			videoDepth = 0;
			secondStage = false;
		}

		#endregion // Construction

		#region IPipelineStage

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"MultibootAssemblyStage"; } }

		#endregion // IPipelineStage Members

		#region IAssemblyCompilerStage Members

		void IAssemblyCompilerStage.Setup(AssemblyCompiler compiler)
		{
			base.Setup(compiler);

			this.linker = compiler.Pipeline.FindFirst<IAssemblyLinker>();
			Debug.Assert(linker != null, @"No linker??");
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IAssemblyCompilerStage.Run()
		{
			if (!secondStage) {
				IntPtr entryPoint = WriteMultibootEntryPoint();
				WriteMultibootHeader(entryPoint);
				secondStage = true;
			}
			else {
				var typeInitializerSchedulerStage = this.compiler.Pipeline.FindFirst<ITypeInitializerSchedulerStage>();

				SigType I4 = new SigType(CilElementType.I4);
				RegisterOperand ecx = new RegisterOperand(I4, GeneralPurposeRegister.ECX);
				RegisterOperand eax = new RegisterOperand(I4, GeneralPurposeRegister.EAX);
				RegisterOperand ebx = new RegisterOperand(I4, GeneralPurposeRegister.EBX);

				InstructionSet instructionSet = new InstructionSet(16);
				Context ctx = new Context(instructionSet, -1);

				ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, ecx, new ConstantOperand(I4, 0x200000));
				ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(I4, ecx.Register, new IntPtr(0x0)), eax);
				ctx.AppendInstruction(CPUx86.Instruction.MovInstruction, new MemoryOperand(I4, ecx.Register, new IntPtr(0x4)), ebx);

			    SymbolOperand entryPoint = SymbolOperand.FromMethod(typeInitializerSchedulerStage.Method);

				ctx.AppendInstruction(CPUx86.Instruction.CallInstruction, null, entryPoint);
				ctx.AppendInstruction(CPUx86.Instruction.RetInstruction);

				CompilerGeneratedMethod method = LinkTimeCodeGenerator.Compile(this.compiler, @"MultibootInit", instructionSet);
				this.linker.EntryPoint = this.linker.GetSymbol(method.ToString());
			}
		}

		#endregion // IAssemblyCompilerStage Members

		#region Internals

		/// <summary>
		/// Writes the multiboot entry point.
		/// </summary>
		/// <returns>The virtualAddress of the real entry point.</returns>
		private IntPtr WriteMultibootEntryPoint()
		{
			/*
			 * FIXME:
			 * 
			 * We can't use the standard entry point of the module. Instead
			 * we write a multiboot compliant entry point here, which populates
			 * the boot structure - so that it can be retrieved later.
			 * 
			 * Unfortunately this means, we need to define the boot structure
			 * in the kernel to be able to access it later.
			 * 
			 */

			return IntPtr.Zero;
		}

		private const string MultibootHeaderSymbolName = @"<$>mosa-multiboot-_header";

		/// <summary>
		/// Writes the multiboot _header.
		/// </summary>
		/// <param name="entryPoint">The virtualAddress of the multiboot compliant entry point.</param>
		private void WriteMultibootHeader(IntPtr entryPoint)
		{
			// HACK: According to the multiboot specification this _header must be within the first 8K of the
			// kernel binary. Since the text section is always first, this should take care of the problem.
			using (Stream stream = this.linker.Allocate(MultibootHeaderSymbolName, SectionKind.Text, 64, 4))
			using (BinaryWriter bw = new BinaryWriter(stream, Encoding.ASCII)) {
				// flags - multiboot flags
				uint flags = /*HEADER_MB_FLAG_VIDEO_MODES_REQUIRED | */HEADER_MB_FLAG_MEMORY_INFO_REQUIRED | HEADER_MB_FLAG_MODULES_PAGE_ALIGNED;
				// The multiboot _header checksum 
				uint csum = 0;
				// header_addr is the load virtualAddress of the multiboot _header
				uint header_addr = 0;
				// load_addr is the base virtualAddress of the binary in memory
				uint load_addr = 0;
				// load_end_addr holds the virtualAddress past the last byte to load From the image
				uint load_end_addr = 0;
				// bss_end_addr is the virtualAddress of the last byte to be zeroed out
				uint bss_end_addr = 0;
				// entry_point the load virtualAddress of the entry point to invoke
				uint entry_point = (uint)entryPoint.ToInt32();

				// Are we linking an ELF binary?
				if (!(this.linker is Elf32Linker || this.linker is Elf64Linker)) {
					// Check the linker layout settings
					if (this.linker.LoadSectionAlignment != this.linker.VirtualSectionAlignment)
						throw new LinkerException(@"Load and virtual section alignment must be identical if you are booting non-ELF binaries with a multiboot bootloader.");

					// No, special multiboot treatment required
					flags |= HEADER_MB_FLAG_NON_ELF_BINARY;

					header_addr = (uint)(this.linker.GetSection(SectionKind.Text).VirtualAddress.ToInt64() + this.linker.GetSymbol(MultibootHeaderSymbolName).SectionAddress);
					load_addr = (uint)this.linker.BaseAddress;
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

				// HACK: Symbol has been hacked. What's the correct way to do this?
				this.linker.Link(LinkType.AbsoluteAddress | LinkType.I4, MultibootHeaderSymbolName, (int)stream.Position, 0, @"Mosa.Tools.Compiler.LinkerGenerated.<$>MultibootInit()", IntPtr.Zero);

				bw.Write(videoMode);
				bw.Write(videoWidth);
				bw.Write(videoHeight);
				bw.Write(videoDepth);
			}
		}

		#endregion // Internals

		#region IHasOptions Members
		/// <summary>
		/// Adds the additional options for the parsing process to the given OptionSet.
		/// </summary>
		/// <param name="optionSet">A given OptionSet to add the options to.</param>
		public void AddOptions(OptionSet optionSet)
		{
			optionSet.Add(
				"multiboot-video-mode=",
				"Specify the video mode for multiboot [{text|graphics}].",
				delegate(string v)
				{
					switch (v.ToLower()) {
						case "text":
							videoMode = 1;
							break;
						case "graphics":
							videoMode = 0;
							break;
						default:
							throw new OptionException("Invalid value for multiboot video mode: " + v, "multiboot-video-mode");
					}
				});

			optionSet.Add(
				"multiboot-video-width=",
				"Specify the {width} for video output, in pixels for graphics mode or in characters for text mode.",
				delegate(string v)
				{
					uint val;
					if (uint.TryParse(v, out val)) {
						// TODO: this probably needs further validation
						videoWidth = val;
					}
					else {
						throw new OptionException("Invalid value for multiboot video width: " + v, "multiboot-video-width");
					}
				});

			optionSet.Add(
				"multiboot-video-height=",
				"Specify the {height} for video output, in pixels for graphics mode or in characters for text mode.",
				delegate(string v)
				{
					uint val;
					if (uint.TryParse(v, out val)) {
						// TODO: this probably needs further validation
						videoHeight = val;
					}
					else {
						throw new OptionException("Invalid value for multiboot video height: " + v, "multiboot-video-height");
					}
				});

			optionSet.Add(
				"multiboot-video-depth=",
				"Specify the {depth} (number of bits per pixel) for graphics mode.",
				delegate(string v)
				{
					uint val;
					if (uint.TryParse(v, out val)) {
						// TODO: this probably needs further validation
						videoDepth = val;
					}
					else {
						throw new OptionException("Invalid value for multiboot video depth: " + v, "multiboot-video-depth");
					}
				});

			optionSet.Add(
				"multiboot-module=",
				"Adds a {0:module} to multiboot, to be loaded at a given {1:virtualAddress} (can be used multiple times).",
				delegate(string file, string address)
				{
					// TODO: validate and add this to a list or something
					Console.WriteLine("Adding multiboot module " + file + " at virtualAddress " + address);
				});
		}
		#endregion
	}
}
