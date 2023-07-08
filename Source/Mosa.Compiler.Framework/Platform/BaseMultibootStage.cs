// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;
using System.Text;
using Mosa.Compiler.Framework.Linker;
using Mosa.Compiler.MosaTypeSystem;

namespace Mosa.Compiler.Framework.Platform;

/// <summary>
/// Writes a multiboot v1 or v2 header into the generated binary.
/// </summary>
/// <remarks>
/// This compiler stage writes a multiboot header into the
/// the data section of the binary file and also creates a multiboot
/// compliant entry point into the binary.
/// The header and entry point written by this stage is compliant with
/// the specification at
/// https://www.gnu.org/software/grub/manual/multiboot/multiboot.html (Multiboot v1) or
/// https://www.gnu.org/software/grub/manual/multiboot2/multiboot.html (Multiboot v2).
/// </remarks>
public abstract class BaseMultibootStage : BaseCompilerStage
{
	private const string MultibootHeaderSymbolName = "<$>mosa-multiboot-header";

	public const string MultibootEAX = "<$>mosa-multiboot-eax";
	public const string MultibootEBX = "<$>mosa-multiboot-ebx";

	#region Constants

	/// <summary>
	/// This address is the top of the initial kernel stack.
	/// </summary>
	private const uint StackAddress = 0x00A00000 - 8;

	private struct MultibootV1Constants
	{
		/// <summary>
		/// Magic value in the multiboot header.
		/// </summary>
		internal const uint HeaderMbMagic = 0x1BADB002;

		/// <summary>
		/// Multiboot flag, which indicates that additional modules must be
		/// loaded on page boundaries.
		/// </summary>
		internal const uint HeaderMbFlagModulesPageAligned = 0x00000001;

		/// <summary>
		/// Multiboot flag, which indicates if memory information must be
		/// provided in the boot information structure.
		/// </summary>
		internal const uint HeaderMbFlagMemoryInfoRequired = 0x00000002;

		/// <summary>
		/// Multiboot flag, which indicates that the supported video mode
		/// table must be provided in the boot information structure.
		/// </summary>
		internal const uint HeaderMbFlagVideoModesRequired = 0x00000004;
	}

	private struct MultibootV2Constants
	{
		/// <summary>
		/// Magic value in the multiboot header.
		/// </summary>
		internal const uint HeaderMbMagic = 0xE85250D6;

		/// <summary>
		/// Architecture in the multiboot header.
		/// </summary>
		internal const uint HeaderMbArchitecture = 0x00000000;

		/// <summary>
		/// Header size.
		/// </summary>
		internal const uint HeaderSize = 0x00000016;

		/// <summary>
		/// Flag indicating the tag is required.
		/// </summary>
		internal const ushort RequiredFlag = 0;

		/// <summary>
		/// Entry tag.
		/// </summary>
		internal const ushort EntryTag = 0x0003;

		/// <summary>
		/// Entry tag size.
		/// </summary>
		internal const uint EntryTagSize = 0x00000012;

		/// <summary>
		/// Framebuffer tag.
		/// </summary>
		internal const ushort FramebufferTag = 0x0005;

		/// <summary>
		/// Framebuffer tag size.
		/// </summary>
		internal const uint FramebufferTagSize = 0x00000020;
	}

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

	public bool IsV2 { get; set; }

	public bool HasVideo { get; set; }

	public int Width { get; set; }

	public int Height { get; set; }

	public int Depth { get; set; }

	public long InitialStackAddress { get; set; }

	#endregion Data Members

	protected override void Initialization()
	{
		IsV2 = MosaSettings.MultibootVersion == "v2";
		HasVideo = MosaSettings.MultibootVideo;
		Width = MosaSettings.MultibootVideoWidth;
		Height = MosaSettings.MultibootVideoHeight;
		Depth = MosaSettings.MultibootVideoDepth;

		InitialStackAddress = MosaSettings.Settings.GetValue("Multiboot.InitialStackAddress", StackAddress);
	}

