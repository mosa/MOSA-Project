namespace Mosa.Demo.SVGAWorld.x86
{
	public static class Center
	{
		// TODO: Fix for e.g. buttons that aren't at the absolute top or bottom of the screen
		public static int GetCenterBetweenYAndHeight(int y, int height, int val)
		{
			return (y + (Display.Height - height) + height) / 2 - val;
		}

		// TODO: Fix for e.g. buttons that aren't at the absolute top or bottom of the screen
		public static int GetCenterBetweenXAndWidth(int x, int width, int val)
		{
			return (x + (Display.Width - width) + width) / 2 - val;
		}
	}
}
