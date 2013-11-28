namespace Mosa.Tool.TinySimulator
{
	public class Watch
	{
		public ulong Address;

		public int Size;

		public Watch(ulong address, int size)
		{
			Address = address;
			Size = size;
		}
	}
}