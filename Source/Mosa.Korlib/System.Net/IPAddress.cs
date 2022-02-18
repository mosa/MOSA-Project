namespace System.Net
{
	public class IPAddress
	{
		public byte[] Address;

		public static IPAddress Parse(params byte[] ip)
		{
			return new IPAddress()
			{
				Address = new byte[]
				{
					ip[0],
					ip[1],
					ip[2],
					ip[3]
				}
			};
		}
	}
}
