// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;
using System.Text;
using Mosa.Compiler.Common.Exceptions;
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
	public const string MultibootInitialStack = "<$>mosa-multiboot-initial-stack";

	#region Constants

	/// <summary>
	/// This is the size of the initial kernel stack. (8KiB)
	/// </summary>
	protected const uint StackSize = 0x2000;

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
		internal const uint FramebufferTagSize = 0x00000014;
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

	#endregion Data Members

	protected override void Initialization()
	{
		IsV2 = MosaSettings.MultibootVersion == "v2";
		HasVideo = MosaSettings.MultibootVideo;
		Width = MosaSettings.MultibootVideoWidth;
		Height = MosaSettings.MultibootVideoHeight;
	}

	protected override void Setup()
	{
		multibootHeader = Linker.DefineSymbol(MultibootHeaderSymbolName, SectionKind.Text, 1, 0x30);

		Linker.DefineSymbol(MultibootEAX, SectionKind.BSS, Architecture.NativeAlignment, Architecture.NativePointerSize);
		Linker.DefineSymbol(MultibootEBX, SectionKind.BSS, Architecture.NativeAlignment, Architecture.NativePointerSize);
		Linker.DefineSymbol(MultibootInitialStack, SectionKind.BSS, Architecture.NativeAlignment, StackSize);

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

		if (!IsV2) throw new CompilerException("Multiboot.Version != v2");

		// Header size + entry tag size + (framebuffer size if chosen) + end tag size
		var headerLength = MultibootV2Constants.HeaderSize + MultibootV2Constants.EntryTagSize + sizeof(uint);
		if (HasVideo) headerLength += MultibootV2Constants.FramebufferTagSize;

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
			writer.Write(32U);
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

	#endregion Internals
}
