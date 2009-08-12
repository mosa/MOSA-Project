using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Mosa.Tools.Compiler.Symbols.Pdb
{
    class CvLineEnumerator : IEnumerable<CvLine>
    {
        #region Data Members

        /// <summary>
        /// Holds the pdb reader.
        /// </summary>
        private PdbReader reader;

        /// <summary>
        /// Holds the pdb stream, that contains the symbol information.
        /// </summary>
        private int stream;

        /// <summary>
        /// The offset to the line number table in the stream.
        /// </summary>
        private int offset;

        /// <summary>
        /// The address of the function to retrieve line numbers for.
        /// </summary>
        private int functionAddress = 0;

        #endregion // Data Members

        #region Construction

        public CvLineEnumerator(PdbReader reader, int stream, int offset)
        {
            this.reader = reader;
            this.stream = stream;
            this.offset = offset;
        }

        #endregion // Construction

        #region IEnumerable<object> Members

        public IEnumerator<CvLine> GetEnumerator()
        {
            // These are fields from a line number table structure
            // _header: The _header of the line number table - always 0x000000F2
            // nextBlockOffset: The number of bytes to skip to get to the next block (skip after reading nextBlockOffset!)
            // start: The function start address whose line number information is provided.
            // seg: The segment of the function
            // size: The number of entries in the 
            // fileOffset: Offset to access corresponding file information
            // numberOfLines: The number of lines 
            // sizeOfLines: The size of the lines in bytes
            int header, nextBlockOffset, start, seg, size, fileOffset, numberOfLines, sizeOfLines;

            using (BinaryReader reader = new BinaryReader(this.reader.GetStream(this.stream)))
            {
                reader.BaseStream.Position = offset;

                do
                {
                    header = reader.ReadInt32();
                    //Debug.Assert(_header == 0x000000F2, @"CvLineEnumerator: Header magic invalid for PDB v7.00");
                    if (header != 0x000000F2)
                        // Skip this, assume no line numbers
                        yield break;

                    nextBlockOffset = reader.ReadInt32();
                    start = reader.ReadInt32();
                    seg = reader.ReadInt32() & 0xFFFF;
                    
                    // Is this the function we're looking for?
                    if (true || start == this.functionAddress)
                    {
                        size = reader.ReadInt32();
                        fileOffset = reader.ReadInt32();
                        numberOfLines = reader.ReadInt32();
                        sizeOfLines = reader.ReadInt32();
                        Debug.WriteLine(String.Format("Line numbers table _header: size={0}, fileOffset={1}, numberOfLines={2}, sizeOfLines={3}, address={4:x4}:{5:x8}", size, fileOffset, numberOfLines, sizeOfLines, seg, start));

                        int[] startCol = new int[numberOfLines], endCol = new int[numberOfLines];
                        long pos = reader.BaseStream.Position;

                        // Skip ahead by numberOfLines*8 bytes
                        reader.BaseStream.Position = pos + (8*numberOfLines);

                        // Start reading in the columns
                        for (int i = 0; i < numberOfLines; i++)
                        {
                            startCol[i] = reader.ReadInt16();
                            endCol[i] = reader.ReadInt16();
                        }

                        // Enumerate the lines
                        reader.BaseStream.Position = pos;
                        for (int i = 0; i < numberOfLines; i++)
                        {
                            int instructionOffset = reader.ReadInt32();
                            int line = reader.ReadInt32() & 0x7FFFFFFF;

                            yield return new CvLine(seg, start + instructionOffset, line, startCol[i], endCol[i]);
                        }

                        // Skip over the lines
                        reader.BaseStream.Position += (numberOfLines * 4);
                    }
                    else
                    {
                        // Skip to the next block
                        reader.BaseStream.Position += (nextBlockOffset - 4);
                    }
                }
                while (nextBlockOffset != 0);
            }
        }

        #endregion // IEnumerable<object> Members

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion // IEnumerable Members
    }
}
