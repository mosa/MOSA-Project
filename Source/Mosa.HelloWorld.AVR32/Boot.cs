/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */


namespace Mosa.HelloWorld.AVR32
{
	/// <summary>
	/// 
	/// </summary>
	public static class Boot
	{

		/// <summary>
		/// Mains this instance.
		/// </summary>
		public static void Main()
		{
			Kernel.AVR32.Kernel.Setup();

			while (true)
			{
				
			}
		}

	}
}