	protected override void Setup()
	{
		multibootHeader = Linker.DefineSymbol(MultibootHeaderSymbolName, SectionKind.Text, 1, 0x30);

		Linker.DefineSymbol(MultibootEAX, SectionKind.BSS, Architecture.NativeAlignment, Architecture.NativePointerSize);
		Linker.DefineSymbol(MultibootEBX, SectionKind.BSS, Architecture.NativeAlignment, Architecture.NativePointerSize);

		multibootMethod = Compiler.CreateLinkerMethod("MultibootInit");
		var methodData = Compiler.GetMethodData(multibootMethod);

		methodData.DoNotInline = true;
		methodData.StackFrameRequired = false;

		Linker.EntryPoint = Linker.GetSymbol(multibootMethod.FullName);

		MethodScanner.MethodInvoked(multibootMethod, multibootMethod);

		var startUpType = TypeSystem.GetTypeByName("Mosa.Runtime.StartUp");
		var initializeMethod = startUpType?.FindMethodByName("Initialize");

		Compiler.GetMethodData(initializeMethod).DoNotInline = true;

		MethodScanner.MethodInvoked(initializeMethod, multibootMethod);
	}

	#region Internals

	/// <summary>
	/// Writes the multiboot header.
	/// </summary>
	/// <param name="entryPoint">The virtualAddress of the multiboot compliant entry point.</param>
	protected void WriteMultibootHeader(LinkerSymbol entryPoint)
	{
		// According to the multiboot specification this header must be within the first 8K of the kernel binary.
		Linker.SetFirst(multibootHeader);

		var writer = new BinaryWriter(multibootHeader.Stream, Encoding.ASCII);

		if (IsV2)
		{
			// Header size + entry tag size + (framebuffer size if chosen) + end tag size
			var headerLength = MultibootV2Constants.HeaderSize + MultibootV2Constants.EntryTagSize + sizeof(uint);

			if (HasVideo)
			{
				headerLength += MultibootV2Constants.FramebufferTagSize;
			}

			// Header
			while (writer.BaseStream.Position % 8 != 0) writer.Write((byte)0);
			writer.Write(MultibootV2Constants.HeaderMbMagic);
			writer.Write(MultibootV2Constants.HeaderMbArchitecture);
			writer.Write(headerLength);
			writer.Write((uint)(0x100000000 - (MultibootV2Constants.HeaderMbMagic + MultibootV2Constants.HeaderMbArchitecture + headerLength)));

			// Framebuffer tag
			if (HasVideo)
			{
				while (writer.BaseStream.Position % 8 != 0) writer.Write((byte)0);
				writer.Write(MultibootV2Constants.FramebufferTag);
				writer.Write(MultibootV2Constants.RequiredFlag);
				writer.Write(MultibootV2Constants.FramebufferTagSize);
				writer.Write((uint)Width);
				writer.Write((uint)Height);
				writer.Write((uint)Depth);
			}

			// Entry tag
			while (writer.BaseStream.Position % 8 != 0) writer.Write((byte)0);
			writer.Write(MultibootV2Constants.EntryTag);
			writer.Write(MultibootV2Constants.RequiredFlag);
			writer.Write(MultibootV2Constants.EntryTagSize);
			Linker.Link(LinkType.AbsoluteAddress, PatchType.I32, multibootHeader, writer.BaseStream.Position, entryPoint, 0);

			// End tag
			while (writer.BaseStream.Position % 8 != 0) writer.Write((byte)0);
			writer.Write((ushort)0);
			writer.Write((ushort)0);
		}
		else
		{
			// flags - multiboot flags
			var flags =
				MultibootV1Constants.HeaderMbFlagMemoryInfoRequired
				| MultibootV1Constants.HeaderMbFlagModulesPageAligned
				| (HasVideo ? MultibootV1Constants.HeaderMbFlagVideoModesRequired : 0);

			// magic
			writer.Write(MultibootV1Constants.HeaderMbMagic);

			// flags
			writer.Write(flags);

			// checksum
			writer.Write(unchecked(0U - MultibootV1Constants.HeaderMbMagic - flags));

			// header_addr - load address of the multiboot header
			Linker.Link(LinkType.AbsoluteAddress, PatchType.I32, multibootHeader, (int)writer.BaseStream.Position, multibootHeader, 0);
			writer.Write(0);

			// load_addr - address of the binary in memory
			writer.Write(0);

			// load_end_addr - address past the last byte to load from the image
			writer.Write(0);

			// bss_end_addr - address of the last byte to be zeroed out
			writer.Write(0);

			// entry_addr - address of the entry point to invoke
			Linker.Link(LinkType.AbsoluteAddress, PatchType.I32, multibootHeader, (int)writer.BaseStream.Position, entryPoint, 0);
			writer.Write(0);

			// Write video settings if video has been specified, otherwise pad
			writer.Write(0);
			writer.Write(HasVideo ? Width : 0);
			writer.Write(HasVideo ? Height : 0);
			writer.Write(HasVideo ? Depth : 0);
		}
	}

	#endregion Internals
}
