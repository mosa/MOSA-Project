namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaAssembly
	{
		public string Name { get; internal set; }

		public MosaAssembly()
		{
		}

		public MosaAssembly(string name)
		{
			this.Name = name;
		}

		public override string ToString()
		{
			return Name;
		}
	}
}