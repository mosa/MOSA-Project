namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaGenericParameter
	{
		public string Name { get; internal set; }

		public MosaGenericParameter(string name)
		{
			this.Name = name;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}