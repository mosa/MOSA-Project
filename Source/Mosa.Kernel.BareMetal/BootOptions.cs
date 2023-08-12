// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal
{
	public class BootOptions
	{
		private static Pointer Options;

		public static void Setup()
		{
			Options = Intrinsic.GetBootOptions();

			var debug = GetValue("Debug.Option");

			if (debug != null)
			{
				Debug.WriteLine("Debug.Option: ", debug);
			}
			else
			{
				Debug.WriteLine("Debug.Option: None");
			}
		}

		public static string GetValue(string key)
		{
			if (Options.IsNull)
				return null;

			var keylen = key.Length;

			var parsekey = true;
			var parsematch = true;
			var parsevalue = false;
			var start = 0;
			var len = 0;

			for (var at = 0; ; at++)
			{
				var c = Options.Load8(at);

				if (c == 0)
					break;

				if (c == ',')
				{
					if (parsematch)
						break;

					parsekey = true;
					parsevalue = false;
					parsematch = true;
					start = 0;
					len = 0;
				}
				else if (c == '=')
				{
					parsematch = parsematch && (len == keylen);
					parsekey = false;
					parsevalue = true;
					start = at + 1;
					len = 0;
				}
				else if (parsekey)
				{
					if (len >= keylen || c != (byte)key[len])
						parsematch = false;

					len++;
				}
				else if (parsevalue)
				{
					len++;
				}
			}

			if (!parsematch)
				return null;

			if (len == 0)
				return string.Empty;

			unsafe
			{
				return new string((sbyte*)Options, start, len);
			}
		}
	}
}
