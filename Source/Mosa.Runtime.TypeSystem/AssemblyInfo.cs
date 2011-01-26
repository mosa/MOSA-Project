using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// TODO: Find a better namespace for this and correct this type

namespace Mosa.Runtime.TypeSystem
{
	public class AssemblyInfo
	{
		public int HashAlgId;
		public int MajorVersion;
		public int MinorVersion;
		public int BuildNumber;
		public int RevisionNumber;
		public int Flags;
		public string Name;
		public string Locale;
		public byte[] PublicKey;
	}
}
