using Mosa.Test.Collection.MbUnit;

namespace Mosa.Test.Debug
{
	internal class Program
	{
		public static void Main()
		{
			var b = new BoxingFixture();
			b.BoxU8(100);

			return;
		}
	}
}