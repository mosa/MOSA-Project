// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Utility.RSP;

internal static class Rle
{
	public static List<byte> Decode(List<byte> data, int start = 0, int length = 0)
	{
		if (length == 0)
			length = data.Count - start;

		var decoded = new List<byte>(length);

		for (var i = start; i < length; i++)
		{
			var c = data[i];

			if (c != '*')
			{
				decoded.Add(c);
				continue;
			}

			if (i == 0)
				return null; // error --- repeat character can not at the start

			var repeated = data[i - 1];
			var len = data[i + 1] - 28;

			if (!(len >= 4 && len <= 97 && len != 6 && len != 7))
				return null; // error --- invalid length

			for (var j = 0; j < len - 1; j++)
			{
				decoded.Add(repeated);
			}

			i++;
		}

		return decoded;
	}
}
