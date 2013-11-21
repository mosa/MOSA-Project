using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaMethod
	{
		public string Name { get; internal set; }

		public string FullName { get; internal set; }

		public string MethodName { get; internal set; }

		public bool IsAbstract { get; internal set; }

		public bool IsGeneric { get; internal set; }

		public bool IsStatic { get; internal set; }

		public bool HasThis { get; internal set; }

		public bool HasExplicitThis { get; internal set; }

		public MosaType ReturnType { get; internal set; }

		public IList<MosaParameter> Parameters { get; internal set; }
	}
}