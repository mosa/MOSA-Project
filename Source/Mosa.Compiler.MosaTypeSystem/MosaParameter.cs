namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaParameter
	{
		public string Name { get; internal set; }

		public int Position { get; internal set; }

		public bool IsIn { get; internal set; }

		public bool IsOut { get; internal set; }

		public MosaType MosaType { get; internal set; }
	}
}