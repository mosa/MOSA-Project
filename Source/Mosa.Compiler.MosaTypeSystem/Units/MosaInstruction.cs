// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaInstruction
	{
		public int Offset { get; set; }

		public ushort OpCode { get; set; }

		public object Operand { get; set; }

		public int? Previous { get; set; }

		public int? Next { get; set; }

		/// <summary>
		/// Document Name
		/// </summary>
		public string Document { get; set; }

		/// <summary>
		/// Start line
		/// </summary>
		public int StartLine { get; set; }

		/// <summary>
		/// Start column
		/// </summary>
		public int StartColumn { get; set; }

		/// <summary>
		/// End line
		/// </summary>
		public int EndLine { get; set; }

		/// <summary>
		/// End column
		/// </summary>
		public int EndColumn { get; set; }
	}
}
