// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Pdb
{
	/// <summary>
	/// Represents a line number mapping for one or more instructions.
	/// </summary>
	/// <remarks>
	/// The segment:offset pair maps to the very first instruction in
	/// a set of instructions, which map to a line. This line covers the
	/// addressed instruction and all following instructions until the
	/// next line mapping or the end of the line number table is reached,
	/// in which case this line mapping covers the remaining instructions
	/// in the function.
	/// </remarks>
	public struct CvLine
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CvLine"/> struct.
		/// </summary>
		/// <param name="segment">The segment.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="line">The line.</param>
		/// <param name="startCol">The start column on the line.</param>
		/// <param name="endCol">The end column on the line.</param>
		public CvLine(int segment, int offset, int line, int startCol, int endCol, int start)
		{
			Segment = segment;
			Offset = offset;
			Line = line;
			StartColumn = startCol;
			EndColumn = endCol;

			Start = start;
		}

		#endregion Construction

		#region Properties

		/// <summary>
		/// Gets the segment of the function.
		/// </summary>
		/// <value>The segment.</value>
		public int Segment { get; }

		/// <summary>
		/// Gets the offset from the function start.
		/// </summary>
		/// <value>The offset.</value>
		public int Offset { get; }

		/// <summary>
		/// Gets the line number associated with this and the following instructions.
		/// </summary>
		/// <value>The line number.</value>
		public int Line { get; }

		/// <summary>
		/// Gets the start column on the line.
		/// </summary>
		/// <value>The start column.</value>
		public int StartColumn { get; }

		/// <summary>
		/// Gets the end column on the line.
		/// </summary>
		/// <value>The end column.</value>
		public int EndColumn { get; }

		public int Start { get; }

		#endregion Properties

		#region Object Overrides

		/// <summary>
		/// Returns the fully qualified type name of this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> containing a fully qualified type name.
		/// </returns>
		public override string ToString()
		{
			return String.Format("Line {0} columns {1}-{2} at {3:x4}:{4:x8}+{5:x8} [{6:x6}]", Line, StartColumn, EndColumn, Segment, Start, Offset, Start + Offset);
		}

		#endregion Object Overrides
	}
}
