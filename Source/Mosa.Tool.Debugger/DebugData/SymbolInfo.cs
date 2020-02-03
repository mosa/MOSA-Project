// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Debugger.DebugData
{
	public class SymbolInfo
	{
		public ulong Address { get; set; }
		public uint Offset { get; set; }
		public uint Length { get; set; }
		public string Kind { get; set; }
		public string Name { get; set; }

		public string CommonName
		{
			get { return Name; }

			//get
			//{
			//	int pos = Name.IndexOf(' ');

			//	if (pos <= 0)
			//		return Name;

			//	return Name.Substring(pos + 1);
			//}
		}
	}
}
