// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Debugger.DebugData
{
	public class TypeInfo
	{
		public int ID { get; set; }
		public ulong DefAddress { get; set; }
		public uint Size { get; set; }
		public string FullName { get; set; }
		public int BaseTypeID { get; set; }
		public int DeclaringTypeID { get; set; }
		public int ElementTypeID { get; set; }

		public override string ToString()
		{
			return FullName;
		}
	}
}
