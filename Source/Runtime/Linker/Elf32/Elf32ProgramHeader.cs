namespace Mosa.Runtime.Linker.Elf32
{
	/// <summary>
	/// 
	/// </summary>
	public class Elf32ProgramHeader
	{
		/// <summary>
		/// This member tells what kind of segment this array element describes or how to 
		/// interpret the array element's information.
		/// </summary>
		public Elf32ProgramHeaderType Type;
		/// <summary>
		/// This member gives the offset from the beginning of the file at which the first byte 
		/// of the segment resides.
		/// </summary>
		public uint Offset;
		/// <summary>
		/// This member gives the virtual virtualAddress at which the first byte of the segment resides 
		/// in memory. 
		/// </summary>
		public uint VirtualAddress;
		/// <summary>
		/// On systems for which physical addressing is relevant, this member is reserved for 
		/// the segment's physical virtualAddress. 
		/// </summary>
		public uint PhysicalAddress;
		/// <summary>
		/// This member gives the number of bytes in the file image of the segment; it may be 
		/// zero.
		/// </summary>
		public uint FileSize;
		/// <summary>
		/// This member gives the number of bytes in the memory image of the segment; it 
		/// may be zero.
		/// </summary>
		public uint MemorySize;
		/// <summary>
		/// This member gives flags relevant to the segment.
		/// </summary>
		public Elf32ProgramHeaderFlags Flags;
		/// <summary>
		/// 
		/// </summary>
		public uint Alignment;

		/// <summary>
		/// Writes the specified writer.
		/// </summary>
		/// <param name="writer">The writer.</param>
		public virtual void Write(System.IO.BinaryWriter writer)
		{
			writer.Write((uint)Type);
			writer.Write(Offset);
			writer.Write(VirtualAddress);
			writer.Write(PhysicalAddress);
			writer.Write(FileSize);
			writer.Write(MemorySize);
			writer.Write((uint)Flags);
			writer.Write(Alignment);
		}
	}
}
