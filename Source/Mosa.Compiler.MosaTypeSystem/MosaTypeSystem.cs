using System.Collections.Generic;

namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaTypeSystem
	{
		private List<MosaType> Types = new List<MosaType>();

		public void AddType(MosaType type)
		{
			Types.Add(type);
		}
	}
}