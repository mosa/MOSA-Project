namespace Mosa.Compiler.MosaTypeSystem
{
	public class MosaAttribute
	{
		public MosaMethod CtorMethod { get; internal set; }

		public byte[] Blob { get; internal set; }

		public override string ToString()
		{
			return "Ctor: " + CtorMethod.ToString();
		}
	}
}