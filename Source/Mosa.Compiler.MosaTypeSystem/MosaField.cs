namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaField
	{
		public MosaType MosaType { get; internal set; }

		public string Name { get; internal set; }

		public string FullName { get; internal set; }

		public bool IsLiteralField { get; internal set; }

		public bool IsStaticField { get; internal set; }
	}
}