// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Demo.AppSystem;

namespace Mosa.Demo.Application;

/// <summary>
/// Credits
/// </summary>
public class Test : BaseApplication, IConsoleApp
{
	public override int Start(string parameters)
	{
		if (string.IsNullOrEmpty(parameters))
		{
			Console.WriteLine("missing paramter");
			return 1;
		}

		Console.WriteLine(parameters);

		return 0;
	}
}
