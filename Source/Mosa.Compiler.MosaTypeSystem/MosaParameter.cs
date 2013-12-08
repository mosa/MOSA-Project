namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaParameter
	{
		public string Name { get; internal set; }

		public MosaType Type { get; internal set; }

		public int Position { get; internal set; }

		public bool IsIn { get; internal set; }

		public bool IsOut { get; internal set; }

		public override string ToString()
		{
			return Type + " " + Name;
		}

		public bool Matches(MosaParameter parameter)
		{
			return Type.Matches(parameter.Type);
		}

		public bool Matches(MosaType type)
		{
			return Type.Matches(type);
		}
	}
}