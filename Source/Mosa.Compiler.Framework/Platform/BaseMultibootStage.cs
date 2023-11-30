// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
	#region Constants

	private const string MultibootHeaderSymbolName = "<$>mosa-multiboot-header";

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
	protected MosaMethod MultibootMethod;

	/// <summary>
	/// The multiboot header
	/// </summary>
	protected LinkerSymbol MultibootHeader;

	#endregion Data Members

	protected override void Setup()
	{
		MultibootHeader = Linker.DefineSymbol(MultibootHeaderSymbolName, SectionKind.Text, 1, 0x30);
		MultibootMethod = Compiler.CreateLinkerMethod("MultibootInit");

		var methodData = Compiler.GetMethodData(MultibootMethod);

		methodData.DoNotInline = true;
		methodData.StackFrameRequired = false;

		Linker.EntryPoint = Linker.GetSymbol(MultibootMethod.FullName);

		MethodScanner.MethodInvoked(MultibootMethod, MultibootMethod);

		var initializeMethod = TypeSystem.GetMethod("Mosa.Runtime.StartUp", "Initialize");

		Compiler.GetMethodData(initializeMethod).DoNotInline = true;

		MethodScanner.MethodInvoked(initializeMethod, MultibootMethod);
	}

	protected override void Finalization()
	{
		CreateMultibootMethod();

		WriteMultibootHeader(Linker.EntryPoint);
	}

	protected virtual void CreateMultibootMethod()
	{
	}

	#region Internals

	/// <summary>
	/// Writes the multiboot header.
	/// </summary>
	/// <param name="entryPoint">The virtualAddress of the multiboot compliant entry point.</param>
	protected void WriteMultibootHeader(LinkerSymbol entryPoint)
	{
		// According to the multiboot specification this header must be within the first 8K of the kernel binary.
		Linker.SetFirst(MultibootHeader);

		var writer = new BinaryWriter(MultibootHeader.Stream, Encoding.ASCII);

		if (MosaSettings.MultibootVersion != "v2")
		{
			throw new CompilerException("Multiboot.Version != v2");
		}

		// Header size + entry tag size + (framebuffer size if chosen) + end tag size
		var headerLength = MultibootV2Constants.HeaderSize + MultibootV2Constants.EntryTagSize + sizeof(uint);

		if (MosaSettings.MultibootVideo)
		{
			headerLength += MultibootV2Constants.FramebufferTagSize;
		}

		// Header
		while (writer.BaseStream.Position % 8 != 0)
		{
			writer.Write((byte)0);
		}

		writer.Write(MultibootV2Constants.HeaderMbMagic);
		writer.Write(MultibootV2Constants.HeaderMbArchitecture);
		writer.Write(headerLength);
		writer.Write((uint)(0x100000000 - (MultibootV2Constants.HeaderMbMagic + MultibootV2Constants.HeaderMbArchitecture + headerLength)));

		// Framebuffer tag
		if (MosaSettings.MultibootVideo)
		{
			while (writer.BaseStream.Position % 8 != 0) writer.Write((byte)0);
			writer.Write(MultibootV2Constants.FramebufferTag);
			writer.Write(MultibootV2Constants.RequiredFlag);
			writer.Write(MultibootV2Constants.FramebufferTagSize);
			writer.Write((uint)MosaSettings.MultibootVideoWidth);
			writer.Write((uint)MosaSettings.MultibootVideoHeight);
			writer.Write(32U);
		}

		// Entry tag
		while (writer.BaseStream.Position % 8 != 0)
		{
			writer.Write((byte)0);
		}

		writer.Write(MultibootV2Constants.EntryTag);
		writer.Write(MultibootV2Constants.RequiredFlag);
		writer.Write(MultibootV2Constants.EntryTagSize);
		Linker.Link(LinkType.AbsoluteAddress, PatchType.I32, MultibootHeader, writer.BaseStream.Position, entryPoint, 0);

		// End tag
		while (writer.BaseStream.Position % 8 != 0)
		{
			writer.Write((byte)0);
		}

		writer.Write((ushort)0);
		writer.Write((ushort)0);
	}

	#endregion Internals
}
