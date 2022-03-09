namespace System.Net
{
	public class IPAddress
	{
		public byte[] Address;

		public static IPAddress Parse(byte one, byte two, byte three, byte four)
		{
			return new IPAddress()
			{
				Address = new byte[]
				{
					one,
					two,
					three,
					four
				}
			};
		}
	}
}
