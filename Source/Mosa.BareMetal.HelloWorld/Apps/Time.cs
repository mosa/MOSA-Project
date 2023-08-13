// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Kernel.BareMetal;

namespace Mosa.BareMetal.HelloWorld.Apps;

public class Time : IApp
{
	public string Name => "Time";

	public string Description => "Shows the date and time.";

	public void Execute()
	{
		var time = Platform.GetTime();

		Console.WriteLine("DD/MM/YYYY HH:MM:SS");
		Console.WriteLine(
			(time.Day < 10 ? "0" + time.Day : time.Day)
			+ "/" +
			(time.Month < 10 ? "0" + time.Month : time.Month)
			+ "/" +
			time.Year // Too annoying to get all combinations right :D
			+ " " +
			(time.Hour < 10 ? "0" + time.Hour : time.Hour)
			+ ":" +
			(time.Minute < 10 ? "0" + time.Minute : time.Minute)
			+ ":" +
			(time.Second < 10 ? "0" + time.Second : time.Second)
		);
	}
}
