using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaField
	{
		public MosaType FieldType { get; internal set; }

		public MosaType DeclaringType { get; internal set; }

		public string Name { get; internal set; }

		public string FullName { get; internal set; }

		public IList<MosaAttribute> CustomAttributes { get; internal set; }

		public bool IsLiteralField { get; internal set; }

		public bool IsStaticField { get; internal set; }

		public int RVA { get; internal set; }

		public MosaField()
		{
			CustomAttributes = new List<MosaAttribute>();
		}

		public override string ToString()
		{
			return FullName;
		}
	}
}