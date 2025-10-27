// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

public static class BootOptions
{
	private static Pointer StaticOptions;
	private static Pointer RuntimeOptions;

	public static void Setup()
	{
		StaticOptions = Intrinsic.GetBootOptions();
		RuntimeOptions = Multiboot.CommandLinePointer;

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
		var result = GetValue(StaticOptions, key);

		if (string.IsNullOrEmpty(result))
		{
			result = GetValue(RuntimeOptions, key);
		}

		return result;
	}

	public static bool Contains(string value)
	{
		if (Contains(StaticOptions, value))
			return true;

		return Contains(RuntimeOptions, value);
	}

	private static string GetValue(Pointer options, string key)
	{
		if (options.IsNull)
			return null;

		var keylen = key.Length;

		var parsekey = true;
		var parsematch = true;
		var parsevalue = false;
		var start = 0;
		var len = 0;

		for (var at = 0; ; at++)
		{
			var c = options.Load8(at);

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
			return new string((sbyte*)options, start, len);
		}
	}

	private static bool Contains(Pointer options, string value)
	{
		var valuelen = value.Length;

		var index = 0;

		for (var at = 0; ; at++)
		{
			var c = options.Load8(at);

			if (c == 0)
				break;

			if (index == valuelen)
				return true;

			if (c == value[index])
				index++;
			else
				index = 0;
		}

		return index == valuelen;
	}
}
